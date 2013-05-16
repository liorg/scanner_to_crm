using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testdotnettwain.Mechanism
{
   
    public class ConfigManager
    {
        public const string TRUE = "1";
        static ConfigManager _configManager;
        public static ConfigManager GetSinglton()
        {
            if (_configManager == null)
            {
                _configManager = new ConfigManager();
            }
            return _configManager;
        }
        public string ShowScanners
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["ShowScanners"];
            }
        }

        public string ShowPreview
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["ShowPreview"];
            }
        }
        

        public string TmpFolder
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["tmpFolder"];
            }
        }

        public string CloseScannerAuto
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["CloseScannerAuto"];
            }
        }

        public string DeleteFileAfterUploading
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["DeleteFileAfterUploading"];
            }
        }

        public string ErrorMessgageHeader
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["ErrorMessgageHeader"];
            }
        }
        public string RestartWIAAuto
        {
            get
            {
                return @System.Configuration.ConfigurationSettings.AppSettings["RestartWIAAuto"];
            }
        }
         
        
    }
}
