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
        public Form3(string userName) // Form1에서 회원가입한 유저네임을 전달받음
        {
            InitializeComponent();
            lb_UserName.Text = userName; // 전달받은 유저네임을 라벨에 표시
        }

        private void bt_Icon_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();

            // Form4를 열고 사용자가 사진을 선택할 때까지 기다림
            // DialogResult.OK는 Form4에서 사진 선택을 완료했다는 뜻
            if (form4.ShowDialog() == DialogResult.OK)
            {
                // Form4에서 선택한 사진을 Form3의 프로필 사진으로 변경
                pb_Profile.Image = form4.SelectedImage;
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


