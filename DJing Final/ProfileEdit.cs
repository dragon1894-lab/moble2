#nullable enable

using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace djing
{
    public partial class ProfileEdit : Form
    {
        // 로그인한 사용자 ID
        private string currentUserId = "";

        // 현재 비밀번호
        private string CurrentPW = "";

        // 선택한 프로필 이미지
        public Image? SelectedImage { get; private set; }

        // DB에 저장할 아이콘 이름
        public string? SelectedIconName { get; private set; }

        // DB 연결 문자열
        private string connStr =
            "Server=localhost;Database=djing;Uid=root;Pwd=1111;";

        public ProfileEdit(string userId)
        {
            InitializeComponent();

            currentUserId = userId;

            // 폼이 열릴 때 현재 회원정보 불러오기
            LoadMemberInfo();
        }

        // =====================================================
        // 회원정보 불러오기
        // =====================================================
        private void LoadMemberInfo()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        SELECT password, phone
                        FROM member
                        WHERE user_id = @userId;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", currentUserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CurrentPW =
                                    reader["password"]?.ToString() ?? "";

                                tb_Number.Text =
                                    reader["phone"]?.ToString() ?? "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"회원정보 조회 실패:\n{ex.Message}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // 프로필 아이콘 선택
        // =====================================================
        private void SelectProfile(
            Button profileButton,
            string iconName)
        {
            SelectedImage =
                profileButton.Image
                ?? profileButton.BackgroundImage;

            SelectedIconName = iconName;

            if (SelectedImage == null)
            {
                MessageBox.Show("선택한 아이콘 이미지가 없습니다.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        UPDATE member
                        SET icon = @iconName
                        WHERE user_id = @userId;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@iconName",
                            SelectedIconName
                        );

                        cmd.Parameters.AddWithValue(
                            "@userId",
                            currentUserId
                        );

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("프로필 아이콘이 변경되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"프로필 아이콘 변경 실패:\n{ex.Message}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void bt_Icon1_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon1, "icon1");
        }

        private void bt_Icon2_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon2, "icon2");
        }

        private void bt_Icon3_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon3, "icon3");
        }

        private void bt_Icon4_Click(object sender, EventArgs e)
        {
            SelectProfile(bt_Icon4, "icon4");
        }

        // =====================================================
        // 비밀번호 변경
        // =====================================================
        private void bt_ChangePW_Click(object sender, EventArgs e)
        {
            if (tb_CurrentPW.Text == "")
            {
                MessageBox.Show("현재 비밀번호를 입력해주세요.");
                tb_CurrentPW.Select();
                return;
            }

            if (tb_CurrentPW.Text != CurrentPW)
            {
                MessageBox.Show("현재 비밀번호가 일치하지 않습니다.");

                tb_CurrentPW.Select();
                tb_CurrentPW.SelectAll();

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
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        UPDATE member
                        SET password = @password
                        WHERE user_id = @userId;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@password",
                            tb_NewPW.Text
                        );

                        cmd.Parameters.AddWithValue(
                            "@userId",
                            currentUserId
                        );

                        cmd.ExecuteNonQuery();
                    }
                }

                // 현재 비밀번호도 새 비밀번호로 갱신
                CurrentPW = tb_NewPW.Text;

                MessageBox.Show("비밀번호가 변경되었습니다.");

                tb_CurrentPW.Text = "";
                tb_NewPW.Text = "";
                tb_NewPWCheck.Text = "";
                lb_PWresult.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"비밀번호 변경 실패:\n{ex.Message}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // 새 비밀번호 확인 실시간 검사
        // =====================================================
        private void tb_NewPWCheck_TextChanged(
            object sender,
            EventArgs e)
        {
            if (tb_NewPWCheck.Text == "")
            {
                lb_PWresult.Text = "";
            }
            else if (tb_NewPW.Text == tb_NewPWCheck.Text)
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

        // =====================================================
        // 전화번호 변경
        // =====================================================
        private void bt_ChangeNumber_Click(object sender, EventArgs e)
        {
            if (tb_Number.Text == "")
            {
                MessageBox.Show("핸드폰 번호를 입력해주세요.");
                tb_Number.Select();
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        UPDATE member
                        SET phone = @phone
                        WHERE user_id = @userId;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@phone",
                            tb_Number.Text.Trim()
                        );

                        cmd.Parameters.AddWithValue(
                            "@userId",
                            currentUserId
                        );

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("핸드폰 번호가 변경되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"핸드폰 번호 변경 실패:\n{ex.Message}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =====================================================
        // 닫기
        // =====================================================
        private void bt_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}