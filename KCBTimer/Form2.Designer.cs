namespace KCBTimer
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDockSound = new System.Windows.Forms.TextBox();
            this.tbMissionSound = new System.Windows.Forms.TextBox();
            this.btBrowseDock = new System.Windows.Forms.Button();
            this.btTestDock = new System.Windows.Forms.Button();
            this.btBrowseMission = new System.Windows.Forms.Button();
            this.btTestMission = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelRevision = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbCondSound = new System.Windows.Forms.TextBox();
            this.btBrowseCond = new System.Windows.Forms.Button();
            this.btTestCond = new System.Windows.Forms.Button();
            this.cbUseNetTcp = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelModeText = new System.Windows.Forms.Label();
            this.btTestNotify = new System.Windows.Forms.Button();
            this.btNotify = new System.Windows.Forms.Button();
            this.tbNotifySound = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbSlackNotifyTest = new System.Windows.Forms.Button();
            this.tbSlackChannel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbSlackUserName = new System.Windows.Forms.TextBox();
            this.tbSlackIcon = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbSlackWebhookUri = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbSlackMessagePrefix = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "修理ドック出渠通知";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "遠征帰還通知";
            // 
            // tbDockSound
            // 
            this.tbDockSound.Location = new System.Drawing.Point(23, 36);
            this.tbDockSound.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbDockSound.Name = "tbDockSound";
            this.tbDockSound.Size = new System.Drawing.Size(589, 25);
            this.tbDockSound.TabIndex = 2;
            // 
            // tbMissionSound
            // 
            this.tbMissionSound.Location = new System.Drawing.Point(23, 92);
            this.tbMissionSound.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbMissionSound.Name = "tbMissionSound";
            this.tbMissionSound.Size = new System.Drawing.Size(589, 25);
            this.tbMissionSound.TabIndex = 3;
            // 
            // btBrowseDock
            // 
            this.btBrowseDock.Location = new System.Drawing.Point(625, 33);
            this.btBrowseDock.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btBrowseDock.Name = "btBrowseDock";
            this.btBrowseDock.Size = new System.Drawing.Size(75, 34);
            this.btBrowseDock.TabIndex = 4;
            this.btBrowseDock.Text = "...";
            this.btBrowseDock.UseVisualStyleBackColor = true;
            this.btBrowseDock.Click += new System.EventHandler(this.btBrowseDock_Click);
            // 
            // btTestDock
            // 
            this.btTestDock.Location = new System.Drawing.Point(710, 33);
            this.btTestDock.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btTestDock.Name = "btTestDock";
            this.btTestDock.Size = new System.Drawing.Size(78, 34);
            this.btTestDock.TabIndex = 5;
            this.btTestDock.Text = "♪";
            this.btTestDock.UseVisualStyleBackColor = true;
            this.btTestDock.Click += new System.EventHandler(this.btTestDock_Click);
            // 
            // btBrowseMission
            // 
            this.btBrowseMission.Location = new System.Drawing.Point(625, 88);
            this.btBrowseMission.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btBrowseMission.Name = "btBrowseMission";
            this.btBrowseMission.Size = new System.Drawing.Size(75, 34);
            this.btBrowseMission.TabIndex = 6;
            this.btBrowseMission.Text = "...";
            this.btBrowseMission.UseVisualStyleBackColor = true;
            this.btBrowseMission.Click += new System.EventHandler(this.btBrowseMission_Click);
            // 
            // btTestMission
            // 
            this.btTestMission.Location = new System.Drawing.Point(710, 88);
            this.btTestMission.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btTestMission.Name = "btTestMission";
            this.btTestMission.Size = new System.Drawing.Size(78, 34);
            this.btTestMission.TabIndex = 7;
            this.btTestMission.Text = "♪";
            this.btTestMission.UseVisualStyleBackColor = true;
            this.btTestMission.Click += new System.EventHandler(this.btTestMission_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(488, 516);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(125, 34);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(662, 516);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(125, 34);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "キャンセル";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelCopyright);
            this.groupBox1.Controls.Add(this.labelDescription);
            this.groupBox1.Controls.Add(this.labelRevision);
            this.groupBox1.Controls.Add(this.labelName);
            this.groupBox1.Location = new System.Drawing.Point(20, 402);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Size = new System.Drawing.Size(418, 148);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "バージョン情報";
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Location = new System.Drawing.Point(10, 114);
            this.labelCopyright.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(115, 18);
            this.labelCopyright.TabIndex = 3;
            this.labelCopyright.Text = "labelCopyright";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(10, 69);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(128, 18);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "labelDescription";
            // 
            // labelRevision
            // 
            this.labelRevision.AutoSize = true;
            this.labelRevision.Location = new System.Drawing.Point(173, 22);
            this.labelRevision.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelRevision.Name = "labelRevision";
            this.labelRevision.Size = new System.Drawing.Size(106, 18);
            this.labelRevision.TabIndex = 1;
            this.labelRevision.Text = "labelRevision";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(10, 22);
            this.labelName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(86, 18);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "labelName";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 124);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "コンディション回復通知";
            // 
            // tbCondSound
            // 
            this.tbCondSound.Location = new System.Drawing.Point(22, 147);
            this.tbCondSound.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbCondSound.Name = "tbCondSound";
            this.tbCondSound.Size = new System.Drawing.Size(591, 25);
            this.tbCondSound.TabIndex = 12;
            // 
            // btBrowseCond
            // 
            this.btBrowseCond.Location = new System.Drawing.Point(625, 144);
            this.btBrowseCond.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btBrowseCond.Name = "btBrowseCond";
            this.btBrowseCond.Size = new System.Drawing.Size(75, 34);
            this.btBrowseCond.TabIndex = 13;
            this.btBrowseCond.Text = "....";
            this.btBrowseCond.UseVisualStyleBackColor = true;
            this.btBrowseCond.Click += new System.EventHandler(this.btBrowseCond_Click);
            // 
            // btTestCond
            // 
            this.btTestCond.Location = new System.Drawing.Point(710, 144);
            this.btTestCond.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btTestCond.Name = "btTestCond";
            this.btTestCond.Size = new System.Drawing.Size(78, 34);
            this.btTestCond.TabIndex = 14;
            this.btTestCond.Text = "♪";
            this.btTestCond.UseVisualStyleBackColor = true;
            this.btTestCond.Click += new System.EventHandler(this.btTestCond_Click);
            // 
            // cbUseNetTcp
            // 
            this.cbUseNetTcp.AutoSize = true;
            this.cbUseNetTcp.Location = new System.Drawing.Point(448, 424);
            this.cbUseNetTcp.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cbUseNetTcp.Name = "cbUseNetTcp";
            this.cbUseNetTcp.Size = new System.Drawing.Size(309, 22);
            this.cbUseNetTcp.TabIndex = 15;
            this.cbUseNetTcp.Text = "通知をTCP(INADDR_ANY)で受け取る";
            this.cbUseNetTcp.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(518, 453);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(220, 18);
            this.label4.TabIndex = 16;
            this.label4.Text = "(設定は次回起動時より有効)";
            // 
            // labelModeText
            // 
            this.labelModeText.AutoSize = true;
            this.labelModeText.Location = new System.Drawing.Point(518, 480);
            this.labelModeText.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelModeText.Name = "labelModeText";
            this.labelModeText.Size = new System.Drawing.Size(117, 18);
            this.labelModeText.TabIndex = 17;
            this.labelModeText.Text = "labelModeText";
            // 
            // btTestNotify
            // 
            this.btTestNotify.Location = new System.Drawing.Point(710, 207);
            this.btTestNotify.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btTestNotify.Name = "btTestNotify";
            this.btTestNotify.Size = new System.Drawing.Size(78, 34);
            this.btTestNotify.TabIndex = 21;
            this.btTestNotify.Text = "♪";
            this.btTestNotify.UseVisualStyleBackColor = true;
            this.btTestNotify.Click += new System.EventHandler(this.btTestNotify_Click);
            // 
            // btNotify
            // 
            this.btNotify.Location = new System.Drawing.Point(625, 207);
            this.btNotify.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btNotify.Name = "btNotify";
            this.btNotify.Size = new System.Drawing.Size(75, 34);
            this.btNotify.TabIndex = 20;
            this.btNotify.Text = "....";
            this.btNotify.UseVisualStyleBackColor = true;
            this.btNotify.Click += new System.EventHandler(this.btNotify_Click);
            // 
            // tbNotifySound
            // 
            this.tbNotifySound.Location = new System.Drawing.Point(20, 210);
            this.tbNotifySound.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbNotifySound.Name = "tbNotifySound";
            this.tbNotifySound.Size = new System.Drawing.Size(591, 25);
            this.tbNotifySound.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 188);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 18);
            this.label5.TabIndex = 18;
            this.label5.Text = "戦闘終了通知";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.tbSlackMessagePrefix);
            this.groupBox2.Controls.Add(this.tbSlackNotifyTest);
            this.groupBox2.Controls.Add(this.tbSlackChannel);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbSlackUserName);
            this.groupBox2.Controls.Add(this.tbSlackIcon);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.tbSlackWebhookUri);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(20, 248);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Size = new System.Drawing.Size(770, 146);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Slack通知";
            // 
            // tbSlackNotifyTest
            // 
            this.tbSlackNotifyTest.Location = new System.Drawing.Point(613, 94);
            this.tbSlackNotifyTest.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbSlackNotifyTest.Name = "tbSlackNotifyTest";
            this.tbSlackNotifyTest.Size = new System.Drawing.Size(125, 34);
            this.tbSlackNotifyTest.TabIndex = 8;
            this.tbSlackNotifyTest.Text = "通知テスト";
            this.tbSlackNotifyTest.UseVisualStyleBackColor = true;
            this.tbSlackNotifyTest.Click += new System.EventHandler(this.tbSlackNotifyTest_Click);
            // 
            // tbSlackChannel
            // 
            this.tbSlackChannel.Location = new System.Drawing.Point(289, 100);
            this.tbSlackChannel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbSlackChannel.Name = "tbSlackChannel";
            this.tbSlackChannel.Size = new System.Drawing.Size(129, 25);
            this.tbSlackChannel.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(287, 78);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 18);
            this.label9.TabIndex = 6;
            this.label9.Text = "通知先チャンネル";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(144, 78);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 18);
            this.label8.TabIndex = 5;
            this.label8.Text = "表示ユーザ名";
            // 
            // tbSlackUserName
            // 
            this.tbSlackUserName.Location = new System.Drawing.Point(147, 100);
            this.tbSlackUserName.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbSlackUserName.Name = "tbSlackUserName";
            this.tbSlackUserName.Size = new System.Drawing.Size(132, 25);
            this.tbSlackUserName.TabIndex = 4;
            // 
            // tbSlackIcon
            // 
            this.tbSlackIcon.Location = new System.Drawing.Point(10, 100);
            this.tbSlackIcon.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbSlackIcon.Name = "tbSlackIcon";
            this.tbSlackIcon.Size = new System.Drawing.Size(127, 25);
            this.tbSlackIcon.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 78);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 18);
            this.label7.TabIndex = 2;
            this.label7.Text = "Icon(emoji/URI)";
            // 
            // tbSlackWebhookUri
            // 
            this.tbSlackWebhookUri.Location = new System.Drawing.Point(13, 45);
            this.tbSlackWebhookUri.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tbSlackWebhookUri.Name = "tbSlackWebhookUri";
            this.tbSlackWebhookUri.Size = new System.Drawing.Size(722, 25);
            this.tbSlackWebhookUri.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 22);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(364, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Incoming WebHooks URI (空白にすると通知無効)";
            // 
            // tbSlackMessagePrefix
            // 
            this.tbSlackMessagePrefix.Location = new System.Drawing.Point(428, 100);
            this.tbSlackMessagePrefix.Name = "tbSlackMessagePrefix";
            this.tbSlackMessagePrefix.Size = new System.Drawing.Size(124, 25);
            this.tbSlackMessagePrefix.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(426, 79);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 18);
            this.label10.TabIndex = 10;
            this.label10.Text = "プレフィクス";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 567);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btTestNotify);
            this.Controls.Add(this.btNotify);
            this.Controls.Add(this.tbNotifySound);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelModeText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbUseNetTcp);
            this.Controls.Add(this.btTestCond);
            this.Controls.Add(this.btBrowseCond);
            this.Controls.Add(this.tbCondSound);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.btTestMission);
            this.Controls.Add(this.btBrowseMission);
            this.Controls.Add(this.btTestDock);
            this.Controls.Add(this.btBrowseDock);
            this.Controls.Add(this.tbMissionSound);
            this.Controls.Add(this.tbDockSound);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Text = "設定";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDockSound;
        private System.Windows.Forms.TextBox tbMissionSound;
        private System.Windows.Forms.Button btBrowseDock;
        private System.Windows.Forms.Button btTestDock;
        private System.Windows.Forms.Button btBrowseMission;
        private System.Windows.Forms.Button btTestMission;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelRevision;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbCondSound;
        private System.Windows.Forms.Button btBrowseCond;
        private System.Windows.Forms.Button btTestCond;
        private System.Windows.Forms.CheckBox cbUseNetTcp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelModeText;
        private System.Windows.Forms.Button btTestNotify;
        private System.Windows.Forms.Button btNotify;
        private System.Windows.Forms.TextBox tbNotifySound;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button tbSlackNotifyTest;
        private System.Windows.Forms.TextBox tbSlackChannel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbSlackUserName;
        private System.Windows.Forms.TextBox tbSlackIcon;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbSlackWebhookUri;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbSlackMessagePrefix;
    }
}