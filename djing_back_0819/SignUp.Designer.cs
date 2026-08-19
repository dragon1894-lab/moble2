namespace DJing
{
    partial class SignUp
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            tb_Name = new TextBox();
            tb_Number = new TextBox();
            tb_ID = new TextBox();
            tb_PW = new TextBox();
            tb_PWCheck = new TextBox();
            groupBox1 = new GroupBox();
            rb_Yes = new RadioButton();
            rb_No = new RadioButton();
            bt_Continue = new Button();
            bt_CheckID = new Button();
            lb_PWresult = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(172, 19);
            label1.Margin = new Padding(0);
            label1.Name = "label1";
            label1.Size = new Size(134, 37);
            label1.TabIndex = 0;
            label1.Text = "회원 가입";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(63, 90);
            label2.Margin = new Padding(0);
            label2.Name = "label2";
            label2.Size = new Size(53, 21);
            label2.TabIndex = 0;
            label2.Text = "Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = Color.White;
            label3.Location = new Point(63, 158);
            label3.Margin = new Padding(0);
            label3.Name = "label3";
            label3.Size = new Size(122, 21);
            label3.TabIndex = 1;
            label3.Text = "Phone Number";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.ForeColor = Color.White;
            label4.Location = new Point(63, 232);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Size = new Size(89, 21);
            label4.TabIndex = 1;
            label4.Text = "User name";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = Color.White;
            label5.Location = new Point(63, 308);
            label5.Margin = new Padding(0);
            label5.Name = "label5";
            label5.Size = new Size(79, 21);
            label5.TabIndex = 1;
            label5.Text = "Password";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label6.ForeColor = Color.White;
            label6.Location = new Point(63, 382);
            label6.Margin = new Padding(0);
            label6.Name = "label6";
            label6.Size = new Size(143, 21);
            label6.TabIndex = 1;
            label6.Text = "Confirm Password";
            // 
            // tb_Name
            // 
            tb_Name.Location = new Point(63, 112);
            tb_Name.Margin = new Padding(0);
            tb_Name.Name = "tb_Name";
            tb_Name.Size = new Size(200, 23);
            tb_Name.TabIndex = 1;
            // 
            // tb_Number
            // 
            tb_Number.Location = new Point(63, 180);
            tb_Number.Margin = new Padding(0);
            tb_Number.Name = "tb_Number";
            tb_Number.Size = new Size(200, 23);
            tb_Number.TabIndex = 2;
            // 
            // tb_ID
            // 
            tb_ID.Location = new Point(63, 255);
            tb_ID.Margin = new Padding(0);
            tb_ID.Name = "tb_ID";
            tb_ID.Size = new Size(200, 23);
            tb_ID.TabIndex = 3;
            // 
            // tb_PW
            // 
            tb_PW.Location = new Point(63, 330);
            tb_PW.Margin = new Padding(0);
            tb_PW.Name = "tb_PW";
            tb_PW.Size = new Size(200, 23);
            tb_PW.TabIndex = 5;
            tb_PW.UseSystemPasswordChar = true;
            // 
            // tb_PWCheck
            // 
            tb_PWCheck.Location = new Point(63, 405);
            tb_PWCheck.Margin = new Padding(0);
            tb_PWCheck.Name = "tb_PWCheck";
            tb_PWCheck.Size = new Size(200, 23);
            tb_PWCheck.TabIndex = 6;
            tb_PWCheck.UseSystemPasswordChar = true;
            tb_PWCheck.TextChanged += tb_PWCheck_TextChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rb_Yes);
            groupBox1.Controls.Add(rb_No);
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(63, 480);
            groupBox1.Margin = new Padding(0);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(0);
            groupBox1.Size = new Size(343, 68);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "개인정보 수집에 동의하시겠습니까?";
            // 
            // rb_Yes
            // 
            rb_Yes.AutoSize = true;
            rb_Yes.ForeColor = Color.White;
            rb_Yes.Location = new Point(77, 31);
            rb_Yes.Margin = new Padding(0);
            rb_Yes.Name = "rb_Yes";
            rb_Yes.Size = new Size(37, 19);
            rb_Yes.TabIndex = 0;
            rb_Yes.TabStop = true;
            rb_Yes.Text = "예";
            rb_Yes.UseVisualStyleBackColor = true;
            // 
            // rb_No
            // 
            rb_No.AutoSize = true;
            rb_No.ForeColor = Color.White;
            rb_No.Location = new Point(203, 31);
            rb_No.Margin = new Padding(0);
            rb_No.Name = "rb_No";
            rb_No.Size = new Size(61, 19);
            rb_No.TabIndex = 1;
            rb_No.TabStop = true;
            rb_No.Text = "아니오";
            rb_No.UseVisualStyleBackColor = true;
            // 
            // bt_Continue
            // 
            bt_Continue.BackColor = Color.FromArgb(255, 85, 0);
            bt_Continue.FlatStyle = FlatStyle.Popup;
            bt_Continue.Font = new Font("맑은 고딕", 11F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Continue.ForeColor = Color.White;
            bt_Continue.Location = new Point(140, 585);
            bt_Continue.Margin = new Padding(0);
            bt_Continue.Name = "bt_Continue";
            bt_Continue.Size = new Size(203, 45);
            bt_Continue.TabIndex = 9;
            bt_Continue.Text = "가입하기";
            bt_Continue.UseVisualStyleBackColor = false;
            bt_Continue.Click += bt_Continue_Click;
            // 
            // bt_CheckID
            // 
            bt_CheckID.BackColor = Color.FromArgb(60, 60, 60);
            bt_CheckID.FlatStyle = FlatStyle.Popup;
            bt_CheckID.ForeColor = Color.White;
            bt_CheckID.Location = new Point(294, 255);
            bt_CheckID.Margin = new Padding(0);
            bt_CheckID.Name = "bt_CheckID";
            bt_CheckID.Size = new Size(77, 22);
            bt_CheckID.TabIndex = 4;
            bt_CheckID.Text = "중복확인";
            bt_CheckID.UseVisualStyleBackColor = false;
            bt_CheckID.Click += bt_CheckID_Click;
            // 
            // lb_PWresult
            // 
            lb_PWresult.AutoSize = true;
            lb_PWresult.Location = new Point(294, 412);
            lb_PWresult.Margin = new Padding(0);
            lb_PWresult.Name = "lb_PWresult";
            lb_PWresult.Size = new Size(0, 15);
            lb_PWresult.TabIndex = 7;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(16, 16, 16);
            ClientSize = new Size(476, 682);
            Controls.Add(lb_PWresult);
            Controls.Add(bt_CheckID);
            Controls.Add(bt_Continue);
            Controls.Add(groupBox1);
            Controls.Add(tb_PWCheck);
            Controls.Add(tb_PW);
            Controls.Add(tb_ID);
            Controls.Add(tb_Number);
            Controls.Add(tb_Name);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(0);
            Name = "SignUp";
            Text = "Form2";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox tb_Name;
        private TextBox tb_Number;
        private TextBox tb_ID;
        private TextBox tb_PW;
        private TextBox tb_PWCheck;
        private GroupBox groupBox1;
        private RadioButton rb_Yes;
        private RadioButton rb_No;
        private Button bt_Continue;
        private Button bt_CheckID;
        private Label lb_PWresult;
    }
}

