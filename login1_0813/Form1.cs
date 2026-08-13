using MySql.Data.MySqlClient;
namespace login1_0811
{
    public partial class Form1 : Form
    {
         string MyID = "";
         string MyPW = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void bt_Login_Click(object sender, EventArgs e)
        {
            if(tb_ID.Text == "")
            {
                MessageBox.Show("아이디를 입력해주세요.");
                tb_ID.Select(); //Select 때매 id입력을 안할 시 id 입력창으로 이동
                return;
            }

            if (tb_PW.Text == "")
            {
                MessageBox.Show("비밀번호를 입력해주세요.");
                tb_PW.Select();
                return;
            }
            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM member WHERE user_id = @userId AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userId", tb_ID.Text);
                    cmd.Parameters.AddWithValue("@password", tb_PW.Text);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        SessionManager.CurrentUserId= reader["user_id"].ToString();
                        SessionManager.CurrentUserName = reader["name"].ToString();

                        reader.Close();
                        // 로그인 성공
                        Form3 form3 = new Form3(tb_ID.Text);
                        form3.Show();
                        this.Hide();
                    }
                    else
                    {
                        // 로그인 실패
                        MessageBox.Show("아이디 또는 비밀번호가 일치하지 않습니다.");
                        tb_PW.Select();
                        tb_PW.SelectAll(); // 비밀번호 텍스트 전체 선택
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("로그인 중 오류: " + ex.Message);
                }
            }
           
        }

        private void bt_Create_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog(); // Form2가 닫힐 때까지 기다림

            if (form2.CreatedID != "")
            {
                MyID = form2.CreatedID;
                MyPW = form2.CreatedPW;
            }
        }

        private void bt_Guest_Click(object sender, EventArgs e)
        {

        }
    }
}

