using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace CommentPart
{
    public partial class comment : Form
    {
        private int currentMixId;
        private bool isLiked;
        public comment(int mixId)
        {
            InitializeComponent();
            currentMixId = mixId;
            // 테스트용 임시 로그인 처리 (나중에 실제 로그인 연결되면 삭제)
            if (string.IsNullOrEmpty(SessionManager.CurrentUserId))
            {
                SessionManager.CurrentUserId = "123";
            }
        }

        private void comment_Load(object sender, EventArgs e)
        {
            SetupGrid();
            LoadComments(currentMixId);
            LoadLikeStatus();

        }
        // DataGridView 컬럼 미리 설정 (한 번만 실행)
        private void SetupGrid()
        {
            dgvComments.Columns.Clear();
            dgvComments.Columns.Add("comment_id", "번호");
            dgvComments.Columns.Add("user_id", "작성자ID");
            dgvComments.Columns.Add("display", "댓글");

            dgvComments.Columns["comment_id"].Visible = false;
            dgvComments.Columns["user_id"].Visible = false;
            dgvComments.Columns["display"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvComments.ColumnHeadersVisible = false;
        }
        // 댓글 작성
        private void btnCommentSubmit_Click(object sender, EventArgs e)
        {
            string content = txtComment.Text;

            if (string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("댓글을 입력해주세요.");
                return;
            }

            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO comment (mix_id, user_id, content) VALUES (@mixId, @userId, @content)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mixId", currentMixId);
                    cmd.Parameters.AddWithValue("@userId", SessionManager.CurrentUserId);
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.ExecuteNonQuery();

                    txtComment.Clear();
                    LoadComments(currentMixId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("댓글 작성 중 오류가 발생했습니다: " + ex.Message);
                }
            }
        }
        // 댓글 불러오기 (comment_id, user_id도 같이 저장)
        private void LoadComments(int mixId)
        {
            dgvComments.Rows.Clear();

            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT c.comment_id, c.user_id, m.name, c.content,c.created_at" +
                        " FROM comment AS c" +
                        " JOIN member AS m ON c.user_id = m.user_id" +
                        " WHERE c.mix_id = @mixId" +
                        " ORDER BY c.created_at ASC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mixId", mixId);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int commentId = Convert.ToInt32(reader["comment_id"]);
                        string writerId = reader["user_id"].ToString();
                        string userName = reader["name"].ToString();
                        string commentContent = reader["content"].ToString();
                        DateTime createdAt = Convert.ToDateTime(reader["created_at"]);

                        string relativeTime = GetRelativeTime(createdAt);
                        string displayText = $"[{userName}] {commentContent} ({relativeTime})";

                        dgvComments.Rows.Add(commentId, writerId, displayText);
                    }
                    lblCommentCount.Text = $"댓글{dgvComments.Rows.Count -1}개";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("댓글 로드 중 오류가 발생했습니다: " + ex.Message);
                }
            }
        }
        private string GetRelativeTime(DateTime createdAt)
        {
            TimeSpan gap = DateTime.Now - createdAt;

            if (gap.TotalMinutes < 1) return "방금 전"; // 올린지 1분전이면 방금 전 이라고 뜸
            if (gap.TotalMinutes < 60) return $"{(int)gap.TotalMinutes}분 전";
            if (gap.TotalHours < 24) return $"{(int)gap.TotalHours} 시간 전";
            if (gap.TotalDays < 7) return $"{(int)gap.TotalDays}일 전";

            return createdAt.ToString("yyyy-MM-dd");
        }
        // 댓글 삭제(본인 댓글만)
        private void btnDeleteComment_Click(object sender, EventArgs e)
        {
            if (dgvComments.SelectedRows.Count == 0)
            {
                MessageBox.Show("삭제할 댓글을 선택해주세요."); return;
            }

            DataGridViewRow selectedRow = dgvComments.SelectedRows[0];
            int commentId = Convert.ToInt32(selectedRow.Cells["comment_id"].Value);
            string writerId = selectedRow.Cells["user_id"].Value.ToString();

            if (writerId != SessionManager.CurrentUserId)
            {
                MessageBox.Show("자신의 댓글만 삭제할 수 있습니다."); return;
            }

            DialogResult confirm = MessageBox.Show("정말로 댓글을 삭제하시겠습니까?", "댓글 삭제", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM comment WHERE comment_id = @commentId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@commentId", commentId);
                    cmd.ExecuteNonQuery();

                    LoadComments(currentMixId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("댓글 삭제 중 오류가 발생했습니다: " + ex.Message);
                }
            }

        }
        //좋아요 상태 불러오기
        private void LoadLikeStatus()
        {
            using (MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM likes WHERE mix_id = @mixId AND user_id = @userId";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@mixId", currentMixId);
                    cmd.Parameters.AddWithValue("@userId", SessionManager.CurrentUserId);
                    int likedcount = Convert.ToInt32(cmd.ExecuteScalar());
                    isLiked = likedcount > 0;

                    string countQuery = "SELECT like_count FROM mix WHERE mix_id = @mixId";
                    MySqlCommand countCmd = new MySqlCommand(countQuery, conn);
                    countCmd.Parameters.AddWithValue("@mixId", currentMixId);
                    int totalLikes = Convert.ToInt32(countCmd.ExecuteScalar());

                    UpdateHeartDisplay(totalLikes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("좋아요 상태 불러오기 중 오류가 발생했습니다: " + ex.Message);
                }
            }
        }
        //하트 클릭(좋아요 토글)
        private void lblHeart_Click(object sender, EventArgs e)
        {
            using(MySqlConnection conn = DBHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    if (!isLiked)
                    {
                        string insertQuery = "INSERT INTO likes (mix_id, user_id) VALUES (@mixId, @userId)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@mixId", currentMixId);
                        insertCmd.Parameters.AddWithValue("@userId", SessionManager.CurrentUserId);
                        insertCmd.ExecuteNonQuery();
                       
                        string updateQuery = "UPDATE mix SET like_count = like_count + 1 WHERE mix_id = @mixId";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@mixId", currentMixId);
                        updateCmd.ExecuteNonQuery();
                        
                        isLiked = true;
                    }
                    else
                    {
                        string deleteQuery = "DELETE FROM likes WHERE mix_id = @mixId AND user_id = @userId";
                        MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn);
                        deleteCmd.Parameters.AddWithValue("@mixId", currentMixId);
                        deleteCmd.Parameters.AddWithValue("@userId", SessionManager.CurrentUserId);
                        deleteCmd.ExecuteNonQuery();
                       
                        string updateQuery = "UPDATE mix SET like_count = like_count - 1 WHERE mix_id = @mixId";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@mixId", currentMixId);
                        updateCmd.ExecuteNonQuery();
                        isLiked = false;
                    }

                    string countQuery = "SELECT like_count FROM mix WHERE mix_id = @mixId";
                    MySqlCommand countCmd = new MySqlCommand(countQuery, conn);
                    countCmd.Parameters.AddWithValue("@mixId", currentMixId);
                    int totalLikes = Convert.ToInt32(countCmd.ExecuteScalar());
                    UpdateHeartDisplay(totalLikes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("좋아요 토글 중 오류가 발생했습니다: " + ex.Message);
                }
            }
        }
       
        private void UpdateHeartDisplay(int totalLikes)
        {
            lblHeart.Text = isLiked ? "♥" : "♡";
            lblLikeCount.Text = totalLikes.ToString();
            
            lblHeart.ForeColor = isLiked ? Color.Red : Color.Black;
        }
    }
}
