using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testdotnettwain.Mechanism
{
    public class ConfigManager
    {
        static ConfigManager _configManager;
        public static ConfigManager GetSinglton()
        {
            if (_configManager == null)
            {
                _configManager = new ConfigManager();
            }
            return _configManager;
        }
        public string ShowScanner 
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["ShowScanner"];
            }
        }

        public string TmpFolder
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["tmpFolder"];
            }
        }

    }
}
