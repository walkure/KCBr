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
    public partial class FormItemList : Form
    {

        public FormItemList(ImageList iconImageList)
        {
            InitializeComponent();
            //ウィンドウハンドルを生成しないと、Invokeで死ぬ
            IntPtr dummyHandle = Handle;
            lvItemList.DoubleBuffer(true);
            ItemListViewItem.InitializeColumn(lvItemList);

            lvItemList.ListViewItemSorter = Comparer<ItemListViewItem>.Default;
            lvItemList.LoadColumnWithOrder(Properties.Settings.Default.ItemListColumnWidth);

            if (!Properties.Settings.Default.ItemListBounds.IsEmpty)
                Bounds = Properties.Settings.Default.ItemListBounds;

            lvItemList.SmallImageList = iconImageList;
            var sm = new SystemMenu(this);
            sm.InsertMenuItem(4, "ウィンドウ復帰", 6);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (SystemMenu.GetSysMenuId(m) == 4)
            {
                WindowState = FormWindowState.Normal;
                Bounds = new Rectangle(100, 100, 300, 300);
            }
        }

        public void UpdateItemList(IEnumerable<MemberData.Item.Info> items)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => updateItemListView(items)));
            else
                updateItemListView(items);
        }

        void updateItemListView(IEnumerable<MemberData.Item.Info> items)
        {
            lvItemList.BeginUpdate();

            //現在登録されてるリスト内容をハッシュへ
            Dictionary<int, ItemListViewItem> updateMap = new Dictionary<int, ItemListViewItem>();
            foreach (ItemListViewItem it in lvItemList.Items)
            {
                updateMap[it.Info.ItemUID] = it;
            }

            //表をする
            foreach (MemberData.Item.Info it in items)
            {
                if (updateMap.ContainsKey(it.ItemUID))
                {
                    //既に存在してるアイテム情報の更新をする
                    updateMap[it.ItemUID].Update(it);
                    updateMap.Remove(it.ItemUID);
                }
                else
                {
                    //なければ新規に追加
                    ItemListViewItem lvit = new ItemListViewItem(it);
                    lvItemList.Items.Add(lvit);
                }
            }

            //消失したアイテムをリストから消す
            foreach (var it in updateMap.Values)
            {
                lvItemList.Items.Remove(it);
            }

            updateFormTitle();

            lvItemList.EndUpdate();

        }

        public void UpdateItemOwner(IDictionary<int, MemberData.Ship.SlotItemOwner> itemOwner)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => updateItemOwnerListView(itemOwner)));
            else
                updateItemOwnerListView(itemOwner);
        }

        void updateItemOwnerListView(IDictionary<int, MemberData.Ship.SlotItemOwner> itemOwner)
        {
            lvItemList.BeginUpdate();

            foreach (ItemListViewItem it in lvItemList.Items)
            {
                if (itemOwner.ContainsKey(it.Info.ItemUID))
                {
                    it.Info.Owner = itemOwner[it.Info.ItemUID];
                    it.SubItems[(int)ItemListViewItem.ColumnIndex.Owner].Text = it.Info.OwnerString;
                }
                else
                {
                    it.Info.Owner = null;
                    it.SubItems[(int)ItemListViewItem.ColumnIndex.Owner].Text = "";
                }
            }

            updateFormTitle();
            lvItemList.EndUpdate();

        }

        public int MaxItem { get; set; }
        void updateFormTitle()
        {
            int nTotal = lvItemList.Items.Count;
            int nUsed = 0;

            foreach (ItemListViewItem it in lvItemList.Items)
                if (it != null && it.Info.Owner != null)
                    nUsed++;

            Text = string.Format("装備一覧 使用中:{0} {1}/{2}", nUsed, nTotal, MaxItem);

        }

        private void FormItemList_Load(object sender, EventArgs e)
        {
            /*
            lvItemList.ListViewItemSorter = Comparer<ItemListViewItem>.Default;
            lvItemList.LoadColumnWithOrder(Properties.Settings.Default.ItemListColumnWidth);
            Bounds = Properties.Settings.Default.ItemListBounds;
             */
        }

        private void FormItemList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Visible = false;
                e.Cancel = true;
            }

            Properties.Settings.Default.ItemListColumnWidth = lvItemList.SaveColumnWithOrder();

            if (WindowState == FormWindowState.Normal)
                Properties.Settings.Default.ItemListBounds = Bounds;
            else
                Properties.Settings.Default.ItemListBounds = RestoreBounds;
        }

        private void lvItemList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ItemListViewItem.Column = e.Column;
            lvItemList.Sort();
            lvItemList.SetSortIcon(e.Column, ItemListViewItem.Order);
        }

        public void UpdateLockState(int item_id, bool bLock)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => _updateLockState(item_id,bLock)));
            else
                _updateLockState(item_id,bLock);
        }

        void _updateLockState(int item_id, bool bLock)
        {
            lvItemList.BeginUpdate();

            foreach (ItemListViewItem it in lvItemList.Items)
            {
                if (it.Info.ItemUID == item_id)
                {
                    it.UpdateItem(ItemListViewItem.ColumnIndex.Locked);
                }
            }

            lvItemList.EndUpdate();
        }

        /// <summary>
        /// リストビューアイテム
        /// </summary>
        class ItemListViewItem : ListViewItem, IComparable<ItemListViewItem>
        {
            //初期ソートはアイテムID
            static private int _column = (int)ColumnIndex.Id;
            static private SortOrder _order = SortOrder.Ascending;

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

            public MemberData.Item.Info Info { get; private set; }
            public ItemListViewItem(MemberData.Item.Info info)
            {
                UseItemStyleForSubItems = false;

                for (int n = 0; n < Enum.GetValues(typeof(ColumnIndex)).Length; n++)
                    SubItems.Add("");

                Update(info);

            }


            /// <summary>
            /// リストビューへカラム名を設定する
            /// </summary>
            /// <param name="lvShip"></param>
            public static void InitializeColumn(System.Windows.Forms.ListView
                    lvItemList)
            {
                foreach (ColumnIndex it in Enum.GetValues(typeof(ColumnIndex)))
                {
                    System.Windows.Forms.ColumnHeader col = new System.Windows.Forms.ColumnHeader();
                    col.Text = GetColumnTitle(it);
                    col.DisplayIndex = (int)it;
                    lvItemList.Columns.Add(col);
                }
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
                    case ColumnIndex.Id:
                        return "ID";
                    case ColumnIndex.Owner:
                        return "装備艦娘";
                    case ColumnIndex.Name:
                        return "名称";
                    case ColumnIndex.Type:
                        return "種類";
                    case ColumnIndex.対空:
                        return "対空";
                    case ColumnIndex.装甲:
                        return "装甲";
                    case ColumnIndex.火力:
                        return "火力";
                    case ColumnIndex.雷撃:
                        return "雷撃";
                    case ColumnIndex.回避:
                        return "回避";
                    case ColumnIndex.Soku:
                        return "Soku";
                    case ColumnIndex.爆装:
                        return "爆装";
                    case ColumnIndex.耐久:
                        return "耐久";
                    case ColumnIndex.対潜:
                        return "対潜";
                    case ColumnIndex.命中:
                        return "命中";
                    case ColumnIndex.Houk:
                        return "Houk";
                    case ColumnIndex.索敵:
                        return "索敵";
                    case ColumnIndex.運:
                        return "運";
                    case ColumnIndex.射程:
                        return "射程";
                    case ColumnIndex.Rarelity:
                        return "レア";
                    case ColumnIndex.Locked:
                        return "ロック";
                    case ColumnIndex.Level:
                        return "レベル";
                    default:
                        throw new ArgumentOutOfRangeException("Unknown column Index");
                }
            }

            public enum ColumnIndex
            {
                Id = 0,
                Owner = 1,
                Name = 2,
                Type = 3,
                Level = 4,
                対空 = 5,
                装甲 = 6,
                火力 = 7,
                雷撃 = 8,
                回避 = 9,
                Soku = 10,
                爆装 = 11,
                耐久 = 12,
                対潜 = 13,
                命中 = 14,
                Houk = 15,
                索敵 = 16,
                運 = 17,
                射程 = 18,
                Rarelity = 19,
                Locked = 20,
            }

            public void Update(MemberData.Item.Info _info)
            {
                Info = _info;
                SubItems[(int)ColumnIndex.Id].Text = _info.ItemUID.ToString();
                SubItems[(int)ColumnIndex.Owner].Text = _info.OwnerString;
                SubItems[(int)ColumnIndex.Name].Text = _info.Name;
                SubItems[(int)ColumnIndex.Type].Text = _info.Type;
                SubItems[(int)ColumnIndex.対空].Text = _info.対空.ToString();
                SubItems[(int)ColumnIndex.装甲].Text = _info.装甲.ToString();
                SubItems[(int)ColumnIndex.火力].Text = _info.火力.ToString();
                SubItems[(int)ColumnIndex.雷撃].Text = _info.雷撃.ToString();
                SubItems[(int)ColumnIndex.回避].Text = _info.砲撃回避.ToString();
                SubItems[(int)ColumnIndex.Soku].Text = _info.Soku.ToString();
                SubItems[(int)ColumnIndex.爆装].Text = _info.爆装.ToString();
                SubItems[(int)ColumnIndex.耐久].Text = _info.耐久.ToString();
                SubItems[(int)ColumnIndex.対潜].Text = _info.対潜.ToString();
                SubItems[(int)ColumnIndex.命中].Text = _info.砲撃命中.ToString();
                SubItems[(int)ColumnIndex.Houk].Text = _info.Houk.ToString();
                SubItems[(int)ColumnIndex.索敵].Text = _info.索敵.ToString();
                SubItems[(int)ColumnIndex.運].Text = _info.運.ToString();
                SubItems[(int)ColumnIndex.射程].Text = _info.射程String;
                SubItems[(int)ColumnIndex.Rarelity].Text = _info.RareString;
                SubItems[(int)ColumnIndex.Locked].Text = _info.Locked.ToString();
                SubItems[(int)ColumnIndex.Level].Text = _info.Level.ToString();


                ImageIndex = _info.TypeNum;
            }

            /// <summary>
            /// 指定したカラムを更新
            /// </summary>
            /// <param name="index"></param>
            public void UpdateItem(ColumnIndex index)
            {
                switch (index)
                {
                    case ColumnIndex.Locked:
                        SubItems[(int)ColumnIndex.Locked].Text = Info.Locked.ToString();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("unknown update index");
                }
            }

            /// <summary>
            /// IComparable<ShipListViewItem> の実装
            /// </summary>
            /// <param name="it">比較対象</param>
            /// <returns> -:引数より前 0:同一 +:引数より後 </returns>
            public int CompareTo(ItemListViewItem it)
            {
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
            private int compare(MemberData.Item.Info it2)
            {
                switch (_column)
                {
                    case (int)ColumnIndex.Houk:
                        return Info.Houk - it2.Houk;
                    case (int)ColumnIndex.Id:
                        return Info.ItemUID - it2.ItemUID;
                    case (int)ColumnIndex.Name:
                        return string.Compare(Info.Name, it2.Name);
                    case (int)ColumnIndex.Owner:
                        return string.Compare(Info.OwnerString, it2.OwnerString);
                    case (int)ColumnIndex.Rarelity:
                        return Info.Rare - it2.Rare;
                    case (int)ColumnIndex.Soku:
                        return Info.Soku - it2.Soku;
                    case (int)ColumnIndex.Type:
                        return Info.TypeNum - it2.TypeNum;
                    case (int)ColumnIndex.運:
                        return Info.運 - it2.運;
                    case (int)ColumnIndex.火力:
                        return Info.火力 - it2.火力;
                    case (int)ColumnIndex.索敵:
                        return Info.索敵 - it2.索敵;
                    case (int)ColumnIndex.回避:
                        return Info.砲撃回避 - it2.砲撃回避;
                    case (int)ColumnIndex.射程:
                        return Info.射程 - it2.射程;
                    case (int)ColumnIndex.装甲:
                        return Info.装甲 - it2.装甲;
                    case (int)ColumnIndex.対空:
                        return Info.対空 - it2.対空;
                    case (int)ColumnIndex.対潜:
                        return Info.対潜 - it2.対潜;
                    case (int)ColumnIndex.耐久:
                        return Info.耐久 - it2.耐久;
                    case (int)ColumnIndex.爆装:
                        return Info.爆装 - it2.爆装;
                    case (int)ColumnIndex.命中:
                        return Info.砲撃命中 - it2.砲撃命中;
                    case (int)ColumnIndex.雷撃:
                        return Info.雷撃 - it2.雷撃;
                    case (int)ColumnIndex.Locked:
                        return (Info.Locked ? 1 : 0) - (it2.Locked ? 1 : 0);
                    case (int)ColumnIndex.Level:
                        return Info.Level - it2.Level;
                    default:
                        System.Diagnostics.Debug.WriteLine("ItemListViewItem compare unknown column" + _column.ToString());
                        throw new NotImplementedException("ItemListViewItem compare unknown column" + _column.ToString());
                }
            }
        }
    }
}
