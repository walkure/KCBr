namespace KCB2
{
    partial class FormQuestList
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
            this.lvQuestList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvQuestList
            // 
            this.lvQuestList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvQuestList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvQuestList.FullRowSelect = true;
            this.lvQuestList.GridLines = true;
            this.lvQuestList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvQuestList.Location = new System.Drawing.Point(0, 0);
            this.lvQuestList.Name = "lvQuestList";
            this.lvQuestList.ShowItemToolTips = true;
            this.lvQuestList.Size = new System.Drawing.Size(610, 110);
            this.lvQuestList.TabIndex = 0;
            this.lvQuestList.UseCompatibleStateImageBehavior = false;
            this.lvQuestList.View = System.Windows.Forms.View.Details;
            this.lvQuestList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvQuestList_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No.";
            this.columnHeader1.Width = 33;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = " ";
            this.columnHeader2.Width = 23;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "説明";
            this.columnHeader3.Width = 166;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "詳細";
            this.columnHeader4.Width = 324;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "進捗";
            this.columnHeader5.Width = 53;
            // 
            // FormQuestList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 110);
            this.Controls.Add(this.lvQuestList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormQuestList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "任務一覧";
            this.Deactivate += new System.EventHandler(this.FormQuestList_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQuestList_FormClosing);
            this.Load += new System.EventHandler(this.FormQuestList_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvQuestList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}