using djing;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DJing
{
    public partial class comment : Form
    {
        private FlowLayoutPanel flpComments;
        private TextBox tbComment;
        private Button btnWrite;
        private Button btnClose;

        private string connStr = "Server=localhost;Database=djing;Uid=root;Pwd=1111;";

        private int songId;

        public comment(int songId)
        {
            this.songId = songId;

            // 폼 설정
            Text = "댓글";
            Size = new Size(600, 600);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(20, 20, 20);

            // 댓글 목록
            flpComments = new FlowLayoutPanel
            {
                Location = new Point(20, 20),
                Size = new Size(540, 430),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            // 댓글 입력창
            tbComment = new TextBox
            {
                Location = new Point(20, 470),
                Size = new Size(430, 30)
            };

            // 등록 버튼
            btnWrite = new Button
            {
                Text = "등록",
                Location = new Point(460, 470),
                Size = new Size(100, 30),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = songId
            };

            // 닫기 버튼
            btnClose = new Button
            {
                Text = "닫기",
                Location = new Point(460, 515),
                Size = new Size(100, 30),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Tag = songId
            };

            btnWrite.Click += btnWrite_Click;
            btnClose.Click += btnClose_Click;

            Controls.Add(flpComments);
            Controls.Add(tbComment);
            Controls.Add(btnWrite);
            Controls.Add(btnClose);

            LoadComments();
        }

        private void btnWrite_Click(object? sender, EventArgs e)
        {
            if (UserSession.UserId == "Guest")
            {
                MessageBox.Show("로그인이 필요한 기능입니다.");
                return;
            }

            string content = tbComment.Text.Trim();

            if (string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("댓글 내용을 입력해주세요.");
                tbComment.Focus();
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                        INSERT INTO comment
                        (SongId, user_id, content)
                        VALUES
                        (@songId, @userId, @content);";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@songId", songId);
                        cmd.Parameters.AddWithValue("@userId", UserSession.UserId);
                        cmd.Parameters.AddWithValue("@content", content);

                        cmd.ExecuteNonQuery();
                    }
                }

                tbComment.Clear();

                // 등록 후 댓글 목록 다시 불러오기
                LoadComments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"댓글 등록 실패:\n{ex.Message}");
            }
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void LoadComments()
        {
            flpComments.Controls.Clear();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string sql = @"
                    SELECT
                        c.comment_id,
                        c.user_id,
                        c.content,
                        c.created_at,
                        m.icon,

                        (SELECT COUNT(*)
                        FROM comment_like cl
                        WHERE cl.comment_id = c.comment_id) AS like_count,

                        EXISTS (
                            SELECT 1
                            FROM comment_like cl2
                            WHERE cl2.comment_id = c.comment_id
                            AND cl2.user_id = @currentUserId
                        ) AS is_liked

                    FROM comment c
                    INNER JOIN member m
                    ON c.user_id = m.user_id
                    WHERE c.SongId = @songId
                    ORDER BY c.created_at DESC;";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@songId", songId);
                        cmd.Parameters.AddWithValue("@currentUserId", UserSession.UserId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int commentId =
                                    Convert.ToInt32(reader["comment_id"]);

                                string userId =
                                    reader["user_id"]?.ToString() ?? "";

                                string content =
                                    reader["content"]?.ToString() ?? "";
                                string iconName =
                                    reader["icon"]?.ToString() ?? "";

                                DateTime createdAt =
                                    Convert.ToDateTime(reader["created_at"]);

                                int likeCount =
                                    Convert.ToInt32(reader["like_count"]);

                                bool isLiked =
                                    Convert.ToInt32(reader["is_liked"]) > 0;

                                Panel panel = new Panel
                                {
                                    Width = flpComments.ClientSize.Width - 40,
                                    Height = 105,
                                    BackColor = Color.FromArgb(30, 30, 30),
                                    Margin = new Padding(5)
                                };

                                PictureBox picProfile = new PictureBox
                                {
                                    Size = new Size(45, 45),
                                    Location = new Point(10, 15),
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    BackColor = Color.FromArgb(45, 45, 45)
                                };

                                picProfile.Image = GetIconImageFromName(iconName);

                                Label lblUser = new Label
                                {
                                    Text = userId,
                                    Location = new Point(65, 10),
                                    AutoSize = true,
                                    ForeColor = Color.White,
                                    Font = new Font("맑은 고딕", 9, FontStyle.Bold)
                                };

                                Label lblDate = new Label
                                {
                                    Text = createdAt.ToString("yyyy-MM-dd HH:mm"),
                                    Location = new Point(400, 10),   // 날짜 오른쪽으로 이동
                                    AutoSize = true,
                                    ForeColor = Color.Gray,
                                    Font = new Font("맑은 고딕", 8)
                                };

                                Label lblContent = new Label
                                {
                                    Text = content,
                                    Location = new Point(65, 38),
                                    Size = new Size(panel.Width - 150, 40),
                                    ForeColor = Color.White
                                };

                                Button btnLike = new Button
                                {
                                    Text = isLiked
                                        ? $"♥ {likeCount}"
                                        : $"♡ {likeCount}",

                                    Size = new Size(70, 25),
                                    Location = new Point(65, 72),

                                    ForeColor = isLiked
                                        ? Color.Red
                                        : Color.White,

                                    BackColor = Color.FromArgb(45, 45, 45),
                                    FlatStyle = FlatStyle.Flat,
                                    Cursor = Cursors.Hand
                                };

                                btnLike.FlatAppearance.BorderSize = 0;

                                btnLike.Click += (s, e) =>
                                {
                                    ToggleLike(commentId);
                                };

                                panel.Controls.Add(picProfile);
                                panel.Controls.Add(lblUser);
                                panel.Controls.Add(lblDate);
                                panel.Controls.Add(lblContent);
                                panel.Controls.Add(btnLike);

                                if (UserSession.UserId == userId || UserSession.UserId == "admin")
                                {
                                    Button btnDelete = new Button
                                    {
                                        Text = "삭제",
                                        Size = new Size(55, 25),
                                        Location = new Point(panel.Width - 65, 72),
                                        ForeColor = Color.White,
                                        BackColor = Color.FromArgb(190, 40, 40),
                                        FlatStyle = FlatStyle.Flat,
                                        Cursor = Cursors.Hand
                                    };

                                    btnDelete.FlatAppearance.BorderSize = 0;

                                    btnDelete.Click += (s, e) =>
                                    {
                                        DeleteComment(commentId);
                                    };

                                    panel.Controls.Add(btnDelete);
                                }

                                flpComments.Controls.Add(panel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"댓글 조회 실패:\n{ex.Message}");
            }
        }

        private Image? GetIconImageFromName(string iconName)
        {
            using (ProfileEdit tempForm = new ProfileEdit(UserSession.UserId))
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

                return null;
            }
        }

        private void DeleteComment(int commentId)
        {
            DialogResult result = MessageBox.Show(
                "댓글을 삭제하시겠습니까?",
                "댓글 삭제",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string sql;

                    if (UserSession.UserId == "admin")
                    {
                        // 관리자는 모든 댓글 삭제 가능
                        sql = @"
                    DELETE FROM comment
                    WHERE comment_id = @commentId;";
                    }
                    else
                    {
                        // 일반 회원은 자신이 쓴 댓글만 삭제
                        sql = @"
                    DELETE FROM comment
                    WHERE comment_id = @commentId
                    AND user_id = @userId;";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@commentId",
                            commentId
                        );

                        if (UserSession.UserId != "admin")
                        {
                            cmd.Parameters.AddWithValue(
                                "@userId",
                                UserSession.UserId
                            );
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                LoadComments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"댓글 삭제 실패:\n{ex.Message}"
                );
            }
        }
        private void ToggleLike(int commentId)
        {
            if (UserSession.UserId == "Guest")
            {
                MessageBox.Show("로그인이 필요한 기능입니다.");
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string checkSql = @"
                SELECT COUNT(*)
                FROM comment_like
                WHERE comment_id = @commentId
                AND user_id = @userId;";

                    bool isLiked;

                    using (MySqlCommand cmd = new MySqlCommand(checkSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@commentId", commentId);
                        cmd.Parameters.AddWithValue("@userId", UserSession.UserId);

                        isLiked =
                            Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }

                    if (isLiked)
                    {
                        string deleteSql = @"
                    DELETE FROM comment_like
                    WHERE comment_id = @commentId
                    AND user_id = @userId;";

                        using (MySqlCommand cmd = new MySqlCommand(deleteSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@commentId", commentId);
                            cmd.Parameters.AddWithValue("@userId", UserSession.UserId);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = @"
                    INSERT INTO comment_like
                    (comment_id, user_id)
                    VALUES
                    (@commentId, @userId);";

                        using (MySqlCommand cmd = new MySqlCommand(insertSql, conn))
                        {
                            cmd.Parameters.AddWithValue("@commentId", commentId);
                            cmd.Parameters.AddWithValue("@userId", UserSession.UserId);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                LoadComments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"좋아요 처리 실패:\n{ex.Message}"
                );
            }
        }
    }
}