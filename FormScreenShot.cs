using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCB2
{
    public partial class FormScreenShot : Form
    {
        public enum ScreenShotSaveTarget
        {
            SaveAsFile,
            Clipboard,
        }

        public ScreenShotSaveTarget SaveTarget { get; private set; }
        public bool HideHeader { get; private set; }
        public bool ActivateSaveFile { get; set; }

        public FormScreenShot()
        {
            InitializeComponent();
        }

        private void btSaveFile_Click(object sender, EventArgs e)
        {
            SaveTarget = ScreenShotSaveTarget.SaveAsFile;
            DialogResult = DialogResult.OK;
            HideHeader = Properties.Settings.Default.HideHeader
                = cbHideHeader.Checked;
            Close();
        }

        private void btCopyToClipboard_Click(object sender, EventArgs e)
        {
            SaveTarget = ScreenShotSaveTarget.Clipboard;
            DialogResult = DialogResult.OK;
            HideHeader = Properties.Settings.Default.HideHeader 
                = cbHideHeader.Checked;
            Close();
        }

        private void FormScreenShot_Load(object sender, EventArgs e)
        {
            cbHideHeader.Checked = Properties.Settings.Default.HideHeader;
            btSaveFile.Enabled = ActivateSaveFile;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Properties.Settings.Default.HideHeader = cbHideHeader.Checked;
        }
    }
}
