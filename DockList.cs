using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCB2
{
    public partial class DockList : UserControl
    {
        public DockList()
        {
            InitializeComponent();
            tableLayoutPanel1.DoubleBuffer(true);
        }

        public void SetOpenDockCount(int nDockCount)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => _setControlTextColor(nDockCount)));
            else
                _setControlTextColor(nDockCount);
        }

        private void _setControlTextColor(int nDockCount)
        {

            lblDock4.ForeColor = SystemColors.Control;
            lblTime4.ForeColor = SystemColors.Control;

            lblDock3.ForeColor = SystemColors.Control;
            lblTime3.ForeColor = SystemColors.Control;

            lblDock2.ForeColor = SystemColors.Control;
            lblTime2.ForeColor = SystemColors.Control;

            lblDock1.ForeColor = SystemColors.Control;
            lblTime1.ForeColor = SystemColors.Control;

            switch (nDockCount)
            {
                case 4:
                    lblDock4.ForeColor = SystemColors.ControlText;
                    lblTime4.ForeColor = SystemColors.ControlText;
                    goto case 3;
                case 3:
                    lblDock3.ForeColor = SystemColors.ControlText;
                    lblTime3.ForeColor = SystemColors.ControlText;
                    goto case 2;
                case 2:
                    lblDock2.ForeColor = SystemColors.ControlText;
                    lblTime2.ForeColor = SystemColors.ControlText;
                    goto case 1;
                case 1:
                    lblDock1.ForeColor = SystemColors.ControlText;
                    lblTime1.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        public class DockItem
        {
            public string Name { get; private set; }
            public DateTime Finish { get; private set; }
            public bool Vacant { get; private set; }
            public int Order { get; private set; }

            public DockItem(MemberData.Dock.DockInfo dat)
            {
                Name = dat.Name;
                Finish = dat.Finish;
                Vacant = dat.Vacant;
                Order = dat.Order;
            }

        }

        public void UpdateDockList(IEnumerable<MemberData.Dock.DockInfo> dockData)
        {
            List<DockItem> items = new List<DockItem>();
            lock (dockData)
            {
                foreach (var it in dockData)
                {
                    items.Add(new DockItem(it));
                }
            }

            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => updateDockList(items)));
            else
                updateDockList(items);

        }

        void updateDockList(IEnumerable<DockItem> items)
        {
            foreach (var it in items)
                updateDockListItem(it.Order, it);

        }

        void updateDockListItem(int slot,DockItem item)
        {
            CountdownLabel lblTime;
            Label lblDock;
            switch (slot)
            {
                case 1:
                    lblTime = lblTime1;
                    lblDock = lblDock1;
                    break;
                case 2:
                    lblTime = lblTime2;
                    lblDock = lblDock2;
                    break;
                case 3:
                    lblTime = lblTime3;
                    lblDock = lblDock3;
                    break;
                case 4:
                    lblTime = lblTime4;
                    lblDock = lblDock4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("unknown dock:"+slot.ToString());
            }

            if (item.Vacant)
            {
                lblDock.Text = string.Format("ドック{0}", slot);
                lblTime.Valid = false;
                toolTip1.SetToolTip(lblTime, null);
            }
            else
            {
                lblDock.Text = item.Name;
                lblTime.FinishTime = item.Finish;
                toolTip1.SetToolTip(lblTime, item.Finish.ToString());
                lblTime.Valid = true;
            }

        }

    }

    
}
  
