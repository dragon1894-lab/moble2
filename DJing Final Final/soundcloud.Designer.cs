namespace DJing
{
    partial class soundcloud
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(soundcloud));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(25, 25, 25);
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, -5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(78, 66);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(25, 25, 25);
            label1.Font = new Font("맑은 고딕", 14F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(70, 16);
            label1.Name = "label1";
            label1.Size = new Size(131, 25);
            label1.TabIndex = 1;
            label1.Text = "Sound Kloud";
            // 
            // soundcloud
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 938);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            Name = "soundcloud";
            Text = "SoundKloud";
            FormClosing += soundcloud_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
    }
}