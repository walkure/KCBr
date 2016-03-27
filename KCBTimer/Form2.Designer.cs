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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "修理ドック出渠通知";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "遠征帰還通知";
            // 
            // tbDockSound
            // 
            this.tbDockSound.Location = new System.Drawing.Point(14, 24);
            this.tbDockSound.Name = "tbDockSound";
            this.tbDockSound.Size = new System.Drawing.Size(355, 19);
            this.tbDockSound.TabIndex = 2;
            // 
            // tbMissionSound
            // 
            this.tbMissionSound.Location = new System.Drawing.Point(14, 61);
            this.tbMissionSound.Name = "tbMissionSound";
            this.tbMissionSound.Size = new System.Drawing.Size(355, 19);
            this.tbMissionSound.TabIndex = 3;
            // 
            // btBrowseDock
            // 
            this.btBrowseDock.Location = new System.Drawing.Point(375, 22);
            this.btBrowseDock.Name = "btBrowseDock";
            this.btBrowseDock.Size = new System.Drawing.Size(45, 23);
            this.btBrowseDock.TabIndex = 4;
            this.btBrowseDock.Text = "...";
            this.btBrowseDock.UseVisualStyleBackColor = true;
            this.btBrowseDock.Click += new System.EventHandler(this.btBrowseDock_Click);
            // 
            // btTestDock
            // 
            this.btTestDock.Location = new System.Drawing.Point(426, 22);
            this.btTestDock.Name = "btTestDock";
            this.btTestDock.Size = new System.Drawing.Size(47, 23);
            this.btTestDock.TabIndex = 5;
            this.btTestDock.Text = "♪";
            this.btTestDock.UseVisualStyleBackColor = true;
            this.btTestDock.Click += new System.EventHandler(this.btTestDock_Click);
            // 
            // btBrowseMission
            // 
            this.btBrowseMission.Location = new System.Drawing.Point(375, 59);
            this.btBrowseMission.Name = "btBrowseMission";
            this.btBrowseMission.Size = new System.Drawing.Size(45, 23);
            this.btBrowseMission.TabIndex = 6;
            this.btBrowseMission.Text = "...";
            this.btBrowseMission.UseVisualStyleBackColor = true;
            this.btBrowseMission.Click += new System.EventHandler(this.btBrowseMission_Click);
            // 
            // btTestMission
            // 
            this.btTestMission.Location = new System.Drawing.Point(426, 59);
            this.btTestMission.Name = "btTestMission";
            this.btTestMission.Size = new System.Drawing.Size(47, 23);
            this.btTestMission.TabIndex = 7;
            this.btTestMission.Text = "♪";
            this.btTestMission.UseVisualStyleBackColor = true;
            this.btTestMission.Click += new System.EventHandler(this.btTestMission_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(293, 252);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(397, 252);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
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
            this.groupBox1.Location = new System.Drawing.Point(12, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(251, 99);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "バージョン情報";
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Location = new System.Drawing.Point(6, 76);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(78, 12);
            this.labelCopyright.TabIndex = 3;
            this.labelCopyright.Text = "labelCopyright";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(6, 46);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(87, 12);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "labelDescription";
            // 
            // labelRevision
            // 
            this.labelRevision.AutoSize = true;
            this.labelRevision.Location = new System.Drawing.Point(104, 15);
            this.labelRevision.Name = "labelRevision";
            this.labelRevision.Size = new System.Drawing.Size(73, 12);
            this.labelRevision.TabIndex = 1;
            this.labelRevision.Text = "labelRevision";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(6, 15);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(58, 12);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "labelName";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "コンディション回復通知";
            // 
            // tbCondSound
            // 
            this.tbCondSound.Location = new System.Drawing.Point(13, 98);
            this.tbCondSound.Name = "tbCondSound";
            this.tbCondSound.Size = new System.Drawing.Size(356, 19);
            this.tbCondSound.TabIndex = 12;
            // 
            // btBrowseCond
            // 
            this.btBrowseCond.Location = new System.Drawing.Point(375, 96);
            this.btBrowseCond.Name = "btBrowseCond";
            this.btBrowseCond.Size = new System.Drawing.Size(45, 23);
            this.btBrowseCond.TabIndex = 13;
            this.btBrowseCond.Text = "....";
            this.btBrowseCond.UseVisualStyleBackColor = true;
            this.btBrowseCond.Click += new System.EventHandler(this.btBrowseCond_Click);
            // 
            // btTestCond
            // 
            this.btTestCond.Location = new System.Drawing.Point(426, 96);
            this.btTestCond.Name = "btTestCond";
            this.btTestCond.Size = new System.Drawing.Size(47, 23);
            this.btTestCond.TabIndex = 14;
            this.btTestCond.Text = "♪";
            this.btTestCond.UseVisualStyleBackColor = true;
            this.btTestCond.Click += new System.EventHandler(this.btTestCond_Click);
            // 
            // cbUseNetTcp
            // 
            this.cbUseNetTcp.AutoSize = true;
            this.cbUseNetTcp.Location = new System.Drawing.Point(269, 191);
            this.cbUseNetTcp.Name = "cbUseNetTcp";
            this.cbUseNetTcp.Size = new System.Drawing.Size(210, 16);
            this.cbUseNetTcp.TabIndex = 15;
            this.cbUseNetTcp.Text = "通知をTCP(INADDR_ANY)で受け取る";
            this.cbUseNetTcp.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(311, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 12);
            this.label4.TabIndex = 16;
            this.label4.Text = "(設定は次回起動時より有効)";
            // 
            // labelModeText
            // 
            this.labelModeText.AutoSize = true;
            this.labelModeText.Location = new System.Drawing.Point(311, 228);
            this.labelModeText.Name = "labelModeText";
            this.labelModeText.Size = new System.Drawing.Size(79, 12);
            this.labelModeText.TabIndex = 17;
            this.labelModeText.Text = "labelModeText";
            // 
            // btTestNotify
            // 
            this.btTestNotify.Location = new System.Drawing.Point(426, 138);
            this.btTestNotify.Name = "btTestNotify";
            this.btTestNotify.Size = new System.Drawing.Size(47, 23);
            this.btTestNotify.TabIndex = 21;
            this.btTestNotify.Text = "♪";
            this.btTestNotify.UseVisualStyleBackColor = true;
            this.btTestNotify.Click += new System.EventHandler(this.btTestNotify_Click);
            // 
            // btNotify
            // 
            this.btNotify.Location = new System.Drawing.Point(375, 138);
            this.btNotify.Name = "btNotify";
            this.btNotify.Size = new System.Drawing.Size(45, 23);
            this.btNotify.TabIndex = 20;
            this.btNotify.Text = "....";
            this.btNotify.UseVisualStyleBackColor = true;
            this.btNotify.Click += new System.EventHandler(this.btNotify_Click);
            // 
            // tbNotifySound
            // 
            this.tbNotifySound.Location = new System.Drawing.Point(12, 140);
            this.tbNotifySound.Name = "tbNotifySound";
            this.tbNotifySound.Size = new System.Drawing.Size(356, 19);
            this.tbNotifySound.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "戦闘終了通知";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 287);
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.Text = "設定";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}