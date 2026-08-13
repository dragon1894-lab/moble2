namespace login1_0811
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            bt_Icon1 = new Button();
            bt_Icon2 = new Button();
            bt_Icon4 = new Button();
            bt_Icon3 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            tb_CurrentPW = new TextBox();
            label4 = new Label();
            tb_NewPW = new TextBox();
            label5 = new Label();
            tb_NewPWCheck = new TextBox();
            lb_PWresult = new Label();
            bt_ChangePW = new Button();
            label6 = new Label();
            tb_Number = new TextBox();
            bt_ChangeNumber = new Button();
            bt_Close = new Button();
            SuspendLayout();
            // 
            // bt_Icon1
            // 
            bt_Icon1.BackgroundImage = (Image)resources.GetObject("bt_Icon1.BackgroundImage");
            bt_Icon1.BackgroundImageLayout = ImageLayout.Zoom;
            bt_Icon1.Location = new Point(40, 40);
            bt_Icon1.Name = "bt_Icon1";
            bt_Icon1.Size = new Size(160, 160);
            bt_Icon1.TabIndex = 0;
            bt_Icon1.UseVisualStyleBackColor = false;
            bt_Icon1.Click += bt_Icon1_Click;
            // 
            // bt_Icon2
            // 
            bt_Icon2.BackgroundImage = (Image)resources.GetObject("bt_Icon2.BackgroundImage");
            bt_Icon2.BackgroundImageLayout = ImageLayout.Zoom;
            bt_Icon2.Location = new Point(220, 40);
            bt_Icon2.Name = "bt_Icon2";
            bt_Icon2.Size = new Size(160, 160);
            bt_Icon2.TabIndex = 0;
            bt_Icon2.UseVisualStyleBackColor = false;
            bt_Icon2.Click += bt_Icon2_Click;
            // 
            // bt_Icon4
            // 
            bt_Icon4.BackgroundImage = (Image)resources.GetObject("bt_Icon4.BackgroundImage");
            bt_Icon4.BackgroundImageLayout = ImageLayout.Zoom;
            bt_Icon4.Location = new Point(220, 220);
            bt_Icon4.Name = "bt_Icon4";
            bt_Icon4.Size = new Size(160, 160);
            bt_Icon4.TabIndex = 0;
            bt_Icon4.UseVisualStyleBackColor = false;
            bt_Icon4.Click += bt_Icon4_Click;
            // 
            // bt_Icon3
            // 
            bt_Icon3.BackgroundImage = (Image)resources.GetObject("bt_Icon3.BackgroundImage");
            bt_Icon3.BackgroundImageLayout = ImageLayout.Zoom;
            bt_Icon3.Location = new Point(40, 220);
            bt_Icon3.Name = "bt_Icon3";
            bt_Icon3.Size = new Size(160, 160);
            bt_Icon3.TabIndex = 0;
            bt_Icon3.UseVisualStyleBackColor = false;
            bt_Icon3.Click += bt_Icon3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(40, 9);
            label1.Name = "label1";
            label1.Size = new Size(175, 21);
            label1.TabIndex = 1;
            label1.Text = "Choose a Profile Icon";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(40, 405);
            label2.Name = "label2";
            label2.Size = new Size(205, 21);
            label2.TabIndex = 2;
            label2.Text = "Edit Account Information";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(40, 447);
            label3.Name = "label3";
            label3.Size = new Size(115, 17);
            label3.TabIndex = 3;
            label3.Text = "Current Password";
            // 
            // tb_CurrentPW
            // 
            tb_CurrentPW.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            tb_CurrentPW.Location = new Point(40, 472);
            tb_CurrentPW.Name = "tb_CurrentPW";
            tb_CurrentPW.Size = new Size(205, 25);
            tb_CurrentPW.TabIndex = 4;
            tb_CurrentPW.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(40, 512);
            label4.Name = "label4";
            label4.Size = new Size(97, 17);
            label4.TabIndex = 5;
            label4.Text = "New Password";
            // 
            // tb_NewPW
            // 
            tb_NewPW.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            tb_NewPW.Location = new Point(40, 537);
            tb_NewPW.Name = "tb_NewPW";
            tb_NewPW.Size = new Size(205, 25);
            tb_NewPW.TabIndex = 6;
            tb_NewPW.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(40, 577);
            label5.Name = "label5";
            label5.Size = new Size(149, 17);
            label5.TabIndex = 7;
            label5.Text = "Confirm New Password";
            // 
            // tb_NewPWCheck
            // 
            tb_NewPWCheck.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            tb_NewPWCheck.Location = new Point(40, 602);
            tb_NewPWCheck.Name = "tb_NewPWCheck";
            tb_NewPWCheck.Size = new Size(205, 25);
            tb_NewPWCheck.TabIndex = 8;
            tb_NewPWCheck.UseSystemPasswordChar = true;
            tb_NewPWCheck.TextChanged += tb_NewPWCheck_TextChanged;
            // 
            // lb_PWresult
            // 
            lb_PWresult.AutoSize = true;
            lb_PWresult.Location = new Point(260, 604);
            lb_PWresult.Name = "lb_PWresult";
            lb_PWresult.Size = new Size(0, 15);
            lb_PWresult.TabIndex = 9;
            // 
            // bt_ChangePW
            // 
            bt_ChangePW.Location = new Point(40, 648);
            bt_ChangePW.Name = "bt_ChangePW";
            bt_ChangePW.Size = new Size(205, 23);
            bt_ChangePW.TabIndex = 10;
            bt_ChangePW.Text = "Change Password";
            bt_ChangePW.UseVisualStyleBackColor = true;
            bt_ChangePW.Click += bt_ChangePW_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(40, 704);
            label6.Name = "label6";
            label6.Size = new Size(100, 17);
            label6.TabIndex = 11;
            label6.Text = "Phone Number";
            // 
            // tb_Number
            // 
            tb_Number.Font = new Font("맑은 고딕", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            tb_Number.Location = new Point(40, 729);
            tb_Number.Name = "tb_Number";
            tb_Number.Size = new Size(205, 25);
            tb_Number.TabIndex = 12;
            // 
            // bt_ChangeNumber
            // 
            bt_ChangeNumber.Location = new Point(40, 775);
            bt_ChangeNumber.Name = "bt_ChangeNumber";
            bt_ChangeNumber.Size = new Size(205, 23);
            bt_ChangeNumber.TabIndex = 13;
            bt_ChangeNumber.Text = "Change Phone Number";
            bt_ChangeNumber.UseVisualStyleBackColor = true;
            bt_ChangeNumber.Click += bt_ChangeNumber_Click;
            // 
            // bt_Close
            // 
            bt_Close.Location = new Point(281, 864);
            bt_Close.Name = "bt_Close";
            bt_Close.Size = new Size(99, 26);
            bt_Close.TabIndex = 14;
            bt_Close.Text = "Close";
            bt_Close.UseVisualStyleBackColor = true;
            bt_Close.Click += bt_Close_Click;
            // 
            // Form4
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(444, 944);
            Controls.Add(bt_Close);
            Controls.Add(bt_ChangeNumber);
            Controls.Add(tb_Number);
            Controls.Add(label6);
            Controls.Add(bt_ChangePW);
            Controls.Add(lb_PWresult);
            Controls.Add(tb_NewPWCheck);
            Controls.Add(label5);
            Controls.Add(tb_NewPW);
            Controls.Add(label4);
            Controls.Add(tb_CurrentPW);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(bt_Icon4);
            Controls.Add(bt_Icon2);
            Controls.Add(bt_Icon3);
            Controls.Add(bt_Icon1);
            Name = "Form4";
            Text = "Form4";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bt_Icon1;
        private Button bt_Icon2;
        private Button bt_Icon4;
        private Button bt_Icon3;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox tb_CurrentPW;
        private Label label4;
        private TextBox tb_NewPW;
        private Label label5;
        private TextBox tb_NewPWCheck;
        private Label lb_PWresult;
        private Button bt_ChangePW;
        private Label label6;
        private TextBox tb_Number;
        private Button bt_ChangeNumber;
        private Button bt_Close;
    }
}
