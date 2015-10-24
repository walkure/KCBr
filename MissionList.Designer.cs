namespace KCB2
{
    partial class MissionList
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
            this.lblMission4 = new System.Windows.Forms.Label();
            this.lblFleetName2 = new System.Windows.Forms.Label();
            this.lblFleetName3 = new System.Windows.Forms.Label();
            this.lblFleetName4 = new System.Windows.Forms.Label();
            this.lblMission2 = new System.Windows.Forms.Label();
            this.lblMission3 = new System.Windows.Forms.Label();
            this.lblTime2 = new KCB2.CountdownLabel();
            this.lblTime3 = new KCB2.CountdownLabel();
            this.lblTime4 = new KCB2.CountdownLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblMission4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblFleetName2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFleetName3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblFleetName4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblMission2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMission3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblTime2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTime3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTime4, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(271, 181);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblMission4
            // 
            this.lblMission4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMission4, 2);
            this.lblMission4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMission4.ForeColor = System.Drawing.SystemColors.Control;
            this.lblMission4.Location = new System.Drawing.Point(5, 147);
            this.lblMission4.Name = "lblMission4";
            this.lblMission4.Size = new System.Drawing.Size(261, 32);
            this.lblMission4.TabIndex = 5;
            this.lblMission4.Text = " ";
            this.lblMission4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFleetName2
            // 
            this.lblFleetName2.AutoSize = true;
            this.lblFleetName2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFleetName2.ForeColor = System.Drawing.SystemColors.Control;
            this.lblFleetName2.Location = new System.Drawing.Point(5, 2);
            this.lblFleetName2.Name = "lblFleetName2";
            this.lblFleetName2.Size = new System.Drawing.Size(37, 27);
            this.lblFleetName2.TabIndex = 0;
            this.lblFleetName2.Text = "　　　　";
            this.lblFleetName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFleetName3
            // 
            this.lblFleetName3.AutoSize = true;
            this.lblFleetName3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFleetName3.ForeColor = System.Drawing.SystemColors.Control;
            this.lblFleetName3.Location = new System.Drawing.Point(5, 60);
            this.lblFleetName3.Name = "lblFleetName3";
            this.lblFleetName3.Size = new System.Drawing.Size(37, 27);
            this.lblFleetName3.TabIndex = 1;
            this.lblFleetName3.Text = "　　　　";
            this.lblFleetName3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFleetName4
            // 
            this.lblFleetName4.AutoSize = true;
            this.lblFleetName4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFleetName4.ForeColor = System.Drawing.SystemColors.Control;
            this.lblFleetName4.Location = new System.Drawing.Point(5, 118);
            this.lblFleetName4.Name = "lblFleetName4";
            this.lblFleetName4.Size = new System.Drawing.Size(37, 27);
            this.lblFleetName4.TabIndex = 2;
            this.lblFleetName4.Text = "　　　　";
            this.lblFleetName4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMission2
            // 
            this.lblMission2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMission2, 2);
            this.lblMission2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMission2.ForeColor = System.Drawing.SystemColors.Control;
            this.lblMission2.Location = new System.Drawing.Point(5, 31);
            this.lblMission2.Name = "lblMission2";
            this.lblMission2.Size = new System.Drawing.Size(261, 27);
            this.lblMission2.TabIndex = 3;
            this.lblMission2.Text = " ";
            this.lblMission2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMission3
            // 
            this.lblMission3.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblMission3, 2);
            this.lblMission3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMission3.ForeColor = System.Drawing.SystemColors.Control;
            this.lblMission3.Location = new System.Drawing.Point(5, 89);
            this.lblMission3.Name = "lblMission3";
            this.lblMission3.Size = new System.Drawing.Size(261, 27);
            this.lblMission3.TabIndex = 6;
            this.lblMission3.Text = " ";
            this.lblMission3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTime2
            // 
            this.lblTime2.AutoSize = true;
            this.lblTime2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime2.FinishTime = new System.DateTime(2013, 11, 9, 9, 38, 1, 798);
            this.lblTime2.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime2.Location = new System.Drawing.Point(50, 2);
            this.lblTime2.Name = "lblTime2";
            this.lblTime2.Size = new System.Drawing.Size(216, 27);
            this.lblTime2.TabIndex = 10;
            this.lblTime2.Text = "N/A";
            this.lblTime2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTime2.Valid = false;
            // 
            // lblTime3
            // 
            this.lblTime3.AutoSize = true;
            this.lblTime3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime3.FinishTime = new System.DateTime(2013, 11, 9, 9, 50, 29, 525);
            this.lblTime3.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime3.Location = new System.Drawing.Point(50, 60);
            this.lblTime3.Name = "lblTime3";
            this.lblTime3.Size = new System.Drawing.Size(216, 27);
            this.lblTime3.TabIndex = 11;
            this.lblTime3.Text = "N/A";
            this.lblTime3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTime3.Valid = false;
            // 
            // lblTime4
            // 
            this.lblTime4.AutoSize = true;
            this.lblTime4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTime4.FinishTime = new System.DateTime(2013, 11, 9, 9, 50, 33, 116);
            this.lblTime4.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTime4.Location = new System.Drawing.Point(50, 118);
            this.lblTime4.Name = "lblTime4";
            this.lblTime4.Size = new System.Drawing.Size(216, 27);
            this.lblTime4.TabIndex = 12;
            this.lblTime4.Text = "N/A";
            this.lblTime4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTime4.Valid = false;
            // 
            // MissionList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MissionList";
            this.Size = new System.Drawing.Size(271, 181);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblFleetName2;
        private System.Windows.Forms.Label lblFleetName3;
        private System.Windows.Forms.Label lblFleetName4;
        private System.Windows.Forms.Label lblMission4;
        private System.Windows.Forms.Label lblMission2;
        private System.Windows.Forms.Label lblMission3;
        private CountdownLabel lblTime2;
        private CountdownLabel lblTime3;
        private CountdownLabel lblTime4;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
