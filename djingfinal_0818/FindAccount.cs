using MySql.Data.MySqlClient;

namespace DJing
{
    // 아이디 찾기와 비밀번호 재설정 기능을 담당하는 폼
    public partial class FindAccount : Form
    {
        // MySQL 연결 정보
        // Server: DB 서버 주소, Database: 사용할 DB, Uid/Pwd: MySQL 계정 정보
        private string connStr = "Server=localhost;Database=djing;Uid=root;Pwd=1111;";

        // FindAccount 폼이 생성될 때 가장 먼저 실행되는 생성자
        public FindAccount()
        {
            // Designer.cs에 작성된 TextBox, Button 등의 화면을 생성함
            InitializeComponent();
        }

        // 아이디 찾기 버튼을 눌렀을 때 실행
        private void bt_FindID_Click(object sender, EventArgs e)
        {
            // 이름 입력창이 비어 있으면 메시지를 띄우고 입력창으로 이동
            if (tb_FindName.Text == "")
            {
                MessageBox.Show("이름을 입력해주세요.");
                tb_FindName.Select();
                return;
            }

            // 전화번호 입력창이 비어 있으면 메시지를 띄우고 입력창으로 이동
            if (tb_FindPhone.Text == "")
            {
                MessageBox.Show("전화번호를 입력해주세요.");
                tb_FindPhone.Select();
                return;
            }

            try
            {
                // MySQL 연결 객체 생성
                // using을 사용하면 작업이 끝난 뒤 연결이 자동으로 정리됨
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    // 데이터베이스 연결 열기
                    conn.Open();

                    // 이름과 전화번호가 모두 일치하는 회원의 아이디 한 개를 조회
                    string query = @"SELECT user_id
                                     FROM member
                                     WHERE name = @name AND phone = @phone
                                     LIMIT 1";

                    // 위 SQL을 현재 DB 연결에서 실행할 명령으로 생성
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // SQL의 @name과 @phone에 사용자가 입력한 값을 넣음
                    // 문자열을 직접 합치지 않아 SQL Injection을 방지할 수 있음
                    cmd.Parameters.AddWithValue("@name", tb_FindName.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", tb_FindPhone.Text.Trim());

                    // ExecuteScalar는 조회 결과의 첫 번째 행, 첫 번째 값만 가져옴
                    // 회원이 없을 수 있으므로 null을 저장할 수 있는 object?를 사용함
                    object? result = cmd.ExecuteScalar();

                    // 조회된 회원이 있으면 찾은 아이디를 화면에 표시
                    if (result != null)
                    {
                        lb_IDResult.Text = "아이디: " + result.ToString();
                        lb_IDResult.ForeColor = Color.FromArgb(255, 85, 0);
                    }
                    else
                    {
                        // 이름과 전화번호가 일치하는 회원이 없을 때 표시
                        lb_IDResult.Text = "일치하는 회원정보가 없습니다.";
                        lb_IDResult.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                // DB 연결 또는 SQL 실행 중 오류가 발생하면 오류 내용을 표시
                MessageBox.Show("아이디 찾기 중 오류가 발생했습니다.\n" + ex.Message);
            }
        }

        // 비밀번호 변경 버튼을 눌렀을 때 실행
        private void bt_ResetPW_Click(object sender, EventArgs e)
        {
            // 아이디 입력 여부 확인
            if (tb_ResetID.Text == "")
            {
                MessageBox.Show("아이디를 입력해주세요.");
                tb_ResetID.Select();
                return;
            }

            // 이름 입력 여부 확인
            if (tb_ResetName.Text == "")
            {
                MessageBox.Show("이름을 입력해주세요.");
                tb_ResetName.Select();
                return;
            }

            // 전화번호 입력 여부 확인
            if (tb_ResetPhone.Text == "")
            {
                MessageBox.Show("전화번호를 입력해주세요.");
                tb_ResetPhone.Select();
                return;
            }

            // 새 비밀번호 입력 여부 확인
            if (tb_NewPW.Text == "")
            {
                MessageBox.Show("새 비밀번호를 입력해주세요.");
                tb_NewPW.Select();
                return;
            }

            // 새 비밀번호 확인 입력 여부 확인
            if (tb_NewPWCheck.Text == "")
            {
                MessageBox.Show("새 비밀번호 확인을 입력해주세요.");
                tb_NewPWCheck.Select();
                return;
            }

            // 새 비밀번호와 비밀번호 확인이 서로 같은지 검사
            if (tb_NewPW.Text != tb_NewPWCheck.Text)
            {
                MessageBox.Show("새 비밀번호가 일치하지 않습니다.");
                tb_NewPWCheck.Select();
                tb_NewPWCheck.SelectAll();
                return;
            }

            try
            {
                // MySQL 연결 객체를 만들고 DB 연결 열기
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // 아이디, 이름, 전화번호가 모두 일치하는 회원의 비밀번호 변경
                    // 기존 비밀번호를 보여주지 않고 새 비밀번호로 바꾸는 방식임
                    string query = @"UPDATE member
                                     SET password = @newPassword
                                     WHERE user_id = @userId
                                     AND name = @name
                                     AND phone = @phone";

                    // SQL 명령 생성
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // SQL에 필요한 새 비밀번호와 회원 확인 정보를 넣음
                    cmd.Parameters.AddWithValue("@newPassword", tb_NewPW.Text);
                    cmd.Parameters.AddWithValue("@userId", tb_ResetID.Text.Trim());
                    cmd.Parameters.AddWithValue("@name", tb_ResetName.Text.Trim());
                    cmd.Parameters.AddWithValue("@phone", tb_ResetPhone.Text.Trim());

                    // UPDATE 실행 후 실제로 변경된 회원 수를 result에 저장
                    int result = cmd.ExecuteNonQuery();

                    // 한 명의 정보가 변경됐으면 비밀번호 변경 성공
                    if (result == 1)
                    {
                        MessageBox.Show("비밀번호가 변경되었습니다.");

                        // 변경 완료 후 사용자가 입력한 내용을 모두 지움
                        tb_ResetID.Text = "";
                        tb_ResetName.Text = "";
                        tb_ResetPhone.Text = "";
                        tb_NewPW.Text = "";
                        tb_NewPWCheck.Text = "";
                        lb_PWResult.Text = "";
                    }
                    else
                    {
                        // 입력한 아이디, 이름, 전화번호와 일치하는 회원이 없을 때
                        MessageBox.Show("일치하는 회원정보가 없습니다.");
                    }
                }
            }
            catch (Exception ex)
            {
                // DB 연결 또는 UPDATE 실행 중 오류가 발생하면 오류 내용을 표시
                MessageBox.Show("비밀번호 변경 중 오류가 발생했습니다.\n" + ex.Message);
            }
        }

        // 새 비밀번호 확인창의 내용이 바뀔 때마다 자동으로 실행
        private void tb_NewPWCheck_TextChanged(object sender, EventArgs e)
        {
            // 비밀번호 확인창이 비어 있으면 결과 문구도 지움
            if (tb_NewPWCheck.Text == "")
            {
                lb_PWResult.Text = "";
            }
            // 새 비밀번호와 확인 비밀번호가 같으면 초록색 문구 표시
            else if (tb_NewPW.Text == tb_NewPWCheck.Text)
            {
                lb_PWResult.Text = "비밀번호가 일치합니다.";
                lb_PWResult.ForeColor = Color.Green;
            }
            else
            {
                // 두 비밀번호가 다르면 빨간색 문구 표시
                lb_PWResult.Text = "비밀번호가 일치하지 않습니다.";
                lb_PWResult.ForeColor = Color.Red;
            }
        }
    }
}
