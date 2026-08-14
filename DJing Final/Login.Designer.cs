namespace DJing
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            label1 = new Label();
            label2 = new Label();
            tb_ID = new TextBox();
            label3 = new Label();
            tb_PW = new TextBox();
            bt_Login = new Button();
            bt_Create = new Button();
            bt_Guest = new Button();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 27.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(359, 181);
            label1.Name = "label1";
            label1.Size = new Size(297, 44);
            label1.TabIndex = 0;
            label1.Text = "SOUND KLOUD";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(291, 270);
            label2.Name = "label2";
            label2.Size = new Size(25, 20);
            label2.TabIndex = 6;
            label2.Text = "ID";
            // 
            // tb_ID
            // 
            tb_ID.Location = new Point(350, 270);
            tb_ID.Name = "tb_ID";
            tb_ID.Size = new Size(270, 23);
            tb_ID.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.ForeColor = Color.White;
            label3.Location = new Point(291, 328);
            label3.Name = "label3";
            label3.Size = new Size(33, 20);
            label3.TabIndex = 6;
            label3.Text = "PW";
            // 
            // tb_PW
            // 
            tb_PW.Location = new Point(350, 328);
            tb_PW.Name = "tb_PW";
            tb_PW.Size = new Size(270, 23);
            tb_PW.TabIndex = 1;
            tb_PW.UseSystemPasswordChar = true;
            // 
            // bt_Login
            // 
            bt_Login.BackColor = Color.FromArgb(255, 85, 0);
            bt_Login.FlatStyle = FlatStyle.Popup;
            bt_Login.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Login.ForeColor = Color.White;
            bt_Login.Location = new Point(350, 378);
            bt_Login.Name = "bt_Login";
            bt_Login.Size = new Size(270, 44);
            bt_Login.TabIndex = 2;
            bt_Login.Text = "로그인";
            bt_Login.UseVisualStyleBackColor = false;
            bt_Login.Click += bt_Login_Click;
            // 
            // bt_Create
            // 
            bt_Create.BackColor = Color.FromArgb(60, 60, 60);
            bt_Create.FlatStyle = FlatStyle.Popup;
            bt_Create.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Create.ForeColor = Color.White;
            bt_Create.Location = new Point(350, 428);
            bt_Create.Name = "bt_Create";
            bt_Create.Size = new Size(132, 44);
            bt_Create.TabIndex = 3;
            bt_Create.Text = "회원가입";
            bt_Create.UseVisualStyleBackColor = false;
            bt_Create.Click += bt_Create_Click;
            // 
            // bt_Guest
            // 
            bt_Guest.BackColor = Color.FromArgb(60, 60, 60);
            bt_Guest.FlatStyle = FlatStyle.Popup;
            bt_Guest.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Guest.ForeColor = Color.White;
            bt_Guest.Location = new Point(488, 428);
            bt_Guest.Name = "bt_Guest";
            bt_Guest.Size = new Size(132, 44);
            bt_Guest.TabIndex = 4;
            bt_Guest.Text = "게스트로 시작";
            bt_Guest.UseVisualStyleBackColor = false;
            bt_Guest.Click += bt_Guest_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(16, 16, 16);
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(270, 156);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(104, 91);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // Login
            // 
            AcceptButton = bt_Login;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(16, 16, 16);
            ClientSize = new Size(965, 685);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(bt_Guest);
            Controls.Add(bt_Create);
            Controls.Add(bt_Login);
            Controls.Add(tb_PW);
            Controls.Add(label3);
            Controls.Add(tb_ID);
            Controls.Add(label2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Login";
            Text = "SOUNDCLOUND";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox tb_ID;
        private Label label3;
        private TextBox tb_PW;
        private Button bt_Login;
        private Button bt_Create;
        private Button bt_Guest;
        private PictureBox pictureBox1;
    }
}

