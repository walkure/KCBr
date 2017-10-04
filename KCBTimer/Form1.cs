using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.ServiceModel;
using System.Net.Http;
using System.Threading.Tasks;

using KCB;
using Newtonsoft.Json.Linq;

namespace KCBTimer
{
    public partial class Form1 : Form
    {
        ServiceHost servHost;
        bool bTcpMode = false;
        public Form1()
        {
            InitializeComponent();
            IntPtr dummyHandle = Handle;

            lvMission.DoubleBuffer(true);
            lvNDock.DoubleBuffer(true);
            lvMission.LoadColumnWithOrder(Properties.Settings.Default.MissionColumnWidth);
            lvNDock.LoadColumnWithOrder(Properties.Settings.Default.DockColumnWidth);

            var host = new RemoteHost(this);
            servHost = new ServiceHost(host);
            if (Properties.Settings.Default.NetTcp)
            {
                var bind = new NetTcpBinding(SecurityMode.None);
                bind.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
                servHost.AddServiceEndpoint(typeof(KCB.RPC.IUpdateNotification), bind, new Uri("net.tcp://localhost/kcb-update-channel"));
                servHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new HogeFugaValidator();
                servHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = System.ServiceModel.Security.UserNamePasswordValidationMode.Custom;
                Debug.WriteLine("Protocol:Net.Tcp");
                bTcpMode = true;
            }
            else
            {
                servHost.AddServiceEndpoint(typeof(KCB.RPC.IUpdateNotification), new NetNamedPipeBinding(), new Uri("net.pipe://localhost/kcb-update-channel"));
                Debug.WriteLine("Protocol:Net.Pipe");
            }
            servHost.Open();

            //起動し終えたのでシグナル状態へ
            Program._mutex.ReleaseMutex();

        }

        class HogeFugaValidator : System.IdentityModel.Selectors.UserNamePasswordValidator
        {
            public override void Validate(string userName, string password)
            {
                if (null == userName || null == password)
                {
                    throw new ArgumentNullException();
                }
                if (!(userName == "hoge" && password == "fuga"))
                {
                    throw new System.IdentityModel.Tokens.SecurityTokenException("Unknown Username or Password");
                }
            }
        }

        public void UpdateMessage(string format,params object[] args)
        {
            var msg = string.Format(format,args);
            Debug.WriteLine(msg);
//            tbLog.Text += msg + "\r\n";
        }

        #region WCF RPCハンドラ

        public void UpdateNDock(int dockNum, string shipName, DateTime finishTime)
        {
            UpdateMessage("UpdateNDock:{0},{1},{2}",
                dockNum, shipName, finishTime);

            if (dockNum > lvNDock.Items.Count)
                return;
            NDockListViewItem it = (NDockListViewItem)lvNDock.Items[dockNum - 1];

            it.Update(dockNum, shipName, finishTime);

        }

        public void UpdateMission(int fleetNum, string fleetName, string missionName, DateTime finishTime)
        {
            UpdateMessage("UpdateMission:{0},{1},{2},{3}",
                fleetNum, fleetName, missionName, finishTime);

            if (fleetNum == 1)
                return;

            if (fleetNum > lvMission.Items.Count + 1)
                return;

            MissionListViewItem it = (MissionListViewItem)lvMission.Items[fleetNum - 2];
            it.Update(string.Format("No.{0}", fleetNum), fleetName, missionName, finishTime);

        }

        string currentMemberID = "";
        public void UpdateParameters(string memberID,int dockMax, int deckMax)
        {
            UpdateMessage("UpdateParameters:{0},{1}", dockMax, deckMax);

            if (currentMemberID != memberID)
            {
                currentMemberID = memberID;
                lvNDock.Items.Clear();
                lvMission.Items.Clear();
            }

            int diff = dockMax - lvNDock.Items.Count;
            for (int n = 0; n < diff; n++)
                lvNDock.Items.Add(new NDockListViewItem(this));

            if (deckMax > 1)
            {

                deckMax--;
                diff = deckMax - lvMission.Items.Count;
                for (int n = 0; n < diff; n++)
                    lvMission.Items.Add(new MissionListViewItem(this));
            }

        }

        DateTime dtCondRecoverTime = DateTime.MaxValue;
        string condRecoverTimerMsg = "";

        public void UpdateConditionTimer(int fleetNum, string fleetName,DateTime finishTime)
        {
            UpdateMessage("UpdateCondTimer:{0},{1},{2}", fleetNum,fleetName, finishTime);

            TimeSpan diff = finishTime - DateTime.Now;
            if (Math.Floor(diff.TotalSeconds) > 0)
            {
                dtCondRecoverTime = finishTime;
                condRecoverTimerMsg = string.Format("まもなく第{0}艦隊({1})のコンディションが回復します",fleetNum,fleetName);
                Text = string.Format("遠征・修理ドック情報 [回復通知:({0}){1} {2}]", fleetNum, fleetName, finishTime.ToString());
                return;
            }

            dtCondRecoverTime = DateTime.MaxValue;
            condRecoverTimerMsg = "";
            Text = "遠征・修理ドック情報";

        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dtNow = DateTime.Now;
            foreach (MissionListViewItem it in lvMission.Items)
            {
                it.OnTimer(dtNow);
            }

            foreach (NDockListViewItem it in lvNDock.Items)
            {
                it.OnTimer(dtNow);
            }

            if (dtNow > dtCondRecoverTime)
            {
                ShowBaloonMessage("コンディション回復通知", condRecoverTimerMsg);
                TimerHandlerListViewItem.PlaySound(Properties.Settings.Default.CondSound);
                dtCondRecoverTime = DateTime.MaxValue;
                condRecoverTimerMsg = "";
                Text = "遠征・修理ドック情報";
            }

        }

        bool bExitApp = false;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!bExitApp)
                e.Cancel = true;

            Properties.Settings.Default.MissionColumnWidth = lvMission.SaveColumnWithOrder();
            Properties.Settings.Default.DockColumnWidth = lvNDock.SaveColumnWithOrder();

            WindowState = FormWindowState.Normal;
            Properties.Settings.Default.FormBounds = Bounds;
            Properties.Settings.Default.Save();

            Visible = false;
        }

        public void ShutdownApplication()
        {
            bExitApp = true;
            Close();
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShutdownApplication();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Visible)
                Activate();
            else
            {
                this.allowshowdisplay = true; 
                Visible = true;
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern bool BringWindowToTop(IntPtr hWnd);

        Form2 confDlg = null;
        public void ShowPreference()
        {
            if (confDlg != null)
            {
                confDlg.Show();
                confDlg.Activate();
                BringWindowToTop(confDlg.Handle);
                return;
            }

            confDlg = new Form2();
            confDlg.TcpMode = bTcpMode;
            confDlg.ParentForm = this;
            confDlg.FormClosed += (FormClosedEventHandler)
                ((Object sender, FormClosedEventArgs e) => { confDlg = null; });
            confDlg.Show();
            confDlg.Activate();
            BringWindowToTop(confDlg.Handle);
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPreference();
        }

        /// <summary>
        /// バルーンチップを表示する
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void ShowBaloonMessage(string title,string msg)
        {
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.BalloonTipText = msg;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(3000);

            var slackWebhookUri = Properties.Settings.Default.SlackWebHookURI;
            if(!string.IsNullOrEmpty(slackWebhookUri))
            {
                PostSlackMessageAsync(slackWebhookUri, Properties.Settings.Default.SlackUserName,
                    Properties.Settings.Default.SlackIcon, Properties.Settings.Default.SlackChannel,
                    Properties.Settings.Default.SlackMessagePrefix, title + ":" + msg);
            }

        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            showToolStripMenuItem_Click(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.FormBounds.IsEmpty)
                Bounds = Properties.Settings.Default.FormBounds;

//            Activate();
        }

        private bool allowshowdisplay = false;
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }

        private HttpClient _client = new HttpClient();

        public Task<HttpResponseMessage> PostSlackMessageAsync(string entryPoint, string userName,
            string icon, string channel, string prefix, string text)
        {
            var obj = new JObject();

            if (!string.IsNullOrEmpty(channel))
                obj.Add("channel", channel);

            if (!string.IsNullOrEmpty(userName))
                obj.Add("username", userName);

            if (!string.IsNullOrEmpty(icon))
            {
                if (icon.StartsWith("https://") || icon.StartsWith("https://"))
                    obj.Add("icon_url", icon);

                if (icon.StartsWith(":") && icon.EndsWith(":"))
                    obj.Add("icon_emoji", icon);
            }

            if (!string.IsNullOrEmpty(prefix))
                text = prefix + text;

            obj.Add("text", text);

            var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "payload", obj.ToString() }, });

            return _client.PostAsync(entryPoint, content);

        }
    }





}
