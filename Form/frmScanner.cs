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

        private Twain tw;
        Rectangle bmprect = new Rectangle(0, 0, 0, 0);
        
        IntPtr bmpptr;
        IntPtr pixptr;
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
            if (_configManager.ShowScanner == "1")
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

        private void TransferReady()
        {
            try
            {
                string tmpFolder = _configManager.TmpFolder;
                ArrayList pics = tw.TransferPictures();
                EndingScan();
                tw.CloseSrc();
                string strFileName;
                strFileName = "";
                TiffBitmapEncoder encoder = new TiffBitmapEncoder();
                BitmapFrame frame;
                if (!(pics != null && pics.Count != 0))
                {
                    MessageBox.Show(this, "No Has Any pages", "Guardian Information Systems");
                    return;
                }
                CreateDirectory(tmpFolder);

                for (int i = 0; i < pics.Count; i++)
                {
                    IntPtr img = (IntPtr)pics[i];
                    bmpptr = GdiWin32.GlobalLock(img);
                    pixptr = GdiWin32.GetPixelInfo(bmpptr);
                    Guid clsid;

                    GdiWin32.GetCodecClsid(strFileName, out clsid);
                    IntPtr img2 = IntPtr.Zero;

                    // GdiWin32.GdipCreateBitmapFromGdiDib(bmpptr, pixptr, ref img2);
                    // GdiWin32.GdipSaveImageToFile(img2, strFileName, ref clsid, IntPtr.Zero);
                    Bitmap bp = GdiWin32.BitmapFromDIB(bmpptr, pixptr);

                    GdiWin32.GdipDisposeImage(img2);

                    BitmapSource image = Imaging.CreateBitmapSourceFromBitmap(bp);
                    frame = BitmapFrame.Create(image);

                    if (frame != null)
                    {
                        encoder.Frames.Add(frame);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                }
               var dtString= DateTime.Now.ToString("yyyymmddhhMMss");

               strFileName = tmpFolder + "\\" + Environment.MachineName + dtString + DateTime.Now.Millisecond + "";
            
                strFileName += ".new.tiff";
                string strNewFileName = tmpFolder + "\\" + Environment.MachineName + DateTime.Now.Millisecond + ".tiff";
                _output = new FileStream(tmpFolder + "\\new.tif", FileMode.OpenOrCreate);
                encoder.Save(_output);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //doc = null;

                //if (ScanSource == "0")
                //    CrmUpdater(strFileName);

                //else
                //    ScanRegular(strFileName);

                //try
                //{


                //    string tmpfile = @System.Configuration.ConfigurationSettings.AppSettings["FileServer"];
                //    if (tmpfile != null && tmpfile != string.Empty)
                //    {
                //        File.Copy(strNewFileName, tmpfile + "\\" + ObjectInfo + ".tiff", true);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(this, "Error : \r\n\r\n" + ex.Message.ToString() + "\r\n\r\n" + ex.StackTrace, "Guardian information system");
                //}

                MessageBox.Show(this, "Done", "Guardian Information Systems");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error : \r\n\r\n" + ex.Message.ToString() + "\r\n\r\n" + ex.StackTrace, "Guardian information system");
            }
            finally
            {
                //Environment.Exit(0);
                //Application.Exit();
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
            if (disposing)
            {
               // for manage code
                _output.Dispose();
            }
            if (tw != null)
            {
                tw.Finish();
            }
            base.Dispose(disposing);
        }

        ~frmScanner()
        {
            Dispose(false);
        }

        static private void ShowException(string message)
        {
            MessageBox.Show(null, message, System.Configuration.ConfigurationSettings.AppSettings["ErrorMessgageHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CreateDirectory(string tmpFolder)
        {
            if (!Directory.Exists( tmpFolder))
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

    }
}
