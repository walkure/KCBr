using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Diagnostics;

namespace KCB2.MasterData
{
    /// <summary>
    /// 艦船情報のマスタデータ
    /// </summary>
    public class Ship
    {
        ConcurrentDictionary<int, Param> _shipMaster = new ConcurrentDictionary<int, Param>();

        List<ShipMasterLVItem> _shipLVList = new List<ShipMasterLVItem>();
        List<ShipTypeLVItem> _shipTypeLVList = new List<ShipTypeLVItem>();

        static bool _bUseMasterData = Properties.Settings.Default.UseMasterDataView;

        /// <summary>
        /// /kcsapi/api_get_master/ship
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool LoadShipMaster(KCB.api_start2.ApiData.ApiMstShip[] api_mst_ship, Item itemMaster)
        {
            _shipMaster.Clear();
            if (_bUseMasterData)
                _shipLVList.Clear();

            foreach (var item in api_mst_ship)
            {
                var it = new Param(item, this);
                _shipMaster[(int)item.api_id] = it;

                if (_bUseMasterData)
                {
                    ///先にStypeを呼んであると仮定
                    var lvit = new ShipMasterLVItem(item);
                    lvit.ShipType = it.ShipTypeName;
                    _shipLVList.Add(lvit);
                }
            }

            return true;
        }

        ConcurrentDictionary<int, ShipTypeInfo> _shipType = new ConcurrentDictionary<int, ShipTypeInfo>();

        /// <summary>
        /// 艦船種別情報
        /// </summary>
        public class ShipTypeInfo
        {
            /// <summary>
            /// 種別ID
            /// </summary>
            public int ShipTypeID { get; private set; }
            /// <summary>
            /// 種別名称
            /// </summary>
            public string ShipTypeName { get; private set; }
            /// <summary>
            /// 修理所要時間
            /// </summary>
            public int ShipRepairTimeParam { get; private set; }

            public ShipTypeInfo(KCB.api_start2.ApiData.ApiMstStype stype)
            {
                ShipTypeName = stype.api_name;
                ShipRepairTimeParam = stype.api_scnt;
                ShipTypeID = stype.api_id;

                Debug.WriteLine(string.Format("SType ID:{0} Name:{1} Repair:{2}",
                    ShipTypeID, ShipTypeName, ShipRepairTimeParam));
            }
        }

        /// <summary>
        /// /kcsapi/api_get_master/stype
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool LoadShipType(KCB.api_start2.ApiData.ApiMstStype[] api_mst_stype)
        {
            _shipType.Clear();
            if (_bUseMasterData)
                _shipTypeLVList.Clear();

            foreach (var item in api_mst_stype)
            {
                _shipType[item.api_id] = new ShipTypeInfo(item);
                if (_bUseMasterData)
                    _shipTypeLVList.Add(new ShipTypeLVItem(item));
            }

            return true;
        }

        /// <summary>
        /// 艦船マスターデータ
        /// </summary>
        public class Param
        {
            //                public Param(dynamic json)
            public Param(KCB.api_start2.ApiData.ApiMstShip json, Ship masterShip)
            {
                NameID = json.api_id;
                SortNo = json.api_sortno;
                Name = json.api_name;
                Yomi = json.api_yomi;
                ShipTypeId = json.api_stype;
                AfterLV = json.api_afterlv;
                MaxFuel = json.api_fuel_max;
                MaxBullet = json.api_bull_max;
                Speed = json.api_soku;
                SlotNum = json.api_slot_num;

                var stype = masterShip.LookupShipType(ShipTypeId);
                ShipTypeName = stype.ShipTypeName;
                RepairTimeParam = stype.ShipRepairTimeParam;

                Debug.WriteLine(string.Format("id:{0} name:{1} yomi:{2} type:{3} AfterLV:{4} MaxFuel:{5} MaxBullet:{6}",
                    NameID, Name, Yomi, ShipTypeId, AfterLV, MaxFuel, MaxBullet));

            }

            /// <summary>
            /// 名前ID
            /// </summary>
            public int NameID { get; private set; }

            /// <summary>
            /// 艦娘カードID
            /// </summary>
            public int SortNo { get; private set; }

            /// <summary>
            /// 名前
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// よみ
            /// </summary>
            public string Yomi { get; private set; }
            /// <summary>
            /// 艦船種別ID
            /// </summary>
            public int ShipTypeId { get; private set; }

            /// <summary>
            /// 艦船種別名
            /// </summary>
            public string ShipTypeName { get; private set; }
            /// <summary>
            /// 入渠時間算出に必要なパラメタ。実際は*0.5して使う
            /// </summary>
            public int RepairTimeParam { get; private set; }

            /// <summary>
            /// 改造レベル
            /// </summary>
            public int AfterLV { get; private set; }
            /// <summary>
            /// 最大燃料
            /// </summary>
            public int MaxFuel { get; private set; }
            /// <summary>
            /// 最大銃弾
            /// </summary>
            public int MaxBullet { get; private set; }
            /// <summary>
            /// 速力
            /// </summary>
            public int Speed { get; private set; }
            /// <summary>
            /// 装備スロット数
            /// </summary>
            public int SlotNum { get; private set; }

        }

        /// <summary>
        /// 艦船IDから艦船情報をとってくる
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Param LookupShipMaster(int id)
        {
            if (_shipMaster.ContainsKey(id))
                return _shipMaster[id];
            return null;
        }

        /// <summary>
        /// 艦船種別idから艦船種別をとってくる
        /// </summary>
        /// <param name="stype"></param>
        /// <returns></returns>
        public ShipTypeInfo LookupShipType(int stype)
        {
            if (_shipType.ContainsKey(stype))
                return _shipType[stype];

            throw new KeyNotFoundException("invalid stype id");
        }

        /// <summary>
        /// マスタデータ表示用ListViewItem
        /// </summary>
        public class ShipMasterLVItem : System.Windows.Forms.ListViewItem,
            IComparable<ShipMasterLVItem>
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

            public ShipMasterLVItem()
            {
                InitializeSubItem();
            }

            public ShipMasterLVItem(KCB.api_start2.ApiData.ApiMstShip json)
            {
                InitializeSubItem();
                UpdateShipInfo(json);
            }

            /// <summary>
            /// リストビューサブアイテムの設定
            /// </summary>
            void InitializeSubItem()
            {
                for (int n = 0; n < Enum.GetValues(typeof(ItemOrder)).Length; n++)
                    SubItems.Add("");
            }

            /// <summary>
            /// カラムを追加
            /// </summary>
            /// <param name="lvShip"></param>
            public static void InitializeColumn(System.Windows.Forms.ListView
                lvShip)
            {
                foreach (ItemOrder it in Enum.GetValues(typeof(ItemOrder)))
                {
                    System.Windows.Forms.ColumnHeader col = new System.Windows.Forms.ColumnHeader();
                    col.Text = GetColumnText(it);
                    col.DisplayIndex = (int)it;
                    lvShip.Columns.Add(col);
                }
            }

            /// <summary>
            /// リストアイテム
            /// </summary>
            enum ItemOrder
            {
                ID = 0,
                SortNo = 1,
                Name = 2,
                Yomi = 3,
                SType = 4,
                AfterLV = 5,
                AfterShipID = 6,
                Taik = 7,
                Souk = 8,
                Houg = 9,
                Raig = 10,
                Tyku = 11,
                Luck = 12,
                Soku = 13,
                Leng = 14,
                SlotNum = 15,
                BuildTime = 16,
                Broken = 17,
                PowUp = 18,
                Backs = 19,
                GetMes = 20,
                AfterFuel = 21,
                AfterBull = 22,
                FuelMax = 23,
                BullMax = 24,
            };

            /// <summary>
            /// ヘッダ文字列を返す
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
                    case ItemOrder.Yomi:
                        return "よみ";
                    case ItemOrder.SType:
                        return "艦船種別";
                    case ItemOrder.AfterLV:
                        return "改造レベル";
                    case ItemOrder.AfterShipID:
                        return "改造後ID";
                    case ItemOrder.Taik:
                        return "対空";
                    case ItemOrder.Souk:
                        return "装甲";
                    case ItemOrder.Houg:
                        return "火力";
                    case ItemOrder.Raig:
                        return "雷撃";
                    case ItemOrder.Tyku:
                        return "耐久";
                    case ItemOrder.Luck:
                        return "運";
                    case ItemOrder.Soku:
                        return "速力";
                    case ItemOrder.Leng:
                        return "射程";
                    case ItemOrder.SlotNum:
                        return "装備スロット数";
                    case ItemOrder.BuildTime:
                        return "建造時間";
                    case ItemOrder.Broken:
                        return "解体時発生資源";
                    case ItemOrder.PowUp:
                        return "近代化改装増分";
                    case ItemOrder.Backs:
                        return "背景色";
                    case ItemOrder.GetMes:
                        return "メッセージ";
                    case ItemOrder.AfterFuel:
                        return "改造燃料";
                    case ItemOrder.AfterBull:
                        return "改造弾薬";
                    case ItemOrder.FuelMax:
                        return "燃料";
                    case ItemOrder.BullMax:
                        return "弾薬";

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

            class LVShipTypeSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVShipTypeSubItem(System.Windows.Forms.ListViewItem lvitOwner, int value)
                    : base(lvitOwner, "")
                {
                    Text = value.ToString();
                    SType = value;
                }
                public int SType { get; private set; }
                public int CompareTo(object obj)
                {
                    LVShipTypeSubItem it2 = obj as LVShipTypeSubItem;
                    if (obj == null)
                        return 0;

                    return SType - it2.SType;
                }

            }

            /*
            class LVCruiserTypeSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVCruiserTypeSubItem(System.Windows.Forms.ListViewItem lvitOwner, int value)
                {
                    Text = value.ToString();
                    Value = value;
                }
                public int Value { get; private set; }
                public int CompareTo(object obj)
                {
                    LVCruiserTypeSubItem it2 = obj as LVCruiserTypeSubItem;
                    if (obj == null)
                        return 0;

                    return Value - it2.Value;
                }

            }
            */
            class LVStringSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVStringSubItem(System.Windows.Forms.ListViewItem lvitOwner, string value)
                    : base(lvitOwner, "")
                {
                    if (value != null)
                    {
                        Text = value;
                        Value = value;
                    }
                    else
                    {
                        Text = "";
                        Value = "";
                    }
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

            class LVNowMaxSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVNowMaxSubItem(System.Windows.Forms.ListViewItem lvitOwner, int[] values)
                    : base(lvitOwner, "")
                {
                    if (values != null)
                    {
                        Text = string.Format("{0}/{1}", values[0], values[1]);
                        Value = values[0];
                    }
                    else
                    {
                        Text = "";
                        Value = 0;
                    }

                }

                public int Value { get; private set; }
                public int CompareTo(object obj)
                {
                    LVNowMaxSubItem it2 = obj as LVNowMaxSubItem;
                    if (obj == null)
                        return 0;

                    return Value - it2.Value;
                }
            }

            class LVParamArraySubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVParamArraySubItem(System.Windows.Forms.ListViewItem lvitOwner, int[] values)
                    : base(lvitOwner, "")
                {
                    Value = 0;
                    if (values != null)
                    {
                        List<string> data = new List<string>();
                        foreach (var it in values)
                        {
                            data.Add(it.ToString());
                            Value += it;
                        }

                        Text = string.Join(",", data);
                    }
                    else
                    {
                        Text = "";
                    }
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

            class LVSlotItemSubItem : System.Windows.Forms.ListViewItem.ListViewSubItem, IComparable
            {
                public LVSlotItemSubItem(System.Windows.Forms.ListViewItem lvitOwner, int[] values, int slotNum)
                    : base(lvitOwner, "")
                {
                    if (values != null)
                    {
                        if (values.Length < slotNum)
                            slotNum = values.Length;
                        _slotItem = values;
                        _slotNum = slotNum;
                        List<string> data = new List<string>();
                        for (int n = 0; n < slotNum; n++)
                        {
                            data.Add(values[n].ToString());
                        }

                        Text = string.Join(",", data);
                        Value = Text;
                    }
                    else
                    {
                        Value = "";
                        Text = "";
                    }
                }
                public string Value { get; private set; }

                int[] _slotItem;
                int _slotNum;

                public int CompareTo(object obj)
                {
                    LVSlotItemSubItem it2 = obj as LVSlotItemSubItem;
                    if (obj == null)
                        return 0;

                    return string.Compare(Value, it2.Value);
                }
                /*
                /// <summary>
                /// 装備情報を反映
                /// </summary>
                /// <param name="masterItem"></param>
                public void UpdateSlotItem(MasterData.Item masterItem)
                {
                    List<string> data = new List<string>();
                    for (int n = 0; n < _slotNum; n++)
                    {
                        MasterData.Item.Param item = masterItem.GetItemParam(_slotItem[n]);

                        if (item != null)
                            data.Add(string.Format("{0}({1})", item.Name, _slotItem[n]));
                        else
                            data.Add(_slotItem[n].ToString());
                    }
                    Text = string.Join(",", data);
                    Value = Text;
                }
                 * */
            }

            #endregion


            //艦船マスタデータの更新
            public void UpdateShipInfo(KCB.api_start2.ApiData.ApiMstShip json)
            {
                SubItems[(int)ItemOrder.ID] = new LVIntSubItem(this, json.api_id);
                SubItems[(int)ItemOrder.SortNo] = new LVIntSubItem(this, json.api_sortno);
                SubItems[(int)ItemOrder.Name] = new LVStringSubItem(this, json.api_name);
                SubItems[(int)ItemOrder.Yomi] = new LVStringSubItem(this, json.api_yomi);
                SubItems[(int)ItemOrder.SType] = new LVShipTypeSubItem(this, json.api_stype);
                SubItems[(int)ItemOrder.AfterLV] = new LVIntSubItem(this, json.api_afterlv);
                SubItems[(int)ItemOrder.AfterShipID] = new LVStringSubItem(this, json.api_aftershipid);
                SubItems[(int)ItemOrder.Taik] = new LVNowMaxSubItem(this, json.api_taik);
                SubItems[(int)ItemOrder.Souk] = new LVNowMaxSubItem(this, json.api_souk);
                SubItems[(int)ItemOrder.Houg] = new LVNowMaxSubItem(this, json.api_houg);
                SubItems[(int)ItemOrder.Raig] = new LVNowMaxSubItem(this, json.api_raig);
                SubItems[(int)ItemOrder.Tyku] = new LVNowMaxSubItem(this, json.api_tyku);
                SubItems[(int)ItemOrder.Luck] = new LVNowMaxSubItem(this, json.api_luck);
                SubItems[(int)ItemOrder.Soku] = new LVIntSubItem(this, json.api_soku);
                SubItems[(int)ItemOrder.Leng] = new LVIntSubItem(this, json.api_leng);
                SubItems[(int)ItemOrder.SlotNum] = new LVIntSubItem(this, json.api_slot_num);
                SubItems[(int)ItemOrder.BuildTime] = new LVIntSubItem(this, json.api_buildtime);
                SubItems[(int)ItemOrder.Broken] = new LVParamArraySubItem(this, json.api_broken);
                SubItems[(int)ItemOrder.PowUp] = new LVParamArraySubItem(this, json.api_powup);
                SubItems[(int)ItemOrder.Backs] = new LVIntSubItem(this, json.api_backs);
                SubItems[(int)ItemOrder.GetMes] = new LVStringSubItem(this, json.api_getmes);
                SubItems[(int)ItemOrder.AfterFuel] = new LVIntSubItem(this, json.api_afterfuel);
                SubItems[(int)ItemOrder.AfterBull] = new LVIntSubItem(this, json.api_afterbull);
                SubItems[(int)ItemOrder.FuelMax] = new LVIntSubItem(this, json.api_fuel_max);
                SubItems[(int)ItemOrder.BullMax] = new LVIntSubItem(this, json.api_bull_max);
            }

            /// <summary>
            /// 艦船種別ID
            /// </summary>
            public int STypeNum
            {
                get
                {
                    LVShipTypeSubItem item = SubItems[(int)ItemOrder.SType] as LVShipTypeSubItem;
                    if (item == null)
                        return 0;

                    return item.SType;
                }
            }

            /// <summary>
            /// 艦船種別名
            /// </summary>
            public string ShipType
            {
                set
                {
                    LVShipTypeSubItem item = SubItems[(int)ItemOrder.SType] as LVShipTypeSubItem;
                    if (item == null)
                        return;

                    item.Text = string.Format("{0}({1})", value, item.SType);
                }
            }

            /// <summary>
            /// IComparableの実装
            /// </summary>
            /// <param name="it"></param>
            /// <returns></returns>
            public int CompareTo(ShipMasterLVItem it)
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

            /// <summary>
            /// ソート関数
            /// </summary>
            /// <param name="it"></param>
            /// <returns></returns>
            int _compare(ShipMasterLVItem it)
            {
                IComparable comp1 = SubItems[_column] as IComparable;
                IComparable comp2 = it.SubItems[_column] as IComparable;

                if (comp1 == null || comp2 == null)
                    return 0;

                return comp1.CompareTo(comp2);

            }
        }

        /// <summary>
        /// 艦船種別一覧
        /// </summary>
        public class ShipTypeLVItem : System.Windows.Forms.ListViewItem,
            IComparable<ShipTypeLVItem>
        {
            //初期ソートはID
            static private int _column = (int)ItemOrder.Id;
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
                Id = 0,
                KCnt = 1,
                SCnt = 2,
                SortNo = 3,
                Name = 4,
                Type1 = 5,
                Type2 = 6,
                Type3 = 7,
                Type4 = 8,
                Type5 = 9,
                Type6 = 10,
                Type7 = 11,
                Type8 = 12,
                Type9 = 13,
                Type10 = 14,
                Type11 = 15,
                Type12 = 16,
                Type13 = 17,
                Type14 = 18,
                Type15 = 19,
                Type16 = 20,
                Type17 = 21,
                Type18 = 22,
                Type19 = 23,
                Type20 = 24,
                Type21 = 25,
                Type22 = 26,
                Type23 = 27,
                Type24 = 28,
                Type25 = 29,
                Type26 = 30,
                Type27 = 31,
                Type28 = 32,
                Type29 = 33,
                Type30 = 34,
                Type31 = 35,
                Type32 = 36,
                Type33 = 37,
                Type34 = 38,
                Type35 = 39,
                Type36 = 40,
                Type37 = 41,
                Type38 = 42,
                Type39 = 43,
                Type40 = 44,
                Type41 = 45,
                Type42 = 46,
                Type43 = 47,
                Type44 = 48,
            }

            static string GetColumnText(ItemOrder it)
            {
                switch (it)
                {
                    case ItemOrder.Id:
                        return "ID";
                    case ItemOrder.KCnt:
                        return "KCnt";
                    case ItemOrder.SCnt:
                        return "修理時間区分";
                    case ItemOrder.SortNo:
                        return "SortNo";
                    case ItemOrder.Name:
                        return "名前";
                    case ItemOrder.Type1:
                    case ItemOrder.Type2:
                    case ItemOrder.Type3:
                    case ItemOrder.Type4:
                    case ItemOrder.Type5:
                    case ItemOrder.Type6:
                    case ItemOrder.Type7:
                    case ItemOrder.Type8:
                    case ItemOrder.Type9:
                    case ItemOrder.Type10:
                    case ItemOrder.Type11:
                    case ItemOrder.Type12:
                    case ItemOrder.Type13:
                    case ItemOrder.Type14:
                    case ItemOrder.Type15:
                    case ItemOrder.Type16:
                    case ItemOrder.Type17:
                    case ItemOrder.Type18:
                    case ItemOrder.Type19:
                    case ItemOrder.Type20:
                    case ItemOrder.Type21:
                    case ItemOrder.Type22:
                    case ItemOrder.Type23:
                    case ItemOrder.Type24:
                    case ItemOrder.Type25:
                    case ItemOrder.Type26:
                    case ItemOrder.Type27:
                    case ItemOrder.Type28:
                    case ItemOrder.Type29:
                    case ItemOrder.Type30:
                    case ItemOrder.Type31:
                    case ItemOrder.Type32:
                    case ItemOrder.Type33:
                    case ItemOrder.Type34:
                    case ItemOrder.Type35:
                    case ItemOrder.Type36:
                    case ItemOrder.Type37:
                    case ItemOrder.Type38:
                    case ItemOrder.Type39:
                    case ItemOrder.Type40:
                    case ItemOrder.Type41:
                    case ItemOrder.Type42:
                    case ItemOrder.Type43:
                    case ItemOrder.Type44:
                        return Item.Param.GetItemDicType((int)it - 4);
                    default:
                        throw new IndexOutOfRangeException();
                }
            }

            void InitializeSubItem()
            {
                for (int n = 0; n < Enum.GetValues(typeof(ItemOrder)).Length; n++)
                    SubItems.Add("");
            }

            /// <summary>
            /// カラムを追加
            /// </summary>
            /// <param name="lvShip"></param>
            public static void InitializeColumn(System.Windows.Forms.ListView
                lvShip)
            {
                foreach (ItemOrder it in Enum.GetValues(typeof(ItemOrder)))
                {
                    System.Windows.Forms.ColumnHeader col = new System.Windows.Forms.ColumnHeader();
                    col.Text = GetColumnText(it);
                    col.DisplayIndex = (int)it;
                    lvShip.Columns.Add(col);
                }
            }

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

            public void UpdateSTypeInfo(KCB.api_start2.ApiData.ApiMstStype json)
            {
                SubItems[(int)ItemOrder.Id] = new LVIntSubItem(this, json.api_id);
                SubItems[(int)ItemOrder.KCnt] = new LVIntSubItem(this, json.api_kcnt);
                SubItems[(int)ItemOrder.SCnt] = new LVIntSubItem(this, json.api_scnt);
                SubItems[(int)ItemOrder.SortNo] = new LVIntSubItem(this, json.api_sortno);
                SubItems[(int)ItemOrder.Name] = new LVStringSubItem(this, json.api_name);
                SubItems[(int)ItemOrder.Type1] = new LVIntSubItem(this, json.api_equip_type.Type1);
                SubItems[(int)ItemOrder.Type2] = new LVIntSubItem(this, json.api_equip_type.Type2);
                SubItems[(int)ItemOrder.Type3] = new LVIntSubItem(this, json.api_equip_type.Type3);
                SubItems[(int)ItemOrder.Type4] = new LVIntSubItem(this, json.api_equip_type.Type4);
                SubItems[(int)ItemOrder.Type5] = new LVIntSubItem(this, json.api_equip_type.Type5);
                SubItems[(int)ItemOrder.Type6] = new LVIntSubItem(this, json.api_equip_type.Type6);
                SubItems[(int)ItemOrder.Type7] = new LVIntSubItem(this, json.api_equip_type.Type7);
                SubItems[(int)ItemOrder.Type8] = new LVIntSubItem(this, json.api_equip_type.Type8);
                SubItems[(int)ItemOrder.Type9] = new LVIntSubItem(this, json.api_equip_type.Type9);
                SubItems[(int)ItemOrder.Type10] = new LVIntSubItem(this, json.api_equip_type.Type10);
                SubItems[(int)ItemOrder.Type11] = new LVIntSubItem(this, json.api_equip_type.Type11);
                SubItems[(int)ItemOrder.Type12] = new LVIntSubItem(this, json.api_equip_type.Type12);
                SubItems[(int)ItemOrder.Type13] = new LVIntSubItem(this, json.api_equip_type.Type13);
                SubItems[(int)ItemOrder.Type14] = new LVIntSubItem(this, json.api_equip_type.Type14);
                SubItems[(int)ItemOrder.Type15] = new LVIntSubItem(this, json.api_equip_type.Type15);
                SubItems[(int)ItemOrder.Type16] = new LVIntSubItem(this, json.api_equip_type.Type16);
                SubItems[(int)ItemOrder.Type17] = new LVIntSubItem(this, json.api_equip_type.Type17);
                SubItems[(int)ItemOrder.Type18] = new LVIntSubItem(this, json.api_equip_type.Type18);
                SubItems[(int)ItemOrder.Type19] = new LVIntSubItem(this, json.api_equip_type.Type19);
                SubItems[(int)ItemOrder.Type20] = new LVIntSubItem(this, json.api_equip_type.Type20);
                SubItems[(int)ItemOrder.Type21] = new LVIntSubItem(this, json.api_equip_type.Type21);
                SubItems[(int)ItemOrder.Type22] = new LVIntSubItem(this, json.api_equip_type.Type22);
                SubItems[(int)ItemOrder.Type23] = new LVIntSubItem(this, json.api_equip_type.Type23);
                SubItems[(int)ItemOrder.Type24] = new LVIntSubItem(this, json.api_equip_type.Type24);
                SubItems[(int)ItemOrder.Type25] = new LVIntSubItem(this, json.api_equip_type.Type25);
                SubItems[(int)ItemOrder.Type26] = new LVIntSubItem(this, json.api_equip_type.Type26);
                SubItems[(int)ItemOrder.Type27] = new LVIntSubItem(this, json.api_equip_type.Type27);
                SubItems[(int)ItemOrder.Type28] = new LVIntSubItem(this, json.api_equip_type.Type28);
                SubItems[(int)ItemOrder.Type29] = new LVIntSubItem(this, json.api_equip_type.Type29);
                SubItems[(int)ItemOrder.Type30] = new LVIntSubItem(this, json.api_equip_type.Type30);
                SubItems[(int)ItemOrder.Type31] = new LVIntSubItem(this, json.api_equip_type.Type31);
                SubItems[(int)ItemOrder.Type32] = new LVIntSubItem(this, json.api_equip_type.Type32);
                SubItems[(int)ItemOrder.Type33] = new LVIntSubItem(this, json.api_equip_type.Type33);
                SubItems[(int)ItemOrder.Type34] = new LVIntSubItem(this, json.api_equip_type.Type34);
                SubItems[(int)ItemOrder.Type35] = new LVIntSubItem(this, json.api_equip_type.Type35);
                SubItems[(int)ItemOrder.Type36] = new LVIntSubItem(this, json.api_equip_type.Type36);
                SubItems[(int)ItemOrder.Type37] = new LVIntSubItem(this, json.api_equip_type.Type37);
                SubItems[(int)ItemOrder.Type38] = new LVIntSubItem(this, json.api_equip_type.Type38);
                SubItems[(int)ItemOrder.Type39] = new LVIntSubItem(this, json.api_equip_type.Type39);
                SubItems[(int)ItemOrder.Type40] = new LVIntSubItem(this, json.api_equip_type.Type40);
                SubItems[(int)ItemOrder.Type41] = new LVIntSubItem(this, json.api_equip_type.Type41);
                SubItems[(int)ItemOrder.Type42] = new LVIntSubItem(this, json.api_equip_type.Type42);
                SubItems[(int)ItemOrder.Type43] = new LVIntSubItem(this, json.api_equip_type.Type43);
                SubItems[(int)ItemOrder.Type44] = new LVIntSubItem(this, json.api_equip_type.Type44);
            }

            public ShipTypeLVItem()
            {
                InitializeSubItem();
            }

            public ShipTypeLVItem(KCB.api_start2.ApiData.ApiMstStype json)
            {
                InitializeSubItem();
                UpdateSTypeInfo(json);
            }

            /// <summary>
            /// IComparableの実装
            /// </summary>
            /// <param name="it"></param>
            /// <returns></returns>
            public int CompareTo(ShipTypeLVItem it)
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

            /// <summary>
            /// ソート関数
            /// </summary>
            /// <param name="it"></param>
            /// <returns></returns>
            int _compare(ShipTypeLVItem it)
            {
                IComparable comp1 = SubItems[_column] as IComparable;
                IComparable comp2 = it.SubItems[_column] as IComparable;

                if (comp1 == null || comp2 == null)
                    return 0;

                return comp1.CompareTo(comp2);

            }


        }

        /// <summary>
        /// マスタデータ表示用データを返す
        /// </summary>
        /// <returns></returns>
        public ShipMasterLVItem[] GetMasterLVItemList()
        {
            return _shipLVList.ToArray();
        }

        public ShipTypeLVItem[] GetMasterSTypeLVItems()
        {
            return _shipTypeLVList.ToArray();
        }
    }

}
