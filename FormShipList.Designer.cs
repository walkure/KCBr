namespace KCB2
{
    partial class FormShipList
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
            this.lvShipList = new KCB2.ListViewEx();
            this.SuspendLayout();
            // 
            // lvShipList
            // 
            this.lvShipList.AllowColumnReorder = true;
            this.lvShipList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvShipList.FullRowSelect = true;
            this.lvShipList.GridLines = true;
            this.lvShipList.Location = new System.Drawing.Point(0, 0);
            this.lvShipList.Name = "lvShipList";
            this.lvShipList.OwnerDraw = true;
            this.lvShipList.ShowItemToolTips = true;
            this.lvShipList.Size = new System.Drawing.Size(1936, 287);
            this.lvShipList.TabIndex = 1;
            this.lvShipList.UseCompatibleStateImageBehavior = false;
            this.lvShipList.View = System.Windows.Forms.View.Details;
            this.lvShipList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvShipList_ColumnClick);
            // 
            // FormShipList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1936, 287);
            this.Controls.Add(this.lvShipList);
            this.Name = "FormShipList";
            this.Text = "艦娘一覧";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormShipList_FormClosing);
            this.Load += new System.EventHandler(this.FormShipList_Load);
            this.ResumeLayout(false);

        }

        #endregion

//        private System.Windows.Forms.ListView lvShipList;
        private ListViewEx lvShipList;

    }
}