using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp_0812
{
    public partial class Form1 : Form
    {
        private KnobControl midKnob;
        private EqualizerEngine eqEngine = new EqualizerEngine();

        // Mid 노브 현재 값 (0 ~ 100)
        private int midValue = 50; // 기본 중앙 (0dB)

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // knobs1(Mid 노브 PictureBox) 컨트롤 연결
            if (knobs1 != null)
            {
                midKnob = new KnobControl(knobs1);
                midKnob.Value = 0.5f; // 기본값 50% 설정

                // 노브 변경 시 Mid EQ 조절 이벤트 연동
                midKnob.ValueChanged += (ratio) =>
                {
                    int percent = (int)(ratio * 100);
                    OnMidKnobChanged(percent);
                };
            }
        }

        // [Mid] PictureBox 노브 조절 시 호출되는 이벤트
        private void OnMidKnobChanged(int knobValue)
        {
            midValue = knobValue;

            // 노브 0일 때 -30dB (보컬 감쇄), 50일 때 0dB, 100일 때 +12dB
            float gainDb = ((knobValue / 100f) * 42f) - 30f;

            // 디버그 출력
            System.Diagnostics.Debug.WriteLine($"[Mid 노브 변경] 입력값: {knobValue} -> 적용 게인: {gainDb} dB");

            eqEngine.SetMidGain(gainDb);
        }

        // 음원 재생 버튼 클릭 (btnOpenAndPlay 또는 button1)
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Audio Files|*.mp3;*.wav" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    eqEngine.LoadAudio(ofd.FileName);
                    eqEngine.Play();
                }
            }
        }

        private void btnOpenAndPlay_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            eqEngine.Dispose(); // 폼 닫힐 때 오디오 리소스 해제
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}