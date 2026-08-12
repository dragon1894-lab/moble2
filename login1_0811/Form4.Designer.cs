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
            SuspendLayout();
            // 
            // bt_Icon1
            // 
            bt_Icon1.BackgroundImageLayout = ImageLayout.Zoom;
            bt_Icon1.Image = (Image)resources.GetObject("bt_Icon1.Image");
            bt_Icon1.Location = new Point(40, 40);
            bt_Icon1.Name = "bt_Icon1";
            bt_Icon1.Size = new Size(160, 160);
            bt_Icon1.TabIndex = 0;
            bt_Icon1.UseVisualStyleBackColor = false;
            bt_Icon1.Click += bt_Icon1_Click;
            // 
            // bt_Icon2
            // 
            bt_Icon2.BackgroundImageLayout = ImageLayout.Zoom;
            bt_Icon2.Image = (Image)resources.GetObject("bt_Icon2.Image");
            bt_Icon2.Location = new Point(220, 40);
            bt_Icon2.Name = "bt_Icon2";
            bt_Icon2.Size = new Size(160, 160);
            bt_Icon2.TabIndex = 0;
            bt_Icon2.UseVisualStyleBackColor = false;
            bt_Icon2.Click += bt_Icon2_Click;
            // 
            // bt_Icon4
            // 
            bt_Icon4.BackgroundImageLayout = ImageLayout.Zoom;
            bt_Icon4.Image = (Image)resources.GetObject("bt_Icon4.Image");
            bt_Icon4.Location = new Point(220, 220);
            bt_Icon4.Name = "bt_Icon4";
            bt_Icon4.Size = new Size(160, 160);
            bt_Icon4.TabIndex = 0;
            bt_Icon4.UseVisualStyleBackColor = false;
            bt_Icon4.Click += bt_Icon4_Click;
            // 
            // bt_Icon3
            // 
            bt_Icon3.BackgroundImageLayout = ImageLayout.Zoom;
            bt_Icon3.Image = (Image)resources.GetObject("bt_Icon3.Image");
            bt_Icon3.Location = new Point(40, 220);
            bt_Icon3.Name = "bt_Icon3";
            bt_Icon3.Size = new Size(160, 160);
            bt_Icon3.TabIndex = 0;
            bt_Icon3.UseVisualStyleBackColor = false;
            bt_Icon3.Click += bt_Icon3_Click;
            // 
            // Form4
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 417);
            Controls.Add(bt_Icon4);
            Controls.Add(bt_Icon2);
            Controls.Add(bt_Icon3);
            Controls.Add(bt_Icon1);
            Name = "Form4";
            Text = "Form4";
            ResumeLayout(false);
        }

        #endregion

        private Button bt_Icon1;
        private Button bt_Icon2;
        private Button bt_Icon4;
        private Button bt_Icon3;
    }
}

