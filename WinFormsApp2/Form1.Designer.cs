namespace WinFormsApp2
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
            components = new System.ComponentModel.Container();
            panelDeckA = new Panel();
            btnLoadA = new Button();
            lblSongA = new Label();
            lblTimeA = new Label();
            btnPlayA = new Button();
            btnPauseA = new Button();
            btnStopA = new Button();
            trackPositionA = new TrackBar();
            trackVolumeA = new TrackBar();
            timerA = new System.Windows.Forms.Timer(components);
            panelDeckB = new Panel();
            btnLoadB = new Button();
            lblSongB = new Label();
            lblTimeB = new Label();
            trackPositionB = new TrackBar();
            btnPauseB = new Button();
            btnPlayB = new Button();
            btnStopB = new Button();
            trackVolumeB = new TrackBar();
            numMasterBpm = new NumericUpDown();
            trackFilterA = new TrackBar();
            trackFilterB = new TrackBar();
            lblFilterA = new Label();
            lblFilterB = new Label();
            trackBassA = new TrackBar();
            trackBassB = new TrackBar();
            lblBassA = new Label();
            lblBassB = new Label();
            btnLoopMinusA = new Button();
            btnLoopPlusA = new Button();
            btnLoopToggleA = new Button();
            btnLoopMinusB = new Button();
            btnLoopToggleB = new Button();
            btnLoopPlusB = new Button();
            cmbFxA = new ComboBox();
            cmbFxB = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)trackPositionA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackVolumeA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackPositionB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackVolumeB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMasterBpm).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackFilterA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackFilterB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBassA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBassB).BeginInit();
            SuspendLayout();
            // 
            // panelDeckA
            // 
            panelDeckA.AllowDrop = true;
            panelDeckA.Location = new Point(36, 47);
            panelDeckA.Name = "panelDeckA";
            panelDeckA.Size = new Size(307, 192);
            panelDeckA.TabIndex = 0;
            // 
            // btnLoadA
            // 
            btnLoadA.Location = new Point(36, 18);
            btnLoadA.Name = "btnLoadA";
            btnLoadA.Size = new Size(75, 23);
            btnLoadA.TabIndex = 1;
            btnLoadA.Text = "📁 곡 선택";
            btnLoadA.UseVisualStyleBackColor = true;
            btnLoadA.Click += btnLoadA_Click;
            // 
            // lblSongA
            // 
            lblSongA.AutoSize = true;
            lblSongA.Location = new Point(182, 26);
            lblSongA.Name = "lblSongA";
            lblSongA.Size = new Size(0, 15);
            lblSongA.TabIndex = 2;
            // 
            // lblTimeA
            // 
            lblTimeA.AutoSize = true;
            lblTimeA.Location = new Point(261, 293);
            lblTimeA.Name = "lblTimeA";
            lblTimeA.Size = new Size(82, 15);
            lblTimeA.TabIndex = 3;
            lblTimeA.Text = "00:00 / 00:00";
            // 
            // btnPlayA
            // 
            btnPlayA.Location = new Point(145, 289);
            btnPlayA.Name = "btnPlayA";
            btnPlayA.Size = new Size(31, 23);
            btnPlayA.TabIndex = 4;
            btnPlayA.Text = "▶";
            btnPlayA.UseVisualStyleBackColor = true;
            btnPlayA.Click += btnPlayA_Click;
            // 
            // btnPauseA
            // 
            btnPauseA.Location = new Point(108, 289);
            btnPauseA.Name = "btnPauseA";
            btnPauseA.Size = new Size(31, 23);
            btnPauseA.TabIndex = 5;
            btnPauseA.Text = "⏸";
            btnPauseA.UseVisualStyleBackColor = true;
            btnPauseA.Click += btnPauseA_Click;
            // 
            // btnStopA
            // 
            btnStopA.Location = new Point(182, 289);
            btnStopA.Name = "btnStopA";
            btnStopA.Size = new Size(31, 23);
            btnStopA.TabIndex = 6;
            btnStopA.Text = "■";
            btnStopA.UseVisualStyleBackColor = true;
            btnStopA.Click += btnStopA_Click;
            // 
            // trackPositionA
            // 
            trackPositionA.Location = new Point(36, 245);
            trackPositionA.Maximum = 100;
            trackPositionA.Name = "trackPositionA";
            trackPositionA.Size = new Size(307, 45);
            trackPositionA.TabIndex = 7;
            trackPositionA.TickStyle = TickStyle.None;
            trackPositionA.Value = 100;
            // 
            // trackVolumeA
            // 
            trackVolumeA.Location = new Point(349, 47);
            trackVolumeA.Maximum = 100;
            trackVolumeA.Name = "trackVolumeA";
            trackVolumeA.Orientation = Orientation.Vertical;
            trackVolumeA.Size = new Size(45, 192);
            trackVolumeA.TabIndex = 8;
            trackVolumeA.TickStyle = TickStyle.TopLeft;
            trackVolumeA.Value = 100;
            trackVolumeA.Scroll += trackVolumeA_Scroll;
            // 
            // timerA
            // 
            timerA.Enabled = true;
            // 
            // panelDeckB
            // 
            panelDeckB.Location = new Point(463, 47);
            panelDeckB.Name = "panelDeckB";
            panelDeckB.Size = new Size(307, 192);
            panelDeckB.TabIndex = 9;
            // 
            // btnLoadB
            // 
            btnLoadB.Location = new Point(463, 18);
            btnLoadB.Name = "btnLoadB";
            btnLoadB.Size = new Size(75, 23);
            btnLoadB.TabIndex = 10;
            btnLoadB.Text = "📁 곡 선택";
            btnLoadB.UseVisualStyleBackColor = true;
            // 
            // lblSongB
            // 
            lblSongB.AutoSize = true;
            lblSongB.Location = new Point(632, 22);
            lblSongB.Name = "lblSongB";
            lblSongB.Size = new Size(0, 15);
            lblSongB.TabIndex = 11;
            // 
            // lblTimeB
            // 
            lblTimeB.AutoSize = true;
            lblTimeB.Location = new Point(688, 293);
            lblTimeB.Name = "lblTimeB";
            lblTimeB.Size = new Size(82, 15);
            lblTimeB.TabIndex = 12;
            lblTimeB.Text = "00:00 / 00:00";
            // 
            // trackPositionB
            // 
            trackPositionB.Location = new Point(463, 245);
            trackPositionB.Maximum = 100;
            trackPositionB.Name = "trackPositionB";
            trackPositionB.Size = new Size(307, 45);
            trackPositionB.TabIndex = 13;
            trackPositionB.TickStyle = TickStyle.None;
            // 
            // btnPauseB
            // 
            btnPauseB.Location = new Point(564, 289);
            btnPauseB.Name = "btnPauseB";
            btnPauseB.Size = new Size(31, 23);
            btnPauseB.TabIndex = 14;
            btnPauseB.Text = "⏸";
            btnPauseB.UseVisualStyleBackColor = true;
            // 
            // btnPlayB
            // 
            btnPlayB.Location = new Point(601, 289);
            btnPlayB.Name = "btnPlayB";
            btnPlayB.Size = new Size(31, 23);
            btnPlayB.TabIndex = 15;
            btnPlayB.Text = "▶";
            btnPlayB.UseVisualStyleBackColor = true;
            // 
            // btnStopB
            // 
            btnStopB.Location = new Point(638, 289);
            btnStopB.Name = "btnStopB";
            btnStopB.Size = new Size(31, 23);
            btnStopB.TabIndex = 16;
            btnStopB.Text = "■";
            btnStopB.UseVisualStyleBackColor = true;
            // 
            // trackVolumeB
            // 
            trackVolumeB.Location = new Point(790, 47);
            trackVolumeB.Maximum = 100;
            trackVolumeB.Name = "trackVolumeB";
            trackVolumeB.Orientation = Orientation.Vertical;
            trackVolumeB.Size = new Size(45, 192);
            trackVolumeB.TabIndex = 17;
            trackVolumeB.TickStyle = TickStyle.TopLeft;
            trackVolumeB.Value = 100;
            // 
            // numMasterBpm
            // 
            numMasterBpm.Location = new Point(349, 333);
            numMasterBpm.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numMasterBpm.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            numMasterBpm.Name = "numMasterBpm";
            numMasterBpm.Size = new Size(120, 23);
            numMasterBpm.TabIndex = 18;
            numMasterBpm.Value = new decimal(new int[] { 120, 0, 0, 0 });
            // 
            // trackFilterA
            // 
            trackFilterA.Location = new Point(80, 318);
            trackFilterA.Maximum = 100;
            trackFilterA.Minimum = -100;
            trackFilterA.Name = "trackFilterA";
            trackFilterA.Size = new Size(157, 45);
            trackFilterA.TabIndex = 19;
            // 
            // trackFilterB
            // 
            trackFilterB.Location = new Point(539, 311);
            trackFilterB.Maximum = 100;
            trackFilterB.Minimum = -100;
            trackFilterB.Name = "trackFilterB";
            trackFilterB.Size = new Size(157, 45);
            trackFilterB.TabIndex = 20;
            // 
            // lblFilterA
            // 
            lblFilterA.AutoSize = true;
            lblFilterA.Location = new Point(243, 333);
            lblFilterA.Name = "lblFilterA";
            lblFilterA.Size = new Size(33, 15);
            lblFilterA.TabIndex = 21;
            lblFilterA.Text = "Filter";
            // 
            // lblFilterB
            // 
            lblFilterB.AutoSize = true;
            lblFilterB.Location = new Point(702, 333);
            lblFilterB.Name = "lblFilterB";
            lblFilterB.Size = new Size(33, 15);
            lblFilterB.TabIndex = 21;
            lblFilterB.Text = "Filter";
            // 
            // trackBassA
            // 
            trackBassA.Location = new Point(80, 354);
            trackBassA.Maximum = 100;
            trackBassA.Minimum = -100;
            trackBassA.Name = "trackBassA";
            trackBassA.Size = new Size(157, 45);
            trackBassA.TabIndex = 22;
            // 
            // trackBassB
            // 
            trackBassB.Location = new Point(539, 354);
            trackBassB.Maximum = 100;
            trackBassB.Minimum = -100;
            trackBassB.Name = "trackBassB";
            trackBassB.Size = new Size(157, 45);
            trackBassB.TabIndex = 23;
            // 
            // lblBassA
            // 
            lblBassA.AutoSize = true;
            lblBassA.Location = new Point(243, 366);
            lblBassA.Name = "lblBassA";
            lblBassA.Size = new Size(30, 15);
            lblBassA.TabIndex = 24;
            lblBassA.Text = "Bass";
            // 
            // lblBassB
            // 
            lblBassB.AutoSize = true;
            lblBassB.Location = new Point(702, 366);
            lblBassB.Name = "lblBassB";
            lblBassB.Size = new Size(30, 15);
            lblBassB.TabIndex = 24;
            lblBassB.Text = "Bass";
            // 
            // btnLoopMinusA
            // 
            btnLoopMinusA.Location = new Point(224, 408);
            btnLoopMinusA.Name = "btnLoopMinusA";
            btnLoopMinusA.Size = new Size(22, 38);
            btnLoopMinusA.TabIndex = 25;
            btnLoopMinusA.Text = "-";
            btnLoopMinusA.UseVisualStyleBackColor = true;
            // 
            // btnLoopPlusA
            // 
            btnLoopPlusA.Location = new Point(323, 408);
            btnLoopPlusA.Name = "btnLoopPlusA";
            btnLoopPlusA.Size = new Size(22, 38);
            btnLoopPlusA.TabIndex = 26;
            btnLoopPlusA.Text = "+";
            btnLoopPlusA.UseVisualStyleBackColor = true;
            // 
            // btnLoopToggleA
            // 
            btnLoopToggleA.Location = new Point(243, 408);
            btnLoopToggleA.Name = "btnLoopToggleA";
            btnLoopToggleA.Size = new Size(82, 38);
            btnLoopToggleA.TabIndex = 27;
            btnLoopToggleA.Text = "↻1";
            btnLoopToggleA.UseVisualStyleBackColor = true;
            // 
            // btnLoopMinusB
            // 
            btnLoopMinusB.Location = new Point(683, 402);
            btnLoopMinusB.Name = "btnLoopMinusB";
            btnLoopMinusB.Size = new Size(22, 38);
            btnLoopMinusB.TabIndex = 28;
            btnLoopMinusB.Text = "-";
            btnLoopMinusB.UseVisualStyleBackColor = true;
            // 
            // btnLoopToggleB
            // 
            btnLoopToggleB.Location = new Point(702, 402);
            btnLoopToggleB.Name = "btnLoopToggleB";
            btnLoopToggleB.Size = new Size(82, 38);
            btnLoopToggleB.TabIndex = 29;
            btnLoopToggleB.Text = "↻1";
            btnLoopToggleB.UseVisualStyleBackColor = true;
            // 
            // btnLoopPlusB
            // 
            btnLoopPlusB.Location = new Point(782, 402);
            btnLoopPlusB.Name = "btnLoopPlusB";
            btnLoopPlusB.Size = new Size(22, 38);
            btnLoopPlusB.TabIndex = 30;
            btnLoopPlusB.Text = "+";
            btnLoopPlusB.UseVisualStyleBackColor = true;
            // 
            // cmbFxA
            // 
            cmbFxA.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFxA.FlatStyle = FlatStyle.Flat;
            cmbFxA.FormattingEnabled = true;
            cmbFxA.Items.AddRange(new object[] { "OFF", "1. Echo", "2. Reverb" });
            cmbFxA.Location = new Point(74, 417);
            cmbFxA.Name = "cmbFxA";
            cmbFxA.Size = new Size(121, 23);
            cmbFxA.TabIndex = 31;
            // 
            // cmbFxB
            // 
            cmbFxB.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFxB.FlatStyle = FlatStyle.Flat;
            cmbFxB.FormattingEnabled = true;
            cmbFxB.Items.AddRange(new object[] { "OFF", "1. Echo", "2. Reverb" });
            cmbFxB.Location = new Point(525, 411);
            cmbFxB.Name = "cmbFxB";
            cmbFxB.Size = new Size(121, 23);
            cmbFxB.TabIndex = 32;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(847, 493);
            Controls.Add(cmbFxB);
            Controls.Add(cmbFxA);
            Controls.Add(btnLoopPlusB);
            Controls.Add(btnLoopToggleB);
            Controls.Add(btnLoopMinusB);
            Controls.Add(btnLoopToggleA);
            Controls.Add(btnLoopPlusA);
            Controls.Add(btnLoopMinusA);
            Controls.Add(lblBassB);
            Controls.Add(lblBassA);
            Controls.Add(trackBassB);
            Controls.Add(trackBassA);
            Controls.Add(lblFilterB);
            Controls.Add(lblFilterA);
            Controls.Add(trackFilterB);
            Controls.Add(trackFilterA);
            Controls.Add(numMasterBpm);
            Controls.Add(trackVolumeB);
            Controls.Add(btnStopB);
            Controls.Add(btnPlayB);
            Controls.Add(btnPauseB);
            Controls.Add(trackPositionB);
            Controls.Add(lblTimeB);
            Controls.Add(lblSongB);
            Controls.Add(btnLoadB);
            Controls.Add(panelDeckB);
            Controls.Add(trackVolumeA);
            Controls.Add(trackPositionA);
            Controls.Add(btnStopA);
            Controls.Add(btnPauseA);
            Controls.Add(btnPlayA);
            Controls.Add(lblTimeA);
            Controls.Add(lblSongA);
            Controls.Add(btnLoadA);
            Controls.Add(panelDeckA);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)trackPositionA).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackVolumeA).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackPositionB).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackVolumeB).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMasterBpm).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackFilterA).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackFilterB).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBassA).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBassB).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelDeckA;
        private Button btnLoadA;
        private Label lblSongA;
        private Label lblTimeA;
        private Button btnPlayA;
        private Button btnPauseA;
        private Button btnStopA;
        private TrackBar trackPositionA;
        private TrackBar trackVolumeA;
        private System.Windows.Forms.Timer timerA;
        private Panel panelDeckB;
        private Button btnLoadB;
        private Label lblSongB;
        private Label lblTimeB;
        private TrackBar trackPositionB;
        private Button btnPauseB;
        private Button btnPlayB;
        private Button btnStopB;
        private TrackBar trackVolumeB;
        private NumericUpDown numMasterBpm;
        private TrackBar trackFilterA;
        private TrackBar trackFilterB;
        private Label lblFilterA;
        private Label lblFilterB;
        private TrackBar trackBassA;
        private TrackBar trackBassB;
        private Label lblBassA;
        private Label lblBassB;
        private Button btnLoopMinusA;
        private Button btnLoopPlusA;
        private Button btnLoopToggleA;
        private Button btnLoopMinusB;
        private Button btnLoopToggleB;
        private Button btnLoopPlusB;
        private ComboBox cmbFxA;
        private ComboBox cmbFxB;
    }
}

