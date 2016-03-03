using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using testdotnettwain.Mechanism;

namespace ScannerToCrm
{
    public partial class FrmConfigDetails : Form
    {
        //public FrmConfigDetails()
        //{
        //    InitializeComponent();
        //    _configManager = ConfigManager.GetSinglton();
        //    chkPrev.Checked = _configManager.ShowPreview == ConfigManager.TRUE ? true : false;
        //    lblTmpFolder.Text = _configManager.TmpFolder;
        //    lblUri.Text = _configManager.UrlUploader;
        //    chkWIA.Checked = _configManager.RestartWIAAuto == ConfigManager.TRUE ? true : false;
        //    chkShowScanners.Checked = _configManager.ShowScanners == ConfigManager.TRUE ? true : false;
        //}
        public FrmConfigDetails(ConfigManager configManager)
        {
            InitializeComponent();
            //_configManager = ConfigManager.GetSinglton();
            chkPrev.Checked = configManager.ShowPreview == ConfigManager.TRUE ? true : false;
            lblTmpFolder.Text = configManager.TmpFolder;
            lblUri.Text = configManager.UrlUploader;
            chkWIA.Checked = configManager.RestartWIAAuto == ConfigManager.TRUE ? true : false;
            chkShowScanners.Checked = configManager.ShowScanners == ConfigManager.TRUE ? true : false;
            lblPreferDriver.Text = configManager.PrefferDriver;

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
