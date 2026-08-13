using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DJing
{
    public partial class Upload : Form
    {
        private readonly string[] allowedAudioExtensions = { ".mp3", ".wav" };
        private readonly string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".bmp" };

        private string selectedImagePath = "";
        private string selectedAudioPath = "";

        private string dbHost = "127.0.0.1";
        private string dbPort = "3306";
        private string dbUser = "root";
        private string dbPass = "1111";
        private string dbName = "djing";

        public Upload()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "이미지 파일 (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = ofd.FileName;
                    picAlbum.ImageLocation = ofd.FileName;
                }
            }
        }

        private void btnFindMusic_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "음원 파일 (*.mp3;*.wav)|*.mp3;*.wav";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedAudioPath = ofd.FileName;
                    tbSongFile.Text = ofd.FileName;
                }
            }
        }

        private bool ValidateUploadedFiles(string audioPath, string imagePath)
        {
            if (string.IsNullOrWhiteSpace(audioPath) || !File.Exists(audioPath))
            {
                MessageBox.Show("올바른 음원 파일을 선택해주세요.");
                return false;
            }

            string audioExt = Path.GetExtension(audioPath).ToLower();

            if (!allowedAudioExtensions.Contains(audioExt))
            {
                MessageBox.Show("지원하지 않는 음원 형식입니다.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(imagePath))
            {
                if (!File.Exists(imagePath))
                {
                    MessageBox.Show("선택한 이미지 파일이 존재하지 않습니다.");
                    return false;
                }

                string imageExt = Path.GetExtension(imagePath).ToLower();

                if (!allowedImageExtensions.Contains(imageExt))
                {
                    MessageBox.Show("지원하지 않는 이미지 형식입니다.");
                    return false;
                }
            }

            return true;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbTitle.Text))
            {
                MessageBox.Show("곡 제목을 입력해주세요.");
                return;
            }

            if (!ValidateUploadedFiles(selectedAudioPath, selectedImagePath))
                return;

            string musicFolder = Path.Combine(Application.StartupPath, "music");
            string albumFolder = Path.Combine(Application.StartupPath, "album");

            Directory.CreateDirectory(musicFolder);
            Directory.CreateDirectory(albumFolder);

            string audioFileName = Path.GetFileName(selectedAudioPath);

            string savedAudioPath =
                Path.Combine(musicFolder, audioFileName);

            File.Copy(selectedAudioPath, savedAudioPath, true);

            string dbAudioPath =
                Path.Combine("music", audioFileName);

            string dbImagePath = "";

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                string imageFileName =
                    Path.GetFileName(selectedImagePath);

                string savedImagePath =
                    Path.Combine(albumFolder, imageFileName);

                File.Copy(selectedImagePath, savedImagePath, true);

                dbImagePath =
                    Path.Combine("album", imageFileName);
            }

            string connectionString =
                $"Server={dbHost};Port={dbPort};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO songs
                        (user_id, Title, Artist, FilePath, ImagePath, ReleaseDate)
                        VALUES
                        (@user_id, @Title, @Artist, @FilePath, @ImagePath, @ReleaseDate);
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@user_id", UserSession.UserId);
                        cmd.Parameters.AddWithValue("@Title", tbTitle.Text.Trim());
                        cmd.Parameters.AddWithValue(
                            "@Artist",
                            string.IsNullOrWhiteSpace(tbArtist.Text)
                                ? "Unknown"
                                : tbArtist.Text.Trim()
                        );

                        cmd.Parameters.AddWithValue("@FilePath", dbAudioPath);
                        cmd.Parameters.AddWithValue("@ImagePath", dbImagePath);
                        cmd.Parameters.AddWithValue(
                            "@ReleaseDate",
                            DateTime.Now.ToString("yyyy-MM-dd")
                        );

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("성공적으로 등록되었습니다!");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"업로드 실패:\n{ex.Message}");
            }
        }
    }
}