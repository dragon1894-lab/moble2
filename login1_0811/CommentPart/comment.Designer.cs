namespace CommentPart
{
    partial class comment
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvComments = new DataGridView();
            lblCommentCount = new Label();
            txtComment = new TextBox();
            btnDeleteComment = new Button();
            btnCommentSubmit = new Button();
            lblHeart = new Label();
            lblLikeCount = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvComments).BeginInit();
            SuspendLayout();
            // 
            // dgvComments
            // 
            dgvComments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvComments.Location = new Point(12, 12);
            dgvComments.Name = "dgvComments";
            dgvComments.RowTemplate.Height = 25;
            dgvComments.Size = new Size(544, 275);
            dgvComments.TabIndex = 0;
            // 
            // lblCommentCount
            // 
            lblCommentCount.AutoSize = true;
            lblCommentCount.Location = new Point(12, 317);
            lblCommentCount.Name = "lblCommentCount";
            lblCommentCount.Size = new Size(39, 15);
            lblCommentCount.TabIndex = 1;
            lblCommentCount.Text = "label1";
            // 
            // txtComment
            // 
            txtComment.Location = new Point(12, 352);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.Size = new Size(379, 53);
            txtComment.TabIndex = 2;
            // 
            // btnDeleteComment
            // 
            btnDeleteComment.Location = new Point(435, 307);
            btnDeleteComment.Name = "btnDeleteComment";
            btnDeleteComment.Size = new Size(121, 26);
            btnDeleteComment.TabIndex = 3;
            btnDeleteComment.Text = "button1";
            btnDeleteComment.UseVisualStyleBackColor = true;
            btnDeleteComment.Click += btnDeleteComment_Click;
            // 
            // btnCommentSubmit
            // 
            btnCommentSubmit.Location = new Point(413, 352);
            btnCommentSubmit.Name = "btnCommentSubmit";
            btnCommentSubmit.Size = new Size(143, 53);
            btnCommentSubmit.TabIndex = 3;
            btnCommentSubmit.Text = "button1";
            btnCommentSubmit.UseVisualStyleBackColor = true;
            btnCommentSubmit.Click += btnCommentSubmit_Click;
            // 
            // lblHeart
            // 
            lblHeart.AutoSize = true;
            lblHeart.Font = new Font("한컴 말랑말랑 Bold", 20.2499962F, FontStyle.Bold, GraphicsUnit.Point);
            lblHeart.ForeColor = Color.Red;
            lblHeart.Location = new Point(79, 297);
            lblHeart.Name = "lblHeart";
            lblHeart.Size = new Size(39, 35);
            lblHeart.TabIndex = 4;
            lblHeart.Text = "♡";
            lblHeart.Click += lblHeart_Click;
            // 
            // lblLikeCount
            // 
            lblLikeCount.AutoSize = true;
            lblLikeCount.Location = new Point(79, 332);
            lblLikeCount.Name = "lblLikeCount";
            lblLikeCount.Size = new Size(39, 15);
            lblLikeCount.TabIndex = 4;
            lblLikeCount.Text = "label1";
            // 
            // comment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 433);
            Controls.Add(lblLikeCount);
            Controls.Add(lblHeart);
            Controls.Add(btnCommentSubmit);
            Controls.Add(btnDeleteComment);
            Controls.Add(txtComment);
            Controls.Add(lblCommentCount);
            Controls.Add(dgvComments);
            Name = "comment";
            Text = "comment";
            Load += comment_Load;
            ((System.ComponentModel.ISupportInitialize)dgvComments).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvComments;
        private Label lblCommentCount;
        private TextBox txtComment;
        private Button btnDeleteComment;
        private Button btnCommentSubmit;
        private Label lblHeart;
        private Label lblLikeCount;
    }
}
