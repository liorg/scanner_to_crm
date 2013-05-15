using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using testdotnettwain.Mechanism;
using testdotnettwain.Mechanism.GDI;
using TwainLib;

namespace testdotnettwain
{
    public partial class frmScanner : Form, IMessageFilter
    {
        public event Action<frmScanner, string, bool, int> Finish;
        private Twain tw;
        Rectangle bmprect = new Rectangle(0, 0, 0, 0);

        
        static string ObjectId = string.Empty;
        static string ObjectType = string.Empty;
        static string ScanSource = string.Empty;
        static string ObjectInfo = string.Empty;
        private ConfigManager _configManager;
        private Stream _output;

        public frmScanner()
        {
            _configManager = ConfigManager.GetSinglton();
            InitializeComponent();
        }

        public void Acquire()
        {
            tw = new Twain();
            tw.Init(this.Handle);
            if (_configManager.ShowScanners == ConfigManager.TRUE)
            {
                tw.Select();
            }

            this.Enabled = false;

            Application.AddMessageFilter(this);

            tw.Acquire();
        }

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            TwainCommand cmd = tw.PassMessage(ref m);
            if (cmd == TwainCommand.Not)
                return false;
            switch (cmd)
            {
                case TwainCommand.CloseRequest:
                    {
                        EndingScan();
                        tw.CloseSrc();
                        break;
                    }
                case TwainCommand.CloseOk:
                    {
                        EndingScan();
                        tw.CloseSrc();
                        break;
                    }
                case TwainCommand.DeviceEvent:
                    {
                        break;
                    }

                case TwainCommand.TransferReady:
                    {
                        TransferReady();
                        break;
                    }
            }
            return true;
        }

       /// <summary>
        /// Transfer Ready To Scan0
       /// </summary>
        private void TransferReady()
        {
            try
            {
                var tmpFolder = _configManager.TmpFolder;
                //tranfer each image that's scann0ed and insert him to array,also dialogbox for a progress bar Indication  
                ArrayList pics = tw.TransferPictures();
                EndingScan();    tw.CloseSrc();
                string strFileName = "";
                // join all the images that's scanned  to one image tiff file
                var encoder = new TiffBitmapEncoder();
                BitmapFrame frame;
                IntPtr bmpptr;
                IntPtr pixptr;
                if (!(pics != null && pics.Count != 0))
                {
                    ShowException("No Has Any pages");
                    return;
                }
                // create temp folder 
                CreateDirectory(tmpFolder);
                int picsCount = pics.Count;
                for (int i = 0; i < pics.Count; i++)
                {
                    IntPtr img = (IntPtr)pics[i];
                    //Locks a global memory object and returns a pointer to the first byte of the object's memory block
                    bmpptr = GdiWin32.GlobalLock(img);
                    //Get Pixel Info by handle
                    pixptr = GdiWin32.GetPixelInfo(bmpptr);
                    Guid clsid;
                    // get clsId GUID by extension file (*.tiff)
                    GdiWin32.GetCodecClsid(strFileName, out clsid);
                    // create bitmap type
                    Bitmap bp = GdiWin32.BitmapFromDIB(bmpptr, pixptr);
                    // get bitmap frame for insert him TiffBitmapEncoder
                    frame = Imaging.GetBitmapFrame(bp);
                    if (frame != null)
                       encoder.Frames.Add(frame);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                // genrate file name to temp folder 
                strFileName = GenerateFileTemp(tmpFolder);
                string strNewFileName = strFileName;
                _output = new FileStream(strNewFileName, FileMode.OpenOrCreate);
                encoder.Save(_output);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Finish != null)
                {
                    // dispatch event to client
                    Finish(this, strNewFileName, true, picsCount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error : \r\n\r\n" + ex.Message.ToString() + "\r\n\r\n" + ex.StackTrace, "Guardian information system");
            }
        }

        private void EndingScan()
        {
            Application.RemoveMessageFilter(this);
            this.Enabled = true;
            this.Activate();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            CloseResources(disposing);
            base.Dispose(disposing);
        }

        ~frmScanner()
        {
            Dispose(false);
        }

        private void ShowException(string message)
        {
            MessageBox.Show(null, message, System.Configuration.ConfigurationSettings.AppSettings["ErrorMessgageHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (Finish != null)
            {
                Finish(this, message, false, 0);
            }
        }

        private void CreateDirectory(string tmpFolder)
        {
            if (!Directory.Exists(tmpFolder))
            {
                try
                {
                    Directory.CreateDirectory(tmpFolder);
                }
                catch (Exception ex)
                {
                    ShowException(ex.Message);
                }
            }
        }

        private string GenerateFileTemp(string tmpFolder)
        {
             var dtString= DateTime.Now.ToString("yyyyMMddhhmmss");
             return tmpFolder + "\\" + Environment.MachineName + dtString + DateTime.Now.Millisecond + ".new.tiff";
        }
    
        /// <summary>
        /// Close Resources output filestream and twain global variable
        /// </summary>
        /// <param name="disposing">true is manage code</param>
        public void CloseResources(bool disposing)
        {
            if (disposing)
            {
                if (_output != null)
                {
                    _output.Dispose();
                }
            }
            if (tw != null)
                tw.Finish();

        }
    }
}
