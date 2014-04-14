namespace _3DSExplorer
{
    partial class frmXORTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmXORTool));
            this.txtFirst = new System.Windows.Forms.TextBox();
            this.btnFirst = new System.Windows.Forms.Button();
            this.txtSecond = new System.Windows.Forms.TextBox();
            this.btnSecond = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupXorArrays = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBoxResult = new System.Windows.Forms.TextBox();
            this.btnXorArrays = new System.Windows.Forms.Button();
            this.txtBox2 = new System.Windows.Forms.TextBox();
            this.txtBox1 = new System.Windows.Forms.TextBox();
            this.groupXorFiles = new System.Windows.Forms.GroupBox();
            this.groupAes = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtIV = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDecData = new System.Windows.Forms.TextBox();
            this.btnAesGo = new System.Windows.Forms.Button();
            this.txtEncData = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupXorArrays.SuspendLayout();
            this.groupXorFiles.SuspendLayout();
            this.groupAes.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFirst
            // 
            this.txtFirst.AllowDrop = true;
            this.txtFirst.Location = new System.Drawing.Point(12, 92);
            this.txtFirst.Name = "txtFirst";
            this.txtFirst.ReadOnly = true;
            this.txtFirst.Size = new System.Drawing.Size(221, 20);
            this.txtFirst.TabIndex = 0;
            this.txtFirst.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileDragDrop);
            this.txtFirst.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileDragEnter);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(239, 86);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(78, 26);
            this.btnFirst.TabIndex = 1;
            this.btnFirst.Text = "Browse...";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // txtSecond
            // 
            this.txtSecond.AllowDrop = true;
            this.txtSecond.Location = new System.Drawing.Point(12, 146);
            this.txtSecond.Name = "txtSecond";
            this.txtSecond.ReadOnly = true;
            this.txtSecond.Size = new System.Drawing.Size(221, 20);
            this.txtSecond.TabIndex = 2;
            this.txtSecond.DragDrop += new System.Windows.Forms.DragEventHandler(this.FileDragDrop);
            this.txtSecond.DragEnter += new System.Windows.Forms.DragEventHandler(this.FileDragEnter);
            // 
            // btnSecond
            // 
            this.btnSecond.Location = new System.Drawing.Point(239, 140);
            this.btnSecond.Name = "btnSecond";
            this.btnSecond.Size = new System.Drawing.Size(78, 26);
            this.btnSecond.TabIndex = 3;
            this.btnSecond.Text = "Browse...";
            this.btnSecond.UseVisualStyleBackColor = true;
            this.btnSecond.Click += new System.EventHandler(this.btnSecond_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(346, 140);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 26);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(755, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 26);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "XORed files(*.xor)|*.xor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "First File:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Second File:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(227, 26);
            this.label3.TabIndex = 9;
            this.label3.Text = "The output file will be the size of the bigger file,\r\nand xored by the smaller fi" +
                "le repeatedly.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 386);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(845, 55);
            this.panel1.TabIndex = 10;
            // 
            // groupXorArrays
            // 
            this.groupXorArrays.Controls.Add(this.label6);
            this.groupXorArrays.Controls.Add(this.label5);
            this.groupXorArrays.Controls.Add(this.label4);
            this.groupXorArrays.Controls.Add(this.txtBoxResult);
            this.groupXorArrays.Controls.Add(this.btnXorArrays);
            this.groupXorArrays.Controls.Add(this.txtBox2);
            this.groupXorArrays.Controls.Add(this.txtBox1);
            this.groupXorArrays.Location = new System.Drawing.Point(12, 210);
            this.groupXorArrays.Name = "groupXorArrays";
            this.groupXorArrays.Size = new System.Drawing.Size(441, 156);
            this.groupXorArrays.TabIndex = 11;
            this.groupXorArrays.TabStop = false;
            this.groupXorArrays.Text = "Xor 2 Byte Arrays";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(386, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Enter two byte arrays of the same size with hex letters, then press the Xor butto" +
                "n.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Second Byte Array:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "First Byte Array:";
            // 
            // txtBoxResult
            // 
            this.txtBoxResult.Location = new System.Drawing.Point(63, 117);
            this.txtBoxResult.MaxLength = 512;
            this.txtBoxResult.Name = "txtBoxResult";
            this.txtBoxResult.ReadOnly = true;
            this.txtBoxResult.Size = new System.Drawing.Size(357, 20);
            this.txtBoxResult.TabIndex = 3;
            // 
            // btnXorArrays
            // 
            this.btnXorArrays.Location = new System.Drawing.Point(6, 113);
            this.btnXorArrays.Name = "btnXorArrays";
            this.btnXorArrays.Size = new System.Drawing.Size(51, 26);
            this.btnXorArrays.TabIndex = 2;
            this.btnXorArrays.Text = "XOR";
            this.btnXorArrays.UseVisualStyleBackColor = true;
            this.btnXorArrays.Click += new System.EventHandler(this.btnXorArrays_Click);
            // 
            // txtBox2
            // 
            this.txtBox2.Location = new System.Drawing.Point(110, 81);
            this.txtBox2.MaxLength = 512;
            this.txtBox2.Name = "txtBox2";
            this.txtBox2.Size = new System.Drawing.Size(310, 20);
            this.txtBox2.TabIndex = 1;
            // 
            // txtBox1
            // 
            this.txtBox1.Location = new System.Drawing.Point(109, 48);
            this.txtBox1.MaxLength = 512;
            this.txtBox1.Name = "txtBox1";
            this.txtBox1.Size = new System.Drawing.Size(311, 20);
            this.txtBox1.TabIndex = 0;
            // 
            // groupXorFiles
            // 
            this.groupXorFiles.Controls.Add(this.label3);
            this.groupXorFiles.Controls.Add(this.txtFirst);
            this.groupXorFiles.Controls.Add(this.btnFirst);
            this.groupXorFiles.Controls.Add(this.btnSave);
            this.groupXorFiles.Controls.Add(this.txtSecond);
            this.groupXorFiles.Controls.Add(this.btnSecond);
            this.groupXorFiles.Controls.Add(this.label2);
            this.groupXorFiles.Controls.Add(this.label1);
            this.groupXorFiles.Location = new System.Drawing.Point(12, 12);
            this.groupXorFiles.Name = "groupXorFiles";
            this.groupXorFiles.Size = new System.Drawing.Size(440, 192);
            this.groupXorFiles.TabIndex = 12;
            this.groupXorFiles.TabStop = false;
            this.groupXorFiles.Text = "Xor 2 Files";
            // 
            // groupAes
            // 
            this.groupAes.Controls.Add(this.label10);
            this.groupAes.Controls.Add(this.txtIV);
            this.groupAes.Controls.Add(this.label7);
            this.groupAes.Controls.Add(this.label8);
            this.groupAes.Controls.Add(this.label9);
            this.groupAes.Controls.Add(this.txtDecData);
            this.groupAes.Controls.Add(this.btnAesGo);
            this.groupAes.Controls.Add(this.txtEncData);
            this.groupAes.Controls.Add(this.txtKey);
            this.groupAes.Location = new System.Drawing.Point(458, 12);
            this.groupAes.Name = "groupAes";
            this.groupAes.Size = new System.Drawing.Size(378, 354);
            this.groupAes.TabIndex = 13;
            this.groupAes.TabStop = false;
            this.groupAes.Text = "AES-CTR Sandbox";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "IV:";
            // 
            // txtIV
            // 
            this.txtIV.Location = new System.Drawing.Point(55, 77);
            this.txtIV.MaxLength = 32;
            this.txtIV.Name = "txtIV";
            this.txtIV.Size = new System.Drawing.Size(311, 20);
            this.txtIV.TabIndex = 11;
            this.txtIV.Text = "0000000000000000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(358, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Enter 16-byte key and IV and an encrypted data byte array, then press Go.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(249, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Encrypted Data (limit of 256bytes = 512 hex-letters):";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(28, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Key:";
            // 
            // txtDecData
            // 
            this.txtDecData.Location = new System.Drawing.Point(9, 200);
            this.txtDecData.MaxLength = 512;
            this.txtDecData.Multiline = true;
            this.txtDecData.Name = "txtDecData";
            this.txtDecData.ReadOnly = true;
            this.txtDecData.Size = new System.Drawing.Size(357, 137);
            this.txtDecData.TabIndex = 3;
            // 
            // btnAesGo
            // 
            this.btnAesGo.Location = new System.Drawing.Point(6, 168);
            this.btnAesGo.Name = "btnAesGo";
            this.btnAesGo.Size = new System.Drawing.Size(51, 26);
            this.btnAesGo.TabIndex = 2;
            this.btnAesGo.Text = "Go";
            this.btnAesGo.UseVisualStyleBackColor = true;
            this.btnAesGo.Click += new System.EventHandler(this.btnAesGo_Click);
            // 
            // txtEncData
            // 
            this.txtEncData.Location = new System.Drawing.Point(9, 120);
            this.txtEncData.MaxLength = 512;
            this.txtEncData.Multiline = true;
            this.txtEncData.Name = "txtEncData";
            this.txtEncData.Size = new System.Drawing.Size(357, 42);
            this.txtEncData.TabIndex = 1;
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(55, 51);
            this.txtKey.MaxLength = 32;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(311, 20);
            this.txtKey.TabIndex = 0;
            // 
            // frmXORTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(845, 441);
            this.Controls.Add(this.groupAes);
            this.Controls.Add(this.groupXorFiles);
            this.Controls.Add(this.groupXorArrays);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmXORTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "XOR Tool";
            this.panel1.ResumeLayout(false);
            this.groupXorArrays.ResumeLayout(false);
            this.groupXorArrays.PerformLayout();
            this.groupXorFiles.ResumeLayout(false);
            this.groupXorFiles.PerformLayout();
            this.groupAes.ResumeLayout(false);
            this.groupAes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtFirst;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.TextBox txtSecond;
        private System.Windows.Forms.Button btnSecond;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupXorArrays;
        private System.Windows.Forms.TextBox txtBoxResult;
        private System.Windows.Forms.Button btnXorArrays;
        private System.Windows.Forms.TextBox txtBox2;
        private System.Windows.Forms.TextBox txtBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupXorFiles;
        private System.Windows.Forms.GroupBox groupAes;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtIV;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtDecData;
        private System.Windows.Forms.Button btnAesGo;
        private System.Windows.Forms.TextBox txtEncData;
        private System.Windows.Forms.TextBox txtKey;

    }
}