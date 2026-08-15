using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // MySQL 라이브러리 추가

namespace DJing
{
    public partial class Login : Form
    {
        // MySQL DB 연결 문자열 (실제 비밀번호로 수정하세요)
        private string connStr = "Server=localhost;Database=djing;Uid=root;Pwd=1111;";

        public Login()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        // 1. 로그인 버튼 클릭
        private void bt_Login_Click(object sender, EventArgs e)
        {
            string userId = tb_ID.Text.Trim();
            string password = tb_PW.Text;

            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("아이디를 입력해주세요.");
                tb_ID.Select();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("비밀번호를 입력해주세요.");
                tb_PW.Select();
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // member 테이블에서 user_id와 password가 모두 일치하는지 확인
                    string query = "SELECT COUNT(*) FROM member WHERE user_id = @userId AND password = @password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@password", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // 일치하는 회원이 존재하는 경우
                        if (count > 0)
                        {
                            UserSession.UserId = tb_ID.Text.Trim();
                            MessageBox.Show($"{userId}님 환영합니다!", "로그인 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Choice 폼으로 로그인한 유저 ID를 전달하며 이동
                            if (UserSession.UserId == "admin")
                            {
                                soundcloud soundkloud = new soundcloud();

                                soundkloud.StartPosition = FormStartPosition.Manual;
                                soundkloud.Location = new Point(
                                    this.Left + (this.Width - soundkloud.Width) / 2,
                                    this.Top + (this.Height - soundkloud.Height) / this.Top
                                );

                                this.Hide();
                                soundkloud.Show();
                            }
                            else
                            {
                                Choice choice = new Choice(userId);

                                choice.StartPosition = FormStartPosition.Manual;
                                choice.Location = new Point(
                                    this.Left + (this.Width - choice.Width) / 2,
                                    this.Top + (this.Height - choice.Height) / this.Top
                                );

                                choice.Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("아이디 또는 비밀번호가 일치하지 않습니다.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tb_PW.Select();
                            tb_PW.SelectAll();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB 연결 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 2. 회원가입 버튼 클릭 (Create Account)
        private void bt_Create_Click(object sender, EventArgs e)
        {
            SignUp singup = new SignUp();

            singup.StartPosition = FormStartPosition.Manual;
            singup.Location = new Point(
                this.Left + (this.Width - singup.Width) / 2,
                this.Top + (this.Height - singup.Height) / this.Top
            );

            // 회원가입 폼이 정상 종료(DialogResult.OK)되었을 때
            if (singup.ShowDialog() == DialogResult.OK)
            {
                // 가입 성공 시 새로 만든 아이디를 로그인 아이디 텍스트박스에 자동 입력
                if (!string.IsNullOrEmpty(singup.CreatedID))
                {
                    tb_ID.Text = singup.CreatedID;
                    tb_PW.Text = ""; // 비밀번호는 직접 입력하도록 비움
                    tb_PW.Select();  // 커서를 비밀번호 입력창으로 이동
                }
            }
        }

        // 3. 게스트 로그인 버튼 클릭
        private void bt_Guest_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query =
                        "SELECT user_id FROM member WHERE user_id = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", "Guest");

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            UserSession.UserId = result.ToString();
                            UserSession.IsGuest = true;

                            soundcloud soundkloud = new soundcloud();

                            soundkloud.StartPosition = FormStartPosition.Manual;
                            soundkloud.Location = new Point(
                                this.Left + (this.Width - soundkloud.Width) / 2,
                                this.Top + (this.Height - soundkloud.Height) / this.Top
                            );

                            this.Hide();
                            soundkloud.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"게스트 접속 실패: {ex.Message}");
            }
        }
    }
}