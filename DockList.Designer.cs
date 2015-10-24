namespace KCB2
{
    partial class DockList
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDock1 = new System.Windows.Forms.Label();
            this.lblDock2 = new System.Windows.Forms.Label();
            this.lblDock3 = new System.Windows.Forms.Label();
            this.lblDock4 = new System.Windows.Forms.Label();
            this.lblTime1 = new KCB2.CountdownLabel();
            this.lblTime2 = new KCB2.CountdownLabel();
            this.lblTime3 = new KCB2.CountdownLabel();
            this.lblTime4 = new KCB2.CountdownLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblTime4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTime3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTime2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDock1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDock2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDock3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblDock4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTime1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(123, 95);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblDock1
            // 
            this.lblDock1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDock1.AutoSize = true;
            this.lblDock1.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDock1.Location = new System.Drawing.Point(6, 7);
            this.lblDock1.Name = "lblDock1";
            this.lblDock1.Size = new System.Drawing.Size(35, 12);
            this.lblDock1.TabIndex = 0;
            this.lblDock1.Text = "ドック1";
            // 
            // lblDock2
            // 
            this.lblDock2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDock2.AutoSize = true;
            this.lblDock2.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDock2.Location = new System.Drawing.Point(6, 30);
            this.lblDock2.Name = "lblDock2";
            this.lblDock2.Size = new System.Drawing.Size(35, 12);
            this.lblDock2.TabIndex = 1;
            this.lblDock2.Text = "ドック2";
            // 
            // lblDock3
            // 
            this.lblDock3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDock3.AutoSize = true;
            this.lblDock3.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDock3.Location = new System.Drawing.Point(6, 53);
            this.lblDock3.Name = "lblDock3";
            this.lblDock3.Size = new System.Drawing.Size(35, 12);
            this.lblDock3.TabIndex = 2;
            this.lblDock3.Text = "ドック3";
            // 
            // lblDock4
            // 
            this.lblDock4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDock4.AutoSize = true;
            this.lblDock4.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDock4.Location = new System.Drawing.Point(6, 76);
            this.lblDock4.Name = "lblDock4";
            this.lblDock4.Size = new System.Drawing.Size(35, 12);
            this.lblDock4.TabIndex = 3;
            this.lblDock4.Text = "ドック4";
            // 
            // lblTime1
            // 
            this.lblTime1.AutoSize = true;
            this.lblTime1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime1.FinishTime = new System.DateTime(((long)(0)));
            this.lblTime1.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime1.Location = new System.Drawing.Point(50, 3);
            this.lblTime1.Name = "lblTime1";
            this.lblTime1.Size = new System.Drawing.Size(67, 20);
            this.lblTime1.TabIndex = 4;
            this.lblTime1.Text = "N/A";
            this.lblTime1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTime1.Valid = false;
            // 
            // lblTime2
            // 
            this.lblTime2.AutoSize = true;
            this.lblTime2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime2.FinishTime = new System.DateTime(((long)(0)));
            this.lblTime2.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime2.Location = new System.Drawing.Point(50, 26);
            this.lblTime2.Name = "lblTime2";
            this.lblTime2.Size = new System.Drawing.Size(67, 20);
            this.lblTime2.TabIndex = 5;
            this.lblTime2.Text = "N/A";
            this.lblTime2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTime2.Valid = false;
            // 
            // lblTime3
            // 
            this.lblTime3.AutoSize = true;
            this.lblTime3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime3.FinishTime = new System.DateTime(((long)(0)));
            this.lblTime3.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime3.Location = new System.Drawing.Point(50, 49);
            this.lblTime3.Name = "lblTime3";
            this.lblTime3.Size = new System.Drawing.Size(67, 20);
            this.lblTime3.TabIndex = 6;
            this.lblTime3.Text = "N/A";
            this.lblTime3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTime3.Valid = false;
            // 
            // lblTime4
            // 
            this.lblTime4.AutoSize = true;
            this.lblTime4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime4.FinishTime = new System.DateTime(((long)(0)));
            this.lblTime4.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime4.Location = new System.Drawing.Point(50, 72);
            this.lblTime4.Name = "lblTime4";
            this.lblTime4.Size = new System.Drawing.Size(67, 20);
            this.lblTime4.TabIndex = 7;
            this.lblTime4.Text = "N/A";
            this.lblTime4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTime4.Valid = false;
            // 
            // DockList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DockList";
            this.Size = new System.Drawing.Size(123, 95);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblDock1;
        private System.Windows.Forms.Label lblDock2;
        private System.Windows.Forms.Label lblDock3;
        private System.Windows.Forms.Label lblDock4;
        private CountdownLabel lblTime4;
        private CountdownLabel lblTime3;
        private CountdownLabel lblTime2;
        private CountdownLabel lblTime1;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}
