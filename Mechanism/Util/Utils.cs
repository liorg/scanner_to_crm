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
            try
            {

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch
            {
                // ...
            }
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
               
                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }
    
        public static void RestartService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch
            {
                // ...
            }
        }

      
    }
}
