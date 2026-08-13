using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace login1_0811
{
    public partial class Form2 : Form
    {
        public string CreatedID = ""; // Form1에서 가져갈 수 있게 회원가입한 아이디 저장
        public string CreatedPW = ""; // 회원가입한 비밀번호 저장

        bool IDChecked = false; // 아이디 중복확인 여부

        public Form2()
        {
            InitializeComponent();
        }

        private void bt_CheckID_Click(object sender, EventArgs e)
        {
            if (tb_ID.Text == "")
            {
                MessageBox.Show("아이디를 입력해주세요");
                tb_ID.Select();
                return;
            }
            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM member WHERE user_id = @userId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId", tb_ID.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if(count > 0)
                    {
                        MessageBox.Show("이미 사용 중인 아이디입니다.");
                        tb_ID.Select();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("사용 가능한 유저네임입니다.");
                        IDChecked = true;  // 아이디 중복확인 완료
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("중복확인 중 오류: " + ex.Message);
                }
            }
           

            //{ } 데이터 베이스 완성 후 아이디 중복 조회 코드 작성
        }

        private void bt_Continue_Click(object sender, EventArgs e)
        {
            if (tb_Name.Text == "")
            {
                MessageBox.Show("이름을 입력해주세요");
                tb_Name.Select(); //회원가입창에서 이름을 입력안하면 이름창으로 가진다
                return;
            }

            if (tb_Number.Text == "")
            {
                MessageBox.Show("번호를 입력해주세요.");
                tb_Number.Select();
                return;
            }

            if (tb_ID.Text == "")
            {
                MessageBox.Show("아이디를 입력해주세요.");
                tb_ID.Select();
                return;
            }

            if (IDChecked == false)
            {
                MessageBox.Show("아이디 중복확인을 해주세요.");
                bt_CheckID.Select();
                return;
            }

            if (tb_PW.Text == "")
            {
                MessageBox.Show("비밀번호를 입력해주세요.");
                tb_PW.Select();
                return;
            }

            if (tb_PWCheck.Text == "")
            {
                MessageBox.Show("비밀번호 확인을 입력해주세요.");
                tb_PWCheck.Select();
                return;
            }

            if (tb_PW.Text != tb_PWCheck.Text)
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.");
                tb_PWCheck.Select();
                return;
            }

            if (rb_Yes.Checked == false)
            {
                MessageBox.Show("개인정보 수집에 동의해주세요.");
                return;
            }
            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO member (user_id,password, name,phone) VALUES (@userId, @password, @name, @phone)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId", tb_ID.Text);
                    cmd.Parameters.AddWithValue("@password", tb_PW.Text);
                    cmd.Parameters.AddWithValue("@name", tb_Name.Text);
                    cmd.Parameters.AddWithValue("@phone", tb_Number.Text);
                    cmd.ExecuteNonQuery();

                    CreatedID = tb_ID.Text; // 입력한 아이디를 CreatedID에 저장
                    CreatedPW = tb_PW.Text; // 입력한 아이디를 CreatedID에 저장


                    MessageBox.Show("회원가입이 완료되었습니다.");

                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("회원가입 중 오류: " + ex.Message);
                }
            }
           

        }

        private void tb_PWCheck_TextChanged(object sender, EventArgs e)
        {
            //비밀번호 확인창이 비어 있으면 안내 문구 지움
            if (tb_PWCheck.Text == "")
            {
                lb_PWresult.Text = "";
            }
            // 비밀번호랑 비밀번호 확인 내용이 같을 때
            else if (tb_PW.Text == tb_PWCheck.Text)
            {
                lb_PWresult.Text = "비밀번호가 일치합니다.";
                lb_PWresult.ForeColor = Color.Green; // 글자색을 초록색으로 변경
            }
            // 비번이 다를 때
            else
            {
                lb_PWresult.Text = "비밀번호가 일치하지 않습니다";
                lb_PWresult.ForeColor = Color.Red; // 글자색을 빨간색으로 변경
            }
        }
    }
}

