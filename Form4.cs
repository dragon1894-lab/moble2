using System;
using System.Drawing;
using System.Windows.Forms;

namespace login1_0811
{
    public partial class Form4 : Form
    {
        // 사용자가 선택한 프로필 사진을 저장하는 변수
        // Form4 안에서는 사진을 저장하고, Form3에서는 저장된 사진을 읽을 수 있게 함
        public Image SelectedImage { get; private set; }

        public Form4()
        {
            InitializeComponent();
        }

        // 클릭한 버튼의 사진을 저장하고 Form4를 닫음
        private void SelectProfile(Button profileButton)
        {
            // 클릭한 버튼의 사진을 SelectedImage에 저장
            // BackgroundImage: 버튼에 넣어둔 프로필 사진
            SelectedImage = profileButton.Image;

            // DialogResult는 다이얼로그 Form4이 어떻게 종료되는지 알려주는 값
            // OK는 사용자가 프로필 사진 선택을 완료했다는 의미
            DialogResult = DialogResult.OK;

            Close();
        }

        // 첫 번째 프로필 선택
        private void bt_Icon1_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon1);
        }

        // 두 번째 프로필 선택
        private void bt_Icon2_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon2);
        }

        // 세 번째 프로필 선택
        private void bt_Icon3_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon3);
        }

        // 네 번째 프로필 선택
        private void bt_Icon4_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon4);
        }
    }
}

