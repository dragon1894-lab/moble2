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

        private int editingSongId = -1;
        private bool isEditMode = false;

        public Upload()
        {
            InitializeComponent();
        }
        public Upload(int songId)
        {
            InitializeComponent();

            editingSongId = songId;
            isEditMode = true;

            btnUpload.Text = "수정하기";

            LoadSongData(songId);
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
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");

            // 1. 제목 검사
            if (string.IsNullOrWhiteSpace(tbTitle.Text))
            {
                MessageBox.Show(
                    "곡 제목을 입력해주세요.",
                    "알림",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            // 2. 파일 검사
            if (!ValidateUploadedFiles(selectedAudioPath, selectedImagePath))
            {
                return;
            }

            // 3. 경로 정리
            string safeAudioPath =
                selectedAudioPath.Replace(@"\", "/");

            string safeImagePath =
                string.IsNullOrEmpty(selectedImagePath)
                ? ""
                : selectedImagePath.Replace(@"\", "/");

            string connectionString =
                $"Server={dbHost};Port={dbPort};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            try
            {
                using (MySqlConnection conn =
                    new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // =========================
                    // 수정 모드
                    // =========================
                    if (isEditMode)
                    {
                        string sql = @"
                    UPDATE songs
                    SET Title = @Title,
                        Artist = @Artist,
                        FilePath = @FilePath,
                        ImagePath = @ImagePath
                    WHERE SongId = @SongId;";

                        using (MySqlCommand cmd =
                            new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue(
                                "@Title",
                                tbTitle.Text.Trim()
                            );

                            cmd.Parameters.AddWithValue(
                                "@Artist",
                                string.IsNullOrWhiteSpace(tbArtist.Text)
                                ? "Unknown"
                                : tbArtist.Text.Trim()
                            );

                            cmd.Parameters.AddWithValue(
                                "@FilePath",
                                safeAudioPath
                            );

                            cmd.Parameters.AddWithValue(
                                "@ImagePath",
                                safeImagePath
                            );

                            cmd.Parameters.AddWithValue(
                                "@SongId",
                                editingSongId
                            );

                            int rows = cmd.ExecuteNonQuery();

                            if (rows > 0)
                            {
                                MessageBox.Show(
                                    "곡 정보가 수정되었습니다.",
                                    "수정 완료",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information
                                );
                            }
                            else
                            {
                                MessageBox.Show(
                                    "수정할 곡을 찾지 못했습니다."
                                );

                                return;
                            }
                        }
                    }

                    // =========================
                    // 새 업로드 모드
                    // =========================
                    else
                    {
                        string sql = @"
                    INSERT INTO songs
                    (
                        user_id,
                        Title,
                        Artist,
                        FilePath,
                        ImagePath,
                        ReleaseDate
                    )
                    VALUES
                    (
                        @user_id,
                        @Title,
                        @Artist,
                        @FilePath,
                        @ImagePath,
                        @ReleaseDate
                    );";

                        using (MySqlCommand cmd =
                            new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue(
                                "@user_id",
                                UserSession.UserId
                            );

                            cmd.Parameters.AddWithValue(
                                "@Title",
                                tbTitle.Text.Trim()
                            );

                            cmd.Parameters.AddWithValue(
                                "@Artist",
                                string.IsNullOrWhiteSpace(tbArtist.Text)
                                ? "Unknown"
                                : tbArtist.Text.Trim()
                            );

                            cmd.Parameters.AddWithValue(
                                "@FilePath",
                                safeAudioPath
                            );

                            cmd.Parameters.AddWithValue(
                                "@ImagePath",
                                safeImagePath
                            );

                            cmd.Parameters.AddWithValue(
                                "@ReleaseDate",
                                dateNow
                            );

                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show(
                            "성공적으로 등록되었습니다!",
                            "성공",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                string realError =
                    ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;

                MessageBox.Show(
                    $"오류 원인:\n{realError}",
                    "오류",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        
        // 수정하기
        private void LoadSongData(int songId)
        {
            string connectionString =
                $"Server={dbHost};Port={dbPort};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"
                SELECT Title, Artist, FilePath, ImagePath
                FROM songs
                WHERE SongId = @songId;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@songId", songId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                tbTitle.Text = reader["Title"]?.ToString() ?? "";
                                tbArtist.Text = reader["Artist"]?.ToString() ?? "";

                                selectedAudioPath =
                                    reader["FilePath"]?.ToString() ?? "";

                                selectedImagePath =
                                    reader["ImagePath"]?.ToString() ?? "";

                                tbSongFile.Text = selectedAudioPath;

                                if (!string.IsNullOrEmpty(selectedImagePath))
                                {
                                    string imagePath =
                                        Path.Combine(
                                            Application.StartupPath,
                                            selectedImagePath
                                        );

                                    if (File.Exists(imagePath))
                                    {
                                        picAlbum.ImageLocation = imagePath;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"곡 정보 불러오기 실패:\n{ex.Message}");
            }
        }
    }
}
