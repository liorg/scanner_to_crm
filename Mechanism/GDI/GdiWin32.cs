using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace testdotnettwain
{
    public static class GdiWin32
    {
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal class BITMAPINFOHEADER
        {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public int biClrUsed;
            public int biClrImportant;
        }

        [DllImport("gdi32.dll", ExactSpelling = true)]
        internal static extern int SetDIBitsToDevice(IntPtr hdc, int xdst, int ydst,
            int width, int height, int xsrc, int ysrc, int start, int lines,
            IntPtr bitsptr, IntPtr bmiptr, int color);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern void OutputDebugString(string outstr);
        [DllImport("gdiplus.dll", ExactSpelling = true)]
        internal static extern int GdipCreateBitmapFromGdiDib(IntPtr bminfo, IntPtr pixdat, ref IntPtr image);

        [DllImport("gdiplus.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        internal static extern int GdipSaveImageToFile(IntPtr image, string filename, [In] ref Guid clsid, IntPtr encparams);

        [DllImport("gdiplus.dll", ExactSpelling = true)]
        internal static extern int GdipDisposeImage(IntPtr image);

        /// <summary>
        /// Get pixel info 
        /// </summary>
        public static IntPtr GetPixelInfo(IntPtr bmpptr)
        {
            Rectangle bmprect = new Rectangle(0, 0, 0, 0);
            var bmi = new BITMAPINFOHEADER();
            Marshal.PtrToStructure(bmpptr, bmi);

            bmprect.X = bmprect.Y = 0;
            bmprect.Width = bmi.biWidth;
            bmprect.Height = bmi.biHeight;

            if (bmi.biSizeImage == 0)
                bmi.biSizeImage = ((((bmi.biWidth * bmi.biBitCount) + 31) & ~31) >> 3) * bmi.biHeight;

            int p = bmi.biClrUsed;
            if ((p == 0) && (bmi.biBitCount <= 8))
                p = 1 << bmi.biBitCount;
            p = (p * 4) + bmi.biSize + (int)bmpptr;
            return (IntPtr)p;
        }
        // Convert a DIB* to a System.Drawing.Bitmap
        // This is useful for scanner APIs that provide a DIB*.
        // A dot.net app can more easily work with System.Drawing.Bitmap.
        // Example usage:
        //   IntPtr pDib = GetDibPtr(); // Theoretical function to get DIB.
        //   Debug.Assert( pDib != IntPtr.Zero );
        //   System.Drawing.Bitmap bmp = BitmapFromDIB( pDib );
        //   bmp.Save( "mybmp.bmp" );
        // http://snipplr.com/view/36593.44712/
        public static Bitmap BitmapFromDIB(IntPtr pDIB, IntPtr pPix)
        {
            //because it's static and not public we must to reflect him
            MethodInfo mi = typeof(Bitmap).GetMethod("FromGDIplus",
                            BindingFlags.Static | BindingFlags.NonPublic);

            if (mi == null)
                return null; // (permission problem) 

            IntPtr pBmp = IntPtr.Zero;
            int status = GdipCreateBitmapFromGdiDib(pDIB, pPix, ref pBmp);

            if ((status == 0) && (pBmp != IntPtr.Zero)) // success 
                return (Bitmap)mi.Invoke(null, new object[] { pBmp });

            else
                return null; // failure 
        }
        /// <summary>
        /// Get clsid by extension file
        /// </summary>
        /// <param name="filename">file name include extension</param>
        /// <param name="clsid"></param>
        /// <returns></returns>
        public static bool GetCodecClsid(string filename, out Guid clsid)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
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
        static Guid? tiffClassId = Guid.Empty;

        public static Guid? GetCodecClsidByTiff()
        {
            if (tiffClassId==Guid.Empty)
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
           //  tiffClassId=    codecs.Where(c => c.FilenameExtension == "").FirstOrDefault();
                tiffClassId = (from c in codecs where c.FormatDescription == "TIFF" select c.Clsid).FirstOrDefault();
            }
            return tiffClassId;
        }

    }
}
