using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DJform
{
    public partial class DJform : Form
    {
        private KnobControl[] knobs = new KnobControl[6];
        private TurntableDeck? leftDeck;
        private TurntableDeck? rightDeck;
        private WaveformDeck? leftWaveform;
        private WaveformDeck? rightWaveform;
        private readonly float[] autoLoopBeatOptions = { 0.125f, 0.25f, 0.5f, 1f, 2f, 4f, 8f, 16f, 32f };
        private readonly string[] autoLoopBeatLabels = { "1/8", "1/4", "1/2", "1", "2", "4", "8", "16", "32" };
        private int autoLoopBeatIndexL = 5;
        private int autoLoopBeatIndexR = 5;

        public DJform()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void DJform_Load(object? sender, EventArgs e)
        {
            leftDeck = new TurntableDeck(TurntableL);
            rightDeck = new TurntableDeck(TurntableR);
            leftWaveform = new WaveformDeck(WaveFormL);
            rightWaveform = new WaveformDeck(WaveFormR);
            leftDeck.LinkWaveform(leftWaveform);
            rightDeck.LinkWaveform(rightWaveform);

            PictureBox[] pictureBoxes = new PictureBox[6]
            {
                BassKnobL, BassKnobR, MidKnobL, MidKnobR, FilterKnobL, FilterKnobR
            };

            for (int i = 0; i < pictureBoxes.Length; i++)
            {
                int index = i;
                knobs[index] = new KnobControl(pictureBoxes[index]);
                knobs[index].Value = 0.5f;
                knobs[index].ValueChanged += (ratio) =>
                {
                    OnKnobValueChanged(index, ratio);
                };
            }

            InitBpmControls();
            InitPitchControls();
            UpdateVolumes(null, EventArgs.Empty);
            InitializeEffectTrackBars();
            UpdateAutoLoopUi(true);
            UpdateAutoLoopUi(false);
        }

        private void AutoLoopL_Click(object? sender, EventArgs e)
        {
            leftWaveform?.ToggleAutoLoop(autoLoopBeatOptions[autoLoopBeatIndexL]);
            UpdateAutoLoopUi(true);
        }

        private void AutoLoopR_Click(object? sender, EventArgs e)
        {
            rightWaveform?.ToggleAutoLoop(autoLoopBeatOptions[autoLoopBeatIndexR]);
            UpdateAutoLoopUi(false);
        }

        private void AutoLoopDownL_Click(object? sender, EventArgs e)
        {
            if (autoLoopBeatIndexL > 0) autoLoopBeatIndexL--;
            leftWaveform?.SetAutoLoopBeats(autoLoopBeatOptions[autoLoopBeatIndexL]);
            UpdateAutoLoopUi(true);
        }

        private void AutoLoopUpL_Click(object? sender, EventArgs e)
        {
            if (autoLoopBeatIndexL < autoLoopBeatOptions.Length - 1) autoLoopBeatIndexL++;
            leftWaveform?.SetAutoLoopBeats(autoLoopBeatOptions[autoLoopBeatIndexL]);
            UpdateAutoLoopUi(true);
        }

        private void AutoLoopDownR_Click(object? sender, EventArgs e)
        {
            if (autoLoopBeatIndexR > 0) autoLoopBeatIndexR--;
            rightWaveform?.SetAutoLoopBeats(autoLoopBeatOptions[autoLoopBeatIndexR]);
            UpdateAutoLoopUi(false);
        }

        private void AutoLoopUpR_Click(object? sender, EventArgs e)
        {
            if (autoLoopBeatIndexR < autoLoopBeatOptions.Length - 1) autoLoopBeatIndexR++;
            rightWaveform?.SetAutoLoopBeats(autoLoopBeatOptions[autoLoopBeatIndexR]);
            UpdateAutoLoopUi(false);
        }

        private void UpdateAutoLoopUi(bool left)
        {
            Button button = left ? AutoLoopL : AutoLoopR;
            int index = left ? autoLoopBeatIndexL : autoLoopBeatIndexR;
            bool active = left
                ? leftWaveform?.IsAutoLoopActive == true
                : rightWaveform?.IsAutoLoopActive == true;

            button.Text = $"⟳  {autoLoopBeatLabels[index]}";
            button.BackColor = active ? Color.Orange : SystemColors.Control;
            button.ForeColor = active ? Color.White : SystemColors.ControlText;
            button.FlatAppearance.BorderColor = active ? Color.OrangeRed : SystemColors.ControlDark;
        }

        #region 🎛️ 이펙터 트랙바 제어

        private void InitializeEffectTrackBars()
        {
            SetupEffectTrackBar(StutterL, (s, e) => OnStutterScroll(leftWaveform, StutterL, CrushL, EchoL, FlangerL));
            SetupEffectTrackBar(CrushL, (s, e) => OnCrushScroll(leftWaveform, CrushL, StutterL, EchoL, FlangerL));
            SetupEffectTrackBar(EchoL, (s, e) => OnEchoScroll(leftWaveform, EchoL, StutterL, CrushL, FlangerL));
            SetupEffectTrackBar(FlangerL, (s, e) => OnFlangerScroll(leftWaveform, FlangerL, StutterL, CrushL, EchoL));

            SetupEffectTrackBar(StutterR, (s, e) => OnStutterScroll(rightWaveform, StutterR, CrushR, EchoR, FlangerR));
            SetupEffectTrackBar(CrushR, (s, e) => OnCrushScroll(rightWaveform, CrushR, StutterR, EchoR, FlangerR));
            SetupEffectTrackBar(EchoR, (s, e) => OnEchoScroll(rightWaveform, EchoR, StutterR, CrushR, FlangerR));
            SetupEffectTrackBar(FlangerR, (s, e) => OnFlangerScroll(rightWaveform, FlangerR, StutterR, CrushR, EchoR));
        }

        private void SetupEffectTrackBar(TrackBar? tb, EventHandler scrollHandler)
        {
            if (tb == null) return;

            tb.Minimum = 0;
            tb.Maximum = 100;
            tb.Scroll += scrollHandler;

            // 💡 더블 클릭 시 0으로 리셋
            tb.MouseDown += (s, e) =>
            {
                if (e.Clicks == 2)
                {
                    tb.Value = 0;
                    scrollHandler?.Invoke(s, e);
                }
            };
        }

        private void OnStutterScroll(WaveformDeck? deck, TrackBar active, params TrackBar[] others)
        {
            ResetOtherTrackBars(deck?.GlitchProvider, active, others);

            var provider = deck?.GlitchProvider;
            if (provider == null) return;

            int val = active.Value;
            if (val == 0) provider.StopStutter();
            else provider.TriggerStutter(150f - (val * 1.35f), 10000f, false);
        }

        private void OnCrushScroll(WaveformDeck? deck, TrackBar active, params TrackBar[] others)
        {
            ResetOtherTrackBars(deck?.GlitchProvider, active, others);
            deck?.GlitchProvider?.SetCrush(active.Value);
        }

        private void OnEchoScroll(WaveformDeck? deck, TrackBar active, params TrackBar[] others)
        {
            ResetOtherTrackBars(deck?.GlitchProvider, active, others);
            deck?.GlitchProvider?.SetEcho(active.Value);
        }

        private void OnFlangerScroll(WaveformDeck? deck, TrackBar active, params TrackBar[] others)
        {
            ResetOtherTrackBars(deck?.GlitchProvider, active, others);
            deck?.GlitchProvider?.SetFlanger(active.Value);
        }

        private void ResetOtherTrackBars(GlitchStutterProvider? provider, TrackBar active, TrackBar[] others)
        {
            if (active.Value > 0)
            {
                foreach (var tb in others)
                {
                    if (tb != null && tb.Value != 0)
                    {
                        tb.Value = 0;
                    }
                }
                provider?.ResetAllEffects();
            }
        }

        #endregion

        #region 🎛️ 노브 & BPM 컨트롤 이벤트

        private void InitBpmControls()
        {
            if (numMasterBpm != null)
            {
                numMasterBpm.ValueChanged -= numMasterBpm_ValueChanged;
                numMasterBpm.ValueChanged += numMasterBpm_ValueChanged;
            }

            if (numBpmL != null)
            {
                numBpmL.ValueChanged -= numBpmL_ValueChanged;
                numBpmL.ValueChanged += numBpmL_ValueChanged;
            }

            if (numBpmR != null)
            {
                numBpmR.ValueChanged -= numBpmR_ValueChanged;
                numBpmR.ValueChanged += numBpmR_ValueChanged;
            }
        }

        private void numBpmL_ValueChanged(object? sender, EventArgs e)
        {
            if (leftWaveform != null && numBpmL != null)
                leftWaveform.SetTargetBpm((float)numBpmL.Value);
        }

        private void numBpmR_ValueChanged(object? sender, EventArgs e)
        {
            if (rightWaveform != null && numBpmR != null)
                rightWaveform.SetTargetBpm((float)numBpmR.Value);
        }

        private void numMasterBpm_ValueChanged(object? sender, EventArgs e)
        {
            if (numMasterBpm == null) return;
            float targetBpm = (float)numMasterBpm.Value;

            SetNumericValue(numBpmL, targetBpm);
            SetNumericValue(numBpmR, targetBpm);

            leftWaveform?.SetTargetBpm(targetBpm);
            rightWaveform?.SetTargetBpm(targetBpm);
        }

        private void btnMasterSync_Click(object? sender, EventArgs e)
        {
            if (numMasterBpm == null) return;

            float masterBpm = (float)numMasterBpm.Value;

            SetNumericValue(numBpmL, masterBpm);
            SetNumericValue(numBpmR, masterBpm);

            leftWaveform?.SetTargetBpm(masterBpm);
            rightWaveform?.SetTargetBpm(masterBpm);
        }

        private void SetNumericValue(NumericUpDown? numControl, float value)
        {
            if (numControl == null) return;
            decimal clamped = (decimal)Math.Min((float)numControl.Maximum, Math.Max((float)numControl.Minimum, value));
            numControl.Value = clamped;
        }

        private void OnKnobValueChanged(int knobIndex, float ratio)
        {
            int percent = (int)(ratio * 100);

            switch (knobIndex)
            {
                case 0:
                    leftWaveform?.SetBass(KnobRatioToBipolar(ratio));
                    break;
                case 1:
                    rightWaveform?.SetBass(KnobRatioToBipolar(ratio));
                    break;
                case 2:
                    leftWaveform?.SetMidGain(KnobPercentToMidGain(percent));
                    break;
                case 3:
                    rightWaveform?.SetMidGain(KnobPercentToMidGain(percent));
                    break;
                case 4:
                    leftWaveform?.SetFilter(KnobRatioToBipolar(ratio));
                    break;
                case 5:
                    rightWaveform?.SetFilter(KnobRatioToBipolar(ratio));
                    break;
            }
        }

        private static float KnobRatioToBipolar(float ratio)
        {
            return Math.Clamp((ratio * 2f) - 1f, -1f, 1f);
        }

        private void InitPitchControls()
        {
            PitchControlL.Scroll -= PitchControlL_Scroll;
            PitchControlL.Scroll += PitchControlL_Scroll;
            PitchControlR.Scroll -= PitchControlR_Scroll;
            PitchControlR.Scroll += PitchControlR_Scroll;

            // 더블 클릭하면 원음(0 semitone)으로 복귀한다.
            PitchControlL.MouseDoubleClick += (_, _) => ResetPitch(PitchControlL, leftWaveform);
            PitchControlR.MouseDoubleClick += (_, _) => ResetPitch(PitchControlR, rightWaveform);

            leftWaveform?.SetPitch(PitchControlL.Value);
            rightWaveform?.SetPitch(PitchControlR.Value);
        }

        private void PitchControlL_Scroll(object? sender, EventArgs e)
        {
            leftWaveform?.SetPitch(PitchControlL.Value);
        }

        private void PitchControlR_Scroll(object? sender, EventArgs e)
        {
            rightWaveform?.SetPitch(PitchControlR.Value);
        }

        private static void ResetPitch(TrackBar control, WaveformDeck? deck)
        {
            control.Value = 0;
            deck?.SetPitch(0f);
        }

        private static float KnobPercentToMidGain(int percent)
        {
            // 0%=-30 dB, 50%=0 dB, 100%=+12 dB.
            // 두 구간을 나눠 중앙 노브가 실제로 0 dB가 되게 한다.
            float clamped = Math.Clamp(percent, 0, 100);
            return clamped <= 50f
                ? -30f + (clamped / 50f * 30f)
                : (clamped - 50f) / 50f * 12f;
        }

        #endregion

        private void DJform_FormClosing(object? sender, FormClosingEventArgs e)
        {
            leftDeck?.Dispose();
            rightDeck?.Dispose();
            leftWaveform?.Dispose();
            rightWaveform?.Dispose();
        }

        private void MusicInsertL_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Left Deck (A) 음원 선택";
                openFileDialog.Filter = "음악 파일 (*.mp3;*.wav)|*.mp3;*.wav|모든 파일 (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    float detectedBpm = AudioBpmHelper.DetectBpm(filePath);

                    SetNumericValue(numBpmL, detectedBpm);

                    leftWaveform?.LoadAudio(filePath, detectedBpm);
                    leftWaveform?.SetTargetBpm(detectedBpm);

                    UpdateVolumes(null, EventArgs.Empty);

                    if (MusicTextL != null) MusicTextL.Text = Path.GetFileName(filePath);

                    if (leftWaveform != null)
                    {
                        leftWaveform.PositionChanged -= LeftWaveform_PositionChanged;
                        leftWaveform.PositionChanged += LeftWaveform_PositionChanged;
                        LeftWaveform_PositionChanged();
                    }

                    UpdateMasterBpmToMax();
                }
            }
        }

        private void LeftWaveform_PositionChanged()
        {
            if (lblTimeL != null && lblTimeL.InvokeRequired)
            {
                lblTimeL.BeginInvoke(new Action(() => UpdateTimeLabel(lblTimeL, leftWaveform)));
            }
            else
            {
                UpdateTimeLabel(lblTimeL, leftWaveform);
            }
        }

        private void MusicInsertR_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Right Deck (B) 음원 선택";
                openFileDialog.Filter = "음악 파일 (*.mp3;*.wav)|*.mp3;*.wav|모든 파일 (*.*)|*.*";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    float detectedBpm = AudioBpmHelper.DetectBpm(filePath);

                    SetNumericValue(numBpmR, detectedBpm);

                    rightWaveform?.LoadAudio(filePath, detectedBpm);
                    rightWaveform?.SetTargetBpm(detectedBpm);

                    UpdateVolumes(null, EventArgs.Empty);

                    if (MusicTextR != null) MusicTextR.Text = Path.GetFileName(filePath);

                    if (rightWaveform != null)
                    {
                        rightWaveform.PositionChanged -= RightWaveform_PositionChanged;
                        rightWaveform.PositionChanged += RightWaveform_PositionChanged;
                        RightWaveform_PositionChanged();
                    }

                    UpdateMasterBpmToMax();
                }
            }
        }

        private void RightWaveform_PositionChanged()
        {
            if (lblTimeR != null && lblTimeR.InvokeRequired)
            {
                lblTimeR.BeginInvoke(new Action(() => UpdateTimeLabel(lblTimeR, rightWaveform)));
            }
            else
            {
                UpdateTimeLabel(lblTimeR, rightWaveform);
            }
        }

        private void LeftMusicStart_Click(object? sender, EventArgs e)
        {
            if (leftWaveform != null)
            {
                if (leftWaveform.IsPlaying)
                    leftWaveform.Pause();
                else
                    leftWaveform.Play();
            }
        }

        private void RightMusicStart_Click(object? sender, EventArgs e)
        {
            if (rightWaveform != null)
            {
                if (rightWaveform.IsPlaying)
                    rightWaveform.Pause();
                else
                    rightWaveform.Play();
            }
        }

        private void UpdateTimeLabel(Label? label, WaveformDeck? deck)
        {
            if (label == null || deck == null) return;

            TimeSpan current = deck.CurrentTime;
            TimeSpan total = deck.TotalTime;

            label.Text = $"{current:mm\\:ss} / {total:mm\\:ss}";
        }

        private void UpdateVolumes(object? sender, EventArgs e)
        {
            if (VolumeControlL == null || VolumeControlR == null || VolumeControlW == null) return;

            // 1. 개별 채널 트랙바 볼륨 비율 (0.0f ~ 1.0f)
            float baseVolL = VolumeControlL.Value / 100f;
            float baseVolR = VolumeControlR.Value / 100f;

            // 2. 크로스페이더 값 (Min: 0, Max: 200, Center: 100)
            int crossVal = VolumeControlW.Value;

            float crossRatioL = 1.0f;
            float crossRatioR = 1.0f;

            // 3. 크로스페이더 비율 연산 (중앙값 100 기준)
            if (crossVal > 100)
            {
                // 오른쪽 이동: L 감소 (100 -> 200 갈 때 1.0 -> 0.0)
                crossRatioL = (200 - crossVal) / 100f;
                crossRatioR = 1.0f;
            }
            else if (crossVal < 100)
            {
                // 왼쪽 이동: R 감소 (100 -> 0 갈 때 1.0 -> 0.0)
                crossRatioL = 1.0f;
                crossRatioR = crossVal / 100f;
            }
            else
            {
                // 중앙(100): 양쪽 100%
                crossRatioL = 1.0f;
                crossRatioR = 1.0f;
            }

            // 4. 최종 볼륨 적용
            leftWaveform?.SetVolume(baseVolL * crossRatioL);
            rightWaveform?.SetVolume(baseVolR * crossRatioR);
        }

        private void UpdateMasterBpmToMax()
        {
            float bpmL = leftWaveform?.BaseBpm ?? (numBpmL != null ? (float)numBpmL.Value : 120.0f);
            float bpmR = rightWaveform?.BaseBpm ?? (numBpmR != null ? (float)numBpmR.Value : 120.0f);

            float maxBpm = Math.Max(bpmL, bpmR);

            SetNumericValue(numMasterBpm, maxBpm);
        }
    }
}
