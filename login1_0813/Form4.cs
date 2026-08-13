using System;
using System.Drawing;
using System.Windows.Forms;

namespace login1_0811
{
    public partial class Form4 : Form
    {
        // 사용자가 고른 프로필 사진을 Form3에서 가져갈 수 있게 저장
        public Image? SelectedImage { get; private set; }

        // 사용자가 고른 기본 프로필 아이콘 번호
        public int SelectedIconNumber { get; private set; }

        public Form4()
        {
            InitializeComponent();
        }

        // 누른 버튼의 사진과 아이콘 번호를 저장하고 선택 창을 닫음
        private void SelectProfile(Button profileButton, int iconNumber)
        {
            SelectedImage = profileButton.BackgroundImage;
            SelectedIconNumber = iconNumber;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void bt_Icon1_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon1, 1);
        }

        private void bt_Icon2_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon2, 2);
        }

        private void bt_Icon3_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon3, 3);
        }

        private void bt_Icon4_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon4, 4);
        }

        private void tb_NewPWCheck_TextChanged(object sender, EventArgs e)
        {
            // 비밀번호 확인창이 비어 있으면 결과 문구를 지움
            if (tb_NewPWCheck.Text == "")
            {
                lb_PWresult.Text = "";
            }
            // 새 비밀번호와 확인 비밀번호가 같을 때
            else if (tb_NewPW.Text == tb_NewPWCheck.Text)
            {
                lb_PWresult.Text = "비밀번호가 일치합니다.";
                lb_PWresult.ForeColor = Color.Green;
            }
            // 새 비밀번호와 확인 비밀번호가 다를 때
            else
            {
                lb_PWresult.Text = "비밀번호가 일치하지 않습니다.";
                lb_PWresult.ForeColor = Color.Red;
            }
        }

        private void bt_ChangePW_Click(object sender, EventArgs e)
        {
            if (tb_CurrentPW.Text == "")
            {
                MessageBox.Show("현재 비밀번호를 입력해주세요.");
                tb_CurrentPW.Select();
                return;
            }

            if (tb_NewPW.Text == "")
            {
                MessageBox.Show("새 비밀번호를 입력해주세요.");
                tb_NewPW.Select();
                return;
            }

            if (tb_NewPWCheck.Text == "")
            {
                MessageBox.Show("새 비밀번호 확인을 입력해주세요.");
                tb_NewPWCheck.Select();
                return;
            }

            if (tb_NewPW.Text != tb_NewPWCheck.Text)
            {
                MessageBox.Show("새 비밀번호가 일치하지 않습니다.");
                tb_NewPWCheck.Select();
                tb_NewPWCheck.SelectAll();
                return;
            }

            try
            {
                // SQL은 MemberRepository가 담당하고 Form4에서는 메서드만 호출
                int result = MemberRepository.ChangePassword(
                    SessionManager.CurrentUserId,
                    tb_CurrentPW.Text,
                    tb_NewPW.Text
                );

                if (result == 1)
                {
                    MessageBox.Show("비밀번호가 변경되었습니다.");
                    tb_CurrentPW.Text = "";
                    tb_NewPW.Text = "";
                    tb_NewPWCheck.Text = "";
                    lb_PWresult.Text = "";
                }
                else
                {
                    MessageBox.Show("현재 비밀번호가 일치하지 않습니다.");
                    tb_CurrentPW.Select();
                    tb_CurrentPW.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("비밀번호 변경 중 오류가 발생했습니다.\n" + ex.Message);
            }
        }

        private void bt_ChangeNumber_Click(object sender, EventArgs e)
        {
            if (tb_Number.Text == "")
            {
                MessageBox.Show("휴대폰 번호를 입력해주세요.");
                tb_Number.Select();
                return;
            }

            try
            {
                // SQL은 MemberRepository가 담당하고 Form4에서는 메서드만 호출
                int result = MemberRepository.ChangePhone(
                    SessionManager.CurrentUserId,
                    tb_Number.Text
                );

                if (result == 1)
                {
                    MessageBox.Show("휴대폰 번호가 변경되었습니다.");
                    tb_Number.Text = "";
                }
                else
                {
                    MessageBox.Show("휴대폰 번호를 변경하지 못했습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("휴대폰 번호 변경 중 오류가 발생했습니다.\n" + ex.Message);
            }
        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
