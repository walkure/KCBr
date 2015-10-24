namespace KCB2
{
    partial class FormBadShipStatus
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
            this.lblThreshold = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lvShipStatus = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "指定損傷率を下回る艦娘がいます。進撃を続行しますか？";
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(12, 25);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(35, 12);
            this.lblThreshold.TabIndex = 1;
            this.lblThreshold.Text = "label2";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonOK.Location = new System.Drawing.Point(205, 314);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "進撃続行";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.No;
            this.button2.Location = new System.Drawing.Point(286, 314);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "進撃中断";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // lvShipStatus
            // 
            this.lvShipStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvShipStatus.FullRowSelect = true;
            this.lvShipStatus.GridLines = true;
            this.lvShipStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvShipStatus.Location = new System.Drawing.Point(12, 40);
            this.lvShipStatus.Name = "lvShipStatus";
            this.lvShipStatus.Size = new System.Drawing.Size(362, 257);
            this.lvShipStatus.TabIndex = 2;
            this.lvShipStatus.UseCompatibleStateImageBehavior = false;
            this.lvShipStatus.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No.";
            this.columnHeader1.Width = 33;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ダメコン";
            this.columnHeader2.Width = 52;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "名前";
            this.columnHeader3.Width = 108;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "調子";
            this.columnHeader4.Width = 46;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "HP";
            this.columnHeader5.Width = 91;
            // 
            // FormBadShipStatus
            // 
            this.AcceptButton = this.button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(384, 349);
            this.ControlBox = false;
            this.Controls.Add(this.lvShipStatus);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.lblThreshold);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormBadShipStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "調子の悪い艦娘がいます";
            this.Load += new System.EventHandler(this.FormBadShipStatus_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView lvShipStatus;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}