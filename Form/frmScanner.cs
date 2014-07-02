using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using testdotnettwain.Mechanism;
using testdotnettwain.Mechanism.GDI;
using testdotnettwain.Mechanism.Util;
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

        public frmScanner(ConfigManager configManager)
        {
            _configManager = configManager;// ConfigManager.GetSinglton();
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
        /// Transfer Ready To Scan
        /// </summary>
        private void TransferReady()
        {
            try
            {
                var tmpFolder = _configManager.TmpFolder;
                bool isUnHandleException;
                //tranfer each image that's scann0ed and insert him to array,also dialogbox for a progress bar Indication  
                ArrayList pics = tw.TransferPictures(out isUnHandleException);
                EndingScan(); tw.CloseSrc();
                if (isUnHandleException)   ShowException(Consts.RestartWIA);

                string strFileName = "";
                // join all the images that's scanned  to one image tiff file
                var encoder = new TiffBitmapEncoder();
                BitmapFrame frame=null;
                Bitmap bp = null;
                IntPtr bmpptr,pixptr;
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
                    bmpptr = Twain.GlobalLock(img);
                    //Get Pixel Info by handle
                    pixptr = GdiWin32.GetPixelInfo(bmpptr);
                    // create bitmap type
                    bp = GdiWin32.BitmapFromDIB(bmpptr, pixptr);

                    // get bitmap frame for insert him TiffBitmapEncoder
                    frame = Imaging.GetBitmapFrame(bp);
                    if (frame != null)
                        encoder.Frames.Add(frame);
                    //decease pointer reference so the gc will see there is no refernce to this obj and realse him from memory
                    Twain.GlobalUnlock(img);
                    // Get the last error and display it.
                    //int error = Marshal.GetLastWin32Error();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                // genrate file name to temp folder 
                strFileName = GenerateFileTemp(tmpFolder);
                string strNewFileName = strFileName;
                _output = new FileStream(strNewFileName, FileMode.OpenOrCreate);
                encoder.Save(_output);
                //dispose mamange component
                if (bp != null) bp.Dispose();
                //collect all!!!
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
                ShowException("Error : \r\n\r\n" + ex.Message.ToString() + "\r\n\r\n" + ex.StackTrace);
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
        //when we not call to dispose GC  goes her to the finilize,to be ensure that's we realse the tw which is unmanage code
        ~frmScanner()
        {
            Dispose(false);
        }

       
        /// <summary>
        /// Sharing function for Close Resources output filestream and twain global variable
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

        private void ShowException(string message)
        {
            if (message != Consts.RestartWIA)
            {
                MessageBox.Show(null, message, _configManager.ErrorMessgageHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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
            var dtString = DateTime.Now.ToString("yyyyMMddhhmmss");
            return tmpFolder + "\\" + Environment.MachineName + dtString + DateTime.Now.Millisecond + ".new.tiff";
        }
    
    }
}
