using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.Dsp;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        #region --- 1. 전역 상수 및 설정 ---

        // 루프 비트 단위 정의
        private readonly float[] loopBeatOptions = { 0.125f, 0.25f, 0.5f, 1f, 2f, 4f, 8f, 16f, 32f };
        private readonly string[] loopBeatLabels = { "1/8", "1/4", "1/2", "1", "2", "4", "8", "16", "32" };

        #endregion

        #region --- 2. Deck A 필드 ---

        private AudioFileReader? audioFileA;
        private SpeedSampleProvider? speedProviderA;
        private FilterSampleProvider? filterProviderA;
        private DirectSoundOut? waveOutA;
        private bool isDraggingPositionA = false;
        private float baseBpmA = 120.0f;

        // Deck A 루프
        private bool isLoopActiveA = false;
        private int loopBeatIndexA = 5; // 기본값: 4비트 ("4")
        private TimeSpan loopStartA;
        private TimeSpan loopEndA;

        #endregion

        #region --- 3. Deck B 필드 ---

        private AudioFileReader? audioFileB;
        private SpeedSampleProvider? speedProviderB;
        private FilterSampleProvider? filterProviderB;
        private DirectSoundOut? waveOutB;
        private bool isDraggingPositionB = false;
        private float baseBpmB = 120.0f;

        // Deck B 루프
        private bool isLoopActiveB = false;
        private int loopBeatIndexB = 5; // 기본값: 4비트 ("4")
        private TimeSpan loopStartB;
        private TimeSpan loopEndB;

        #endregion

        #region --- 4. 생성자 및 초기화 ---

        public Form1()
        {
            InitializeComponent();

            InitDeckAEvents();
            InitDeckBEvents();

            // 마스터 BPM 이벤트
            if (numMasterBpm != null)
            {
                numMasterBpm.ValueChanged -= numMasterBpm_ValueChanged;
                numMasterBpm.ValueChanged += numMasterBpm_ValueChanged;
            }

            // 실시간 타이머 (루프 감지 및 UI 업데이트)
            if (timerA != null)
            {
                timerA.Interval = 50; // 50ms 설정
                timerA.Tick -= timer_Tick;
                timerA.Tick += timer_Tick;
                timerA.Start();
            }
        }

        #endregion

        #region --- 5. 마스터 BPM 제어 ---

        private void numMasterBpm_ValueChanged(object? sender, EventArgs e)
        {
            if (numMasterBpm == null) return;
            float targetBpm = (float)numMasterBpm.Value;

            ApplyBpmToDeckA(targetBpm);
            ApplyBpmToDeckB(targetBpm);
        }

        private void ApplyBpmToDeckA(float targetBpm)
        {
            if (speedProviderA == null || baseBpmA <= 0) return;
            speedProviderA.PlaybackRate = targetBpm / baseBpmA;
            if (isLoopActiveA) RecalculateLoopEndA();
        }

        private void ApplyBpmToDeckB(float targetBpm)
        {
            if (speedProviderB == null || baseBpmB <= 0) return;
            speedProviderB.PlaybackRate = targetBpm / baseBpmB;
            if (isLoopActiveB) RecalculateLoopEndB();
        }

        #endregion

        #region --- 6. Deck A 로직 ---

        private void InitDeckAEvents()
        {
            // EQ / Filter
            if (trackFilterA != null)
            {
                trackFilterA.Minimum = -100; trackFilterA.Maximum = 100; trackFilterA.Value = 0;
                trackFilterA.Scroll -= trackFilterA_Scroll; trackFilterA.Scroll += trackFilterA_Scroll;
            }
            if (trackBassA != null)
            {
                trackBassA.Minimum = -100; trackBassA.Maximum = 100; trackBassA.Value = 0;
                trackBassA.Scroll -= trackBassA_Scroll; trackBassA.Scroll += trackBassA_Scroll;
            }

            // Auto Loop 버튼
            if (btnLoopToggleA != null)
            {
                btnLoopToggleA.FlatStyle = FlatStyle.Flat;
                btnLoopToggleA.FlatAppearance.BorderSize = 1;
                btnLoopToggleA.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                btnLoopToggleA.Click -= btnLoopToggleA_Click;
                btnLoopToggleA.Click += btnLoopToggleA_Click;
            }
            if (btnLoopMinusA != null) { btnLoopMinusA.Click -= btnLoopMinusA_Click; btnLoopMinusA.Click += btnLoopMinusA_Click; }
            if (btnLoopPlusA != null) { btnLoopPlusA.Click -= btnLoopPlusA_Click; btnLoopPlusA.Click += btnLoopPlusA_Click; }

            // FX 콤보박스 (드롭다운 - Echo, Reverb만 유지)
            if (cmbFxA != null)
            {
                cmbFxA.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbFxA.DrawMode = DrawMode.OwnerDrawFixed;
                cmbFxA.FlatStyle = FlatStyle.Flat;
                cmbFxA.Items.Clear();
                cmbFxA.Items.AddRange(new object[] { "OFF", "1. Echo", "2. Reverb" });
                cmbFxA.SelectedIndex = 0;
                cmbFxA.SelectedIndexChanged -= cmbFxA_SelectedIndexChanged;
                cmbFxA.SelectedIndexChanged += cmbFxA_SelectedIndexChanged;
                cmbFxA.DrawItem -= cmbFx_DrawItem;
                cmbFxA.DrawItem += cmbFx_DrawItem;
            }

            // 재생 컨트롤
            if (btnLoadA != null) { btnLoadA.Click -= btnLoadA_Click; btnLoadA.Click += btnLoadA_Click; }
            if (btnPlayA != null) { btnPlayA.Click -= btnPlayA_Click; btnPlayA.Click += btnPlayA_Click; }
            if (btnPauseA != null) { btnPauseA.Click -= btnPauseA_Click; btnPauseA.Click += btnPauseA_Click; }
            if (btnStopA != null) { btnStopA.Click -= btnStopA_Click; btnStopA.Click += btnStopA_Click; }
            if (trackVolumeA != null) { trackVolumeA.Scroll -= trackVolumeA_Scroll; trackVolumeA.Scroll += trackVolumeA_Scroll; }
            if (trackPositionA != null)
            {
                trackPositionA.MouseDown -= trackPositionA_MouseDown; trackPositionA.MouseDown += trackPositionA_MouseDown;
                trackPositionA.MouseUp -= trackPositionA_MouseUp; trackPositionA.MouseUp += trackPositionA_MouseUp;
            }

            UpdateLoopUI_A();
            UpdateFxUI_A();
        }

        private void LoadTrackA(string filePath)
        {
            try
            {
                waveOutA?.Stop();
                audioFileA?.Dispose();
                waveOutA?.Dispose();

                isLoopActiveA = false;
                if (cmbFxA != null) cmbFxA.SelectedIndex = 0;
                UpdateLoopUI_A();
                UpdateFxUI_A();

                audioFileA = new AudioFileReader(filePath);
                speedProviderA = new SpeedSampleProvider(audioFileA.ToSampleProvider());
                filterProviderA = new FilterSampleProvider(speedProviderA);

                waveOutA = new DirectSoundOut();
                waveOutA.Init(filterProviderA);

                UpdateVolumeA();
                UpdateFilterA();
                UpdateBassA();
                if (numMasterBpm != null) ApplyBpmToDeckA((float)numMasterBpm.Value);

                if (lblSongA != null) lblSongA.Text = Path.GetFileName(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Deck A 로드 오류: {ex.Message}");
            }
        }

        // --- Deck A FX (드롭다운) ---
        private void cmbFxA_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ApplyFxA();
            UpdateFxUI_A();
            if (cmbFxA != null)
            {
                cmbFxA.SelectionLength = 0;
                this.ActiveControl = null;
            }
        }

        private void ApplyFxA()
        {
            if (filterProviderA == null || cmbFxA == null) return;
            int selectedFx = cmbFxA.SelectedIndex; // 0: OFF, 1: Echo, 2: Reverb
            filterProviderA.SetFx(selectedFx, selectedFx > 0 ? 1.0f : 0.0f);
        }

        private void UpdateFxUI_A()
        {
            if (cmbFxA == null) return;
            if (cmbFxA.SelectedIndex > 0)
            {
                cmbFxA.BackColor = Color.Orange;
                cmbFxA.ForeColor = Color.White;
            }
            else
            {
                cmbFxA.BackColor = Color.White;
                cmbFxA.ForeColor = Color.Black;
            }
            cmbFxA.Invalidate();
        }

        // --- Deck A Auto Loop ---
        private void btnLoopToggleA_Click(object? sender, EventArgs e)
        {
            if (audioFileA == null) return;
            isLoopActiveA = !isLoopActiveA;
            if (isLoopActiveA)
            {
                loopStartA = audioFileA.CurrentTime;
                RecalculateLoopEndA();
            }
            UpdateLoopUI_A();
        }

        private void btnLoopMinusA_Click(object? sender, EventArgs e)
        {
            if (loopBeatIndexA > 0)
            {
                loopBeatIndexA--;
                if (isLoopActiveA) RecalculateLoopEndA();
                UpdateLoopUI_A();
            }
        }

        private void btnLoopPlusA_Click(object? sender, EventArgs e)
        {
            if (loopBeatIndexA < loopBeatOptions.Length - 1)
            {
                loopBeatIndexA++;
                if (isLoopActiveA) RecalculateLoopEndA();
                UpdateLoopUI_A();
            }
        }

        private void RecalculateLoopEndA()
        {
            float currentBpm = numMasterBpm != null ? (float)numMasterBpm.Value : baseBpmA;
            if (currentBpm <= 0) currentBpm = 120.0f;

            float beats = loopBeatOptions[loopBeatIndexA];
            double durationSeconds = beats * (60.0 / currentBpm);
            loopEndA = loopStartA + TimeSpan.FromSeconds(durationSeconds);
        }

        private void UpdateLoopUI_A()
        {
            if (btnLoopToggleA == null) return;
            btnLoopToggleA.Text = $"⟳  {loopBeatLabels[loopBeatIndexA]}";

            if (isLoopActiveA)
            {
                btnLoopToggleA.BackColor = Color.Orange;
                btnLoopToggleA.ForeColor = Color.White;
                btnLoopToggleA.FlatAppearance.BorderColor = Color.OrangeRed;
            }
            else
            {
                btnLoopToggleA.BackColor = Color.White;
                btnLoopToggleA.ForeColor = Color.Black;
                btnLoopToggleA.FlatAppearance.BorderColor = Color.LightGray;
            }
        }

        // --- Deck A 기본 제어 ---
        private void btnLoadA_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Deck A 음악 파일 선택";
                dialog.Filter = "음악 파일 (*.mp3;*.wav;*.flac)|*.mp3;*.wav;*.flac|모든 파일 (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK) LoadTrackA(dialog.FileName);
            }
        }

        private void btnPlayA_Click(object? sender, EventArgs e)
        {
            if (audioFileA == null || waveOutA == null) return;
            if (audioFileA.Position >= audioFileA.Length) audioFileA.Position = 0;
            waveOutA.Play();
        }

        private void btnPauseA_Click(object? sender, EventArgs e) => waveOutA?.Pause();

        private void btnStopA_Click(object? sender, EventArgs e)
        {
            if (waveOutA != null && audioFileA != null)
            {
                waveOutA.Stop();
                audioFileA.Position = 0;
                isLoopActiveA = false;
                if (cmbFxA != null) cmbFxA.SelectedIndex = 0;
                UpdateLoopUI_A();
                UpdateFxUI_A();
            }
        }

        private void trackVolumeA_Scroll(object? sender, EventArgs e) => UpdateVolumeA();
        private void UpdateVolumeA() { if (audioFileA != null && trackVolumeA != null) audioFileA.Volume = trackVolumeA.Value / 100.0f; }

        private void trackFilterA_Scroll(object? sender, EventArgs e) => UpdateFilterA();
        private void UpdateFilterA() { if (filterProviderA != null && trackFilterA != null) filterProviderA.FilterValue = trackFilterA.Value / 100.0f; }

        private void trackBassA_Scroll(object? sender, EventArgs e) => UpdateBassA();
        private void UpdateBassA() { if (filterProviderA != null && trackBassA != null) filterProviderA.BassValue = trackBassA.Value / 100.0f; }

        private void trackPositionA_MouseDown(object? sender, MouseEventArgs e) => isDraggingPositionA = true;
        private void trackPositionA_MouseUp(object? sender, MouseEventArgs e)
        {
            if (audioFileA != null && trackPositionA != null && audioFileA.TotalTime.TotalSeconds > 0)
            {
                double targetSeconds = (trackPositionA.Value / 100.0) * audioFileA.TotalTime.TotalSeconds;
                audioFileA.CurrentTime = TimeSpan.FromSeconds(targetSeconds);
                if (isLoopActiveA)
                {
                    loopStartA = audioFileA.CurrentTime;
                    RecalculateLoopEndA();
                }
            }
            isDraggingPositionA = false;
        }

        #endregion

        #region --- 7. Deck B 로직 ---

        private void InitDeckBEvents()
        {
            // EQ / Filter
            if (trackFilterB != null)
            {
                trackFilterB.Minimum = -100; trackFilterB.Maximum = 100; trackFilterB.Value = 0;
                trackFilterB.Scroll -= trackFilterB_Scroll; trackFilterB.Scroll += trackFilterB_Scroll;
            }
            if (trackBassB != null)
            {
                trackBassB.Minimum = -100; trackBassB.Maximum = 100; trackBassB.Value = 0;
                trackBassB.Scroll -= trackBassB_Scroll; trackBassB.Scroll += trackBassB_Scroll;
            }

            // Auto Loop 버튼
            if (btnLoopToggleB != null)
            {
                btnLoopToggleB.FlatStyle = FlatStyle.Flat;
                btnLoopToggleB.FlatAppearance.BorderSize = 1;
                btnLoopToggleB.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
                btnLoopToggleB.Click -= btnLoopToggleB_Click;
                btnLoopToggleB.Click += btnLoopToggleB_Click;
            }
            if (btnLoopMinusB != null) { btnLoopMinusB.Click -= btnLoopMinusB_Click; btnLoopMinusB.Click += btnLoopMinusB_Click; }
            if (btnLoopPlusB != null) { btnLoopPlusB.Click -= btnLoopPlusB_Click; btnLoopPlusB.Click += btnLoopPlusB_Click; }

            // FX 콤보박스 (드롭다운 - Echo, Reverb만 유지)
            if (cmbFxB != null)
            {
                cmbFxB.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbFxB.DrawMode = DrawMode.OwnerDrawFixed;
                cmbFxB.FlatStyle = FlatStyle.Flat;
                cmbFxB.Items.Clear();
                cmbFxB.Items.AddRange(new object[] { "OFF", "1. Echo", "2. Reverb" });
                cmbFxB.SelectedIndex = 0;
                cmbFxB.SelectedIndexChanged -= cmbFxB_SelectedIndexChanged;
                cmbFxB.SelectedIndexChanged += cmbFxB_SelectedIndexChanged;
                cmbFxB.DrawItem -= cmbFx_DrawItem;
                cmbFxB.DrawItem += cmbFx_DrawItem;
            }

            // 재생 컨트롤
            if (btnLoadB != null) { btnLoadB.Click -= btnLoadB_Click; btnLoadB.Click += btnLoadB_Click; }
            if (btnPlayB != null) { btnPlayB.Click -= btnPlayB_Click; btnPlayB.Click += btnPlayB_Click; }
            if (btnPauseB != null) { btnPauseB.Click -= btnPauseB_Click; btnPauseB.Click += btnPauseB_Click; }
            if (btnStopB != null) { btnStopB.Click -= btnStopB_Click; btnStopB.Click += btnStopB_Click; }
            if (trackVolumeB != null) { trackVolumeB.Scroll -= trackVolumeB_Scroll; trackVolumeB.Scroll += trackVolumeB_Scroll; }
            if (trackPositionB != null)
            {
                trackPositionB.MouseDown -= trackPositionB_MouseDown; trackPositionB.MouseDown += trackPositionB_MouseDown;
                trackPositionB.MouseUp -= trackPositionB_MouseUp; trackPositionB.MouseUp += trackPositionB_MouseUp;
            }

            UpdateLoopUI_B();
            UpdateFxUI_B();
        }

        private void LoadTrackB(string filePath)
        {
            try
            {
                waveOutB?.Stop();
                audioFileB?.Dispose();
                waveOutB?.Dispose();

                isLoopActiveB = false;
                if (cmbFxB != null) cmbFxB.SelectedIndex = 0;
                UpdateLoopUI_B();
                UpdateFxUI_B();

                audioFileB = new AudioFileReader(filePath);
                speedProviderB = new SpeedSampleProvider(audioFileB.ToSampleProvider());
                filterProviderB = new FilterSampleProvider(speedProviderB);

                waveOutB = new DirectSoundOut();
                waveOutB.Init(filterProviderB);

                UpdateVolumeB();
                UpdateFilterB();
                UpdateBassB();
                if (numMasterBpm != null) ApplyBpmToDeckB((float)numMasterBpm.Value);

                if (lblSongB != null) lblSongB.Text = Path.GetFileName(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Deck B 로드 오류: {ex.Message}");
            }
        }

        // --- Deck B FX (드롭다운) ---
        private void cmbFxB_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ApplyFxB();
            UpdateFxUI_B();
            if (cmbFxB != null)
            {
                cmbFxB.SelectionLength = 0;
                this.ActiveControl = null;
            }
        }

        private void ApplyFxB()
        {
            if (filterProviderB == null || cmbFxB == null) return;
            int selectedFx = cmbFxB.SelectedIndex; // 0: OFF, 1: Echo, 2: Reverb
            filterProviderB.SetFx(selectedFx, selectedFx > 0 ? 1.0f : 0.0f);
        }

        private void UpdateFxUI_B()
        {
            if (cmbFxB == null) return;
            if (cmbFxB.SelectedIndex > 0)
            {
                cmbFxB.BackColor = Color.Orange;
                cmbFxB.ForeColor = Color.White;
            }
            else
            {
                cmbFxB.BackColor = Color.White;
                cmbFxB.ForeColor = Color.Black;
            }
            cmbFxB.Invalidate();
        }

        // 공용 커스텀 드로잉 (파란색 포커스 제거)
        private void cmbFx_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not ComboBox cmb || e.Index < 0) return;

            e.DrawBackground();

            bool isSelectedState = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            bool isComboActive = cmb.SelectedIndex > 0;

            Color backColor;
            Color textColor;

            if (isSelectedState)
            {
                backColor = Color.FromArgb(255, 200, 100);
                textColor = Color.Black;
            }
            else
            {
                backColor = isComboActive ? Color.Orange : Color.White;
                textColor = isComboActive ? Color.White : Color.Black;
            }

            using (SolidBrush bgBrush = new SolidBrush(backColor))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                e.Graphics.FillRectangle(bgBrush, e.Bounds);
                string text = cmb.Items[e.Index]?.ToString() ?? "";

                StringFormat sf = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near
                };

                e.Graphics.DrawString(text, e.Font ?? cmb.Font, textBrush, e.Bounds, sf);
            }
        }

        // --- Deck B Auto Loop ---
        private void btnLoopToggleB_Click(object? sender, EventArgs e)
        {
            if (audioFileB == null) return;
            isLoopActiveB = !isLoopActiveB;
            if (isLoopActiveB)
            {
                loopStartB = audioFileB.CurrentTime;
                RecalculateLoopEndB();
            }
            UpdateLoopUI_B();
        }

        private void btnLoopMinusB_Click(object? sender, EventArgs e)
        {
            if (loopBeatIndexB > 0)
            {
                loopBeatIndexB--;
                if (isLoopActiveB) RecalculateLoopEndB();
                UpdateLoopUI_B();
            }
        }

        private void btnLoopPlusB_Click(object? sender, EventArgs e)
        {
            if (loopBeatIndexB < loopBeatOptions.Length - 1)
            {
                loopBeatIndexB++;
                if (isLoopActiveB) RecalculateLoopEndB();
                UpdateLoopUI_B();
            }
        }

        private void RecalculateLoopEndB()
        {
            float currentBpm = numMasterBpm != null ? (float)numMasterBpm.Value : baseBpmB;
            if (currentBpm <= 0) currentBpm = 120.0f;

            float beats = loopBeatOptions[loopBeatIndexB];
            double durationSeconds = beats * (60.0 / currentBpm);
            loopEndB = loopStartB + TimeSpan.FromSeconds(durationSeconds);
        }

        private void UpdateLoopUI_B()
        {
            if (btnLoopToggleB == null) return;
            btnLoopToggleB.Text = $"⟳  {loopBeatLabels[loopBeatIndexB]}";

            if (isLoopActiveB)
            {
                btnLoopToggleB.BackColor = Color.Orange;
                btnLoopToggleB.ForeColor = Color.White;
                btnLoopToggleB.FlatAppearance.BorderColor = Color.OrangeRed;
            }
            else
            {
                btnLoopToggleB.BackColor = Color.White;
                btnLoopToggleB.ForeColor = Color.Black;
                btnLoopToggleB.FlatAppearance.BorderColor = Color.LightGray;
            }
        }

        // --- Deck B 기본 제어 ---
        private void btnLoadB_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Deck B 음악 파일 선택";
                dialog.Filter = "음악 파일 (*.mp3;*.wav;*.flac)|*.mp3;*.wav;*.flac|모든 파일 (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK) LoadTrackB(dialog.FileName);
            }
        }

        private void btnPlayB_Click(object? sender, EventArgs e)
        {
            if (audioFileB == null || waveOutB == null) return;
            if (audioFileB.Position >= audioFileB.Length) audioFileB.Position = 0;
            waveOutB.Play();
        }

        private void btnPauseB_Click(object? sender, EventArgs e) => waveOutB?.Pause();

        private void btnStopB_Click(object? sender, EventArgs e)
        {
            if (waveOutB != null && audioFileB != null)
            {
                waveOutB.Stop();
                audioFileB.Position = 0;
                isLoopActiveB = false;
                if (cmbFxB != null) cmbFxB.SelectedIndex = 0;
                UpdateLoopUI_B();
                UpdateFxUI_B();
            }
        }

        private void trackVolumeB_Scroll(object? sender, EventArgs e) => UpdateVolumeB();
        private void UpdateVolumeB() { if (audioFileB != null && trackVolumeB != null) audioFileB.Volume = trackVolumeB.Value / 100.0f; }

        private void trackFilterB_Scroll(object? sender, EventArgs e) => UpdateFilterB();
        private void UpdateFilterB() { if (filterProviderB != null && trackFilterB != null) filterProviderB.FilterValue = trackFilterB.Value / 100.0f; }

        private void trackBassB_Scroll(object? sender, EventArgs e) => UpdateBassB();
        private void UpdateBassB() { if (filterProviderB != null && trackBassB != null) filterProviderB.BassValue = trackBassB.Value / 100.0f; }

        private void trackPositionB_MouseDown(object? sender, MouseEventArgs e) => isDraggingPositionB = true;
        private void trackPositionB_MouseUp(object? sender, MouseEventArgs e)
        {
            if (audioFileB != null && trackPositionB != null && audioFileB.TotalTime.TotalSeconds > 0)
            {
                double targetSeconds = (trackPositionB.Value / 100.0) * audioFileB.TotalTime.TotalSeconds;
                audioFileB.CurrentTime = TimeSpan.FromSeconds(targetSeconds);
                if (isLoopActiveB)
                {
                    loopStartB = audioFileB.CurrentTime;
                    RecalculateLoopEndB();
                }
            }
            isDraggingPositionB = false;
        }

        #endregion

        #region --- 8. 공용 타이머 및 종료 처리 ---

        private void timer_Tick(object? sender, EventArgs e)
        {
            // Deck A 타이머
            if (audioFileA != null)
            {
                TimeSpan currentA = audioFileA.CurrentTime;
                if (isLoopActiveA && currentA >= loopEndA)
                {
                    audioFileA.CurrentTime = loopStartA;
                    currentA = loopStartA;
                }

                TimeSpan totalA = audioFileA.TotalTime;
                if (lblTimeA != null) lblTimeA.Text = $"{currentA:mm\\:ss} / {totalA:mm\\:ss}";

                if (!isDraggingPositionA && trackPositionA != null && totalA.TotalSeconds > 0)
                {
                    double percentA = (currentA.TotalSeconds / totalA.TotalSeconds) * 100.0;
                    trackPositionA.Value = Math.Min(100, Math.Max(0, (int)percentA));
                }
            }

            // Deck B 타이머
            if (audioFileB != null)
            {
                TimeSpan currentB = audioFileB.CurrentTime;
                if (isLoopActiveB && currentB >= loopEndB)
                {
                    audioFileB.CurrentTime = loopStartB;
                    currentB = loopStartB;
                }

                TimeSpan totalB = audioFileB.TotalTime;
                if (lblTimeB != null) lblTimeB.Text = $"{currentB:mm\\:ss} / {totalB:mm\\:ss}";

                if (!isDraggingPositionB && trackPositionB != null && totalB.TotalSeconds > 0)
                {
                    double percentB = (currentB.TotalSeconds / totalB.TotalSeconds) * 100.0;
                    trackPositionB.Value = Math.Min(100, Math.Max(0, (int)percentB));
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timerA?.Stop();

            audioFileA?.Dispose();
            waveOutA?.Dispose();

            audioFileB?.Dispose();
            waveOutB?.Dispose();

            base.OnFormClosing(e);
        }

        #endregion
    }

    #region --- 9. 음원 속도(BPM) 엔진 ---

    public class SpeedSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private float playbackRate = 1.0f;

        public SpeedSampleProvider(ISampleProvider source)
        {
            this.source = source;
            this.WaveFormat = source.WaveFormat;
        }

        public WaveFormat WaveFormat { get; }

        public float PlaybackRate
        {
            get => playbackRate;
            set => playbackRate = Math.Max(0.01f, Math.Min(2.0f, value));
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (Math.Abs(playbackRate - 1.0f) < 0.01f)
            {
                return source.Read(buffer, offset, count);
            }

            int channels = WaveFormat.Channels;
            int framesRequested = count / channels;
            int sourceFramesNeeded = (int)Math.Ceiling(framesRequested * playbackRate) + 2;
            int sourceSamplesNeeded = sourceFramesNeeded * channels;

            float[] sourceBuffer = new float[sourceSamplesNeeded];
            int sourceSamplesRead = source.Read(sourceBuffer, 0, sourceSamplesNeeded);
            int sourceFramesRead = sourceSamplesRead / channels;

            if (sourceFramesRead == 0) return 0;

            int framesWritten = 0;
            for (int i = 0; i < framesRequested; i++)
            {
                float srcFramePos = i * playbackRate;
                int frame1 = (int)srcFramePos;
                int frame2 = frame1 + 1;
                float frac = srcFramePos - frame1;

                if (frame1 >= sourceFramesRead) break;
                if (frame2 >= sourceFramesRead) frame2 = frame1;

                for (int ch = 0; ch < channels; ch++)
                {
                    float s1 = sourceBuffer[frame1 * channels + ch];
                    float s2 = sourceBuffer[frame2 * channels + ch];
                    buffer[offset + (i * channels) + ch] = s1 + (s2 - s1) * frac;
                }
                framesWritten++;
            }

            return framesWritten * channels;
        }
    }

    #endregion

    #region --- 10. DJ 필터 + Bass + FX 통합 엔진 ---

    public class FilterSampleProvider : ISampleProvider
    {
        private readonly ISampleProvider source;
        private BiQuadFilter[]? filtersPass1;
        private BiQuadFilter[]? filtersPass2;
        private BiQuadFilter[]? bassFilters;

        private float filterValue = 0f;
        private float bassValue = 0f;
        private readonly object lockObject = new object();

        // FX 전용 필드 (0: OFF, 1: Echo, 2: Reverb)
        private int currentFxType = 0;
        private float fxAmount = 0f;

        private float[] delayBuffer = new float[44100 * 2]; // 2초 딜레이 버퍼
        private int delayWritePos = 0;

        public FilterSampleProvider(ISampleProvider source)
        {
            this.source = source;
            this.WaveFormat = source.WaveFormat;
        }

        public WaveFormat WaveFormat { get; }

        public float FilterValue
        {
            get => filterValue;
            set { filterValue = Math.Max(-1.0f, Math.Min(1.0f, value)); CreateFilters(); }
        }

        public float BassValue
        {
            get => bassValue;
            set { bassValue = Math.Max(-1.0f, Math.Min(1.0f, value)); CreateFilters(); }
        }

        // FX 제어 메서드
        public void SetFx(int type, float amount)
        {
            lock (lockObject)
            {
                currentFxType = type;
                fxAmount = amount;
            }
        }

        private void CreateFilters()
        {
            lock (lockObject)
            {
                int channels = WaveFormat.Channels;
                int sampleRate = WaveFormat.SampleRate;

                // High-Pass / Low-Pass Filter
                if (Math.Abs(filterValue) < 0.05f)
                {
                    filtersPass1 = null;
                    filtersPass2 = null;
                }
                else
                {
                    filtersPass1 = new BiQuadFilter[channels];
                    filtersPass2 = new BiQuadFilter[channels];

                    if (filterValue < 0)
                    {
                        float val = -filterValue;
                        float minCutoff = 70f;
                        float maxCutoff = 20000f;
                        float cutoff = maxCutoff * (float)Math.Pow(minCutoff / maxCutoff, val);
                        cutoff = Math.Min(sampleRate / 2 - 100, Math.Max(30f, cutoff));
                        float qLPF = 2.2f;

                        for (int n = 0; n < channels; n++)
                        {
                            filtersPass1[n] = BiQuadFilter.LowPassFilter(sampleRate, cutoff, qLPF);
                            filtersPass2[n] = BiQuadFilter.LowPassFilter(sampleRate, cutoff, qLPF);
                        }
                    }
                    else
                    {
                        float val = filterValue;
                        float minCutoff = 20f;
                        float maxCutoff = 6500f;
                        float cutoff = minCutoff * (float)Math.Pow(maxCutoff / minCutoff, val);
                        cutoff = Math.Min(sampleRate / 2 - 100, Math.Max(20f, cutoff));
                        float qHPF = 3.5f;

                        for (int n = 0; n < channels; n++)
                        {
                            filtersPass1[n] = BiQuadFilter.HighPassFilter(sampleRate, cutoff, qHPF);
                            filtersPass2[n] = BiQuadFilter.HighPassFilter(sampleRate, cutoff, qHPF);
                        }
                    }
                }

                // Bass (Low-Shelf EQ)
                if (Math.Abs(bassValue) < 0.05f)
                {
                    bassFilters = null;
                }
                else
                {
                    bassFilters = new BiQuadFilter[channels];
                    float gainDb = bassValue < 0 ? bassValue * 40f : bassValue * 18f;
                    float cutoff = 300f;
                    float slope = 1.5f;

                    for (int n = 0; n < channels; n++)
                    {
                        bassFilters[n] = BiQuadFilter.LowShelf(sampleRate, cutoff, slope, gainDb);
                    }
                }
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int samplesRead = source.Read(buffer, offset, count);
            if (samplesRead == 0) return 0;

            lock (lockObject)
            {
                int channels = WaveFormat.Channels;
                int sampleRate = WaveFormat.SampleRate;

                for (int i = 0; i < samplesRead; i++)
                {
                    int ch = i % channels;
                    float sample = buffer[offset + i];

                    // 1. HPF / LPF
                    if (filtersPass1 != null && filtersPass2 != null && ch < filtersPass1.Length)
                    {
                        if (filtersPass1[ch] != null && filtersPass2[ch] != null)
                        {
                            sample = filtersPass1[ch].Transform(sample);
                            sample = filtersPass2[ch].Transform(sample);
                        }
                    }

                    // 2. Bass EQ
                    if (bassFilters != null && ch < bassFilters.Length && bassFilters[ch] != null)
                    {
                        sample = bassFilters[ch].Transform(sample);
                    }

                    // 3. 실시간 FX 처리 (Echo / Reverb 전용)
                    if (currentFxType > 0 && fxAmount > 0)
                    {
                        switch (currentFxType)
                        {
                            case 1: // Echo (에코)
                                {
                                    int delaySamples = (int)(sampleRate * 0.375f) * channels;
                                    int readPos = (delayWritePos - delaySamples + delayBuffer.Length) % delayBuffer.Length;
                                    float delayedSample = delayBuffer[readPos];

                                    delayBuffer[delayWritePos] = sample + (delayedSample * 0.5f);
                                    sample = sample + (delayedSample * 0.6f);
                                }
                                break;

                            case 2: // Reverb (리버브)
                                {
                                    int delaySamples = (int)(sampleRate * 0.15f) * channels;
                                    int readPos = (delayWritePos - delaySamples + delayBuffer.Length) % delayBuffer.Length;
                                    float delayedSample = delayBuffer[readPos];

                                    delayBuffer[delayWritePos] = sample + (delayedSample * 0.7f);
                                    sample = (sample * 0.6f) + (delayedSample * 0.5f);
                                }
                                break;
                        }

                        delayWritePos = (delayWritePos + 1) % delayBuffer.Length;
                    }

                    buffer[offset + i] = sample;
                }
            }

            return samplesRead;
        }
    }

    #endregion
}
