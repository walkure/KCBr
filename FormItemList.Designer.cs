namespace KCB2
{
    partial class FormItemList
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
            this.components = new System.ComponentModel.Container();
            this.lvItemList = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // lvItemList
            // 
            this.lvItemList.AllowColumnReorder = true;
            this.lvItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvItemList.FullRowSelect = true;
            this.lvItemList.GridLines = true;
            this.lvItemList.Location = new System.Drawing.Point(0, 0);
            this.lvItemList.Name = "lvItemList";
            this.lvItemList.Size = new System.Drawing.Size(824, 199);
            this.lvItemList.TabIndex = 1;
            this.lvItemList.UseCompatibleStateImageBehavior = false;
            this.lvItemList.View = System.Windows.Forms.View.Details;
            this.lvItemList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvItemList_ColumnClick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FormItemList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 199);
            this.Controls.Add(this.lvItemList);
            this.Name = "FormItemList";
            this.Text = "装備一覧";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormItemList_FormClosing);
            this.Load += new System.EventHandler(this.FormItemList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvItemList;
        private System.Windows.Forms.ImageList imageList1;
    }
}