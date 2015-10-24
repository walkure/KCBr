namespace KCB2
{
    partial class FormSessionInfo
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
            this.tbFirstSesssionDate = new System.Windows.Forms.TextBox();
            this.tbLatestSessionDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tbAPIToken = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbServerHost = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "初回セッション取得日時";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "最新セッション取得日時";
            // 
            // tbFirstSesssionDate
            // 
            this.tbFirstSesssionDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFirstSesssionDate.Location = new System.Drawing.Point(31, 69);
            this.tbFirstSesssionDate.Name = "tbFirstSesssionDate";
            this.tbFirstSesssionDate.ReadOnly = true;
            this.tbFirstSesssionDate.Size = new System.Drawing.Size(197, 12);
            this.tbFirstSesssionDate.TabIndex = 4;
            this.tbFirstSesssionDate.Text = "SessionFirst";
            // 
            // tbLatestSessionDate
            // 
            this.tbLatestSessionDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbLatestSessionDate.Location = new System.Drawing.Point(31, 113);
            this.tbLatestSessionDate.Name = "tbLatestSessionDate";
            this.tbLatestSessionDate.ReadOnly = true;
            this.tbLatestSessionDate.Size = new System.Drawing.Size(197, 12);
            this.tbLatestSessionDate.TabIndex = 6;
            this.tbLatestSessionDate.Text = "LatestSession";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "APIトークン";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(153, 175);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // tbAPIToken
            // 
            this.tbAPIToken.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbAPIToken.Location = new System.Drawing.Point(31, 152);
            this.tbAPIToken.Name = "tbAPIToken";
            this.tbAPIToken.ReadOnly = true;
            this.tbAPIToken.Size = new System.Drawing.Size(197, 12);
            this.tbAPIToken.TabIndex = 8;
            this.tbAPIToken.Text = "API Token";
            this.tbAPIToken.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tbAPIToken_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "接続サーバ";
            // 
            // tbServerHost
            // 
            this.tbServerHost.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbServerHost.Location = new System.Drawing.Point(31, 24);
            this.tbServerHost.Name = "tbServerHost";
            this.tbServerHost.ReadOnly = true;
            this.tbServerHost.Size = new System.Drawing.Size(197, 12);
            this.tbServerHost.TabIndex = 2;
            this.tbServerHost.Text = "ServerHost";
            // 
            // FormSessionInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 210);
            this.Controls.Add(this.tbServerHost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbAPIToken);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLatestSessionDate);
            this.Controls.Add(this.tbFirstSesssionDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSessionInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "セッション情報";
            this.Load += new System.EventHandler(this.FormSessionInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFirstSesssionDate;
        private System.Windows.Forms.TextBox tbLatestSessionDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox tbAPIToken;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbServerHost;
    }
}