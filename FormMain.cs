using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using System.Threading;

namespace KCB2
{
    public partial class FormMain : Form
    {
        FormShipList _wndShipList = null;
        FormItemList _wndItemList = null;
        FormSlotItemList _wndSlotItemList = null;
        FormLog _wndLog = null;
        FormMasterData _wndMaster = null;
        LogManager.LogManager _logManager = null;
        GSpread.SpreadSheetWrapper _gsWrapper = null;
        RingBuffer<JSONData> _logLastJSON = null;
        HTTProxy _httProxy = null;
        System.Net.IWebProxy _sysProxy = null;

        /// <summary>
        /// 表示がロックされている時true
        /// </summary>
        bool _bLock;

        bool _bPortable;

#if RESTORE_VOLUME
        /// 起動時の音量とミュートフラグ
        bool bootMute;
        uint bootVol;
#endif
//        string _fiddlerOverrideGateway;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="bPortable">設定を保存しない時true</param>
        public FormMain(bool bPortable)
        {
            InitializeComponent();
            IntPtr ptr = Handle;
            _bPortable = bPortable;

            MouseWheel += FormMain_MouseWheel;
            MouseEnter += FormMain_MouseEnter;
            _sysProxy = System.Net.WebRequest.DefaultWebProxy;

            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            imageListSlotItemType.Images.AddStrip(new Bitmap(
                asm.GetManifestResourceStream("KCB2.SlotItemsSmallIcon.bmp")));
            imageListSlotItemType.TransparentColor = Color.FromArgb(255, 0, 255);
            deckMemberList.SlotItemIconImageList = imageListSlotItemType;

#if RESTORE_VOLUME
            ///起動時の音量設定を覚えておく
            using (var mixer = new MixerAPI())
            {
                bootVol = mixer.Volume;
                bootMute = mixer.Mute;
            }
#endif

            // WebRequestの同時処理数を引き上げる
            if (ServicePointManager.DefaultConnectionLimit < 20)
            {
                Debug.WriteLine(string.Format("ServicePointManager.DefaultConnectionLimit {0} -> 20",
                    ServicePointManager.DefaultConnectionLimit));
                ServicePointManager.DefaultConnectionLimit = 20;
            }

            //スレッドプール数を増やす
            int minWorkerThread, minCompletionPortThread;
            ThreadPool.GetMinThreads(out minWorkerThread, out minCompletionPortThread);
            Debug.WriteLine(string.Format("ManagedThread minWorkder:{0} minCompPortThread:{1}",
                minWorkerThread, minCompletionPortThread));
            if (minWorkerThread < 20)
            {
                Debug.WriteLine(string.Format("minWorkerThread {0} -> 20", minWorkerThread));
                ThreadPool.SetMinThreads(20, minCompletionPortThread);
            }


            _httProxy = new HTTProxy();
            _httProxy.BeforeRequest += new HTTProxy.HTTProxyCallbackHandler(_httProxy_BeforeRequest);
            _httProxy.AfterSessionCompleted += new HTTProxy.HTTProxyCallbackHandler(_httProxy_AfterSessionCompleted);

            ///過去の設定を引っ張ってる場合。
            if (Properties.Settings.Default.ProxyPort <= 0)
                Properties.Settings.Default.ProxyPort = 8088;

            _gsWrapper = new GSpread.SpreadSheetWrapper();

            if (!_httProxy.Start(Properties.Settings.Default.ProxyPort))
                MessageBox.Show("HttpListenerの起動に失敗しました。情報は取得されません。\n設定を確認してください");

            else
                UpdateProxyConfiguration();


            _logManager = new LogManager.LogManager(this);
//            _fiddlerOverrideGateway = Properties.Settings.Default.FiddlerOverrideGateway;

#if !DEBUG
            if (Properties.Settings.Default.UseDevMenu)
#endif
            {
                _watchSession = true;
                _logLastJSON = new RingBuffer<JSONData>(Properties.Settings.Default.SkipbackJSON+1);
            }


            deckMemberList.UpdateDeckStatus = UpdateDeckConditionTime;

            ///縦横切替を使うかどうか
            switchViewModeToolStripMenuItem.Visible = false;

            setLogStore();
        }

        /// <summary>
        /// プロキシ設定を反映
        /// </summary>
        void UpdateProxyConfiguration()
        {
            if (Properties.Settings.Default.UseUpstreamProxy)
            {
                _httProxy.UpstreamProxy = new System.Net.WebProxy(
                    Properties.Settings.Default.UpstreamProxyHost,
                    (int)Properties.Settings.Default.UpstreamProxyPort);

                HTTProxy.SetProcessProxy(string.Format("http=localhost:{0} https={1}:{2}",
                    Properties.Settings.Default.ProxyPort,
                    Properties.Settings.Default.UpstreamProxyHost,
                    Properties.Settings.Default.UpstreamProxyPort), "");
            }
            else
            {
                _httProxy.UpstreamProxy = _sysProxy;

                HTTProxy.SetProcessProxy(string.Format("http=localhost:{0}",
                    Properties.Settings.Default.ProxyPort), "");
            }

            //Google Spreadsheet API関連のプロクシ設定
            _gsWrapper.Proxy = _httProxy.UpstreamProxy;


        }

        void FormMain_MouseEnter(object sender, EventArgs e)
        {
            Focus();
        }

        void FormMain_MouseWheel(object sender, MouseEventArgs e)
        {
            deckMemberList.DoDeckListMouseWheelEvent(e);
        }

        /// <summary>
        /// ログストアを切り替える
        /// </summary>
        void setLogStore()
        {
            Debug.WriteLine("Setting LogType:" + Properties.Settings.Default.LogStoreType.ToString());
            switch (Properties.Settings.Default.LogStoreType)
            {
                case 0:
                    //CSV
                    Debug.WriteLine("Set CSVLog");
                    _logManager.LogStore = new LogStore.CSVLogStore();
                    return;

                case 1:
                    //GoogleStorage
                    Debug.WriteLine("Set GoogleLog");
                    var glogStore = new LogStore.GSpreadLogStore(_gsWrapper);

                    if (!_gsWrapper.Refresh())
                        MessageBox.Show("アクセストークンの取得に失敗しました。ログは保存されません。\n\n再認証してください。");

                    _logManager.LogStore = glogStore;
                    return;

                default:
                    throw new ArgumentOutOfRangeException("Unknown storeType");
            }
        }

        SessionProcessor _processor = null;
        ShipStatusManager _statusManager = new ShipStatusManager();
        TimerRPCManager _timerRPC = new TimerRPCManager();

#if DEBUG
        FormJSONTest testerWnd;
#endif
        FormJSONLog _logWnd = null;

        #region フォームハンドラ
        private void FormMain_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = 
                Properties.Settings.Default.SuppressBrowserDialog;


            ///ジョブキュー起動
            _processor = new SessionProcessor(this, _statusManager);

            ///タイマを起動
            if (Properties.Settings.Default.SyncronizeTimerProcess)
            {
                // TODO:KCBTimer.exeを起動してメッセージループの開始を待つ
                using (var proc = Process.Start(GetTimerPath()))
                    proc.WaitForInputIdle();

                //WCFサーバが起動するのを待つ
                TimerRPCManager.WaitForWCFStartup();
            }

#if DEBUG
            testerWnd = new FormJSONTest();
            testerWnd.processor = _processor;
            testerWnd.Show();
#endif

            if (Properties.Settings.Default.UseLogWindow)
            {
                _wndShipList = new FormShipList(imageListSlotItemType);
                if (_wndShipList.Visible = Properties.Settings.Default.ShipListVisible)
                    _wndShipList.Show();

                _wndItemList = new FormItemList(imageListSlotItemType);
                if (_wndItemList.Visible = Properties.Settings.Default.ItemListVisible)
                    _wndItemList.Show();

                _wndSlotItemList = new FormSlotItemList(imageListSlotItemType);
                if (_wndSlotItemList.Visible = Properties.Settings.Default.SlotItemListVisible)
                    _wndSlotItemList.Show();

                _wndLog = new FormLog(_logManager);
                if (_wndLog.Visible = Properties.Settings.Default.LogWndVisible)
                    _wndLog.Show();
            }

            if (Properties.Settings.Default.UseMasterDataView)
            {
                _wndMaster = new FormMasterData(imageListSlotItemType);
                masterDataToolStripMenuItem.Visible = true;
                if(_wndMaster.Visible = Properties.Settings.Default.MasterDataWndVisible)
                    _wndMaster.Show();
            }

            devMenuToolStripMenuItem.Visible = Properties.Settings.Default.UseDevMenu;
#if DEBUG
            devMenuToolStripMenuItem.Visible = true;
            showJsonLogToolStripMenuItem_Click(null, null);
#endif

            // panel内にWebBrowserを入れておかないと何故かLoad時はnullのまま
            var obj = webBrowser1.ActiveXInstance;

            //セキュリティマネージャの上書き。クロスドメインでのフレームまたぎを許可
            KCB.COM.IServiceProvider sp = obj as KCB.COM.IServiceProvider;
            object ops;
            sp.QueryService(ref KCB.COM.SID_SProfferService, ref KCB.COM.IID_IProfferService, out ops);
            KCB.COM.IProfferService ps = ops as KCB.COM.IProfferService;
            int cookie = 0;
            ps.ProfferService(ref KCB.COM.IID_IInternetSecurityManager, webBrowser1, ref cookie);

            Graphics g = Graphics.FromHwnd(Handle);
            Debug.WriteLine(string.Format("DPI X:{0} Y:{1}",g.DpiX,g.DpiY));

            if(!Properties.Settings.Default.MainFormLocation.IsEmpty)
                Location = Properties.Settings.Default.MainFormLocation;


            webBrowser1.Navigate(Properties.Settings.Default.GadgetURI);
            UpdateStatus("ゲームURIの読み込みを開始します");


        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("終了しますか？", "KCBr2", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            {
                e.Cancel = true;
                return;
            }

            if (Properties.Settings.Default.SyncronizeTimerProcess)
                _timerRPC.ShutdownTimer();

            _logManager.SaveLog();

            //Closeを叩かないと、FormClosingなどが呼ばれずに終わるようで、設定が保存されない
            if (_wndShipList != null)
            {
                Properties.Settings.Default.ShipListVisible 
                    = _wndShipList.Visible && _wndShipList.WindowState != FormWindowState.Minimized;
                _wndShipList.Close();
            }
            
            if (_wndItemList != null)
            {
                Properties.Settings.Default.ItemListVisible
                    = _wndItemList.Visible && _wndItemList.WindowState != FormWindowState.Minimized;
                _wndItemList.Close();
            }
            
            if (_wndSlotItemList != null)
            {
                Properties.Settings.Default.SlotItemListVisible
                    = _wndSlotItemList.Visible && _wndSlotItemList.WindowState != FormWindowState.Minimized;
                _wndSlotItemList.Close();
            }

            if (_wndLog != null)
            {
                Properties.Settings.Default.LogWndVisible
                    = _wndLog.Visible && _wndLog.WindowState != FormWindowState.Minimized;
                _wndLog.Close();
            }

            if (_wndMaster != null)
            {
                Properties.Settings.Default.MasterDataWndVisible
                    = _wndMaster.Visible && _wndMaster.WindowState != FormWindowState.Minimized;
                _wndMaster.Close();
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {

#if DEBUG
            testerWnd.Close();
#endif
            if(_logWnd != null)
                _logWnd.Close();


#if RESTORE_VOLUME
            ///起動時の設定を戻す
            using (var mixer = new MixerAPI())
            {
                mixer.Volume = bootVol;
                mixer.Mute = bootMute; 
            }
#endif
            if (WindowState == FormWindowState.Normal)
                Properties.Settings.Default.MainFormLocation = Location;
            if (WindowState == FormWindowState.Minimized)
                Visible = false;

            _httProxy.Stop();

            if (!_bPortable)
            {
                Debug.WriteLine("Save Configuration");
                Properties.Settings.Default.Save();
            }

        }

        /// <summary>
        /// 最小化前にミュートされていたかどうかの状態フラグ
        /// </summary>
        bool bMuted = false;

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.MuteOnMinimize)
            {
                try
                {
                    using (var mixer = new MixerAPI())
                    {
                        //ウィンドウが最小化された。
                        if (WindowState == FormWindowState.Minimized)
                        {
                            bMuted = mixer.Mute;
                            mixer.Mute = true;
                        }
                        else
                        {
                            mixer.Mute = bMuted;
                        }
                    }
                }
                catch (MixerAPI.MixerException ex)
                {
                    //RDP経由するとMixerが開けなくてコンストラクタが例外を投げる
                    Debug.WriteLine("MixerException:" + ex.ToString());
                }
            }
        }

        #endregion フォームハンドラ

        /// <summary>
        /// フラッシュ位置を検出してウィンドウをスクロール
        /// </summary>
        /// <returns>成功したらtrue</returns>
        bool adjustFlashPosition()
        {
            //もしリロード前に表示されていたら消す
            enemyFleetList.Visible = false;

            KCB.WebBrowserEx.IWebBrowser2 wb = (webBrowser1.ActiveXInstance as KCB.WebBrowserEx.IWebBrowser2);
            mshtml.IHTMLDocument3 doc = (wb.Document as mshtml.IHTMLDocument3);
            mshtml.IHTMLElement elm = doc.getElementById("game_frame");

            if (elm == null)
                return false;

            /*
             * http://stackoverflow.com/questions/10645143/webbrowsercontrol-unauthorizedaccessexception-when-accessing-property-of-a-fram
             */
            object ppvObject = null;
            mshtml.IHTMLWindow2 wnd = (((mshtml.IHTMLFrameBase2)elm).contentWindow);
            ((KCB.WebBrowserEx.IServiceProvider)wnd).QueryService(
                KCB.WebBrowserEx.IWebBrowserApp_GUID, KCB.WebBrowserEx.IWebBrowser2_GUID, out ppvObject);

            KCB.WebBrowserEx.IWebBrowser2 inFrm = ppvObject as KCB.WebBrowserEx.IWebBrowser2;

            /* 何故かフレーム間のアクセス制限を解除しておかないとここで例外が飛ぶ。
             */
            mshtml.IHTMLDocument3 doc_frm = inFrm.Document as mshtml.IHTMLDocument3;
            mshtml.IHTMLElement c_elm = doc_frm.getElementById("externalswf");
            if (c_elm == null)
                return false;

            //スクロールバーを消す
            webBrowser1.Document.Body.Style = "overflow-x:hidden;overflow-y:hidden";

            // iframeのオフセットを算出
            int frameOffsetLeft = 0, frameOffsetTop = 0;
            while (c_elm != null)
            {
                Debug.WriteLine(string.Format("FrameOffset {0}+{1}={2},{3}+{4}={5}",
                    frameOffsetTop, c_elm.offsetTop, frameOffsetTop + c_elm.offsetTop,
                    frameOffsetLeft, c_elm.offsetLeft, frameOffsetLeft + c_elm.offsetLeft));

                frameOffsetTop += c_elm.offsetTop;
                frameOffsetLeft += c_elm.offsetLeft;

                c_elm = c_elm.offsetParent;
            }

            //flashオブジェクトのiframe内オフセットを算出
            int elementOffsetTop = 0, elementOffsetLeft = 0;
            while (elm != null)
            {
                Debug.WriteLine(string.Format("ElementOffset {0}+{1}={2},{3}+{4}={5}",
                    elementOffsetTop, elm.offsetTop, elementOffsetTop + elm.offsetTop,
                    elementOffsetLeft, elm.offsetLeft, elementOffsetLeft + elm.offsetLeft));

                elementOffsetTop += elm.offsetTop;
                elementOffsetLeft += elm.offsetLeft;

                elm = elm.offsetParent;
            }

            HtmlWindow targetHtmlWnd = webBrowser1.Document.Window;
            targetHtmlWnd.ScrollTo(0, 0);
            targetHtmlWnd.ScrollTo(elementOffsetLeft + frameOffsetLeft,
                elementOffsetTop + frameOffsetTop);

            UpdateStatus("表示範囲を調整しました");

            return true;
        }

        #region HTTProxyハンドラ

        void _httProxy_AfterSessionCompleted(HTTProxy.SessionInfo info)
        {
            //MIME形式を見る
            if (info.Response.ContentType != "text/plain")
            {
                Debug.WriteLine("MIMEType is not text/plain. return");
                return;
            }


            var httproxySession = new HTTProxySessionData(info);

            if (_watchSession)
            {
                var q = httproxySession.QueryParam;
                if (q.ContainsKey("api_token"))
                    _updateAPIToken(q["api_token"]);
            }

            _processor.Add(httproxySession);
        }

        void _httProxy_BeforeRequest(HTTProxy.SessionInfo info)
        {
            Debug.WriteLine("BeforeRequest:" + info.Uri);


            //ゲーム開始時のトークンと時間を覚えておく
            if (info.Uri.ToString().Contains("mainD2.swf"))
            {
                //別スレッドから呼ばれるのでUIスレッドに処理を投げる
                BeginInvoke((MethodInvoker)(() => adjustFlashPosition()));

                if (_watchSession)
                    _logFirstSession(info.Uri.ToString());
            }


            /* 艦これAPI以外へのアクセスは全部無視する
             */
            if (!info.Uri.PathAndQuery.StartsWith("/kcsapi/"))
            {
                info.Ignore = true;
                return;
            }

            switch (info.Uri.PathAndQuery)
            {
                case "/kcsapi/api_req_map/next":
                    CheckShipStatus(info);
                    return;
            }

        }

        #endregion

        DateTime _firstSesson;
        DateTime _latestSession;
        string _serverHost = "";
        string _apiToken = "";
        bool _watchSession = false;

        /// <summary>
        /// mainD2.swfへのアクセスからセッション取得時刻とかを取ってくる
        /// </summary>
        /// <param name="mainD2URL"></param>
        void _logFirstSession(string mainD2URL)
        {
            _firstSesson = DateTime.Now;
            _latestSession = DateTime.Now;

            Uri uri = new Uri(mainD2URL);

            _serverHost = uri.Host;
            var q = SessionData.ParsePostQuery(uri.Query);
            _apiToken = q["api_token"];

            Debug.WriteLine(string.Format("New session: host:{0} token:{1}",_serverHost,_apiToken));

        }

        void _updateAPIToken(string currentApiToken)
        {
            if (_apiToken != currentApiToken)
            {
                Debug.WriteLine("APIToken update detected");
                _latestSession = DateTime.Now;
                _apiToken = currentApiToken;
            }
        }

        #region 更新反映ハンドラ

        public void AddJSONLog(SessionData oData)
        {
            if(_logLastJSON != null)
                _logLastJSON.Add(new JSONData(oData));

            if (_logWnd != null)
                _logWnd.AddJSON(new JSONData(oData));
        }

        public void JSONLogWndClosed()
        {
            _logWnd = null;
        }

        /// <summary>
        /// ステータスバーへのステータス表示メッセージ更新
        /// </summary>
        /// <param name="format">書式文字列</param>
        /// <param name="args">引数</param>
        public void UpdateStatus(string format, params object[] args )
        {
            string msg = string.Format(format, args);
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => toolStripStatusLabel1.Text = msg));
            else
                toolStripStatusLabel1.Text = msg;
        }

        /// <summary>
        /// ウィンドウタイトルを書き換え
        /// </summary>
        /// <param name="Title"></param>
        public void UpdateWindowTitle(string Title)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => Text = Title));
            else
                Text = Title;
        }

        public void UpdateDeckMemberList(MemberData.Ship shipData, IEnumerable<MemberData.Deck.Fleet> deckList)
        {
            deckMemberList.UpdateDeck(deckList, shipData);
            missionList.UpdateMissionList(deckList,shipData);
            _timerRPC.UpdateMission(deckList);
        }

        public void UpdateDockCount(int kdock, int ndock)
        {
            dockBuild.SetOpenDockCount(kdock);
            dockRepair.SetOpenDockCount(ndock);
        }

        public void UpdateDeckCount(int count)
        {
            missionList.SetActiveDeck(count);
        }

        public void UpdateMaterialData(MemberData.Material dat)
        {
            materialList.Update(dat);
        }

        public void UpdateBuildDock(MemberData.Dock dock)
        {
            dockBuild.UpdateDockList(dock.BuildDock);
        }

        public void UpdateRepairDock(MemberData.Dock dock)
        {
            dockRepair.UpdateDockList(dock.RepairDock);
            _timerRPC.UpdateNDock(dock);
        }

        public void UpdateQuestList(IEnumerable<MemberData.Quest.Info> questList)
        {
            currentQuestList = questList;
            if(lbQuest.InvokeRequired)
                lbQuest.Invoke((MethodInvoker)(() => _updateQuestLBForm(questList)));
            else
                _updateQuestLBForm(questList);

        }

        void _updateQuestLBForm(IEnumerable<MemberData.Quest.Info> questList)
        {
            lbQuest.BeginUpdate();
            lbQuest.Items.Clear();
            foreach (var it in questList)
            {
                var item = new ListBoxEx.ListBoxItem();
                item.Text = string.Format("{0}{1} {2}",it.StateString,it.Name , it.ProgressMsg);
                item.ToolTip = it.Description;
                if (it.ProgressFlag == 1)
                    item.BackColor = Color.LightGreen;
                else if (it.ProgressFlag == 2)
                    item.BackColor = Color.LimeGreen;

                lbQuest.Items.Add(item);
            }
            lbQuest.Refresh();
            lbQuest.EndUpdate();
        }

        public void UpdateShipList(IEnumerable<MemberData.Ship.Info> shipList)
        {
            if(_wndShipList != null)
                _wndShipList.UpdateShipList(shipList);
        }

        public void UpdateShipListDock(MemberData.Dock dockData)
        {
            if (_wndShipList != null)
                _wndShipList.UpdateShipListDock(dockData);
        }

        public void UpdateShipListDeck(MemberData.Deck deckData)
        {
            if (_wndShipList != null)
                _wndShipList.UpdateShipListDeck(deckData);
        }

        public void UpdateMaxShipItemValue(int maxShip, int maxItem)
        {
            if (_wndShipList != null)
                _wndShipList.MaxShip = maxShip;
            
            if (_wndItemList != null)
                _wndItemList.MaxItem = maxItem;
            
            if (_wndSlotItemList != null)
                _wndSlotItemList.MaxItem = maxItem;
        }

        public void UpdateShipLock(int ship_id,bool bLock)
        {
            if (_wndShipList != null)
                _wndShipList.UpdateShipLock(ship_id, bLock);
        }

        public void UpdateItemList(IEnumerable<MemberData.Item.Info> itemList)
        {
            if (_wndItemList != null)
                _wndItemList.UpdateItemList(itemList);
            if (_wndSlotItemList != null)
                _wndSlotItemList.UpdateSlotItemList(itemList);
        }

        public void UpdateItemOwner(IDictionary<int, MemberData.Ship.SlotItemOwner> itemOwner,
            IDictionary<int,int> itemType)
        {
            if (_wndItemList != null)
                _wndItemList.UpdateItemOwner(itemOwner);

            if (_wndSlotItemList != null)
                _wndSlotItemList.UpdateSlotItemOwner(itemOwner, itemType);
        }

        public void UpdateSlotItemLock(int itemId, bool bLock)
        {
            if (_wndItemList != null)
                _wndItemList.UpdateLockState(itemId, bLock);
        }

        public void AddBattleResult(LogData.BattleResultInfo info)
        {
            _logManager.AddBattleResult(info);
        }

        public void AddMissionResult(LogData.MissionResultInfo info)
        {
            _logManager.AddMissionResult(info);
        }

        public void AddCreateShipResult(IEnumerable<LogData.CreateShipInfo> infoL)
        {
            _logManager.AddCreateShipResult(infoL);
        }

        public void AddCreateItemResult(LogData.CreateItemInfo info)
        {
            _logManager.AddCreateItemResult(info);
        }

        public void UpdateMemberID(string memberID)
        {
            _logManager.LoadLog(memberID);
        }

        public void AddMaterialsChange(LogData.MaterialChangeInfo info)
        {
            _logManager.AddMaterialsChangeResult(info);
        }

        public void UpdateBasicInfo(MemberData.Basic basicInfo)
        {
            _timerRPC.UpdateParameters(basicInfo);
        }

        public void UpdateMasterData(MasterData.Ship shipMaster, MasterData.Item itemMaster)
        {
            if (_wndMaster != null)
                _wndMaster.UpdateMaster(shipMaster,itemMaster);
        }

        public void NotifyFinishBattle(string type)
        {
            _timerRPC.RPCFinishBattle(type);
        }

        #endregion

        #region コントロールハンドラ

        private void rdRepair_CheckedChanged(object sender, EventArgs e)
        {
            dockBuild.Visible = false;
            dockRepair.Visible = true;
            lbQuest.Visible = false;
        }

        private void rdBuild_CheckedChanged(object sender, EventArgs e)
        {
            dockBuild.Visible = true;
            dockRepair.Visible = false;
            lbQuest.Visible = false;

        }

        private void rdQuest_CheckedChanged(object sender, EventArgs e)
        {
            dockBuild.Visible = false;
            dockRepair.Visible = false;
            lbQuest.Visible = true;
        }

        private void reloadBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bLock)
            {
                MessageBox.Show("ロック中は操作できません。", "KCBr2");
                return;
            }

            if (MessageBox.Show("艦これゲームページを再読込します。\nよろしいですか？",
                 "KCBr2", MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) != DialogResult.OK)
                return;

            webBrowser1.Refresh(WebBrowserRefreshOption.Completely);
            enemyFleetList.Visible = false;
            UpdateStatus("ゲーム画面の再読み込みを開始します");

        }

        IEnumerable<MemberData.Quest.Info> currentQuestList = null;

        private void showQuestButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentQuestList == null)
            {
                MessageBox.Show("任務一覧が読み込まれていません。");
                return;
            }

            var dlg = new FormQuestList();

            dlg.StartPosition = FormStartPosition.Manual;
            dlg.Size = Properties.Settings.Default.QuestListSize;
            dlg.QuestList = currentQuestList;

            dlg.Location = statusStrip1.PointToScreen(
                new Point(showQuestButton.Bounds.Location.X,
                    showQuestButton.Bounds.Location.Y - dlg.Height - 2));

            dlg.Show();

        }

        private void screenShotButton_Click(object sender, EventArgs ev)
        {
            //usingでくくるとbmpの挿げ替えが出来ない。
            var bmp = webBrowser1.GetScreenShot();
            try
            {
                if (bmp == null)
                {
                    UpdateStatus("スクリーンショットの取得に失敗しました");
                    return;
                }

                UpdateStatus("スクリーンショットを取得しました");
                using (var dlg = new FormScreenShot())
                {
                    dlg.ActivateSaveFile = Properties.Settings.Default.ImageStoreDir.Length > 0;
                    if (dlg.ShowDialog() == DialogResult.Cancel)
                    {
                        UpdateStatus("取得したスクリーンショットを破棄しました");
                        return;
                    }
                    
                    //ヘッダを隠す
                    if (dlg.HideHeader)
                    {
                        Bitmap offImg = OffsetImage(bmp, new Point(0, 30));
                        if (offImg != null)
                        {
                            bmp.Dispose();
                            bmp = offImg;
                        }
                    }

                    if (dlg.SaveTarget
                        == FormScreenShot.ScreenShotSaveTarget.SaveAsFile)
                    {
                        string saveFile = DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmmss");
                        string savePath =
                            string.Format("{0}\\{1}.png", Properties.Settings.Default.ImageStoreDir,
                                saveFile);
                        Debug.WriteLine("SaveImage:" + savePath);
                        try
                        {
                            bmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                            UpdateStatus("スクリーンショット[{0}]を保存しました", saveFile);
                        }

                        catch(Exception e)
                        {
                            Debug.WriteLine("Image.Save thrown exception\n" + e.ToString());
                            UpdateStatus("スクリーンショットの保存に失敗しました[{0}]",e.Message);
                        }

                    }
                    else if (dlg.SaveTarget
                        == FormScreenShot.ScreenShotSaveTarget.Clipboard)
                    {
                        Clipboard.SetImage(bmp);

                        UpdateStatus("スクリーンショットをクリップボードへ転送しました");
                    }
                }
            }
            finally
            {
                if (bmp != null)
                    bmp.Dispose();
            }
        }

        //画像をオフセットしてコピーする
        Bitmap OffsetImage(Bitmap orgBmp,Point origin)
        {
            Debug.WriteLine(string.Format("OffsetImage:{0},{1}", origin.X, origin.Y));
            var bmp = new Bitmap(orgBmp.Width - origin.X, orgBmp.Height - origin.Y);
            if (bmp == null)
                return null;

            using (var g = Graphics.FromImage(bmp))
            {
                Point ptDraw = new Point(-origin.X, -origin.Y);
                g.DrawImage(orgBmp, ptDraw);
            }

            return bmp;
        }

        private void shipListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_wndShipList == null)
                return;

            if (_wndShipList.WindowState == FormWindowState.Minimized)
                _wndShipList.WindowState = FormWindowState.Normal;

            if (!_wndShipList.Visible)
                _wndShipList.Visible = true;
            else
                _wndShipList.Activate();
        }

        private void slotItemDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_wndItemList == null)
                return;

            if (_wndItemList.WindowState == FormWindowState.Minimized)
                _wndItemList.WindowState = FormWindowState.Normal;

            if (!_wndItemList.Visible)
                _wndItemList.Visible = true;
            else
                _wndItemList.Activate();

        }

        private void slotItemSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_wndSlotItemList == null)
                return;

            if (_wndSlotItemList.WindowState == FormWindowState.Minimized)
                _wndSlotItemList.WindowState = FormWindowState.Normal;

            if (!_wndSlotItemList.Visible)
                _wndSlotItemList.Visible = true;
            else
                _wndSlotItemList.Activate();


        }

        private void LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_wndLog == null)
                return;

            if (_wndLog.WindowState == FormWindowState.Minimized)
                _wndLog.WindowState = FormWindowState.Normal;

            if (!_wndLog.Visible)
                _wndLog.Visible = true;
            else
                _wndLog.Activate();

        }

        private void masterDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_wndMaster == null)
                return;

            if (_wndMaster.WindowState == FormWindowState.Minimized)
                _wndMaster.WindowState = FormWindowState.Normal;

            if (!_wndMaster.Visible)
                _wndMaster.Visible = true;
            else
                _wndMaster.Activate();
        }


        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new FormPreference())
            {
                dlg.BrowserVersion = webBrowser1.Version;
                dlg.TimerRPC = _timerRPC;
                dlg.SpreadWraper = _gsWrapper;
                dlg.ShowDialog();
            }

            webBrowser1.ScriptErrorsSuppressed = Properties.Settings.Default.SuppressBrowserDialog;
            UpdateProxyConfiguration();
            setLogStore();
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(FormAbout dlg = new FormAbout(webBrowser1.Version.ToString()))
                dlg.ShowDialog();
        }

        private void clearCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Internet Explorerのキャッシュを削除します。\nよろしいですか？",
                 "KCBr2", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            System.Diagnostics.Process.Start("rundll32.exe",
                "InetCpl.cpl,ClearMyTracksByProcess 8");

            if (Properties.Settings.Default.VerboseStatus)
                UpdateStatus("Internet Explorerのキャッシュを削除しました");
        }

        private void volumeButton_Click(object sender, EventArgs e)
        {
            var dlg = new FormVolume();

            dlg.StartPosition = FormStartPosition.Manual;

            dlg.Location = statusStrip1.PointToScreen(
                new Point(volumeButton.Bounds.Location.X,
                    volumeButton.Bounds.Location.Y - dlg.Height - 2));

            dlg.Show();
        }

        private void sendTimerInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_timerRPC.ExistWCFServer)
            {
                if (MessageBox.Show("タイマの起動を確認できませんでした。タイマを起動しますか？",
                    "KCBr2", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
                    return;

                // TODO:KCBTimer.exeを起動してメッセージループの開始を待つ
                using (var proc = Process.Start(GetTimerPath()))
                    proc.WaitForInputIdle();

                //WCFサーバが起動するのを待つ
                TimerRPCManager.WaitForWCFStartup();

            }
            _timerRPC.UpdateTimerState();
        }

        private void adjustFlashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adjustFlashPosition();
        }

        private void btUnlock_Click(object sender, EventArgs e)
        {
            if (!_bLock)
                return;

            if (tbUnlockPassword.Text != _unlockPassword)
            {
                MessageBox.Show("パスワードが違います", "KCBr2");
                return;
            }

            panelLock.Visible = false;
            panelLock.SendToBack();
            enableUserInterface(true);
            _bLock = false;
        }

        private void lockUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bLock)
                return;

            using (var dlg = new FormLock())
            {
                if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                panelLock.Visible = true;
                panelLock.BringToFront();
                enableUserInterface(false);
                _bLock = true;

                _unlockPassword = dlg.UnlockPassword;
                tbUnlockPassword.Text = "";
                Debug.WriteLine("Start Lock UI. password:" + _unlockPassword);
            }
        }

        string _unlockPassword = "";
        void enableUserInterface(bool bEnable)
        {
            //webBrowser1.Visible = bEnable;
            panelWebBrowserContainer.Enabled = bEnable;

            toolStripDropDownButton1.Enabled = bEnable;
            toolStripDropDownButton2.Enabled = bEnable;
            screenShotButton.Enabled = bEnable;
            showQuestButton.Enabled = bEnable;
            deckMemberList.Enabled = bEnable;
            rdBuild.Enabled = bEnable;
            rdRepair.Enabled = bEnable;

        }

        bool _bVerticalDesign = false;

        private void switchViewModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bVerticalDesign)
            {
                //デフォルトデザイン
                this.Size = new System.Drawing.Size(1034, 546);

                panelWebBrowserContainer.Location = new Point(214, 0);
                panelLock.Size = new System.Drawing.Size(1034, 546);

                materialList.Location = new Point(3, 4);
                materialList.Size = new Size(208, 82);

                deckMemberList.Location = new Point(3, 92);
                deckMemberList.DetailWndOrigin = DeckMemberList.DetailWindowAlignmentOrigin.Top;
                missionList.Location = new Point(3, 233);

                groupDock.Location = new Point(3, 355);
                groupDock.Size = new Size(208, 127);
                dockBuild.Size = new Size(194, 82);
                dockRepair.Size = new Size(194, 82);
                lbQuest.Size = new Size(194, 88);

                _bVerticalDesign = false;
            }
            else
            {
                //縦デザイン
                this.Size = new System.Drawing.Size(812, 662);

                panelWebBrowserContainer.Location = new Point(0, 0);
                panelLock.Size = new System.Drawing.Size(812, 662);

                materialList.Location = new Point(0, 480);
                materialList.Size = new Size(197, 82);

                deckMemberList.Location = new Point(197, 480);
                deckMemberList.DetailWndOrigin = DeckMemberList.DetailWindowAlignmentOrigin.Bottom;
                missionList.Location = new Point(405, 480);

                groupDock.Location = new Point(613, 480);
                groupDock.Size = new Size(187, 127);
                dockBuild.Size = new Size(173, 82);
                dockRepair.Size = new Size(173, 82);
                lbQuest.Size = new Size(173, 88);

                _bVerticalDesign = true;
            }
            Invalidate();

        }

        private void sessionInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (var dlg = new FormSessionInfo())
            {
                dlg.ServerHost = _serverHost;
                dlg.APIToken = _apiToken;
                dlg.LatestSession = _latestSession;
                dlg.FirstSession = _firstSesson;
                dlg.ShowDialog();
            }

        }

        private void showJsonLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_logWnd == null)
                _logWnd = new FormJSONLog(this, _logLastJSON);

            _logWnd.Show();
        }

        private void clearQuestInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _processor.ClearQuest();
            lbQuest.Items.Clear();
            currentQuestList = null;
        }

        #endregion

        #region 出撃インターバルタイマ
        DeckMemberList.DeckStatus deckStat = null;

        /// <summary>
        /// サイドバーの艦隊選択/メンバーが変更されると呼ばれるコールバック
        /// </summary>
        /// <param name="deckStat">艦隊情報</param>

        void UpdateDeckConditionTime(DeckMemberList.DeckStatus deckStat)
        {
            this.deckStat = deckStat;
            timerBattleInterval.Enabled = deckStat.Tired;

            //直ちに更新
            timerBattleInterval_Tick(null, null);
        }

        void timerBattleInterval_Tick(object sender, EventArgs e)
        {
            if (deckStat == null)
                return;

            DateTime dtNow = DateTime.Now;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0})", deckStat.FleetNo);

            //艦隊メンバーが存在するとき
            if (deckStat.MemberCount > 0)
            {
                sb.AppendFormat("速度:{0} 合計Lv:{1}", deckStat.SpeedTypeString, deckStat.LevelSum);

                if (deckStat.AirSuperiority > 0)
                    sb.AppendFormat(" 制空値:{0}", deckStat.AirSuperiority);

                //索敵値表示
                sb.AppendFormat(" 索敵値:{0}(合計{1})", deckStat.ModifiedSearch, deckStat.TotalSearch);

                //疲れてる
                TimeSpan diff = deckStat.RecoverTime - dtNow;
                if (Math.Floor(diff.TotalSeconds) > 0)
                {
                    sb.AppendFormat(" 回復まで{0}", FormatTimeSpan(diff));
                }

                //戦闘中もcond値は上下するが、下がってもタイマは起動しない
                if (!_processor.DuringCombat)
                    _timerRPC.UpdateConditionTimer(deckStat);

            }

            toolStripTimeFromLastBattle.Text = sb.ToString();

        }

        /// <summary>
        /// TimeSpanをフォーマットする
        /// </summary>
        /// <param name="ts">TimeSpan</param>
        /// <returns>フォーマットされた文字列</returns>
        string FormatTimeSpan(TimeSpan ts)
        {
            int d = ts.Days;
            int h = ts.Hours;
            int m = ts.Minutes;
            int s = ts.Seconds;

            StringBuilder sb = new StringBuilder();

            if (d > 0)
                sb.AppendFormat("{0}日",d);
            if (h > 0)
                sb.AppendFormat("{0}時間",h);
            if (m > 0)
                sb.AppendFormat("{0}分",m);
            if (s > 0)
                sb.AppendFormat("{0}秒",s);

            return sb.ToString();
        }
        #endregion

        /// <summary>
        /// 必要ならば進撃を中断させる。
        /// </summary>
        /// <param name="oSession"></param>
        void CheckShipStatus(HTTProxy.SessionInfo info)
        {
            if (_statusManager.CheckFleetCondition())
                return;

            //処理失敗のレスポンスJSONを作って、サーバにリクエストを飛ばさずflashへ食わせる
            string ret_json = "svdata={\"api_result\":0,\"api_result_msg\":\"aborted\"}";
            info.Response.ContentString = ret_json;
            info.BypassServerRequest = true;
            UpdateStatus("進撃を中止しました");
        }

        /// <summary>
        /// タイマのパスを取得
        /// </summary>
        /// <returns></returns>
        string GetTimerPath()
        {
            Assembly myAssembly = Assembly.GetEntryAssembly();
            string exeDir = System.IO.Path.GetDirectoryName(myAssembly.Location);
            return System.IO.Path.Combine(exeDir, "KCBTimer.exe");
        }

        #region 夜戦突入前判定
        public void BeginWaitForNightBattle(int estimatedTickCount,BattleResult.Result result)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)(() => _beginWaitForNightBattle(estimatedTickCount,result)));
            else
                _beginWaitForNightBattle(estimatedTickCount, result);
        }

        public void EndWaitForNightBattle()
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)(() => _endWaitForNightBattle()));
            else
                _endWaitForNightBattle();
        }

        BattleResult.Result _battleResult = null;

        void _beginWaitForNightBattle(int estimatedTickCount, BattleResult.Result result)
        {
            if (Properties.Settings.Default.ShowEnemyBattleStatus && estimatedTickCount > 0)
            {
                _battleResult = result;
                timerWaitForNightBattle.Interval = estimatedTickCount * 1000;
                timerWaitForNightBattle.Enabled = true;
                Debug.WriteLine(string.Format("夜戦入り検出を開始。{0}秒後に起動",
                    estimatedTickCount));
            }
            else
            {
                Debug.WriteLine("夜戦入り検出は開始してません。");
                timerWaitForNightBattle.Enabled = false;
            }

        }

        void _endWaitForNightBattle()
        {
            enemyFleetList.Visible = false;

            Debug.WriteLine("夜戦入り検出を終了。");

            //何らかの理由で離脱判定が出なかった場合でもタイマを止める。
            timerWaitForNightBattle.Enabled = false;
            _battleResult = null;
        }

        private void timerWaitForNightBattle_Tick(object sender, EventArgs e)
        {
            var img1 = webBrowser1.GetScreenShot();

            //「離脱判定」の文字部分座標エリア
            var rect = new Rectangle(39, 30, 87, 22);

            /* 
             * LockBitsが返すIntPtr(BitmapData.Scan0)の示すメモリはLockする領域によらず
             * 変わらない様子。なのでBitmap.Cloneで領域を切り抜いてから全体をlockする。
             * 一部分のみlockした場合は自力でScanLine辿ってlocked領域のみアクセスするように
             * しないと例外が飛ぶ。
             */

            var img = img1.Clone(rect, img1.PixelFormat);
            var dat = img.LockBits(new Rectangle(0, 0, rect.Width, rect.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, img.PixelFormat);

            // ビットイメージのメモリ内容をbyte配列へ転送する
            int bsize = dat.Stride * img.Height;
            byte[] byteStream = new byte[bsize];
            System.Runtime.InteropServices.Marshal.Copy(dat.Scan0, byteStream, 0, bsize);
            img.UnlockBits(dat);

            //MD5チェックサムを算出
            var md5prv = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5bin = md5prv.ComputeHash(byteStream);

            StringBuilder hashedText = new StringBuilder();
            for (int i = 0; i < md5bin.Length; i++)
            {
                hashedText.AppendFormat("{0:X2}", md5bin[i]);
            }
            Debug.WriteLine("MD5hash:" + hashedText.ToString());

            //離脱判定が表示されているかどうかチェック
            if (hashedText.ToString() == Properties.Settings.Default.WithdrawalKey)
            {
                //離脱判定が表示されていたので、ステートを更新してタイマを終了する。
                timerWaitForNightBattle.Enabled = false;
                if (_battleResult != null)
                {
                    _processor._memberShip.ApplyBattleResult(_battleResult);
                    UpdateShipList(_processor._memberShip.ShipList);
                    UpdateDeckMemberList(_processor._memberShip, _processor._memberDeck.DeckList);
                    var st = _battleResult.BattleState;
                    Debug.WriteLine("戦闘結果予測" + st.ToString());

                    enemyFleetList.Visible = true;
                    enemyFleetList.Fleet = new EnemyFleetList.FleetInfo(_battleResult.Enemy);
                    enemyFleetList.BattleStatus = BattleResult.Result.BattleResultStateString(st);

                    if (_battleResult.Practice)
                        _timerRPC.RPCFinishBattle("昼戦演習");
                    else
                        _timerRPC.RPCFinishBattle("昼戦");
                }
                else
                {
                    // 連合艦隊の戦闘は解析してないから_battleResultはnullになってる
                    _timerRPC.RPCFinishBattle("昼戦");
                }



                Debug.WriteLine("離脱判定を検出したのでUIを更新して終了");
                return;
            }

            // 検出できなかったので、1秒ごとにチェックする
            timerWaitForNightBattle.Interval = 1000;
            Debug.WriteLine("離脱判定を検出できず。1秒ごとに再チェックしに行きます。");
        }
        #endregion 夜戦突入前判定

    }
}
