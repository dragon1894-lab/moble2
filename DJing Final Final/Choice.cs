using djing;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DJing
{
    public partial class Choice : Form
    {
        private string currentUserId; // 로그인한 유저 ID
        private string connStr = "Server=localhost;Database=djing;Uid=root;Pwd=1111;";
        private DJform? dJform;
        private soundcloud? soundkloud;
        private bool isNavigating = false;

        public Choice(string userName)
        {
            InitializeComponent();
            this.currentUserId = userName;
            lb_UserName.Text = userName;

            // 폼 열릴 때 DB에 저장된 아이콘 정보 불러오기
            LoadUserProfileIcon();
            this.FormClosed += Choice_FormClosed;
        }

        // 1. DB에서 member 테이블의 profile_icon 컬럼 값 불러와 표시
        private void LoadUserProfileIcon()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    // profile_icon -> icon 으로 변경
                    string query = "SELECT icon FROM member WHERE user_id = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", currentUserId);
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string iconName = result.ToString() ?? "";
                            pb_Profile.Image = GetIconImageFromName(iconName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"프로필 이미지 조회 실패: {ex.Message}");
            }
        }

        // 2. 아이콘 변경 버튼 클릭 시 Icon 폼 열기
        private void bt_Icon_Click(object sender, EventArgs e)
        {
            ProfileEdit profileEdit =
                new ProfileEdit(UserSession.UserId);

            profileEdit.StartPosition = FormStartPosition.Manual;

            profileEdit.Location = new Point(
                this.Left + (this.Width - profileEdit.Width) / 2,
                this.Top + (this.Height - profileEdit.Height) / this.Top - 150
            );

            profileEdit.ShowDialog();

            // ProfileEdit에서 변경된 아이콘을 DB에서 다시 읽어옴
            LoadUserProfileIcon();
        }

        // 3. DB에 profile_icon 저장 (UPDATE)
        private void UpdateProfileIconToDB(string? iconName)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    // profile_icon -> icon 으로 변경
                    string query = "UPDATE member SET icon = @iconName WHERE user_id = @userId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@iconName", iconName);
                        cmd.Parameters.AddWithValue("@userId", currentUserId);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("프로필 아이콘이 저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ★ DB의 아이콘 이름("icon1", "icon2" 등)에 맞춰 Icon 폼 버튼에 세팅되어 있는 이미지 추출
        private Image? GetIconImageFromName(string iconName)
        {
            using (ProfileEdit tempForm = new ProfileEdit(currentUserId))
            {
                Button? targetButton = null;

                switch (iconName)
                {
                    case "icon1":
                        targetButton = tempForm.Controls["bt_Icon1"] as Button;
                        break;

                    case "icon2":
                        targetButton = tempForm.Controls["bt_Icon2"] as Button;
                        break;

                    case "icon3":
                        targetButton = tempForm.Controls["bt_Icon3"] as Button;
                        break;

                    case "icon4":
                        targetButton = tempForm.Controls["bt_Icon4"] as Button;
                        break;
                }

                if (targetButton != null)
                {
                    return targetButton.Image ?? targetButton.BackgroundImage;
                }
            }

            return null;
        }

        private void Choice_FormClosed(object? sender, FormClosedEventArgs e)
        {
            // 일반 종료(X 버튼)일 때만 프로그램 전체를 종료
            if (!isNavigating)
            {
                Environment.Exit(0);
            }
        }

        // 로그아웃 과정에서 Choice가 닫힐 때 전체 프로그램이 종료되지 않도록 설정
        public void PrepareForNavigation()
        {
            isNavigating = true;
        }

        private void bt_Logout_Click(object sender, EventArgs e)
        {
            // 로그인 정보를 지우고 기존 로그인 창으로 돌아감
            UserSession.UserId = "";
            UserSession.IsGuest = false;

            Login? loginForm = Application.OpenForms
                .OfType<Login>()
                .FirstOrDefault();

            if (loginForm == null || loginForm.IsDisposed)
            {
                loginForm = new Login();
            }

            PrepareForNavigation();
            loginForm.Show();
            loginForm.BringToFront();
            loginForm.Activate();
            Close();
        }

        private void bt_Mixing_Click(object sender, EventArgs e)
        {
            if (dJform == null || dJform.IsDisposed)
            {
                dJform = new DJform();
            }

            if (Application.OpenForms.OfType<DJform>().Any())
                MessageBox.Show("이미 Mixing이 열려있습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                dJform.StartPosition = FormStartPosition.Manual;
                dJform.Location = new Point(
                    this.Left + (this.Width - dJform.Width) / 2,
                    this.Top + (this.Height - dJform.Height) / this.Top);

                dJform.Owner = this;
                dJform.Show();
                dJform.BringToFront();
            }
        }
        private void bt_SoundCloud_Click(object sender, EventArgs e)
        {
            if (soundkloud == null || soundkloud.IsDisposed)
            {
                soundkloud = new soundcloud();
            }

            if (Application.OpenForms.OfType<soundcloud>().Any())
                MessageBox.Show("이미 Sound Kloud가 열려있습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {

                soundkloud.StartPosition = FormStartPosition.Manual;
                soundkloud.Location = new Point(
                this.Left + (this.Width - soundkloud.Width) / 2,
                this.Top + (this.Height - soundkloud.Height) / this.Top);

                soundkloud.Owner = this;
                soundkloud.Show();
            }
        }
    }
}

