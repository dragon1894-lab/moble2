namespace DJform
{
    partial class DJform
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
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            WaveFormL = new PictureBox();
            WaveFormR = new PictureBox();
            TurntableL = new PictureBox();
            TurntableR = new PictureBox();
            VolumeControlL = new TrackBar();
            VolumeControlR = new TrackBar();
            StutterR = new TrackBar();
            CrushR = new TrackBar();
            EchoR = new TrackBar();
            FlangerR = new TrackBar();
            StutterL = new TrackBar();
            EchoL = new TrackBar();
            CrushL = new TrackBar();
            FlangerL = new TrackBar();
            VolumeControlW = new TrackBar();
            LeftMusicStart = new Button();
            RightMusicStart = new Button();
            MidKnobL = new PictureBox();
            BassKnobL = new PictureBox();
            FilterKnobL = new PictureBox();
            MidKnobR = new PictureBox();
            BassKnobR = new PictureBox();
            FilterKnobR = new PictureBox();
            MusicTextL = new Label();
            MusicTextR = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            label15 = new Label();
            label16 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            MusicInsertL = new Button();
            MusicInsertR = new Button();
            numBpmL = new NumericUpDown();
            numBpmR = new NumericUpDown();
            numMasterBpm = new NumericUpDown();
            btnMasterSync = new Button();
            label17 = new Label();
            label20 = new Label();
            label21 = new Label();
            label22 = new Label();
            lblTimeL = new Label();
            lblTimeR = new Label();
            ((System.ComponentModel.ISupportInitialize)WaveFormL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)WaveFormR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TurntableL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TurntableR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VolumeControlL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VolumeControlR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)StutterR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CrushR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)EchoR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FlangerR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)StutterL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)EchoL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)CrushL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FlangerL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VolumeControlW).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MidKnobL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BassKnobL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FilterKnobL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MidKnobR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BassKnobR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FilterKnobR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBpmL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBpmR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMasterBpm).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1074, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // WaveFormL
            // 
            WaveFormL.Location = new Point(12, 59);
            WaveFormL.Name = "WaveFormL";
            WaveFormL.Size = new Size(400, 50);
            WaveFormL.TabIndex = 1;
            WaveFormL.TabStop = false;
            // 
            // WaveFormR
            // 
            WaveFormR.Location = new Point(662, 59);
            WaveFormR.Name = "WaveFormR";
            WaveFormR.Size = new Size(400, 50);
            WaveFormR.TabIndex = 1;
            WaveFormR.TabStop = false;
            // 
            // TurntableL
            // 
            TurntableL.Location = new Point(292, 124);
            TurntableL.Name = "TurntableL";
            TurntableL.Size = new Size(120, 120);
            TurntableL.TabIndex = 2;
            TurntableL.TabStop = false;
            // 
            // TurntableR
            // 
            TurntableR.Location = new Point(662, 124);
            TurntableR.Name = "TurntableR";
            TurntableR.Size = new Size(120, 120);
            TurntableR.TabIndex = 2;
            TurntableR.TabStop = false;
            // 
            // VolumeControlL
            // 
            VolumeControlL.Location = new Point(482, 95);
            VolumeControlL.Maximum = 100;
            VolumeControlL.Name = "VolumeControlL";
            VolumeControlL.Orientation = Orientation.Vertical;
            VolumeControlL.Size = new Size(45, 149);
            VolumeControlL.TabIndex = 3;
            VolumeControlL.TickFrequency = 10;
            VolumeControlL.TickStyle = TickStyle.Both;
            VolumeControlL.Value = 100;
            VolumeControlL.Scroll += UpdateVolumes;
            // 
            // VolumeControlR
            // 
            VolumeControlR.Location = new Point(546, 95);
            VolumeControlR.Maximum = 100;
            VolumeControlR.Name = "VolumeControlR";
            VolumeControlR.Orientation = Orientation.Vertical;
            VolumeControlR.Size = new Size(45, 149);
            VolumeControlR.TabIndex = 3;
            VolumeControlR.TickFrequency = 10;
            VolumeControlR.TickStyle = TickStyle.Both;
            VolumeControlR.Value = 100;
            VolumeControlR.Scroll += UpdateVolumes;
            // 
            // StutterR
            // 
            StutterR.Location = new Point(807, 124);
            StutterR.Maximum = 100;
            StutterR.Name = "StutterR";
            StutterR.Orientation = Orientation.Vertical;
            StutterR.Size = new Size(45, 149);
            StutterR.TabIndex = 3;
            StutterR.TickFrequency = 10;
            StutterR.TickStyle = TickStyle.Both;
            // 
            // CrushR
            // 
            CrushR.Location = new Point(871, 124);
            CrushR.Maximum = 100;
            CrushR.Name = "CrushR";
            CrushR.Orientation = Orientation.Vertical;
            CrushR.Size = new Size(45, 149);
            CrushR.TabIndex = 3;
            CrushR.TickFrequency = 10;
            CrushR.TickStyle = TickStyle.Both;
            // 
            // EchoR
            // 
            EchoR.Location = new Point(942, 124);
            EchoR.Maximum = 100;
            EchoR.Name = "EchoR";
            EchoR.Orientation = Orientation.Vertical;
            EchoR.Size = new Size(45, 149);
            EchoR.TabIndex = 3;
            EchoR.TickFrequency = 10;
            EchoR.TickStyle = TickStyle.Both;
            // 
            // FlangerR
            // 
            FlangerR.Location = new Point(1006, 124);
            FlangerR.Maximum = 100;
            FlangerR.Name = "FlangerR";
            FlangerR.Orientation = Orientation.Vertical;
            FlangerR.Size = new Size(45, 149);
            FlangerR.TabIndex = 3;
            FlangerR.TickFrequency = 10;
            FlangerR.TickStyle = TickStyle.Both;
            // 
            // StutterL
            // 
            StutterL.Location = new Point(32, 124);
            StutterL.Maximum = 100;
            StutterL.Name = "StutterL";
            StutterL.Orientation = Orientation.Vertical;
            StutterL.Size = new Size(45, 149);
            StutterL.TabIndex = 3;
            StutterL.TickFrequency = 10;
            StutterL.TickStyle = TickStyle.Both;
            // 
            // EchoL
            // 
            EchoL.Location = new Point(167, 124);
            EchoL.Maximum = 100;
            EchoL.Name = "EchoL";
            EchoL.Orientation = Orientation.Vertical;
            EchoL.Size = new Size(45, 149);
            EchoL.TabIndex = 3;
            EchoL.TickFrequency = 10;
            EchoL.TickStyle = TickStyle.Both;
            // 
            // CrushL
            // 
            CrushL.Location = new Point(96, 124);
            CrushL.Maximum = 100;
            CrushL.Name = "CrushL";
            CrushL.Orientation = Orientation.Vertical;
            CrushL.Size = new Size(45, 149);
            CrushL.TabIndex = 3;
            CrushL.TickFrequency = 10;
            CrushL.TickStyle = TickStyle.Both;
            // 
            // FlangerL
            // 
            FlangerL.Location = new Point(231, 124);
            FlangerL.Maximum = 100;
            FlangerL.Name = "FlangerL";
            FlangerL.Orientation = Orientation.Vertical;
            FlangerL.Size = new Size(45, 149);
            FlangerL.TabIndex = 3;
            FlangerL.TickFrequency = 10;
            FlangerL.TickStyle = TickStyle.Both;
            // 
            // VolumeControlW
            // 
            VolumeControlW.Location = new Point(418, 264);
            VolumeControlW.Maximum = 200;
            VolumeControlW.Name = "VolumeControlW";
            VolumeControlW.Size = new Size(238, 45);
            VolumeControlW.TabIndex = 3;
            VolumeControlW.TickFrequency = 10;
            VolumeControlW.TickStyle = TickStyle.Both;
            VolumeControlW.Value = 100;
            VolumeControlW.Scroll += UpdateVolumes;
            // 
            // LeftMusicStart
            // 
            LeftMusicStart.Location = new Point(367, 261);
            LeftMusicStart.Name = "LeftMusicStart";
            LeftMusicStart.Size = new Size(45, 45);
            LeftMusicStart.TabIndex = 4;
            LeftMusicStart.Text = "button1";
            LeftMusicStart.UseVisualStyleBackColor = true;
            LeftMusicStart.Click += LeftMusicStart_Click;
            // 
            // RightMusicStart
            // 
            RightMusicStart.Location = new Point(662, 264);
            RightMusicStart.Name = "RightMusicStart";
            RightMusicStart.Size = new Size(45, 45);
            RightMusicStart.TabIndex = 4;
            RightMusicStart.Text = "button1";
            RightMusicStart.UseVisualStyleBackColor = true;
            RightMusicStart.Click += RightMusicStart_Click;
            // 
            // MidKnobL
            // 
            MidKnobL.Location = new Point(431, 106);
            MidKnobL.Name = "MidKnobL";
            MidKnobL.Size = new Size(30, 30);
            MidKnobL.TabIndex = 5;
            MidKnobL.TabStop = false;
            // 
            // BassKnobL
            // 
            BassKnobL.Location = new Point(431, 155);
            BassKnobL.Name = "BassKnobL";
            BassKnobL.Size = new Size(30, 30);
            BassKnobL.TabIndex = 5;
            BassKnobL.TabStop = false;
            // 
            // FilterKnobL
            // 
            FilterKnobL.Location = new Point(431, 205);
            FilterKnobL.Name = "FilterKnobL";
            FilterKnobL.Size = new Size(30, 30);
            FilterKnobL.TabIndex = 5;
            FilterKnobL.TabStop = false;
            // 
            // MidKnobR
            // 
            MidKnobR.Location = new Point(606, 106);
            MidKnobR.Name = "MidKnobR";
            MidKnobR.Size = new Size(30, 30);
            MidKnobR.TabIndex = 5;
            MidKnobR.TabStop = false;
            // 
            // BassKnobR
            // 
            BassKnobR.Location = new Point(606, 155);
            BassKnobR.Name = "BassKnobR";
            BassKnobR.Size = new Size(30, 30);
            BassKnobR.TabIndex = 5;
            BassKnobR.TabStop = false;
            // 
            // FilterKnobR
            // 
            FilterKnobR.Location = new Point(606, 205);
            FilterKnobR.Name = "FilterKnobR";
            FilterKnobR.Size = new Size(30, 30);
            FilterKnobR.TabIndex = 5;
            FilterKnobR.TabStop = false;
            // 
            // MusicTextL
            // 
            MusicTextL.AutoSize = true;
            MusicTextL.Location = new Point(12, 41);
            MusicTextL.Name = "MusicTextL";
            MusicTextL.Size = new Size(108, 15);
            MusicTextL.TabIndex = 6;
            MusicTextL.Text = "노래 제목 - 작곡가";
            // 
            // MusicTextR
            // 
            MusicTextR.AutoSize = true;
            MusicTextR.Location = new Point(662, 41);
            MusicTextR.Name = "MusicTextR";
            MusicTextR.Size = new Size(108, 15);
            MusicTextR.TabIndex = 6;
            MusicTextR.Text = "노래 제목 - 작곡가";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(431, 238);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 7;
            label3.Text = "Filter";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(606, 238);
            label4.Name = "label4";
            label4.Size = new Size(33, 15);
            label4.TabIndex = 7;
            label4.Text = "Filter";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(606, 188);
            label5.Name = "label5";
            label5.Size = new Size(30, 15);
            label5.TabIndex = 7;
            label5.Text = "Bass";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(431, 187);
            label6.Name = "label6";
            label6.Size = new Size(30, 15);
            label6.TabIndex = 7;
            label6.Text = "Bass";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(431, 139);
            label7.Name = "label7";
            label7.Size = new Size(28, 15);
            label7.TabIndex = 7;
            label7.Text = "Mid";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(606, 137);
            label8.Name = "label8";
            label8.Size = new Size(28, 15);
            label8.TabIndex = 7;
            label8.Text = "Mid";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(32, 276);
            label9.Name = "label9";
            label9.Size = new Size(43, 15);
            label9.TabIndex = 8;
            label9.Text = "Stutter";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(96, 276);
            label10.Name = "label10";
            label10.Size = new Size(38, 15);
            label10.TabIndex = 8;
            label10.Text = "Crush";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(167, 276);
            label11.Name = "label11";
            label11.Size = new Size(33, 15);
            label11.TabIndex = 8;
            label11.Text = "Echo";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(231, 276);
            label12.Name = "label12";
            label12.Size = new Size(46, 15);
            label12.TabIndex = 8;
            label12.Text = "Flanger";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(807, 276);
            label13.Name = "label13";
            label13.Size = new Size(43, 15);
            label13.TabIndex = 8;
            label13.Text = "Stutter";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(871, 276);
            label14.Name = "label14";
            label14.Size = new Size(38, 15);
            label14.TabIndex = 8;
            label14.Text = "Crush";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(942, 276);
            label15.Name = "label15";
            label15.Size = new Size(33, 15);
            label15.TabIndex = 8;
            label15.Text = "Echo";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(1006, 276);
            label16.Name = "label16";
            label16.Size = new Size(46, 15);
            label16.TabIndex = 8;
            label16.Text = "Flanger";
            // 
            // MusicInsertL
            // 
            MusicInsertL.Location = new Point(292, 261);
            MusicInsertL.Name = "MusicInsertL";
            MusicInsertL.Size = new Size(45, 45);
            MusicInsertL.TabIndex = 4;
            MusicInsertL.Text = "button1";
            MusicInsertL.UseVisualStyleBackColor = true;
            MusicInsertL.Click += MusicInsertL_Click;
            // 
            // MusicInsertR
            // 
            MusicInsertR.Location = new Point(737, 264);
            MusicInsertR.Name = "MusicInsertR";
            MusicInsertR.Size = new Size(45, 45);
            MusicInsertR.TabIndex = 4;
            MusicInsertR.Text = "button1";
            MusicInsertR.UseVisualStyleBackColor = true;
            MusicInsertR.Click += MusicInsertR_Click;
            // 
            // numBpmL
            // 
            numBpmL.DecimalPlaces = 1;
            numBpmL.Location = new Point(167, 369);
            numBpmL.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numBpmL.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            numBpmL.Name = "numBpmL";
            numBpmL.Size = new Size(110, 23);
            numBpmL.TabIndex = 10;
            numBpmL.Value = new decimal(new int[] { 120, 0, 0, 0 });
            numBpmL.ValueChanged += numBpmL_ValueChanged;
            // 
            // numBpmR
            // 
            numBpmR.DecimalPlaces = 1;
            numBpmR.Location = new Point(807, 369);
            numBpmR.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numBpmR.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            numBpmR.Name = "numBpmR";
            numBpmR.Size = new Size(110, 23);
            numBpmR.TabIndex = 10;
            numBpmR.Value = new decimal(new int[] { 120, 0, 0, 0 });
            numBpmR.ValueChanged += numBpmR_ValueChanged;
            // 
            // numMasterBpm
            // 
            numMasterBpm.DecimalPlaces = 1;
            numMasterBpm.Location = new Point(431, 66);
            numMasterBpm.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numMasterBpm.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            numMasterBpm.Name = "numMasterBpm";
            numMasterBpm.Size = new Size(110, 23);
            numMasterBpm.TabIndex = 10;
            numMasterBpm.Value = new decimal(new int[] { 120, 0, 0, 0 });
            numMasterBpm.ValueChanged += numMasterBpm_ValueChanged;
            // 
            // btnMasterSync
            // 
            btnMasterSync.Location = new Point(564, 64);
            btnMasterSync.Name = "btnMasterSync";
            btnMasterSync.Size = new Size(75, 23);
            btnMasterSync.TabIndex = 11;
            btnMasterSync.Text = "Sync";
            btnMasterSync.UseVisualStyleBackColor = true;
            btnMasterSync.Click += btnMasterSync_Click;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(494, 48);
            label17.Name = "label17";
            label17.Size = new Size(68, 15);
            label17.TabIndex = 12;
            label17.Text = "MasterBPM";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(197, 369);
            label20.Name = "label20";
            label20.Size = new Size(0, 15);
            label20.TabIndex = 13;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(829, 351);
            label21.Name = "label21";
            label21.Size = new Size(60, 15);
            label21.TabIndex = 13;
            label21.Text = "BPM 변환";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(191, 351);
            label22.Name = "label22";
            label22.Size = new Size(60, 15);
            label22.TabIndex = 13;
            label22.Text = "BPM 변환";
            // 
            // lblTimeL
            // 
            lblTimeL.AutoSize = true;
            lblTimeL.Location = new Point(330, 41);
            lblTimeL.Name = "lblTimeL";
            lblTimeL.Size = new Size(82, 15);
            lblTimeL.TabIndex = 6;
            lblTimeL.Text = "00:00 / 00:00";
            // 
            // lblTimeR
            // 
            lblTimeR.AutoSize = true;
            lblTimeR.Location = new Point(980, 41);
            lblTimeR.Name = "lblTimeR";
            lblTimeR.Size = new Size(82, 15);
            lblTimeR.TabIndex = 6;
            lblTimeR.Text = "00:00 / 00:00";
            // 
            // DJform
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 505);
            Controls.Add(label22);
            Controls.Add(label21);
            Controls.Add(label20);
            Controls.Add(label17);
            Controls.Add(btnMasterSync);
            Controls.Add(numBpmR);
            Controls.Add(numMasterBpm);
            Controls.Add(numBpmL);
            Controls.Add(label16);
            Controls.Add(label12);
            Controls.Add(label15);
            Controls.Add(label11);
            Controls.Add(label14);
            Controls.Add(label10);
            Controls.Add(label13);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(MusicTextR);
            Controls.Add(lblTimeR);
            Controls.Add(lblTimeL);
            Controls.Add(MusicTextL);
            Controls.Add(FilterKnobR);
            Controls.Add(FilterKnobL);
            Controls.Add(BassKnobR);
            Controls.Add(BassKnobL);
            Controls.Add(MidKnobR);
            Controls.Add(MidKnobL);
            Controls.Add(MusicInsertR);
            Controls.Add(RightMusicStart);
            Controls.Add(MusicInsertL);
            Controls.Add(LeftMusicStart);
            Controls.Add(FlangerL);
            Controls.Add(FlangerR);
            Controls.Add(CrushL);
            Controls.Add(CrushR);
            Controls.Add(VolumeControlW);
            Controls.Add(VolumeControlR);
            Controls.Add(EchoL);
            Controls.Add(EchoR);
            Controls.Add(StutterL);
            Controls.Add(StutterR);
            Controls.Add(VolumeControlL);
            Controls.Add(TurntableR);
            Controls.Add(TurntableL);
            Controls.Add(WaveFormR);
            Controls.Add(WaveFormL);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "DJform";
            Text = "DJform";
            FormClosing += DJform_FormClosing;
            Load += DJform_Load;
            ((System.ComponentModel.ISupportInitialize)WaveFormL).EndInit();
            ((System.ComponentModel.ISupportInitialize)WaveFormR).EndInit();
            ((System.ComponentModel.ISupportInitialize)TurntableL).EndInit();
            ((System.ComponentModel.ISupportInitialize)TurntableR).EndInit();
            ((System.ComponentModel.ISupportInitialize)VolumeControlL).EndInit();
            ((System.ComponentModel.ISupportInitialize)VolumeControlR).EndInit();
            ((System.ComponentModel.ISupportInitialize)StutterR).EndInit();
            ((System.ComponentModel.ISupportInitialize)CrushR).EndInit();
            ((System.ComponentModel.ISupportInitialize)EchoR).EndInit();
            ((System.ComponentModel.ISupportInitialize)FlangerR).EndInit();
            ((System.ComponentModel.ISupportInitialize)StutterL).EndInit();
            ((System.ComponentModel.ISupportInitialize)EchoL).EndInit();
            ((System.ComponentModel.ISupportInitialize)CrushL).EndInit();
            ((System.ComponentModel.ISupportInitialize)FlangerL).EndInit();
            ((System.ComponentModel.ISupportInitialize)VolumeControlW).EndInit();
            ((System.ComponentModel.ISupportInitialize)MidKnobL).EndInit();
            ((System.ComponentModel.ISupportInitialize)BassKnobL).EndInit();
            ((System.ComponentModel.ISupportInitialize)FilterKnobL).EndInit();
            ((System.ComponentModel.ISupportInitialize)MidKnobR).EndInit();
            ((System.ComponentModel.ISupportInitialize)BassKnobR).EndInit();
            ((System.ComponentModel.ISupportInitialize)FilterKnobR).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBpmL).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBpmR).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMasterBpm).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private PictureBox WaveFormL;
        private PictureBox WaveFormR;
        private PictureBox TurntableL;
        private PictureBox TurntableR;
        private TrackBar VolumeControlL;
        private TrackBar VolumeControlR;
        private TrackBar StutterR;
        private TrackBar CrushR;
        private TrackBar EchoR;
        private TrackBar FlangerR;
        private TrackBar StutterL;
        private TrackBar EchoL;
        private TrackBar CrushL;
        private TrackBar FlangerL;
        private TrackBar VolumeControlW;
        private Button LeftMusicStart;
        private Button RightMusicStart;
        private PictureBox MidKnobL;
        private PictureBox BassKnobL;
        private PictureBox FilterKnobL;
        private PictureBox MidKnobR;
        private PictureBox BassKnobR;
        private PictureBox FilterKnobR;
        private Label MusicTextL;
        private Label MusicTextR;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private System.Windows.Forms.Timer timer1;
        private Button MusicInsertL;
        private Button MusicInsertR;
        private NumericUpDown numBpmL;
        private NumericUpDown numBpmR;
        private NumericUpDown numMasterBpm;
        private Button btnMasterSync;
        private Label label17;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label lblTimeL;
        private Label lblTimeR;
    }
}