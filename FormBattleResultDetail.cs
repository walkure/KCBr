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
    public partial class FormBattleResultDetail : Form
    {
        public FormBattleResultDetail()
        {
            InitializeComponent();
        }

        public LogData.BattleResultInfo Info;

        private void FormBattleResultDetail_Load(object sender, EventArgs e)
        {
            if (Info == null)
                Close();

            lvDetail.Items[0].SubItems[1].Text = Info.Friend.DeckName;
            lvDetail.Items[0].SubItems[2].Text = Info.Foe.DeckName;

            for (int i = 0; i < 6; i++)
            {
                lvDetail.Items[i + 1].SubItems[1].Text = Info.Friend.ShipList[i];
                lvDetail.Items[i + 1].SubItems[2].Text = Info.Foe.ShipList[i];
            }

            if(!Properties.Settings.Default.BattleDetailBounds.IsEmpty)
                Bounds = Properties.Settings.Default.BattleDetailBounds;

            lvDetail.LoadColumnWithOrder(Properties.Settings.Default.BattleDetailColumnWidth);

        }

        private void FormBattleResultDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Properties.Settings.Default.BattleDetailBounds = Bounds;

            Properties.Settings.Default.BattleDetailColumnWidth 
                = lvDetail.SaveColumnWithOrder();
        }
    }
}
