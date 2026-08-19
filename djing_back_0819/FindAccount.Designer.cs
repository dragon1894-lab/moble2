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
            lb_FindIDResult = new Label();
            bt_FindID = new Button();
            tb_FindPhone = new TextBox();
            lb_FindPhone = new Label();
            tb_FindName = new TextBox();
            lb_FindName = new Label();
            tabResetPW = new TabPage();
            lb_PWResult = new Label();
            bt_ResetPW = new Button();
            tb_NewPWCheck = new TextBox();
            lb_NewPWCheck = new Label();
            tb_NewPW = new TextBox();
            lb_NewPW = new Label();
            tb_ResetPhone = new TextBox();
            lb_ResetPhone = new Label();
            tb_ResetName = new TextBox();
            lb_ResetName = new Label();
            tb_ResetID = new TextBox();
            lb_ResetID = new Label();
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
            tabFindID.Controls.Add(lb_FindIDResult);
            tabFindID.Controls.Add(bt_FindID);
            tabFindID.Controls.Add(tb_FindPhone);
            tabFindID.Controls.Add(lb_FindPhone);
            tabFindID.Controls.Add(tb_FindName);
            tabFindID.Controls.Add(lb_FindName);
            tabFindID.Location = new Point(4, 26);
            tabFindID.Name = "tabFindID";
            tabFindID.Padding = new Padding(3);
            tabFindID.Size = new Size(562, 410);
            tabFindID.TabIndex = 0;
            tabFindID.Text = "아이디 찾기";
            // 
            // lb_FindIDResult
            // 
            lb_FindIDResult.Font = new Font("맑은 고딕", 11F, FontStyle.Bold, GraphicsUnit.Point);
            lb_FindIDResult.ForeColor = Color.White;
            lb_FindIDResult.Location = new Point(80, 270);
            lb_FindIDResult.Name = "lb_FindIDResult";
            lb_FindIDResult.Size = new Size(400, 45);
            lb_FindIDResult.TabIndex = 5;
            lb_FindIDResult.TextAlign = ContentAlignment.MiddleCenter;
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
            // lb_FindPhone
            // 
            lb_FindPhone.AutoSize = true;
            lb_FindPhone.ForeColor = Color.White;
            lb_FindPhone.Location = new Point(80, 138);
            lb_FindPhone.Name = "lb_FindPhone";
            lb_FindPhone.Size = new Size(69, 19);
            lb_FindPhone.TabIndex = 2;
            lb_FindPhone.Text = "전화번호";
            // 
            // tb_FindName
            // 
            tb_FindName.Location = new Point(200, 85);
            tb_FindName.Name = "tb_FindName";
            tb_FindName.Size = new Size(280, 25);
            tb_FindName.TabIndex = 0;
            // 
            // lb_FindName
            // 
            lb_FindName.AutoSize = true;
            lb_FindName.ForeColor = Color.White;
            lb_FindName.Location = new Point(80, 88);
            lb_FindName.Name = "lb_FindName";
            lb_FindName.Size = new Size(37, 19);
            lb_FindName.TabIndex = 0;
            lb_FindName.Text = "이름";
            // 
            // tabResetPW
            // 
            tabResetPW.BackColor = Color.FromArgb(30, 30, 30);
            tabResetPW.Controls.Add(lb_PWResult);
            tabResetPW.Controls.Add(bt_ResetPW);
            tabResetPW.Controls.Add(tb_NewPWCheck);
            tabResetPW.Controls.Add(lb_NewPWCheck);
            tabResetPW.Controls.Add(tb_NewPW);
            tabResetPW.Controls.Add(lb_NewPW);
            tabResetPW.Controls.Add(tb_ResetPhone);
            tabResetPW.Controls.Add(lb_ResetPhone);
            tabResetPW.Controls.Add(tb_ResetName);
            tabResetPW.Controls.Add(lb_ResetName);
            tabResetPW.Controls.Add(tb_ResetID);
            tabResetPW.Controls.Add(lb_ResetID);
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
            // lb_NewPWCheck
            // 
            lb_NewPWCheck.AutoSize = true;
            lb_NewPWCheck.ForeColor = Color.White;
            lb_NewPWCheck.Location = new Point(60, 248);
            lb_NewPWCheck.Name = "lb_NewPWCheck";
            lb_NewPWCheck.Size = new Size(113, 19);
            lb_NewPWCheck.TabIndex = 8;
            lb_NewPWCheck.Text = "새 비밀번호 확인";
            // 
            // tb_NewPW
            // 
            tb_NewPW.Location = new Point(200, 195);
            tb_NewPW.Name = "tb_NewPW";
            tb_NewPW.Size = new Size(300, 25);
            tb_NewPW.TabIndex = 3;
            tb_NewPW.UseSystemPasswordChar = true;
            // 
            // lb_NewPW
            // 
            lb_NewPW.AutoSize = true;
            lb_NewPW.ForeColor = Color.White;
            lb_NewPW.Location = new Point(60, 198);
            lb_NewPW.Name = "lb_NewPW";
            lb_NewPW.Size = new Size(85, 19);
            lb_NewPW.TabIndex = 6;
            lb_NewPW.Text = "새 비밀번호";
            // 
            // tb_ResetPhone
            // 
            tb_ResetPhone.Location = new Point(200, 145);
            tb_ResetPhone.Name = "tb_ResetPhone";
            tb_ResetPhone.Size = new Size(300, 25);
            tb_ResetPhone.TabIndex = 2;
            // 
            // lb_ResetPhone
            // 
            lb_ResetPhone.AutoSize = true;
            lb_ResetPhone.ForeColor = Color.White;
            lb_ResetPhone.Location = new Point(60, 148);
            lb_ResetPhone.Name = "lb_ResetPhone";
            lb_ResetPhone.Size = new Size(69, 19);
            lb_ResetPhone.TabIndex = 4;
            lb_ResetPhone.Text = "전화번호";
            // 
            // tb_ResetName
            // 
            tb_ResetName.Location = new Point(200, 95);
            tb_ResetName.Name = "tb_ResetName";
            tb_ResetName.Size = new Size(300, 25);
            tb_ResetName.TabIndex = 1;
            // 
            // lb_ResetName
            // 
            lb_ResetName.AutoSize = true;
            lb_ResetName.ForeColor = Color.White;
            lb_ResetName.Location = new Point(60, 98);
            lb_ResetName.Name = "lb_ResetName";
            lb_ResetName.Size = new Size(37, 19);
            lb_ResetName.TabIndex = 2;
            lb_ResetName.Text = "이름";
            // 
            // tb_ResetID
            // 
            tb_ResetID.Location = new Point(200, 45);
            tb_ResetID.Name = "tb_ResetID";
            tb_ResetID.Size = new Size(300, 25);
            tb_ResetID.TabIndex = 0;
            // 
            // lb_ResetID
            // 
            lb_ResetID.AutoSize = true;
            lb_ResetID.ForeColor = Color.White;
            lb_ResetID.Location = new Point(60, 48);
            lb_ResetID.Name = "lb_ResetID";
            lb_ResetID.Size = new Size(53, 19);
            lb_ResetID.TabIndex = 0;
            lb_ResetID.Text = "아이디";
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
        private Label lb_FindName;
        private TextBox tb_FindName;
        private Label lb_FindPhone;
        private TextBox tb_FindPhone;
        private Button bt_FindID;
        private Label lb_FindIDResult;
        private Label lb_ResetID;
        private TextBox tb_ResetID;
        private Label lb_ResetName;
        private TextBox tb_ResetName;
        private Label lb_ResetPhone;
        private TextBox tb_ResetPhone;
        private Label lb_NewPW;
        private TextBox tb_NewPW;
        private Label lb_NewPWCheck;
        private TextBox tb_NewPWCheck;
        private Button bt_ResetPW;
        private Label lb_PWResult;
    }
}


