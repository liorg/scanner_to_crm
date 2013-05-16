using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace testdotnettwain.Mechanism.Util
{
    public static class Consts
    {
        public const string RestartWIA="RestartWIA";
    }
    public static class Utils
    {
        

        public static void RestartWIA()
        {
            string serviceName="stisvc";
            ServiceController service = new ServiceController(serviceName);
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped);

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running);
           
        }

    }
}
