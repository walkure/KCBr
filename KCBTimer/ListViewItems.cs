using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCBTimer
{
    /// <summary>
    /// タイマとカウントダウン機能を持つリストビューアイテムの共通実装
    /// </summary>
    abstract class TimerHandlerListViewItem : ListViewItem
    {
        protected DateTime _finishTime;

        protected Form1 _parent;

        enum Status
        {
            /// <summary>
            /// 未設定
            /// </summary>
            BLANK,
            /// <summary>
            /// アラーム鳴動済み
            /// </summary>
            ALARMED,
            /// <summary>
            /// カウントダウン中
            /// </summary>
            TIME_WAIT,
        };

        Status _state;

        /// <summary>
        /// 残り時間表示を更新する際に呼ばれるハンドラ
        /// </summary>
        /// <param name="remainTime"></param>
        abstract protected void UpdateRemainTime(string remainTime);

        /// <summary>
        /// アラームとして鳴らすファイル
        /// </summary>
        abstract protected string AlarmFile { get; }

        /// <summary>
        /// タイマを鳴らす残り秒数
        /// </summary>
        abstract protected int TimerInvokeSeconds { get; }

        /// <summary>
        /// タイマハンドラ
        /// </summary>
        /// <param name="now"></param>
        public void OnTimer(DateTime now)
        {
            if (_state == Status.BLANK)
            {
                UpdateRemainTime("");
                return;
            }

            TimeSpan diff = _finishTime - now;
            if (diff.TotalMilliseconds < 0)
               UpdateRemainTime("00:00:00");
            else
            {
                if (diff.TotalDays >= 1.0)
                    UpdateRemainTime(diff.ToString(@"d\日\ hh\:mm\:ss"));
                else
                    UpdateRemainTime(diff.ToString(@"hh\:mm\:ss"));
            }
            
            if (!Checked)
                return;

            if (_state != Status.TIME_WAIT)
                return;

            if (diff.TotalSeconds < TimerInvokeSeconds)
            {
                _state = Status.ALARMED;
                _parent.ShowBaloonMessage(BaloonTitle,BaloonText);
                PlaySound(AlarmFile);
                Checked = false;
            }
        }

        public static void PlaySound(string wavPath)
        {
            if (wavPath.Length == 0)
            {
                System.Diagnostics.Debug.WriteLine("Timeout:Play SystemBeep");
                System.Media.SystemSounds.Beep.Play();
            }
            else
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Timeout:Play specified wav");
                    System.Media.SoundPlayer player =
                        new System.Media.SoundPlayer(wavPath);
                    player.Play();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("soundPlayer exception:" + ex.Message);
                }
            }

        }

        /// <summary>
        /// アイテムを使用中(タイマ起動)にする場合はtrue
        /// </summary>
        protected bool ItemUsing
        {
            set
            {
                if (value)
                {

                    if (_state == Status.BLANK)
                    {
                        _state = Status.TIME_WAIT;
                        Checked = true;
                    }
                }
                else
                {
                    _state = Status.BLANK;
                    Checked = false;
                    UpdateRemainTime("");
                }

            }
        }

        /// <summary>
        /// バルーンチップタイトル
        /// </summary>
        abstract protected string BaloonTitle { get; }

        /// <summary>
        /// バルーンチップテキスト
        /// </summary>
        abstract protected string BaloonText { get; }
    }

    class NDockListViewItem : TimerHandlerListViewItem
    {
        public NDockListViewItem(Form1 parent)
        {
            _parent = parent;
            Text = "";
            SubItems.Add("");
            SubItems.Add("");
            SubItems.Add("");
            ItemUsing = false;
        }

        string _exitMsg = "";
        public void Update(int dockNum, string shipName, DateTime finishTime)
        {
            SubItems[0].Text = string.Format("ドック{0}", dockNum);
            SubItems[1].Text = shipName;
            if (shipName == "")
            {
                SubItems[2].Text = "";
                SubItems[3].Text = "";
                ItemUsing = false;
            }
            else
            {

                _finishTime = finishTime;
                SubItems[2].Text = finishTime.ToString();
                _exitMsg = string.Format("まもなく{0}(ドック{1})の修理が完了します。", shipName, dockNum);
                ItemUsing = true;
            }
        }

        protected override int TimerInvokeSeconds { get { return 60; } }

        protected override void UpdateRemainTime(string remainTime)
        { SubItems[3].Text = remainTime; }

        protected override string AlarmFile
        { get { return Properties.Settings.Default.DockOutSound; } }

        protected override string BaloonTitle { get { return "修理完了予告"; } }

        protected override string BaloonText { get { return _exitMsg;} }

    }

    class MissionListViewItem : TimerHandlerListViewItem
    {
        public MissionListViewItem(Form1 parent)
        {
            _parent = parent;

            Text = "";
            SubItems.Add("");
            SubItems.Add("");
            SubItems.Add("");
            SubItems.Add("");
            ItemUsing = false;
        }

        string _exitMsg = "";
        public void Update(string fleetNum,string fleetName, string missionName,DateTime finishTime)
        {
            SubItems[0].Text = fleetNum;
            SubItems[1].Text = fleetName;
            SubItems[2].Text = missionName;
            if (missionName == "")
            {
                SubItems[3].Text = "";
                SubItems[4].Text = "";
                ItemUsing = false;
            }
            else
            {

                _finishTime = finishTime;
                SubItems[3].Text = finishTime.ToString();
                _exitMsg = string.Format("まもなく{0}が{1}から帰還します", fleetName, missionName);
                ItemUsing = true;
            }
        }

        protected override int TimerInvokeSeconds { get { return 60; } }

        protected override void UpdateRemainTime(string remainTime)
        { SubItems[4].Text = remainTime; }

        protected override string AlarmFile
        { get { return Properties.Settings.Default.MissionFinishSound; } }

        protected override string BaloonTitle { get{return "遠征終了予告";}}

        protected override string BaloonText { get { return _exitMsg; } }
    }

}
