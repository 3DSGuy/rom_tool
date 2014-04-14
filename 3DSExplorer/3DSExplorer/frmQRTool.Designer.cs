namespace _3DSExplorer
{
    partial class frmQRTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQRTool));
            this.btnQRToBin = new System.Windows.Forms.Button();
            this.btnBinToQR = new System.Windows.Forms.Button();
            this.lblor0 = new System.Windows.Forms.Label();
            this.lblor1 = new System.Windows.Forms.Label();
            this.ButtonFromText = new System.Windows.Forms.Button();
            this.txtQRText = new System.Windows.Forms.TextBox();
            this.lblDragBinToQr = new System.Windows.Forms.Label();
            this.lblDragQrToBin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnQRToBin
            // 
            this.btnQRToBin.Location = new System.Drawing.Point(10, 7);
            this.btnQRToBin.Name = "btnQRToBin";
            this.btnQRToBin.Size = new System.Drawing.Size(47, 31);
            this.btnQRToBin.TabIndex = 0;
            this.btnQRToBin.Text = "Read";
            this.btnQRToBin.UseVisualStyleBackColor = true;
            this.btnQRToBin.Click += new System.EventHandler(this.QrToBin);
            // 
            // btnBinToQR
            // 
            this.btnBinToQR.Location = new System.Drawing.Point(10, 46);
            this.btnBinToQR.Name = "btnBinToQR";
            this.btnBinToQR.Size = new System.Drawing.Size(47, 31);
            this.btnBinToQR.TabIndex = 1;
            this.btnBinToQR.Text = "Make";
            this.btnBinToQR.UseVisualStyleBackColor = true;
            this.btnBinToQR.Click += new System.EventHandler(this.BinToQr);
            // 
            // lblor0
            // 
            this.lblor0.AutoSize = true;
            this.lblor0.Location = new System.Drawing.Point(62, 16);
            this.lblor0.Name = "lblor0";
            this.lblor0.Size = new System.Drawing.Size(16, 13);
            this.lblor0.TabIndex = 4;
            this.lblor0.Text = "or";
            // 
            // lblor1
            // 
            this.lblor1.AutoSize = true;
            this.lblor1.Location = new System.Drawing.Point(62, 55);
            this.lblor1.Name = "lblor1";
            this.lblor1.Size = new System.Drawing.Size(16, 13);
            this.lblor1.TabIndex = 4;
            this.lblor1.Text = "or";
            // 
            // ButtonFromText
            // 
            this.ButtonFromText.Location = new System.Drawing.Point(151, 89);
            this.ButtonFromText.Name = "ButtonFromText";
            this.ButtonFromText.Size = new System.Drawing.Size(62, 22);
            this.ButtonFromText.TabIndex = 7;
            this.ButtonFromText.Text = "From text";
            this.ButtonFromText.UseVisualStyleBackColor = true;
            this.ButtonFromText.Click += new System.EventHandler(this.ButtonFromTextClick);
            // 
            // txtQRText
            // 
            this.txtQRText.Location = new System.Drawing.Point(10, 90);
            this.txtQRText.Name = "txtQRText";
            this.txtQRText.Size = new System.Drawing.Size(139, 20);
            this.txtQRText.TabIndex = 10;
            // 
            // lblDragBinToQr
            // 
            this.lblDragBinToQr.AllowDrop = true;
            this.lblDragBinToQr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDragBinToQr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDragBinToQr.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold);
            this.lblDragBinToQr.Image = global::_3DSExplorer.Properties.Resources.text_align_left;
            this.lblDragBinToQr.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDragBinToQr.Location = new System.Drawing.Point(83, 48);
            this.lblDragBinToQr.Name = "lblDragBinToQr";
            this.lblDragBinToQr.Size = new System.Drawing.Size(130, 29);
            this.lblDragBinToQr.TabIndex = 3;
            this.lblDragBinToQr.Text = "Drag Bin Here";
            this.lblDragBinToQr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDragBinToQr.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDrop);
            this.lblDragBinToQr.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnter);
            // 
            // lblDragQrToBin
            // 
            this.lblDragQrToBin.AllowDrop = true;
            this.lblDragQrToBin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDragQrToBin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDragQrToBin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold);
            this.lblDragQrToBin.Image = ((System.Drawing.Image)(resources.GetObject("lblDragQrToBin.Image")));
            this.lblDragQrToBin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDragQrToBin.Location = new System.Drawing.Point(83, 9);
            this.lblDragQrToBin.Name = "lblDragQrToBin";
            this.lblDragQrToBin.Size = new System.Drawing.Size(130, 29);
            this.lblDragQrToBin.TabIndex = 2;
            this.lblDragQrToBin.Text = "Drag QR Here";
            this.lblDragQrToBin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDragQrToBin.DragDrop += new System.Windows.Forms.DragEventHandler(this.DragDrop);
            this.lblDragQrToBin.DragEnter += new System.Windows.Forms.DragEventHandler(this.DragEnter);
            // 
            // frmQRTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 126);
            this.Controls.Add(this.txtQRText);
            this.Controls.Add(this.ButtonFromText);
            this.Controls.Add(this.lblor1);
            this.Controls.Add(this.lblor0);
            this.Controls.Add(this.lblDragBinToQr);
            this.Controls.Add(this.lblDragQrToBin);
            this.Controls.Add(this.btnBinToQR);
            this.Controls.Add(this.btnQRToBin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmQRTool";
            this.Opacity = 0.95D;
            this.Text = "QR Tool";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnQRToBin;
        private System.Windows.Forms.Button btnBinToQR;
        private System.Windows.Forms.Label lblDragQrToBin;
        private System.Windows.Forms.Label lblDragBinToQr;
        private System.Windows.Forms.Label lblor0;
        private System.Windows.Forms.Label lblor1;
        private System.Windows.Forms.Button ButtonFromText;
        private System.Windows.Forms.TextBox txtQRText;
    }
}

