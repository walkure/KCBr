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
    public partial class FormBadShipStatus : Form
    {
        public List<ShipStatusManager.ShipStatus> ShipList
        {
            set
            {
                foreach (var it in value)
                {
                    lvShipStatus.Items.Add(it.GetLVItem());
                }

            }
        }
        public FormBadShipStatus()
        {
            InitializeComponent();
            IntPtr dummy = Handle;
            lblThreshold.Text = string.Format("HP参照閾値:{0}%",
                Properties.Settings.Default.HPThreshold);
        }

        private void FormBadShipStatus_Load(object sender, EventArgs e)
        {
            Activate();
        }
    }
}
