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
    public partial class FormSlotItemList : Form
    {
        public FormSlotItemList(ImageList iconImageList)
        {
            InitializeComponent();

            lvSlotItemList.DoubleBuffer(true);
            lvSlotItemList.ListViewItemSorter = Comparer<SlotItemLVItem>.Default;
            lvSlotItemList.LoadColumnWithOrder(Properties.Settings.Default.SlotItemListColumnWidth);
            if (!Properties.Settings.Default.SlotItemListBounds.IsEmpty)
                Bounds = Properties.Settings.Default.SlotItemListBounds;

            lvSlotItemList.SmallImageList = iconImageList;

            var sm = new SystemMenu(this);
            sm.InsertMenuItem(5, "ウィンドウ復帰", 6);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (SystemMenu.GetSysMenuId(m) == 5)
            {
                WindowState = FormWindowState.Normal;
                Bounds = new Rectangle(100, 100, 300, 300);
            }
        }

        /// <summary>
        /// 装備数上限
        /// </summary>
        public int MaxItem { get; set; }

        /// <summary>
        /// 装備情報の更新
        /// </summary>
        /// <param name="items"></param>
        public void UpdateSlotItemList(IEnumerable<MemberData.Item.Info> items)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => _updateSlotItemList(items)));
            else
                _updateSlotItemList(items);
        }

        void _updateSlotItemList(IEnumerable<MemberData.Item.Info> items)
        {
            ///内部情報の再構成
            _slotItemData.Clear();
            foreach (var it in items)
            {
                SlotItemInfo info;
                if (_slotItemData.ContainsKey(it.SlotItemType))
                {
                    info = _slotItemData[it.SlotItemType];
                    info.AddSlotItemOwner(it.Owner);
                    info.Count++;
                }
                else
                {
                    info = new SlotItemInfo(it);
                    _slotItemData[it.SlotItemType] = info;
                    info.Count = 1;
                }
            }

            ///ビューの更新
            lvSlotItemList.BeginUpdate();

            Dictionary<int, SlotItemLVItem> updateMap = new Dictionary<int, SlotItemLVItem>();
            foreach (SlotItemLVItem it in lvSlotItemList.Items)
            {
                updateMap[it.Info.ID] = it;
            }

            foreach (var it in _slotItemData)
            {
                if (updateMap.ContainsKey(it.Key))
                {
                    updateMap[it.Key].Update(it.Value);
                    updateMap.Remove(it.Key);
                }
                else
                {
                    lvSlotItemList.Items.Add(new SlotItemLVItem(it.Value));
                }
            }

            foreach (var it in updateMap.Values)
            {
                lvSlotItemList.Items.Remove(it);
            }

            updateFormTitle();

            lvSlotItemList.EndUpdate();

        }


        /// <summary>
        /// 装備オーナーの更新
        /// </summary>
        /// <param name="itemOwner"></param>
        public void UpdateSlotItemOwner(IDictionary<int, MemberData.Ship.SlotItemOwner> itemOwner,
            IDictionary<int, int> itemType)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => _updateSlotItemOwner(itemOwner, itemType)));
            else
                _updateSlotItemOwner(itemOwner, itemType);

        }

        void _updateSlotItemOwner(IDictionary<int, MemberData.Ship.SlotItemOwner> itemOwner,
            IDictionary<int, int> itemType)
        {
            ///装備種別一覧ループ
            foreach (var it in _slotItemData)
            {
                //装備所有者情報をクリア
                it.Value.ClearSlotItemOwner();

                ///装備一覧を舐める
                foreach (var it2 in itemOwner)
                {
                    ///装備種別が一致したら更新
                    
                    if (itemType.ContainsKey(it2.Key) && itemType[it2.Key] == it.Key)
                        it.Value.AddSlotItemOwner(it2.Value);
                }
            }

            lvSlotItemList.BeginUpdate();

            foreach (SlotItemLVItem it in lvSlotItemList.Items)
            {
                it.Update(_slotItemData[it.Info.ID]);
            }

            updateFormTitle();

            lvSlotItemList.EndUpdate();
        }

        void updateFormTitle()
        {
            int nTotal = 0;
            int nUsed = 0;

            foreach (var it in _slotItemData)
            {
                nTotal += it.Value.Count;
                nUsed += it.Value.UsingCount;
            }

            Text = string.Format("装備一覧 使用中:{0} {1}/{2}", nUsed, nTotal, MaxItem);

        }


        Dictionary<int, SlotItemInfo> _slotItemData = new Dictionary<int, SlotItemInfo>();

        /// <summary>
        /// 装備情報
        /// </summary>
        class SlotItemInfo
        {
            /// <summary>
            /// 装備種別ID
            /// </summary>
            public int ID { get; private set; }
            /// <summary>
            /// 装備種別名称
            /// </summary>
            public string Type { get; private set; }
            /// <summary>
            /// 装備種別num
            /// </summary>
            public int TypeNum { get; private set; }
            /// <summary>
            /// 装備名
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// 保有数
            /// </summary>
            public int Count { get; set; }

            public class Owner
            {
                /// <summary>
                /// 艦船ID
                /// </summary>
                public int ID { get; private set; }
                /// <summary>
                /// 名前
                /// </summary>
                public string Name { get; private set; }
                /// <summary>
                /// レベル
                /// </summary>
                public int Level { get; private set;}
                public Owner(MemberData.Ship.SlotItemOwner owner)
                {
                    ID = owner.ID;
                    Name = owner.Name;
                    Level = owner.Level;
                    Count = 1;
                }

                /// <summary>
                /// 装着数
                /// </summary>
                public int Count { get; set; }
            }

            /// <summary>
            /// 装備所有者一覧
            /// </summary>
            public List<Owner> ItemOwner { get; private set; }

            public SlotItemInfo(MemberData.Item.Info info)
            {
                ItemOwner = new List<Owner>();
                ID = info.SlotItemType;
                Name = info.Name;
                Type = info.Type;
                TypeNum = info.TypeNum;
                Count = 1;
                AddSlotItemOwner(info.Owner);
            }

            /// <summary>
            /// 装備所有者を追加。nullの場合は黙って無視する
            /// </summary>
            /// <param name="itemOwner"></param>
            public void AddSlotItemOwner(MemberData.Ship.SlotItemOwner itemOwner)
            {
                if (itemOwner != null)
                {
                    ///複数同種の装備をつけてる場合はかぶせる
                    foreach (var it in ItemOwner)
                    {
                        if (it.ID == itemOwner.ID)
                        {
                            it.Count++;
                            return;
                        }
                    }

                    //最初の装備
                    ItemOwner.Add(new Owner(itemOwner));
                }
            }

            public void ClearSlotItemOwner()
            {
                ItemOwner.Clear();
            }

            /// <summary>
            /// 使用してる装備数
            /// </summary>
            public int UsingCount
            {
                get
                {
                    int count = 0;
                    foreach (var it in ItemOwner)
                        count += it.Count;
                    return count;
                }
            }
        }

        /// <summary>
        /// リストビュー
        /// </summary>
        class SlotItemLVItem : ListViewItem, IComparable<SlotItemLVItem>
        {
            //初期ソートはアイテムID
            static private int _column = (int)ColumnIndex.ID;
            static private SortOrder _order = SortOrder.Ascending;

            /// <summary>
            /// ソートする対象カラム
            /// </summary>
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

            /// <summary>
            /// ソート順
            /// </summary>
            static public SortOrder Order { get { return _order; } }
            public enum ColumnIndex
            {
                ID = 0,
                種別 = 1,
                装備名 = 2,
                保有数 = 3,
                装着数 = 4,
                装備艦娘 = 5,
            }

            public SlotItemInfo Info { get; private set; }

            public SlotItemLVItem()
            {
                SubItems.Add("");
                SubItems.Add("");
                SubItems.Add("");
                SubItems.Add("");
                SubItems.Add("");
            }

            public SlotItemLVItem(SlotItemInfo info)
            {
                SubItems.Add("");
                SubItems.Add("");
                SubItems.Add("");
                SubItems.Add("");
                SubItems.Add("");

                SubItems[(int)ColumnIndex.ID].Text = info.ID.ToString();
                SubItems[(int)ColumnIndex.種別].Text = info.Type;
                SubItems[(int)ColumnIndex.装備名].Text = info.Name;

                Update(info);
            }

            public void Update(SlotItemInfo info)
            {
                SubItems[(int)ColumnIndex.保有数].Text = info.Count.ToString();
                SubItems[(int)ColumnIndex.装着数].Text = info.UsingCount.ToString();

                StringBuilder sb = new StringBuilder();
                foreach (var it in info.ItemOwner)
                {
                    if(it.Count > 1)
                        sb.AppendFormat("{0}(Lv{1})x{2} ", it.Name, it.Level,it.Count);
                    else
                        sb.AppendFormat("{0}(Lv{1}) ", it.Name, it.Level);
                }
                SubItems[(int)ColumnIndex.装備艦娘].Text = sb.ToString();

                ImageIndex = info.TypeNum;
                Info = info;

            }

            /// <summary>
            /// IComparableインタフェイスの実装
            /// </summary>
            /// <param name="it"></param>
            /// <returns></returns>
            public int CompareTo(SlotItemLVItem it)
            {
                int result = _compare(it.Info);

                if (_order == SortOrder.Descending)
                    result = -result;
                else if (_order == SortOrder.None)
                    result = 0;

                return result;
            }

            int _compare(SlotItemInfo info2)
            {
                switch ((ColumnIndex)_column)
                {
                    case ColumnIndex.ID:
                        return Info.ID - info2.ID;
                    case ColumnIndex.種別:
                        return Info.TypeNum - info2.TypeNum;
                    case ColumnIndex.装備名:
                        return string.Compare(Info.Name, info2.Name);
                    case ColumnIndex.保有数:
                        return Info.Count - info2.Count;
                    case ColumnIndex.装着数:
                        return Info.UsingCount - info2.UsingCount;
                    case ColumnIndex.装備艦娘:
                    default:
                        return 0;
                }

            }
        }

        private void FormSlotItemList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Visible = false;
                e.Cancel = true;
            }

            Properties.Settings.Default.SlotItemListColumnWidth = lvSlotItemList.SaveColumnWithOrder();

            if (WindowState == FormWindowState.Normal)
                Properties.Settings.Default.SlotItemListBounds = Bounds;
            else
                Properties.Settings.Default.SlotItemListBounds = RestoreBounds;
        }

        private void lvSlotItemList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SlotItemLVItem.Column = e.Column;
            lvSlotItemList.Sort();
            lvSlotItemList.SetSortIcon(e.Column, SlotItemLVItem.Order);
        }
    }
}
