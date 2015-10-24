namespace KCB2
{
    partial class FormSlotItemList
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
            this.lvSlotItemList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvSlotItemList
            // 
            this.lvSlotItemList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvSlotItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSlotItemList.FullRowSelect = true;
            this.lvSlotItemList.GridLines = true;
            this.lvSlotItemList.Location = new System.Drawing.Point(0, 0);
            this.lvSlotItemList.Name = "lvSlotItemList";
            this.lvSlotItemList.ShowItemToolTips = true;
            this.lvSlotItemList.Size = new System.Drawing.Size(650, 368);
            this.lvSlotItemList.TabIndex = 0;
            this.lvSlotItemList.UseCompatibleStateImageBehavior = false;
            this.lvSlotItemList.View = System.Windows.Forms.View.Details;
            this.lvSlotItemList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSlotItemList_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 39;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "種別";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "装備名";
            this.columnHeader3.Width = 71;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "保有数";
            this.columnHeader4.Width = 55;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "装着数";
            this.columnHeader5.Width = 45;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "装備艦娘";
            this.columnHeader6.Width = 350;
            // 
            // FormSlotItemList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 368);
            this.Controls.Add(this.lvSlotItemList);
            this.Name = "FormSlotItemList";
            this.Text = "装備一覧";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSlotItemList_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvSlotItemList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}