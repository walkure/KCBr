namespace KCB2
{
    partial class FormShipDetail
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
            this.lvStatus = new ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvStatus
            // 
            this.lvStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvStatus.FullRowSelect = true;
            this.lvStatus.GridLines = true;
            this.lvStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvStatus.Location = new System.Drawing.Point(0, 0);
            this.lvStatus.Name = "lvStatus";
            this.lvStatus.ShowItemToolTips = true;
            this.lvStatus.Size = new System.Drawing.Size(250, 188);
            this.lvStatus.TabIndex = 0;
            this.lvStatus.UseCompatibleStateImageBehavior = false;
            this.lvStatus.View = System.Windows.Forms.View.Details;
            this.lvStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvStatus_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名前";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "　";
            this.columnHeader2.Width = 173;
            // 
            // FormShipDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(250, 188);
            this.ControlBox = false;
            this.Controls.Add(this.lvStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormShipDetail";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "詳細";
            this.Deactivate += new System.EventHandler(this.FormShipDetail_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormShipDetail_FormClosing);
            this.Load += new System.EventHandler(this.FormShipDetail_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ListViewEx lvStatus;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;


    }
}