namespace DJing
{
    partial class FindAccount
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabAccount = new TabControl();
            tabFindID = new TabPage();
            lb_IDResult = new Label();
            bt_FindID = new Button();
            tb_FindPhone = new TextBox();
            label2 = new Label();
            tb_FindName = new TextBox();
            label1 = new Label();
            tabResetPW = new TabPage();
            lb_PWResult = new Label();
            bt_ResetPW = new Button();
            tb_NewPWCheck = new TextBox();
            label7 = new Label();
            tb_NewPW = new TextBox();
            label6 = new Label();
            tb_ResetPhone = new TextBox();
            label5 = new Label();
            tb_ResetName = new TextBox();
            label4 = new Label();
            tb_ResetID = new TextBox();
            label3 = new Label();
            tabAccount.SuspendLayout();
            tabFindID.SuspendLayout();
            tabResetPW.SuspendLayout();
            SuspendLayout();
            // 
            // tabAccount
            // 
            tabAccount.Controls.Add(tabFindID);
            tabAccount.Controls.Add(tabResetPW);
            tabAccount.Font = new Font("맑은 고딕", 10F, FontStyle.Bold, GraphicsUnit.Point);
            tabAccount.Location = new Point(25, 25);
            tabAccount.Name = "tabAccount";
            tabAccount.SelectedIndex = 0;
            tabAccount.Size = new Size(570, 440);
            tabAccount.TabIndex = 0;
            // 
            // tabFindID
            // 
            tabFindID.BackColor = Color.FromArgb(30, 30, 30);
            tabFindID.Controls.Add(lb_IDResult);
            tabFindID.Controls.Add(bt_FindID);
            tabFindID.Controls.Add(tb_FindPhone);
            tabFindID.Controls.Add(label2);
            tabFindID.Controls.Add(tb_FindName);
            tabFindID.Controls.Add(label1);
            tabFindID.Location = new Point(4, 26);
            tabFindID.Name = "tabFindID";
            tabFindID.Padding = new Padding(3);
            tabFindID.Size = new Size(562, 410);
            tabFindID.TabIndex = 0;
            tabFindID.Text = "아이디 찾기";
            // 
            // lb_IDResult
            // 
            lb_IDResult.Font = new Font("맑은 고딕", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lb_IDResult.ForeColor = Color.White;
            lb_IDResult.Location = new Point(80, 270);
            lb_IDResult.Name = "lb_IDResult";
            lb_IDResult.Size = new Size(400, 45);
            lb_IDResult.TabIndex = 5;
            lb_IDResult.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // bt_FindID
            // 
            bt_FindID.BackColor = Color.FromArgb(255, 85, 0);
            bt_FindID.FlatStyle = FlatStyle.Popup;
            bt_FindID.ForeColor = Color.White;
            bt_FindID.Location = new Point(200, 200);
            bt_FindID.Name = "bt_FindID";
            bt_FindID.Size = new Size(280, 42);
            bt_FindID.TabIndex = 2;
            bt_FindID.Text = "아이디 찾기";
            bt_FindID.UseVisualStyleBackColor = false;
            bt_FindID.Click += bt_FindID_Click;
            // 
            // tb_FindPhone
            // 
            tb_FindPhone.Location = new Point(200, 135);
            tb_FindPhone.Name = "tb_FindPhone";
            tb_FindPhone.Size = new Size(280, 25);
            tb_FindPhone.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(80, 138);
            label2.Name = "label2";
            label2.Size = new Size(69, 19);
            label2.TabIndex = 2;
            label2.Text = "전화번호";
            // 
            // tb_FindName
            // 
            tb_FindName.Location = new Point(200, 85);
            tb_FindName.Name = "tb_FindName";
            tb_FindName.Size = new Size(280, 25);
            tb_FindName.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(80, 88);
            label1.Name = "label1";
            label1.Size = new Size(37, 19);
            label1.TabIndex = 0;
            label1.Text = "이름";
            // 
            // tabResetPW
            // 
            tabResetPW.BackColor = Color.FromArgb(30, 30, 30);
            tabResetPW.Controls.Add(lb_PWResult);
            tabResetPW.Controls.Add(bt_ResetPW);
            tabResetPW.Controls.Add(tb_NewPWCheck);
            tabResetPW.Controls.Add(label7);
            tabResetPW.Controls.Add(tb_NewPW);
            tabResetPW.Controls.Add(label6);
            tabResetPW.Controls.Add(tb_ResetPhone);
            tabResetPW.Controls.Add(label5);
            tabResetPW.Controls.Add(tb_ResetName);
            tabResetPW.Controls.Add(label4);
            tabResetPW.Controls.Add(tb_ResetID);
            tabResetPW.Controls.Add(label3);
            tabResetPW.Location = new Point(4, 26);
            tabResetPW.Name = "tabResetPW";
            tabResetPW.Padding = new Padding(3);
            tabResetPW.Size = new Size(562, 410);
            tabResetPW.TabIndex = 1;
            tabResetPW.Text = "비밀번호 재설정";
            // 
            // lb_PWResult
            // 
            lb_PWResult.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lb_PWResult.ForeColor = Color.White;
            lb_PWResult.Location = new Point(200, 278);
            lb_PWResult.Name = "lb_PWResult";
            lb_PWResult.Size = new Size(300, 25);
            lb_PWResult.TabIndex = 11;
            // 
            // bt_ResetPW
            // 
            bt_ResetPW.BackColor = Color.FromArgb(255, 85, 0);
            bt_ResetPW.FlatStyle = FlatStyle.Popup;
            bt_ResetPW.ForeColor = Color.White;
            bt_ResetPW.Location = new Point(200, 325);
            bt_ResetPW.Name = "bt_ResetPW";
            bt_ResetPW.Size = new Size(300, 42);
            bt_ResetPW.TabIndex = 5;
            bt_ResetPW.Text = "비밀번호 변경";
            bt_ResetPW.UseVisualStyleBackColor = false;
            bt_ResetPW.Click += bt_ResetPW_Click;
            // 
            // tb_NewPWCheck
            // 
            tb_NewPWCheck.Location = new Point(200, 245);
            tb_NewPWCheck.Name = "tb_NewPWCheck";
            tb_NewPWCheck.Size = new Size(300, 25);
            tb_NewPWCheck.TabIndex = 4;
            tb_NewPWCheck.UseSystemPasswordChar = true;
            tb_NewPWCheck.TextChanged += tb_NewPWCheck_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.White;
            label7.Location = new Point(60, 248);
            label7.Name = "label7";
            label7.Size = new Size(113, 19);
            label7.TabIndex = 8;
            label7.Text = "새 비밀번호 확인";
            // 
            // tb_NewPW
            // 
            tb_NewPW.Location = new Point(200, 195);
            tb_NewPW.Name = "tb_NewPW";
            tb_NewPW.Size = new Size(300, 25);
            tb_NewPW.TabIndex = 3;
            tb_NewPW.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.White;
            label6.Location = new Point(60, 198);
            label6.Name = "label6";
            label6.Size = new Size(85, 19);
            label6.TabIndex = 6;
            label6.Text = "새 비밀번호";
            // 
            // tb_ResetPhone
            // 
            tb_ResetPhone.Location = new Point(200, 145);
            tb_ResetPhone.Name = "tb_ResetPhone";
            tb_ResetPhone.Size = new Size(300, 25);
            tb_ResetPhone.TabIndex = 2;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(60, 148);
            label5.Name = "label5";
            label5.Size = new Size(69, 19);
            label5.TabIndex = 4;
            label5.Text = "전화번호";
            // 
            // tb_ResetName
            // 
            tb_ResetName.Location = new Point(200, 95);
            tb_ResetName.Name = "tb_ResetName";
            tb_ResetName.Size = new Size(300, 25);
            tb_ResetName.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(60, 98);
            label4.Name = "label4";
            label4.Size = new Size(37, 19);
            label4.TabIndex = 2;
            label4.Text = "이름";
            // 
            // tb_ResetID
            // 
            tb_ResetID.Location = new Point(200, 45);
            tb_ResetID.Name = "tb_ResetID";
            tb_ResetID.Size = new Size(300, 25);
            tb_ResetID.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.White;
            label3.Location = new Point(60, 48);
            label3.Name = "label3";
            label3.Size = new Size(53, 19);
            label3.TabIndex = 0;
            label3.Text = "아이디";
            // 
            // FindAccount
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(16, 16, 16);
            ClientSize = new Size(620, 500);
            Controls.Add(tabAccount);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FindAccount";
            StartPosition = FormStartPosition.CenterParent;
            Text = "아이디 / 비밀번호 찾기";
            tabAccount.ResumeLayout(false);
            tabFindID.ResumeLayout(false);
            tabFindID.PerformLayout();
            tabResetPW.ResumeLayout(false);
            tabResetPW.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabAccount;
        private TabPage tabFindID;
        private TabPage tabResetPW;
        private Label label1;
        private TextBox tb_FindName;
        private Label label2;
        private TextBox tb_FindPhone;
        private Button bt_FindID;
        private Label lb_IDResult;
        private Label label3;
        private TextBox tb_ResetID;
        private Label label4;
        private TextBox tb_ResetName;
        private Label label5;
        private TextBox tb_ResetPhone;
        private Label label6;
        private TextBox tb_NewPW;
        private Label label7;
        private TextBox tb_NewPWCheck;
        private Button bt_ResetPW;
        private Label lb_PWResult;
    }
}
