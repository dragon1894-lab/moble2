using DJing;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DJing
{
    public partial class Mypage : Form
    {
        private DJing.soundcloud? mainForm;
        private Button? btnMainUpload;

        // DB 접속 정보 (본인 환경에 맞게 수정)
        private string dbHost = "127.0.0.1";
        private string dbPort = "3306";
        private string dbUser = "root";
        private string dbPass = "1111";
        private string dbName = "djing";

        private int btnNum = 1;
        private Button? currentPlayButton;
        private int currentPlayingSongId = -1;

        public Mypage()
        {
            InitializeComponent();
            InitializeFormUI();
        }

        public Mypage(DJing.soundcloud form) : this()
        {
            this.mainForm = form;

            if (this.mainForm != null)
            {
                this.Size = mainForm.Size;
            }
        }

        private void InitializeFormUI()
        {
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.Size = new Size(920, 720); // 기본 예비 크기 지정

            // Load 이벤트 연결
            this.Load += Mypage_Load;

            // ★ [음원 업로드 버튼] 복구 및 생성
            btnMainUpload = new Button
            {
                Text = "음원 업로드",
                Size = new Size(120, 36),
                Location = new Point(640, 12),
                Font = new Font("맑은 고딕", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(255, 85, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnMainUpload.FlatAppearance.BorderSize = 0;
            btnMainUpload.Click += BtnMainUpload_Click;

            this.Controls.Add(btnMainUpload);
        }

        private void BtnMainUpload_Click(object? sender, EventArgs e)
        {
            if (UserSession.UserId == "Guest")
            {
                MessageBox.Show("로그인이 필요한 기능입니다.");
            }
            else
            {
                Upload uploadForm = new Upload();

                uploadForm.StartPosition = FormStartPosition.Manual;
                uploadForm.Location = new Point(
                    this.Left + (this.Width - uploadForm.Width) / 2,
                    this.Top + (this.Height - uploadForm.Height) / this.Top
                );

                uploadForm.FormClosed += (s, args) =>
                {
                    if (uploadForm.DialogResult == DialogResult.OK)
                    {
                        // 업로드 완료 후 목록 새로고침
                        LoadMySongsFromDB();
                        mainForm?.LoadSongsToUI();
                    }
                };

                uploadForm.ShowDialog();
            }
        }

        private void Mypage_Load(object? sender, EventArgs e)
        {
            // 1. 하단 플레이어 패널 이동 배치
            if (mainForm != null && mainForm.bottomPlayerPanel != null)
            {
                this.Controls.Add(mainForm.bottomPlayerPanel);
                mainForm.bottomPlayerPanel.BringToFront();
            }

            // 2. 로그인된 사용자 프로필(아이디, 아이콘) 정보 로드
            string currentUserId = UserSession.UserId;
            if (!string.IsNullOrEmpty(currentUserId))
            {
                LoadUserProfileFromDB(currentUserId);
            }

            // 3. DB에서 내 곡 목록 로드
            LoadMySongsFromDB();
        }

        // --- 프로필 및 아이디 연동 기능 ---
        private void LoadUserProfileFromDB(string userId)
        {
            string connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT user_id, icon FROM member WHERE user_id = @userId;";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // 레이블에 user_id 표시
                                if (lbUserName != null)
                                {
                                    lbUserName.Text = reader["user_id"].ToString();
                                }

                                // 프로필 아이콘 로드
                                string iconValue = reader["icon"] != DBNull.Value ? reader["icon"]?.ToString() ?? "" : "";

                                SetProfileIcon(iconValue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"프로필 정보 로드 오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetProfileIcon(string iconValue)
        {
            if (picProfile == null) return;

            if (picProfile.Image != null)
            {
                var oldImg = picProfile.Image;
                picProfile.Image = null;
                oldImg.Dispose();
            }

            picProfile.SizeMode = PictureBoxSizeMode.Zoom;

            if (string.IsNullOrWhiteSpace(iconValue)) return;

            // DB 값에 확장자가 없으면 .png 붙이기
            string fileName = iconValue.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                ? iconValue
                : $"{iconValue}.png";

            string imagePath = Path.Combine(Application.StartupPath, "Images", fileName);

            if (File.Exists(imagePath))
            {
                try
                {
                    using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                    {
                        picProfile.Image = new Bitmap(stream);
                        picProfile.BackColor = Color.Transparent;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[프로필 로드 실패] {ex.Message}");
                }
            }
            else
            {
                picProfile.BackColor = Color.FromArgb(45, 45, 45);
            }
        }

        public void LoadMySongsFromDB()
        {
            if (panBase == null) return;
            panBase.Controls.Clear();

            string connStr = $"Server={dbHost};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            // ★ 현재 로그인된 사용자 아이디 가져오기
            string currentUserId = UserSession.UserId;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // ★ WHERE user_id 조건을 추가하여 내 곡만 조회하도록 쿼리 수정
                    string query = "SELECT * FROM songs WHERE user_id = @userId ORDER BY 1 DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // ★ @userId 파라미터 바인딩
                        cmd.Parameters.AddWithValue("@userId", currentUserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int yPos = 8;

                            while (reader.Read())
                            {
                                int songId = 0;
                                if (HasColumn(reader, "Song_id")) songId = Convert.ToInt32(reader["Song_id"]);
                                else if (HasColumn(reader, "SongID")) songId = Convert.ToInt32(reader["SongID"]);
                                else if (HasColumn(reader, "id")) songId = Convert.ToInt32(reader["id"]);

                                string title = reader["Title"]?.ToString() ?? "";
                                string artist = reader["Artist"]?.ToString() ?? "";

                                string? imagePath = reader["ImagePath"] != DBNull.Value
                                    ? reader["ImagePath"]?.ToString()
                                    : null;

                                Image? coverImg = null;

                                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                                {
                                    coverImg = Image.FromFile(imagePath);
                                }

                                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                                {
                                    try { coverImg = Image.FromFile(imagePath); } catch { }
                                }

                                Panel panSongItem = CreateSongItemPanel(title, artist, coverImg, null, songId, 1);

                                panSongItem.Location = new Point(8, yPos);
                                yPos += panSongItem.Height + 8;

                                panBase.Controls.Add(panSongItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB 데이터 조회 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLikedSongsFromDB()
        {
            if (panBase == null) return;

            panBase.Controls.Clear();

            string connStr =
                $"Server={dbHost};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            string currentUserId = UserSession.UserId;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        SELECT songs.*
                        FROM songs
                        INNER JOIN song_likes
                        ON songs.SongId = song_likes.SongId
                        WHERE song_likes.user_id = @userId
                        ORDER BY song_likes.liked_at DESC;
                        ";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", currentUserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int yPos = 8;

                            while (reader.Read())
                            {
                                int songId = Convert.ToInt32(reader["SongId"]);

                                string title =
                                    reader["Title"]?.ToString() ?? "";

                                string artist =
                                    reader["Artist"]?.ToString() ?? "";

                                string? imagePath =
                                    reader["ImagePath"] != DBNull.Value
                                    ? reader["ImagePath"]?.ToString()
                                    : null;

                                Image? coverImg = null;

                                if (!string.IsNullOrEmpty(imagePath))
                                {
                                    string realImagePath =
                                        Path.Combine(Application.StartupPath, imagePath);

                                    if (File.Exists(realImagePath))
                                    {
                                        coverImg = Image.FromFile(realImagePath);
                                    }
                                }

                                Panel panSongItem =
                                    CreateSongItemPanel(
                                        title,
                                        artist,
                                        coverImg,
                                        null,
                                        songId,
                                        2
                                    );

                                panSongItem.Location =
                                    new Point(8, yPos);

                                yPos += panSongItem.Height + 8;

                                panBase.Controls.Add(panSongItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"좋아요 곡 조회 실패:\n{ex.Message}"
                );
            }
        }

        private void LoadPlaylistFromDB()
        {
            if (panBase == null) return;

            panBase.Controls.Clear();

            string connStr =
                $"Server={dbHost};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            string currentUserId = UserSession.UserId;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                SELECT songs.*
                FROM songs
                INNER JOIN playlist_songs
                    ON songs.SongId = playlist_songs.SongId
                WHERE playlist_songs.user_id = @userId
                ORDER BY playlist_songs.added_at DESC;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", currentUserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            int yPos = 8;

                            while (reader.Read())
                            {
                                int songId =
                                    Convert.ToInt32(reader["SongId"]);

                                string title =
                                    reader["Title"]?.ToString() ?? "";

                                string artist =
                                    reader["Artist"]?.ToString() ?? "";

                                string? imagePath =
                                    reader["ImagePath"] != DBNull.Value
                                    ? reader["ImagePath"]?.ToString()
                                    : null;

                                Image? coverImg = null;

                                if (!string.IsNullOrEmpty(imagePath))
                                {
                                    string realImagePath =
                                        Path.Combine(
                                            Application.StartupPath,
                                            imagePath
                                        );

                                    if (File.Exists(realImagePath))
                                    {
                                        coverImg =
                                            Image.FromFile(realImagePath);
                                    }
                                }

                                Panel songPanel =
                                    CreateSongItemPanel(
                                        title,
                                        artist,
                                        coverImg,
                                        null,
                                        songId,
                                        3
                                    );

                                songPanel.Location =
                                    new Point(8, yPos);

                                yPos += songPanel.Height + 8;

                                panBase.Controls.Add(songPanel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"재생목록 조회 실패:\n{ex.Message}"
                );
            }
        }

        private bool HasColumn(MySqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private Panel CreateSongItemPanel(string title, string artist, Image? coverImage, Image? waveformImage, int songId, int mode)
        {
            Panel panSongItem = new Panel
            {
                Name = "panSongItem",
                Size = new Size(775, 60),
                BackColor = Color.FromArgb(28, 28, 28)
            };

            PictureBox picCover = new PictureBox
            {
                Size = new Size(48, 48),
                Location = new Point(8, 6),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(40, 40, 40)
            };
            if (coverImage != null) picCover.Image = coverImage;

            Button btnPlay = new Button
            {
                Text = "▶",
                Font = new Font("맑은 고딕", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(255, 85, 0),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(34, 34),
                Location = new Point(62, 13),
                Cursor = Cursors.Hand,
                Tag = songId
            };

            btnPlay.FlatAppearance.BorderSize = 0;

            btnPlay.Click += (s, e) =>
            {
                if (mainForm == null)
                    return;

                // 현재 재생 중인 곡의 버튼을 다시 누름
                if (currentPlayingSongId == songId)
                {
                    bool isPlaying = mainForm.TogglePlayPause();

                    btnPlay.Text = isPlaying ? "||" : "▶";

                    return;
                }

                // 다른 곡을 눌렀다면 이전 버튼 원상복구
                if (currentPlayButton != null)
                {
                    currentPlayButton.Text = "▶";
                }

                // 새로운 곡 재생
                mainForm.PlaySongById(songId);

                btnPlay.Text = "||";

                currentPlayButton = btnPlay;
                currentPlayingSongId = songId;
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("맑은 고딕", 9.5f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(104, 10)
            };

            Label lblArtist = new Label
            {
                Text = $"{artist}",
                Font = new Font("맑은 고딕", 8.5f, FontStyle.Regular),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(104, 31)
            };

            PictureBox picWaveform = new PictureBox
            {
                Size = new Size(350, 40),
                Location = new Point(220, 10),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = waveformImage,
                BackColor = Color.Transparent
            };

            Button btnEdit = new Button
            {
                Text = "✏\n수정",
                Font = new Font("맑은 고딕", 8f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(52, 40),
                Location = new Point(650, 10),
                Cursor = Cursors.Hand,
                Tag = songId
            };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += btnEdit_Click;

            Button btnDelete = new Button
            {
                Text = "🗑\n삭제",
                Font = new Font("맑은 고딕", 8f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(215, 35, 35),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(52, 40),
                Location = new Point(710, 10),
                Cursor = Cursors.Hand,
                Tag = songId
            };
            btnDelete.FlatAppearance.BorderSize = 0;

            if (mode == 1)
            {
                // 내 업로드 곡
                btnEdit.Visible = true;

                btnDelete.Click += (s, e) =>
                {
                    DialogResult result = MessageBox.Show(
                        $"'{title}' 곡을 삭제하시겠습니까?",
                        "곡 삭제 확인",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        DeleteSongFromDB(songId);
                    }
                };
            }
            else if (mode == 2)
            {
                // 좋아요 목록
                btnEdit.Visible = false;

                btnDelete.Click += (s, e) =>
                {
                    DeleteLikedSong(songId);
                };
            }
            else if (mode == 3)
            {
                // 재생목록
                btnEdit.Visible = false;

                btnDelete.Click += (s, e) =>
                {
                    DeletePlaylistSong(songId);
                };
            }

            panSongItem.Controls.Add(picCover);
            panSongItem.Controls.Add(btnPlay);
            panSongItem.Controls.Add(lblTitle);
            panSongItem.Controls.Add(lblArtist);
            panSongItem.Controls.Add(picWaveform);
            panSongItem.Controls.Add(btnEdit);
            panSongItem.Controls.Add(btnDelete);

            return panSongItem;
        }

        private void DeleteSongFromDB(int songId)
        {
            string connStr = $"Server={dbHost};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string pkColumnName = "id";
                    string schemaQuery = @"
                        SELECT COLUMN_NAME 
                        FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                        WHERE TABLE_SCHEMA = @dbName 
                          AND TABLE_NAME = 'songs' 
                          AND CONSTRAINT_NAME = 'PRIMARY' 
                        LIMIT 1;";

                    using (MySqlCommand schemaCmd = new MySqlCommand(schemaQuery, conn))
                    {
                        schemaCmd.Parameters.AddWithValue("@dbName", dbName);
                        object result = schemaCmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            pkColumnName = result.ToString() ?? "id";
                        }
                    }

                    string query = $"DELETE FROM songs WHERE `{pkColumnName}` = @songId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@songId", songId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("곡이 성공적으로 삭제되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadMySongsFromDB();
                            mainForm?.LoadSongsToUI();
                        }
                        else
                        {
                            MessageBox.Show("삭제할 데이터를 찾지 못했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DB 삭제 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteLikedSong(int songId)
        {
            string connStr =
                $"Server={dbHost};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                DELETE FROM song_likes
                WHERE user_id = @userId
                AND SongId = @songId;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@userId",
                            UserSession.UserId);

                        cmd.Parameters.AddWithValue(
                            "@songId",
                            songId);

                        cmd.ExecuteNonQuery();
                    }

                    // songs의 좋아요 수도 1 감소
                    string updateQuery = @"
                UPDATE songs
                SET LikeCount = GREATEST(LikeCount - 1, 0)
                WHERE SongId = @songId;";

                    using (MySqlCommand cmd =
                        new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@songId", songId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("좋아요가 취소되었습니다.");

                LoadLikedSongsFromDB();

                // 메인화면 좋아요 숫자도 갱신
                mainForm?.LoadSongsToUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"좋아요 삭제 실패:\n{ex.Message}");
            }
        }

        private void DeletePlaylistSong(int songId)
        {
            string connStr =
                $"Server={dbHost};Database={dbName};Uid={dbUser};Pwd={dbPass};";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                DELETE FROM playlist_songs
                WHERE user_id = @userId
                AND SongId = @songId;";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", UserSession.UserId);
                        cmd.Parameters.AddWithValue("@songId", songId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("재생목록에서 삭제되었습니다.");

                LoadPlaylistFromDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"재생목록 삭제 실패:\n{ex.Message}");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (mainForm != null)
            {
                mainForm.Controls.Add(mainForm.bottomPlayerPanel);
                mainForm.StartPosition = FormStartPosition.Manual;
                mainForm.Location = this.Location;

                mainForm.Show();
                this.Hide();
            }
        }

        // 로그인 정보를 지우고 로그인 화면으로 이동
        private void btnLogout_Click(object sender, EventArgs e)
        {
            AppNavigation.Logout(this);
        }

        private void btnMyUpload_Click(object sender, EventArgs e)
        {
            if (btnNum == 1)
            {
                MessageBox.Show("현재 내 업로드 곡 화면입니다.");
            }
            else
            {
                LoadMySongsFromDB();
                btnNum = 1;
            }
        }

        private void btnLike_Click(object sender, EventArgs e)
        {
            if (btnNum == 2)
            {
                MessageBox.Show("현재 좋아요 한 곡 화면입니다.");
            }
            else
            {
                LoadLikedSongsFromDB();
                btnNum = 2;
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            if (btnNum == 3)
            {
                MessageBox.Show("현재 내 재생목록 화면입니다.");
            }
            else
            {
                LoadPlaylistFromDB();
                btnNum = 3;
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn)
                return;

            int songId = Convert.ToInt32(btn.Tag);

            Upload uploadForm = new Upload(songId);

            uploadForm.StartPosition = FormStartPosition.Manual;
            uploadForm.Location = new Point(
                this.Left + (this.Width - uploadForm.Width) / 2,
                this.Top + (this.Height - uploadForm.Height) / 2
            );

            if (uploadForm.ShowDialog() == DialogResult.OK)
            {
                LoadMySongsFromDB();
                mainForm?.LoadSongsToUI();
            }
        }
    }
}

