using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL 라이브러리 추가

namespace DJing
{
    public partial class SignUp : Form
    {
        public string CreatedID = ""; // Form1에서 가져갈 수 있게 회원가입한 아이디 저장
        public string CreatedPW = ""; // 회원가입한 비밀번호 저장

        bool IDChecked = false; // 아이디 중복확인 여부

        // MySQL DB 연결 문자열 (실제 비밀번호로 수정하세요)
        private string connStr = "Server=localhost;Database=djing;Uid=root;Pwd=1111;";

        public SignUp()
        {
            InitializeComponent();
        }

        // 1. 아이디 중복확인 버튼 클릭
        private void bt_CheckID_Click(object sender, EventArgs e)
        {
            string userId = tb_ID.Text.Trim();

            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("아이디를 입력해주세요.");
                tb_ID.Select();
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // member 테이블에서 user_id 중복 여부 확인
                    string query = "SELECT COUNT(*) FROM member WHERE user_id = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("이미 사용 중인 아이디입니다.");
                            IDChecked = false;
                            tb_ID.SelectAll();
                        }
                        else
                        {
                            MessageBox.Show("사용 가능한 유저네임입니다.");
                            IDChecked = true; // 중복확인 성공
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB 연결 오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. 아이디 텍스트박스 내용이 변경되면 중복확인을 다시 받아야 함
        private void tb_ID_TextChanged(object sender, EventArgs e)
        {
            IDChecked = false;
        }

        // 3. 회원가입 버튼 클릭 (Continue)
        private void bt_Continue_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_Name.Text.Trim()))
            {
                MessageBox.Show("이름을 입력해주세요.");
                tb_Name.Select();
                return;
            }

            if (string.IsNullOrEmpty(tb_Number.Text.Trim()))
            {
                MessageBox.Show("번호를 입력해주세요.");
                tb_Number.Select();
                return;
            }

            if (string.IsNullOrEmpty(tb_ID.Text.Trim()))
            {
                MessageBox.Show("아이디를 입력해주세요.");
                tb_ID.Select();
                return;
            }

            if (!IDChecked)
            {
                MessageBox.Show("아이디 중복확인을 해주세요.");
                bt_CheckID.Select();
                return;
            }

            if (string.IsNullOrEmpty(tb_PW.Text))
            {
                MessageBox.Show("비밀번호를 입력해주세요.");
                tb_PW.Select();
                return;
            }

            if (string.IsNullOrEmpty(tb_PWCheck.Text))
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

            if (!rb_Yes.Checked)
            {
                MessageBox.Show("개인정보 수집에 동의해주세요.");
                return;
            }

            // DB에 회원정보 저장 (INSERT)
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // member 테이블 속성 구조에 맞춘 INSERT 쿼리 (join_date는 NOW()로 현재 시간 저장)
                    string insertQuery = @"INSERT INTO member (user_id, password, name, phone, icon, join_date) 
                                           VALUES (@userId, @password, @name, @phone, @icon, NOW())";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", tb_ID.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", tb_PW.Text);
                        cmd.Parameters.AddWithValue("@name", tb_Name.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", tb_Number.Text.Trim());
                        cmd.Parameters.AddWithValue("@icon", "icon1");

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            CreatedID = tb_ID.Text.Trim();
                            CreatedPW = tb_PW.Text;

                            MessageBox.Show("회원가입이 완료되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.DialogResult = DialogResult.OK;
                            Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"회원가입 처리 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 4. 비밀번호 일치 실시간 확인
        private void tb_PWCheck_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_PWCheck.Text))
            {
                lb_PWresult.Text = "";
            }
            else if (tb_PW.Text == tb_PWCheck.Text)
            {
                lb_PWresult.Text = "비밀번호가 일치합니다.";
                lb_PWresult.ForeColor = Color.Green;
            }
            else
            {
                lb_PWresult.Text = "비밀번호가 일치하지 않습니다.";
                lb_PWresult.ForeColor = Color.Red;
            }
        }
    }
}