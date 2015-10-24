using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCB2
{
    /// <summary>
    /// 自動的にカウントダウンされるタイマ
    /// </summary>
    public class CountdownLabel : Label
    {
        DateTime _finish;

        /// <summary>
        /// 終了時刻
        /// </summary>
        public DateTime FinishTime
        {
            get { return _finish; }
            set
            {
                _finish = value;
                _valid = true;
                ShowTime = false;
                _timer.Enabled = true;
            }
        }
        Timer _timer = new Timer();

        public CountdownLabel()
        {
            _timer.Interval = 1000;
            _timer.Enabled = false;
            _timer.Tick += new EventHandler(_timer_Tick);


//            FinishTime = DateTime.Now;
            Click += new EventHandler(CountdownLabel_Click);

            Text = "N/A";
        }


        bool _valid = false;

        /// <summary>
        /// 有効かどうか
        /// </summary>
        public bool Valid
        {
            get { return _valid; }
            set
            {
                _valid = value;
                if (!_valid)
                    Text = "N/A";
            }
        }

        bool ShowTime = false;

        const string _notAvailMsg = "N/A";

        void _timer_Tick(object sender, EventArgs e)
        {
            if (!_valid)
            {
                Text = _notAvailMsg;
                return;
            }

            TimeSpan diff = _finish - DateTime.Now;

            if (diff.TotalMilliseconds < 0)
                Text = "00:00:00";
            else
            {
                if (diff.TotalDays >= 1.0)
                {
                    int hours = (int)Math.Floor(diff.TotalHours);
                    Text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, diff.Minutes, diff.Seconds);
                }
                else
                    Text = diff.ToString(@"hh\:mm\:ss");
            }
        }

        void CountdownLabel_Click(object sender, EventArgs e)
        {
            if (!_valid)
            {
                Text = _notAvailMsg;
                return;
            }

            ShowTime = !ShowTime;
            _timer.Enabled = ShowTime;
            if (!ShowTime)
            {
                Text = _finish.ToString();
            }
            else
            {
                _timer_Tick(null, null);
            }
        }

    }
}
