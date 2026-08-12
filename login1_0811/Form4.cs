using System;
using System.Drawing;
using System.Windows.Forms;

namespace login1_0811
{
    public partial class Form4 : Form
    {
        // 사용자가 선택한 프로필 사진을 저장하는 변수
        // Form3에서 선택한 사진을 가져갈 수 있도록 public으로 설정
        public Image SelectedImage { get; private set; }

        string CurrentPW = ""; // Form3에서 전달받은 현재 비밀번호 저장
        public string ChangedPW = ""; // 변경한 새 비밀번호를 Form3에 전달

        // Form3에서 현재 비밀번호를 받아 Form4를 생성
        public Form4(string password)
        {
            InitializeComponent();
            CurrentPW = password; // 전달받은 비밀번호를 현재 비밀번호로 저장
        }

        // 클릭한 버튼의 사진을 저장하고 Form4를 닫음
        private void SelectProfile(Button profileButton)
        {
            // 클릭한 버튼의 배경 사진을 SelectedImage에 저장
            SelectedImage = profileButton.BackgroundImage;

            // 프로필 사진 선택이 완료됐다는 것을 Form3에 알려줌
            DialogResult = DialogResult.OK;

            Close();
        }

        // 첫 번째 프로필 사진 선택
        private void bt_Icon1_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon1);
        }

        private void bt_Icon2_Click(object sender, EventArgs e)
        {
            // 두 번째 프로필 사진 선택
            SelectProfile(bt_Icon2);
        }

        private void bt_Icon3_Click(object sender, EventArgs e)
        {
            // 세 번째 프로필 사진 선택
            SelectProfile(bt_Icon3);
        }

        private void bt_Icon4_Click(object sender, EventArgs e)
        {
            // 네 번째 프로필 사진 선택
            SelectProfile(bt_Icon4);
        }

        // Change Password 버튼을 눌렀을 때 실행
        private void bt_ChangePW_Click(object sender, EventArgs e)
        {
            // 현재 비밀번호를 입력하지 않았을 때
            if (tb_CurrentPW.Text == "")
            {
                MessageBox.Show("현재 비밀번호를 입력해주세요.");
                tb_CurrentPW.Select();
                return;
            }

            // 입력한 현재 비밀번호가 회원정보의 비밀번호와 다를 때
            if (tb_CurrentPW.Text != CurrentPW)
            {
                MessageBox.Show("현재 비밀번호가 일치하지 않습니다.");
                tb_CurrentPW.Select();
                tb_CurrentPW.SelectAll();
                return;
            }

            // 새 비밀번호를 입력하지 않았을 때
            if (tb_NewPW.Text == "")
            {
                MessageBox.Show("새 비밀번호를 입력해주세요.");
                tb_NewPW.Select();
                return;
            }

            // 새 비밀번호 확인을 입력하지 않았을 때
            if (tb_NewPWCheck.Text == "")
            {
                MessageBox.Show("새 비밀번호 확인을 입력해주세요.");
                tb_NewPWCheck.Select();
                return;
            }

            // 새 비밀번호와 새 비밀번호 확인이 서로 다를 때
            if (tb_NewPW.Text != tb_NewPWCheck.Text)
            {
                MessageBox.Show("새 비밀번호가 일치하지 않습니다.");
                tb_NewPWCheck.Select();
                tb_NewPWCheck.SelectAll();
                return;
            }

            ChangedPW = tb_NewPW.Text; // 새 비밀번호를 변경된 비밀번호에 저장
            CurrentPW = ChangedPW; // 현재 비밀번호도 새 비밀번호로 변경
            MessageBox.Show("비밀번호가 변경되었습니다.");

            // 비밀번호 변경 후 입력창과 확인 문구 비우기
            tb_CurrentPW.Text = "";
            tb_NewPW.Text = "";
            tb_NewPWCheck.Text = "";
        }

        // 새 비밀번호 확인창의 내용이 바뀔 때마다 실행
        private void tb_NewPWCheck_TextChanged(object sender, EventArgs e)
        {
            // 새 비밀번호 확인창이 비어 있으면 안내 문구 지움
            if (tb_NewPWCheck.Text == "")
            {
                lb_PWresult.Text = "";
            }
            // 새 비밀번호와 새 비밀번호 확인이 같을 때
            else if (tb_NewPW.Text == tb_NewPWCheck.Text)
            {
                lb_PWresult.Text = "비밀번호가 일치합니다.";
                lb_PWresult.ForeColor = Color.Green; // 글자색을 초록색으로 변경
            }
            // 새 비밀번호와 새 비밀번호 확인이 다를 때
            else
            {
                lb_PWresult.Text = "비밀번호가 일치하지 않습니다.";
                lb_PWresult.ForeColor = Color.Red; // 글자색을 빨간색으로 변경
            }
        }

        // Change Phone Number 버튼을 눌렀을 때 실행
        private void bt_ChangeNumber_Click(object sender, EventArgs e)
        {
            if (tb_Number.Text == "")
            {
                // 핸드폰 번호를 입력하지 않았으면 입력창으로 이동
                MessageBox.Show("핸드폰 번호를 입력해주세요.");
                tb_Number.Select();
                return;
            }
            // 데이터베이스 연결 전까지 변경 완료 메시지만 표시
            MessageBox.Show("핸드폰 번호가 변경되었습니다.");
        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
            // 회원정보 수정 창 닫기
            Close();
        }
    }
}


