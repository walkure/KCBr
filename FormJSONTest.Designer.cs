namespace KCB2
{
    partial class FormJSONTest
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbAPIEntry = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tbReq = new System.Windows.Forms.TextBox();
            this.tbRes = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbAPIEntry, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbReq, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbRes, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(449, 247);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "API EntryPoint";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Request";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 193);
            this.label3.TabIndex = 2;
            this.label3.Text = "Response";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbAPIEntry
            // 
            this.cbAPIEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbAPIEntry.FormattingEnabled = true;
            this.cbAPIEntry.Items.AddRange(new object[] {
            "/kcsapi/api_get_member/basic",
            "/kcsapi/api_get_master/ship",
            "/kcsapi/api_get_member/material",
            "/kcsapi/api_get_master/stype",
            "/kcsapi/api_get_member/deck",
            "/kcsapi/api_get_member/deck_port",
            "/kcsapi/api_get_member/ship",
            "/kcsapi/api_get_member/ship2",
            "/kcsapi/api_get_member/slotitem",
            "/kcsapi/api_get_master/mission",
            "/kcsapi/api_get_member/ndock",
            "/kcsapi/api_get_member/kdock",
            "/kcsapi/api_req_kousyou/createship",
            "/kcsapi/api_get_member/questlist",
            "/kcsapi/api_req_quest/clearitemget",
            "/kcsapi/api_req_kousyou/createitem",
            "/kcsapi/api_req_sortie/battleresult",
            "/kcsapi/api_req_map/start",
            "/kcsapi/api_req_mission/result"});
            this.cbAPIEntry.Location = new System.Drawing.Point(89, 3);
            this.cbAPIEntry.Name = "cbAPIEntry";
            this.cbAPIEntry.Size = new System.Drawing.Size(276, 20);
            this.cbAPIEntry.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(371, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbReq
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbReq, 2);
            this.tbReq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReq.Location = new System.Drawing.Point(89, 32);
            this.tbReq.Name = "tbReq";
            this.tbReq.Size = new System.Drawing.Size(357, 19);
            this.tbReq.TabIndex = 5;
            // 
            // tbRes
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tbRes, 2);
            this.tbRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRes.Location = new System.Drawing.Point(89, 57);
            this.tbRes.MaxLength = 0;
            this.tbRes.Multiline = true;
            this.tbRes.Name = "tbRes";
            this.tbRes.Size = new System.Drawing.Size(357, 187);
            this.tbRes.TabIndex = 6;
            // 
            // FormJSONTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 247);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormJSONTest";
            this.Text = "JSONを送り込むテスト";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormJSONTest_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbAPIEntry;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbReq;
        private System.Windows.Forms.TextBox tbRes;
    }
}