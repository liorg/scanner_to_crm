using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using testdotnettwain.Mechanism;
using testdotnettwain.UploadLargeImages;

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
        private Button btnUpload;
        private ProgressBar progressBar1;
        private ListBox LogListBox;
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
            this.btnUpload = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // BtnScan
            // 
            this.BtnScan.Location = new System.Drawing.Point(30, 169);
            this.BtnScan.Name = "BtnScan";
            this.BtnScan.Size = new System.Drawing.Size(64, 24);
            this.BtnScan.TabIndex = 9;
            this.BtnScan.Text = "סרוק";
            this.BtnScan.Click += new System.EventHandler(this.BtnScan_Click);
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(56, 34);
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
            this.label2.Location = new System.Drawing.Point(374, 37);
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
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(251, 171);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 10;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 128);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(408, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // LogListBox
            // 
            this.LogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogListBox.FormattingEnabled = true;
            this.LogListBox.IntegralHeight = false;
            this.LogListBox.Location = new System.Drawing.Point(8, 200);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(406, 66);
            this.LogListBox.TabIndex = 12;
            // 
            // frmStartScan
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(426, 275);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnUpload);
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

        void frmmain_Finish(frmScanner frmmain, string path, bool isSuccess, int picsCount)
        {

            if (isSuccess)
            {
                LogText(String.Format("Successfully end scanning {0}", picsCount));
                frmmain.CloseResources(true);
                OnCloseMissionHandler();
                if (_configManager.ShowScannedPages == ConfigManager.TRUE)
                {
                    using (var pages = new frmPages(path))
                    {
                        pages.ShowDialog();
                    }
                }
            }
            else{
                LogText(String.Format("UnSuccessfully end scanning!!!"));
                OnCloseMissionHandler();
            }

        }
        void OnCloseMissionHandler()
        {
            if (_configManager.CloseScannerAuto == ConfigManager.TRUE)
            {
                Environment.Exit(0);
                Application.Exit();
            }
        }
        private void frmStartScan_Load(object sender, EventArgs e)
        {

        }

        private void btnShowPagesScanned_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                string textFile = @"C:\gili\new.tiff";
                textFile = @"C:\gili\LIORGLAP20135707030543479.new.tiff";
               //  textFile = @"C:\gili\largedata.bmp";
                // get some info about the input file
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(textFile);

                // show start message
                LogText("Starting uploading " + fileInfo.Name);
                LogText("Size : " + fileInfo.Length);

                // open input stream
                using (System.IO.FileStream stream = new System.IO.FileStream(textFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (StreamWithProgress uploadStreamWithProgress = new StreamWithProgress(stream))
                    {
                        uploadStreamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;

                        // start service client
                       FileTransferServiceClient client = new FileTransferServiceClient();
                
                        // upload file
                        client.UploadFile(fileInfo.Name, fileInfo.Length, uploadStreamWithProgress);
                       
                        LogText("Done!");

                        // close service client
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogText("Exception : " + ex.Message);
                if (ex.InnerException != null) LogText("Inner Exception : " + ex.InnerException.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        void uploadStreamWithProgress_ProgressChanged(object sender, StreamWithProgress.ProgressChangedEventArgs e)
        {
            if (e.Length != 0)
                progressBar1.Value = (int)(e.BytesRead * 100 / e.Length);
        }

        private void LogText(string text)
        {
            LogListBox.Items.Add(text);
            LogListBox.SelectedIndex = LogListBox.Items.Count - 1;
        }
	}
}
