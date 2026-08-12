using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace login1_0811
{
    public partial class Form3 : Form
    {
        string CurrentPW = ""; // 현재 비밀번호 저장
        public string ChangedPW = ""; // 변경된 비밀번호를 Form1에 전달

        public Form3(string userName, string password) // Form1에서 유저네임과 비밀번호를 전달받음
        {
            InitializeComponent();
            lb_UserName.Text = userName; // 전달받은 유저네임을 라벨에 표시
            CurrentPW = password; // 전달받은 현재 비밀번호 저장
        }

        private void bt_Icon_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(CurrentPW);

            // Form4를 열고 닫힐 때까지 기다림
            form4.ShowDialog();

            // Form4에서 프로필 사진을 선택했으면 Form3의 사진을 변경
            if (form4.SelectedImage != null)
            {
                pb_Profile.Image = form4.SelectedImage;
            }

            // Form4에서 비밀번호를 변경했으면 새 비밀번호 저장
            if (form4.ChangedPW != "")
            {
                CurrentPW = form4.ChangedPW;
                ChangedPW = form4.ChangedPW;
            }
        }

        private void bt_Mixing_Click(object sender, EventArgs e)
        {

        }

        private void bt_SoundCloud_Click(object sender, EventArgs e)
        {

        }
    }
}


