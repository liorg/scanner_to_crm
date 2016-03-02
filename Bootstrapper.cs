using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace testdotnettwain
{
    class Bootstrapper
    {
        static string _version = "";
        static private NameValueCollection GetQueryStringParameters()
        {
            NameValueCollection nameValueTable = new NameValueCollection();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
              //  MessageBox.Show(ApplicationDeployment.CurrentDeployment.ActivationUri.AbsolutePath);
                string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri != null ? ApplicationDeployment.CurrentDeployment.ActivationUri.Query : "";
                //MessageBox.Show(queryString);
                nameValueTable = HttpUtility.ParseQueryString(queryString);
            }
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment ad =
                System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                _version = ad.CurrentVersion.ToString();
            }
            return (nameValueTable);
        }

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                #region mocking
                //for test
                //Application.Run(new frmStartScan("111", null));
                #endregion

                #region prod
                String name = Process.GetCurrentProcess().ProcessName;
                Process[] localByName = Process.GetProcessesByName(name);
                NameValueCollection nameValue = null;
                if (localByName.Length > 1) Environment.Exit(0);
                nameValue = GetQueryStringParameters();

                Application.Run(new frmStartScan(_version, nameValue));
                #endregion
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
