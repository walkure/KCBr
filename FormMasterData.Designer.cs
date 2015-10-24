namespace KCB2
{
    partial class FormMasterData
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabShip = new System.Windows.Forms.TabPage();
            this.lvShip = new System.Windows.Forms.ListView();
            this.tabSlotItem = new System.Windows.Forms.TabPage();
            this.lvSlotItem = new System.Windows.Forms.ListView();
            this.tabSType = new System.Windows.Forms.TabPage();
            this.lvSType = new System.Windows.Forms.ListView();
            this.tabControl1.SuspendLayout();
            this.tabShip.SuspendLayout();
            this.tabSlotItem.SuspendLayout();
            this.tabSType.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabShip);
            this.tabControl1.Controls.Add(this.tabSlotItem);
            this.tabControl1.Controls.Add(this.tabSType);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(674, 366);
            this.tabControl1.TabIndex = 0;
            // 
            // tabShip
            // 
            this.tabShip.Controls.Add(this.lvShip);
            this.tabShip.Location = new System.Drawing.Point(4, 22);
            this.tabShip.Name = "tabShip";
            this.tabShip.Size = new System.Drawing.Size(666, 340);
            this.tabShip.TabIndex = 0;
            this.tabShip.Text = "艦船情報";
            this.tabShip.UseVisualStyleBackColor = true;
            // 
            // lvShip
            // 
            this.lvShip.AllowColumnReorder = true;
            this.lvShip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvShip.FullRowSelect = true;
            this.lvShip.GridLines = true;
            this.lvShip.Location = new System.Drawing.Point(0, 0);
            this.lvShip.Name = "lvShip";
            this.lvShip.ShowItemToolTips = true;
            this.lvShip.Size = new System.Drawing.Size(666, 340);
            this.lvShip.TabIndex = 0;
            this.lvShip.UseCompatibleStateImageBehavior = false;
            this.lvShip.View = System.Windows.Forms.View.Details;
            this.lvShip.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvShip_ColumnClick);
            // 
            // tabSlotItem
            // 
            this.tabSlotItem.Controls.Add(this.lvSlotItem);
            this.tabSlotItem.Location = new System.Drawing.Point(4, 22);
            this.tabSlotItem.Name = "tabSlotItem";
            this.tabSlotItem.Size = new System.Drawing.Size(666, 340);
            this.tabSlotItem.TabIndex = 1;
            this.tabSlotItem.Text = "装備情報";
            this.tabSlotItem.UseVisualStyleBackColor = true;
            // 
            // lvSlotItem
            // 
            this.lvSlotItem.AllowColumnReorder = true;
            this.lvSlotItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSlotItem.FullRowSelect = true;
            this.lvSlotItem.GridLines = true;
            this.lvSlotItem.Location = new System.Drawing.Point(0, 0);
            this.lvSlotItem.Name = "lvSlotItem";
            this.lvSlotItem.ShowItemToolTips = true;
            this.lvSlotItem.Size = new System.Drawing.Size(666, 340);
            this.lvSlotItem.TabIndex = 0;
            this.lvSlotItem.UseCompatibleStateImageBehavior = false;
            this.lvSlotItem.View = System.Windows.Forms.View.Details;
            this.lvSlotItem.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSlotItem_ColumnClick);
            // 
            // tabSType
            // 
            this.tabSType.Controls.Add(this.lvSType);
            this.tabSType.Location = new System.Drawing.Point(4, 22);
            this.tabSType.Name = "tabSType";
            this.tabSType.Size = new System.Drawing.Size(666, 340);
            this.tabSType.TabIndex = 2;
            this.tabSType.Text = "艦船種別";
            this.tabSType.UseVisualStyleBackColor = true;
            // 
            // lvSType
            // 
            this.lvSType.AllowColumnReorder = true;
            this.lvSType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSType.FullRowSelect = true;
            this.lvSType.GridLines = true;
            this.lvSType.Location = new System.Drawing.Point(0, 0);
            this.lvSType.Name = "lvSType";
            this.lvSType.ShowItemToolTips = true;
            this.lvSType.Size = new System.Drawing.Size(666, 340);
            this.lvSType.TabIndex = 0;
            this.lvSType.UseCompatibleStateImageBehavior = false;
            this.lvSType.View = System.Windows.Forms.View.Details;
            this.lvSType.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSType_ColumnClick);
            // 
            // FormMasterData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 366);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMasterData";
            this.Text = "マスタデータ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMasterData_FormClosing);
            this.Load += new System.EventHandler(this.FormMasterData_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabShip.ResumeLayout(false);
            this.tabSlotItem.ResumeLayout(false);
            this.tabSType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabShip;
        private System.Windows.Forms.ListView lvShip;
        private System.Windows.Forms.TabPage tabSlotItem;
        private System.Windows.Forms.ListView lvSlotItem;
        private System.Windows.Forms.TabPage tabSType;
        private System.Windows.Forms.ListView lvSType;
    }
}