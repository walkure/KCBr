namespace KCB2
{
    partial class DeckMemberList
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cbDeck = new ComboBoxEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.fleetList1 = new KCB2.FleetList();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbDeck
            // 
            this.cbDeck.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbDeck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeck.FormattingEnabled = true;
            this.cbDeck.Location = new System.Drawing.Point(0, 0);
            this.cbDeck.Name = "cbDeck";
            this.cbDeck.Size = new System.Drawing.Size(184, 20);
            this.cbDeck.TabIndex = 3;
            this.cbDeck.SelectedIndexChanged += new System.EventHandler(this.cbDeck_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fleetList1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(184, 117);
            this.panel1.TabIndex = 4;
            // 
            // fleetList1
            // 
            this.fleetList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fleetList1.Location = new System.Drawing.Point(0, 0);
            this.fleetList1.Name = "fleetList1";
            this.fleetList1.Size = new System.Drawing.Size(184, 117);
            this.fleetList1.TabIndex = 0;
            // 
            // DeckMemberList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbDeck);
            this.Name = "DeckMemberList";
            this.Size = new System.Drawing.Size(184, 137);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBoxEx cbDeck;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private FleetList fleetList1;
    }
}
