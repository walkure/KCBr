using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCB2
{
    public partial class DateTimeLabel : UserControl
    {
        public DateTime FinishTime { get; set; }

        bool _valid = false;
        public bool Valid
        {
            get { return _valid; }
            set
            {
                _valid = value;
                if (!_valid)
                    label1.Text = "00:00:00";
            }
        }

        bool ShowTime = false;


        public DateTimeLabel()
        {
            
//            label1.Text = "12:34:56"; //FinishTime.ToString();
            InitializeComponent();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan diff = FinishTime - DateTime.Now;

            if (diff.TotalMilliseconds < 0)
                label1.Text = "00:00:00";
            else
            {
                if (diff.TotalDays >= 1.0)
                    label1.Text = diff.ToString(@"d\d\ hh\:mm\:ss");
                else
                    label1.Text = diff.ToString(@"hh\:mm\:ss");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ShowTime = !ShowTime;
            timer1.Enabled = ShowTime;
            if (!ShowTime)
            {
                label1.Text = FinishTime.ToString();
            }
        }
    }
}
