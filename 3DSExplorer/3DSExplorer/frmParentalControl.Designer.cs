namespace _3DSExplorer
{
    partial class frmParentalControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmParentalControl));
            this.comboAlgorithm = new System.Windows.Forms.ComboBox();
            this.labelAlgorithm = new System.Windows.Forms.Label();
            this.labelEnquiryNumber = new System.Windows.Forms.Label();
            this.labelDateOnDevice = new System.Windows.Forms.Label();
            this.dateTimeDevice = new System.Windows.Forms.DateTimePicker();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.textCode = new System.Windows.Forms.TextBox();
            this.numericEnquiryNumber = new System.Windows.Forms.NumericUpDown();
            this.labelCode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericEnquiryNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // comboAlgorithm
            // 
            this.comboAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAlgorithm.FormattingEnabled = true;
            this.comboAlgorithm.Items.AddRange(new object[] {
            "3DS / Wii U",
            "DSi / Wii"});
            this.comboAlgorithm.Location = new System.Drawing.Point(15, 33);
            this.comboAlgorithm.Name = "comboAlgorithm";
            this.comboAlgorithm.Size = new System.Drawing.Size(201, 21);
            this.comboAlgorithm.TabIndex = 0;
            // 
            // labelAlgorithm
            // 
            this.labelAlgorithm.AutoSize = true;
            this.labelAlgorithm.Location = new System.Drawing.Point(12, 13);
            this.labelAlgorithm.Name = "labelAlgorithm";
            this.labelAlgorithm.Size = new System.Drawing.Size(53, 13);
            this.labelAlgorithm.TabIndex = 1;
            this.labelAlgorithm.Text = "Algorithm:";
            // 
            // labelEnquiryNumber
            // 
            this.labelEnquiryNumber.AutoSize = true;
            this.labelEnquiryNumber.Location = new System.Drawing.Point(12, 66);
            this.labelEnquiryNumber.Name = "labelEnquiryNumber";
            this.labelEnquiryNumber.Size = new System.Drawing.Size(85, 13);
            this.labelEnquiryNumber.TabIndex = 2;
            this.labelEnquiryNumber.Text = "Enquiry Number:";
            // 
            // labelDateOnDevice
            // 
            this.labelDateOnDevice.AutoSize = true;
            this.labelDateOnDevice.Location = new System.Drawing.Point(12, 116);
            this.labelDateOnDevice.Name = "labelDateOnDevice";
            this.labelDateOnDevice.Size = new System.Drawing.Size(101, 13);
            this.labelDateOnDevice.TabIndex = 4;
            this.labelDateOnDevice.Text = "Date on the device:";
            // 
            // dateTimeDevice
            // 
            this.dateTimeDevice.CustomFormat = "";
            this.dateTimeDevice.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeDevice.Location = new System.Drawing.Point(15, 136);
            this.dateTimeDevice.Name = "dateTimeDevice";
            this.dateTimeDevice.Size = new System.Drawing.Size(201, 20);
            this.dateTimeDevice.TabIndex = 5;
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(77, 173);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(74, 23);
            this.buttonCalculate.TabIndex = 6;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // textCode
            // 
            this.textCode.Location = new System.Drawing.Point(15, 230);
            this.textCode.Name = "textCode";
            this.textCode.ReadOnly = true;
            this.textCode.Size = new System.Drawing.Size(201, 20);
            this.textCode.TabIndex = 7;
            // 
            // numericEnquiryNumber
            // 
            this.numericEnquiryNumber.Location = new System.Drawing.Point(15, 86);
            this.numericEnquiryNumber.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numericEnquiryNumber.Name = "numericEnquiryNumber";
            this.numericEnquiryNumber.Size = new System.Drawing.Size(201, 20);
            this.numericEnquiryNumber.TabIndex = 8;
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(12, 211);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(35, 13);
            this.labelCode.TabIndex = 9;
            this.labelCode.Text = "Code:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label1.Location = new System.Drawing.Point(131, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "neimod , marcan";
            // 
            // frmParentalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelCode);
            this.Controls.Add(this.numericEnquiryNumber);
            this.Controls.Add(this.textCode);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.dateTimeDevice);
            this.Controls.Add(this.labelDateOnDevice);
            this.Controls.Add(this.labelEnquiryNumber);
            this.Controls.Add(this.labelAlgorithm);
            this.Controls.Add(this.comboAlgorithm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmParentalControl";
            this.Text = "Parental Control Unlocker";
            ((System.ComponentModel.ISupportInitialize)(this.numericEnquiryNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboAlgorithm;
        private System.Windows.Forms.Label labelAlgorithm;
        private System.Windows.Forms.Label labelEnquiryNumber;
        private System.Windows.Forms.Label labelDateOnDevice;
        private System.Windows.Forms.DateTimePicker dateTimeDevice;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.TextBox textCode;
        private System.Windows.Forms.NumericUpDown numericEnquiryNumber;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label label1;
    }
}