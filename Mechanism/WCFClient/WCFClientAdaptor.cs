using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using testdotnettwain.UploadLargeImages;

namespace testdotnettwain.Mechanism.WCFClient
{
    public class WCFClientAdaptor
    {
        Action<string> LogText; Cursor _cursor; ProgressBar _progressBar;

        public WCFClientAdaptor(Action<string> log, Cursor cursor, ProgressBar progressBar)
        {
            LogText = log; _cursor = cursor; _progressBar = progressBar;
        }

        public void Upload( string textFile)
        {
            _cursor = Cursors.WaitCursor;
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
                _cursor = Cursors.Default;
            }
        }

        void uploadStreamWithProgress_ProgressChanged(object sender, StreamWithProgress.ProgressChangedEventArgs e)
        {
            if (_progressBar != null)
            {
                if (e.Length != 0)
                    _progressBar.Value = (int)(e.BytesRead * 100 / e.Length);
            }
        }
    }
}
