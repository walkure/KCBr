using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Collections.Specialized;
using System.Net;
using System.Diagnostics;
using KCB;

namespace KCB2
{
    public partial class FormLog : Form
    {
        LogManager.LogManager _logManager;
        public FormLog(LogManager.LogManager logManager)
        {
            InitializeComponent();
            //非表示でもウィンドウハンドルを生成させる。さもなくば、Invokeが失敗する
            IntPtr wndHandle = Handle;

            lvBattle.DoubleBuffer(true);
            lvItem.DoubleBuffer(true);
            lvMaterials.DoubleBuffer(true);
            lvMission.DoubleBuffer(true);
            lvShip.DoubleBuffer(true);

            chartMaterial.DataSource = logManager.InitMaterialDataTable();
            chartMaterial.DataBind();

            /* フォームは開かれていないのでLoadは呼ばれないが、
             * フォームが開かれる前にログ追加が行われるとデータの順番が腐る
             */

            if (!Properties.Settings.Default.LogFormBounds.IsEmpty)
                Bounds = Properties.Settings.Default.LogFormBounds;

            lvBattle.LoadColumnWithOrder(Properties.Settings.Default.LogBattleColumnWidth);
            lvItem.LoadColumnWithOrder(Properties.Settings.Default.LogItemColumnWidth);
            lvMaterials.LoadColumnWithOrder(Properties.Settings.Default.LogMaterialColumnWidth);
            lvMission.LoadColumnWithOrder(Properties.Settings.Default.LogMissionColumnWidth);
            lvShip.LoadColumnWithOrder(Properties.Settings.Default.LogShipColumnWidth);

            logManager.AttachLogControls(lvShip, lvItem, lvBattle, lvMission, lvMaterials,chartMaterial,this);
            _logManager = logManager;

            Text += string.Format("(Store:{0})", logManager.LogStore.StoreType);

            var sm = new SystemMenu(this);
            sm.InsertMenuItem(3, "ウィンドウ復帰", 6);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (SystemMenu.GetSysMenuId(m) == 3)
            {
                WindowState = FormWindowState.Normal;
                Bounds = new Rectangle(100, 100, 300, 300);
            }
        }

        private void FormLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Visible = false;
                e.Cancel = true;
            }

            if (WindowState == FormWindowState.Normal)
                Properties.Settings.Default.LogFormBounds = Bounds;
            else
                Properties.Settings.Default.LogFormBounds = RestoreBounds;

            Properties.Settings.Default.LogBattleColumnWidth = lvBattle.SaveColumnWithOrder();
            Properties.Settings.Default.LogItemColumnWidth = lvItem.SaveColumnWithOrder();
            Properties.Settings.Default.LogMaterialColumnWidth = lvMaterials.SaveColumnWithOrder();
            Properties.Settings.Default.LogMissionColumnWidth = lvMission.SaveColumnWithOrder();
            Properties.Settings.Default.LogShipColumnWidth = lvShip.SaveColumnWithOrder();

        }

        // 艦隊メンバを表示
        private void lvBattle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvBattle.SelectedIndices.Count == 0)
                return;

            LogData.BattleResultInfo item = _logManager.GetBattleResultItem(lvBattle.SelectedIndices[0]);

            using (FormBattleResultDetail dlg = new FormBattleResultDetail())
            {
                dlg.Info = item;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog();
            }
        }

        private void exportLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region 戦闘結果
            if (contextMenuStrip1.SourceControl.Contains(lvBattle))
            {
                using(var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSVファイル (*.csv)|*.csv|全ての形式(*.*)|";
                    sfd.RestoreDirectory = true;
                    sfd.Title = "戦闘結果をエクスポートします";
                    sfd.FileName = "BattleResult.csv";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var stream = sfd.OpenFile())
                        {
                            if (stream == null)
                                return;
                            using(var sw = new System.IO.StreamWriter(stream))
                                _logManager.ExportBattleLog(sw, LogManager.ExportLogDataType.CSV);
                        }
                    }
                }
                return;
            }
            #endregion

            #region 開発結果
            if (contextMenuStrip1.SourceControl.Contains(lvItem))
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSVファイル (*.csv)|*.csv|全ての形式(*.*)|";
                    sfd.RestoreDirectory = true;
                    sfd.Title = "装備開発結果をエクスポートします";
                    sfd.FileName = "CreateItems.csv";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var stream = sfd.OpenFile())
                        {
                            if (stream == null)
                                return;
                            using (var sw = new System.IO.StreamWriter(stream))
                                _logManager.ExportCreateItemLog(sw, LogManager.ExportLogDataType.CSV);
                        }
                    }
                }
                return;
            }
            #endregion

            #region 建造結果
            if (contextMenuStrip1.SourceControl.Contains(lvShip))
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSVファイル (*.csv)|*.csv|全ての形式(*.*)|";
                    sfd.RestoreDirectory = true;
                    sfd.Title = "艦船建造結果をエクスポートします";
                    sfd.FileName = "CreateShips.csv";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var stream = sfd.OpenFile())
                        {
                            if (stream == null)
                                return;
                            using (var sw = new System.IO.StreamWriter(stream))
                                _logManager.ExportCreateShipLog(sw, LogManager.ExportLogDataType.CSV);
                        }
                    }
                }
                return;
            }
            #endregion

            #region 遠征結果
            if (contextMenuStrip1.SourceControl.Contains(lvMission))
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSVファイル (*.csv)|*.csv|全ての形式(*.*)|";
                    sfd.RestoreDirectory = true;
                    sfd.Title = "遠征結果をエクスポートします";
                    sfd.FileName = "Missions.csv";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var stream = sfd.OpenFile())
                        {
                            if (stream == null)
                                return;
                            using (var sw = new System.IO.StreamWriter(stream))
                                _logManager.ExportMissionLog(sw, LogManager.ExportLogDataType.CSV);
                        }
                    }
                }
                return;
            }
            #endregion

            #region 資源情報
            if (contextMenuStrip1.SourceControl.Contains(lvMaterials))
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSVファイル (*.csv)|*.csv|全ての形式(*.*)|";
                    sfd.RestoreDirectory = true;
                    sfd.Title = "資源推移情報をエクスポートします";
                    sfd.FileName = "Materials.csv";
                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var stream = sfd.OpenFile())
                        {
                            if (stream == null)
                                return;
                            using (var sw = new System.IO.StreamWriter(stream))
                                _logManager.ExportMaterialsLog(sw, LogManager.ExportLogDataType.CSV);
                        }
                    }
                }
                return;
            }
            #endregion
        }
    }
}
