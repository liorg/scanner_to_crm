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
using ScannerToCrm;
using System.ServiceModel;
using System.Collections.Specialized;
using System.Net;
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
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripSeparator toolStripSeparator3;
       // Guid _objectId = Guid.Empty;
        string _field1;
        string _field2;
        string _field3;
        //for test
 
        public frmStartScan()
        {
            _configManager = ConfigManager.GetSinglton();
            _configManager.ShowPreview = "1";
            InitializeComponent();
            //init the backgroundWorker
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            //set this property for Progressing Monitor
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }
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

            LogText("version:" + _version);
            if (nameValue != null)
            {
 
                var field1 = nameValue.Get(ConfigManager.Field1Key);
                if (!String.IsNullOrEmpty(field1))
                {
                    _field1 = field1;
                  //  if (Guid.TryParse(objectid, out _objectId))
                 //      LogText("Get Doc  id" + _objectId.ToString());
                    LogText("field1" + _field1);
                }
                else
                    LogText("no Geting any Doc field1");

                var field2 = nameValue.Get(ConfigManager.Field2Key);
                if (!String.IsNullOrEmpty(field2))
                {
                    _field2 = field2;
                    LogText("field2" + _field2);
                }
                else
                    LogText("no Geting any Doc field2");

                var field3 = nameValue.Get(ConfigManager.Field3Key);
                if (!String.IsNullOrEmpty(field3))
                {
                    _field3 = field3;
                    LogText("field3" + _field3);
                }
                else
                    LogText("no Geting any Doc field3");

                var urlUploader = nameValue.Get(ConfigManager.UrlUploaderKey);
                if (!String.IsNullOrEmpty(urlUploader))
                {
                    _configManager.UrlUploader = urlUploader;
                    LogText("UrlUploader:" + _configManager.UrlUploader);
                  
                }
                var prefferDriver = nameValue.Get(ConfigManager.PrefferDriverKey);
                if (!String.IsNullOrEmpty(prefferDriver))
                {
                    _configManager.PrefferDriver = prefferDriver;
                    LogText("PrefferDriver:" + _configManager.PrefferDriver);
                   
                }
                var showScanners = nameValue.Get(ConfigManager.ShowScannersKey);
                if (!String.IsNullOrEmpty(showScanners))
                {
                    _configManager.ShowScanners = showScanners;
                    LogText("ShowScanners:" + _configManager.ShowScanners);
                }
                var showPreview = nameValue.Get(ConfigManager.ShowPreviewKey);
                if (!String.IsNullOrEmpty(showPreview))
                    _configManager.ShowPreview = showPreview;
 
                var closeScannerAuto = nameValue.Get(ConfigManager.CloseScannerAutoKey);
                if (!String.IsNullOrEmpty(closeScannerAuto))
                    _configManager.CloseScannerAuto = closeScannerAuto;
 
                var tmpFolder = nameValue.Get(ConfigManager.TmpFolderKey);
                if (!String.IsNullOrEmpty(tmpFolder))
                {
                    _configManager.TmpFolder = tmpFolder;
                    LogText("TmpFolder:" + _configManager.TmpFolder);
                }
 
                var deleteFileAfterUploading = nameValue.Get(ConfigManager.DeleteFileAfterUploadingKey);
                if (!String.IsNullOrEmpty(deleteFileAfterUploading))
                {
                    _configManager.DeleteFileAfterUploading = deleteFileAfterUploading;
                    LogText("DeleteFileAfterUploading:" + _configManager.DeleteFileAfterUploading);
                }
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
                var result = new FileInfoUpload { IsSuccess = true, Path = textFile, Desc = "" };

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
 
                        var res = client.UploadFile(_field1,_field2,_field3, fileInfo.Name, fileInfo.Length, uploadStreamWithProgress, out  errDesc);

                        if (res == true)
                        {
                            // has err
                            LogText(errDesc);
                            result.IsSuccess = false;
                            result.Desc = errDesc;
                        }
                        LogText("Done!");

                        // close service client
                        client.Close();
                        // Send Result to Complete Task event hanlder
                        //(backgroundWorker1_RunWorkerCompleted)
                        e.Result = result;
                    }
                }
            }
            catch (Exception ex)
            {
                LogText("Exception : " + ex.Message);
                if (ex.InnerException != null) LogText("Inner Exception : " + ex.InnerException.Message);
                // Send Result to Complete Task event hanlder
                //(backgroundWorker1_RunWorkerCompleted)
                e.Result = new FileInfoUpload { IsSuccess = false, Path = textFile, Desc = "Unhadle exception" };

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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.restartWIAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
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
            this.BtnScan.Text = "סרוק";
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
            this.LogListBox.Size = new System.Drawing.Size(399, 126);
            this.LogListBox.TabIndex = 12;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(420, 24);
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
            this.toolStripMenuItem3,
            this.toolStripSeparator3,
            this.configurationToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // restartWIAToolStripMenuItem
            // 
            this.restartWIAToolStripMenuItem.Name = "restartWIAToolStripMenuItem";
            this.restartWIAToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.restartWIAToolStripMenuItem.Text = "Restart WIA";
            this.restartWIAToolStripMenuItem.Click += new System.EventHandler(this.RestartWia_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.Configuration_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem3.Text = "Clean Log";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // frmStartScan
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(420, 233);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.BtnScan);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmStartScan";
            this.Text = "Guardian Scanner ver 3.0";
            this.Load += new System.EventHandler(this.frmStartScan_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
 
 
        private void BtnScan_Click(object sender, System.EventArgs e)
        {
            StartScan();
        }

        void StartScan(){
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
            _frmmain = new frmScanner(_configManager,LogText);
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
            using (var pages = new frmPages(path,_field1,_field2,_field3, _configManager, LogText))
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
            var ver = "1.0.0.0";
            MessageBox.Show("Version Publisher:" + _version + "  All right resrved Lior  LTD 2016" + "version deployment:" + ver);
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
                var result = new FileInfoUpload { IsSuccess = true, Path = textFile, Desc = "" };

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
                         client.Endpoint.Address = new EndpointAddress(_configManager.UrlUploader);
                      
 
                      client.ClientCredentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                      client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                      
                        if (client.Endpoint != null && client.Endpoint.Address != null && client.Endpoint.Address.Uri != null)
                            LogText("Upload To Server :" + client.Endpoint.Address.Uri.Host + ":" + client.Endpoint.Address.Uri.Port);
                        else
                            LogText("Upload To Server :UNKNWON");
 
                        var res = client.UploadFile(_field1,_field2,_field3,  fileInfo.Name, fileInfo.Length, uploadStreamWithProgress, out  errDesc);

                        if (res == true)
                        {
                            // has err
                            LogText(errDesc);
                            result.IsSuccess = false;
                            result.Desc = errDesc;
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
               
            }
        }
 
        private void frmStartScan_Load(object sender, EventArgs e)
        {
            //wj_class1 computer
            StartScan();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            LogListBox.Items.Clear();
        }
    }
 
}
