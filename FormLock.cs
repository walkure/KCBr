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
    public partial class FormLock : Form
    {
        public string UnlockPassword { get; private set; }

        public FormLock()
        {
            InitializeComponent();
        }

        private void tbUnlockPassword_TextChanged(object sender, EventArgs e)
        {
            btOK.Enabled = tbUnlockPassword.TextLength > 0 ? true:false;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            UnlockPassword = tbUnlockPassword.Text;
        }


    }
}
