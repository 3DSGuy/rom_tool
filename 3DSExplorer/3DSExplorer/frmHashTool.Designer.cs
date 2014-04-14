namespace _3DSExplorer
{
    partial class frmHashTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHashTool));
            this.txtList = new System.Windows.Forms.TextBox();
            this.btnCompute = new System.Windows.Forms.Button();
            this.lblSize = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.cbAlgo = new System.Windows.Forms.ComboBox();
            this.lblAlgo = new System.Windows.Forms.Label();
            this.txtOffset = new System.Windows.Forms.TextBox();
            this.lblOffset = new System.Windows.Forms.Label();
            this.txtBlocks = new System.Windows.Forms.TextBox();
            this.lblBlocks = new System.Windows.Forms.Label();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnBrute = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtSize = new System.Windows.Forms.ComboBox();
            this.cbOption = new System.Windows.Forms.ComboBox();
            this.btnSuperBrute = new System.Windows.Forms.Button();
            this.superBruteForce = new System.ComponentModel.BackgroundWorker();
            this.subProgressBar = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbComputeBlockSize = new System.Windows.Forms.ComboBox();
            this.lblHashesSize = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.picTool = new System.Windows.Forms.PictureBox();
            this.chkHighCPU = new System.Windows.Forms.CheckBox();
            this.lblSuper = new System.Windows.Forms.Label();
            this.lblFilename = new System.Windows.Forms.Label();
            this.chkEntireFile = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTool)).BeginInit();
            this.SuspendLayout();
            // 
            // txtList
            // 
            this.txtList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtList.HideSelection = false;
            this.txtList.Location = new System.Drawing.Point(10, 151);
            this.txtList.Multiline = true;
            this.txtList.Name = "txtList";
            this.txtList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtList.Size = new System.Drawing.Size(601, 238);
            this.txtList.TabIndex = 0;
            this.txtList.WordWrap = false;
            // 
            // btnCompute
            // 
            this.btnCompute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompute.Enabled = false;
            this.btnCompute.Location = new System.Drawing.Point(517, 4);
            this.btnCompute.Name = "btnCompute";
            this.btnCompute.Size = new System.Drawing.Size(78, 24);
            this.btnCompute.TabIndex = 1;
            this.btnCompute.Text = "Compute";
            this.btnCompute.UseVisualStyleBackColor = true;
            this.btnCompute.Click += new System.EventHandler(this.btnCompute_Click);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(9, 35);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(58, 13);
            this.lblSize.TabIndex = 2;
            this.lblSize.Text = "Block size:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(10, 418);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(600, 17);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 4;
            // 
            // cbAlgo
            // 
            this.cbAlgo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAlgo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlgo.FormattingEnabled = true;
            this.cbAlgo.Items.AddRange(new object[] {
            "SHA-2, 256bit",
            "SHA-2, 512bit",
            "SHA-1, 160bit",
            "MD-5, 128bit",
            "CRC16 Modbus",
            "CRC32"});
            this.cbAlgo.Location = new System.Drawing.Point(415, 14);
            this.cbAlgo.Name = "cbAlgo";
            this.cbAlgo.Size = new System.Drawing.Size(107, 21);
            this.cbAlgo.TabIndex = 5;
            // 
            // lblAlgo
            // 
            this.lblAlgo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAlgo.AutoSize = true;
            this.lblAlgo.Location = new System.Drawing.Point(356, 17);
            this.lblAlgo.Name = "lblAlgo";
            this.lblAlgo.Size = new System.Drawing.Size(53, 13);
            this.lblAlgo.TabIndex = 6;
            this.lblAlgo.Text = "Algorithm:";
            // 
            // txtOffset
            // 
            this.txtOffset.Location = new System.Drawing.Point(307, 7);
            this.txtOffset.Name = "txtOffset";
            this.txtOffset.Size = new System.Drawing.Size(69, 20);
            this.txtOffset.TabIndex = 7;
            this.txtOffset.Text = "0";
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(217, 10);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(84, 13);
            this.lblOffset.TabIndex = 8;
            this.lblOffset.Text = "Start from offset:";
            // 
            // txtBlocks
            // 
            this.txtBlocks.Location = new System.Drawing.Point(152, 7);
            this.txtBlocks.Name = "txtBlocks";
            this.txtBlocks.Size = new System.Drawing.Size(57, 20);
            this.txtBlocks.TabIndex = 9;
            this.txtBlocks.Text = "-1";
            // 
            // lblBlocks
            // 
            this.lblBlocks.AutoSize = true;
            this.lblBlocks.Location = new System.Drawing.Point(82, 10);
            this.lblBlocks.Name = "lblBlocks";
            this.lblBlocks.Size = new System.Drawing.Size(64, 13);
            this.lblBlocks.TabIndex = 10;
            this.lblBlocks.Text = "# of Blocks:";
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(9, 9);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(59, 13);
            this.lblSearch.TabIndex = 11;
            this.lblSearch.Text = "Search for:";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Courier New", 7.5F);
            this.txtSearch.Location = new System.Drawing.Point(79, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(339, 19);
            this.txtSearch.TabIndex = 12;
            // 
            // btnBrute
            // 
            this.btnBrute.Enabled = false;
            this.btnBrute.Location = new System.Drawing.Point(150, 32);
            this.btnBrute.Name = "btnBrute";
            this.btnBrute.Size = new System.Drawing.Size(130, 24);
            this.btnBrute.TabIndex = 13;
            this.btnBrute.Text = "Brute-Search for Hash";
            this.btnBrute.UseVisualStyleBackColor = true;
            this.btnBrute.Click += new System.EventHandler(this.btnBrute_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(10, 11);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(86, 24);
            this.btnOpen.TabIndex = 14;
            this.btnOpen.Text = "&Open File...";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtSize
            // 
            this.txtSize.FormattingEnabled = true;
            this.txtSize.Items.AddRange(new object[] {
            "256",
            "512",
            "1024",
            "2048",
            "4096",
            "8192"});
            this.txtSize.Location = new System.Drawing.Point(79, 32);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(65, 21);
            this.txtSize.TabIndex = 15;
            this.txtSize.Text = "512";
            // 
            // cbOption
            // 
            this.cbOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOption.FormattingEnabled = true;
            this.cbOption.Items.AddRange(new object[] {
            "Regular",
            "CNG",
            "HMAC"});
            this.cbOption.Location = new System.Drawing.Point(529, 14);
            this.cbOption.Name = "cbOption";
            this.cbOption.Size = new System.Drawing.Size(84, 21);
            this.cbOption.TabIndex = 17;
            // 
            // btnSuperBrute
            // 
            this.btnSuperBrute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSuperBrute.Enabled = false;
            this.btnSuperBrute.Location = new System.Drawing.Point(465, 32);
            this.btnSuperBrute.Name = "btnSuperBrute";
            this.btnSuperBrute.Size = new System.Drawing.Size(130, 24);
            this.btnSuperBrute.TabIndex = 18;
            this.btnSuperBrute.Text = "Super Brute Force";
            this.btnSuperBrute.UseVisualStyleBackColor = true;
            this.btnSuperBrute.Click += new System.EventHandler(this.btnSuperBrute_Click);
            // 
            // superBruteForce
            // 
            this.superBruteForce.WorkerReportsProgress = true;
            this.superBruteForce.WorkerSupportsCancellation = true;
            this.superBruteForce.DoWork += new System.ComponentModel.DoWorkEventHandler(this.superBruteForce_DoWork);
            this.superBruteForce.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.superBruteForce_ProgressChanged);
            this.superBruteForce.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.superBruteForce_RunWorkerCompleted);
            // 
            // subProgressBar
            // 
            this.subProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subProgressBar.Location = new System.Drawing.Point(10, 395);
            this.subProgressBar.Name = "subProgressBar";
            this.subProgressBar.Size = new System.Drawing.Size(600, 17);
            this.subProgressBar.Step = 1;
            this.subProgressBar.TabIndex = 19;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(364, 32);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 24);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.chkEntireFile);
            this.panel1.Controls.Add(this.cbComputeBlockSize);
            this.panel1.Controls.Add(this.lblBlocks);
            this.panel1.Controls.Add(this.lblHashesSize);
            this.panel1.Controls.Add(this.txtOffset);
            this.panel1.Controls.Add(this.btnCompute);
            this.panel1.Controls.Add(this.txtBlocks);
            this.panel1.Controls.Add(this.lblOffset);
            this.panel1.Location = new System.Drawing.Point(12, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(598, 34);
            this.panel1.TabIndex = 21;
            // 
            // cbComputeBlockSize
            // 
            this.cbComputeBlockSize.FormattingEnabled = true;
            this.cbComputeBlockSize.Items.AddRange(new object[] {
            "256",
            "512",
            "1024",
            "2048",
            "4096",
            "8192"});
            this.cbComputeBlockSize.Location = new System.Drawing.Point(446, 7);
            this.cbComputeBlockSize.Name = "cbComputeBlockSize";
            this.cbComputeBlockSize.Size = new System.Drawing.Size(65, 21);
            this.cbComputeBlockSize.TabIndex = 23;
            this.cbComputeBlockSize.Text = "512";
            // 
            // lblHashesSize
            // 
            this.lblHashesSize.AutoSize = true;
            this.lblHashesSize.Location = new System.Drawing.Point(382, 10);
            this.lblHashesSize.Name = "lblHashesSize";
            this.lblHashesSize.Size = new System.Drawing.Size(58, 13);
            this.lblHashesSize.TabIndex = 22;
            this.lblHashesSize.Text = "Block size:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.picTool);
            this.panel2.Controls.Add(this.chkHighCPU);
            this.panel2.Controls.Add(this.lblSize);
            this.panel2.Controls.Add(this.lblSearch);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.txtSize);
            this.panel2.Controls.Add(this.btnBrute);
            this.panel2.Controls.Add(this.btnSuperBrute);
            this.panel2.Controls.Add(this.lblSuper);
            this.panel2.Location = new System.Drawing.Point(12, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(598, 64);
            this.panel2.TabIndex = 22;
            // 
            // picTool
            // 
            this.picTool.Image = global::_3DSExplorer.Properties.Resources.help;
            this.picTool.Location = new System.Drawing.Point(340, 35);
            this.picTool.Name = "picTool";
            this.picTool.Size = new System.Drawing.Size(16, 16);
            this.picTool.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTool.TabIndex = 25;
            this.picTool.TabStop = false;
            this.picTool.Click += new System.EventHandler(this.picTool_Click);
            // 
            // chkHighCPU
            // 
            this.chkHighCPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkHighCPU.AutoSize = true;
            this.chkHighCPU.Location = new System.Drawing.Point(436, 9);
            this.chkHighCPU.Name = "chkHighCPU";
            this.chkHighCPU.Size = new System.Drawing.Size(159, 17);
            this.chkHighCPU.TabIndex = 23;
            this.chkHighCPU.Text = "High CPU usage (no sleeps)";
            this.chkHighCPU.UseVisualStyleBackColor = true;
            // 
            // lblSuper
            // 
            this.lblSuper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSuper.AutoSize = true;
            this.lblSuper.Location = new System.Drawing.Point(372, 38);
            this.lblSuper.Name = "lblSuper";
            this.lblSuper.Size = new System.Drawing.Size(87, 13);
            this.lblSuper.TabIndex = 24;
            this.lblSuper.Text = "Every block size:";
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(102, 17);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(0, 13);
            this.lblFilename.TabIndex = 23;
            // 
            // chkEntireFile
            // 
            this.chkEntireFile.AutoSize = true;
            this.chkEntireFile.Location = new System.Drawing.Point(7, 9);
            this.chkEntireFile.Name = "chkEntireFile";
            this.chkEntireFile.Size = new System.Drawing.Size(69, 17);
            this.chkEntireFile.TabIndex = 24;
            this.chkEntireFile.Text = "Entire file";
            this.chkEntireFile.UseVisualStyleBackColor = true;
            this.chkEntireFile.CheckedChanged += new System.EventHandler(this.chkEntireFile_CheckedChanged);
            // 
            // frmHashTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.lblFilename);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.subProgressBar);
            this.Controls.Add(this.cbOption);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.lblAlgo);
            this.Controls.Add(this.cbAlgo);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.txtList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "frmHashTool";
            this.Text = "HashTool";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTool)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtList;
        private System.Windows.Forms.Button btnCompute;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox cbAlgo;
        private System.Windows.Forms.Label lblAlgo;
        private System.Windows.Forms.TextBox txtOffset;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.TextBox txtBlocks;
        private System.Windows.Forms.Label lblBlocks;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnBrute;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ComboBox txtSize;
        private System.Windows.Forms.ComboBox cbOption;
        private System.Windows.Forms.Button btnSuperBrute;
        private System.ComponentModel.BackgroundWorker superBruteForce;
        private System.Windows.Forms.ProgressBar subProgressBar;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbComputeBlockSize;
        private System.Windows.Forms.Label lblHashesSize;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkHighCPU;
        private System.Windows.Forms.Label lblSuper;
        private System.Windows.Forms.PictureBox picTool;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.CheckBox chkEntireFile;
    }
}