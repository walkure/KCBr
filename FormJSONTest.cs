using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCB2
{
    public partial class FormJSONTest : Form
    {
        public SessionProcessor processor;

        public FormJSONTest()
        {
            InitializeComponent();
        }

        private void FormJSONTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            processor.Add(new DummySessionData(
                tbReq.Text, tbRes.Text
                , cbAPIEntry.Text, "text/plain"));

        }
    }
}
