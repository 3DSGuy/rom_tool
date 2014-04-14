namespace _3DSExplorer
{
    partial class frmDSDecmpGUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDSDecmpGUI));
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.numOffset = new System.Windows.Forms.NumericUpDown();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.lblOffset = new System.Windows.Forms.Label();
            this.lblLength = new System.Windows.Forms.Label();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.btnFileSize = new System.Windows.Forms.Button();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.btnCompress = new System.Windows.Forms.Button();
            this.btnDecompress = new System.Windows.Forms.Button();
            this.lblAlgorithm = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFilePath
            // 
            this.txtFilePath.AllowDrop = true;
            this.txtFilePath.Location = new System.Drawing.Point(19, 36);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(248, 20);
            this.txtFilePath.TabIndex = 0;
            this.txtFilePath.Text = "(you can drag a file in here)";
            this.txtFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileDragDrop);
            this.txtFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileDragEnter);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(273, 32);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(87, 26);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // numOffset
            // 
            this.numOffset.Location = new System.Drawing.Point(117, 70);
            this.numOffset.Maximum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.numOffset.Name = "numOffset";
            this.numOffset.Size = new System.Drawing.Size(100, 20);
            this.numOffset.TabIndex = 2;
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(16, 15);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(88, 13);
            this.lblFilePath.TabIndex = 3;
            this.lblFilePath.Text = "Choose input file:";
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.Location = new System.Drawing.Point(16, 72);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(65, 13);
            this.lblOffset.TabIndex = 4;
            this.lblOffset.Text = "Offset in file:";
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(16, 106);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(95, 13);
            this.lblLength.TabIndex = 5;
            this.lblLength.Text = "Length to process:";
            // 
            // numLength
            // 
            this.numLength.Location = new System.Drawing.Point(117, 104);
            this.numLength.Maximum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.numLength.Name = "numLength";
            this.numLength.Size = new System.Drawing.Size(100, 20);
            this.numLength.TabIndex = 6;
            // 
            // btnFileSize
            // 
            this.btnFileSize.Location = new System.Drawing.Point(223, 102);
            this.btnFileSize.Name = "btnFileSize";
            this.btnFileSize.Size = new System.Drawing.Size(69, 22);
            this.btnFileSize.TabIndex = 7;
            this.btnFileSize.Text = "File\'s size";
            this.btnFileSize.UseVisualStyleBackColor = true;
            this.btnFileSize.Click += new System.EventHandler(this.btnFileSize_Click);
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Items.AddRange(new object[] {
            "Huffman4",
            "Huffman8",
            "LZ10",
            "LZ11",
            "LZOvl"});
            this.cmbAlgorithm.Location = new System.Drawing.Point(19, 192);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(123, 21);
            this.cmbAlgorithm.TabIndex = 8;
            // 
            // btnCompress
            // 
            this.btnCompress.Enabled = false;
            this.btnCompress.Location = new System.Drawing.Point(37, 219);
            this.btnCompress.Name = "btnCompress";
            this.btnCompress.Size = new System.Drawing.Size(87, 26);
            this.btnCompress.TabIndex = 9;
            this.btnCompress.Text = "Compress";
            this.btnCompress.UseVisualStyleBackColor = true;
            this.btnCompress.Click += new System.EventHandler(this.btnCompress_Click);
            // 
            // btnDecompress
            // 
            this.btnDecompress.Enabled = false;
            this.btnDecompress.Location = new System.Drawing.Point(235, 219);
            this.btnDecompress.Name = "btnDecompress";
            this.btnDecompress.Size = new System.Drawing.Size(87, 26);
            this.btnDecompress.TabIndex = 10;
            this.btnDecompress.Text = "Deompress";
            this.btnDecompress.UseVisualStyleBackColor = true;
            this.btnDecompress.Click += new System.EventHandler(this.btnDecompress_Click);
            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.AutoSize = true;
            this.lblAlgorithm.Location = new System.Drawing.Point(16, 171);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(53, 13);
            this.lblAlgorithm.TabIndex = 11;
            this.lblAlgorithm.Text = "Algorithm:";
            // 
            // frmDSDecmpGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 268);
            this.Controls.Add(this.lblAlgorithm);
            this.Controls.Add(this.btnDecompress);
            this.Controls.Add(this.btnCompress);
            this.Controls.Add(this.cmbAlgorithm);
            this.Controls.Add(this.btnFileSize);
            this.Controls.Add(this.numLength);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.lblOffset);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.numOffset);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDSDecmpGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DSDecmp GUI";
            this.Load += new System.EventHandler(this.frmDSDecmpGUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.NumericUpDown numOffset;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.Button btnFileSize;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.Button btnDecompress;
        private System.Windows.Forms.Label lblAlgorithm;
    }
}