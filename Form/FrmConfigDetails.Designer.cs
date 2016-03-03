using testdotnettwain.Mechanism;

namespace ScannerToCrm
{
    partial class FrmConfigDetails
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblTmpFolder = new System.Windows.Forms.Label();
            this.chkPrev = new System.Windows.Forms.CheckBox();
            this.chkShowScanners = new System.Windows.Forms.CheckBox();
            this.chkCloseScannerAuto = new System.Windows.Forms.CheckBox();
            this.lblUri = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkWIA = new System.Windows.Forms.CheckBox();
            this.chkDeleteFileAfterUploading = new System.Windows.Forms.CheckBox();
            this.lblPreferDriver = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "temp folder:";
            // 
            // lblTmpFolder
            // 
            this.lblTmpFolder.AutoSize = true;
            this.lblTmpFolder.Location = new System.Drawing.Point(81, 18);
            this.lblTmpFolder.Name = "lblTmpFolder";
            this.lblTmpFolder.Size = new System.Drawing.Size(16, 13);
            this.lblTmpFolder.TabIndex = 1;
            this.lblTmpFolder.Text = "...";
            // 
            // chkPrev
            // 
            this.chkPrev.AutoSize = true;
            this.chkPrev.Enabled = false;
            this.chkPrev.Location = new System.Drawing.Point(16, 50);
            this.chkPrev.Name = "chkPrev";
            this.chkPrev.Size = new System.Drawing.Size(94, 17);
            this.chkPrev.TabIndex = 3;
            this.chkPrev.Text = "Show Preview";
            this.chkPrev.UseVisualStyleBackColor = true;
            // 
            // chkShowScanners
            // 
            this.chkShowScanners.AutoSize = true;
            this.chkShowScanners.Enabled = false;
            this.chkShowScanners.Location = new System.Drawing.Point(16, 84);
            this.chkShowScanners.Name = "chkShowScanners";
            this.chkShowScanners.Size = new System.Drawing.Size(116, 17);
            this.chkShowScanners.TabIndex = 4;
            this.chkShowScanners.Text = "Show Scanners list";
            this.chkShowScanners.UseVisualStyleBackColor = true;
            // 
            // chkCloseScannerAuto
            // 
            this.chkCloseScannerAuto.AutoSize = true;
            this.chkCloseScannerAuto.Enabled = false;
            this.chkCloseScannerAuto.Location = new System.Drawing.Point(16, 119);
            this.chkCloseScannerAuto.Name = "chkCloseScannerAuto";
            this.chkCloseScannerAuto.Size = new System.Drawing.Size(120, 17);
            this.chkCloseScannerAuto.TabIndex = 5;
            this.chkCloseScannerAuto.Text = "Close Scanner Auto";
            this.chkCloseScannerAuto.UseVisualStyleBackColor = true;
            // 
            // lblUri
            // 
            this.lblUri.AutoSize = true;
            this.lblUri.Location = new System.Drawing.Point(83, 155);
            this.lblUri.Name = "lblUri";
            this.lblUri.Size = new System.Drawing.Size(16, 13);
            this.lblUri.TabIndex = 7;
            this.lblUri.Text = "...";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Url Upload:";
            // 
            // chkWIA
            // 
            this.chkWIA.AutoSize = true;
            this.chkWIA.Enabled = false;
            this.chkWIA.Location = new System.Drawing.Point(16, 185);
            this.chkWIA.Name = "chkWIA";
            this.chkWIA.Size = new System.Drawing.Size(151, 17);
            this.chkWIA.TabIndex = 8;
            this.chkWIA.Text = "On Error Restart WIA Auto";
            this.chkWIA.UseVisualStyleBackColor = true;
            // 
            // chkDeleteFileAfterUploading
            // 
            this.chkDeleteFileAfterUploading.AutoSize = true;
            this.chkDeleteFileAfterUploading.Enabled = false;
            this.chkDeleteFileAfterUploading.Location = new System.Drawing.Point(16, 219);
            this.chkDeleteFileAfterUploading.Name = "chkDeleteFileAfterUploading";
            this.chkDeleteFileAfterUploading.Size = new System.Drawing.Size(152, 17);
            this.chkDeleteFileAfterUploading.TabIndex = 9;
            this.chkDeleteFileAfterUploading.Text = "Delete File After Uploading";
            this.chkDeleteFileAfterUploading.UseVisualStyleBackColor = true;
            // 
            // lblPreferDriver
            // 
            this.lblPreferDriver.AutoSize = true;
            this.lblPreferDriver.Location = new System.Drawing.Point(89, 254);
            this.lblPreferDriver.Name = "lblPreferDriver";
            this.lblPreferDriver.Size = new System.Drawing.Size(16, 13);
            this.lblPreferDriver.TabIndex = 11;
            this.lblPreferDriver.Text = "...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Driver Preffer:";
            // 
            // FrmConfigDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 295);
            this.Controls.Add(this.lblPreferDriver);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkDeleteFileAfterUploading);
            this.Controls.Add(this.chkWIA);
            this.Controls.Add(this.lblUri);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkCloseScannerAuto);
            this.Controls.Add(this.chkShowScanners);
            this.Controls.Add(this.chkPrev);
            this.Controls.Add(this.lblTmpFolder);
            this.Controls.Add(this.label1);
            this.Name = "FrmConfigDetails";
            this.Text = "Conifiguration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTmpFolder;
        private System.Windows.Forms.CheckBox chkPrev;
     
        private System.Windows.Forms.CheckBox chkShowScanners;
        private System.Windows.Forms.CheckBox chkCloseScannerAuto;
        private System.Windows.Forms.Label lblUri;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkWIA;
        private System.Windows.Forms.CheckBox chkDeleteFileAfterUploading;
        private System.Windows.Forms.Label lblPreferDriver;
        private System.Windows.Forms.Label label4;

        //public FrmConfigDetails()
        //{
        //    _configManager = ConfigManager.GetSinglton();
        //    chkPrev.Checked = _configManager.ShowPreview == ConfigManager.TRUE ? true : false;
        //    lblTmpFolder.Text = _configManager.TmpFolder;

        //}
    }
}