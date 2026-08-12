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

            if(tb_ID.Text == MyID && tb_PW.Text == MyPW) // 입력한 아이디와 비밀번호가 회원가입 정보와 일치하면
            {
                Form3 form3 = new Form3(MyID, MyPW);
                this.Hide();
                form3.ShowDialog();

                // Form3에서 비밀번호를 변경했으면 새 비밀번호를 저장
                if (form3.ChangedPW != "")
                {
                    MyPW = form3.ChangedPW;
                }

                this.Show();
            }

            else
            {
                MessageBox.Show("아이디 또는 비밀번호가 일치하지 않습니다.");
                tb_PW.Select();
                tb_PW.SelectAll(); // 비밀번호 텍스트 전체 선택
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

