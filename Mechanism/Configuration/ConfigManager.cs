using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testdotnettwain.Mechanism
{

    public class ConfigManager
    {
        public const string TRUE = "1";
        public const string ObjectIdKey = "ObjectId";
        public const string ShowScannersKey = "ShowScanners";
        public const string UrlUploaderKey = "UrlUploader";
        public const string ShowPreviewKey = "ShowPreview";
        public const string TmpFolderKey = "tmpFolder";
        public const string CloseScannerAutoKey = "CloseScannerAuto";
        public const string DeleteFileAfterUploadingKey = "DeleteFileAfterUploading";
        public const string RestartWIAAutoKey = "RestartWIAAuto";
        static ConfigManager _configManager;
        public static ConfigManager GetSinglton()
        {
            if (_configManager == null)
            {
                _configManager = new ConfigManager();
            }
            return _configManager;
        }
        static string _showScanners;
        static string _showPreview;
        static string _tmpFolder;
        static string _urlUploader;
        static string _closeScannerAuto;
        static string _deleteFileAfterUploading;

        public string UrlUploader
        {
            get
            {
                if (String.IsNullOrEmpty(_urlUploader))
                {
                    _urlUploader = ConfigurationManager.AppSettings[UrlUploaderKey];
                } 
                return _urlUploader;
            }
            set
            {
                _urlUploader = value;
            }
        }

        public string ShowScanners
        {
            get
            {
                if (String.IsNullOrEmpty(_showScanners))
                {
                    _showScanners = ConfigurationManager.AppSettings[ShowScannersKey];
                }
                return _showScanners;
            }
            set
            {
                _showScanners = value;
            }
        }

        public string ShowPreview
        {
            get
            {
                if (String.IsNullOrEmpty(_showPreview))
                {
                    _showPreview = ConfigurationManager.AppSettings[ShowPreviewKey];
                }
                return _showPreview;
            }
            set
            {
                _showPreview = value;
            }

        }

        public string TmpFolder
        {
            get
            {
                if (String.IsNullOrEmpty(_tmpFolder))
                {
                    _tmpFolder = ConfigurationManager.AppSettings[TmpFolderKey];
                }
                return _tmpFolder;
            }
            set
            {
                _tmpFolder = value;
            }

        }

        public string CloseScannerAuto
        {
            get
            {
                if (String.IsNullOrEmpty(_tmpFolder))
                {
                    _closeScannerAuto = ConfigurationManager.AppSettings[CloseScannerAutoKey];
                }
                return _closeScannerAuto;
            }
            set
            {
                _closeScannerAuto = value;
            }


        }

        public string DeleteFileAfterUploading
        {
            get
            {
                if (String.IsNullOrEmpty(_deleteFileAfterUploading))
                {
                    _deleteFileAfterUploading = ConfigurationManager.AppSettings[DeleteFileAfterUploadingKey];
                }
                return _deleteFileAfterUploading;
            }
            set
            {
                _deleteFileAfterUploading = value;
            }
        }

        public string ErrorMessgageHeader
        {
            get
            {
                return ConfigurationManager.AppSettings["ErrorMessgageHeader"];
            }
        }

        public string RestartWIAAuto
        {
            get
            {
                return ConfigurationManager.AppSettings[RestartWIAAutoKey];
            }
        }

    }
}
