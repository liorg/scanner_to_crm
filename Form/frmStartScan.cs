using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using testdotnettwain.Mechanism;

namespace testdotnettwain
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class frmStartScan : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button BtnScan;
		private System.Windows.Forms.TextBox txtNote;
		private System.Windows.Forms.TextBox txtHeader;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
        private ConfigManager _configManager;
        private frmScanner _frmmain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public frmStartScan()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            _configManager = ConfigManager.GetSinglton();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
                if (_frmmain != null)
                {
                    _frmmain.Dispose();
                }
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.BtnScan = new System.Windows.Forms.Button();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.txtHeader = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnScan
            // 
            this.BtnScan.Location = new System.Drawing.Point(56, 136);
            this.BtnScan.Name = "BtnScan";
            this.BtnScan.Size = new System.Drawing.Size(64, 24);
            this.BtnScan.TabIndex = 9;
            this.BtnScan.Text = "סרוק";
            this.BtnScan.Click += new System.EventHandler(this.BtnScan_Click);
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(56, 40);
            this.txtNote.MaxLength = 500;
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtNote.Size = new System.Drawing.Size(312, 88);
            this.txtNote.TabIndex = 8;
            // 
            // txtHeader
            // 
            this.txtHeader.Location = new System.Drawing.Point(56, 8);
            this.txtHeader.MaxLength = 50;
            this.txtHeader.Name = "txtHeader";
            this.txtHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtHeader.Size = new System.Drawing.Size(312, 20);
            this.txtHeader.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(376, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "מסמך";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(376, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "כותרת";
            // 
            // frmStartScan
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(440, 173);
            this.Controls.Add(this.BtnScan);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.txtHeader);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmStartScan";
            this.Text = "Guardian Scanner ver 3.0";
            this.Load += new System.EventHandler(this.frmStartScan_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		

		private void BtnScan_Click(object sender, System.EventArgs e)
		{
            if (_frmmain != null)
                _frmmain.Dispose();
            _frmmain = new frmScanner();
            _frmmain.Finish += frmmain_Finish;
            _frmmain.Acquire();
		}

        void frmmain_Finish(frmScanner frmmain, string path, bool isSuccess)
        {

            if (isSuccess)
            {
                frmmain.CloseResources(true);
                if (_configManager.CloseScannerAuto == ConfigManager.TRUE)
                {
                    Environment.Exit(0);
                    Application.Exit();
                }
                if (_configManager.ShowScannedPages == ConfigManager.TRUE)
                {
                    using (var pages = new frmPages(path))
                    {
                        pages.ShowDialog();
                    }
                }
                
            }

        }

        private void frmStartScan_Load(object sender, EventArgs e)
        {

        }

        private void btnShowPagesScanned_Click(object sender, EventArgs e)
        {

        }
	}
}
