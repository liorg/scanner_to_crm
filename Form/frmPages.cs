﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using testdotnettwain.Mechanism;
using testdotnettwain.Mechanism.DataModel;
using testdotnettwain.UploadLargeImages;

/* Multipage tiff viewer
 * Created by Matjaž Grahek - 25.11.2008
 * 
 * Hi. This is my first article for CodeProject.
 * What you're seeing is an application for opening multipage tiffs and viewing the selected page.
 * 
 * This example is just a part of what might come out from one of you, but i think its good for a start.
 * 
 * Some comments are added next to the code. If you have some more questions, email me at matjaz.grahek@gmail.com
 * or post something on the article page.
 * 
 * PS: This code and the article was written by me and i dont know that much code. Intelisense helps a lot :)
 * I'm trying to say, that the code and application can be written MUCH better but its ment just as something to start from.
 */

namespace testdotnettwain
{
    public partial class frmPages : Form
    {
        Action<string> LogText;
        string _path;
        Guid _objectId;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private ConfigManager _configManager;
        public frmPages(string path,Guid objId, ConfigManager configManager, Action<string> log)
        {
            InitializeComponent();
            _objectId = objId;
            _configManager = configManager;
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;

            LogText = log;
            intCurrPage = 0; // reseting the counter
            lblFile.Text = path; // showing the file name in the lblFile
            _path = path;
            RefreshImage(); // refreshing and showing the new file
            opened = true; // the files was opened.


        }

        private int intCurrPage = 0; // defining the current page (its some sort of a counter)
        bool opened = false; // if an image was opened

        public void RefreshImage()
        {
            Image myImg; // setting the selected tiff
            Image myBmp; // a new occurance of Image for viewing

            myImg = System.Drawing.Image.FromFile(@lblFile.Text); // setting the image from a file

            int intPages = myImg.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page); // getting the number of pages of this tiff
            intPages--; // the first page is 0 so we must correct the number of pages to -1
            lblNumPages.Text = Convert.ToString(intPages); // showing the number of pages
            lblCurrPage.Text = Convert.ToString(intCurrPage); // showing the number of page on which we're on

            myImg.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, intCurrPage); // going to the selected page

            myBmp = new Bitmap(myImg, pictureBox1.Width, pictureBox1.Height); // setting the new page as an image
            // Description on Bitmap(SOURCE, X,Y)

            pictureBox1.Image = myBmp; // showing the page in the pictureBox1

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (opened) // the button works if the file is opened. you could go with button.enabled
            {
                if (intCurrPage == 0) // it stops here if you reached the bottom, the first page of the tiff
                { intCurrPage = 0; }
                else
                {
                    intCurrPage--; // if its not the first page, then go to the previous page
                    RefreshImage(); // refresh the image on the selected page
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (opened) // the button works if the file is opened. you could go with button.enabled
            {
                if (intCurrPage == Convert.ToInt32(lblNumPages.Text)) // if you have reached the last page it ends here
                // the "-1" should be there for normalizing the number of pages
                { intCurrPage = Convert.ToInt32(lblNumPages.Text); }
                else
                {
                    intCurrPage++;
                    RefreshImage();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            Upload(_path);

        }

        private void Upload(string textFile)
        {
            backgroundWorker1.RunWorkerAsync(textFile);

        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LogText("Worker Completed!");
            Cursor = Cursors.Default;
            var result = e.Result as FileInfoUpload;
            if (result == null)
            {
                LogText("Error Occur!!!");
            }
            if (result.IsSuccess)
            {
                LogText("Finish Uploading " + result.Path);

            }
            else
            {
                LogText("Error Occur!");
            }
            Close();
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar1.Value = e.ProgressPercentage;
        }

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
                        string errDesc;
                        // start service client
                        FileTransferServiceClient client = new FileTransferServiceClient();
                        client.Endpoint.Address = new EndpointAddress(_configManager.UrlUploader);

                        client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                       
                        client.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                      
                        
                        
                        
                        //client.ChannelFactory.Credentials.Windows.ClientCredential.UserName = "hdmalam";
                        //client.ChannelFactory.Credentials.Windows.ClientCredential.Password = "HDC@ll100";
                        //client.ChannelFactory.Credentials.Windows.ClientCredential.Domain = "mevaker";
                       // LogText("User :"+client.ChannelFactory.Credentials.Windows.ClientCredential.UserName);
                        if (client.Endpoint != null && client.Endpoint.Address != null && client.Endpoint.Address.Uri != null)
                            LogText("Upload To Server :" + client.Endpoint.Address.Uri.Host + ":" + client.Endpoint.Address.Uri.Port);

                        else
                            LogText("Upload To Server :UNKNWON");


                        // upload file
                        //client.UploadFile(fileInfo.Name, fileInfo.Length, uploadStreamWithProgress,Guid.NewGuid(),"12");

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
            //string textFile = e.Argument as string;
            //try
            //{
            //    // get some info about the input file
            //    System.IO.FileInfo fileInfo = new System.IO.FileInfo(textFile);

            //    // show start message
            //    LogText("Starting uploading " + fileInfo.Name);
            //    LogText("Size : " + fileInfo.Length);

            //    // open input stream
            //    using (System.IO.FileStream stream = new System.IO.FileStream(textFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            //    {
            //        string errDesc = "";
            //        using (StreamWithProgress uploadStreamWithProgress = new StreamWithProgress(stream))
            //        {
            //            uploadStreamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;

            //            // start service client
            //            FileTransferServiceClient client = new FileTransferServiceClient();
            //            if (client.Endpoint != null && client.Endpoint.Address != null && client.Endpoint.Address.Uri != null)
            //            {
            //                LogText("Upload To Server :" + client.Endpoint.Address.Uri.Host + ":" + client.Endpoint.Address.Uri.Port);
            //            }
            //            else
            //            {
            //                LogText("Upload To Server :UNKNWON");
            //            }
            //            // upload file
            //        //    client.UploadFile(fileInfo.Name, fileInfo.Length, uploadStreamWithProgress);
            //            //client.UploadFile(fileInfo.Name, fileInfo.Length, Guid.NewGuid(), "12", uploadStreamWithProgress);
            //            var res = client.UploadFile(fileInfo.Name, fileInfo.Length, Guid.NewGuid(), "12", uploadStreamWithProgress, out  errDesc);
            //            if (res == true)
            //            {
            //                LogText(errDesc);
            //            }
            //            LogText("Done!");

            //            // close service client
            //            client.Close();

            //            e.Result = new FileInfoUpload { IsSuccess = true, Path = textFile, Desc = "" };
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogText("Exception : " + ex.Message);
            //    if (ex.InnerException != null) LogText("Inner Exception : " + ex.InnerException.Message);
            //    e.Result = new FileInfoUpload { IsSuccess = false, Path = textFile, Desc = "Ok" };

            //}
            //finally
            //{
            //}

        }

        void uploadStreamWithProgress_ProgressChanged(object sender, StreamWithProgress.ProgressChangedEventArgs e)
        {
            if (backgroundWorker1 != null)
            {
                backgroundWorker1.ReportProgress((int)(e.BytesRead * 100 / e.Length));
            }
        }

        private void frmPages_Load(object sender, EventArgs e)
        {

        }
    }




}
