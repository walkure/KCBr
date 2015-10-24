namespace KCB2
{
    partial class FormPreference
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numHPThreshold = new System.Windows.Forms.NumericUpDown();
            this.cbDetailStatus = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbConfPath = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbBrowserVersion = new System.Windows.Forms.ComboBox();
            this.cbGPURendering = new System.Windows.Forms.CheckBox();
            this.lblIEVer = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSuppressBrowserDialogs = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbSupressSuceessWriteLog = new System.Windows.Forms.CheckBox();
            this.panelGSpread = new System.Windows.Forms.Panel();
            this.tbGoogleAuthorizeToken = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tbGoogleAuthorizeURL = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btGoogleAuthorize = new System.Windows.Forms.Button();
            this.cbLogStoreType = new System.Windows.Forms.ComboBox();
            this.panelLocalStorage = new System.Windows.Forms.Panel();
            this.tbLogDir = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btBrowseLogDir = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btBrowseImageStore = new System.Windows.Forms.Button();
            this.tbImageStore = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbSyncronizeTimerProcess = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbShowEnemyStatus = new System.Windows.Forms.CheckBox();
            this.cbHideDrop = new System.Windows.Forms.CheckBox();
            this.cbMuteOnMinimize = new System.Windows.Forms.CheckBox();
            this.cbActivateDevMenu = new System.Windows.Forms.CheckBox();
            this.cbUseMasterDataView = new System.Windows.Forms.CheckBox();
            this.btTimerConf = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.numCondCeil = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.clbShipDetail = new System.Windows.Forms.CheckedListBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbUseUpstreamProxy = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.numUpstreamProxyPort = new System.Windows.Forms.NumericUpDown();
            this.tbUpstreamProxyHost = new System.Windows.Forms.TextBox();
            this.numProxyPort = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numHPThreshold)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panelGSpread.SuspendLayout();
            this.panelLocalStorage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCondCeil)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpstreamProxyPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(496, 420);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "適用";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(642, 420);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "大破判定するHP閾値";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numHPThreshold
            // 
            this.numHPThreshold.Location = new System.Drawing.Point(12, 33);
            this.numHPThreshold.Name = "numHPThreshold";
            this.numHPThreshold.Size = new System.Drawing.Size(53, 19);
            this.numHPThreshold.TabIndex = 1;
            this.numHPThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numHPThreshold.Value = new decimal(new int[] {
            26,
            0,
            0,
            0});
            // 
            // cbDetailStatus
            // 
            this.cbDetailStatus.AutoSize = true;
            this.cbDetailStatus.Location = new System.Drawing.Point(11, 40);
            this.cbDetailStatus.Name = "cbDetailStatus";
            this.cbDetailStatus.Size = new System.Drawing.Size(189, 16);
            this.cbDetailStatus.TabIndex = 1;
            this.cbDetailStatus.Text = "詳細な状況をステータスバーに表示";
            this.cbDetailStatus.UseVisualStyleBackColor = true;
            this.cbDetailStatus.CheckedChanged += new System.EventHandler(this.cbDetailStatus_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "設定保存先";
            // 
            // tbConfPath
            // 
            this.tbConfPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbConfPath.Location = new System.Drawing.Point(79, 117);
            this.tbConfPath.Name = "tbConfPath";
            this.tbConfPath.ReadOnly = true;
            this.tbConfPath.Size = new System.Drawing.Size(271, 19);
            this.tbConfPath.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 58);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(135, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "※大破閾値0%で判定停止";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(249, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "エミュレートするInternet Explorer(再起動後に反映)";
            // 
            // cbBrowserVersion
            // 
            this.cbBrowserVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBrowserVersion.FormattingEnabled = true;
            this.cbBrowserVersion.Items.AddRange(new object[] {
            "Internet Explorer 7 (標準)",
            "Internet Explorer 8 (標準)",
            "Internet Explorer 8 (強制)",
            "Internet Explorer 9 (標準)",
            "Internet Explorer 9 (強制)",
            "Internet Explorer 10 (標準)",
            "Internet Explorer 10 (強制)",
            "Internet Explorer 11 (標準)",
            "Internet Explorer 11 (強制)"});
            this.cbBrowserVersion.Location = new System.Drawing.Point(11, 30);
            this.cbBrowserVersion.Name = "cbBrowserVersion";
            this.cbBrowserVersion.Size = new System.Drawing.Size(244, 20);
            this.cbBrowserVersion.TabIndex = 1;
            // 
            // cbGPURendering
            // 
            this.cbGPURendering.AutoSize = true;
            this.cbGPURendering.Location = new System.Drawing.Point(17, 68);
            this.cbGPURendering.Name = "cbGPURendering";
            this.cbGPURendering.Size = new System.Drawing.Size(208, 16);
            this.cbGPURendering.TabIndex = 3;
            this.cbGPURendering.Text = "GPU描画支援有効(再起動後に反映)";
            this.cbGPURendering.UseVisualStyleBackColor = true;
            // 
            // lblIEVer
            // 
            this.lblIEVer.AutoSize = true;
            this.lblIEVer.Location = new System.Drawing.Point(6, 53);
            this.lblIEVer.Name = "lblIEVer";
            this.lblIEVer.Size = new System.Drawing.Size(182, 12);
            this.lblIEVer.TabIndex = 2;
            this.lblIEVer.Text = "Internet Explorer Version Message";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSuppressBrowserDialogs);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lblIEVer);
            this.groupBox1.Controls.Add(this.cbGPURendering);
            this.groupBox1.Controls.Add(this.cbBrowserVersion);
            this.groupBox1.Location = new System.Drawing.Point(10, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 112);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ブラウザ設定";
            // 
            // cbSuppressBrowserDialogs
            // 
            this.cbSuppressBrowserDialogs.AutoSize = true;
            this.cbSuppressBrowserDialogs.Location = new System.Drawing.Point(17, 90);
            this.cbSuppressBrowserDialogs.Name = "cbSuppressBrowserDialogs";
            this.cbSuppressBrowserDialogs.Size = new System.Drawing.Size(126, 16);
            this.cbSuppressBrowserDialogs.TabIndex = 4;
            this.cbSuppressBrowserDialogs.Text = "ダイアログ表示を抑止";
            this.cbSuppressBrowserDialogs.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbSupressSuceessWriteLog);
            this.groupBox3.Controls.Add(this.panelGSpread);
            this.groupBox3.Controls.Add(this.cbLogStoreType);
            this.groupBox3.Controls.Add(this.panelLocalStorage);
            this.groupBox3.Location = new System.Drawing.Point(10, 300);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(329, 138);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "記録";
            // 
            // cbSupressSuceessWriteLog
            // 
            this.cbSupressSuceessWriteLog.AutoSize = true;
            this.cbSupressSuceessWriteLog.Location = new System.Drawing.Point(10, 116);
            this.cbSupressSuceessWriteLog.Name = "cbSupressSuceessWriteLog";
            this.cbSupressSuceessWriteLog.Size = new System.Drawing.Size(172, 16);
            this.cbSupressSuceessWriteLog.TabIndex = 13;
            this.cbSupressSuceessWriteLog.Text = "ログ書き込み成功を報告しない";
            this.cbSupressSuceessWriteLog.UseVisualStyleBackColor = true;
            // 
            // panelGSpread
            // 
            this.panelGSpread.Controls.Add(this.tbGoogleAuthorizeToken);
            this.panelGSpread.Controls.Add(this.label16);
            this.panelGSpread.Controls.Add(this.tbGoogleAuthorizeURL);
            this.panelGSpread.Controls.Add(this.label12);
            this.panelGSpread.Controls.Add(this.btGoogleAuthorize);
            this.panelGSpread.Location = new System.Drawing.Point(7, 44);
            this.panelGSpread.Name = "panelGSpread";
            this.panelGSpread.Size = new System.Drawing.Size(316, 63);
            this.panelGSpread.TabIndex = 15;
            this.panelGSpread.Visible = false;
            // 
            // tbGoogleAuthorizeToken
            // 
            this.tbGoogleAuthorizeToken.Location = new System.Drawing.Point(72, 32);
            this.tbGoogleAuthorizeToken.Name = "tbGoogleAuthorizeToken";
            this.tbGoogleAuthorizeToken.Size = new System.Drawing.Size(149, 19);
            this.tbGoogleAuthorizeToken.TabIndex = 4;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 35);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 12);
            this.label16.TabIndex = 3;
            this.label16.Text = "認証トークン";
            // 
            // tbGoogleAuthorizeURL
            // 
            this.tbGoogleAuthorizeURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbGoogleAuthorizeURL.Location = new System.Drawing.Point(59, 3);
            this.tbGoogleAuthorizeURL.Name = "tbGoogleAuthorizeURL";
            this.tbGoogleAuthorizeURL.ReadOnly = true;
            this.tbGoogleAuthorizeURL.Size = new System.Drawing.Size(254, 19);
            this.tbGoogleAuthorizeURL.TabIndex = 2;
            this.tbGoogleAuthorizeURL.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbGoogleAuthorizeURL_MouseDoubleClick);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 5);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 12);
            this.label12.TabIndex = 1;
            this.label12.Text = "認証URL";
            // 
            // btGoogleAuthorize
            // 
            this.btGoogleAuthorize.Location = new System.Drawing.Point(227, 30);
            this.btGoogleAuthorize.Name = "btGoogleAuthorize";
            this.btGoogleAuthorize.Size = new System.Drawing.Size(86, 23);
            this.btGoogleAuthorize.TabIndex = 0;
            this.btGoogleAuthorize.Text = "認証";
            this.btGoogleAuthorize.UseVisualStyleBackColor = true;
            this.btGoogleAuthorize.Click += new System.EventHandler(this.btGoogleAuthorize_Click);
            // 
            // cbLogStoreType
            // 
            this.cbLogStoreType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLogStoreType.FormattingEnabled = true;
            this.cbLogStoreType.Items.AddRange(new object[] {
            "ディスク",
            "Google Spreadsheet"});
            this.cbLogStoreType.Location = new System.Drawing.Point(12, 18);
            this.cbLogStoreType.Name = "cbLogStoreType";
            this.cbLogStoreType.Size = new System.Drawing.Size(247, 20);
            this.cbLogStoreType.TabIndex = 14;
            this.cbLogStoreType.SelectedIndexChanged += new System.EventHandler(this.cbLogStoreType_SelectedIndexChanged);
            // 
            // panelLocalStorage
            // 
            this.panelLocalStorage.Controls.Add(this.tbLogDir);
            this.panelLocalStorage.Controls.Add(this.label13);
            this.panelLocalStorage.Controls.Add(this.btBrowseLogDir);
            this.panelLocalStorage.Location = new System.Drawing.Point(7, 113);
            this.panelLocalStorage.Name = "panelLocalStorage";
            this.panelLocalStorage.Size = new System.Drawing.Size(316, 63);
            this.panelLocalStorage.TabIndex = 13;
            this.panelLocalStorage.Visible = false;
            // 
            // tbLogDir
            // 
            this.tbLogDir.Location = new System.Drawing.Point(5, 22);
            this.tbLogDir.Name = "tbLogDir";
            this.tbLogDir.Size = new System.Drawing.Size(252, 19);
            this.tbLogDir.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 7);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 12);
            this.label13.TabIndex = 10;
            this.label13.Text = "記録保存先ディレクトリ";
            // 
            // btBrowseLogDir
            // 
            this.btBrowseLogDir.Location = new System.Drawing.Point(269, 22);
            this.btBrowseLogDir.Name = "btBrowseLogDir";
            this.btBrowseLogDir.Size = new System.Drawing.Size(26, 18);
            this.btBrowseLogDir.TabIndex = 12;
            this.btBrowseLogDir.Text = "...";
            this.btBrowseLogDir.UseVisualStyleBackColor = true;
            this.btBrowseLogDir.Click += new System.EventHandler(this.btBrowseLogDir_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.numHPThreshold);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Location = new System.Drawing.Point(522, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(195, 84);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "大破進撃抑止";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btBrowseImageStore);
            this.groupBox5.Controls.Add(this.tbImageStore);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Location = new System.Drawing.Point(350, 194);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(368, 63);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "スクリーンショット";
            // 
            // btBrowseImageStore
            // 
            this.btBrowseImageStore.Location = new System.Drawing.Point(290, 33);
            this.btBrowseImageStore.Name = "btBrowseImageStore";
            this.btBrowseImageStore.Size = new System.Drawing.Size(23, 19);
            this.btBrowseImageStore.TabIndex = 2;
            this.btBrowseImageStore.Text = "...";
            this.btBrowseImageStore.UseVisualStyleBackColor = true;
            this.btBrowseImageStore.Click += new System.EventHandler(this.btBrowseImageStore_Click);
            // 
            // tbImageStore
            // 
            this.tbImageStore.Location = new System.Drawing.Point(11, 34);
            this.tbImageStore.Name = "tbImageStore";
            this.tbImageStore.Size = new System.Drawing.Size(273, 19);
            this.tbImageStore.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(212, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "保存先 (空白でクリップボードへの転送のみ)";
            // 
            // cbSyncronizeTimerProcess
            // 
            this.cbSyncronizeTimerProcess.AutoSize = true;
            this.cbSyncronizeTimerProcess.Location = new System.Drawing.Point(11, 18);
            this.cbSyncronizeTimerProcess.Name = "cbSyncronizeTimerProcess";
            this.cbSyncronizeTimerProcess.Size = new System.Drawing.Size(213, 16);
            this.cbSyncronizeTimerProcess.TabIndex = 0;
            this.cbSyncronizeTimerProcess.Text = "出渠通知タイマを同時に起動/終了する";
            this.cbSyncronizeTimerProcess.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cbShowEnemyStatus);
            this.groupBox6.Controls.Add(this.cbHideDrop);
            this.groupBox6.Controls.Add(this.cbMuteOnMinimize);
            this.groupBox6.Controls.Add(this.cbActivateDevMenu);
            this.groupBox6.Controls.Add(this.cbUseMasterDataView);
            this.groupBox6.Controls.Add(this.btTimerConf);
            this.groupBox6.Controls.Add(this.cbSyncronizeTimerProcess);
            this.groupBox6.Controls.Add(this.cbDetailStatus);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.tbConfPath);
            this.groupBox6.Location = new System.Drawing.Point(351, 270);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(367, 141);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "その他";
            // 
            // cbShowEnemyStatus
            // 
            this.cbShowEnemyStatus.AutoSize = true;
            this.cbShowEnemyStatus.Location = new System.Drawing.Point(11, 60);
            this.cbShowEnemyStatus.Name = "cbShowEnemyStatus";
            this.cbShowEnemyStatus.Size = new System.Drawing.Size(174, 16);
            this.cbShowEnemyStatus.TabIndex = 2;
            this.cbShowEnemyStatus.Text = "離脱判定時に敵艦状態を表示";
            this.cbShowEnemyStatus.UseVisualStyleBackColor = true;
            // 
            // cbHideDrop
            // 
            this.cbHideDrop.AutoSize = true;
            this.cbHideDrop.Location = new System.Drawing.Point(234, 40);
            this.cbHideDrop.Name = "cbHideDrop";
            this.cbHideDrop.Size = new System.Drawing.Size(127, 16);
            this.cbHideDrop.TabIndex = 8;
            this.cbHideDrop.Text = "艦娘ドロップを非表示";
            this.cbHideDrop.UseVisualStyleBackColor = true;
            // 
            // cbMuteOnMinimize
            // 
            this.cbMuteOnMinimize.AutoSize = true;
            this.cbMuteOnMinimize.Location = new System.Drawing.Point(234, 60);
            this.cbMuteOnMinimize.Name = "cbMuteOnMinimize";
            this.cbMuteOnMinimize.Size = new System.Drawing.Size(103, 16);
            this.cbMuteOnMinimize.TabIndex = 9;
            this.cbMuteOnMinimize.Text = "最小化でミュート";
            this.cbMuteOnMinimize.UseVisualStyleBackColor = true;
            // 
            // cbActivateDevMenu
            // 
            this.cbActivateDevMenu.AutoSize = true;
            this.cbActivateDevMenu.Location = new System.Drawing.Point(11, 97);
            this.cbActivateDevMenu.Name = "cbActivateDevMenu";
            this.cbActivateDevMenu.Size = new System.Drawing.Size(220, 16);
            this.cbActivateDevMenu.TabIndex = 4;
            this.cbActivateDevMenu.Text = "開発者メニューを有効化(再起動後反映)";
            this.cbActivateDevMenu.UseVisualStyleBackColor = true;
            // 
            // cbUseMasterDataView
            // 
            this.cbUseMasterDataView.AutoSize = true;
            this.cbUseMasterDataView.Location = new System.Drawing.Point(11, 79);
            this.cbUseMasterDataView.Name = "cbUseMasterDataView";
            this.cbUseMasterDataView.Size = new System.Drawing.Size(227, 16);
            this.cbUseMasterDataView.TabIndex = 3;
            this.cbUseMasterDataView.Text = "マスタデータ表示を有効化(再起動後反映)";
            this.cbUseMasterDataView.UseVisualStyleBackColor = true;
            // 
            // btTimerConf
            // 
            this.btTimerConf.Location = new System.Drawing.Point(246, 14);
            this.btTimerConf.Name = "btTimerConf";
            this.btTimerConf.Size = new System.Drawing.Size(75, 23);
            this.btTimerConf.TabIndex = 7;
            this.btTimerConf.Text = "タイマ設定";
            this.btTimerConf.UseVisualStyleBackColor = true;
            this.btTimerConf.Click += new System.EventHandler(this.btTimerConf_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label18);
            this.groupBox7.Controls.Add(this.numCondCeil);
            this.groupBox7.Controls.Add(this.label17);
            this.groupBox7.Location = new System.Drawing.Point(522, 102);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(196, 84);
            this.groupBox7.TabIndex = 7;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "疲労回復インターバル表示";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(73, 50);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(117, 12);
            this.label18.TabIndex = 2;
            this.label18.Text = "※基準値0で表示停止";
            // 
            // numCondCeil
            // 
            this.numCondCeil.Location = new System.Drawing.Point(11, 48);
            this.numCondCeil.Maximum = new decimal(new int[] {
            49,
            0,
            0,
            0});
            this.numCondCeil.Name = "numCondCeil";
            this.numCondCeil.Size = new System.Drawing.Size(50, 19);
            this.numCondCeil.TabIndex = 1;
            this.numCondCeil.Value = new decimal(new int[] {
            49,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 20);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(169, 12);
            this.label17.TabIndex = 0;
            this.label17.Text = "疲労が取れたとみなすコンディション";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.clbShipDetail);
            this.groupBox8.Location = new System.Drawing.Point(350, 12);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(162, 174);
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "艦船詳細表示";
            // 
            // clbShipDetail
            // 
            this.clbShipDetail.AllowDrop = true;
            this.clbShipDetail.FormattingEnabled = true;
            this.clbShipDetail.Location = new System.Drawing.Point(15, 18);
            this.clbShipDetail.Name = "clbShipDetail";
            this.clbShipDetail.Size = new System.Drawing.Size(133, 144);
            this.clbShipDetail.TabIndex = 0;
            this.clbShipDetail.DragDrop += new System.Windows.Forms.DragEventHandler(this.clbShipDetail_DragDrop);
            this.clbShipDetail.DragEnter += new System.Windows.Forms.DragEventHandler(this.clbShipDetail_DragEnter);
            this.clbShipDetail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.clbShipDetail_MouseDown);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label1);
            this.groupBox9.Controls.Add(this.cbUseUpstreamProxy);
            this.groupBox9.Controls.Add(this.label20);
            this.groupBox9.Controls.Add(this.groupBox10);
            this.groupBox9.Controls.Add(this.numProxyPort);
            this.groupBox9.Controls.Add(this.label4);
            this.groupBox9.Location = new System.Drawing.Point(10, 12);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(329, 157);
            this.groupBox9.TabIndex = 2;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "ネットワーク設定";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "設定は再起動後に反映されます";
            // 
            // cbUseUpstreamProxy
            // 
            this.cbUseUpstreamProxy.AutoSize = true;
            this.cbUseUpstreamProxy.Location = new System.Drawing.Point(18, 84);
            this.cbUseUpstreamProxy.Name = "cbUseUpstreamProxy";
            this.cbUseUpstreamProxy.Size = new System.Drawing.Size(179, 16);
            this.cbUseUpstreamProxy.TabIndex = 4;
            this.cbUseUpstreamProxy.Text = "外部への接続時にプロキシを使う";
            this.cbUseUpstreamProxy.UseVisualStyleBackColor = true;
            this.cbUseUpstreamProxy.CheckedChanged += new System.EventHandler(this.cbUseUpstreamProxy_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(35, 44);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(249, 12);
            this.label20.TabIndex = 2;
            this.label20.Text = "プロキシポートをnetsh等でアクセス許可してください。";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.numUpstreamProxyPort);
            this.groupBox10.Controls.Add(this.tbUpstreamProxyHost);
            this.groupBox10.Location = new System.Drawing.Point(39, 106);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(281, 40);
            this.groupBox10.TabIndex = 5;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "上流プロキシ設定";
            // 
            // numUpstreamProxyPort
            // 
            this.numUpstreamProxyPort.Location = new System.Drawing.Point(211, 13);
            this.numUpstreamProxyPort.Maximum = new decimal(new int[] {
            655335,
            0,
            0,
            0});
            this.numUpstreamProxyPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpstreamProxyPort.Name = "numUpstreamProxyPort";
            this.numUpstreamProxyPort.Size = new System.Drawing.Size(63, 19);
            this.numUpstreamProxyPort.TabIndex = 1;
            this.numUpstreamProxyPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tbUpstreamProxyHost
            // 
            this.tbUpstreamProxyHost.Location = new System.Drawing.Point(13, 13);
            this.tbUpstreamProxyHost.Name = "tbUpstreamProxyHost";
            this.tbUpstreamProxyHost.Size = new System.Drawing.Size(192, 19);
            this.tbUpstreamProxyHost.TabIndex = 0;
            // 
            // numProxyPort
            // 
            this.numProxyPort.Location = new System.Drawing.Point(145, 22);
            this.numProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numProxyPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numProxyPort.Name = "numProxyPort";
            this.numProxyPort.Size = new System.Drawing.Size(107, 19);
            this.numProxyPort.TabIndex = 1;
            this.numProxyPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "傍受に使うプロキシポート";
            // 
            // FormPreference
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 447);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPreference";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "設定";
            this.Load += new System.EventHandler(this.FormPreference_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numHPThreshold)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panelGSpread.ResumeLayout(false);
            this.panelGSpread.PerformLayout();
            this.panelLocalStorage.ResumeLayout(false);
            this.panelLocalStorage.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCondCeil)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpstreamProxyPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numProxyPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numHPThreshold;
        private System.Windows.Forms.CheckBox cbDetailStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbConfPath;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbBrowserVersion;
        private System.Windows.Forms.CheckBox cbGPURendering;
        private System.Windows.Forms.Label lblIEVer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btBrowseImageStore;
        private System.Windows.Forms.TextBox tbImageStore;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panelLocalStorage;
        private System.Windows.Forms.TextBox tbLogDir;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btBrowseLogDir;
        private System.Windows.Forms.CheckBox cbSyncronizeTimerProcess;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btTimerConf;
        private System.Windows.Forms.Panel panelGSpread;
        private System.Windows.Forms.TextBox tbGoogleAuthorizeToken;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbGoogleAuthorizeURL;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btGoogleAuthorize;
        private System.Windows.Forms.ComboBox cbLogStoreType;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown numCondCeil;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckedListBox clbShipDetail;
        private System.Windows.Forms.CheckBox cbSupressSuceessWriteLog;
        private System.Windows.Forms.CheckBox cbUseMasterDataView;
        private System.Windows.Forms.CheckBox cbActivateDevMenu;
        private System.Windows.Forms.CheckBox cbMuteOnMinimize;
        private System.Windows.Forms.CheckBox cbHideDrop;
        private System.Windows.Forms.CheckBox cbShowEnemyStatus;
        private System.Windows.Forms.CheckBox cbSuppressBrowserDialogs;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.CheckBox cbUseUpstreamProxy;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.NumericUpDown numUpstreamProxyPort;
        private System.Windows.Forms.TextBox tbUpstreamProxyHost;
        private System.Windows.Forms.NumericUpDown numProxyPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
    }
}