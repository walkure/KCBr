namespace KCB2
{
    partial class FormAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelRevision = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.labelIEVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelProcessMode = new System.Windows.Forms.Label();
            this.labelGCMode = new System.Windows.Forms.Label();
            this.labelBuildDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(497, 394);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // labelRevision
            // 
            this.labelRevision.AutoSize = true;
            this.labelRevision.Location = new System.Drawing.Point(179, 9);
            this.labelRevision.Name = "labelRevision";
            this.labelRevision.Size = new System.Drawing.Size(73, 12);
            this.labelRevision.TabIndex = 1;
            this.labelRevision.Text = "labelRevision";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(12, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(58, 12);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "labelName";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(12, 25);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(87, 12);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "labelDescription";
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Location = new System.Drawing.Point(12, 42);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(78, 12);
            this.labelCopyright.TabIndex = 4;
            this.labelCopyright.Text = "labelCopyright";
            // 
            // labelIEVersion
            // 
            this.labelIEVersion.AutoSize = true;
            this.labelIEVersion.Location = new System.Drawing.Point(268, 9);
            this.labelIEVersion.Name = "labelIEVersion";
            this.labelIEVersion.Size = new System.Drawing.Size(78, 12);
            this.labelIEVersion.TabIndex = 5;
            this.labelIEVersion.Text = "labelIEVersion";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(560, 108);
            this.label1.TabIndex = 6;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(12, 187);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(560, 204);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // labelProcessMode
            // 
            this.labelProcessMode.AutoSize = true;
            this.labelProcessMode.Location = new System.Drawing.Point(268, 25);
            this.labelProcessMode.Name = "labelProcessMode";
            this.labelProcessMode.Size = new System.Drawing.Size(97, 12);
            this.labelProcessMode.TabIndex = 9;
            this.labelProcessMode.Text = "labelProcessMode";
            // 
            // labelGCMode
            // 
            this.labelGCMode.AutoSize = true;
            this.labelGCMode.Location = new System.Drawing.Point(268, 42);
            this.labelGCMode.Name = "labelGCMode";
            this.labelGCMode.Size = new System.Drawing.Size(72, 12);
            this.labelGCMode.TabIndex = 10;
            this.labelGCMode.Text = "labelGCMode";
            // 
            // labelBuildDate
            // 
            this.labelBuildDate.AutoSize = true;
            this.labelBuildDate.Location = new System.Drawing.Point(12, 59);
            this.labelBuildDate.Name = "labelBuildDate";
            this.labelBuildDate.Size = new System.Drawing.Size(79, 12);
            this.labelBuildDate.TabIndex = 11;
            this.labelBuildDate.Text = "labelBuildDate";
            // 
            // FormAbout
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 427);
            this.Controls.Add(this.labelBuildDate);
            this.Controls.Add(this.labelGCMode);
            this.Controls.Add(this.labelProcessMode);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelIEVersion);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelRevision);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelRevision;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.Label labelIEVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelProcessMode;
        private System.Windows.Forms.Label labelGCMode;
        private System.Windows.Forms.Label labelBuildDate;
    }
}