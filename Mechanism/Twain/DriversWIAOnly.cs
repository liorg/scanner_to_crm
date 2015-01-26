using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScannerToCrm.Mechanism.Twain
{
    public class DriversWIAOnly
    {
        static List<string> DriversList;
        static DriversWIAOnly drivers = null;
        protected DriversWIAOnly()
        {
            DriversList = new List<string>();
            //DriversList.Add("AV186U", true);
            //DriversList.Add("TW-Brother MFC-8880DN 3.8", true);
            DriversList.Add("WIA-Brother MFC-8880DN");//, false);
        }

        public static DriversWIAOnly GetSinglton()
        {
            if (drivers == null)
                drivers = new DriversWIAOnly();
            return drivers;
        }
        //        }
        static DriversWIAOnly()
        {

        }
        public List<string> GetDrivers
        {
            get
            {
                return DriversList;
            }
        }

    }
}