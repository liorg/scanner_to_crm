using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testdotnettwain
{
    class Bootstrapper
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                //if (args.Length<3) 
                //{


                //    throw (new Exception(System.Configuration.ConfigurationSettings.AppSettings["loadingErrorMessgage"]));
                //}

                //if(args[0] != string.Empty)
                //{
                //    ScanSource=args[0];
                //    ObjectId = args[1];
                //    ObjectInfo=args[2];
                //    if (args.Length>3)
                //        ObjectType=args[3];
                //    if (ScanSource=="0" && ObjectType==String.Empty)
                //        throw (new Exception(System.Configuration.ConfigurationSettings.AppSettings["loadingErrorMessgage"]));
                //}

                String name = Process.GetCurrentProcess().ProcessName;
                Process[] localByName = Process.GetProcessesByName(name);
                if (localByName.Length > 1) Environment.Exit(0);
              //  Application.Run(new Form2());

                Application.Run(new frmStartScan());


            }
            catch (Exception ex)
            {
                ShowException(ex.Message);
            }
            finally
            {
                Environment.Exit(0);
                Application.Exit();
            }

        }

        static private void ShowException(string message)
        {
            MessageBox.Show(null, message, System.Configuration.ConfigurationSettings.AppSettings["ErrorMessgageHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
