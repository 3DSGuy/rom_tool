namespace _3DSExplorer
{
    partial class frm3DVideo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm3DVideo));
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.picThumb = new System.Windows.Forms.PictureBox();
            this.chk3D = new System.Windows.Forms.CheckBox();
            this.cmbOrientation = new System.Windows.Forms.ComboBox();
            this.lblOrientation = new System.Windows.Forms.Label();
            this.lblYoutube = new System.Windows.Forms.Label();
            this.txtYoutube = new System.Windows.Forms.TextBox();
            this.btnSourceBrowse = new System.Windows.Forms.Button();
            this.txtSourceFile = new System.Windows.Forms.TextBox();
            this.radSourceYoutube = new System.Windows.Forms.RadioButton();
            this.radSourceFile = new System.Windows.Forms.RadioButton();
            this.grpDestination = new System.Windows.Forms.GroupBox();
            this.tbCores = new System.Windows.Forms.TrackBar();
            this.txtCores = new System.Windows.Forms.TextBox();
            this.lblThreads = new System.Windows.Forms.Label();
            this.chkDeleteTempFiles = new System.Windows.Forms.CheckBox();
            this.lblOutputFile = new System.Windows.Forms.Label();
            this.btnDestinationBrowse = new System.Windows.Forms.Button();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.lblQualityWorst = new System.Windows.Forms.Label();
            this.lblQualityBest = new System.Windows.Forms.Label();
            this.numFps = new System.Windows.Forms.NumericUpDown();
            this.lblFPS = new System.Windows.Forms.Label();
            this.lblQuality = new System.Windows.Forms.Label();
            this.tbQuality = new System.Windows.Forms.TrackBar();
            this.txtQuality = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.openFfmpegDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnSet = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.grpVideo = new System.Windows.Forms.GroupBox();
            this.lblKbps = new System.Windows.Forms.Label();
            this.txtVideoBitrate = new System.Windows.Forms.TextBox();
            this.lblVideoBitRate = new System.Windows.Forms.Label();
            this.chkAdvanced = new System.Windows.Forms.CheckBox();
            this.grpAudio = new System.Windows.Forms.GroupBox();
            this.lblVolumeDefault = new System.Windows.Forms.Label();
            this.lblAudioKbps = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.txtAudioBitrate = new System.Windows.Forms.TextBox();
            this.lblAudioBitrate = new System.Windows.Forms.Label();
            this.numVolume = new System.Windows.Forms.NumericUpDown();
            this.chkSplit = new System.Windows.Forms.CheckBox();
            this.lblSampleRate = new System.Windows.Forms.Label();
            this.cmbSampleRate = new System.Windows.Forms.ComboBox();
            this.lvlOutputRes = new System.Windows.Forms.Label();
            this.numLimit = new System.Windows.Forms.NumericUpDown();
            this.grpSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThumb)).BeginInit();
            this.grpDestination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).BeginInit();
            this.grpVideo.SuspendLayout();
            this.grpAudio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.picThumb);
            this.grpSource.Controls.Add(this.chk3D);
            this.grpSource.Controls.Add(this.cmbOrientation);
            this.grpSource.Controls.Add(this.lblOrientation);
            this.grpSource.Controls.Add(this.lblYoutube);
            this.grpSource.Controls.Add(this.txtYoutube);
            this.grpSource.Controls.Add(this.btnSourceBrowse);
            this.grpSource.Controls.Add(this.txtSourceFile);
            this.grpSource.Controls.Add(this.radSourceYoutube);
            this.grpSource.Controls.Add(this.radSourceFile);
            this.grpSource.Location = new System.Drawing.Point(12, 8);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(556, 135);
            this.grpSource.TabIndex = 0;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source:";
            // 
            // picThumb
            // 
            this.picThumb.BackColor = System.Drawing.Color.White;
            this.picThumb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picThumb.Location = new System.Drawing.Point(407, 19);
            this.picThumb.Name = "picThumb";
            this.picThumb.Size = new System.Drawing.Size(134, 101);
            this.picThumb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picThumb.TabIndex = 7;
            this.picThumb.TabStop = false;
            this.picThumb.Visible = false;
            // 
            // chk3D
            // 
            this.chk3D.AutoSize = true;
            this.chk3D.Checked = true;
            this.chk3D.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk3D.Location = new System.Drawing.Point(15, 101);
            this.chk3D.Name = "chk3D";
            this.chk3D.Size = new System.Drawing.Size(40, 17);
            this.chk3D.TabIndex = 8;
            this.chk3D.Text = "3D";
            this.chk3D.UseVisualStyleBackColor = true;
            this.chk3D.CheckedChanged += new System.EventHandler(this.chk3D_CheckedChanged);
            // 
            // cmbOrientation
            // 
            this.cmbOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrientation.FormattingEnabled = true;
            this.cmbOrientation.Items.AddRange(new object[] {
            "Side-by-side (Left-Right)",
            "Side-by-side (Right-Left)",
            "Top-over-Bottom (Left-Right)",
            "Top-over-Bottom (Right-Left)"});
            this.cmbOrientation.Location = new System.Drawing.Point(167, 99);
            this.cmbOrientation.Name = "cmbOrientation";
            this.cmbOrientation.Size = new System.Drawing.Size(222, 21);
            this.cmbOrientation.TabIndex = 7;
            // 
            // lblOrientation
            // 
            this.lblOrientation.AutoSize = true;
            this.lblOrientation.Location = new System.Drawing.Point(100, 102);
            this.lblOrientation.Name = "lblOrientation";
            this.lblOrientation.Size = new System.Drawing.Size(61, 13);
            this.lblOrientation.TabIndex = 6;
            this.lblOrientation.Text = "Orientation:";
            // 
            // lblYoutube
            // 
            this.lblYoutube.AutoSize = true;
            this.lblYoutube.Location = new System.Drawing.Point(100, 50);
            this.lblYoutube.Name = "lblYoutube";
            this.lblYoutube.Size = new System.Drawing.Size(178, 13);
            this.lblYoutube.TabIndex = 5;
            this.lblYoutube.Text = "http://www.youtube.com/watch?v=";
            this.lblYoutube.Visible = false;
            // 
            // txtYoutube
            // 
            this.txtYoutube.Location = new System.Drawing.Point(276, 47);
            this.txtYoutube.Name = "txtYoutube";
            this.txtYoutube.Size = new System.Drawing.Size(114, 20);
            this.txtYoutube.TabIndex = 4;
            this.toolTip.SetToolTip(this.txtYoutube, "You can paste a full youtube link here for auto-parsing");
            this.txtYoutube.Visible = false;
            this.txtYoutube.TextChanged += new System.EventHandler(this.txtYoutube_TextChanged);
            // 
            // btnSourceBrowse
            // 
            this.btnSourceBrowse.Location = new System.Drawing.Point(340, 20);
            this.btnSourceBrowse.Name = "btnSourceBrowse";
            this.btnSourceBrowse.Size = new System.Drawing.Size(50, 21);
            this.btnSourceBrowse.TabIndex = 3;
            this.btnSourceBrowse.Text = "...";
            this.btnSourceBrowse.UseVisualStyleBackColor = true;
            this.btnSourceBrowse.Click += new System.EventHandler(this.btnSourceBrowse_Click);
            // 
            // txtSourceFile
            // 
            this.txtSourceFile.Location = new System.Drawing.Point(103, 21);
            this.txtSourceFile.Name = "txtSourceFile";
            this.txtSourceFile.ReadOnly = true;
            this.txtSourceFile.Size = new System.Drawing.Size(231, 20);
            this.txtSourceFile.TabIndex = 2;
            // 
            // radSourceYoutube
            // 
            this.radSourceYoutube.AutoSize = true;
            this.radSourceYoutube.Location = new System.Drawing.Point(16, 48);
            this.radSourceYoutube.Name = "radSourceYoutube";
            this.radSourceYoutube.Size = new System.Drawing.Size(69, 17);
            this.radSourceYoutube.TabIndex = 1;
            this.radSourceYoutube.Text = "YouTube";
            this.radSourceYoutube.UseVisualStyleBackColor = true;
            this.radSourceYoutube.CheckedChanged += new System.EventHandler(this.RadioSourceCheckedChanged);
            // 
            // radSourceFile
            // 
            this.radSourceFile.AutoSize = true;
            this.radSourceFile.Checked = true;
            this.radSourceFile.Location = new System.Drawing.Point(16, 22);
            this.radSourceFile.Name = "radSourceFile";
            this.radSourceFile.Size = new System.Drawing.Size(41, 17);
            this.radSourceFile.TabIndex = 0;
            this.radSourceFile.TabStop = true;
            this.radSourceFile.Text = "File";
            this.radSourceFile.UseVisualStyleBackColor = true;
            this.radSourceFile.CheckedChanged += new System.EventHandler(this.RadioSourceCheckedChanged);
            // 
            // grpDestination
            // 
            this.grpDestination.Controls.Add(this.numLimit);
            this.grpDestination.Controls.Add(this.chkSplit);
            this.grpDestination.Controls.Add(this.tbCores);
            this.grpDestination.Controls.Add(this.txtCores);
            this.grpDestination.Controls.Add(this.lblThreads);
            this.grpDestination.Controls.Add(this.chkDeleteTempFiles);
            this.grpDestination.Controls.Add(this.lblOutputFile);
            this.grpDestination.Controls.Add(this.btnDestinationBrowse);
            this.grpDestination.Controls.Add(this.txtOutputFile);
            this.grpDestination.Location = new System.Drawing.Point(12, 274);
            this.grpDestination.Name = "grpDestination";
            this.grpDestination.Size = new System.Drawing.Size(556, 78);
            this.grpDestination.TabIndex = 1;
            this.grpDestination.TabStop = false;
            this.grpDestination.Text = "Destination:";
            // 
            // tbCores
            // 
            this.tbCores.AutoSize = false;
            this.tbCores.Location = new System.Drawing.Point(490, 45);
            this.tbCores.Maximum = 8;
            this.tbCores.Minimum = 1;
            this.tbCores.Name = "tbCores";
            this.tbCores.Size = new System.Drawing.Size(60, 29);
            this.tbCores.TabIndex = 16;
            this.tbCores.Value = 2;
            this.tbCores.Scroll += new System.EventHandler(this.tbCores_Scroll);
            // 
            // txtCores
            // 
            this.txtCores.Location = new System.Drawing.Point(459, 47);
            this.txtCores.Name = "txtCores";
            this.txtCores.ReadOnly = true;
            this.txtCores.Size = new System.Drawing.Size(25, 20);
            this.txtCores.TabIndex = 15;
            this.txtCores.Text = "2";
            // 
            // lblThreads
            // 
            this.lblThreads.AutoSize = true;
            this.lblThreads.Location = new System.Drawing.Point(404, 49);
            this.lblThreads.Name = "lblThreads";
            this.lblThreads.Size = new System.Drawing.Size(49, 13);
            this.lblThreads.TabIndex = 14;
            this.lblThreads.Text = "Threads:";
            // 
            // chkDeleteTempFiles
            // 
            this.chkDeleteTempFiles.AutoSize = true;
            this.chkDeleteTempFiles.Checked = true;
            this.chkDeleteTempFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeleteTempFiles.Location = new System.Drawing.Point(15, 48);
            this.chkDeleteTempFiles.Name = "chkDeleteTempFiles";
            this.chkDeleteTempFiles.Size = new System.Drawing.Size(206, 17);
            this.chkDeleteTempFiles.TabIndex = 9;
            this.chkDeleteTempFiles.Text = "Delete temporary files after conversion";
            this.chkDeleteTempFiles.UseVisualStyleBackColor = true;
            // 
            // lblOutputFile
            // 
            this.lblOutputFile.AutoSize = true;
            this.lblOutputFile.Location = new System.Drawing.Point(13, 23);
            this.lblOutputFile.Name = "lblOutputFile";
            this.lblOutputFile.Size = new System.Drawing.Size(58, 13);
            this.lblOutputFile.TabIndex = 6;
            this.lblOutputFile.Text = "Output file:";
            // 
            // btnDestinationBrowse
            // 
            this.btnDestinationBrowse.Location = new System.Drawing.Point(500, 19);
            this.btnDestinationBrowse.Name = "btnDestinationBrowse";
            this.btnDestinationBrowse.Size = new System.Drawing.Size(50, 21);
            this.btnDestinationBrowse.TabIndex = 4;
            this.btnDestinationBrowse.Text = "...";
            this.btnDestinationBrowse.UseVisualStyleBackColor = true;
            this.btnDestinationBrowse.Click += new System.EventHandler(this.btnDestinationBrowse_Click);
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.Location = new System.Drawing.Point(103, 20);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.ReadOnly = true;
            this.txtOutputFile.Size = new System.Drawing.Size(391, 20);
            this.txtOutputFile.TabIndex = 3;
            // 
            // lblQualityWorst
            // 
            this.lblQualityWorst.AutoSize = true;
            this.lblQualityWorst.Location = new System.Drawing.Point(243, 76);
            this.lblQualityWorst.Name = "lblQualityWorst";
            this.lblQualityWorst.Size = new System.Drawing.Size(35, 13);
            this.lblQualityWorst.TabIndex = 13;
            this.lblQualityWorst.Text = "Worst";
            // 
            // lblQualityBest
            // 
            this.lblQualityBest.AutoSize = true;
            this.lblQualityBest.Location = new System.Drawing.Point(22, 76);
            this.lblQualityBest.Name = "lblQualityBest";
            this.lblQualityBest.Size = new System.Drawing.Size(28, 13);
            this.lblQualityBest.TabIndex = 12;
            this.lblQualityBest.Text = "Best";
            // 
            // numFps
            // 
            this.numFps.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numFps.Location = new System.Drawing.Point(42, 48);
            this.numFps.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numFps.Name = "numFps";
            this.numFps.Size = new System.Drawing.Size(59, 20);
            this.numFps.TabIndex = 11;
            this.numFps.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numFps.Visible = false;
            // 
            // lblFPS
            // 
            this.lblFPS.AutoSize = true;
            this.lblFPS.Location = new System.Drawing.Point(6, 50);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(30, 13);
            this.lblFPS.TabIndex = 10;
            this.lblFPS.Text = "FPS:";
            this.lblFPS.Visible = false;
            // 
            // lblQuality
            // 
            this.lblQuality.AutoSize = true;
            this.lblQuality.Location = new System.Drawing.Point(6, 50);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(72, 13);
            this.lblQuality.TabIndex = 8;
            this.lblQuality.Text = "Video Quality:";
            // 
            // tbQuality
            // 
            this.tbQuality.AutoSize = false;
            this.tbQuality.Location = new System.Drawing.Point(56, 71);
            this.tbQuality.Maximum = 31;
            this.tbQuality.Minimum = 1;
            this.tbQuality.Name = "tbQuality";
            this.tbQuality.Size = new System.Drawing.Size(181, 29);
            this.tbQuality.TabIndex = 7;
            this.tbQuality.Value = 5;
            this.tbQuality.Scroll += new System.EventHandler(this.tbQuality_Scroll);
            // 
            // txtQuality
            // 
            this.txtQuality.Location = new System.Drawing.Point(80, 48);
            this.txtQuality.Name = "txtQuality";
            this.txtQuality.ReadOnly = true;
            this.txtQuality.Size = new System.Drawing.Size(36, 20);
            this.txtQuality.TabIndex = 4;
            this.txtQuality.Text = "5";
            this.txtQuality.TextChanged += new System.EventHandler(this.txtYoutube_TextChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Video Files (avi/flv/mpg/...)|*.avi;*.mpg;*.mpeg;*.flv;*.mp4;*.wmv";
            this.openFileDialog.Title = "Choose source file";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "AVI files (*.avi)|*.avi";
            this.saveFileDialog.Title = "Output AVI file (suggested to name it in the format (XXX_####.avi)";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 358);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(556, 20);
            this.progressBar.TabIndex = 2;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(484, 392);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(84, 26);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 392);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(84, 26);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(134, 392);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(344, 26);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status:";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFfmpegDialog
            // 
            this.openFfmpegDialog.Filter = "ffmpeg.exe|ffmpeg.exe";
            this.openFfmpegDialog.Title = "Find ffmpeg.exe";
            // 
            // btnSet
            // 
            this.btnSet.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSet.Image = global::_3DSExplorer.Properties.Resources.cog;
            this.btnSet.Location = new System.Drawing.Point(102, 392);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(26, 26);
            this.btnSet.TabIndex = 7;
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSetLocation_Click);
            // 
            // grpVideo
            // 
            this.grpVideo.Controls.Add(this.lblKbps);
            this.grpVideo.Controls.Add(this.txtVideoBitrate);
            this.grpVideo.Controls.Add(this.lblVideoBitRate);
            this.grpVideo.Controls.Add(this.chkAdvanced);
            this.grpVideo.Controls.Add(this.lvlOutputRes);
            this.grpVideo.Controls.Add(this.lblQuality);
            this.grpVideo.Controls.Add(this.lblQualityWorst);
            this.grpVideo.Controls.Add(this.numFps);
            this.grpVideo.Controls.Add(this.txtQuality);
            this.grpVideo.Controls.Add(this.lblFPS);
            this.grpVideo.Controls.Add(this.tbQuality);
            this.grpVideo.Controls.Add(this.lblQualityBest);
            this.grpVideo.Location = new System.Drawing.Point(12, 153);
            this.grpVideo.Name = "grpVideo";
            this.grpVideo.Size = new System.Drawing.Size(285, 115);
            this.grpVideo.TabIndex = 17;
            this.grpVideo.TabStop = false;
            this.grpVideo.Text = "Video Options:";
            // 
            // lblKbps
            // 
            this.lblKbps.AutoSize = true;
            this.lblKbps.Location = new System.Drawing.Point(239, 50);
            this.lblKbps.Name = "lblKbps";
            this.lblKbps.Size = new System.Drawing.Size(30, 13);
            this.lblKbps.TabIndex = 17;
            this.lblKbps.Text = "kbps";
            this.lblKbps.Visible = false;
            // 
            // txtVideoBitrate
            // 
            this.txtVideoBitrate.Location = new System.Drawing.Point(164, 47);
            this.txtVideoBitrate.Name = "txtVideoBitrate";
            this.txtVideoBitrate.Size = new System.Drawing.Size(73, 20);
            this.txtVideoBitrate.TabIndex = 16;
            this.txtVideoBitrate.Text = "2000";
            this.txtVideoBitrate.Visible = false;
            // 
            // lblVideoBitRate
            // 
            this.lblVideoBitRate.AutoSize = true;
            this.lblVideoBitRate.Location = new System.Drawing.Point(122, 50);
            this.lblVideoBitRate.Name = "lblVideoBitRate";
            this.lblVideoBitRate.Size = new System.Drawing.Size(40, 13);
            this.lblVideoBitRate.TabIndex = 15;
            this.lblVideoBitRate.Text = "Bitrate:";
            this.lblVideoBitRate.Visible = false;
            // 
            // chkAdvanced
            // 
            this.chkAdvanced.AutoSize = true;
            this.chkAdvanced.Location = new System.Drawing.Point(9, 24);
            this.chkAdvanced.Name = "chkAdvanced";
            this.chkAdvanced.Size = new System.Drawing.Size(75, 17);
            this.chkAdvanced.TabIndex = 14;
            this.chkAdvanced.Text = "Advanced";
            this.chkAdvanced.UseVisualStyleBackColor = true;
            this.chkAdvanced.CheckedChanged += new System.EventHandler(this.chkAdvanced_CheckedChanged);
            // 
            // grpAudio
            // 
            this.grpAudio.Controls.Add(this.cmbSampleRate);
            this.grpAudio.Controls.Add(this.lblSampleRate);
            this.grpAudio.Controls.Add(this.lblVolumeDefault);
            this.grpAudio.Controls.Add(this.lblAudioKbps);
            this.grpAudio.Controls.Add(this.lblVolume);
            this.grpAudio.Controls.Add(this.txtAudioBitrate);
            this.grpAudio.Controls.Add(this.lblAudioBitrate);
            this.grpAudio.Controls.Add(this.numVolume);
            this.grpAudio.Location = new System.Drawing.Point(303, 153);
            this.grpAudio.Name = "grpAudio";
            this.grpAudio.Size = new System.Drawing.Size(265, 115);
            this.grpAudio.TabIndex = 17;
            this.grpAudio.TabStop = false;
            this.grpAudio.Text = "Audio Options:";
            // 
            // lblVolumeDefault
            // 
            this.lblVolumeDefault.AutoSize = true;
            this.lblVolumeDefault.Location = new System.Drawing.Point(135, 25);
            this.lblVolumeDefault.Name = "lblVolumeDefault";
            this.lblVolumeDefault.Size = new System.Drawing.Size(71, 13);
            this.lblVolumeDefault.TabIndex = 17;
            this.lblVolumeDefault.Text = "[Default: 256]";
            // 
            // lblAudioKbps
            // 
            this.lblAudioKbps.AutoSize = true;
            this.lblAudioKbps.Location = new System.Drawing.Point(135, 54);
            this.lblAudioKbps.Name = "lblAudioKbps";
            this.lblAudioKbps.Size = new System.Drawing.Size(30, 13);
            this.lblAudioKbps.TabIndex = 17;
            this.lblAudioKbps.Text = "kbps";
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(18, 25);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(45, 13);
            this.lblVolume.TabIndex = 15;
            this.lblVolume.Text = "Volume:";
            // 
            // txtAudioBitrate
            // 
            this.txtAudioBitrate.Location = new System.Drawing.Point(69, 51);
            this.txtAudioBitrate.Name = "txtAudioBitrate";
            this.txtAudioBitrate.Size = new System.Drawing.Size(59, 20);
            this.txtAudioBitrate.TabIndex = 16;
            this.txtAudioBitrate.Text = "96";
            // 
            // lblAudioBitrate
            // 
            this.lblAudioBitrate.AutoSize = true;
            this.lblAudioBitrate.Location = new System.Drawing.Point(18, 54);
            this.lblAudioBitrate.Name = "lblAudioBitrate";
            this.lblAudioBitrate.Size = new System.Drawing.Size(40, 13);
            this.lblAudioBitrate.TabIndex = 15;
            this.lblAudioBitrate.Text = "Bitrate:";
            // 
            // numVolume
            // 
            this.numVolume.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numVolume.Location = new System.Drawing.Point(69, 23);
            this.numVolume.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numVolume.Name = "numVolume";
            this.numVolume.Size = new System.Drawing.Size(59, 20);
            this.numVolume.TabIndex = 11;
            this.numVolume.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            // 
            // chkSplit
            // 
            this.chkSplit.AutoSize = true;
            this.chkSplit.Checked = true;
            this.chkSplit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSplit.Location = new System.Drawing.Point(232, 48);
            this.chkSplit.Name = "chkSplit";
            this.chkSplit.Size = new System.Drawing.Size(160, 17);
            this.chkSplit.TabIndex = 17;
            this.chkSplit.Text = "Split to                 sec videos";
            this.chkSplit.UseVisualStyleBackColor = true;
            this.chkSplit.CheckedChanged += new System.EventHandler(this.chkSplit_CheckedChanged);
            // 
            // lblSampleRate
            // 
            this.lblSampleRate.AutoSize = true;
            this.lblSampleRate.Location = new System.Drawing.Point(18, 82);
            this.lblSampleRate.Name = "lblSampleRate";
            this.lblSampleRate.Size = new System.Drawing.Size(71, 13);
            this.lblSampleRate.TabIndex = 18;
            this.lblSampleRate.Text = "Sample Rate:";
            // 
            // cmbSampleRate
            // 
            this.cmbSampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSampleRate.FormattingEnabled = true;
            this.cmbSampleRate.Items.AddRange(new object[] {
            "8000",
            "11025",
            "16000",
            "22050",
            "44100",
            "48000",
            "96000"});
            this.cmbSampleRate.Location = new System.Drawing.Point(95, 79);
            this.cmbSampleRate.Name = "cmbSampleRate";
            this.cmbSampleRate.Size = new System.Drawing.Size(108, 21);
            this.cmbSampleRate.TabIndex = 19;
            // 
            // lvlOutputRes
            // 
            this.lvlOutputRes.AutoSize = true;
            this.lvlOutputRes.Location = new System.Drawing.Point(154, 16);
            this.lvlOutputRes.Name = "lvlOutputRes";
            this.lvlOutputRes.Size = new System.Drawing.Size(124, 13);
            this.lvlOutputRes.TabIndex = 5;
            this.lvlOutputRes.Text = "Video output is: 400x240";
            this.lvlOutputRes.Visible = false;
            // 
            // numLimit
            // 
            this.numLimit.Location = new System.Drawing.Point(287, 47);
            this.numLimit.Maximum = new decimal(new int[] {
            599,
            0,
            0,
            0});
            this.numLimit.Name = "numLimit";
            this.numLimit.Size = new System.Drawing.Size(43, 20);
            this.numLimit.TabIndex = 18;
            this.numLimit.Value = new decimal(new int[] {
            599,
            0,
            0,
            0});
            // 
            // frm3DVideo
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(581, 429);
            this.Controls.Add(this.grpAudio);
            this.Controls.Add(this.grpVideo);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.grpDestination);
            this.Controls.Add(this.grpSource);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm3DVideo";
            this.Text = "Create 3D Videos (Based on spinal_cord\'s idea, influenced by SifJar & amzg)";
            this.Activated += new System.EventHandler(this.frm3DVideo_Activated);
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picThumb)).EndInit();
            this.grpDestination.ResumeLayout(false);
            this.grpDestination.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbCores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).EndInit();
            this.grpVideo.ResumeLayout(false);
            this.grpVideo.PerformLayout();
            this.grpAudio.ResumeLayout(false);
            this.grpAudio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.TextBox txtYoutube;
        private System.Windows.Forms.Button btnSourceBrowse;
        private System.Windows.Forms.TextBox txtSourceFile;
        private System.Windows.Forms.RadioButton radSourceYoutube;
        private System.Windows.Forms.RadioButton radSourceFile;
        private System.Windows.Forms.Label lblYoutube;
        private System.Windows.Forms.GroupBox grpDestination;
        private System.Windows.Forms.Button btnDestinationBrowse;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblOutputFile;
        private System.Windows.Forms.CheckBox chk3D;
        private System.Windows.Forms.ComboBox cmbOrientation;
        private System.Windows.Forms.Label lblOrientation;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.TrackBar tbQuality;
        private System.Windows.Forms.TextBox txtQuality;
        private System.Windows.Forms.CheckBox chkDeleteTempFiles;
        private System.Windows.Forms.OpenFileDialog openFfmpegDialog;
        private System.Windows.Forms.PictureBox picThumb;
        private System.Windows.Forms.NumericUpDown numFps;
        private System.Windows.Forms.Label lblFPS;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Label lblQualityWorst;
        private System.Windows.Forms.Label lblQualityBest;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TrackBar tbCores;
        private System.Windows.Forms.TextBox txtCores;
        private System.Windows.Forms.Label lblThreads;
        private System.Windows.Forms.GroupBox grpVideo;
        private System.Windows.Forms.Label lblKbps;
        private System.Windows.Forms.TextBox txtVideoBitrate;
        private System.Windows.Forms.Label lblVideoBitRate;
        private System.Windows.Forms.CheckBox chkAdvanced;
        private System.Windows.Forms.GroupBox grpAudio;
        private System.Windows.Forms.Label lblVolumeDefault;
        private System.Windows.Forms.Label lblAudioKbps;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.TextBox txtAudioBitrate;
        private System.Windows.Forms.Label lblAudioBitrate;
        private System.Windows.Forms.NumericUpDown numVolume;
        private System.Windows.Forms.CheckBox chkSplit;
        private System.Windows.Forms.ComboBox cmbSampleRate;
        private System.Windows.Forms.Label lblSampleRate;
        private System.Windows.Forms.Label lvlOutputRes;
        private System.Windows.Forms.NumericUpDown numLimit;
    }
}