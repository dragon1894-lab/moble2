namespace login1_0811
{
    partial class Form1
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
            label1 = new Label();
            label2 = new Label();
            tb_ID = new TextBox();
            label3 = new Label();
            tb_PW = new TextBox();
            bt_Login = new Button();
            bt_Create = new Button();
            bt_Guest = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 27.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(331, 181);
            label1.Name = "label1";
            label1.Size = new Size(314, 44);
            label1.TabIndex = 0;
            label1.Text = "SOUNDCLOUND";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(291, 270);
            label2.Name = "label2";
            label2.Size = new Size(25, 20);
            label2.TabIndex = 6;
            label2.Text = "ID";
            // 
            // tb_ID
            // 
            tb_ID.Location = new Point(350, 270);
            tb_ID.Multiline = true;
            tb_ID.Name = "tb_ID";
            tb_ID.Size = new Size(270, 23);
            tb_ID.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
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
            tb_PW.TabIndex = 4;
            tb_PW.UseSystemPasswordChar = true;
            // 
            // bt_Login
            // 
            bt_Login.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Login.Location = new Point(350, 378);
            bt_Login.Name = "bt_Login";
            bt_Login.Size = new Size(270, 44);
            bt_Login.TabIndex = 7;
            bt_Login.Text = "Login";
            bt_Login.UseVisualStyleBackColor = true;
            bt_Login.Click += bt_Login_Click;
            // 
            // bt_Create
            // 
            bt_Create.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Create.Location = new Point(350, 428);
            bt_Create.Name = "bt_Create";
            bt_Create.Size = new Size(132, 44);
            bt_Create.TabIndex = 7;
            bt_Create.Text = "Create account";
            bt_Create.UseVisualStyleBackColor = true;
            bt_Create.Click += bt_Create_Click;
            // 
            // bt_Guest
            // 
            bt_Guest.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Guest.Location = new Point(488, 428);
            bt_Guest.Name = "bt_Guest";
            bt_Guest.Size = new Size(132, 44);
            bt_Guest.TabIndex = 7;
            bt_Guest.Text = "Guest";
            bt_Guest.UseVisualStyleBackColor = true;
            bt_Guest.Click += bt_Guest_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(965, 685);
            Controls.Add(bt_Guest);
            Controls.Add(bt_Create);
            Controls.Add(bt_Login);
            Controls.Add(tb_PW);
            Controls.Add(label3);
            Controls.Add(tb_ID);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Form1";
            Text = "SOUNDCLOUND";
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
    }
}

