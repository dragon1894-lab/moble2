#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DJing
{
    internal class SongItemControl : UserControl
    {
        public event EventHandler<SongData>? PlayRequested;
        public event EventHandler<double>? SeekRequested;

        private SongData? Data;
        private bool isLiked = false;
        private string connectionString;

        private PictureBox PicAlbum = null!;
        private Button BtnPlay = null!;
        private Label LblTitle = null!;
        private Label LblArtist = null!;
        private Label LblDate = null!;
        private Button BtnLike = null!;
        private Button BtnSave = null!;
        private Button BtnComment = null!;
        private WaveformControl WaveControl = null!;
        private Button BtnDelete = null!;

        public SongItemControl(string connStr = "")
        {
            this.connectionString = connStr;
            InitializeControlUI();
        }

        private void InitializeControlUI()
        {
            this.Size = new Size(860, 160);
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.Margin = new Padding(0, 0, 0, 15);

            PicAlbum = new PictureBox
            {
                Size = new Size(120, 120),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(40, 40, 40)
            };

            BtnPlay = new Button
            {
                Size = new Size(45, 45),
                Location = new Point(145, 15),
                Text = "▶",
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(255, 85, 0),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            BtnPlay.FlatAppearance.BorderSize = 0;
            BtnPlay.Click += (s, e) =>
            {
                if (Data != null)
                {
                    PlayRequested?.Invoke(this, Data);
                }
            };

            LblTitle = new Label
            {
                Location = new Point(200, 15),
                AutoSize = true,
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            LblTitle.Click += (s, e) =>
            {
                if (Data != null)
                {
                    PlayRequested?.Invoke(this, Data);
                }
            };

            LblArtist = new Label
            {
                Location = new Point(200, 42),
                AutoSize = true,
                Font = new Font("맑은 고딕", 9, FontStyle.Regular),
                ForeColor = Color.DarkGray
            };

            LblDate = new Label
            {
                Location = new Point(790, 15),
                AutoSize = true,
                Font = new Font("맑은 고딕", 9, FontStyle.Regular),
                ForeColor = Color.Gray
            };

            WaveControl = new WaveformControl
            {
                Location = new Point(145, 63),
                Size = new Size(750, 50),
                BackColor = Color.FromArgb(30, 30, 30),
                Cursor = Cursors.Hand
            };
            WaveControl.SeekRequested += (s, targetRatio) => SeekRequested?.Invoke(this, targetRatio);

            BtnLike = new Button
            {
                Size = new Size(70, 28),
                Location = new Point(145, 120),
                Text = "♥ 0",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 40, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            BtnLike.FlatAppearance.BorderSize = 0;
            BtnLike.Click += BtnLike_Click;

            BtnSave = new Button
            {
                Size = new Size(60, 28),
                Location = new Point(220, 120),
                Text = "저장",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 40, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            BtnSave.FlatAppearance.BorderSize = 0;
            BtnSave.Click += BtnSave_Click;

            BtnComment = new Button
            {
                Size = new Size(60, 28),
                Location = new Point(285, 120),
                Text = "댓글",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(40, 40, 40),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            BtnComment.FlatAppearance.BorderSize = 0;
            BtnComment.Click += BtnComment_Click;

            if (UserSession.UserId == "admin")
            {
                BtnDelete = new Button
                {
                    Size = new Size(60, 28),
                    Location = new Point(800, 120),
                    Text = "삭제",
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(215, 35, 35),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };

                BtnDelete.FlatAppearance.BorderSize = 0;
                BtnDelete.Click += BtnDelete_Click;

                this.Controls.Add(BtnDelete);
            }

            this.Controls.Add(PicAlbum);
            this.Controls.Add(BtnPlay);
            this.Controls.Add(LblTitle);
            this.Controls.Add(LblArtist);
            this.Controls.Add(LblDate);
            this.Controls.Add(WaveControl);
            this.Controls.Add(BtnLike);
            this.Controls.Add(BtnSave);
            this.Controls.Add(BtnComment);
        }

        public void BindData(SongData data)
        {
            Data = data;
            LblTitle.Text = data.Title;
            LblArtist.Text = data.Artist;
            LblDate.Text = data.ReleaseDate;
            BtnLike.Text = $"♥ {data.LikeCount}";
            LoadLikeState();

            if (PicAlbum.Image != null)
            {
                var oldImg = PicAlbum.Image;
                PicAlbum.Image = null;
                oldImg.Dispose();
            }

            if (!string.IsNullOrEmpty(data.ImagePath) && File.Exists(data.ImagePath))
            {
                try
                {
                    using (var stream = new FileStream(data.ImagePath, FileMode.Open, FileAccess.Read))
                    {
                        PicAlbum.Image = new Bitmap(stream);
                    }
                }
                catch
                {
                    PicAlbum.BackColor = Color.FromArgb(40, 40, 40);
                }
            }
        }

        private void LoadLikeState()
        {
            if (Data == null || string.IsNullOrEmpty(UserSession.UserId))
                return;

            string sql = @"
            SELECT COUNT(*)
            FROM song_likes
            WHERE user_id = @userId
            AND SongId = @songId;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", UserSession.UserId);
                    cmd.Parameters.AddWithValue("@songId", Data.SongId);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    isLiked = count > 0;

                    BtnLike.ForeColor = isLiked
                        ? Color.FromArgb(255, 85, 0)
                        : Color.White;
                }
            }
        }

        public void SetWaveformData(float[] peaks)
        {
            if (this.WaveControl != null)
            {
                this.WaveControl.SetWaveformData(peaks);
            }
        }

        public void UpdateWaveState(bool isPlaying, double progressRatio)
        {
            BtnPlay.Text = isPlaying ? "||" : "▶";
            WaveControl.SetProgress(progressRatio);
        }

        private void BtnLike_Click(object? sender, EventArgs e)
        {
            if (UserSession.UserId == "Guest")
            {
                MessageBox.Show("로그인이 필요한 기능입니다.");
            }
            else
            {
                if (Data == null || string.IsNullOrEmpty(connectionString))
                    return;

                string userId = UserSession.UserId;

                if (string.IsNullOrEmpty(userId))
                {
                    MessageBox.Show("로그인이 필요합니다.");
                    return;
                }

                int delta;

                // 현재 좋아요가 안 되어 있다면 → 좋아요
                if (!isLiked)
                {
                    Data.LikeCount++;
                    delta = 1;

                    isLiked = true;

                    BtnLike.ForeColor =
                        Color.FromArgb(255, 85, 0);
                }
                // 현재 좋아요 상태라면 → 좋아요 취소
                else
                {
                    Data.LikeCount--;
                    delta = -1;

                    isLiked = false;

                    BtnLike.ForeColor = Color.White;
                }

                BtnLike.Text = $"♥ {Data.LikeCount}";

                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        // songs 테이블의 총 좋아요 수 변경
                        string sql = @"
                        UPDATE songs
                        SET LikeCount = LikeCount + @delta
                        WHERE SongId = @id;";

                        using (var cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@delta", delta);
                            cmd.Parameters.AddWithValue("@id", Data.SongId);

                            cmd.ExecuteNonQuery();
                        }


                        // 좋아요 상태가 되었다면
                        if (isLiked)
                        {
                            string sql2 = @"
                            INSERT INTO song_likes (user_id, SongId)
                            VALUES (@userId, @songId);";

                            using (var cmd2 = new MySqlCommand(sql2, conn))
                            {
                                cmd2.Parameters.AddWithValue("@userId", userId);
                                cmd2.Parameters.AddWithValue("@songId", Data.SongId);

                                cmd2.ExecuteNonQuery();
                            }
                        }

                        // 좋아요 취소 상태가 되었다면
                        else
                        {
                            string sql2 = @"
                            DELETE FROM song_likes
                            WHERE user_id = @userId
                            AND SongId = @songId;";

                            using (var cmd2 = new MySqlCommand(sql2, conn))
                            {
                                cmd2.Parameters.AddWithValue("@userId", userId);
                                cmd2.Parameters.AddWithValue("@songId", Data.SongId);

                                cmd2.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"좋아요 반영 실패:\n{ex.Message}"
                    );
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (UserSession.UserId == "Guest")
            {
                MessageBox.Show("로그인이 필요한 기능입니다.");
            }
            else
            {
                if (Data == null || string.IsNullOrEmpty(UserSession.UserId))
                    return;

                string userId = UserSession.UserId;

                try
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();

                        string sql = @"
                    INSERT INTO playlist_songs (user_id, SongId)
                    VALUES (@userId, @songId);";

                        using (var cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);
                            cmd.Parameters.AddWithValue("@songId", Data.SongId);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("재생목록에 추가되었습니다.");
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    MessageBox.Show("이미 재생목록에 있는 곡입니다.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"재생목록 추가 실패:\n{ex.Message}");
                }
            }
        }
        private void BtnComment_Click(object? sender, EventArgs e)
        {
            //if (UserSession.UserId == "Guest")
            //{
            //    MessageBox.Show("로그인이 필요한 기능입니다.");
            //}
            //else
            //{

            //}
            if (Data == null)
                return;

            comment commentForm = new comment(Data.SongId);

            commentForm.ShowDialog();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (Data == null)
                return;

            DialogResult result = MessageBox.Show(
                $"'{Data.Title}' 곡을 정말 삭제하시겠습니까?",
                "음악 삭제",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. 재생목록에서 먼저 삭제
                    string sql1 = @"
                        DELETE FROM playlist_songs
                        WHERE SongId = @songId;";

                    using (MySqlCommand cmd1 = new MySqlCommand(sql1, conn))
                    {
                        cmd1.Parameters.AddWithValue("@songId", Data.SongId);
                        cmd1.ExecuteNonQuery();
                    }

                    // 2. 좋아요 기록 삭제
                    string sql2 = @"
                        DELETE FROM song_likes
                        WHERE SongId = @songId;";

                    using (MySqlCommand cmd2 = new MySqlCommand(sql2, conn))
                    {
                        cmd2.Parameters.AddWithValue("@songId", Data.SongId);
                        cmd2.ExecuteNonQuery();
                    }

                    // 3. 마지막에 실제 음악 삭제
                    string sql3 = @"
                        DELETE FROM songs
                        WHERE SongId = @songId;";

                    using (MySqlCommand cmd3 = new MySqlCommand(sql3, conn))
                    {
                        cmd3.Parameters.AddWithValue("@songId", Data.SongId);

                        int rows = cmd3.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("음악이 삭제되었습니다.");

                            this.Parent?.Controls.Remove(this);
                            this.Dispose();
                        }
                        else
                        {
                            MessageBox.Show("삭제할 음악을 찾지 못했습니다.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"음악 삭제 실패:\n{ex.Message}");
            }
        }
    }
}
