namespace WinFormsApp_0812
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
            knobs1 = new PictureBox();
            knobs2 = new PictureBox();
            knobs3 = new PictureBox();
            button1 = new Button();
            trackBar1 = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)knobs1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)knobs2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)knobs3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // knobs1
            // 
            knobs1.Location = new Point(107, 99);
            knobs1.Name = "knobs1";
            knobs1.Size = new Size(100, 50);
            knobs1.TabIndex = 0;
            knobs1.TabStop = false;
            // 
            // knobs2
            // 
            knobs2.Location = new Point(264, 99);
            knobs2.Name = "knobs2";
            knobs2.Size = new Size(100, 50);
            knobs2.TabIndex = 0;
            knobs2.TabStop = false;
            // 
            // knobs3
            // 
            knobs3.Location = new Point(420, 99);
            knobs3.Name = "knobs3";
            knobs3.Size = new Size(100, 50);
            knobs3.TabIndex = 0;
            knobs3.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(197, 332);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(103, 208);
            trackBar1.Maximum = 12;
            trackBar1.Minimum = -12;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(231, 45);
            trackBar1.TabIndex = 2;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(trackBar1);
            Controls.Add(button1);
            Controls.Add(knobs3);
            Controls.Add(knobs2);
            Controls.Add(knobs1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)knobs1).EndInit();
            ((System.ComponentModel.ISupportInitialize)knobs2).EndInit();
            ((System.ComponentModel.ISupportInitialize)knobs3).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox knobs1;
        private PictureBox knobs2;
        private PictureBox knobs3;
        private Button button1;
        private TrackBar trackBar1;
    }
}
