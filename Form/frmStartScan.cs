using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using testdotnettwain.Mechanism;

using System.IO;
using testdotnettwain.Mechanism.DataModel;
using testdotnettwain.Mechanism.Util;

using System.Security.Principal;

using System.ServiceModel;
using System.Collections.Specialized;
using System.Net;
using ScannerToCrm;
using testdotnettwain.UploadLargeImages;

namespace testdotnettwain
{
    /// <summary>
    /// Summary description for Form2.
    /// </summary>
    public class frmStartScan : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button BtnScan;
        private ConfigManager _configManager;
        private ListBox LogListBox;
        //declare the backgroundWorker
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private ProgressBar progressBar1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;

        private frmScanner _frmmain;
        private ToolStripMenuItem restartWIAToolStripMenuItem;
        private ToolStripMenuItem configurationToolStripMenuItem;
        string _version;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        FrmConfigDetails _fconfig;
        Guid _objectId = Guid.Empty;
        //for test


        public frmStartScan(string version, NameValueCollection nameValue)
        {
            _version = version;

            _configManager = ConfigManager.GetSinglton();

            InitializeComponent();
            //init the backgroundWorker
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            //set this property for Progressing Monitor 
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            if (nameValue != null)
            {

                var objectid = nameValue.Get(ConfigManager.ObjectIdKey);
                if (!String.IsNullOrEmpty(objectid))
                {
                    if (Guid.TryParse(objectid, out _objectId))
                        LogText("Get Doc crm id" + _objectId.ToString());
                }
                else
                    LogText("no Geting any Doc crm Id");

                var urlUploader = nameValue.Get(ConfigManager.UrlUploaderKey);
                if (!String.IsNullOrEmpty(urlUploader))
                {
                    // MessageBox.Show(urlUploader);
                    // MessageBox.Show(_configManager.UrlUploader);
                    _configManager.UrlUploader = urlUploader;
                    // MessageBox.Show(_configManager.UrlUploader);
                }

                var showScanners = nameValue.Get(ConfigManager.ShowScannersKey);
                if (!String.IsNullOrEmpty(showScanners))
                    _configManager.ShowScanners = showScanners;

                var showPreview = nameValue.Get(ConfigManager.ShowPreviewKey);
                if (!String.IsNullOrEmpty(showPreview))
                    _configManager.ShowPreview = showPreview;

                var closeScannerAuto = nameValue.Get(ConfigManager.CloseScannerAutoKey);
                if (!String.IsNullOrEmpty(closeScannerAuto))
                    _configManager.CloseScannerAuto = closeScannerAuto;

                var tmpFolder = nameValue.Get(ConfigManager.TmpFolderKey);
                if (!String.IsNullOrEmpty(tmpFolder))
                    _configManager.TmpFolder = tmpFolder;

                var deleteFileAfterUploading = nameValue.Get(ConfigManager.DeleteFileAfterUploadingKey);
                if (!String.IsNullOrEmpty(deleteFileAfterUploading))
                    _configManager.DeleteFileAfterUploading = deleteFileAfterUploading;
            }
            _fconfig = new FrmConfigDetails(_configManager);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            //Console.Beep();
            //Console.Beep();

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
                using (var stream = new System.IO.FileStream(textFile, FileMode.Open, FileAccess.Read))
                {
                    using (StreamWithProgress uploadStreamWithProgress = new StreamWithProgress(stream))
                    {
                        uploadStreamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;
                        string errDesc;
                        // start service client
                        FileTransferServiceClient client = new FileTransferServiceClient();
                        // client.ChannelFactory.Credentials.Windows.AllowNtlm = false;
                        client.Endpoint.Address = new EndpointAddress(_configManager.UrlUploader);
                        client.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                        client.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;

                        if (client.Endpoint != null && client.Endpoint.Address != null && client.Endpoint.Address.Uri != null)
                            LogText("Upload To Server :" + client.Endpoint.Address.Uri.Host + ":" + client.Endpoint.Address.Uri.Port);
                        else
                            LogText("Upload To Server :UNKNWON");

                        var res = client.UploadFile(fileInfo.Name, fileInfo.Length, _objectId, "12", uploadStreamWithProgress, out  errDesc);

                        if (res == true)
                        {
                            LogText(errDesc);
                        }
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


        }

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


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnScan = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.restartWIAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnScan
            // 
            this.BtnScan.BackColor = System.Drawing.Color.Bisque;
            this.BtnScan.Location = new System.Drawing.Point(8, 66);
            this.BtnScan.Name = "BtnScan";
            this.BtnScan.Size = new System.Drawing.Size(165, 31);
            this.BtnScan.TabIndex = 9;
            this.BtnScan.Text = "����";
            this.BtnScan.UseVisualStyleBackColor = false;
            this.BtnScan.Click += new System.EventHandler(this.BtnScan_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 37);
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
            this.LogListBox.Location = new System.Drawing.Point(8, 103);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(408, 60);
            this.LogListBox.TabIndex = 12;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(429, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.restartWIAToolStripMenuItem,
            this.toolStripSeparator2,
            this.configurationToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "About";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.About_Click);
            // 
            // restartWIAToolStripMenuItem
            // 
            this.restartWIAToolStripMenuItem.Name = "restartWIAToolStripMenuItem";
            this.restartWIAToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.restartWIAToolStripMenuItem.Text = "Restart WIA";
            this.restartWIAToolStripMenuItem.Click += new System.EventHandler(this.RestartWia_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.Configuration_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // frmStartScan
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(429, 167);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.BtnScan);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmStartScan";
            this.Text = "Guardian Scanner ver 3.0";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private void BtnScan_Click(object sender, System.EventArgs e)
        {
            LogText("Start Scanning...");

            progressBar1.Value = 0;
            //excute the backgroundWorker  and putting argument 

            #region mocking
            //var scanfile = _configManager.TmpFolder + "\\1.tiff";
            //MessageBox.Show("<<mocking scan file>>" + scanfile);
            //  backgroundWorker1.RunWorkerAsync(scanfile);
            //Sync(scanfile);
           // return;

            #endregion

            if (_frmmain != null)
                _frmmain.Dispose();
            _frmmain = new frmScanner(_configManager);
            _frmmain.Finish += FrmmainFinish;
            _frmmain.Acquire();
        }

        private void FrmmainFinish(frmScanner frmmain, string info, bool isSuccess, int picsCount)
        {
            if (frmmain != null)
                frmmain.CloseResources(true);

            if (isSuccess)
            {
                Cursor = Cursors.WaitCursor;
                LogText(String.Format("Successfully end scanning {0}", picsCount));
                if (_configManager.ShowPreview == ConfigManager.TRUE)
                {
                    ShowPrev(info);
                    OnFileTempHandler(info);
                    OnCloseMissionHandler();
                }
                else
                    backgroundWorker1.RunWorkerAsync(info);
            }
            else
            {

                LogText(String.Format("UnSuccessfully end scanning"));
                if (info == Consts.RestartWIA)
                {
                    LogText("the is a error must restart Windows Image Acquisition (WIA) ,and please scan Again");
                    if (_configManager.RestartWIAAuto == ConfigManager.TRUE)
                    {
                        LogText("Restart Windows Image Acquisition (WIA) ...");
                        Utils.RestartWIA();
                        LogText("Done Windows Image Acquisition (WIA)");

                    }
                }
                else
                    LogText("Error:" + info);
                OnCloseMissionHandler();
            }

        }
        void ShowPrev(string path)
        {
            using (var pages = new frmPages(path,_objectId, _configManager, LogText))
            {
                pages.ShowDialog();
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

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version Publisher:" + _version + "  All right resrved Guardian system LTD 2014");
        }

        private void RestartWia_Click(object sender, EventArgs e)
        {
            LogText("Restart Windows Image Acquisition (WIA) ...");
            Utils.RestartWIA();
            LogText("Done Windows Image Acquisition (WIA)");
        }

        private void Configuration_Click(object sender, EventArgs e)
        {
            _fconfig.ShowDialog();
        }


        void Sync(string textFile)
        {
            // string textFile = e.Argument as string;
            try
            {
                // get some info about the input file
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(textFile);

                // show start message
                LogText("Starting uploading " + fileInfo.Name);
                LogText("Size : " + fileInfo.Length);

                // open input stream
                using (var stream = new System.IO.FileStream(textFile, FileMode.Open, FileAccess.Read))
                {
                    using (StreamWithProgress uploadStreamWithProgress = new StreamWithProgress(stream))
                    {
                        uploadStreamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;
                        string errDesc;
                        // start service client
                        FileTransferServiceClient client = new FileTransferServiceClient();
                        // client.ChannelFactory.Credentials.Windows.AllowNtlm = false;
                        client.Endpoint.Address = new EndpointAddress(_configManager.UrlUploader);
                        //client.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Delegation;
                        //client.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;


                      client.ClientCredentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                        //client.ClientCredentials.Windows.ClientCredential=new NetworkCredential("mevaker\\hdmalam", "HDC@ll100");
                      //client.ClientCredentials.Windows.AllowNtlm = false;

                        //client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
                      client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                       
                        
                        
                        // client.ClientCredentials.Windows.AllowNtlm = true;
                        if (client.Endpoint != null && client.Endpoint.Address != null && client.Endpoint.Address.Uri != null)
                            LogText("Upload To Server :" + client.Endpoint.Address.Uri.Host + ":" + client.Endpoint.Address.Uri.Port);
                        else
                            LogText("Upload To Server :UNKNWON");

                        var res = client.UploadFile(fileInfo.Name, fileInfo.Length, _objectId, "12", uploadStreamWithProgress, out  errDesc);

                        if (res == true)
                        {
                            LogText(errDesc);
                        }
                        LogText("Done!");

                        // close service client
                        client.Close();
                        // Send Result to Complete Task event hanlder
                        //(backgroundWorker1_RunWorkerCompleted)

                    }
                }
            }
            catch (Exception ex)
            {
                LogText("Exception : " + ex.Message);
                if (ex.InnerException != null) LogText("Inner Exception : " + ex.InnerException.Message);
                // Send Result to Complete Task event hanlder
                //(backgroundWorker1_RunWorkerCompleted)


            }
        }
    }

}