using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO;

namespace login1_0811
{
    public partial class Form3 : Form
    {
        public Form3(string userName) // Form1에서 회원가입한 유저네임을 전달받음
        {
            InitializeComponent();
            lb_UserName.Text = userName; // 전달받은 유저네임을 라벨에 표시
            using (MySqlConnection conn = DBHelper.GetConnection())
            {
              
                conn.Open();
                string query = "SELECT profile_icon FROM member WHERE user_id = @userId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", SessionManager.CurrentUserId);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int iconNumber = Convert.ToInt32(result);
                    ComponentResourceManager form4Resources = new ComponentResourceManager(typeof(Form4));

                    switch(iconNumber)
                    {
                        case 1:
                            pb_Profile.Image = (Image?)form4Resources.GetObject("bt_Icon1.BackgroundImage"); break;
                            
                        case 2:
                            pb_Profile.Image = (Image?)form4Resources.GetObject("bt_Icon2.BackgroundImage"); break;
                            
                        case 3:
                            pb_Profile.Image = (Image?)form4Resources.GetObject("bt_Icon3.BackgroundImage"); break;
                           
                        case 4:
                            pb_Profile.Image = (Image?)form4Resources.GetObject("bt_Icon4.BackgroundImage"); break;
                           
                    }
                }
                
            }
           
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

                SaveProfileIcon(form4.SelectedIconNumber); // 선택한 이미지를 저장
            }
        }
        private void SaveProfileIcon(int iconNumber)
        { 
                
            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE member SET profile_icon = @iconNumber WHERE user_id = @userId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@iconNumber", iconNumber);
                    cmd.Parameters.AddWithValue("@userId", SessionManager.CurrentUserId);
                    cmd.ExecuteNonQuery();
                }
                catch( Exception ex)
                {
                    MessageBox.Show("프로필 사진 저장 중 오류가 발생했습니다: " + ex.Message);
                }

            }
            MessageBox.Show("프로필 사진이 저장되었습니다.");
            
            
        }
        private void bt_Mixing_Click(object sender, EventArgs e)
        {

        }

        private void bt_SoundCloud_Click(object sender, EventArgs e)
        {

        }
    }
}

