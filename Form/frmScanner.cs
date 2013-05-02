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
using TwainLib;

namespace testdotnettwain
{
    public partial class frmScanner : Form, IMessageFilter
    {

        private Twain tw;
        Rectangle bmprect = new Rectangle(0, 0, 0, 0);
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
        IntPtr bmpptr;
        IntPtr pixptr;
        static string ObjectId = string.Empty;
        static string ObjectType = string.Empty;
        static string ScanSource = string.Empty;
        static string ObjectInfo = string.Empty;
        string txtHeader = string.Empty;
        string txtNote = string.Empty;


        public frmScanner(string txtHeader, string txtNote)
        {
             this.txtHeader = txtHeader;
             this.txtNote = txtNote;
            InitializeComponent();
        }

        public void Acquire()
        {
            tw = new Twain();
            tw.Init(this.Handle);
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
                        try
                        {
                            string TmpFolder = @System.Configuration.ConfigurationSettings.AppSettings["tmpFolder"];
                            ArrayList pics = tw.TransferPictures();
                            EndingScan();
                            tw.CloseSrc();
                            string strFileName;
                            strFileName = "";
                            //MODI.DocumentClass doc = new MODI.DocumentClass();
                            //MODI.DocumentClass page ;
                            //doc.Create(String.Empty);
                            for (int i = 0; i < pics.Count; i++)
                            {
                                IntPtr img = (IntPtr)pics[i];
                                bmpptr = GdiWin32.GlobalLock(img);
                                pixptr = GdiWin32.GetPixelInfo(bmpptr);
                                Guid clsid;

                                if (!Directory.Exists(TmpFolder))
                                {
                                    try
                                    {
                                        Directory.CreateDirectory(TmpFolder);
                                    }
                                    catch (Exception ex)
                                    {
                                        ShowException(ex.Message);
                                    }

                                }

                                strFileName = TmpFolder + "\\" + Environment.MachineName + i.ToString() + DateTime.Now.Millisecond + ".tiff";
                                GetCodecClsid(strFileName, out clsid);
                                IntPtr img2 = IntPtr.Zero;
                                GdiWin32.GdipCreateBitmapFromGdiDib(bmpptr, pixptr, ref img2);
                                GdiWin32.GdipSaveImageToFile(img2, strFileName, ref clsid, IntPtr.Zero);


                                Bitmap bp = GdiWin32.BitmapFromDIB(bmpptr, pixptr);


                                //// create a new control
                                //PictureBox pb = new PictureBox();

                                //// assign the image
                                //pb.Image = new Bitmap(bp);

                                //// stretch the image
                                //pb.SizeMode = PictureBoxSizeMode.StretchImage;

                                //// set the size of the picture box
                                //pb.Height = pb.Image.Height / 10;
                                //pb.Width = pb.Image.Width / 10;
                                //this.Controls.Add(pb);

                                GdiWin32.GdipDisposeImage(img2);
                                //page = new MODI.DocumentClass();
                                //page.Create(strFileName);
                                //doc.Images.Add(page.Images[0],null);

                                //page.Close(false);
                                //System.Runtime.InteropServices.Marshal.ReleaseComObject(page);

                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                GC.Collect();
                                GC.WaitForPendingFinalizers();
                                //page = null;
                                try
                                {

                                    //		File.Delete(strFileName);
                                }
                                catch { }
                            }
                            string OldFileName = strFileName;
                            strFileName += ".new.tiff";
                            string strNewFileName = TmpFolder + "\\" + Environment.MachineName + DateTime.Now.Millisecond + ".tiff";
                            //doc.SaveAs(strFileName,MODI.MiFILE_FORMAT.miFILE_FORMAT_TIFF,MODI.MiCOMP_LEVEL.miCOMP_LEVEL_LOW);
                            //doc.SaveAs(strNewFileName,MODI.MiFILE_FORMAT.miFILE_FORMAT_TIFF,MODI.MiCOMP_LEVEL.miCOMP_LEVEL_LOW);

                            //doc.Close(false);
                            //System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            //if (doc!=null)
                            //{
                            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                            //}

                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            //doc = null;

                            //if (ScanSource=="0")
                            //    CrmUpdater(strFileName);

                            //else
                            //    ScanRegular(strFileName);


                            //						fs.Close();
                            GC.WaitForPendingFinalizers();
                            try
                            {


                                string tmpfile = @System.Configuration.ConfigurationSettings.AppSettings["FileServer"];
                                if (tmpfile != null && tmpfile != string.Empty)
                                {
                                    File.Copy(strNewFileName, tmpfile + "\\" + ObjectInfo + ".tiff", true);
                                }



                                //File.Delete(strNewFileName);
                                //File.Delete(strFileName);
                                //File.Delete(OldFileName);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(this, "Error : \r\n\r\n" + ex.Message.ToString() + "\r\n\r\n" + ex.StackTrace, "Guardian information system");
                            }

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
                        break;
                    }
            }
            return true;
        }

        private void EndingScan()
        {
            Application.RemoveMessageFilter(this);
            this.Enabled = true;
            this.Activate();
        }

        private bool GetCodecClsid(string filename, out Guid clsid)
        {
            clsid = Guid.Empty;
            string ext = Path.GetExtension(filename);
            if (ext == null)
                return false;
            ext = "*" + ext.ToUpper();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FilenameExtension.IndexOf(ext) >= 0)
                {
                    clsid = codec.Clsid;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            //if (disposing)
            //{
            //    //if (components != null)
            //    //{
            //    //    components.Dispose();
            //    //    System.Runtime.InteropServices.Marshal.ReleaseComObject(components);

            //    //    GC.Collect();
            //    //    GC.WaitForPendingFinalizers();
            //    //}
            //}
            base.Dispose(disposing);
        }

        static private void ShowException(string message)
        {
            MessageBox.Show(null, message, System.Configuration.ConfigurationSettings.AppSettings["ErrorMessgageHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void frmScanner_Load(object sender, EventArgs e)
        {

        }
    }
}
