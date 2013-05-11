using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using testdotnettwain.Mechanism;
using testdotnettwain.UploadLargeImages;
using System.IO;
using testdotnettwain.Mechanism.DataModel;

namespace testdotnettwain
{
    /// <summary>
    /// Summary description for Form2.
    /// </summary>
    public class frmStartScan : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button BtnScan;
        private System.Windows.Forms.TextBox txtHeader;
        private System.Windows.Forms.Label label1;
        private ConfigManager _configManager;
        private frmScanner _frmmain;
        private ProgressBar progressBar1;
        private ListBox LogListBox;
        //declare the backgroundWorker
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmStartScan()
        {
            InitializeComponent();
            //init the backgroundWorker
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            //set this property for Progressing Monitor 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
          
            _configManager = ConfigManager.GetSinglton();
        }
        /// <summary>
        /// When it's Complete
        /// </summary>
        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
             LogText("Finish!");
            Cursor = Cursors.Default;
            var result = e.Result as FileInfoUpload;
            if (result == null)
            {
                LogText("Error Occur!!!");
            }
            if (result.IsSuccess)
            {
                LogText("Finish Uploading " + result.Path);
                OnFileTempHandler(result.Path);
                OnCloseMissionHandler();
            }
            else
            {
                LogText("Error Occur!");
            }
        }
        /// <summary>
        /// Event Handler Streaming Progress
        /// </summary>
        void uploadStreamWithProgress_ProgressChanged(object sender, StreamWithProgress.ProgressChangedEventArgs e)
        {
            if (backgroundWorker1 != null)
            {
                backgroundWorker1.ReportProgress((int)(e.BytesRead * 100 / e.Length));
            }
        }
        /// <summary>
        /// Update UI Control ,our progressBar ,now it's safe to update progressBar1
        /// </summary>
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar1.Value = e.ProgressPercentage;
        }
        /// <summary>
        /// Do Work on Asynchronous mode 
        /// </summary>
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string textFile = e.Argument as string;
            try
            {
               
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
                        if (client.Endpoint != null && client.Endpoint.Address != null && client.Endpoint.Address.Uri != null)
                        {
                            LogText("Upload To Server :" + client.Endpoint.Address.Uri.Host + ":" + client.Endpoint.Address.Uri.Port);
                        }
                        else
                        {
                            LogText("Upload To Server :UNKNWON");
                        }
                        // upload file
                        client.UploadFile(fileInfo.Name, fileInfo.Length, uploadStreamWithProgress);

                        LogText("Done!");
                        
                        // close service client
                        client.Close();
                        // Send Result to Complete Task event hanlder
                        //(backgroundWorker1_RunWorkerCompleted)
                        e.Result = new FileInfoUpload { IsSuccess = true, Path = textFile, Desc = "" };
                    } 
                }
            }
            catch (Exception ex)
            {
                LogText("Exception : " + ex.Message);
                if (ex.InnerException != null) LogText("Inner Exception : " + ex.InnerException.Message);
                // Send Result to Complete Task event hanlder
                //(backgroundWorker1_RunWorkerCompleted)
                      
                e.Result = new FileInfoUpload { IsSuccess = false, Path = textFile, Desc = "Ok" };
             
            }
            finally
            {
            }

        }
       

        /// <summary>
        /// 
        /// </summary>
        private void LogText(string text)
        {

            if (LogListBox.InvokeRequired)
            {
                // Occur only when in async mode 
                LogListBox.Invoke(new Action<string>(LogText), text);
                return;
            }
            LogListBox.Items.Add(text);
            LogListBox.SelectedIndex = LogListBox.Items.Count - 1;
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_frmmain != null)
                {
                    _frmmain.Dispose();
                }
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
            this.BtnScan = new System.Windows.Forms.Button();
            this.txtHeader = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // BtnScan
            // 
            this.BtnScan.Location = new System.Drawing.Point(8, 64);
            this.BtnScan.Name = "BtnScan";
            this.BtnScan.Size = new System.Drawing.Size(64, 24);
            this.BtnScan.TabIndex = 9;
            this.BtnScan.Text = "סרוק";
            this.BtnScan.Click += new System.EventHandler(this.BtnScan_Click);
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(376, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "כותרת";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 35);
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
            this.LogListBox.Location = new System.Drawing.Point(8, 94);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(408, 36);
            this.LogListBox.TabIndex = 12;
            // 
            // frmStartScan
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(429, 142);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.BtnScan);
            this.Controls.Add(this.txtHeader);
            this.Controls.Add(this.label1);
            this.Name = "frmStartScan";
            this.Text = "Guardian Scanner ver 3.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion



        private void BtnScan_Click(object sender, System.EventArgs e)
        {
            LogText("Start Scanning...");
            progressBar1.Value = 0;
           // //excute the backgroundWorker  and putting argument 
           // backgroundWorker1.RunWorkerAsync(@"C:\gili\new.tiff");
           //// backgroundWorker1.RunWorkerAsync(@"C:\gili\LIORGLAP20134909100501252.new.tiff");
           // return;
            if (_frmmain != null)
                _frmmain.Dispose();
            _frmmain = new frmScanner();
            _frmmain.Finish += FrmmainFinish;
            _frmmain.Acquire();
        }

        private void FrmmainFinish(frmScanner frmmain, string info, bool isSuccess, int picsCount)
        {
           
            if (isSuccess)
            {
                Cursor = Cursors.WaitCursor;
                LogText(String.Format("Successfully end scanning {0}", picsCount));
                frmmain.CloseResources(true);
                if (_configManager.ShowPreview == ConfigManager.TRUE)
                {
                    using (var pages = new frmPages(info, LogText))
                    {
                        pages.ShowDialog();
                    }
                    OnFileTempHandler(info);
                    OnCloseMissionHandler();
                }
                else
                {
                    backgroundWorker1.RunWorkerAsync(info);
                }
               
            }
            else
            {
                LogText(String.Format("UnSuccessfully end scanning"));
                LogText("Error:" + info);
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

        void OnFileTempHandler(string path)
        {
            if (_configManager.DeleteFileAfterUploading == ConfigManager.TRUE)
            {
                File.Delete(path);
            }
        }

        
    }
}
