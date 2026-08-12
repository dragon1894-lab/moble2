using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace WinFormsApp6
{
    public partial class Form1 : Form
    {
        // ==========================================
        // Deck A
        // ==========================================
        private AudioFileReader? audioFileA;
        private WaveOutEvent? outputDeviceA;

        // Deck A Hot Cue
        private TimeSpan hotCueA1 = TimeSpan.FromMinutes(2);
        private TimeSpan hotCueA2 = TimeSpan.FromSeconds(85);

        // Deck A Loop (박자 쪼개기 변수)
        private System.Windows.Forms.Timer timerLoop;
        private double bpmA = 120.0;          // 기본 BPM
        private double loopBeatsA = 4.0;       // 기본 4박자
        private bool isLoopActiveA = false;   // 루프 활성화 여부
        private TimeSpan loopStartA;          // 루프 시작 시점
        private TimeSpan loopEndA;            // 루프 종료 시점


        // ==========================================
        // Deck B
        // ==========================================
        private AudioFileReader? audioFileB;
        private WaveOutEvent? outputDeviceB;

        // Deck B Hot Cue
        private TimeSpan hotCueB1 = TimeSpan.FromMinutes(2);
        private TimeSpan hotCueB2 = TimeSpan.FromSeconds(85);


        public Form1()
        {
            InitializeComponent();

            // Form이 키보드 입력을 받도록 설정
            this.KeyPreview = true;

            // 키보드 이벤트 연결
            this.KeyDown += Form1_KeyDown;

            // 실시간 루프 감지 타이머 설정 (30ms 간격)
            timerLoop = new System.Windows.Forms.Timer();
            timerLoop.Interval = 30;
            timerLoop.Tick += TimerLoop_Tick;
            timerLoop.Start();

            // 초기 버튼 텍스트 표시
            UpdateButton5Text();

            this.FormClosing += Form1_FormClosing;
        }

        // button5 텍스트 업데이트 (현재 상태 및 박자)
        private void UpdateButton5Text()
        {
            string displayBeats = loopBeatsA < 1 ? $"1/{(int)(1 / loopBeatsA)}" : loopBeatsA.ToString();
            button5.Text = isLoopActiveA ? $"🔁 ON ({displayBeats})" : $"🔁 {displayBeats}";
        }

        // 실시간 루프 검사 및 위치 리셋
        private void TimerLoop_Tick(object? sender, EventArgs e)
        {
            if (isLoopActiveA && audioFileA != null)
            {
                // 재생 위치가 루프 종료 지점에 도달하거나 넘어서면 시작 위치로 강제 이동
                if (audioFileA.CurrentTime >= loopEndA)
                {
                    audioFileA.CurrentTime = loopStartA;
                }
            }
        }

        // 박자에 맞춘 루프 구간 재계산
        private void UpdateLoopRangeA()
        {
            if (audioFileA == null) return;

            // 1박자의 초 단위 시간 = 60 / BPM
            double secondsPerBeat = 60.0 / bpmA;
            double loopDurationSeconds = secondsPerBeat * loopBeatsA;

            loopEndA = loopStartA.Add(TimeSpan.FromSeconds(loopDurationSeconds));

            // 총 길이를 초과하지 않도록 예외 처리
            if (loopEndA > audioFileA.TotalTime)
            {
                loopEndA = audioFileA.TotalTime;
            }
        }


        // ==========================================
        // 🎵 Deck A 노래 불러오기
        // ==========================================
        private void button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Deck A 노래 선택";

            dialog.Filter =
                "MP3 파일|*.mp3|" +
                "WAV 파일|*.wav|" +
                "모든 파일|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                // 기존 Deck A 및 루프 상태 정리
                isLoopActiveA = false;
                UpdateButton5Text();

                outputDeviceA?.Stop();
                outputDeviceA?.Dispose();
                audioFileA?.Dispose();

                // 노래 열기
                audioFileA = new AudioFileReader(dialog.FileName);

                // 출력 장치
                outputDeviceA = new WaveOutEvent();

                // 연결
                outputDeviceA.Init(audioFileA);

                // 바로 재생
                outputDeviceA.Play();

                MessageBox.Show(
                    "Deck A 재생 시작!\n\n" +
                    Path.GetFileName(dialog.FileName)
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Deck A 재생 오류:\n\n" +
                    ex.ToString()
                );
            }
        }


        // ==========================================
        // 🎹 키보드 입력 (J: +, K: -, L: Loop)
        // ==========================================
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            // --------------------------------------
            // Deck A Hot Cue
            // --------------------------------------
            if (e.KeyCode == Keys.A)
            {
                HotCueA1();
            }

            if (e.KeyCode == Keys.Z)
            {
                HotCueA2();
            }

            // --------------------------------------
            // Deck A Loop 조작 (J: +, K: -, L: Loop)
            // --------------------------------------
            if (e.KeyCode == Keys.J)
            {
                button3_Click(sender!, e); // (+) 버튼 실행
            }

            if (e.KeyCode == Keys.K)
            {
                button4_Click(sender!, e); // (-) 버튼 실행
            }

            if (e.KeyCode == Keys.L)
            {
                button5_Click(sender!, e); // (중앙 루프 ON/OFF) 버튼 실행
            }

            // --------------------------------------
            // Deck B Hot Cue
            // --------------------------------------
            if (e.KeyCode == Keys.Q)
            {
                HotCueB1();
            }

            if (e.KeyCode == Keys.W)
            {
                HotCueB2();
            }
        }


        // ==========================================
        // 🔴 Deck A / Hot Cue
        // ==========================================
        private void HotCueA1()
        {
            if (audioFileA == null)
            {
                MessageBox.Show("Deck A에 노래를 먼저 넣으세요.");
                return;
            }

            audioFileA.CurrentTime = hotCueA1;

            if (isLoopActiveA)
            {
                loopStartA = audioFileA.CurrentTime;
                UpdateLoopRangeA();
            }

            outputDeviceA?.Play();
        }

        private void HotCueA2()
        {
            if (audioFileA == null)
            {
                MessageBox.Show("Deck A에 노래를 먼저 넣으세요.");
                return;
            }

            audioFileA.CurrentTime = hotCueA2;

            if (isLoopActiveA)
            {
                loopStartA = audioFileA.CurrentTime;
                UpdateLoopRangeA();
            }

            outputDeviceA?.Play();
        }


        // ==========================================
        // 🔵 Deck B / Hot Cue
        // ==========================================
        private void HotCueB1()
        {
            if (audioFileB == null)
            {
                MessageBox.Show("Deck B에 노래를 먼저 넣으세요.");
                return;
            }

            audioFileB.CurrentTime = hotCueB1;
            outputDeviceB?.Play();
        }

        private void HotCueB2()
        {
            if (audioFileB == null)
            {
                MessageBox.Show("Deck B에 노래를 먼저 넣으세요.");
                return;
            }

            audioFileB.CurrentTime = hotCueB2;
            outputDeviceB?.Play();
        }


        // ==========================================
        // 🎵 Deck B 노래 불러오기
        // ==========================================
        private void button2_Click_1(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Deck B 노래 선택";

            dialog.Filter =
                "MP3 파일|*.mp3|" +
                "WAV 파일|*.wav|" +
                "모든 파일|*.*";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                outputDeviceB?.Stop();
                outputDeviceB?.Dispose();
                audioFileB?.Dispose();

                audioFileB = new AudioFileReader(dialog.FileName);
                outputDeviceB = new WaveOutEvent();

                outputDeviceB.Init(audioFileB);
                outputDeviceB.Play();

                MessageBox.Show(
                    "Deck B 재생 시작!\n\n" +
                    Path.GetFileName(dialog.FileName)
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Deck B 재생 오류:\n\n" +
                    ex.ToString()
                );
            }
        }


        // ==========================================
        // 🔄 박자 루프 조작 버튼
        // ==========================================

        // [버튼 3 / Key: J] 플러스 (+) : 박자 2배로 늘리기
        private void button3_Click(object sender, EventArgs e)
        {
            loopBeatsA = Math.Min(32.0, loopBeatsA * 2.0); // 최대 32박자
            UpdateLoopRangeA();
            UpdateButton5Text();
        }

        // [버튼 4 / Key: K] 마이너스 (-) : 박자 절반으로 쪼개기
        private void button4_Click(object sender, EventArgs e)
        {
            loopBeatsA = Math.Max(0.125, loopBeatsA / 2.0); // 최소 1/8박자
            UpdateLoopRangeA();
            UpdateButton5Text();
        }

        // [버튼 5 / Key: L] 중앙 루프 : 루프 켜기 / 끄기
        private void button5_Click(object sender, EventArgs e)
        {
            if (audioFileA == null)
            {
                MessageBox.Show("Deck A에 노래를 먼저 넣으세요.");
                return;
            }

            // 루프 상태 토글 (ON/OFF)
            isLoopActiveA = !isLoopActiveA;

            if (isLoopActiveA)
            {
                loopStartA = audioFileA.CurrentTime; // 현재 위치를 루프 시작점으로 잡음
                UpdateLoopRangeA();
            }

            UpdateButton5Text();
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            timerLoop?.Stop();
            outputDeviceA?.Dispose();
            audioFileA?.Dispose();
            outputDeviceB?.Dispose();
            audioFileB?.Dispose();
        }
    }
}