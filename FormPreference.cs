using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace KCB2
{
    public partial class FormPreference : Form
    {
        public Version BrowserVersion;
        public TimerRPCManager TimerRPC;
        public GSpread.SpreadSheetWrapper SpreadWraper;

        const string totalShipListItem = "火力,対空,装甲,雷装,回避,索敵,対潜,HP,燃料,弾薬,射程,速力,運,装備";

        public FormPreference()
        {
            InitializeComponent();
        }

        private void FormPreference_Load(object sender, EventArgs e)
        {
            numProxyPort.Value = Properties.Settings.Default.ProxyPort;
            numHPThreshold.Value = Properties.Settings.Default.HPThreshold;
            cbDetailStatus.Checked = Properties.Settings.Default.VerboseStatus;

            cbLogStoreType.SelectedIndex = (int)Properties.Settings.Default.LogStoreType;
            if (SpreadWraper.Refresh())
            {
                tbGoogleAuthorizeURL.Enabled = false;
                tbGoogleAuthorizeToken.Enabled = false;
                btGoogleAuthorize.Text = "再認証";
                tbGoogleAuthorizeURL.Text = "認証済み";
            }
            else
            {
                tbGoogleAuthorizeURL.Text = SpreadWraper.AuthorizationURL;
            }

            tbLogDir.Text = Properties.Settings.Default.LogStoreDir;

            lblIEVer.Text = "Internet Explorer " + BrowserVersion.ToString();

            //GPUレンダリングはIE9以降で有効
            cbGPURendering.Enabled = BrowserVersion.Major >= 9 ? true : false;

            Configuration config = ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.PerUserRoamingAndLocal);

            tbConfPath.Text = config.FilePath;
            cbGPURendering.Checked = BrowserSetting.EnableGPURendering;

            #region ブラウザバージョン情報設定
            switch (BrowserSetting.EmurateBrowser)
            {
                default:
                case BrowserSetting.BrowserVer.IE7:
                    cbBrowserVersion.SelectedIndex = 0;
                    break;

                case BrowserSetting.BrowserVer.IE8:
                    cbBrowserVersion.SelectedIndex = 1;
                    break;
                case BrowserSetting.BrowserVer.IE8_FORCE:
                    cbBrowserVersion.SelectedIndex = 2;
                    break;

                case BrowserSetting.BrowserVer.IE9:
                    cbBrowserVersion.SelectedIndex = 3;
                    break;
                case BrowserSetting.BrowserVer.IE9_FORCE:
                    cbBrowserVersion.SelectedIndex = 4;
                    break;

                case BrowserSetting.BrowserVer.IE10:
                    cbBrowserVersion.SelectedIndex = 5;
                    break;
                case BrowserSetting.BrowserVer.IE10_FORCE:
                    cbBrowserVersion.SelectedIndex = 6;
                    break;

                case BrowserSetting.BrowserVer.IE11:
                    cbBrowserVersion.SelectedIndex = 7;
                    break;
                case BrowserSetting.BrowserVer.IE11_FORCE:
                    cbBrowserVersion.SelectedIndex = 8;
                    break;

            }
            #endregion ブラウザバージョン情報設定


            tbImageStore.Text = Properties.Settings.Default.ImageStoreDir;

            cbSyncronizeTimerProcess.Checked = Properties.Settings.Default.SyncronizeTimerProcess;

            btTimerConf.Enabled = TimerRPCManager.ExistWCFServer;

            numCondCeil.Value = Properties.Settings.Default.CondCeil;

            var totalItem = totalShipListItem.Split(',');
            var usedItem = Properties.Settings.Default.ShipDetailItem.Split(',');

            foreach (var item in usedItem)
            {
                foreach (var tItem in totalItem)
                {
                    if (item == tItem)
                    {
                        clbShipDetail.Items.Add(item, true);
                        break;
                    }
                }
            }

            foreach(var item in totalItem)
            {
                bool bUsed = false;
                foreach(var uItem in usedItem)
                {
                    if(uItem == item)
                    {
                        bUsed = true;
                        break;
                    }
                }

                if(bUsed)
                    continue;

                clbShipDetail.Items.Add(item, false);
            }

            cbSupressSuceessWriteLog.Checked = Properties.Settings.Default.SuppressSuceedWriteLog;

            cbUseMasterDataView.Checked = Properties.Settings.Default.UseMasterDataView;
            cbActivateDevMenu.Checked = Properties.Settings.Default.UseDevMenu;

            cbMuteOnMinimize.Checked = Properties.Settings.Default.MuteOnMinimize;
            cbHideDrop.Checked = Properties.Settings.Default.HideDroppedShip;
            cbShowEnemyStatus.Checked = Properties.Settings.Default.ShowEnemyBattleStatus;
            cbSuppressBrowserDialogs.Checked = Properties.Settings.Default.SuppressBrowserDialog;

            groupBox10.Enabled = cbUseUpstreamProxy.Checked = Properties.Settings.Default.UseUpstreamProxy;
            tbUpstreamProxyHost.Text = Properties.Settings.Default.UpstreamProxyHost;
            numUpstreamProxyPort.Value = Properties.Settings.Default.UpstreamProxyPort;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ProxyPort = (int)numProxyPort.Value;
            Properties.Settings.Default.HPThreshold = (int)numHPThreshold.Value;
            Properties.Settings.Default.VerboseStatus = cbDetailStatus.Checked;

            BrowserSetting.EnableGPURendering = cbGPURendering.Checked;

            switch (cbBrowserVersion.SelectedIndex)
            {
                default:
                case 0:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE7;
                    break;

                case 1:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE8;
                    break;
                case 2:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE8_FORCE;
                    break;

                case 3:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE9;
                    break;
                case 4:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE9_FORCE;
                    break;

                case 5:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE10;
                    break;
                case 6:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE10_FORCE;
                    break;

                case 7:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE11;
                    break;
                case 8:
                    BrowserSetting.EmurateBrowser = BrowserSetting.BrowserVer.IE11_FORCE;
                    break;

            }

            Properties.Settings.Default.ImageStoreDir = tbImageStore.Text;
            Properties.Settings.Default.LogStoreDir = tbLogDir.Text;

            Properties.Settings.Default.SyncronizeTimerProcess = cbSyncronizeTimerProcess.Checked;

            Properties.Settings.Default.LogStoreType = (uint)cbLogStoreType.SelectedIndex;
            Properties.Settings.Default.CondCeil = (int)numCondCeil.Value;

            List<string> selectedItem = new List<string>();
            foreach (var it in clbShipDetail.CheckedItems)
            {
                selectedItem.Add((string)it);
            }
            Properties.Settings.Default.ShipDetailItem = string.Join(",", selectedItem);
            Properties.Settings.Default.SuppressSuceedWriteLog = cbSupressSuceessWriteLog.Checked;
            Properties.Settings.Default.UseMasterDataView = cbUseMasterDataView.Checked;
            Properties.Settings.Default.UseDevMenu = cbActivateDevMenu.Checked;
            Properties.Settings.Default.MuteOnMinimize = cbMuteOnMinimize.Checked;
            Properties.Settings.Default.HideDroppedShip = cbHideDrop.Checked;
            Properties.Settings.Default.ShowEnemyBattleStatus = cbShowEnemyStatus.Checked;
            Properties.Settings.Default.SuppressBrowserDialog = cbSuppressBrowserDialogs.Checked;

            Properties.Settings.Default.UseUpstreamProxy = cbUseUpstreamProxy.Checked;
            Properties.Settings.Default.UpstreamProxyHost = tbUpstreamProxyHost.Text;
            Properties.Settings.Default.UpstreamProxyPort = numUpstreamProxyPort.Value;

        }

        private void btBrowseImageStore_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "スクリーンショットを保存するディレクトリ";
            dlg.SelectedPath = tbImageStore.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
                tbImageStore.Text = dlg.SelectedPath;
        }


        private void btBrowseLogDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "ログ情報を保存するディレクトリ";
            dlg.SelectedPath = tbLogDir.Text;

            if (dlg.ShowDialog() == DialogResult.OK)
                tbLogDir.Text = dlg.SelectedPath;
        }

        private void btTimerConf_Click(object sender, EventArgs e)
        {
            TimerRPC.ShowTimerPreferenceDlg();
        }

        private void btGoogleAuthorize_Click(object sender, EventArgs e)
        {
            if (tbGoogleAuthorizeURL.Enabled)
            {
                if (SpreadWraper.Autorize(tbGoogleAuthorizeToken.Text))
                {
                    tbGoogleAuthorizeURL.Enabled = false;
                    tbGoogleAuthorizeToken.Enabled = false;
                    btGoogleAuthorize.Text = "再認証";
                    tbGoogleAuthorizeURL.Text = "認証済み";
                }
                else
                    MessageBox.Show("認証に失敗しました");

                tbGoogleAuthorizeToken.Text = "";
            }
            else
            {
                tbGoogleAuthorizeURL.Enabled = true;
                tbGoogleAuthorizeToken.Enabled = true;
                btGoogleAuthorize.Text = "認証";
                tbGoogleAuthorizeURL.Text = SpreadWraper.AuthorizationURL;
            }
        }

        private void cbLogStoreType_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelLocalStorage.Visible = false;
            panelLocalStorage.Location = new Point(7, 44);

            panelGSpread.Location = new Point(7, 44);
            panelGSpread.Visible = false;

            switch (cbLogStoreType.SelectedIndex)
            {
                case 0:
                    panelLocalStorage.Visible = true;
                    return;
                case 1:
                    panelGSpread.Visible = true;
                    return;
                default:
                    throw new ArgumentOutOfRangeException("Unknown Selected Logstore Index");
            }
        }

        private void tbGoogleAuthorizeURL_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!tbGoogleAuthorizeURL.Text.StartsWith("http"))
                return;

            if (MessageBox.Show("クリップボードに認証用URIを貼り付けますか？",
                "KCB2", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            Clipboard.SetText(tbGoogleAuthorizeURL.Text);
        }

        #region ドラッグドロップによるリストボックスの並べ替え
        private void clbShipDetail_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            int dragIndex = clbShipDetail.IndexFromPoint(e.Location);
            if (dragIndex == -1)
                return;

            var dragItem = new DragItem(clbShipDetail,dragIndex);
            clbShipDetail.DoDragDrop(dragItem, DragDropEffects.All);
        }

        private void clbShipDetail_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(typeof(DragItem)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void clbShipDetail_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(DragItem)))
                return;

            DragItem item = (DragItem)e.Data.GetData(typeof(DragItem));

            clbShipDetail.Items.RemoveAt(item.Index);

            int dropIndex = clbShipDetail.IndexFromPoint(clbShipDetail.PointToClient(
                new Point(e.X, e.Y)));
            if (dropIndex == -1)
                dropIndex = clbShipDetail.Items.Count;

            clbShipDetail.Items.Insert(dropIndex, item.Object);
            clbShipDetail.SetItemChecked(dropIndex, item.Checked);
            clbShipDetail.SelectedItem = null;
            clbShipDetail.SelectedItem = item.Object;

        }

        class DragItem
        {
            public DragItem(CheckedListBox listBox,int index)
            {
                this.Object = listBox.Items[index];
                this.Checked = listBox.GetItemChecked(index);
                this.Index = index;
            }
            public object Object { get; private set; }
            public bool Checked { get; private set; }
            public int Index { get; private set; }
        }

        #endregion

        private void cbDetailStatus_CheckedChanged(object sender, EventArgs e)
        {
            cbHideDrop.Enabled = cbDetailStatus.Checked;
        }

        private void cbUseUpstreamProxy_CheckedChanged(object sender, EventArgs e)
        {
            groupBox10.Enabled = cbUseUpstreamProxy.Checked;
        }
    }
}
