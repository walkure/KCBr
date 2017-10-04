using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using KCB;

namespace KCB2
{
    public partial class FormShipList : Form
    {
        ImageList _ilSlotItem;

        public FormShipList(ImageList ilSlotItem)
        {
            _ilSlotItem = ilSlotItem;
            InitializeComponent();
            lvShipList.DoubleBuffer(true);
            //ウィンドウハンドルを生成しないと、Invokeで死ぬ
            IntPtr dummyHandle = Handle;

            ShipListViewItem.InitializeColumn(lvShipList);

            lvShipList.ListViewItemSorter = Comparer<ShipListViewItem>.Default;
            lvShipList.LoadColumnWithOrder(Properties.Settings.Default.ShipListColumnWidth);

            if(!Properties.Settings.Default.ShipListBounds.IsEmpty)
                Bounds = Properties.Settings.Default.ShipListBounds;

            var sm = new SystemMenu(this);
            sm.InsertMenuItem(1, "ウィンドウ復帰", 6);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (SystemMenu.GetSysMenuId(m) == 1)
            {
                WindowState = FormWindowState.Normal;
                Bounds = new Rectangle(100, 100, 300, 300);
            }
        }

        private void FormShipList_Load(object sender, EventArgs e)
        {
            /*
            lvShipList.ListViewItemSorter = Comparer<ShipListViewItem>.Default;
            lvShipList.LoadColumnWithOrder(Properties.Settings.Default.ShipListColumnWidth);
            Bounds = Properties.Settings.Default.ShipListBounds;
             */
        }

        private void FormShipList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Visible = false;
                e.Cancel = true;
            }

            Properties.Settings.Default.ShipListColumnWidth = lvShipList.SaveColumnWithOrder();

            if (WindowState == FormWindowState.Normal)
                Properties.Settings.Default.ShipListBounds = Bounds;
            else
                Properties.Settings.Default.ShipListBounds = RestoreBounds;

        }

        /// <summary>
        /// 艦娘一覧を更新
        /// </summary>
        /// <param name="shipList">艦娘一覧</param>
        /// <param name="maxShip">艦娘最大数</param>
        public void UpdateShipList(IEnumerable<MemberData.Ship.Info> shipList)
        {
            if(InvokeRequired)
                BeginInvoke((MethodInvoker)(() => updateShipListView(shipList)));
            else
                updateShipListView(shipList);
        }

        /// <summary>
        /// 最大艦娘数
        /// </summary>
        public int MaxShip { get; set; }

        void updateShipListView(IEnumerable<MemberData.Ship.Info> ships)
        {
            lvShipList.BeginUpdate();

            //現在登録されてるリスト内容をハッシュへ
            Dictionary<int, ShipListViewItem> updateMap = new Dictionary<int, ShipListViewItem>();
            foreach (ShipListViewItem it in lvShipList.Items)
            {
                updateMap[it.Info.ShipId] = it;
            }

            //表を更新/追加する
            foreach (MemberData.Ship.Info it in ships)
            {
                if (updateMap.ContainsKey(it.ShipId))
                {
                    //既に存在してる艦艇情報の更新をする
                    updateMap[it.ShipId].Update(it);
                    updateMap.Remove(it.ShipId);
                }
                else
                {
                    //なければ新規に追加
                    ShipListViewItem lvit = new ShipListViewItem(it,_ilSlotItem);
                    lvShipList.Items.Add(lvit);
                }
            }

            //消失した艦艇をリストから消す
            foreach (var it in updateMap.Values)
            {
                lvShipList.Items.Remove(it);
            }

            int used = 0, total= lvShipList.Items.Count;
            foreach (ShipListViewItem it in lvShipList.Items)
            {
                if (it.Info.Fleet != null && it.Info.Fleet.Length > 0 )
                    used++;
            }

            if (used == total)
                used = -1;

            Text = string.Format("艦娘一覧 艦隊:{0} {1}/{2}", used, total, MaxShip);

            lvShipList.EndUpdate();
        }

        /// <summary>
        /// 入渠情報を反映
        /// </summary>
        /// <param name="dockData"></param>
        public void UpdateShipListDock(MemberData.Dock dockData)
        {
            if(InvokeRequired)
                BeginInvoke((MethodInvoker)(() => updateDockData(dockData)));
            else
                updateDockData(dockData);

        }

        void updateDockData(MemberData.Dock dockData)
        {
            lvShipList.BeginUpdate();
            foreach (ShipListViewItem it in lvShipList.Items)
            {
                it.Info.UpdateNDock(dockData.GetNDockNum(it.Info.ShipId));
                it.UpdateItem(ShipListViewItem.ColumnIndex.DockName);

            }
            lvShipList.EndUpdate();
        }

        /// <summary>
        /// 艦隊情報を反映
        /// </summary>
        /// <param name="deckData"></param>
        public void UpdateShipListDeck(MemberData.Deck deckData)
        {
            if(InvokeRequired)
                BeginInvoke((MethodInvoker)(() => updateDeck(deckData)));
            else
                updateDeck(deckData);
        }

        void updateDeck(MemberData.Deck deckData)
        {
            lvShipList.BeginUpdate();
            int used = 0;
            foreach (ShipListViewItem it in lvShipList.Items)
            {
                var fld = deckData.GetShipDeckData(it.Info.ShipId);
                it.Info.UpdateDeckInfo(fld);
                it.UpdateItem(ShipListViewItem.ColumnIndex.Fleet);

                if (it.Info.Fleet != null && it.Info.Fleet.Length > 0)
                    used++;
            }

            Text = string.Format("艦娘一覧 艦隊:{0} {1}/{2}", used, lvShipList.Items.Count, MaxShip);
            lvShipList.EndUpdate();

        }

        /// <summary>
        /// 艦娘のロック状態を更新
        /// </summary>
        /// <param name="ship_id"></param>
        /// <param name="bLock"></param>
        public void UpdateShipLock(int ship_id,bool bLock)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => _updateShipLock(ship_id,bLock)));
            else
                _updateShipLock(ship_id, bLock);

        }

        void _updateShipLock(int ship_id,bool bLock)
        {
             lvShipList.BeginUpdate();
            foreach (ShipListViewItem it in lvShipList.Items)
            {
                if (it.Info.ShipId == ship_id)
                {
                    it.UpdateItem(ShipListViewItem.ColumnIndex.Locked);
                }
            }
            lvShipList.EndUpdate();
        }

        /// <summary>
        /// 装備情報を更新
        /// </summary>
        /// <param name="ship_id"></param>
        public void UpdateSlotItem(int ship_id)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => _updateSlotItem(ship_id)));
            else
                _updateSlotItem(ship_id);
        }

        void _updateSlotItem(int ship_id)
        {
            lvShipList.BeginUpdate();
            foreach (ShipListViewItem it in lvShipList.Items)
            {
                if (it.Info.ShipId == ship_id)
                {
                    it.UpdateItem(ShipListViewItem.ColumnIndex.SlotItems);
                }
            }
            lvShipList.EndUpdate();
        }

        /// <summary>
        /// リストビューアイテム
        /// </summary>
        class ShipListViewItem : ListViewItem, IComparable<ShipListViewItem>
        {
            //初期ソートは艦船ID
            static private int _column = (int)ColumnIndex.ShipId;
            static private SortOrder _order = SortOrder.Ascending;

            /// <summary>
            /// 最終更新時刻
            /// </summary>
            DateTime LastUpdated = DateTime.MinValue;

            static public int Column
            {
                set
                {
                    //カラムに変化がなかった
                    if (_column == value)
                    {
                        if (_order == SortOrder.Ascending)
                            _order = SortOrder.Descending;
                        else if (_order == SortOrder.Descending)
                            _order = SortOrder.Ascending;
                    }
                    else//カラムが変わったのでオーダーは保持しない
                        _order = SortOrder.Ascending;
                    _column = value;
                }
                get
                {
                    return _column;
                }
            }
            static public SortOrder Order { get { return _order; } }

            public MemberData.Ship.Info Info { get; private set; }

            ImageList _iconIageList;

            public enum ColumnIndex
            {
                ShipId = 0,
                ShipSortNo = 1,

                Fleet = 2,
                ShipName = 3,
                ShipNameYomi = 4,

                ShipType = 5,
                DockName = 6,
                RepairTime = 7,
                RepairSteel = 8,
                RepairFuel = 9,
                SlotItems = 10,

                Level = 11,
                UpdateLevel = 12,
                HP = 13,
                Experience = 14,
                RequiredExperience = 15,
                UpdateExperience = 16,

                Fuel = 17,
                Bullet = 18,
                Condition = 19,

                Escape = 20,
                Fire = 21,
                Luck = 22,
                Torpedo = 23,
                Search = 24,
                Armor = 25,
                AntiAir = 26,
                AntiSubm = 27,
                Speed = 28,
                Range = 29,
                Locked = 30,

                SallyArea = 31,
            }

            /// <summary>
            /// カラム名を返す
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            static string GetColumnTitle(ColumnIndex index)
            {
                switch (index)
                {
                    case ColumnIndex.ShipId:
                        return "艦娘ID";
                    case ColumnIndex.ShipSortNo:
                        return "艦娘No.";

                    case ColumnIndex.Fleet:
                        return "艦隊";
                    case ColumnIndex.ShipName:
                        return "艦名";
                    case ColumnIndex.ShipNameYomi:
                        return "艦名よみ";

                    case ColumnIndex.ShipType:
                        return "種別";
                    case ColumnIndex.DockName:
                        return "入渠";
                    case ColumnIndex.RepairTime:
                        return "修理時間";
                    case ColumnIndex.RepairSteel:
                        return "修理鋼材";
                    case ColumnIndex.RepairFuel:
                        return "修理燃料";
                    case ColumnIndex.SlotItems:
                        return "装備";

                    case ColumnIndex.Level:
                        return "Lv";
                    case ColumnIndex.UpdateLevel:
                        return "改造Lv";
                    case ColumnIndex.HP:
                        return "HP";
                    case ColumnIndex.Experience:
                        return "Exp.";
                    case ColumnIndex.RequiredExperience:
                        return "次LvまでのExp";
                    case ColumnIndex.UpdateExperience:
                        return "改造までのExp";

                    case ColumnIndex.Fuel:
                        return "燃料";
                    case ColumnIndex.Bullet:
                        return "弾薬";
                    case ColumnIndex.Condition:
                        return "疲労度";

                    case ColumnIndex.Escape:
                        return "回避";
                    case ColumnIndex.Fire:
                        return "火力";
                    case ColumnIndex.Luck:
                        return "運";
                    case ColumnIndex.Torpedo:
                        return "雷装";
                    case ColumnIndex.Search:
                        return "索敵";
                    case ColumnIndex.Armor:
                        return "装甲";
                    case ColumnIndex.AntiAir:
                        return "対空";
                    case ColumnIndex.AntiSubm:
                        return "対潜";
                    case ColumnIndex.Speed:
                        return "速度";
                    case ColumnIndex.Range:
                        return "射程";
                    case ColumnIndex.Locked:
                        return "ロック";
                    case ColumnIndex.SallyArea:
                        return "出撃海域";
                    default:
                        throw new ArgumentOutOfRangeException("Unknown ShipList index");

                }
            }

            /// <summary>
            /// リストビューへカラム名を設定する
            /// </summary>
            /// <param name="lvShip"></param>
            public static void InitializeColumn(System.Windows.Forms.ListView
                    lvShip)
            {
                foreach (ColumnIndex it in Enum.GetValues(typeof(ColumnIndex)))
                {
                    System.Windows.Forms.ColumnHeader col = new System.Windows.Forms.ColumnHeader();
                    col.Text = GetColumnTitle(it);
                    col.DisplayIndex = (int)it;
                    lvShip.Columns.Add(col);
                }
            }

            public ShipListViewItem(MemberData.Ship.Info info,ImageList iconImageList)
            {
                UseItemStyleForSubItems = false;
                _iconIageList = iconImageList;

                for (int n = 0; n < Enum.GetValues(typeof(ColumnIndex)).Length; n++)
                    SubItems.Add("");

                //GDIオブジェクトは再利用するようにしないと表示更新時にリソースが尽きて落ちる
                SubItems[(int)ColumnIndex.SlotItems] = new SlotItemsLVSubItem();

                Update(info);
            }

            /// <summary>
            /// 表示内容の更新
            /// </summary>
            /// <param name="newInfo"></param>
            /// <returns></returns>
            public void Update(MemberData.Ship.Info newInfo)
            {
                Info = newInfo;

                // データの更新がされていなければビューを更新しない
                if (LastUpdated >= Info.LastUpdated)
                    return;

                LastUpdated = Info.LastUpdated;

                SubItems[(int)ColumnIndex.ShipId].Text = Info.ShipId.ToString();
                SubItems[(int)ColumnIndex.ShipSortNo].Text = Info.ShipSortNo.ToString();

                if (Info.Fleet != null)
                    SubItems[(int)ColumnIndex.Fleet].Text = Info.Fleet.ToString();
                else
                    SubItems[(int)ColumnIndex.Fleet].Text = "";
                SubItems[(int)ColumnIndex.ShipName].Text = Info.ShipName;
                SubItems[(int)ColumnIndex.ShipNameYomi].Text = Info.ShipNameYomi;

                SubItems[(int)ColumnIndex.ShipType].Text = Info.ShipType;
                SubItems[(int)ColumnIndex.DockName].Text = Info.DockName;
                SubItems[(int)ColumnIndex.RepairTime].Text = Info.RepairTimeString;
                SubItems[(int)ColumnIndex.RepairSteel].Text = Info.RepairSteel.ToString();
                SubItems[(int)ColumnIndex.RepairFuel].Text = Info.RepairFuel.ToString();

                SubItems[(int)ColumnIndex.Level].Text = Info.Level.ToString();
                SubItems[(int)ColumnIndex.Level].BackColor = Info.LevelBackgroundColor;

                SubItems[(int)ColumnIndex.UpdateLevel].Text = Info.UpdateLevel.ToString();

                SubItems[(int)ColumnIndex.HP].Text = Info.HP.ToString();
                SubItems[(int)ColumnIndex.HP].BackColor = Info.HP.BackgroundColor;

                SubItems[(int)ColumnIndex.Experience].Text = Info.Experience.ToString();
                SubItems[(int)ColumnIndex.RequiredExperience].Text
                    = Info.ExperienceRequiredToNextLevel.ToString();

                SubItems[(int)ColumnIndex.UpdateExperience].Text = Info.ExperienceRequiredToUpgrade.ToString();

                SubItems[(int)ColumnIndex.Fuel].Text = Info.Fuel.ToString();
                SubItems[(int)ColumnIndex.Fuel].BackColor = Info.Fuel.BackgroundColor;

                SubItems[(int)ColumnIndex.Bullet].Text = Info.Bullet.ToString();
                SubItems[(int)ColumnIndex.Bullet].BackColor = Info.Bullet.BackgroundColor;

                SubItems[(int)ColumnIndex.Condition].Text = Info.Condition.ToString();
                SubItems[(int)ColumnIndex.Condition].BackColor = Info.ConditionColor;

                SubItems[(int)ColumnIndex.Escape].Text = Info.Escape.ToString();
                SubItems[(int)ColumnIndex.Fire].Text = Info.Fire.ToString();
                SubItems[(int)ColumnIndex.Fire].BackColor = GetUpgradedParameterColor(ColumnIndex.Fire,
                    Info.Fire);
                SubItems[(int)ColumnIndex.Luck].Text = Info.Lucky.ToString();
                SubItems[(int)ColumnIndex.Torpedo].Text = Info.Torpedo.ToString();
                SubItems[(int)ColumnIndex.Torpedo].BackColor = GetUpgradedParameterColor(ColumnIndex.Torpedo,
                    Info.Torpedo);
                SubItems[(int)ColumnIndex.Search].Text = Info.Search.ToString();
                SubItems[(int)ColumnIndex.Armor].Text = Info.Armor.ToString();
                SubItems[(int)ColumnIndex.Armor].BackColor = GetUpgradedParameterColor(ColumnIndex.Armor,
                    Info.Armor);
                SubItems[(int)ColumnIndex.AntiAir].Text = Info.AntiAir.ToString();
                SubItems[(int)ColumnIndex.AntiAir].BackColor = GetUpgradedParameterColor(ColumnIndex.AntiAir,
                    Info.AntiAir);
                SubItems[(int)ColumnIndex.AntiSubm].Text = Info.AntiSubm.ToString();
                SubItems[(int)ColumnIndex.Speed].Text = Info.SpeedString;
                SubItems[(int)ColumnIndex.Range].Text = Info.ShotRangeString;

                SubItems[(int)ColumnIndex.Locked].Text = Info.Locked.ToString();
                SubItems[(int)ColumnIndex.SallyArea].Text = Info.SallyArea.ToString();

                ((SlotItemsLVSubItem)SubItems[(int)ColumnIndex.SlotItems]).Update(Info.SlotItem,Info.SlotNum, _iconIageList);

            }

            /// <summary>
            /// 指定したアイテムの再描画
            /// </summary>
            /// <param name="index"></param>
            public void UpdateItem(ColumnIndex index)
            {
                switch (index)
                {
                    case ColumnIndex.DockName:
                        SubItems[(int)ColumnIndex.DockName].Text = Info.DockName;
                        break;
                    case ColumnIndex.Fleet:
                        if (Info.Fleet != null)
                            SubItems[(int)ColumnIndex.Fleet].Text = Info.Fleet.ToString();
                        else
                            SubItems[(int)ColumnIndex.Fleet].Text = "";
                        break;
                    case ColumnIndex.HP:
                        SubItems[(int)ColumnIndex.HP].Text = Info.HP.ToString();
                        SubItems[(int)ColumnIndex.HP].BackColor = Info.HP.BackgroundColor;

                        //HPが変わると修理時間が発生/変化する
                        SubItems[(int)ColumnIndex.RepairTime].Text = Info.RepairTimeString;
                        SubItems[(int)ColumnIndex.RepairSteel].Text = Info.RepairSteel.ToString();
                        SubItems[(int)ColumnIndex.RepairFuel].Text = Info.RepairFuel.ToString();

                        break;
                    case ColumnIndex.Locked:
                        SubItems[(int)ColumnIndex.Locked].Text = Info.Locked.ToString();
                        break;

                    case ColumnIndex.SlotItems:
                        ((SlotItemsLVSubItem)SubItems[(int)ColumnIndex.SlotItems]).Update(Info.SlotItem, Info.SlotNum, _iconIageList);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("Not supported index:" + index);
                }
            }

            Color GetUpgradedParameterColor(ColumnIndex type, MemberData.Ship.Info.NowMax value)
            {
                if (value.Full)
                {
                    switch (type)
                    {
                        case ColumnIndex.Fire:
                            return Color.LightPink;
                        case ColumnIndex.Torpedo:
                            return Color.LightSkyBlue;
                        case ColumnIndex.AntiAir:
                            return Color.Gold;
                        case ColumnIndex.Armor:
                            return Color.Khaki;
                    }
                }
                return SystemColors.Window;
            }

            /// <summary>
            /// IComparable<ShipListViewItem> の実装
            /// </summary>
            /// <param name="it">比較対象</param>
            /// <returns> -:引数より前 0:同一 +:引数より後 </returns>
            public int CompareTo(ShipListViewItem it)
            {
                if (it == null || Info == null)
                    return 0;

                int result = compare(it.Info);

                if (_order == SortOrder.Descending)
                    result = -result;
                else if (_order == SortOrder.None)
                    result = 0;

                return result;
            }
            /// <summary>
            /// IComparable<ShipListViewItem> の魂の実装
            /// </summary>
            /// <param name="it2"></param>
            /// <returns></returns>
            private int compare(MemberData.Ship.Info it2)
            {
                switch (_column)
                {
                    case (int)ColumnIndex.ShipId:
                        return Info.ShipId - it2.ShipId;
                    case (int)ColumnIndex.ShipSortNo:
                        return Info.ShipSortNo - it2.ShipSortNo;

                    case (int)ColumnIndex.Fleet:
                        String ft1, ft2;
                        if (Info.Fleet == null)
                            ft1 = "";
                        else
                            ft1 = Info.Fleet.ToString();

                        if (it2.Fleet == null)
                            ft2 = "";
                        else
                            ft2 = it2.Fleet.ToString();

                        return string.Compare(ft1, ft2);
                    case (int)ColumnIndex.ShipName:
                        return string.Compare(Info.ShipName, it2.ShipName);
                    case (int)ColumnIndex.ShipNameYomi:
                        return string.Compare(Info.ShipNameYomi, it2.ShipNameYomi);

                    case (int)ColumnIndex.ShipType:
                        return string.Compare(Info.ShipType, it2.ShipType);
                    case (int)ColumnIndex.DockName:
                        return Info.DockNum - it2.DockNum;
                    case (int)ColumnIndex.RepairTime:
                        return TimeSpan.Compare(Info.RepairTime, it2.RepairTime);
                    case(int)ColumnIndex.RepairSteel:
                        return Info.RepairSteel - it2.RepairSteel;
                    case (int)ColumnIndex.RepairFuel:
                        return Info.RepairFuel - it2.RepairFuel;

                    case (int)ColumnIndex.SlotItems:
                        return Info.SlotItem.Count - it2.SlotItem.Count;

                    case (int)ColumnIndex.Level:
                        return Info.Level - it2.Level;
                    case (int)ColumnIndex.UpdateLevel:
                        return Info.UpdateLevel - it2.UpdateLevel;
                    case (int)ColumnIndex.HP:
                        return Info.HP.Max - it2.HP.Max;
                    case (int)ColumnIndex.Experience:
                        return Info.Experience - it2.Experience;
                    case (int)ColumnIndex.RequiredExperience:
                        return Info.ExperienceRequiredToNextLevel - it2.ExperienceRequiredToNextLevel;
                    case (int)ColumnIndex.UpdateExperience:
                        return Info.ExperienceRequiredToUpgrade - it2.ExperienceRequiredToUpgrade;

                    case (int)ColumnIndex.Fuel:
                        return Info.Fuel.Max - it2.Fuel.Max;
                    case (int)ColumnIndex.Bullet:
                        return Info.Bullet.Max - it2.Bullet.Max;
                    case (int)ColumnIndex.Condition:
                        return Info.Condition - it2.Condition;

                    case (int)ColumnIndex.Escape:
                        return Info.Escape.Now - it2.Escape.Now;

                    case (int)ColumnIndex.Fire:
                        return Info.Fire.CompareTo(it2.Fire);
                    case (int)ColumnIndex.Torpedo:
                        return Info.Torpedo.CompareTo(it2.Torpedo);
                    case (int)ColumnIndex.Armor:
                        return Info.Armor.CompareTo(it2.Armor);
                    case (int)ColumnIndex.AntiAir:
                        return Info.AntiAir.CompareTo(it2.AntiAir);

                    case (int)ColumnIndex.Search:
                        return Info.Search.Now - it2.Search.Now;
                    case (int)ColumnIndex.AntiSubm:
                        return Info.AntiSubm.Now - it2.AntiSubm.Now;
                    case (int)ColumnIndex.Speed:
                        return Info.Speed - it2.Speed;
                    case (int)ColumnIndex.Range:
                        return Info.ShotRange - it2.ShotRange;
                    case (int)ColumnIndex.Locked:
                        return (Info.Locked ? 1 : 0) - (it2.Locked ? 1 : 0);
                    case (int)ColumnIndex.Luck:
                        return Info.Lucky.Now - it2.Lucky.Now;
                    case (int)ColumnIndex.SallyArea:
                        return Info.SallyArea - it2.SallyArea;
                    default:
                        System.Diagnostics.Debug.WriteLine("ShipListViewItem compare unknown column" + _column.ToString());
                        throw new NotImplementedException("ShipListViewItem compare unknown column" + _column.ToString());
                }

            }

            /// <summary>
            /// 装備一覧を描画するサブアイテム
            /// </summary>
            class SlotItemsLVSubItem : ListViewItem.ListViewSubItem, IOwnerDrawLVSubItem
            {
                Image _slotItemImage,_slotImageSelected;

                public SlotItemsLVSubItem() : this(null,4,null) {}

                public SlotItemsLVSubItem(List<MemberData.Ship.Info.SlotItemInfo> slotItems,
                    int slotNum,ImageList iconImageList)
                {
                    _slotItemImage = new Bitmap(16 * slotNum + 1, 16);
                    _slotImageSelected = new Bitmap(16 * slotNum + 1, 16);

                    if (slotItems != null && iconImageList != null)
                        Update(slotItems,slotNum, iconImageList);
                    else
                    {
                        _drawBackground(_slotItemImage,SystemBrushes.Window);
                        _drawBackground(_slotImageSelected, SystemBrushes.Highlight);
                    }
                }

                /// <summary>
                /// 背景色で塗りつぶす
                /// </summary>
                /// <param name="image">対象イメージ</param>
                /// <param name="bkBrush">背景ブラシ</param>
                void _drawBackground(Image image, Brush bkBrush)
                {
                    using (Graphics g = Graphics.FromImage(image))
                        g.FillRectangle(bkBrush, 0, 0, image.Width, image.Height);

                }

                /// <summary>
                /// 装備情報を更新
                /// </summary>
                /// <param name="slotItems"></param>
                /// <param name="iconImageList"></param>
                public void Update(List<MemberData.Ship.Info.SlotItemInfo> slotItems,
                    int slotNum,ImageList iconImageList)
                {
                    DrawSlotItemsImage(slotItems, iconImageList, _slotItemImage, SystemBrushes.Window);
                    DrawSlotItemsImage(slotItems, iconImageList, _slotImageSelected, SystemBrushes.Highlight);

                    StringBuilder sb = new StringBuilder();
                    int AirSuperiorityAbility = 0;
                    for (int n = 0; n < slotNum; n++)
                    {
                        string name = string.Format("装備{0}", n + 1);
                        string value = "(装備不可)";

                        if (n < slotItems.Count)
                        {
                            var slotItem = slotItems[n];
                            //艦載機の場合
                            if (slotItem.Count > 0)
                            {
                                name = string.Format("装備{0} x{1}", n + 1, slotItem.Count);
                                AirSuperiorityAbility += slotItem.AirSuperiorityAbility;
                            }

                            if (slotItem.Info == null)
                                value = "(未装備)";
                            else
                                value = string.Format("({0}){1}", slotItem.TypeName, slotItem.Name);

                        }
                        sb.AppendFormat("{0}: {1} ", name, value);

                    }
                    if (AirSuperiorityAbility > 0)
                        sb.AppendFormat(" 制空値:{0}", AirSuperiorityAbility);

                    Text = sb.ToString();

                }

                /// <summary>
                /// 対象イメージに装備一覧画像を書き込む
                /// </summary>
                /// <param name="slotItems">装備情報</param>
                /// <param name="iconImageList">装備アイコンのイメージリスト</param>
                /// <param name="canvas">対象イメージ</param>
                /// <param name="backBrush">背景色</param>
                void DrawSlotItemsImage(List<MemberData.Ship.Info.SlotItemInfo> slotItems,
                    ImageList iconImageList, Image canvas,Brush backBrush)
                {
                    using (Graphics g = Graphics.FromImage(canvas))
                    {
                        g.FillRectangle(backBrush,
                            new Rectangle(new Point(0, 0), canvas.Size));

                        for (int n = 0; n < 4; n++)
                        {
                            //解放済みスロットについてのみ描画
                            if (n < slotItems.Count)
                            {
                                MemberData.Ship.Info.SlotItemInfo slotItem = slotItems[n];
                                int itemType = slotItem.TypeNum;
                                //装着されてるスロット かつ 装備情報読み込み済み
                                if (slotItem != null && itemType > 0 && itemType < iconImageList.Images.Count)
                                    iconImageList.Draw(g, new Point(15 * n + 1, 0), itemType);
                                else
                                    iconImageList.Draw(g, new Point(15 * n + 1, 0), 0);

                            }
                        }
                    }
                }

                /// <summary>
                /// サブアイテム描画ハンドラ
                /// </summary>
                /// <param name="e">描画イベントパラメタ</param>
                public void DrawSubItem(DrawListViewSubItemEventArgs e)
                {
                    e.Graphics.SetClip(e.Bounds);
                    if (e.ItemState.HasFlag(ListViewItemStates.Selected))
                    {
                        e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                        e.Graphics.DrawImage(_slotImageSelected, new Point(e.Bounds.X, e.Bounds.Y));
                    }
                    else
                    {
                        e.DrawBackground();
                        e.Graphics.DrawImage(_slotItemImage, new Point(e.Bounds.X, e.Bounds.Y));
                    }
                }
            }
        }

        //ソートハンドラ
        void lvShipList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ShipListViewItem.Column = e.Column;
            lvShipList.Sort();
            lvShipList.SetSortIcon(e.Column, ShipListViewItem.Order);
        }


    }
}
