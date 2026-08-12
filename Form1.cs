using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave; // NuGet 패키지 관리자에서 NAudio 설치 필수

namespace DJ
{
    public partial class Form1 : Form
    {
        private AudioFileReader audioFile; //음원 파일 데이터 관리자 (노래의 전체 길이:TotalTime, 현재 재생 위치:CurrentTime)
        private WaveOutEvent waveOut; // 사운드 카드를 통한 오디오 출력 장치 (audioFile이 읽어온 걸 출력(Play, Stop)하기 위해)
        private float[] peaks; //소리 크기 저장 배열 
        private System.Windows.Forms.Timer timer; // 실시간 화면 갱신용 타이머 (pictureBox1.Invalidate 명령함)

        public Form1()
        {
            InitializeComponent();

            // 화면 실시간 갱신용 타이머 (약 33 FPS)
            timer = new System.Windows.Forms.Timer(); //타이머 객체 생성
            timer.Interval = 30; //타이머 실행 주기 30ms로 지정
            timer.Tick += (s, e) => pictureBox1.Invalidate(); //s는 이벤트 발생 객체, e는 이벤트 매개변수, 즉시 다시 그려라

            // PictureBox 이벤트 연결
            pictureBox1.Paint += PictureBox1_Paint; //e.Graphics 객체 통해 피쳐박스에 선등등 직접 그림
            pictureBox1.MouseDown += PictureBox1_MouseDown; // 클릭된 위치의 좌표를 얻거나 마우스 선택 동작을 구현함
            pictureBox1.Resize += (s, e) => pictureBox1.Invalidate(); // 크기 변경 시 파형 재계산/재렌더링

            // 폼 종료 시 오디오 리소스 해제
            this.FormClosing += Form1_FormClosing;
        }

        // 버튼 클릭 시 음악 파일 열기
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "오디오 파일 (*.mp3;*.wav)|*.mp3;*.wav|모든 파일 (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadAudio(openFileDialog.FileName);
                }
            }
        }

        // 음원 불러오기 및 파형 데이터 추출
        public void LoadAudio(string filePath)
        {
            CleanupAudio();

            // 1. [파형 분석] 독립된 임시 객체로 피크 데이터 추출 후 해제
            using (var tempReader = new AudioFileReader(filePath))
            {
                int width = Math.Max(1, pictureBox1.Width);
                peaks = new float[width];
                int samplesPerPixel = (int)(tempReader.Length / (width * sizeof(float)));
                if (samplesPerPixel < 1) samplesPerPixel = 1;

                float[] buffer = new float[samplesPerPixel];

                for (int i = 0; i < width; i++)
                {
                    int read = tempReader.Read(buffer, 0, samplesPerPixel);
                    if (read == 0) break;
                    peaks[i] = buffer.Max(f => Math.Abs(f));
                }
            }

            // 2. [실제 재생] 깨끗한 상태의 새로운 AudioFileReader 생성 및 재생
            audioFile = new AudioFileReader(filePath);
            waveOut = new WaveOutEvent();
            waveOut.Init(audioFile);
            waveOut.Play();        // 재생 시작
            timer.Start();         // 애니메이션 타이머 시작
        }

        // PictureBox 크기에 맞춰 반쪽 파형 그리기
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (peaks == null || audioFile == null || peaks.Length == 0) return;

            // 어두운 배경색 채우기
            e.Graphics.Clear(Color.FromArgb(20, 22, 32));

            int currentWidth = pictureBox1.Width;
            int currentHeight = pictureBox1.Height;

            // 현재 진행 비율 계산
            double progress = audioFile.CurrentTime.TotalMilliseconds / audioFile.TotalTime.TotalMilliseconds;
            int progressX = (int)(currentWidth * progress);

            // PictureBox 너비에 맞게 막대 간격과 두께 자동 계산 (빈 틈 방지)
            float stepX = (float)currentWidth / peaks.Length;
            float penWidth = Math.Max(1.0f, stepX);

            for (int i = 0; i < peaks.Length; i++)
            {
                float x = i * stepX;
                // PictureBox 높이의 최대 95%까지 사용
                int barHeight = (int)(peaks[i] * (currentHeight * 0.6f));
                Color barColor = (x < progressX) ? Color.FromArgb(255, 120, 0) : Color.FromArgb(80, 85, 100);

                using (Pen pen = new Pen(barColor, penWidth))
                {
                    // 바닥(currentHeight)에서 위쪽(currentHeight - barHeight)으로만 그리기
                    e.Graphics.DrawLine(pen, x, currentHeight, x, currentHeight - barHeight);
                }
            }

            // 재생 위치 세로선 표시 (흰색)
            using (Pen whitePen = new Pen(Color.White, 2))
            {
                e.Graphics.DrawLine(whitePen, progressX, 0, progressX, currentHeight);
            }
        }

        // 마우스 클릭 위치로 이동 (Seek)
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (audioFile == null || audioFile.TotalTime.TotalMilliseconds <= 0) return;

            double targetRatio = (double)e.X / pictureBox1.Width;
            targetRatio = Math.Max(0, Math.Min(1, targetRatio));

            audioFile.CurrentTime = TimeSpan.FromMilliseconds(audioFile.TotalTime.TotalMilliseconds * targetRatio);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // MouseDown에서 위치 이동을 처리하므로 비워둡니다.
        }

        private void CleanupAudio()
        {
            timer?.Stop();
            waveOut?.Stop();
            waveOut?.Dispose();
            audioFile?.Dispose();
            waveOut = null;
            audioFile = null;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanupAudio();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (glitchProvider == null) return;

            int sliderValue = trackBar1.Value;

            if (sliderValue == 0)
            {
                // 0으로 놓으면 스터터 해제 (정상 재생)
                glitchProvider.StopStutter();
            }
            else
            {
                // 트랙바 위치(예: 10ms~100ms)에 맞춰 실시간 스터터 발동
                // totalMs를 충분히 길게 지정하여 드래그하는 동안 스터터가 유지되도록 설정
                float sliceMs = sliderValue;
                float totalMs = 5000f; // 드래그 중 지속 시간 (5초)

                glitchProvider.TriggerStutter(sliceMs, totalMs, chkReverse.Checked);
            }
        }
            private void trackBar1_MouseUp(object me, MouseEventArgs e)
        {
            // 손을 떼면 원상복구되는 스위치 형태로 쓰고 싶을 때 활성화
             trackBar1.Value = 0;
             glitchProvider?.StopStutter();
        }
        public class GlitchStutterProvider : ISampleProvider
        {           
            /// <summary>
            /// 트랙바가 0이 되었을 때 스터터를 즉시 중단하고 원본 오디오로 복귀
            /// </summary>
            public void StopStutter()
            {
                this.isStuttering = false;
                this.stutterRemainingSamples = 0;
            }
        }
    }
}
