namespace login1_0811
{
    partial class Form3
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            pb_Profile = new PictureBox();
            bt_Icon = new Button();
            bt_Mixing = new Button();
            bt_SoundCloud = new Button();
            lb_UserName = new Label();
            ((System.ComponentModel.ISupportInitialize)pb_Profile).BeginInit();
            SuspendLayout();
            // 
            // pb_Profile
            // 
            pb_Profile.Image = (Image)resources.GetObject("pb_Profile.Image");
            pb_Profile.Location = new Point(220, 67);
            pb_Profile.Name = "pb_Profile";
            pb_Profile.Size = new Size(160, 160);
            pb_Profile.SizeMode = PictureBoxSizeMode.Zoom;
            pb_Profile.TabIndex = 0;
            pb_Profile.TabStop = false;
            // 
            // bt_Icon
            // 
            bt_Icon.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Icon.Location = new Point(220, 38);
            bt_Icon.Name = "bt_Icon";
            bt_Icon.Size = new Size(160, 23);
            bt_Icon.TabIndex = 1;
            bt_Icon.Text = "Change avatar";
            bt_Icon.UseVisualStyleBackColor = true;
            bt_Icon.Click += bt_Icon_Click;
            // 
            // bt_Mixing
            // 
            bt_Mixing.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point);
            bt_Mixing.Location = new Point(220, 279);
            bt_Mixing.Name = "bt_Mixing";
            bt_Mixing.Size = new Size(160, 30);
            bt_Mixing.TabIndex = 2;
            bt_Mixing.Text = "Mixing";
            bt_Mixing.UseVisualStyleBackColor = true;
            bt_Mixing.Click += bt_Mixing_Click;
            // 
            // bt_SoundCloud
            // 
            bt_SoundCloud.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point);
            bt_SoundCloud.Location = new Point(220, 314);
            bt_SoundCloud.Name = "bt_SoundCloud";
            bt_SoundCloud.Size = new Size(160, 30);
            bt_SoundCloud.TabIndex = 3;
            bt_SoundCloud.Text = "SoundCloud";
            bt_SoundCloud.UseVisualStyleBackColor = true;
            bt_SoundCloud.Click += bt_SoundCloud_Click;
            // 
            // lb_UserName
            // 
            lb_UserName.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            lb_UserName.Location = new Point(173, 232);
            lb_UserName.Name = "lb_UserName";
            lb_UserName.Size = new Size(250, 35);
            lb_UserName.TabIndex = 4;
            lb_UserName.Text = "label1";
            lb_UserName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 441);
            Controls.Add(lb_UserName);
            Controls.Add(bt_SoundCloud);
            Controls.Add(bt_Mixing);
            Controls.Add(bt_Icon);
            Controls.Add(pb_Profile);
            Name = "Form3";
            Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)pb_Profile).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox pb_Profile;
        private System.Windows.Forms.Button bt_Icon;
        private System.Windows.Forms.Button bt_Mixing;
        private System.Windows.Forms.Button bt_SoundCloud;
        private Label lb_UserName;
    }
}

