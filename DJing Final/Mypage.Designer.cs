namespace DJing
{
    partial class Mypage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mypage));
            btnBack = new Button();
            lbUserName = new Label();
            picProfile = new PictureBox();
            btnMyUpload = new Button();
            btnLike = new Button();
            btnList = new Button();
            panBase = new Panel();
            ((System.ComponentModel.ISupportInitialize)picProfile).BeginInit();
            SuspendLayout();
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(255, 85, 0);
            btnBack.Cursor = Cursors.Hand;
            btnBack.FlatAppearance.BorderColor = Color.FromArgb(255, 85, 0);
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(775, 12);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(120, 36);
            btnBack.TabIndex = 0;
            btnBack.Text = "뒤로가기";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // lbUserName
            // 
            lbUserName.AutoSize = true;
            lbUserName.BackColor = Color.Transparent;
            lbUserName.Font = new Font("맑은 고딕", 20F, FontStyle.Regular, GraphicsUnit.Point);
            lbUserName.ForeColor = Color.White;
            lbUserName.Location = new Point(354, 52);
            lbUserName.Name = "lbUserName";
            lbUserName.Size = new Size(90, 37);
            lbUserName.TabIndex = 1;
            lbUserName.Text = "label1";
            // 
            // picProfile
            // 
            picProfile.BackColor = Color.Transparent;
            picProfile.Image = (Image)resources.GetObject("picProfile.Image");
            picProfile.Location = new Point(186, 12);
            picProfile.Name = "picProfile";
            picProfile.Size = new Size(149, 138);
            picProfile.SizeMode = PictureBoxSizeMode.Zoom;
            picProfile.TabIndex = 2;
            picProfile.TabStop = false;
            // 
            // btnMyUpload
            // 
            btnMyUpload.BackColor = Color.FromArgb(60, 60, 60);
            btnMyUpload.Cursor = Cursors.Hand;
            btnMyUpload.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
            btnMyUpload.FlatStyle = FlatStyle.Flat;
            btnMyUpload.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnMyUpload.ForeColor = Color.White;
            btnMyUpload.Location = new Point(50, 173);
            btnMyUpload.Name = "btnMyUpload";
            btnMyUpload.Size = new Size(263, 39);
            btnMyUpload.TabIndex = 0;
            btnMyUpload.Tag = " ";
            btnMyUpload.Text = "내 업로드 곡";
            btnMyUpload.UseVisualStyleBackColor = false;
            btnMyUpload.Click += btnMyUpload_Click;
            // 
            // btnLike
            // 
            btnLike.BackColor = Color.FromArgb(60, 60, 60);
            btnLike.Cursor = Cursors.Hand;
            btnLike.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
            btnLike.FlatStyle = FlatStyle.Flat;
            btnLike.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnLike.ForeColor = Color.White;
            btnLike.Location = new Point(320, 173);
            btnLike.Name = "btnLike";
            btnLike.Size = new Size(263, 39);
            btnLike.TabIndex = 0;
            btnLike.Text = "좋아요 한 곡";
            btnLike.UseVisualStyleBackColor = false;
            btnLike.Click += btnLike_Click;
            // 
            // btnList
            // 
            btnList.BackColor = Color.FromArgb(60, 60, 60);
            btnList.Cursor = Cursors.Hand;
            btnList.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
            btnList.FlatStyle = FlatStyle.Flat;
            btnList.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            btnList.ForeColor = Color.White;
            btnList.Location = new Point(590, 173);
            btnList.Name = "btnList";
            btnList.Size = new Size(263, 39);
            btnList.TabIndex = 0;
            btnList.Text = "내 재생목록";
            btnList.UseVisualStyleBackColor = false;
            btnList.Click += btnList_Click;
            // 
            // panBase
            // 
            panBase.AutoScroll = true;
            panBase.BackColor = Color.FromArgb(16, 16, 16);
            panBase.Location = new Point(50, 226);
            panBase.Name = "panBase";
            panBase.Size = new Size(803, 376);
            panBase.TabIndex = 3;
            // 
            // Mypage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(16, 16, 16);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(904, 681);
            Controls.Add(panBase);
            Controls.Add(picProfile);
            Controls.Add(lbUserName);
            Controls.Add(btnList);
            Controls.Add(btnLike);
            Controls.Add(btnMyUpload);
            Controls.Add(btnBack);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Mypage";
            Text = "Mypage";
            Load += Mypage_Load;
            ((System.ComponentModel.ISupportInitialize)picProfile).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBack;
        private Label lbUserName;
        private PictureBox picProfile;
        private Button btnMyUpload;
        private Button btnLike;
        private Button btnList;
        private Panel panBase;
    }
}