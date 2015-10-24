namespace KCB2
{
    partial class FormScreenShot
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
            this.btSaveFile = new System.Windows.Forms.Button();
            this.btCopyToClipboard = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.cbHideHeader = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "スクリーンショットを取得しました。何処へ保存しますか？";
            // 
            // btSaveFile
            // 
            this.btSaveFile.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btSaveFile.Location = new System.Drawing.Point(14, 39);
            this.btSaveFile.Name = "btSaveFile";
            this.btSaveFile.Size = new System.Drawing.Size(109, 23);
            this.btSaveFile.TabIndex = 1;
            this.btSaveFile.Text = "画像として保存";
            this.btSaveFile.UseVisualStyleBackColor = true;
            this.btSaveFile.Click += new System.EventHandler(this.btSaveFile_Click);
            // 
            // btCopyToClipboard
            // 
            this.btCopyToClipboard.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btCopyToClipboard.Location = new System.Drawing.Point(141, 39);
            this.btCopyToClipboard.Name = "btCopyToClipboard";
            this.btCopyToClipboard.Size = new System.Drawing.Size(119, 23);
            this.btCopyToClipboard.TabIndex = 2;
            this.btCopyToClipboard.Text = "クリップボードへ転送";
            this.btCopyToClipboard.UseVisualStyleBackColor = true;
            this.btCopyToClipboard.Click += new System.EventHandler(this.btCopyToClipboard_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(285, 39);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 3;
            this.btCancel.Text = "キャンセル";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // cbHideHeader
            // 
            this.cbHideHeader.AutoSize = true;
            this.cbHideHeader.Location = new System.Drawing.Point(94, 73);
            this.cbHideHeader.Name = "cbHideHeader";
            this.cbHideHeader.Size = new System.Drawing.Size(266, 16);
            this.cbHideHeader.TabIndex = 4;
            this.cbHideHeader.Text = "画像上部30px(提督名の表示など)を範囲から外す";
            this.cbHideHeader.UseVisualStyleBackColor = true;
            // 
            // FormScreenShot
            // 
            this.AcceptButton = this.btSaveFile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(375, 101);
            this.Controls.Add(this.cbHideHeader);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btCopyToClipboard);
            this.Controls.Add(this.btSaveFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormScreenShot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "スクリーンショットを保存します";
            this.Load += new System.EventHandler(this.FormScreenShot_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btSaveFile;
        private System.Windows.Forms.Button btCopyToClipboard;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.CheckBox cbHideHeader;
    }
}