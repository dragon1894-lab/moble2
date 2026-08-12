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
        private GlitchStutterProvider glitchProvider;
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

            // 트랙바 이벤트 직접 수동 연결
            trackBar2.Scroll += trackBar2_Scroll;
            trackBar2.MouseUp += trackBar2_MouseUp;

            trackBar4.Minimum = 0;
            trackBar4.Maximum = 100;
            trackBar4.Scroll += trackBar4_Scroll;
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
            glitchProvider = new GlitchStutterProvider(audioFile);
            waveOut = new WaveOutEvent();
            waveOut.Init(glitchProvider);
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
            glitchProvider = null;
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
                // 0일 때는 스터터 해제 (정상 오디오 재생)
                glitchProvider.StopStutter();
            }
            else
            {
                // 수식 반전: 왼쪽(1)에서 오른쪽(100)으로 갈수록 ms가 줄어들어 속도가 빨라짐
                // - 트랙바 1   : 150ms (느린 "두 .. 두 .. 두")
                // - 트랙바 50  : 82.5ms (중간 속도 "두-두-두-두")
                // - 트랙바 100 : 15ms (매우 빠른 "두두두두두")
                float sliceMs = 150f - (sliderValue * 1.35f);

                // 스터터 발동 (100000ms 동안 유지)
                glitchProvider.TriggerStutter(sliceMs, 10000f, false);
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
            private readonly ISampleProvider source;
            private readonly float[] ringBuffer;
            private int ringWritePos;

            //스터터
            private bool isStuttering;
            private int stutterStartPos;
            private int stutterLength;
            private int stutterReadOffset;
            private int stutterRemainingSamples;
            private bool isReverse;


            // [중요] public 키워드가 반드시 붙어야 합니다.
            // ★ [추가] 크러시 수치 (0: 원본, 1~100: 크러시 강도)
            private float crushLevel = 0f;
            private float lastCrushedSample = 0f;
            private int downsampleCounter = 0;

            // ★ [추가] 에코(Echo/Delay) 변수
            private float echoLevel = 0f;
            private readonly float[] delayBuffer;
            private int delayWritePos;
            private readonly int delaySampleLength; // 약 250ms 지연 시간 (1/4 박자)
            // ★ [추가] 플랜저 변수
            private float flangerLevel = 0f;
            private readonly float[] flangerBuffer;
            private int flangerWritePos;
            private float flangerPhase = 0f;
            public WaveFormat WaveFormat => source.WaveFormat;

            public GlitchStutterProvider(ISampleProvider source, float maxBufferSeconds = 1.0f)
            {
                this.source = source;
                int channels = source.WaveFormat.Channels;
                int sampleRate = source.WaveFormat.SampleRate;

                // 스터터 버퍼 생성
                int bufferSize = (int)(sampleRate * channels * maxBufferSeconds);
                this.ringBuffer = new float[bufferSize];

                // 에코 버퍼 생성 (250ms 지연)
                this.delaySampleLength = (int)(sampleRate * channels * 0.25f);
                this.delaySampleLength -= this.delaySampleLength % channels;
                this.delayBuffer = new float[sampleRate * channels * 2]; // 2초 분량 버퍼
                // 플랜저 버퍼 (20ms 버퍼)
                int flangerBufSize = sampleRate * channels;
                this.flangerBuffer = new float[flangerBufSize];
            }
            // ★ [추가] trackBar2에서 호출할 SetCrush 메서드 (CS1061 에러 해결)
            public void SetCrush(float level)
            {
                this.crushLevel = Math.Max(0f, Math.Min(100f, level));
            }
            public void SetEcho(float level)
            {
                this.echoLevel = Math.Max(0f, Math.Min(100f, level));
            }
            // ★ [추가] SetFlanger
            public void SetFlanger(float level)
            {
                this.flangerLevel = Math.Max(0f, Math.Min(100f, level));
            }
            public void TriggerStutter(float sliceMs, float totalMs, bool reverse = false)
            {
                int channels = WaveFormat.Channels;
                int samplesPerMs = (WaveFormat.SampleRate * channels) / 100;

                // 스테레오 채널 짝수 정렬
                int calculatedLength = Math.Max(channels, (int)(sliceMs * samplesPerMs));
                calculatedLength -= calculatedLength % channels;

                // ★ [핵심] 이미 스터터 중이라면 시작 위치를 유지하고 조각 길이만 실시간 변경
                if (isStuttering)
                {
                    this.stutterLength = calculatedLength;
                    return;
                }

                // 처음 스터터를 켤 때만 버퍼 위치 지정
                this.stutterLength = calculatedLength;
                this.stutterRemainingSamples = (int)(totalMs * samplesPerMs);

                int rawStart = ringWritePos - stutterLength;
                this.stutterStartPos = ((rawStart % ringBuffer.Length) + ringBuffer.Length) % ringBuffer.Length;
                this.stutterStartPos -= this.stutterStartPos % channels; // 채널 정렬

                this.stutterReadOffset = reverse ? stutterLength - channels : 0;
                this.isReverse = reverse;
                this.isStuttering = true;
            }

            public void StopStutter()
            {
                this.isStuttering = false;
                this.stutterRemainingSamples = 0;
            }

            // [중요] public 키워드가 반드시 붙어야 합니다.
            public int Read(float[] buffer, int offset, int count)
            {
                int samplesRead = source.Read(buffer, offset, count);

                for (int i = 0; i < samplesRead; i++)
                {
                    int currentIdx = offset + i;
                    float outSample = 0f;
                    //1. 스터터 연산
                    if (isStuttering && stutterLength > 0)
                    {
                        // 음수 나머지 방지 인덱스 연산
                        int rawReadIdx = stutterStartPos + stutterReadOffset;
                        int readIndex = ((rawReadIdx % ringBuffer.Length) + ringBuffer.Length) % ringBuffer.Length;

                        outSample = ringBuffer[readIndex];

                        if (isReverse)
                        {
                            stutterReadOffset--;
                            if (stutterReadOffset < 0) stutterReadOffset = stutterLength - 1;
                        }
                        else
                        {
                            stutterReadOffset = (stutterReadOffset + 1) % stutterLength;
                        }

                        stutterRemainingSamples--;
                        if (stutterRemainingSamples <= 0) isStuttering = false;
                    }
                    else
                    {
                        outSample = buffer[currentIdx];
                        ringBuffer[ringWritePos] = outSample;
                        ringWritePos = (ringWritePos + 1) % ringBuffer.Length;
                    }
                    //크러시
                    if (crushLevel > 0)
                    {
                        float ratio = crushLevel / 100f;

                        // A. 다운샘플링 (샘플링 레이트를 떨어뜨려 로보틱/금속성 질감 생성)
                        int holdFactor = (int)(1 + (ratio * 12)); // 샘플 유지 주기

                        downsampleCounter++;
                        if (downsampleCounter >= holdFactor)
                        {
                            downsampleCounter = 0;

                            // B. 비트 해상도 절삭 (16bit -> 3bit)
                            float bits = 16f - (ratio * 13f);
                            float steps = MathF.Pow(2, Math.Max(2f, bits));
                            float quantized = MathF.Round(outSample * steps) / steps;

                            // C. 게인 보정 (적절한 드라이브)
                            float drive = 1.0f + (ratio * 0.4f);
                            lastCrushedSample = Math.Max(-1.0f, Math.Min(1.0f, quantized * drive));
                        }

                        // 이전 샘플값을 유지해서 계단 현상(Aliasing)을 만듦
                        outSample = lastCrushedSample;
                    }
                    else
                    {
                        downsampleCounter = 0; // 초기화
                    }
                    // 3. 에코(Echo / Delay) 연산
                    if (echoLevel > 0)
                    {
                        int delayReadIndex = (delayWritePos - delaySampleLength + delayBuffer.Length) % delayBuffer.Length;
                        float delayedSample = delayBuffer[delayReadIndex];

                        float feedback = (echoLevel / 100f) * 0.6f;
                        float wet = (echoLevel / 100f) * 0.5f;

                        delayBuffer[delayWritePos] = outSample + (delayedSample * feedback);
                        delayWritePos = (delayWritePos + 1) % delayBuffer.Length;

                        outSample = outSample + (delayedSample * wet);
                    }
                    // 4. ★ [추가] 플랜저(Flanger)
                    // 4. YOUDJ 스타일 강렬한 제트 플랜저 (High Feedback + Deep Comb Filter)
                    flangerBuffer[flangerWritePos] = outSample;

                    if (flangerLevel > 0)
                    {
                        float ratio = flangerLevel / 100f; // 0.0 ~ 1.0
                        int channels = WaveFormat.Channels;

                        // LFO (0.6Hz 주기로 상하 스윕)
                        flangerPhase += (float)(2.0 * Math.PI * 0.6 / (WaveFormat.SampleRate * channels));
                        if (flangerPhase > Math.PI * 2) flangerPhase -= (float)(Math.PI * 2);

                        // 1. 짧고 촘촘한 지연 시간 (0.2ms ~ 6.0ms) -> 날카로운 고주파 빗살 필터 형성
                        float lfo = (MathF.Sin(flangerPhase) + 1.0f) * 0.5f;
                        float delayMs = 0.2f + (lfo * 5.8f);

                        int delaySamples = (int)(delayMs * (WaveFormat.SampleRate / 1000f)) * channels;

                        int readPos = ((flangerWritePos - delaySamples) % flangerBuffer.Length + flangerBuffer.Length) % flangerBuffer.Length;
                        float delayedSample = flangerBuffer[readPos];

                        // 2. YOUDJ의 핵심: 트랙바를 올릴수록 강해지는 고피드백 (최대 75%) -> 금속성 비행기음 생성
                        float feedback = 0.75f * ratio;
                        flangerBuffer[flangerWritePos] += delayedSample * feedback;

                        // 3. 위상 상쇄가 가장 깊게 일어나는 50:50 믹스 비율 (음량 변화 없이 가장 강렬한 플랜저)
                        float dryGain = 1.0f - (0.5f * ratio); // 1.0 -> 0.5
                        float wetGain = 0.5f * ratio;          // 0.0 -> 0.5

                        outSample = (outSample * dryGain) + (delayedSample * wetGain);
                        outSample = Math.Max(-1.0f, Math.Min(1.0f, outSample));
                    }

                    flangerWritePos = (flangerWritePos + 1) % flangerBuffer.Length;
                    // 최종 처리된 샘플 저장
                    buffer[currentIdx] = outSample;
                }
                return samplesRead;
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (glitchProvider == null) return;

            // trackBar2의 값(0~100)을 실시간으로 크러시 필터에 전달
            glitchProvider.SetCrush(trackBar2.Value);
        }
        // (선택) 마우스 클릭을 뗐을 때 크러시 효과를 끄고 원본 소리로 복구
        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            // 손을 떼는 순간 맑은 소리로 즉시 돌아오게 하려면 아래 두 줄의 주석(//)을 해제하세요.
            trackBar2.Value = 0;
            glitchProvider?.SetCrush(0);
        }

        // trackBar3 스크롤 시 에코 실시간 적용
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            if (glitchProvider == null) return;

            // 0~100 수치를 에코 필터로 전달
            glitchProvider.SetEcho(trackBar3.Value);
        }

        // (선택) 손을 뗐을 때 잔향을 끄고 싶다면 주석(//) 해제
        private void trackBar3_MouseUp(object sender, MouseEventArgs e)
        {
            trackBar3.Value = 0;
            glitchProvider?.SetEcho(0);
        }

        // ★ [추가] trackBar4: 플랜저(Flanger)
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            if (glitchProvider == null) return;
            glitchProvider.SetFlanger(trackBar4.Value);
        }

        private void trackBar4_MouseUp(object sender, MouseEventArgs e)
        {
            trackBar4.Value = 0;
            glitchProvider?.SetFlanger(0);
        }
    }
}
 