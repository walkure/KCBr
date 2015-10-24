namespace KCB2
{
    partial class FormVolume
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
            this.tbVolume = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.cbMute = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbVolume
            // 
            this.tbVolume.LargeChange = 10;
            this.tbVolume.Location = new System.Drawing.Point(25, 22);
            this.tbVolume.Maximum = 100;
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbVolume.Size = new System.Drawing.Size(42, 104);
            this.tbVolume.TabIndex = 0;
            this.tbVolume.TickFrequency = 10;
            this.tbVolume.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tbVolume.Scroll += new System.EventHandler(this.tbVolume_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "音量調整";
            // 
            // cbMute
            // 
            this.cbMute.AutoSize = true;
            this.cbMute.Location = new System.Drawing.Point(10, 129);
            this.cbMute.Name = "cbMute";
            this.cbMute.Size = new System.Drawing.Size(57, 16);
            this.cbMute.TabIndex = 2;
            this.cbMute.Text = "ミュート";
            this.cbMute.UseVisualStyleBackColor = true;
            this.cbMute.CheckedChanged += new System.EventHandler(this.cbMute_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbMute);
            this.panel1.Controls.Add(this.tbVolume);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(82, 162);
            this.panel1.TabIndex = 3;
            // 
            // FormVolume
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(82, 162);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVolume";
            this.Text = "音量";
            this.Deactivate += new System.EventHandler(this.FormVolume_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormVolume_FormClosed);
            this.Load += new System.EventHandler(this.FormVolume_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar tbVolume;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbMute;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}