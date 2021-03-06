

using System;

using System.Collections;

using System.Runtime.InteropServices;

using System.Windows.Forms;

using System.Linq;

using System.Collections.Generic;
using testdotnettwain.Mechanism.Twain;

namespace TwainLib
{

    /*       *** thank's god ***         */

    /*

     *********************** current ver 1.0.0.2  ******************************

   */
    //ver 2.0.0.0
    // 1. perfer ds
    // ver 1.0.0.1 

    //1. add log (CTOR)

    //2. find wia contain in the word ProductName (Acquire)

    //3. change version twain to 2.3(TwainDefs.cs)



    /// <summary>

    /// 

    /// </summary>

    public enum TwainCommand
    {

        Not = -1,

        Null = 0,

        TransferReady = 1,

        CloseRequest = 2,

        CloseOk = 3,

        DeviceEvent = 4

    }

    public class Twain
    {
        private const short CountryUSA = 1;
        private const short LanguageUSA = 13;

        Action<string> _log;

        /// <param name="log">add log  ver 1.0.0.1 </param>

        public Twain(Action<string> log)
        {

            _log = log;

            appid = new TwIdentity();

            appid.Id = IntPtr.Zero;

            appid.Version.MajorNum = 1;

            appid.Version.MinorNum = 1;

            appid.Version.Language = LanguageUSA;

            appid.Version.Country = CountryUSA;

            appid.Version.Info = "Hack 1";

            appid.ProtocolMajor = TwProtocol.Major;

            appid.ProtocolMinor = TwProtocol.Minor;

            appid.SupportedGroups = (int)(TwDG.Image | TwDG.Control);

            appid.Manufacturer = "NETMaster";

            appid.ProductFamily = "Freeware";

            appid.ProductName = "Hack";



            srcds = new TwIdentity();

            srcds.Id = IntPtr.Zero;

            evtmsg.EventPtr = Marshal.AllocHGlobal(Marshal.SizeOf(winmsg));

            Log("twain version is Major= " + TwProtocol.Major.ToString() + ",Minor=" + TwProtocol.Minor);

        }

        void Log(string s)
        {

            if (_log != null)
            {

                _log(s);

            }

        }

        ~Twain()
        {

            Marshal.FreeHGlobal(evtmsg.EventPtr);

        }



        public void Init(IntPtr hwndp)
        {

            Finish();

            TwRC rc = DSMparent(appid, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.OpenDSM, ref hwndp);

            if (rc == TwRC.Success)
            {

                rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetDefault, srcds);

                if (rc == TwRC.Success)

                    hwnd = hwndp;

                else

                    rc = DSMparent(appid, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.CloseDSM, ref hwndp);

            }

        }

        bool allowSelectScanner = false;



        public void Select()
        {

            TwRC rc;

            CloseSrc();

            if (appid.Id == IntPtr.Zero)
            {

                Init(hwnd);

                if (appid.Id == IntPtr.Zero)

                    return;

            }

            allowSelectScanner = true;

            // ver 1.0.0.1 

            Log("Select your preffer driver... ");

            rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.UserSelect, srcds);

        }

        public void Acquire(string dsPreffer)
        {
            // start ver 2.0.0.0 
            Log("Begin Acquire...");
            if (!String.IsNullOrEmpty(dsPreffer))
                Log("Preffer Driver " + dsPreffer);
            // end ver 2.0.0.0 
            // var dsPreffer = "WIA-HP Scanjet G2410";// "HP Scanjet G2410 TWAIN";
            TwRC rc;

            CloseSrc();

            if (appid.Id == IntPtr.Zero)
            {
                Init(hwnd);
                if (appid.Id == IntPtr.Zero)
                    return;

            }
            //   rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.OpenDS, srcds);

            // ver 1.0.0.1 

            if (!allowSelectScanner)
            {  // start ver 2.0.0.0 
                if (!String.IsNullOrEmpty(dsPreffer))
                {
                    bool hasFound = SetDataSource(dsPreffer);
                    if (!hasFound)
                    {
                        Log("no Found please select  driver... ");
                        rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.UserSelect, srcds);

                    }

                }
                // end ver 2.0.0.0
                else
                {
                    //Find WIA Contain ver 1.0.0.1 
                    int driverScount;

                    Log("Allow Select Scanner is Disable ,try find WIA Scanner Driver...");

                    var isFoundWIA = SelectByWIAPrefer(out driverScount);

                    if (!isFoundWIA)
                    {
                        if (driverScount > 1)
                        {
                            Log("Found more then one driver" + driverScount.ToString() + " select  driver... ");
                            rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.UserSelect, srcds);
                        }
                        else
                            rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetDefault, srcds);
                    }
                    Log(" preffer driver WIA Found... ");
                }
            }

            rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.OpenDS, srcds);

            //LOG ver 1.0.0.1 
            Log("working on " + srcds.ProductName + "... ");
            Log("Manufacturer = " + srcds.Manufacturer + " ");
            Log("ProductFamily = " + srcds.ProductFamily + " ");
            Log("Protocol Version  = " + srcds.ProtocolMajor + "." + srcds.ProtocolMinor);
            Log("Version Info = " + srcds.Version.Info + " ");
            Log("Version   = " + srcds.Version.MajorNum + "." + srcds.Version.MinorNum);
            Log("SupportedGroups   = " + srcds.SupportedGroups);
            if (rc != TwRC.Success)
                return;

            TwCapability cap = new TwCapability(TwCap.XferCount, -1);
            rc = DScap(appid, srcds, TwDG.Control, TwDAT.Capability, TwMSG.Set, cap);
            if (rc != TwRC.Success)
            {
                CloseSrc();
                return;
            }
            var getdriversWIA = ScannerToCrm.Mechanism.Twain.DriversWIAOnly.GetSinglton().GetDrivers;
            IsWIAProtocol = (getdriversWIA.Where(c => string.Compare(srcds.ProductName, c, StringComparison.OrdinalIgnoreCase) == 0).Any());
           
            Log("Log IS WIA   = " + IsWIAProtocol);
            //(getdrivers.Contains(srcds.ProductName,) && getdrivers[srcds.ProductName] == false);

            TwUserInterface guif = new TwUserInterface();
            // show scanner view 
            guif.ShowUI = 1;
            guif.ModalUI = 0;

            guif.ParentHand = hwnd;
            rc = DSuserif(appid, srcds, TwDG.Control, TwDAT.UserInterface, TwMSG.EnableDS, guif);

            if (rc != TwRC.Success)
            { 
                CloseSrc();
                return;
            }
        }

        public bool IsWIAProtocol { get; set; }

        /// <summary>
        /// Find WIA Contain in the ProductName  ver 1.0.0.1 
        /// </summary>
        /// <param name="countDrivers">How many drivers has on itarator each driver</param>
        /// <returns>found driver contain in the ProductName with WIA =true, no found =false</returns>
        public bool SelectByWIAPrefer(out int countDrivers)
        {
            //var driverBeginWith = "WIA";
            var getdriversWIA = ScannerToCrm.Mechanism.Twain.DriversWIAOnly.GetSinglton().GetDrivers;

            var saveFirstName = "";
            countDrivers = 0;

            TwRC rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetFirst, srcds);

            Log("Get first driver " + srcds.ProductName);

            countDrivers++;

            // if (srcds.ProductName.ToLower().Contains(driverBeginWith))
            if (getdriversWIA.Where(c => string.Compare(srcds.ProductName, c, StringComparison.OrdinalIgnoreCase) == 0).Any()) // is  twain
            {
                Log("XFound wia" + srcds.ProductName);
                return true;
            }

            saveFirstName = srcds.ProductName;

            rc = TwRC.Success;

            while (rc == TwRC.Success)
            {

                rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetNext, srcds);
                Log("Get next driver " + srcds.ProductName);
                if (saveFirstName != srcds.ProductName)
                {
                    countDrivers++;
                }
                else
                {
                    Log("some drivers go  next driver " + srcds.ProductName);
                }

                //if (srcds.ProductName.Contains(driverBeginWith))

                if (getdriversWIA.Where(c => string.Compare(srcds.ProductName, c, StringComparison.OrdinalIgnoreCase) == 0).Any())
                {
                    Log("Found wia" + srcds.ProductName);
                    return true;
                }
            }
            return false;
        }

        // ver 2.0.0.0
        public bool SetDataSource(string ds)
        {

            //var driverBeginWith = "WIA";
            TwRC rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetFirst, srcds);

            Log("Get first driver " + srcds.ProductName);

            // if (srcds.ProductName.ToLower().Contains(driverBeginWith))

            if (string.Compare(srcds.ProductName, ds, StringComparison.OrdinalIgnoreCase) == 0)
            {

                Log("Found " + srcds.ProductName);
                return true;
            }
            rc = TwRC.Success;

            while (rc == TwRC.Success)
            {

                rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetNext, srcds);
                Log("Get next driver " + srcds.ProductName);
                if (string.Compare(srcds.ProductName, ds, StringComparison.OrdinalIgnoreCase) == 0)
                    return true;
                else
                    Log("some drivers go  next driver " + srcds.ProductName);

            }
            return false;
        }

        public ArrayList TransferPictures(out bool isUnHandleException)
        {
            isUnHandleException = false;
            ArrayList pics = new ArrayList();
            if (srcds.Id == IntPtr.Zero)
                return pics;

            TwRC rc;

            IntPtr hbitmap = IntPtr.Zero;

            TwPendingXfers pxfr = new TwPendingXfers();

            do
            {
                pxfr.Count = 0;
                hbitmap = IntPtr.Zero;
                TwImageInfo iinf = new TwImageInfo();
                rc = DSiinf(appid, srcds, TwDG.Image, TwDAT.ImageInfo, TwMSG.Get, iinf);
                if (rc != TwRC.Success)
                {
                    CloseSrc();
                    return pics;
                }
                rc = DSixfer(appid, srcds, TwDG.Image, TwDAT.ImageNativeXfer, TwMSG.Get, ref hbitmap);

                if (rc != TwRC.XferDone)
                {
                    TwStatus dsmstat = new TwStatus();
                    rc = DSstatus(appid, srcds, TwDG.Control, TwDAT.Status, TwMSG.Get, dsmstat);

                    if (rc == TwRC.Success && dsmstat.ConditionCode == (short)TwCC.Bummer)
                        isUnHandleException = true;

                    CloseSrc();
                    return pics;
                }

                rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.EndXfer, pxfr);
                if (rc != TwRC.Success)
                {
                    CloseSrc();
                    return pics;
                }
                pics.Add(hbitmap);
            }

            while (pxfr.Count != 0);
            rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.Reset, pxfr);
            return pics;
        }

        /// <summary>
        /// Pass Message Message Queuing of API WIN32
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public TwainCommand PassMessage(ref Message m)
        {
            if (srcds.Id == IntPtr.Zero)
                return TwainCommand.Not;

            int pos = GetMessagePos();

            winmsg.hwnd = m.HWnd;

            winmsg.message = m.Msg;

            winmsg.wParam = m.WParam;

            winmsg.lParam = m.LParam;

            winmsg.time = GetMessageTime();

            winmsg.x = (short)pos;

            winmsg.y = (short)(pos >> 16);

            Marshal.StructureToPtr(winmsg, evtmsg.EventPtr, false);

            evtmsg.Message = 0;

            TwRC rc = DSevent(appid, srcds, TwDG.Control, TwDAT.Event, TwMSG.ProcessEvent, ref evtmsg);

            if (rc == TwRC.NotDSEvent)
                return TwainCommand.Not;

            //_log("message="+evtmsg.Message.ToString());

            if (evtmsg.Message == (short)TwMSG.XFerReady)
                return TwainCommand.TransferReady;

            if (evtmsg.Message == (short)TwMSG.CloseDSReq)
                return TwainCommand.CloseRequest;

            if (evtmsg.Message == (short)TwMSG.CloseDSOK)
                return TwainCommand.CloseOk;

            if (evtmsg.Message == (short)TwMSG.DeviceEvent)
                return TwainCommand.DeviceEvent;

            return TwainCommand.Null;
        }

        public void CloseSrc()
        {
            TwRC rc;
            if (srcds.Id != IntPtr.Zero)
            {
                TwUserInterface guif = new TwUserInterface();
                TwStatus dsmstatus = new TwStatus();
                rc = DSuserif(appid, srcds, TwDG.Control, TwDAT.UserInterface, TwMSG.DisableDS, guif);

                if (rc != TwRC.Success)
                {
                    rc = DSstatus(appid, srcds, TwDG.Control, TwDAT.Status, TwMSG.Get, dsmstatus);
                }
                //http://www.twainforum.org/viewtopic.php?p=15089&sid=2b9cb062441dacd49de8a5afd8129078
                /*
                 
                 Found my problem. Was due to the way the source was being closed.
                I used CloseDSOK with Windows 8 and CloseDS with Windows 7.
                 */
                OS osver = Win32Api.GetOS();
                switch (osver)
                {
                    case OS._2000:
                    case OS.XP:
                    case OS.Server2003:
                    case OS.Vista:
                    case OS.Server2008:
                    case OS._7:
                        rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.CloseDS, srcds);
                        break;
                    default:
                        rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.CloseDSOK, srcds);
                        break;
                }
            }
        }

        /// <summary>
        /// close resource of twian object
        /// </summary>
        public void Finish()
        {
            TwRC rc;
            CloseSrc();
            if (appid.Id != IntPtr.Zero)
                rc = DSMparent(appid, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.CloseDSM, ref hwnd);

            appid.Id = IntPtr.Zero;
        }

        private IntPtr hwnd;

        private TwIdentity appid;

        private TwIdentity srcds;

        private TwEvent evtmsg;

        private WINMSG winmsg;

        // ------ DSM entry point DAT_ variants:
        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSMparent([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, ref IntPtr refptr);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSMident([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwIdentity idds);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSMstatus([In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwStatus dsmstat);

        // ------ DSM entry point DAT_ variants to DS:

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSuserif([In, Out] TwIdentity origin, [In, Out] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, TwUserInterface guif);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSevent([In, Out] TwIdentity origin, [In, Out] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, ref TwEvent evt);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSstatus([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwStatus dsmstat);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DScap([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwCapability capa);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSiinf([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwImageInfo imginf);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSixfer([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, ref IntPtr hbitmap);

        [DllImport("twain_32.dll", EntryPoint = "#1")]
        private static extern TwRC DSpxfer([In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwPendingXfers pxfr);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalAlloc(int flags, int size);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalLock(IntPtr handle);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern bool GlobalUnlock(IntPtr handle);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GlobalFree(IntPtr handle);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int GetMessagePos();

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int GetMessageTime();

        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern int GetDeviceCaps(IntPtr hDC, int nIndex);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CreateDC(string szdriver, string szdevice, string szoutput, IntPtr devmode);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern bool DeleteDC(IntPtr hdc);

        public static int ScreenBitDepth
        {
            get
            {
                IntPtr screenDC = CreateDC("DISPLAY", null, null, IntPtr.Zero);
                int bitDepth = GetDeviceCaps(screenDC, 12);
                bitDepth *= GetDeviceCaps(screenDC, 14);
                DeleteDC(screenDC);
                return bitDepth;
            }

        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        internal struct WINMSG
        {
            public IntPtr hwnd;
            public int message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int time;
            public int x;
            public int y;
        }
    } // class Twain

}



