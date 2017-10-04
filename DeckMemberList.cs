using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace KCB2
{
    public partial class DeckMemberList : UserControl
    {
        /// <summary>
        /// コンディションが戻るまでの時間が更新された時に叩かれるコールバック定義
        /// </summary>
        /// <param name="deckStat">艦隊情報</param>
        public delegate void UpdateDeckStatusCallback(DeckStatus deckStat);

        /// <summary>
        /// 詳細運動の配列
        /// </summary>
        public enum DetailWindowAlignmentOrigin
        { Top,Bottom };

        /// <summary>
        /// 詳細ウィンドウの表示位置オリジン
        /// </summary>
        public DetailWindowAlignmentOrigin DetailWndOrigin = DetailWindowAlignmentOrigin.Top;

        /// <summary>
        /// 艦隊情報
        /// </summary>
        public class DeckStatus
        {
            /// <summary>
            /// 艦隊の速度種別
            /// </summary>
            public enum FleetSpeed
            {
                None,Fast, Slow, Mixed
            };

            /// <summary>
            /// 艦隊番号
            /// </summary>
            public int FleetNo { get; private set; }

            /// <summary>
            /// 艦隊名
            /// </summary>
            public string FleetName { get; private set; }

            /// <summary>
            /// 速度種別
            /// </summary>
            public FleetSpeed SpeedType { get; private set; }

            /// <summary>
            /// 速度種別文字列
            /// </summary>
            public string SpeedTypeString
            {
                get
                {
                    switch (SpeedType)
                    {
                        case FleetSpeed.Fast:
                            return "高速";
                        case FleetSpeed.Slow:
                            return "低速";
                        case FleetSpeed.Mixed:
                            return "混成";
                        default:
                            return "?";
                    }

                }
            }

            /// <summary>
            /// コンディションが戻る時間
            /// </summary>
            public DateTime RecoverTime { get; private set; }

            /// <summary>
            /// レベル合計
            /// </summary>
            public int LevelSum { get; private set; }

            /// <summary>
            /// メンバー数
            /// </summary>
            public int MemberCount { get; private set; }

            /// <summary>
            /// 疲労しているかどうか
            /// </summary>
            public bool Tired { get; private set; }

            /// <summary>
            /// 制空値
            /// </summary>
            public int AirSuperiority { get; private set; }

            /// <summary>
            /// 索敵値(艦隊の単純合計)
            /// </summary>
            public int TotalSearch { get; private set; }

            /// <summary>
            /// 索敵値(2-5用)
            /// </summary>
            public int ModifiedSearch { get; private set; }

            public DeckStatus(int fleetNum,string fleetName, int minCond, DateTime dtLastUpdated, int sumLevel,
                int numMember, int speedType, int airSuperiority,int aircraftSearch,
                int radarSearch,int shipSearch)
            {
                this.FleetNo = fleetNum;
                this.FleetName = fleetName;
                LevelSum = sumLevel;
                MemberCount = numMember;
                AirSuperiority = airSuperiority;

                ///速度種別
                SpeedType = FleetSpeed.None;
                switch (speedType)
                {
                    case 0:
                        SpeedType = FleetSpeed.Mixed;
                        Debug.WriteLine("速度:混成");
                        break;
                    case 5:
                        SpeedType = FleetSpeed.Slow;
                        Debug.WriteLine("速度:低速");
                        break;
                    case 10:
                        SpeedType = FleetSpeed.Fast;
                        Debug.WriteLine("速度:高速");
                        break;

                }

                int condCeil = Properties.Settings.Default.CondCeil;
                Debug.WriteLine(string.Format("Minimum Condition value:{0} ceil:{1}", minCond, condCeil));

                ///索敵値
                ///APIの返す艦娘索敵値は装備補正が込み
                TotalSearch = shipSearch;

                ///2-5用の索敵値
                /// 偵察機索敵値×2 ＋ 電探索敵値 ＋ √(艦隊の装備込み索敵値合計 - 偵察機索敵値 - 電探索敵値) ≧ 76
                /// http://wikiwiki.jp/kancolle/?%C6%EE%C0%BE%BD%F4%C5%E7%B3%A4%B0%E8#area5 (13 Aug 2014現在)
                /// sqrtは四捨五入をしたほうがよさげ。
                ModifiedSearch = aircraftSearch * 2 + radarSearch
                    + (int)Math.Round(Math.Sqrt(shipSearch - radarSearch - aircraftSearch));
                Debug.WriteLine(string.Format("索敵値 合計:{0} 2-5:{1}", TotalSearch, ModifiedSearch));

                ///疲労回復時間の計算
                ///
                //タイマ無効
                Tired = false;
                RecoverTime = DateTime.MinValue;
                if (condCeil == 0)
                {
                    Debug.WriteLine("チェックしない");
                    return;
                }

                //誰も疲れてない
                if (minCond >= condCeil)
                {
                    Debug.WriteLine("誰も疲れてない");
                    return;
                }

                // 疲れは1分で1回復する(内部的には3分で3)
                Tired = true;
                int condTime = (condCeil - minCond);
                RecoverTime = dtLastUpdated.AddMinutes(condTime);
                Debug.WriteLine(string.Format("疲労が取れるまで{0}分 時刻:{1}", condTime, RecoverTime.ToString()));


            }
        }


        /// <summary>
        /// コンディションが戻るまでの時間が更新された時に叩かれるコールバック
        /// </summary>
        public UpdateDeckStatusCallback UpdateDeckStatus { get; set; }

        public DeckMemberList()
        {
            InitializeComponent();
            fleetList1.ShowDetail = showDetail;
        }

        public ImageList SlotItemIconImageList
        {
            set
            {
                _imgSlotItemIcon = value;
                fleetList1.SlotItemIconImageList = value;
            }
        }

        ImageList _imgSlotItemIcon;

        /// <summary>
        /// 艦娘の装備など詳細情報を表示する
        /// </summary>
        /// <param name="row">表示する艦娘の順位</param>
        void showDetail(MemberData.Ship.Info info)
        {
            if (Properties.Settings.Default.ShipDetailItem.Length == 0)
                return;

            FormShipDetail dlg = new FormShipDetail(_imgSlotItemIcon);
            dlg.StartPosition = FormStartPosition.Manual;
            dlg.Size = Properties.Settings.Default.ShipDetailSize;

            if (DetailWndOrigin == DetailWindowAlignmentOrigin.Top)
            {
                //ウィンドウトップをあわせる
                dlg.Location = this.PointToScreen(new Point(fleetList1.Location.X
                    + fleetList1.Width + 2, fleetList1.Location.Y));
            }
            else
            {
                //ウィンドウボトムを合わせる
                dlg.Location = this.PointToScreen(new Point(fleetList1.Location.X
                    + fleetList1.Width + 2, fleetList1.Location.Y
                    + (fleetList1.Size.Height + cbDeck.Size.Height - dlg.Size.Height)));
            }
            dlg.Info = info;

            dlg.Show();
        }

        /// <summary>
        /// 情報更新
        /// </summary>
        /// <param name="shipData">艦隊情報</param>
        /// <param name="deckList">編成情報</param>
        public void UpdateDeck(IEnumerable<MemberData.Deck.Fleet> deckList,MemberData.Ship shipData)
        {
            List<FleetList.Deck> deck = new List<FleetList.Deck>();
            lock (deckList)
            {
                foreach (var it in deckList)
                {
                    deck.Add(new FleetList.Deck(it, shipData));
                }
            }

            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => updateDeckView(deck)));
            else
                updateDeckView(deck);
        }

        void updateDeckView(List<FleetList.Deck> deck)
        {
            cbDeck.BeginUpdate();

            ///艦隊が減ることは想定しない。
            int nDiff = deck.Count - cbDeck.Items.Count;
            for (int i = 0; i < nDiff; i++)
                cbDeck.Items.Add("");

            for (int n = 0; n < cbDeck.Items.Count; n++)
                cbDeck.Items[n] = deck[n];

            if (cbDeck.SelectedIndex == -1 && cbDeck.Items.Count >0)
                cbDeck.SelectedIndex = 0;

            cbDeck.EndUpdate();
        }


        /// <summary>
        /// 表示する艦隊を変更した時に呼ばれるハンドラ
        /// 何故かメンバー変更時にも呼ばれる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDeck_SelectedIndexChanged(object sender, EventArgs e)
        {
            FleetList.Deck item = cbDeck.SelectedItem as FleetList.Deck;
            if (item == null)
                return;

            fleetList1.SelectedDeck = item;

            //コンディション最大値は100
            int minCond = 100;
            int sumLevel = 0;
            int airSuperiority = 0;

            ///速度は 0:混成 5:低速 10:高速
            int speed = -1;

            ///航空機索敵値
            int aircraftSearch = 0;

            ///電探の索敵値
            int radarSearch = 0;

            ///艦娘の索敵値(装備込)
            int shipSearch = 0;

            ///艦隊の最低コンディション値とレベル合計と速度構成を調べる
            for (int n = 0; n < 6; n++)
            {
                if (item.Member.Count > n)
                {
                    MemberData.Ship.Info shipInfo = item.Member[n];

                    //艦隊メンバーが存在するとき
                    sumLevel += shipInfo.Level;
                    airSuperiority += shipInfo.AirSuperiorityAbility;

                    if (minCond > shipInfo.Condition)
                        minCond = shipInfo.Condition;

                    //速度種別
                    if (speed == -1)
                        speed = shipInfo.Speed;
                    else
                    {
                        if (speed > 0 && speed != shipInfo.Speed)
                            speed = 0;
                    }

                    //索敵値
                    aircraftSearch += shipInfo.AircraftSearch;
                    radarSearch += shipInfo.RadarSearch;
                    shipSearch += shipInfo.Search.Now;

                    Debug.WriteLine(string.Format("索敵値({0}) 艦載機:{1} 電探:{2} 装備込艦娘:{3}",
                        shipInfo.ShipName, shipInfo.AircraftSearch, shipInfo.RadarSearch, shipInfo.Search.Now));

                }
            }

            if (UpdateDeckStatus != null)
                UpdateDeckStatus(new DeckStatus(item.Num,item.Name, minCond, item.ShipDataLastUpdated,
                    sumLevel,item.Member.Count, speed,airSuperiority,aircraftSearch,radarSearch,
                    shipSearch));

        }

        /// <summary>
        /// マウスホイールイベントをハンドルする
        /// </summary>
        /// <param name="args"></param>
        public void DoDeckListMouseWheelEvent(MouseEventArgs args)
        {
            int nWheel = args.Delta; // / 120;

            if (nWheel < 0)
                cbDeck.SelectNext();
            else
                cbDeck.SelectPrev();


            Debug.WriteLine(string.Format("DoDeckListMouseWheelEvent:{0}",nWheel));
        }

        /// <summary>
        /// 選択アイテムを進めたり戻したりする昨日をカプセル化したComboBox
        /// </summary>
        class ComboBoxEx : ComboBox
        {
            public void SelectNext()
            {
                int nIndex = SelectedIndex;

                nIndex++;

                if(nIndex < Items.Count)
                    SelectedIndex = nIndex;
            }

            public void SelectPrev()
            {
                int nIndex = SelectedIndex;

                nIndex--;

                if (nIndex >= 0)
                    SelectedIndex = nIndex;

            }
        }

        /// <summary>
        /// リスト再描画
        /// </summary>
        public void RedrawFleetList()
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => fleetList1.RegisterFleetsInfo()));
            else
                fleetList1.RegisterFleetsInfo();
        }
    }
}
