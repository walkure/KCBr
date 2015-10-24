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
    public partial class FormSessionInfo : Form
    {
        public string APIToken;
        public DateTime FirstSession;
        public DateTime LatestSession;
        public string ServerHost;

        Dictionary<string, string> _serverMap = new Dictionary<string, string>();

        public FormSessionInfo()
        {
            InitializeComponent();

            /*
             * http://203.104.209.7/gadget/js/kcs_const.js
             * http://203.104.209.7/kcs/world.swf
             * http://203.104.209.7/kcsapi/api_world/get_worldinfo (POST)
             */
            _serverMap["203.104.105.167"] = "横須賀鎮守府";
            _serverMap["125.6.184.15"]    = "呉鎮守府";
            _serverMap["125.6.184.16"]    = "佐世保鎮守府";
            _serverMap["125.6.187.205"]   = "舞鶴鎮守府";
            _serverMap["125.6.187.229"]   = "大湊警備府";
            _serverMap["125.6.187.253"]   = "トラック泊地";
            _serverMap["125.6.188.25"]    = "リンガ泊地";
            _serverMap["203.104.248.135"] = "ラバウル基地";
            _serverMap["125.6.189.7"]     = "ショートランド泊地";
            _serverMap["125.6.189.39"]    = "ブイン基地";
            _serverMap["125.6.189.71"]    = "タウイタウイ泊地";
            _serverMap["125.6.189.103"]   = "パラオ泊地";
            _serverMap["125.6.189.135"]   = "ブルネイ泊地";
            _serverMap["125.6.189.167"]   = "単冠湾泊地";
            _serverMap["125.6.189.215"]   = "幌筵泊地";
            _serverMap["125.6.189.247"]   = "宿毛湾泊地";
            _serverMap["203.104.209.23"]  = "鹿屋基地";
            _serverMap["203.104.209.39"]  = "岩川基地";
        }

        private void FormSessionInfo_Load(object sender, EventArgs e)
        {
            tbAPIToken.Text = APIToken;
            tbFirstSesssionDate.Text = FirstSession.ToString();
            tbLatestSessionDate.Text = LatestSession.ToString();
            if (ServerHost.Length > 0)
            {
                string serverName = "未知";
                if (_serverMap.ContainsKey(ServerHost))
                    serverName = _serverMap[ServerHost];

                tbServerHost.Text = string.Format("{0} ({1})", ServerHost,
                    serverName);
            }
            else
                tbServerHost.Text = "";
        }

        private void tbAPIToken_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tbAPIToken.Text.Length == 0)
                return;

            if (MessageBox.Show("クリップボードにAPIトークンを貼り付けますか？",
                "KCB2", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            Clipboard.SetText(tbAPIToken.Text);
        }
    }
}
