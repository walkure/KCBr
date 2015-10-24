namespace KCB2
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.timerWaitPageLoad = new System.Windows.Forms.Timer(this.components);
            this.panelWebBrowserContainer = new System.Windows.Forms.Panel();
            this.enemyFleetList = new KCB2.EnemyFleetList();
            this.webBrowser1 = new KCB.WebBrowserEx();
            this.panelLock = new System.Windows.Forms.Panel();
            this.btUnlock = new System.Windows.Forms.Button();
            this.tbUnlockPassword = new System.Windows.Forms.TextBox();
            this.lblLockMessage = new System.Windows.Forms.Label();
            this.groupDock = new System.Windows.Forms.GroupBox();
            this.lbQuest = new KCB2.ListBoxEx();
            this.rdQuest = new System.Windows.Forms.RadioButton();
            this.dockRepair = new KCB2.DockList();
            this.dockBuild = new KCB2.DockList();
            this.rdRepair = new System.Windows.Forms.RadioButton();
            this.rdBuild = new System.Windows.Forms.RadioButton();
            this.showQuestButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.shipListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ItemListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slotItemDetailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slotItemSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenShotButton = new System.Windows.Forms.ToolStripButton();
            this.volumeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripTimeFromLastBattle = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.reloadBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustFlashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.switchViewModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sendTimerInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sessionInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showJsonLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerBattleInterval = new System.Windows.Forms.Timer(this.components);
            this.imageListSlotItemType = new System.Windows.Forms.ImageList(this.components);
            this.timerWaitForNightBattle = new System.Windows.Forms.Timer(this.components);
            this.materialList = new KCB2.MaterialList();
            this.deckMemberList = new KCB2.DeckMemberList();
            this.missionList = new KCB2.MissionList();
            this.panelWebBrowserContainer.SuspendLayout();
            this.panelLock.SuspendLayout();
            this.groupDock.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerWaitPageLoad
            // 
            this.timerWaitPageLoad.Interval = 1500;
            // 
            // panelWebBrowserContainer
            // 
            this.panelWebBrowserContainer.Controls.Add(this.enemyFleetList);
            this.panelWebBrowserContainer.Controls.Add(this.webBrowser1);
            this.panelWebBrowserContainer.Location = new System.Drawing.Point(214, 0);
            this.panelWebBrowserContainer.Name = "panelWebBrowserContainer";
            this.panelWebBrowserContainer.Size = new System.Drawing.Size(800, 480);
            this.panelWebBrowserContainer.TabIndex = 2;
            // 
            // enemyFleetList
            // 
            this.enemyFleetList.Location = new System.Drawing.Point(505, 317);
            this.enemyFleetList.Name = "enemyFleetList";
            this.enemyFleetList.Size = new System.Drawing.Size(288, 155);
            this.enemyFleetList.TabIndex = 1;
            this.enemyFleetList.Visible = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(800, 480);
            this.webBrowser1.TabIndex = 0;
            // 
            // panelLock
            // 
            this.panelLock.Controls.Add(this.btUnlock);
            this.panelLock.Controls.Add(this.tbUnlockPassword);
            this.panelLock.Controls.Add(this.lblLockMessage);
            this.panelLock.Location = new System.Drawing.Point(0, 0);
            this.panelLock.Name = "panelLock";
            this.panelLock.Size = new System.Drawing.Size(1014, 480);
            this.panelLock.TabIndex = 9;
            this.panelLock.Visible = false;
            // 
            // btUnlock
            // 
            this.btUnlock.Location = new System.Drawing.Point(564, 268);
            this.btUnlock.Name = "btUnlock";
            this.btUnlock.Size = new System.Drawing.Size(75, 22);
            this.btUnlock.TabIndex = 9;
            this.btUnlock.Text = "ロック解除";
            this.btUnlock.UseVisualStyleBackColor = true;
            this.btUnlock.Click += new System.EventHandler(this.btUnlock_Click);
            // 
            // tbUnlockPassword
            // 
            this.tbUnlockPassword.Location = new System.Drawing.Point(367, 233);
            this.tbUnlockPassword.Name = "tbUnlockPassword";
            this.tbUnlockPassword.Size = new System.Drawing.Size(272, 19);
            this.tbUnlockPassword.TabIndex = 8;
            this.tbUnlockPassword.UseSystemPasswordChar = true;
            // 
            // lblLockMessage
            // 
            this.lblLockMessage.AutoSize = true;
            this.lblLockMessage.Location = new System.Drawing.Point(365, 206);
            this.lblLockMessage.Name = "lblLockMessage";
            this.lblLockMessage.Size = new System.Drawing.Size(144, 24);
            this.lblLockMessage.TabIndex = 7;
            this.lblLockMessage.Text = "画面をロックしています。\r\nパスワードを入力してください。";
            // 
            // groupDock
            // 
            this.groupDock.Controls.Add(this.lbQuest);
            this.groupDock.Controls.Add(this.rdQuest);
            this.groupDock.Controls.Add(this.dockRepair);
            this.groupDock.Controls.Add(this.dockBuild);
            this.groupDock.Controls.Add(this.rdRepair);
            this.groupDock.Controls.Add(this.rdBuild);
            this.groupDock.Location = new System.Drawing.Point(3, 347);
            this.groupDock.Name = "groupDock";
            this.groupDock.Size = new System.Drawing.Size(208, 127);
            this.groupDock.TabIndex = 3;
            this.groupDock.TabStop = false;
            this.groupDock.Text = "ドック/任務";
            // 
            // lbQuest
            // 
            this.lbQuest.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbQuest.FormattingEnabled = true;
            this.lbQuest.HorizontalScrollbar = true;
            this.lbQuest.ItemHeight = 12;
            this.lbQuest.Location = new System.Drawing.Point(7, 34);
            this.lbQuest.Name = "lbQuest";
            this.lbQuest.Size = new System.Drawing.Size(194, 88);
            this.lbQuest.TabIndex = 7;
            this.lbQuest.Visible = false;
            // 
            // rdQuest
            // 
            this.rdQuest.AutoSize = true;
            this.rdQuest.Location = new System.Drawing.Point(113, 15);
            this.rdQuest.Name = "rdQuest";
            this.rdQuest.Size = new System.Drawing.Size(47, 16);
            this.rdQuest.TabIndex = 2;
            this.rdQuest.TabStop = true;
            this.rdQuest.Text = "任務";
            this.rdQuest.UseVisualStyleBackColor = true;
            this.rdQuest.CheckedChanged += new System.EventHandler(this.rdQuest_CheckedChanged);
            // 
            // dockRepair
            // 
            this.dockRepair.Location = new System.Drawing.Point(7, 36);
            this.dockRepair.Name = "dockRepair";
            this.dockRepair.Size = new System.Drawing.Size(194, 82);
            this.dockRepair.TabIndex = 6;
            // 
            // dockBuild
            // 
            this.dockBuild.Location = new System.Drawing.Point(7, 36);
            this.dockBuild.Name = "dockBuild";
            this.dockBuild.Size = new System.Drawing.Size(194, 82);
            this.dockBuild.TabIndex = 2;
            this.dockBuild.Visible = false;
            // 
            // rdRepair
            // 
            this.rdRepair.AutoSize = true;
            this.rdRepair.Checked = true;
            this.rdRepair.Location = new System.Drawing.Point(62, 15);
            this.rdRepair.Name = "rdRepair";
            this.rdRepair.Size = new System.Drawing.Size(47, 16);
            this.rdRepair.TabIndex = 1;
            this.rdRepair.TabStop = true;
            this.rdRepair.Text = "修復";
            this.rdRepair.UseVisualStyleBackColor = true;
            this.rdRepair.CheckedChanged += new System.EventHandler(this.rdRepair_CheckedChanged);
            // 
            // rdBuild
            // 
            this.rdBuild.AutoSize = true;
            this.rdBuild.Location = new System.Drawing.Point(9, 15);
            this.rdBuild.Name = "rdBuild";
            this.rdBuild.Size = new System.Drawing.Size(47, 16);
            this.rdBuild.TabIndex = 0;
            this.rdBuild.Text = "建造";
            this.rdBuild.UseVisualStyleBackColor = true;
            this.rdBuild.CheckedChanged += new System.EventHandler(this.rdBuild_CheckedChanged);
            // 
            // showQuestButton
            // 
            this.showQuestButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.showQuestButton.Image = ((System.Drawing.Image)(resources.GetObject("showQuestButton.Image")));
            this.showQuestButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showQuestButton.Name = "showQuestButton";
            this.showQuestButton.Size = new System.Drawing.Size(77, 20);
            this.showQuestButton.Text = "[進行中任務]";
            this.showQuestButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.showQuestButton_MouseDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AllowMerge = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton2,
            this.screenShotButton,
            this.volumeButton,
            this.showQuestButton,
            this.toolStripTimeFromLastBattle,
            this.toolStripStatusLabel1,
            this.toolStripDropDownButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 482);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1014, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shipListToolStripMenuItem,
            this.ItemListToolStripMenuItem,
            this.LogToolStripMenuItem,
            this.masterDataToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(42, 20);
            this.toolStripDropDownButton2.Text = "表示";
            // 
            // shipListToolStripMenuItem
            // 
            this.shipListToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.shipListToolStripMenuItem.Name = "shipListToolStripMenuItem";
            this.shipListToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.shipListToolStripMenuItem.Text = "艦娘一覧";
            this.shipListToolStripMenuItem.Click += new System.EventHandler(this.shipListToolStripMenuItem_Click);
            // 
            // ItemListToolStripMenuItem
            // 
            this.ItemListToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ItemListToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.slotItemDetailToolStripMenuItem,
            this.slotItemSummaryToolStripMenuItem});
            this.ItemListToolStripMenuItem.Name = "ItemListToolStripMenuItem";
            this.ItemListToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ItemListToolStripMenuItem.Text = "装備一覧";
            // 
            // slotItemDetailToolStripMenuItem
            // 
            this.slotItemDetailToolStripMenuItem.Name = "slotItemDetailToolStripMenuItem";
            this.slotItemDetailToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.slotItemDetailToolStripMenuItem.Text = "装備個別表示";
            this.slotItemDetailToolStripMenuItem.Click += new System.EventHandler(this.slotItemDetailToolStripMenuItem_Click);
            // 
            // slotItemSummaryToolStripMenuItem
            // 
            this.slotItemSummaryToolStripMenuItem.Name = "slotItemSummaryToolStripMenuItem";
            this.slotItemSummaryToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.slotItemSummaryToolStripMenuItem.Text = "装備をまとめて表示";
            this.slotItemSummaryToolStripMenuItem.Click += new System.EventHandler(this.slotItemSummaryToolStripMenuItem_Click);
            // 
            // LogToolStripMenuItem
            // 
            this.LogToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.LogToolStripMenuItem.Name = "LogToolStripMenuItem";
            this.LogToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.LogToolStripMenuItem.Text = "記録一覧";
            this.LogToolStripMenuItem.Click += new System.EventHandler(this.LogToolStripMenuItem_Click);
            // 
            // masterDataToolStripMenuItem
            // 
            this.masterDataToolStripMenuItem.Name = "masterDataToolStripMenuItem";
            this.masterDataToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.masterDataToolStripMenuItem.Text = "マスタデータ一覧";
            this.masterDataToolStripMenuItem.Visible = false;
            this.masterDataToolStripMenuItem.Click += new System.EventHandler(this.masterDataToolStripMenuItem_Click);
            // 
            // screenShotButton
            // 
            this.screenShotButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.screenShotButton.Image = ((System.Drawing.Image)(resources.GetObject("screenShotButton.Image")));
            this.screenShotButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.screenShotButton.Name = "screenShotButton";
            this.screenShotButton.Size = new System.Drawing.Size(61, 20);
            this.screenShotButton.Text = "[キャプチャ]";
            this.screenShotButton.Click += new System.EventHandler(this.screenShotButton_Click);
            // 
            // volumeButton
            // 
            this.volumeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.volumeButton.Image = ((System.Drawing.Image)(resources.GetObject("volumeButton.Image")));
            this.volumeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.volumeButton.Name = "volumeButton";
            this.volumeButton.Size = new System.Drawing.Size(41, 20);
            this.volumeButton.Text = "[音量]";
            this.volumeButton.Click += new System.EventHandler(this.volumeButton_Click);
            // 
            // toolStripTimeFromLastBattle
            // 
            this.toolStripTimeFromLastBattle.Name = "toolStripTimeFromLastBattle";
            this.toolStripTimeFromLastBattle.Size = new System.Drawing.Size(13, 17);
            this.toolStripTimeFromLastBattle.Text = "  ";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(723, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "レディ";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reloadBrowserToolStripMenuItem,
            this.adjustFlashToolStripMenuItem,
            this.clearCacheToolStripMenuItem,
            this.switchViewModeToolStripMenuItem,
            this.lockUIToolStripMenuItem,
            this.toolStripSeparator1,
            this.sendTimerInfoToolStripMenuItem,
            this.configToolStripMenuItem,
            this.devMenuToolStripMenuItem,
            this.versionToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(42, 20);
            this.toolStripDropDownButton1.Text = "操作";
            // 
            // reloadBrowserToolStripMenuItem
            // 
            this.reloadBrowserToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.reloadBrowserToolStripMenuItem.Name = "reloadBrowserToolStripMenuItem";
            this.reloadBrowserToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.reloadBrowserToolStripMenuItem.Text = "画面を再読み込み";
            this.reloadBrowserToolStripMenuItem.Click += new System.EventHandler(this.reloadBrowserToolStripMenuItem_Click);
            // 
            // adjustFlashToolStripMenuItem
            // 
            this.adjustFlashToolStripMenuItem.Name = "adjustFlashToolStripMenuItem";
            this.adjustFlashToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.adjustFlashToolStripMenuItem.Text = "フラッシュ位置の再設定";
            this.adjustFlashToolStripMenuItem.Click += new System.EventHandler(this.adjustFlashToolStripMenuItem_Click);
            // 
            // clearCacheToolStripMenuItem
            // 
            this.clearCacheToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clearCacheToolStripMenuItem.Name = "clearCacheToolStripMenuItem";
            this.clearCacheToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.clearCacheToolStripMenuItem.Text = "ブラウザキャッシュの削除";
            this.clearCacheToolStripMenuItem.Click += new System.EventHandler(this.clearCacheToolStripMenuItem_Click);
            // 
            // switchViewModeToolStripMenuItem
            // 
            this.switchViewModeToolStripMenuItem.Name = "switchViewModeToolStripMenuItem";
            this.switchViewModeToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.switchViewModeToolStripMenuItem.Text = "縦横切替";
            this.switchViewModeToolStripMenuItem.Click += new System.EventHandler(this.switchViewModeToolStripMenuItem_Click);
            // 
            // lockUIToolStripMenuItem
            // 
            this.lockUIToolStripMenuItem.Name = "lockUIToolStripMenuItem";
            this.lockUIToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.lockUIToolStripMenuItem.Text = "画面をロック";
            this.lockUIToolStripMenuItem.Click += new System.EventHandler(this.lockUIToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // sendTimerInfoToolStripMenuItem
            // 
            this.sendTimerInfoToolStripMenuItem.Name = "sendTimerInfoToolStripMenuItem";
            this.sendTimerInfoToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.sendTimerInfoToolStripMenuItem.Text = "タイマへ情報を送信";
            this.sendTimerInfoToolStripMenuItem.Click += new System.EventHandler(this.sendTimerInfoToolStripMenuItem_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.configToolStripMenuItem.Text = "設定";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // devMenuToolStripMenuItem
            // 
            this.devMenuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sessionInfoToolStripMenuItem,
            this.showJsonLogToolStripMenuItem});
            this.devMenuToolStripMenuItem.Name = "devMenuToolStripMenuItem";
            this.devMenuToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.devMenuToolStripMenuItem.Text = "開発メニュー";
            // 
            // sessionInfoToolStripMenuItem
            // 
            this.sessionInfoToolStripMenuItem.Name = "sessionInfoToolStripMenuItem";
            this.sessionInfoToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.sessionInfoToolStripMenuItem.Text = "セッション情報";
            this.sessionInfoToolStripMenuItem.Click += new System.EventHandler(this.sessionInfoToolStripMenuItem_Click_1);
            // 
            // showJsonLogToolStripMenuItem
            // 
            this.showJsonLogToolStripMenuItem.Name = "showJsonLogToolStripMenuItem";
            this.showJsonLogToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.showJsonLogToolStripMenuItem.Text = "JSONログ表示";
            this.showJsonLogToolStripMenuItem.Click += new System.EventHandler(this.showJsonLogToolStripMenuItem_Click);
            // 
            // versionToolStripMenuItem
            // 
            this.versionToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.versionToolStripMenuItem.Text = "バージョン情報";
            this.versionToolStripMenuItem.Click += new System.EventHandler(this.versionToolStripMenuItem_Click);
            // 
            // timerBattleInterval
            // 
            this.timerBattleInterval.Interval = 1000;
            this.timerBattleInterval.Tick += new System.EventHandler(this.timerBattleInterval_Tick);
            // 
            // imageListSlotItemType
            // 
            this.imageListSlotItemType.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListSlotItemType.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListSlotItemType.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timerWaitForNightBattle
            // 
            this.timerWaitForNightBattle.Tick += new System.EventHandler(this.timerWaitForNightBattle_Tick);
            // 
            // materialList
            // 
            this.materialList.Location = new System.Drawing.Point(3, 4);
            this.materialList.Name = "materialList";
            this.materialList.Size = new System.Drawing.Size(208, 82);
            this.materialList.TabIndex = 6;
            // 
            // deckMemberList
            // 
            this.deckMemberList.Location = new System.Drawing.Point(3, 92);
            this.deckMemberList.Name = "deckMemberList";
            this.deckMemberList.Size = new System.Drawing.Size(208, 131);
            this.deckMemberList.TabIndex = 5;
            this.deckMemberList.UpdateDeckStatus = null;
            // 
            // missionList
            // 
            this.missionList.Location = new System.Drawing.Point(3, 228);
            this.missionList.Name = "missionList";
            this.missionList.Size = new System.Drawing.Size(208, 118);
            this.missionList.TabIndex = 4;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 504);
            this.Controls.Add(this.materialList);
            this.Controls.Add(this.deckMemberList);
            this.Controls.Add(this.missionList);
            this.Controls.Add(this.groupDock);
            this.Controls.Add(this.panelWebBrowserContainer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelLock);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "KCBr2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.panelWebBrowserContainer.ResumeLayout(false);
            this.panelLock.ResumeLayout(false);
            this.panelLock.PerformLayout();
            this.groupDock.ResumeLayout(false);
            this.groupDock.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KCB.WebBrowserEx webBrowser1;
        private System.Windows.Forms.Timer timerWaitPageLoad;
        private System.Windows.Forms.Panel panelWebBrowserContainer;
        private MissionList missionList;
        private DeckMemberList deckMemberList;
        private System.Windows.Forms.GroupBox groupDock;
        private DockList dockBuild;
        private System.Windows.Forms.RadioButton rdRepair;
        private System.Windows.Forms.RadioButton rdBuild;
        private DockList dockRepair;
        private MaterialList materialList;
        private System.Windows.Forms.ToolStripButton showQuestButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem reloadBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem shipListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ItemListToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton screenShotButton;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTimeFromLastBattle;
        private System.Windows.Forms.Timer timerBattleInterval;
        private System.Windows.Forms.ToolStripButton volumeButton;
        private System.Windows.Forms.ToolStripMenuItem sendTimerInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adjustFlashToolStripMenuItem;
        private System.Windows.Forms.Label lblLockMessage;
        private System.Windows.Forms.TextBox tbUnlockPassword;
        private System.Windows.Forms.Panel panelLock;
        private System.Windows.Forms.Button btUnlock;
        private System.Windows.Forms.ToolStripMenuItem lockUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem switchViewModeToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListSlotItemType;
        private System.Windows.Forms.ToolStripMenuItem masterDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem slotItemDetailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem slotItemSummaryToolStripMenuItem;
        private System.Windows.Forms.RadioButton rdQuest;
        private ListBoxEx lbQuest;
        private System.Windows.Forms.ToolStripMenuItem devMenuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sessionInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showJsonLogToolStripMenuItem;
        private System.Windows.Forms.Timer timerWaitForNightBattle;
        private EnemyFleetList enemyFleetList;
    }
}

