namespace DJing
{
    partial class Upload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Upload));
            picAlbum = new PictureBox();
            label1 = new Label();
            tbTitle = new TextBox();
            panel1 = new Panel();
            label2 = new Label();
            tbArtist = new TextBox();
            panel2 = new Panel();
            tbSongFile = new TextBox();
            panel3 = new Panel();
            btnFindMusic = new Button();
            label3 = new Label();
            btnUpload = new Button();
            ((System.ComponentModel.ISupportInitialize)picAlbum).BeginInit();
            SuspendLayout();
            // 
            // picAlbum
            // 
            picAlbum.Image = (Image)resources.GetObject("picAlbum.Image");
            picAlbum.Location = new Point(19, 29);
            picAlbum.Name = "picAlbum";
            picAlbum.Size = new Size(220, 220);
            picAlbum.SizeMode = PictureBoxSizeMode.StretchImage;
            picAlbum.TabIndex = 0;
            picAlbum.TabStop = false;
            picAlbum.Click += pictureBox1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(274, 32);
            label1.Name = "label1";
            label1.Size = new Size(42, 21);
            label1.TabIndex = 1;
            label1.Text = "제목";
            // 
            // tbTitle
            // 
            tbTitle.BackColor = Color.FromArgb(16, 16, 16);
            tbTitle.BorderStyle = BorderStyle.None;
            tbTitle.Font = new Font("맑은 고딕", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbTitle.ForeColor = Color.White;
            tbTitle.Location = new Point(274, 64);
            tbTitle.Name = "tbTitle";
            tbTitle.Size = new Size(541, 18);
            tbTitle.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Location = new Point(274, 87);
            panel1.Name = "panel1";
            panel1.Size = new Size(541, 1);
            panel1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(274, 113);
            label2.Name = "label2";
            label2.Size = new Size(74, 21);
            label2.TabIndex = 1;
            label2.Text = "아티스트";
            // 
            // tbArtist
            // 
            tbArtist.BackColor = Color.FromArgb(16, 16, 16);
            tbArtist.BorderStyle = BorderStyle.None;
            tbArtist.Font = new Font("맑은 고딕", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbArtist.ForeColor = Color.White;
            tbArtist.Location = new Point(274, 145);
            tbArtist.Name = "tbArtist";
            tbArtist.Size = new Size(541, 18);
            tbArtist.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Location = new Point(274, 168);
            panel2.Name = "panel2";
            panel2.Size = new Size(541, 1);
            panel2.TabIndex = 3;
            // 
            // tbSongFile
            // 
            tbSongFile.BackColor = Color.FromArgb(16, 16, 16);
            tbSongFile.BorderStyle = BorderStyle.None;
            tbSongFile.Font = new Font("맑은 고딕", 10F, FontStyle.Regular, GraphicsUnit.Point);
            tbSongFile.ForeColor = Color.White;
            tbSongFile.Location = new Point(274, 221);
            tbSongFile.Name = "tbSongFile";
            tbSongFile.Size = new Size(418, 18);
            tbSongFile.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Location = new Point(274, 244);
            panel3.Name = "panel3";
            panel3.Size = new Size(418, 1);
            panel3.TabIndex = 3;
            // 
            // btnFindMusic
            // 
            btnFindMusic.BackColor = Color.FromArgb(60, 60, 60);
            btnFindMusic.FlatStyle = FlatStyle.Popup;
            btnFindMusic.ForeColor = Color.White;
            btnFindMusic.Location = new Point(709, 217);
            btnFindMusic.Name = "btnFindMusic";
            btnFindMusic.Size = new Size(106, 31);
            btnFindMusic.TabIndex = 4;
            btnFindMusic.Text = "파일불러오기";
            btnFindMusic.UseVisualStyleBackColor = false;
            btnFindMusic.Click += btnFindMusic_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.White;
            label3.Location = new Point(274, 190);
            label3.Name = "label3";
            label3.Size = new Size(80, 21);
            label3.TabIndex = 1;
            label3.Text = "노래 파일";
            // 
            // btnUpload
            // 
            btnUpload.BackColor = Color.FromArgb(60, 60, 60);
            btnUpload.FlatStyle = FlatStyle.Popup;
            btnUpload.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            btnUpload.ForeColor = Color.White;
            btnUpload.Location = new Point(333, 326);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(212, 72);
            btnUpload.TabIndex = 4;
            btnUpload.Text = "업로드";
            btnUpload.UseVisualStyleBackColor = false;
            btnUpload.Click += btnUpload_Click;
            // 
            // Upload
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(16, 16, 16);
            ClientSize = new Size(877, 484);
            Controls.Add(btnUpload);
            Controls.Add(btnFindMusic);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(tbSongFile);
            Controls.Add(tbArtist);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(tbTitle);
            Controls.Add(label1);
            Controls.Add(picAlbum);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Upload";
            Text = "Upload";
            ((System.ComponentModel.ISupportInitialize)picAlbum).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picAlbum;
        private Label label1;
        private TextBox tbTitle;
        private Panel panel1;
        private Label label2;
        private TextBox tbArtist;
        private Panel panel2;
        private TextBox tbSongFile;
        private Panel panel3;
        private Button btnFindMusic;
        private Label label3;
        private Button btnUpload;
    }
}