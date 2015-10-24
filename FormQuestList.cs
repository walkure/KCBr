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
    public partial class FormQuestList : Form
    {
        public IEnumerable<MemberData.Quest.Info> QuestList { get; set; }

        public FormQuestList()
        {
            InitializeComponent();
        }

        private void FormQuestList_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        private void lvQuestList_MouseDown(object sender, MouseEventArgs e)
        {
            //右クリックでサイズ可変化
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;

            Point curLoc = new Point(Location.X, Location.Y + Height);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Location = new Point(curLoc.X, curLoc.Y - Height);
        }

        private void FormQuestList_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.QuestListSize = lvQuestList.Size;
            Properties.Settings.Default.QuestListColumnWidth = lvQuestList.SaveColumnWithOrder();

        }

        private void FormQuestList_Load(object sender, EventArgs e)
        {
            lvQuestList.LoadColumnWithOrder(Properties.Settings.Default.QuestListColumnWidth);

            if (QuestList == null)
                return;

            foreach (var it in QuestList)
            {
                var item = new ListViewItem(new string[]{
                    it.Id.ToString(),
                    it.StateString,
                    it.Name,
                    it.Description,
                    it.ProgressMsg
                });
                item.UseItemStyleForSubItems = false;

                if (it.ProgressFlag == 1)
                    item.SubItems[4].BackColor = Color.LightGreen;
                else if (it.ProgressFlag == 2)
                    item.SubItems[4].BackColor = Color.LimeGreen;

                lvQuestList.Items.Add(item);
            }

        }

    }
}
