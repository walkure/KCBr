using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KCB;

namespace KCB2
{
    public partial class FormMasterData : Form
    {
        public FormMasterData(ImageList slotItemImageList)
        {
            InitializeComponent();
            lvShip.ListViewItemSorter = Comparer<MasterData.Ship.ShipMasterLVItem>.Default;
            lvSlotItem.ListViewItemSorter = Comparer<MasterData.Item.SlotItemMasterLVItem>.Default;
            lvSType.ListViewItemSorter = Comparer<MasterData.Ship.ShipTypeLVItem>.Default;
            lvSlotItem.SmallImageList = slotItemImageList;

            var sm = new SystemMenu(this);
            sm.InsertMenuItem(2, "ウィンドウ復帰", 6);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (SystemMenu.GetSysMenuId(m) == 2)
            {
                WindowState = FormWindowState.Normal;
                Bounds = new Rectangle(100, 100, 300, 300);
            }
        }

        private void FormMasterData_Load(object sender, EventArgs e)
        {
            MasterData.Ship.ShipMasterLVItem.InitializeColumn(lvShip);
            lvShip.LoadColumnWithOrder(Properties.Settings.Default.MasterShipColumnWidth);

            MasterData.Item.SlotItemMasterLVItem.InitializeColumn(lvSlotItem);
            lvSlotItem.LoadColumnWithOrder(Properties.Settings.Default.MasterSlotItemColumnWidth);

            if (!Properties.Settings.Default.MasterListBounds.IsEmpty)
                Bounds = Properties.Settings.Default.MasterListBounds;
        }

        private void FormMasterData_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Visible = false;
                e.Cancel = true;
            }
            Properties.Settings.Default.MasterShipColumnWidth = lvShip.SaveColumnWithOrder();
            Properties.Settings.Default.MasterSlotItemColumnWidth = lvSlotItem.SaveColumnWithOrder();
            Properties.Settings.Default.MasterShipTypeColumnWidth = lvSType.SaveColumnWithOrder();



            if (WindowState == FormWindowState.Normal)
                Properties.Settings.Default.MasterListBounds = Bounds;
            else
                Properties.Settings.Default.MasterListBounds = RestoreBounds;
        }

        /// <summary>
        /// 艦船マスターの更新
        /// </summary>
        /// <param name="shipMaster"></param>
        public void UpdateMaster(MasterData.Ship shipMaster, MasterData.Item itemMaster)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => _updateMasterData(shipMaster, itemMaster)));
            else
                _updateMasterData(shipMaster, itemMaster);
        }

        void _updateMasterData(MasterData.Ship shipMaster,MasterData.Item itemMaster)
        {
            lvShip.BeginUpdate();
            lvShip.Items.Clear();
            lvShip.Items.AddRange(shipMaster.GetMasterLVItemList());
            lvShip.EndUpdate();

            lvSlotItem.BeginUpdate();
            lvSlotItem.Items.Clear();
            lvSlotItem.Items.AddRange(itemMaster.GetLVList());
            lvSlotItem.EndUpdate();

            lvSType.BeginUpdate();
            lvSType.Items.Clear();

            MasterData.Ship.ShipTypeLVItem.InitializeColumn(itemMaster, lvSType);
            lvSType.LoadColumnWithOrder(Properties.Settings.Default.MasterShipTypeColumnWidth);

            lvSType.Items.AddRange(shipMaster.GetMasterSTypeLVItems());
            lvSType.EndUpdate();

        }


        private void lvShip_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            MasterData.Ship.ShipMasterLVItem.Column = e.Column;
            lvShip.Sort();
            lvShip.SetSortIcon(e.Column, MasterData.Ship.ShipMasterLVItem.Order);
        }

        private void lvSlotItem_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            MasterData.Item.SlotItemMasterLVItem.Column = e.Column;
            lvSlotItem.Sort();
            lvSlotItem.SetSortIcon(e.Column, MasterData.Item.SlotItemMasterLVItem.Order);
        }

        private void lvSType_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            MasterData.Ship.ShipTypeLVItem.Column = e.Column;
            lvSType.Sort();
            lvSType.SetSortIcon(e.Column, MasterData.Ship.ShipTypeLVItem.Order);
        }
    }
}
