using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KCB2
{
    public partial class FleetList : UserControl
    {
        /// <summary>
        /// クリックされた際に呼び出すdelegate
        /// </summary>
        /// <param name="row"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public delegate void ShowShipDetailCallback(MemberData.Ship.Info info);

        /// <summary>
        /// アイテムクリック時に呼び出すcallbackを設定
        /// </summary>
        public ShowShipDetailCallback ShowDetail { get; set; }


        /// <summary>
        /// コンボボックスに入れる艦隊データ
        /// </summary>
        public class Deck : IComparable<Deck>
        {
            /// <summary>
            /// 艦隊名
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// 遠征番号
            /// </summary>
            public int MissionNum { get; private set; }

            /// <summary>
            /// 所属艦隊番号
            /// </summary>
            public int Num { get; private set; }

            /// <summary>
            /// 艦隊所属メンバ
            /// </summary>
            public List<MemberData.Ship.Info> Member { get; private set; }

            /// <summary>
            /// 艦隊情報が更新された時刻
            /// </summary>
            public DateTime ShipDataLastUpdated { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="fleet">艦隊情報</param>
            /// <param name="shipData">艦娘情報</param>
            public Deck(MemberData.Deck.Fleet fleet, MemberData.Ship shipData)
            {
                Name = fleet.Name;
                MissionNum = fleet.MissionNum;
                Member = new List<MemberData.Ship.Info>();
                Num = fleet.Num;
                ShipDataLastUpdated = shipData.LastUpdated;

                foreach (var it in fleet.Member)
                {
                    var ship = shipData.GetShip(it);
                    if (ship == null)
                        continue;
                    Member.Add(ship);
                }
            }

            public int CompareTo(Deck it)
            {
                return Num - it.Num;
            }

            public override string ToString()
            {
                if (MissionNum != 0)
                    return string.Format("{0}){1}(遠征中)", Num, Name);

                return string.Format("{0}){1}", Num, Name); ;
            }
        }

        Deck _selectedDeck;

        public Deck SelectedDeck
        {
            get { return _selectedDeck; }
            set
            {
                _selectedDeck = value;

                RegisterFleetsInfo();
            }
        }

        /// <summary>
        /// 装備アイテムイメージリスト
        /// </summary>
        public ImageList SlotItemIconImageList
        {
            set
            {
                pbSlotItem1.IconImageList = value;
                pbSlotItem2.IconImageList = value;
                pbSlotItem3.IconImageList = value;
                pbSlotItem4.IconImageList = value;
                pbSlotItem5.IconImageList = value;
                pbSlotItem6.IconImageList = value;
                _imgSlotItemIcon = value;
            }
        }
        ImageList _imgSlotItemIcon;

        public FleetList()
        {
            InitializeComponent();
            tableLayoutPanel1.DoubleBuffer(true);
            addHandler();
        }

#if DEBUG
        //* こいつらが長い
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            lblFleet1.Text = "千代田航改二|150";
            lblFleet1.BackColor = Color.AliceBlue;
            lblCond1.Text = "100";
            lblCond1.BackColor = Color.LightPink;

            lblHP1.Text = "100";
            lblHP1.BackColor = Color.Gold;
            pbSlotItem1.DrawDebugImage(4);

            lblFleet2.Text = "Верный|133";
            lblFleet2.BackColor = Color.AliceBlue;
            lblCond2.Text = "100";
            lblCond2.BackColor = Color.LightPink;
            lblHP2.Text = "100";
            lblHP2.BackColor = Color.Gold;
            pbSlotItem2.DrawDebugImage(3);

            lblFleet3.Text = "Bismarck drei|133";
            lblFleet3.BackColor = Color.AliceBlue;
            lblCond3.Text = "100";
            lblCond3.BackColor = Color.LightPink;
            lblHP3.Text = "100";
            lblHP3.BackColor = Color.Gold;
            pbSlotItem3.DrawDebugImage(4);

            lblFleet4.Text = "Bismarck zwei|135";
            lblFleet4.BackColor = Color.AliceBlue;
            lblCond4.Text = "100";
            lblCond4.BackColor = Color.LightPink;
            lblHP4.Text = "100";
            lblHP4.BackColor = Color.Gold;
            pbSlotItem4.DrawDebugImage(4);


            pbSlotItem5.ClearSlotItem();
        }
        //*/
#endif
        /// <summary>
        /// マウスクリックイベントを追加する
        /// </summary>
        void addHandler()
        {
            lblFleet1.MouseClick += wnd_MouseClick1;
            lblFleet2.MouseClick += wnd_MouseClick2;
            lblFleet3.MouseClick += wnd_MouseClick3;
            lblFleet4.MouseClick += wnd_MouseClick4;
            lblFleet5.MouseClick += wnd_MouseClick5;
            lblFleet6.MouseClick += wnd_MouseClick6;

            lblCond1.MouseClick += wnd_MouseClick1;
            lblCond2.MouseClick += wnd_MouseClick2;
            lblCond3.MouseClick += wnd_MouseClick3;
            lblCond4.MouseClick += wnd_MouseClick4;
            lblCond5.MouseClick += wnd_MouseClick5;
            lblCond6.MouseClick += wnd_MouseClick6;

            lblHP1.MouseClick += wnd_MouseClick1;
            lblHP2.MouseClick += wnd_MouseClick2;
            lblHP3.MouseClick += wnd_MouseClick3;
            lblHP4.MouseClick += wnd_MouseClick4;
            lblHP5.MouseClick += wnd_MouseClick5;
            lblHP6.MouseClick += wnd_MouseClick6;

            pbSlotItem1.MouseClick += wnd_MouseClick1;
            pbSlotItem2.MouseClick += wnd_MouseClick2;
            pbSlotItem3.MouseClick += wnd_MouseClick3;
            pbSlotItem4.MouseClick += wnd_MouseClick4;
            pbSlotItem5.MouseClick += wnd_MouseClick5;
            pbSlotItem6.MouseClick += wnd_MouseClick6;

        }

        void wnd_MouseClick1(object sender, MouseEventArgs e) { _showDetailCallDelegate(0); }
        void wnd_MouseClick2(object sender, MouseEventArgs e) { _showDetailCallDelegate(1); }
        void wnd_MouseClick3(object sender, MouseEventArgs e) { 
            _showDetailCallDelegate(2); }
        void wnd_MouseClick4(object sender, MouseEventArgs e) { _showDetailCallDelegate(3); }
        void wnd_MouseClick5(object sender, MouseEventArgs e) { _showDetailCallDelegate(4); }
        void wnd_MouseClick6(object sender, MouseEventArgs e) { _showDetailCallDelegate(5); }

        /// <summary>
        /// マウスクリックイベントでdelegateを呼び出す
        /// </summary>
        /// <param name="row"></param>
        void _showDetailCallDelegate(int row)
        {
            if (ShowDetail == null)
                return;

            if (_selectedDeck == null)
                return;

            if (_selectedDeck.Member.Count <= row)
                return;

            ShowDetail(_selectedDeck.Member[row]);
        }

        /// <summary>
        /// 装備アイテム一覧の画像を描画するPictureBox
        /// </summary>
        class SlotItemPictureBox : PictureBox
        {
            public SlotItemPictureBox()
            {
                SlotNum = 4;
            }

            /// <summary>
            /// 画像バッファを生成
            /// </summary>
            private void CreateImageBuffer()
            {
                if (Image != null)
                    Image.Dispose();

                //まるゆはスロット数0だから幅0のイメージリスト作成を試みて落ちるので+1
                Image = new Bitmap(16 * _slotNum + 1, 16);
                ClearSlotItem();
            }

            /// <summary>
            /// 描画に使うイメージリスト。16x16を想定
            /// </summary>
            public ImageList IconImageList { get; set; }

            /// <summary>
            /// 装備スロット数
            /// </summary>
            private int _slotNum;

            /// <summary>
            /// 装備スロット数を指定する
            /// </summary>
            public int SlotNum
            {
                get { return _slotNum; }
                set
                {
                    //装備スロット数が変化した時だけ再生成する
                    if (_slotNum != value)
                    {
                        _slotNum = value;
                        CreateImageBuffer();
                    }
                }
            }

            /// <summary>
            /// 装備スロットをクリアする
            /// </summary>
            public void ClearSlotItem()
            {
                Size = new Size(0, 0);
                using (Graphics g = Graphics.FromImage(Image))
                    g.FillRectangle(SystemBrushes.Control, new Rectangle(new Point(0, 0), Image.Size));

                Invalidate();
            }

#if DEBUG
            /// <summary>
            /// デバッグ用に適当な画像を描く
            /// </summary>
            /// <param name="slotNum"></param>
            public void DrawDebugImage(int slotNum)
            {
                Size = new Size(16 * slotNum, 16);
                using (Graphics g = Graphics.FromImage(Image))
                {
                    ///全消去
                    g.FillRectangle(SystemBrushes.Control, new Rectangle(new Point(0, 0), Image.Size));
                    for (int n = 0; n < _slotNum; n++)
                    {
                        if (n < slotNum && IconImageList != null)
                        {
                            IconImageList.Draw(g, new Point(15 * n, 0), n + 2);
                        }
                    }
                }
                Invalidate();
            }
#endif

            /// <summary>
            /// 装備スロットにアイテムを設定する
            /// </summary>
            /// <param name="slotItem">装備アイテム一覧</param>
            public void SetSlotItem(List<MemberData.Ship.Info.SlotItemInfo> slotItems)
            {
                StringBuilder sb = new StringBuilder();
                using (Graphics g = Graphics.FromImage(Image))
                {
                    ///全消去
                    g.FillRectangle(SystemBrushes.Control, new Rectangle(new Point(0, 0), Image.Size));

                    int AirSuperiorityAbility = 0;

                    for (int n = 0; n < _slotNum; n++)
                    {
                        string name = string.Format("装備{0}", n + 1);
                        string value = "(装備不可)";

                        //解放済みスロットについてのみ描画
                        if (n < slotItems.Count)
                        {
                            MemberData.Ship.Info.SlotItemInfo slotItem = slotItems[n];
                            int itemType = slotItem.TypeNum;
                            //装着されてるスロット かつ 装備情報読み込み済み
                            if (slotItem != null && itemType > 0 && itemType < IconImageList.Images.Count)
                            {
                                IconImageList.Draw(g, new Point(15 * n, 0), itemType);
                            }
                            else
                            {
                                IconImageList.Draw(g, new Point(15 * n, 0), 0);
                            }

                            //艦載機の場合
                            if (slotItem.Count > 0)
                            {
                                name = string.Format("装備{0} x{1}", n + 1, slotItem.Count);
                                AirSuperiorityAbility += slotItem.AirSuperiorityAbility;
                            }

                            if (slotItem.Info == null)
                            {
                                if (slotItem.ID == -1)
                                    value = "(未装備)";
                                else
                                    value = string.Format("装備ID:{0}", slotItem.ID);
                            }
                            else
                            {
                                if (slotItem.Level > 0)
                                    value = string.Format("({0})[{1}]{2}", slotItem.TypeName,slotItem.Level, slotItem.Name);
                                else
                                    value = string.Format("({0}){1}", slotItem.TypeName, slotItem.Name);
                            }

                            sb.AppendFormat("{0}: {1}\n", name, value);
                        }
                    }

                    if (AirSuperiorityAbility > 0)
                        sb.AppendFormat("\n制空値:{0}", AirSuperiorityAbility);
                }
                Size = new Size(16 * slotItems.Count, 16);

                //再描画
                Invalidate();

                ToolTipText = sb.ToString();
            }

            /// <summary>
            /// ツールチップに設定する文字列
            /// </summary>
            public string ToolTipText { get; private set; }

        }

        /// <summary>
        /// 補給状態をテキスト化するクラス
        /// </summary>
        class SupplyStatus
        {
            SupplyStatusFlags Flags = SupplyStatusFlags.None;

            public SupplyStatus(MemberData.Ship.Info info)
            {
                if (!info.Fuel.Full)
                {
                    Debug.WriteLine("Fuel not filled");
                    Flags |= SupplyStatusFlags.Fuel;
                }

                if (!info.Bullet.Full)
                {
                    Debug.WriteLine("Bullet not filled");
                    Flags |= SupplyStatusFlags.Bullet;
                }

                ToolTip = string.Format("\n燃料:{0}({1}%)\n弾薬:{2}({3}%)",
                info.Fuel, info.Fuel.SimplePercent, info.Bullet, info.Bullet.SimplePercent);
            }

            /// <summary>
            /// 表示用文字列
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                switch (Flags)
                {
                    case SupplyStatusFlags.None:
                        return "補給O";
                    case SupplyStatusFlags.Fuel:
                        return "燃料X";
                    case SupplyStatusFlags.Bullet:
                        return "弾薬X";
                    case SupplyStatusFlags.Both:
                        return "補給X";
                    default:
                        throw new InvalidEnumArgumentException();
                }
            }

            /// <summary>
            /// 背景色
            /// </summary>
            public Color BackgroundColor
            {
                get
                {
                    switch (Flags)
                    {
                        case SupplyStatusFlags.None:
                            return Color.LightGreen;
                        case SupplyStatusFlags.Fuel:
                            return Color.LightPink;
                        case SupplyStatusFlags.Bullet:
                            return Color.MistyRose;
                        case SupplyStatusFlags.Both:
                            return Color.HotPink;
                        default:
                            throw new InvalidEnumArgumentException();
                    }
                }
            }

            /// <summary>
            /// ツールチップに表示する文字列(\nから始まる)
            /// </summary>
            public string ToolTip { get; private set; }


            [Flags]
            enum SupplyStatusFlags
            {
                None = 0x00,
                Fuel = 0x01,
                Bullet = 0x02,
                Both = Bullet | Fuel,
            };
        }

        /// <summary>
        /// 艦娘情報をリストに登録する
        /// </summary>
        /// <param name="order">登録する場所</param>
        /// <param name="info">登録する情報</param>
        void SetFriendFleetValue(int order, MemberData.Ship.Info info)
        {
            Label lblName, lblCond, lblHP;
            SlotItemPictureBox pbSlotItem;

            #region コントロール選択
            switch (order)
            {
                case 0:
                    lblName = lblFleet1;
                    lblCond = lblCond1;
                    lblHP = lblHP1;
                    pbSlotItem = pbSlotItem1;
                    break;
                case 1:
                    lblName = lblFleet2;
                    lblCond = lblCond2;
                    lblHP = lblHP2;
                    pbSlotItem = pbSlotItem2;
                    break;
                case 2:
                    lblName = lblFleet3;
                    lblCond = lblCond3;
                    lblHP = lblHP3;
                    pbSlotItem = pbSlotItem3;
                    break;
                case 3:
                    lblName = lblFleet4;
                    lblCond = lblCond4;
                    lblHP = lblHP4;
                    pbSlotItem = pbSlotItem4;
                    break;
                case 4:
                    lblName = lblFleet5;
                    lblCond = lblCond5;
                    lblHP = lblHP5;
                    pbSlotItem = pbSlotItem5;
                    break;
                case 5:
                    lblName = lblFleet6;
                    lblCond = lblCond6;
                    lblHP = lblHP6;
                    pbSlotItem = pbSlotItem6;
                    break;
                default:
                    throw new IndexOutOfRangeException("Unknown order:" + order.ToString());
            }
            #endregion
            if (info == null)
            {
                lblName.Text = "　　";
                lblName.BackColor = SystemColors.Control;
                lblCond.Text = "  ";
                lblCond.BackColor = SystemColors.Control;
                lblHP.Text = "     ";
                lblHP.BackColor = SystemColors.Control;
                pbSlotItem.ClearSlotItem();

                toolTip1.SetToolTip(lblName, null);
                toolTip1.SetToolTip(lblCond, null);
                toolTip1.SetToolTip(pbSlotItem, null);
                toolTip1.SetToolTip(lblHP, null);

                return;
            }

            lblName.Text = string.Format("{0}|{1}", info.ShipName, info.Level);
            SupplyStatus supplyStatus = new SupplyStatus(info);


            lblName.BackColor = supplyStatus.BackgroundColor;

            lblCond.Text = info.Condition.ToString();
            lblCond.BackColor = info.ConditionColor;
            lblHP.Text = string.Format("{0}", info.HP.Percent);

            if (info.DockNum != 0)
                lblHP.BackColor = Color.Aquamarine;
            else
                lblHP.BackColor = info.HP.BackgroundColor;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} Lv.{1} Exp.{2}\n射程:{3} 速度:{4}",
                info.ShipName, info.Level, info.Experience, info.ShotRangeString, info.SpeedString);

            if (info.ExperienceRequiredToNextLevel > 0)
                sb.AppendFormat("\n次Lvまで{0}exp", info.ExperienceRequiredToNextLevel);
            if (info.UpdateLevel > 0)
            {
                if (info.ExperienceRequiredToUpgrade == 0)
                    sb.AppendFormat("\n改造Lv{0}に到達しています", info.UpdateLevel);
                else
                    sb.AppendFormat("\n改造Lv{0}まで{1}exp", info.UpdateLevel,
                        info.ExperienceRequiredToUpgrade);
            }

            sb.Append(supplyStatus.ToolTip);

            toolTip1.SetToolTip(lblName, sb.ToString());


            toolTip1.SetToolTip(lblCond, string.Format("コンディション:{0}%", info.Condition));

            sb.Clear();
            sb.AppendFormat("HP {0}({1})", info.HP, info.HP.Ratio.ToString("0.00%"));
            if (info.HP.Percent < 100)
                sb.AppendFormat("\n入渠所要 {0}", info.RepairTimeString);
            if (info.DockNum > 0)
                sb.AppendFormat("\n{0}に入渠中", info.DockName);

            toolTip1.SetToolTip(lblHP, sb.ToString());

            pbSlotItem.SlotNum = info.SlotNum;
            pbSlotItem.SetSlotItem(info.SlotItem);

            toolTip1.SetToolTip(pbSlotItem, pbSlotItem.ToolTipText);

            ResumeLayout();
        }

        /// <summary>
        /// 情報を描画する
        /// </summary>
        public void RegisterFleetsInfo()
        {
            if (_selectedDeck == null)
                return;

            ///指定した艦隊を登録する
            for (int n = 0; n < 6; n++)
            {
                if (_selectedDeck.Member.Count > n)
                {
                    MemberData.Ship.Info shipInfo = _selectedDeck.Member[n];
                    //艦隊メンバーが存在するとき
                    SetFriendFleetValue(n, shipInfo);
                }
                else
                {
                    ///メンバーが存在しない
                    SetFriendFleetValue(n, null);
                }

            }

        }
    }
}
