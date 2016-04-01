using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace KCB2.MasterData
{
    /// <summary>
    /// 装備のマスタデータ
    /// </summary>
    public class Item
    {
        ConcurrentDictionary<int, Param> _itemMaster = new ConcurrentDictionary<int, Param>();
        List<SlotItemMasterLVItem> _itemList = new List<SlotItemMasterLVItem>();
        static bool _bUseMasterData = Properties.Settings.Default.UseMasterDataView;

        /// <summary>
        /// 装備マスタ情報読み込み
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool LoadItemMaster(List<KCB.api_start2.ApiData.ApiMstSlotitem> api_mst_slotitem)
        {
            _itemMaster.Clear();
            if (_bUseMasterData)
                _itemList.Clear();
            //                foreach (dynamic item in (object[])json.api_data)
            foreach (var item in api_mst_slotitem)
            {
                _itemMaster[(int)item.api_id] = new Param(item);

                if (_bUseMasterData)
                    _itemList.Add(new SlotItemMasterLVItem(item));

                //                    Debug.WriteLine(_itemMaster[(int)item.api_id]);
            }


            return true;
        }

        /// <summary>
        /// 装備マスタから装備情報を取得
        /// </summary>
        /// <param name="item_id">装備ID</param>
        /// <returns></returns>
        public Param GetItemParam(int item_id)
        {
            Param ret;
            if (_itemMaster.TryGetValue(item_id, out ret))
                return ret;
            return null;
        }

        /// <summary>
        /// 装備マスタ情報
        /// </summary>
        public class Param
        {
            /// <summary>
            /// 装備名
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// 装備種別名
            /// </summary>
            public string Type { get; private set; }

            /// <summary>
            /// 装備種別番号
            /// </summary>
            public int TypeNum { get; private set; }

            public int TypeDetailNum { get; private set; }

            /// <summary>
            /// 装備ID
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// 艦載機かどうか
            /// </summary>
            public bool Aircraft { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="json">DynamicJSON</param>
            //                public Param(dynamic json)
            public Param(KCB.api_start2.ApiData.ApiMstSlotitem json)
            {
                Name = json.api_name;
                Id = json.api_id;
                TypeNum = json.api_type[3];
                TypeDetailNum = json.api_type[2];
                Type = GetItemType(TypeNum);
                Aircraft = IsAircraft(TypeNum);

                対空 = json.api_tyku;
                装甲 = json.api_souk;
                火力 = json.api_houg;
                雷撃 = json.api_raig;
                Soku = json.api_soku;
                爆装 = json.api_baku;
                Bakk = json.api_bakk;
                耐久 = json.api_taik;
                対潜 = json.api_tais;
                砲撃命中 = json.api_houm;
                砲撃回避 = json.api_houk;
                雷撃回避 = json.api_raik;
                雷撃命中 = json.api_raim;
                Houk = json.api_houk;
                索敵 = json.api_saku;
                運 = json.api_luck;
                射程 = json.api_leng;
                Rare = json.api_rare;
                Atap = json.api_atap;

            }

            public int 対空 { get; private set; }
            public int 装甲 { get; private set; }
            public int 火力 { get; private set; }
            public int 雷撃 { get; private set; }
            public int Soku { get; private set; }
            public int 爆装 { get; private set; }
            public int 耐久 { get; private set; }
            public int 対潜 { get; private set; }
            public int 砲撃命中 { get; private set; }
            public int 砲撃回避 { get; private set; }
            public int 雷撃回避 { get; private set; }
            public int 雷撃命中 { get; private set; }
            public int Houk { get; private set; }
            public int 索敵 { get; private set; }
            public int 運 { get; private set; }
            public int Rare { get; private set; }
            public int 射程 { get; private set; }
            public int Bakk { get; private set; }
            public int Atap { get; private set; }

            /// <summary>
            /// アイテム種別
            /// </summary>
            /// <param name="dType">api_type[3]</param>
            /// <returns></returns>
            static public string GetItemType(int itemType)
            {
                switch (itemType)
                {
                    case 1: return "小口径主砲";
                    case 2: return "中口径主砲";
                    case 3: return "大口径主砲";
                    case 4: return "副砲";
                    case 5: return "魚雷";
                    case 6: return "艦戦";
                    case 7: return "艦爆";
                    case 8: return "艦攻";
                    case 9: return "偵察機";
                    case 10: return "水上機";
                    case 11: return "電探";
                    case 12: return "三式弾";
                    case 13: return "徹甲弾";
                    case 14: return "ダメコン";
                    case 15: return "機銃";
                    case 16: return "高角砲";
                    case 17: return "爆雷投射機";
                    case 18: return "ソナー";
                    case 19: return "機関部強化";
                    case 20: return "上陸用舟艇";
                    case 21: return "オートジャイロ";
                    case 22: return "対潜哨戒機";
                    case 23: return "追加装甲";
                    case 24: return "探照灯";
                    case 25: return "簡易輸送部材";
                    case 26: return "艦艇修理施設";
                    case 27: return "照明弾";
                    case 28: return "艦隊司令部施設";
                    case 29: return "航空要員";
                    case 30: return "高射装置";
                    case 31: return "対地装備";
                    case 32: return "水上艦要員";
                    case 33: return "大型飛行艇";
                    case 34: return "戦闘糧食";
                    case 35: return "補給物資";
                    case 36: return "特型内火艇";
                    default: return string.Format("未知({0})", itemType);
                }

            }
            /// <summary>
            /// 装備辞典でのアイテム種別(2)
            /// </summary>
            /// 
            /// <param name="dType">api_type[2]</param>
            /// <returns></returns>
            static public string GetItemDicType(int itemDetailType)
            {
                switch (itemDetailType)
                {
                    case 1: return "小口径主砲";
                    case 2: return "中口径主砲";
                    case 3: return "大口径主砲";
                    case 4: return "副砲";
                    case 5: return "魚雷";
                    case 6: return "艦戦";
                    case 7: return "艦爆";
                    case 8: return "艦攻";
                    case 9: return "艦偵";
                    case 10: return "水上偵察機";
                    case 11: return "水上爆撃機";
                    case 12: return "小型電探";
                    case 13: return "大型電探";
                    case 14: return "ソナー";
                    case 15: return "爆雷";
                    case 16: return "(なし)";
                    case 17: return "機関部強化";
                    case 18: return "三式弾";
                    case 19: return "徹甲弾";
                    case 20: return "(なし)";
                    case 21: return "機銃";
                    case 22: return "特殊潜航艇";
                    case 23: return "応急修理要員";
                    case 24: return "上陸用舟艇";
                    case 25: return "オートジャイロ";
                    case 26: return "対潜哨戒機";
                    case 27: return "追加装甲(中型)";
                    case 28: return "追加装甲(大型)";
                    case 29: return "探照灯";
                    case 30: return "簡易輸送部材";
                    case 31: return "艦艇修理施設";
                    case 32: return "潜水艦魚雷";
                    case 33: return "照明弾";
                    case 34: return "艦隊司令部施設";
                    case 35: return "航空要員";
                    case 36: return "高射装置";
                    case 37: return "対地装備";
                    case 38: return "(なし)";
                    case 39: return "水上艦要員";
                    case 40: return "大型ソナー";
                    case 41: return "大型飛行艇";
                    case 42: return "大型探照灯";
                    case 43: return "戦闘糧食";
                    case 44: return "補給物資";
                    case 45: return "多用途水上機";
                    case 46: return "特型内火艇";
                    default: return string.Format("未知({0})", itemDetailType);
                }
            }


            /// <summary>
            /// 艦載機装備ならtrue
            /// </summary>
            /// <param name="dType">api_type[3]</param>
            /// <returns></returns>
            static public bool IsAircraft(int itemType)
            {
                switch (itemType)
                {
                    case 6:  //艦戦
                    case 7:  //艦爆
                    case 8:  //艦攻
                    case 9:  //偵察機
                    case 10: //水上機
                        return true;

                    default:
                        return false;
                }
            }

            public override string ToString()
            {
                return string.Format("ItemMaster ID:{0} Name:{1} Type:{2}",
                    Id, Name, Type);
            }



        }

        /// <summary>
        /// 射程文字列を作成
        /// </summary>
        /// <param name="shotRange">射程値</param>
        /// <returns>文字列</returns>
        public static string GetShotRangeString(int shotRange)
        {
            switch (shotRange)
            {
                case 0:
                    return "無";
                case 1:
                    return "短";
                case 2:
                    return "中";
                case 3:
                    return "長";
                case 4:
                    return "超長";
                default:
                    return string.Format("未知({0})", shotRange);
            }

        }

        /// <summary>
        /// レア度文字列を作成
        /// </summary>
        /// <param name="Rarity">レア度値</param>
        /// <returns>文字列</returns>
        public static string GetRarityString(int Rarity)
        {
            switch (Rarity)
            {
                case 0:
                    return "コモン";
                case 1:
                    return "レア";
                case 2:
                    return "ホロ";
                case 3:
                    return "Sホロ";
                case 4:
                case 5:
                    return "SSホロ";
                default:
                    return Rarity.ToString();
            }
        }



        public class SlotItemMasterLVItem : System.Windows.Forms.ListViewItem,
            IComparable<SlotItemMasterLVItem>
        {
            //初期ソートはID
            static private int _column = (int)ItemOrder.ID;
            static private System.Windows.Forms.SortOrder _order
                = System.Windows.Forms.SortOrder.Ascending;

            static public int Column
            {
                set
                {
                    //カラムに変化がなかった
                    if (_column == value)
                    {
                        if (_order == System.Windows.Forms.SortOrder.Ascending)
                            _order = System.Windows.Forms.SortOrder.Descending;
                        else if (_order == System.Windows.Forms.SortOrder.Descending)
                            _order = System.Windows.Forms.SortOrder.Ascending;
                    }
                    else//カラムが変わったのでオーダーは保持しない
                        _order = System.Windows.Forms.SortOrder.Ascending;
                    _column = value;
                }
                get
                {
                    return _column;
                }
            }
            static public System.Windows.Forms.SortOrder Order { get { return _order; } }

            enum ItemOrder
            {
                ID = 0,
                SortNo = 1,
                Name = 2,
                Type = 3,
                //                    Taik = 4,
                Souk = 4,
                Houg = 5,
                Soku = 6,
                Baku = 7,
                Tyku = 8,
                Tais = 9,
                //                    Atap = 11,
                Houm = 10,
                Raim = 11,
                Houk = 12,
                Raik = 13,
                //                    Bakk = 15,
                Saku = 14,
                //                    Sakb = 14,
                Luck = 15,
                Leng = 16,
                Rare = 17,
                Broken = 18,
                Info = 19,
                //                    UseBull = 20,
            }

            void InitializeSubItem()
            {
                for (int n = 0; n < Enum.GetValues(typeof(ItemOrder)).Length; n++)
                    SubItems.Add("");
            }

            public SlotItemMasterLVItem()
            {
                InitializeSubItem();
            }

            public SlotItemMasterLVItem(KCB.api_start2.ApiData.ApiMstSlotitem json)
            {
                InitializeSubItem();
                UpdateSlotItemInfo(json);
            }

            public void UpdateSlotItemInfo(KCB.api_start2.ApiData.ApiMstSlotitem json)
            {
                SubItems[(int)ItemOrder.ID] = new LVIntSubItem(this, json.api_id);
                SubItems[(int)ItemOrder.SortNo] = new LVIntSubItem(this, json.api_sortno);
                SubItems[(int)ItemOrder.Name] = new LVStringSubItem(this, json.api_name);
                SubItems[(int)ItemOrder.Type] = new LVSlotItemTypeSubItem(this, json.api_type);
                //                    SubItems[(int)ItemOrder.Taik] = new LVIntSubItem(this, json.api_taik);
                SubItems[(int)ItemOrder.Souk] = new LVIntSubItem(this, json.api_souk);
                SubItems[(int)ItemOrder.Houg] = new LVIntSubItem(this, json.api_houg);
                SubItems[(int)ItemOrder.Soku] = new LVIntSubItem(this, json.api_soku);
                SubItems[(int)ItemOrder.Baku] = new LVIntSubItem(this, json.api_baku);
                SubItems[(int)ItemOrder.Tyku] = new LVIntSubItem(this, json.api_tyku);
                SubItems[(int)ItemOrder.Tais] = new LVIntSubItem(this, json.api_tais);
                //                    SubItems[(int)ItemOrder.Atap] = new LVIntSubItem(this, json.api_atap);
                SubItems[(int)ItemOrder.Houm] = new LVIntSubItem(this, json.api_houm);
                SubItems[(int)ItemOrder.Raim] = new LVIntSubItem(this, json.api_raim);
                SubItems[(int)ItemOrder.Houk] = new LVIntSubItem(this, json.api_houk);
                SubItems[(int)ItemOrder.Raik] = new LVIntSubItem(this, json.api_raik);
                //                    SubItems[(int)ItemOrder.Bakk] = new LVIntSubItem(this, json.api_bakk);
                SubItems[(int)ItemOrder.Saku] = new LVIntSubItem(this, json.api_saku);
                //                    SubItems[(int)ItemOrder.Sakb] = new LVIntSubItem(this, json.api_sakb);
                SubItems[(int)ItemOrder.Luck] = new LVIntSubItem(this, json.api_luck);
                SubItems[(int)ItemOrder.Leng] = new LVShotLengthSubItem(this, json.api_leng);
                SubItems[(int)ItemOrder.Rare] = new LVRaretySubItem(this, json.api_rare);
                SubItems[(int)ItemOrder.Broken] = new LVParamArraySubItem(this, json.api_broken);
                SubItems[(int)ItemOrder.Info] = new LVStringSubItem(this, json.api_info);
                //                    SubItems[(int)ItemOrder.UseBull] = new LVStringSubItem(this, json.api_usebull);

                ImageIndex = json.api_type[3];
            }

            /// <summary>
            /// カラムを追加
            /// </summary>
            /// <param name="lvShip"></param>
            public static void InitializeColumn(System.Windows.Forms.ListView
                lvSlotItem)
            {
                foreach (ItemOrder it in Enum.GetValues(typeof(ItemOrder)))
                {
                    System.Windows.Forms.ColumnHeader col
                        = new System.Windows.Forms.ColumnHeader();
                    col.Text = GetColumnText(it);
                    col.DisplayIndex = (int)it;
                    lvSlotItem.Columns.Add(col);
                }
            }
            /// <summary>
            /// カラム文字列を取得
            /// </summary>
            /// <param name="it"></param>
            /// <returns></returns>
            static string GetColumnText(ItemOrder it)
            {
                switch (it)
                {
                    case ItemOrder.ID:
                        return "ID";
                    case ItemOrder.SortNo:
                        return "図鑑番号";
                    case ItemOrder.Name:
                        return "名前";
                    case ItemOrder.Type:
                        return "種別";
                    //                        case ItemOrder.Taik:
                    //                            return "対空";
                    case ItemOrder.Souk:
                        return "装甲";
                    case ItemOrder.Houg:
                        return "火力";
                    case ItemOrder.Soku:
                        return "速度";
                    case ItemOrder.Baku:
                        return "爆装";
                    case ItemOrder.Tyku:
                        return "対空";
                    case ItemOrder.Tais:
                        return "対潜";
                    //                        case ItemOrder.Atap:
                    //                            return "Atap";
                    case ItemOrder.Houm:
                        return "砲撃命中";
                    case ItemOrder.Raim:
                        return "雷撃命中";
                    case ItemOrder.Houk:
                        return "砲撃回避";
                    case ItemOrder.Raik:
                        return "雷撃回避";
                    //                        case ItemOrder.Bakk:
                    //                            return "Bakk";
                    case ItemOrder.Saku:
                        return "索敵";
                    //                        case ItemOrder.Sakb:
                    //                            return "Sakb";
                    case ItemOrder.Luck:
                        return "運";
                    case ItemOrder.Leng:
                        return "射程";
                    case ItemOrder.Rare:
                        return "レアリティ";
                    case ItemOrder.Broken:
                        return "解体時発生資源";
                    case ItemOrder.Info:
                        return "情報";
                    //                        case ItemOrder.UseBull:
                    //                            return "UseBull";
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            #region リストビューのサブアイテム

            /*
                 * ListViewSubItemを自作する場合、ListViewSubItemを所有するListViewItemを
                 * コンストラクタで指定しておかないと、サブアイテム値を更新しても
                 * 更新が反映されない
                 */

            class LVIntSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVIntSubItem(System.Windows.Forms.ListViewItem lvitOwner, int value)
                    : base(lvitOwner, "")
                {
                    Text = value.ToString();
                    Value = value;
                }
                public int Value { get; private set; }
                public int CompareTo(object obj)
                {
                    LVIntSubItem it2 = obj as LVIntSubItem;
                    if (obj == null)
                        return 0;

                    return Value - it2.Value;
                }

            }

            class LVRaretySubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVRaretySubItem(System.Windows.Forms.ListViewItem lvitOwner, int value)
                    : base(lvitOwner, "")
                {
                    Text = string.Format("{0}({1})", GetRarityString(value)
                        , value);
                    Value = value;
                }
                public int Value { get; private set; }
                public int CompareTo(object obj)
                {
                    LVRaretySubItem it2 = obj as LVRaretySubItem;
                    if (obj == null)
                        return 0;

                    return Value - it2.Value;
                }

            }

            class LVShotLengthSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVShotLengthSubItem(System.Windows.Forms.ListViewItem lvitOwner, int value)
                    : base(lvitOwner, "")
                {
                    Text = string.Format("{0}({1})", GetShotRangeString(value), value);
                    Value = value;
                }
                public int Value { get; private set; }
                public int CompareTo(object obj)
                {
                    LVShotLengthSubItem it2 = obj as LVShotLengthSubItem;
                    if (obj == null)
                        return 0;

                    return Value - it2.Value;
                }

            }


            class LVStringSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVStringSubItem(System.Windows.Forms.ListViewItem lvitOwner, string value)
                    : base(lvitOwner, "")
                {
                    Text = value;
                    Value = value;
                }
                public string Value { get; private set; }
                public int CompareTo(object obj)
                {
                    LVStringSubItem it2 = obj as LVStringSubItem;
                    if (obj == null)
                        return 0;

                    return string.Compare(Value, it2.Value);
                }
            }

            class LVParamArraySubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVParamArraySubItem(System.Windows.Forms.ListViewItem lvitOwner, List<int> values)
                    : base(lvitOwner, "")
                {
                    List<string> data = new List<string>();
                    Value = 0;
                    foreach (var it in values)
                    {
                        data.Add(it.ToString());
                        Value += it;
                    }

                    Text = string.Join(",", data);
                }
                public int Value { get; private set; }
                public int CompareTo(object obj)
                {
                    LVParamArraySubItem it2 = obj as LVParamArraySubItem;
                    if (obj == null)
                        return 0;

                    return Value - it2.Value;
                }
            }

            class LVSlotItemTypeSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVSlotItemTypeSubItem(System.Windows.Forms.ListViewItem lvitOwner, List<int> values)
                    : base(lvitOwner, "")
                {
                    Text = string.Format("{0},{1},{2}({4}),{3}({5})",
                        values[0], values[1], values[2], values[3], Param.GetItemDicType(values[2]), Param.GetItemType(values[3]));
                    Value = values[2];
                }
                public int Value { get; private set; }
                public int CompareTo(object obj)
                {
                    LVSlotItemTypeSubItem it2 = obj as LVSlotItemTypeSubItem;
                    if (obj == null)
                        return 0;

                    return Value - it2.Value;
                }
            }

            #endregion


            /// <summary>
            /// IComparableの実装
            /// </summary>
            /// <param name="it"></param>
            /// <returns></returns>
            public int CompareTo(SlotItemMasterLVItem it)
            {
                if (it == null)
                    return 0;

                int result = _compare(it);

                if (_order == System.Windows.Forms.SortOrder.Descending)
                    result = -result;
                else if (_order == System.Windows.Forms.SortOrder.None)
                    result = 0;

                return result;
            }

            int _compare(SlotItemMasterLVItem it)
            {
                IComparable comp1 = SubItems[_column] as IComparable;
                IComparable comp2 = it.SubItems[_column] as IComparable;

                if (comp1 == null || comp2 == null)
                    return 0;

                return comp1.CompareTo(comp2);
            }
        }

        /// <summary>
        /// 装備リストビュー一覧の取得
        /// </summary>
        /// <returns></returns>
        public SlotItemMasterLVItem[] GetLVList()
        {
            return _itemList.ToArray();
        }

    }
}
