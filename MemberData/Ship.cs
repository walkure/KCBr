using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace KCB2.MemberData
{
    /// <summary>
    /// 艦娘情報
    /// /kcsapi/api_get_member/ship2
    /// /kcsapi/api_get_member/ship3
    /// /kcsapi/api_port/port
    /// </summary>
    public class Ship
    {
        /// <summary>
        /// 艦娘IDをキーとする艦娘のハッシュ
        /// </summary>
        ConcurrentDictionary<int, Info> _shipDic = new ConcurrentDictionary<int, Info>();

        /// <summary>
        /// アイテムIDをキーとする所有者のハッシュ
        /// </summary>
        ConcurrentDictionary<int, SlotItemOwner> _itemOwner = new ConcurrentDictionary<int, SlotItemOwner>();

        /// <summary>
        /// アイテム所有者表示用情報
        /// </summary>
        public class SlotItemOwner
        {
            /// <summary>
            /// 艦船ID
            /// </summary>
            public int ID { get; private set; }
            /// <summary>
            /// レベル
            /// </summary>
            public int Level { get; private set; }
            /// <summary>
            /// 名前
            /// </summary>
            public string Name { get; private set; }
            public SlotItemOwner(Info info)
            {
                ID = info.ShipId;
                Level = info.Level;
                Name = info.ShipName;
            }

            public override string ToString()
            {
                return string.Format("{0}({1})", Name, ID);
            }
        }

        /// <summary>
        /// データの最終更新時刻
        /// </summary>
        public DateTime LastUpdated { get; private set; }


        /// <summary>
        /// /kcsapi/api_get_member/ship2
        /// 艦船状態と艦隊情報を更新
        /// </summary>
        /// <param name="JSON">JSON</param>
        /// <param name="shipData">艦船情報マスタ</param>
        /// <param name="deckData">艦隊情報</param>
        /// <returns></returns>
        public bool LoadShip2(string JSON, MasterData.Ship shipMaster, Deck deckData, MasterData.Mission masterMission)
        {
            //                var json = DynamicJson.Parse(JSON);
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.Ship2>(JSON);
            if ((int)json.api_result != 1)
                return false;

            if (json.api_data_deck != null)
                deckData.UpdateDeck(json.api_data_deck, masterMission);

            LoadShipInfo(json.api_data, shipMaster, 0);

            return true;
        }

        /// <summary>
        /// /kcsapi/api_get_member/ship3
        /// 艦船状態と艦隊情報を更新
        /// </summary>
        /// <param name="JSON">JSON</param>
        /// <param name="shipData">艦船情報マスタ</param>
        /// <param name="deckData">艦隊情報</param>
        /// <returns></returns>
        public bool LoadShip3(string JSON, MasterData.Ship shipMaster, MasterData.Mission masterMission,
            Deck deckData, IDictionary<string, string> queryParam)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.Ship3>(JSON);
            if ((int)json.api_result != 1)
                return false;

            //取り敢えず一隻だけ更新される場合を見ておく。
            int updateShipId = 0;
            if (queryParam.ContainsKey("api_shipid"))
                updateShipId = int.Parse(queryParam["api_shipid"]);
            Debug.WriteLine("LoadShip:" + updateShipId.ToString());

            if (json.api_data.api_deck_data != null)
                deckData.UpdateDeck(json.api_data.api_deck_data, masterMission);

            return LoadShipInfo(json.api_data.api_ship_data, shipMaster, updateShipId);
        }

        /// <summary>
        /// /kcsapi/api_get_member/ship_deck
        /// Deck情報も降ってくるけど艦船情報だけ更新する
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool UpdateShipsInfo(IEnumerable<KCB.api_get_member.ApiDataShip> api_ship_data, MasterData.Ship shipMaster)
        {

            //すでに所有する艦娘の情報だけ降ってくると仮定していいのかしらね
            //TODO: 出撃中に轟沈した場合はどんなデータが降ってくるのか。出撃終了までゾンビ化する可能性があるけどどうしよう
            foreach (var it in api_ship_data)
            {
                var ship = this.GetShip(it.api_id);
                if (ship != null)
                {
                    Debug.WriteLine(string.Format("Update Ship{0} id:{1}",ship.ShipName,it.api_id));
                    ship.Update(it, shipMaster);
                }
                else
                {
                    //未知だった場合は追加しておく
                    Debug.WriteLine(string.Format("Add Ship{0} id:{1}",shipMaster.LookupShipMaster(it.api_ship_id).Name, it.api_id));
                    AddShip(it, shipMaster);
                }
            }

            return true;

        }

        /// <summary>
        /// /kcsapi/api_port/port
        /// </summary>
        /// <param name="shipInfo"></param>
        /// <param name="shipMaster"></param>
        /// <param name="updateShipId">情報が更新される艦船ID すべての場合は0</param>
        /// <returns></returns>
        public bool LoadShipInfo(List<KCB.api_get_member.ApiDataShip> shipInfo, MasterData.Ship shipMaster,
            int updateShipId)
        {
            //全艦船情報が降って来た
            if (updateShipId == 0)
            {
                _shipDic.Clear();
                _itemOwner.Clear();
            }
            else
            {
                //指定した艦船IDの情報だけ降って来た。よって指定した艦船IDが保有する装備の所有者情報だけ削除する
                List<int> updateShipItem = new List<int>();
                foreach (var it in _itemOwner)
                {
                    if (it.Value.ID == updateShipId)
                        updateShipItem.Add(it.Key);
                }

                SlotItemOwner removeInfo;
                foreach (var it in updateShipItem)
                    _itemOwner.TryRemove(it, out removeInfo);
            }

            foreach (var data in shipInfo)
            {
                AddShip(data, shipMaster);
            }
            LastUpdated = DateTime.Now;
            return true;
        }

        /// <summary>
        /// 艦船追加
        /// </summary>
        /// <param name="data">追加する艦船</param>
        /// <param name="shipMaster">艦船マスタ</param>
        public void AddShip(KCB.api_get_member.ApiDataShip data, MasterData.Ship shipMaster)
        {
            Info info;
            if (!_shipDic.TryGetValue(data.api_id, out info))
            {
                info = new Info(data, shipMaster);
                _shipDic[data.api_id] = info;
            }
            else
            {
                info.Update(data, shipMaster);
            }
            //Debug.WriteLine("Ship:" + info.ToString());

            foreach (var item in info.SlotItem)
            {
                _itemOwner[item.ID] = new SlotItemOwner(info);
                //                    Debug.WriteLine(string.Format("ID:{0} -> {1}", item.ID, info.ShipName));

            }
        }

        /// <summary>
        /// 補給
        /// /kcsapi/api_req_hokyu/charge を反映させる
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool Charge(string JSON)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_hokyu.Charge>(JSON);
            if ((int)json.api_result != 1)
                return false;

            foreach (var it in json.api_data.api_ship)
            {
                var ship = GetShip(it.api_id);
                ship.Fuel.Now = it.api_fuel;
                ship.Bullet.Now = it.api_bull;
                for (int i = 0; i < ship.SlotItem.Count; i++)
                    ship.SlotItem[i].Count = it.api_onslot[i];
                ship.Refresh();
            }

            return true;
        }

        /// <summary>
        /// 近代化改装
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool Powerup(string JSON, IDictionary<string, string> queryParam,
            Ship memberShip, Deck memberDeck, Item memberItem, MasterData.Ship masterShip,
            MasterData.Mission masterMission)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kaisou.Powerup>(JSON);
            if (json.api_result != 1)
                return false;

            string materialShips = queryParam["api_id_items"];

            bool bResult = true;
            if (json.api_data.api_powerup_flag == 0)
                bResult = false;

            memberDeck.UpdateDeck(json.api_data.api_deck, masterMission);

            List<KCB.api_get_member.ApiDataShip> shipDataList = new List<KCB.api_get_member.ApiDataShip>();
            shipDataList.Add(json.api_data.api_ship);

            LoadShipInfo(shipDataList, masterShip, json.api_data.api_ship.api_id);



            //素材になった艦娘のデータを削除
            foreach (var ma in materialShips.Split(','))
            {
                int ship_id = int.Parse(ma);

                memberItem.DestroySlottedItem(ship_id);
                DestoryShip(ship_id);
            }

            ///装備の値を引く
            var pwupShip = GetShip(json.api_data.api_ship.api_id);
            pwupShip.AppliedSlotItemInfo = false;
            pwupShip.ApplySlotItemData(memberItem);

            return bResult;

        }

        /// <summary>
        /// 艦船をリストから消す
        /// </summary>
        /// <param name="ship_id"></param>
        public void DestoryShip(int ship_id)
        {
            //艦船を消す
            Info remVal;
            _shipDic.TryRemove(ship_id, out remVal);

            //艦船が所有してた装備を削除
            List<int> destroyedItems = new List<int>();
            foreach (var it in _itemOwner)
            {
                if (it.Value.ID == ship_id)
                    destroyedItems.Add(it.Key);
            }
            foreach (var it in destroyedItems)
            {
                SlotItemOwner deletedItem;
                _itemOwner.TryRemove(it, out deletedItem);

            }
        }

        /// <summary>
        /// 艦船をロック(解除)する
        /// /kcsapi/api_req_hensei/lock
        /// </summary>
        /// <param name="ship_id"></param>
        /// <returns></returns>
        public APIResponse Lock(string JSON, int ship_id)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_hensei.Lock>(JSON);
            if (json.api_result != 1)
                return APIResponse.Error;

            var ship = GetShip(ship_id);
            if (ship == null)
                return APIResponse.Error;
            ship.Refresh();
            if (json.api_data.api_locked == 1)
            {
                ship.Locked = true;
                return APIResponse.True;
            }

            ship.Locked = false;
            return APIResponse.False;
        }


        /// <summary>
        /// 指定したIDの艦娘情報を取ってくる
        /// </summary>
        /// <param name="ship_id">艦娘ID</param>
        /// <returns></returns>
        public Info GetShip(int ship_id)
        {
            Info ret;
            if (_shipDic.TryGetValue(ship_id, out ret))
                return ret;
            Debug.WriteLine("cannot GetShip:" + ship_id.ToString());
            return null;

        }

        /// <summary>
        /// 指定したIDの艦娘情報を取ってくる
        /// </summary>
        /// <param name="ship_id">艦娘ID</param>
        /// <returns></returns>
        public Info GetShip(string ship_id_s)
        {
            int ship_id;
            if (int.TryParse(ship_id_s, out ship_id))
                return GetShip(ship_id);

            return null;
        }

        /// <summary>
        /// 指定したアイテムIDのオーナー艦娘を取得
        /// </summary>
        /// <param name="item_id">アイテムID</param>
        /// <returns></returns>
        public SlotItemOwner GetItemOwner(int item_id)
        {
            SlotItemOwner ret;
            if (_itemOwner.TryGetValue(item_id, out ret))
                return ret;
            return null;

        }

        /// <summary>
        /// アイテムオーナ一覧のハッシュ。中身をそのまま返すのでいじると不幸になるかも
        /// </summary>
        public IDictionary<int, SlotItemOwner> ItemOwnerList
        {
            get
            {
                return _itemOwner;
            }
        }

        /// <summary>
        /// 艦船数
        /// </summary>
        public int Count
        {
            get
            {
                return _shipDic.Count;
            }
        }

        /// <summary>
        /// 艦船情報
        /// </summary>
        public class Info : ICloneable
        {
            //最大値/最小値を一括ハンドルするクラス
            public class NowMax : IComparable<NowMax>
            {
                public int Now { get; set; }
                public int Max { get; private set; }

                /// <summary>
                /// 値を指定してオブジェクトを生成
                /// </summary>
                /// <param name="_now">現状値</param>
                /// <param name="_max">最大値</param>
                public NowMax(double _now, double _max) { Now = (int)_now; Max = (int)_max; }

                /// <summary>
                /// 更新
                /// </summary>
                /// <param name="_now"></param>
                /// <param name="_max"></param>
                public void Update(double _now, double _max) { Now = (int)_now; Max = (int)_max; }

                /// <summary>
                /// コピーコンストラクタ
                /// </summary>
                /// <param name="org">複製元</param>
                public NowMax(NowMax org) { Now = org.Now; Max = org.Max; }

                /// <summary>
                /// デフォルトのコンストラクタ
                /// </summary>
                public NowMax() { Now = 0; Max = 0; }

                /// <summary>
                /// IComparableインタフェイスの実装
                /// </summary>
                /// <param name="it"></param>
                /// <returns></returns>
                public int CompareTo(NowMax it)
                {
                    return SimplePercent - it.SimplePercent;
                }

                /// <summary>
                /// カスタマイズされたToString()
                /// </summary>
                /// <returns>現在地/最大値 の形式でフォーマットされた値を返す</returns>
                public override string ToString()
                {
                    return string.Format("{0}/{1}", Now, Max);
                }

                public string CSV
                {
                    get
                    {
                        return string.Format("{0},{1}", Now, Max);
                    }
                }

                /// <summary>
                /// 割合
                /// </summary>
                public double Ratio
                {
                    get
                    {
                        return ((double)Now) / Max;
                    }
                }

                /// <summary>
                /// パーセント(小数点以下切り捨て整数で返す)
                /// </summary>
                public int Percent
                {
                    get
                    {
                        //大破
                        if (Ratio <= 0.25)
                            return (int)Math.Floor(Ratio * 100D);
                        else
                            return (int)Math.Ceiling(Ratio * 100D);
                    }
                }

                /// <summary>
                /// パーセント(単純に小数点以下切り捨て整数で返す)
                /// </summary>
                public int SimplePercent
                {
                    get
                    {
                        return (int)Math.Floor(Ratio * 100D);
                    }
                }

                /// <summary>
                /// 割合に応じた背景色
                /// </summary>
                public Color BackgroundColor
                {
                    get
                    {
                        return GetPercentageColor(Ratio);
                    }
                }

                /// <summary>
                /// 割合から色を算出する
                /// </summary>
                /// <param name="data">割合を算出する値</param>
                /// <returns>対応する色</returns>
                static Color GetPercentageColor(double ratio_)
                {
                    //大破
                    if (ratio_ <= 0.25)
                        return Color.LightPink;
                    //中破
                    if (ratio_ <= 0.5)
                        return Color.Gold;
                    //小破
                    if (ratio_ <= 0.4)
                        return Color.Beige;
                    //無感
                    if (ratio_ <= 0.8)
                        return Color.LightYellow;

                    return Color.LightGreen;
                }

                /// <summary>
                /// 最大値以上になっているかどうか
                /// </summary>
                public bool Full
                {
                    get
                    {
                        return Now >= Max;
                    }
                }

                /// <summary>
                /// 差分(必ず0以上になる)
                /// </summary>
                public int Diff
                {
                    get
                    {
                        int diff = Max - Now;
                        if (diff > 0)
                            return diff;
                        return 0;
                    }
                }
            }

            //アイテム情報
            public class SlotItemInfo
            {
                /// <summary>
                /// アイテムUID
                /// </summary>
                public int ID { get; private set; }

                /// <summary>
                /// 艦載機数(補給すると復活する)
                /// </summary>
                public int Count { get; set; }

                /// <summary>
                /// アイテム情報
                /// </summary>
                public Item.Info Info { get; set; }

                /// <summary>
                /// 名前
                /// </summary>
                public string Name
                {
                    get
                    {
                        if (ID == -1)
                            return "(未装備)";
                        if (Info == null)
                            return string.Format("ID:{0}", ID);
                        return Info.Name;
                    }
                }

                /// <summary>
                /// 装備レベル
                /// </summary>
                public int Level
                {
                    get
                    {
                        if (Info == null)
                            return 0;
                        return Info.Level;
                    }
                }

                /// <summary>
                /// アイテム種別ID
                /// </summary>
                public int TypeNum
                {
                    get
                    {
                        if (Info == null)
                            return -1;
                        return Info.TypeNum;
                    }
                }

                /// <summary>
                /// 装備Item種別名
                /// </summary>
                public string TypeName
                {
                    get
                    {
                        if (TypeNum == -1)
                            return "";
                        return MasterData.Item.Param.GetItemType(TypeNum);
                    }
                }

                /// <summary>
                /// 装備情報を生成
                /// </summary>
                /// <param name="ID_">装備ID</param>
                /// <param name="_count">搭載艦載機数</param>
                public SlotItemInfo(int _ID, int _count)
                {
                    Debug.WriteLine(string.Format("new SlotItemInfo(ID=>{0},count=>{1})", _ID, _count));
                    Count = _count;
                    ID = _ID;
                    Info = null;
                }

                public SlotItemInfo(SlotItemInfo org)
                {
                    Count = org.Count;
                    Info = org.Info;
                    ID = org.ID;
                }

                public override string ToString()
                {
                    return Name;
                }

                /// <summary>
                /// スロットの制空値算出
                /// </summary>
                public int AirSuperiorityAbility
                {
                    get
                    {
                        //艦載機がない
                        if (Count == 0)
                            return 0;

                        //スロット情報がない
                        if (Info == null)
                            return 0;

                        // 熟練度ボーナス
                        Double lvBonus = Math.Sqrt(Info.Level);

                        //装備種別
                        int typeBonus = 0;
                        switch (Info.TypeDetailNum)
                        {
                            case 6:  // 艦戦
                            case 45: // 水戦
                                switch (Info.Level)
                                {
                                    case 0:
                                    case 1:
                                        typeBonus = 0;
                                        break;
                                    case 2:
                                        typeBonus = 2;
                                        break;
                                    case 3:
                                        typeBonus = 5;
                                        break;
                                    case 4:
                                        typeBonus = 9;
                                        break;
                                    case 5:
                                    case 6:
                                        typeBonus = 14;
                                        break;
                                    case 7:
                                        typeBonus = 22;
                                        break;
                                }
                                break;
                            case 7:  // 艦爆(爆戦の み対空値>0)
                            case 8:  // 艦攻
                                break;
                            case 11: // 水爆
                                switch (Info.Level)
                                {
                                    case 0:
                                    case 1:
                                        typeBonus = 0;
                                        break;
                                    case 2:
                                    case 3:
                                    case 4:
                                        typeBonus = 1;
                                        break;
                                    case 5:
                                    case 6:
                                        typeBonus = 3;
                                        break;
                                    case 7:
                                        typeBonus = 6;
                                        break;
                                }
                                break;
                            default:
                                return 0;
                        }

                        // 制空値 = floor(装備対空 * sqrt(装備数) + 練度ボーナス + 制空ボーナス)
                        return (int)Math.Floor(Info.対空 * Math.Sqrt(Count) + lvBonus + typeBonus);
                    }
                }

                /// <summary>
                /// 航空機による索敵値
                /// </summary>
                public int AirSearch
                {
                    get
                    {
                        //艦載機がない
                        if (Count == 0)
                            return 0;

                        //スロット情報がない
                        if (Info == null)
                            return 0;

                        //装備種別
                        switch (Info.TypeDetailNum)
                        {

                            case 9: // 艦偵
                            case 10: // 水偵
                            case 11: // 水爆
                                break;
                            default:
                                return 0;
                        }

                        return Info.索敵;

                    }
                }

                /// <summary>
                /// 電探(探照灯は？)による索敵値
                /// </summary>
                public int RadarSearch
                {
                    get
                    {
                        //スロット情報がない
                        if (Info == null)
                            return 0;

                        //装備種別
                        switch (Info.TypeDetailNum)
                        {

                            case 12: // 小型電探
                            case 13: // 大型電探
                                //                                case 29: // 探照灯
                                break;
                            default:
                                return 0;
                        }

                        return Info.索敵;
                    }
                }
            }

            /// <summary>
            /// カスタマイズされたToString
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("ShipInfo ID:{0} Name:{1} Lv:{2} Fuel:{3} Item:{4}",
                    ShipId, ShipName, Level, Fuel, string.Join(",", SlotItem));
            }

            #region ドックと艦隊情報の更新
            /// <summary>
            /// 艦隊情報を更新
            /// </summary>
            /// <param name="data">艦隊データ</param>
            public void UpdateDeckInfo(MemberData.Deck.ShipDeckData data)
            {
                if (data == null)
                {
                    FleetName = "";
                    FleetNum = 0;
                    FleetOrder = 0;
                }
                else
                {
                    FleetName = data.Name;
                    FleetOrder = data.Order;
                    FleetNum = data.Num;
                }
                Refresh();
            }

            /// <summary>
            /// 入渠ドック番号を更新
            /// </summary>
            /// <param name="ndock">ドック番号(0で未入渠)</param>
            public void UpdateNDock(int ndock)
            {
                DockNum = ndock;
                Refresh();
            }
            #endregion

            #region パラメータ

            /// <summary>
            /// 艦船名ID
            /// </summary>
            public int ShipNameId { get; protected set; }

            /// <summary>
            /// 艦娘カードID
            /// </summary>
            public int ShipSortNo { get; protected set; }

            /// <summary>
            /// 艦船名
            /// </summary>
            public String ShipName { get; protected set; }
            /// <summary>
            /// 艦船名よみ
            /// </summary>
            public String ShipNameYomi { get; protected set; }
            /// <summary>
            /// 艦船種別
            /// </summary>
            public int ShipTypeId { get; protected set; }
            /// <summary>
            /// 艦船種別名称
            /// </summary>
            public string ShipType { get; protected set; }
            /// <summary>
            /// 艦隊内艦船ID
            /// </summary>
            public int ShipId { get; protected set; }
            /// <summary>
            /// 修復時間
            /// </summary>
            public TimeSpan RepairTime { get; protected set; }
            /// <summary>
            /// 修復時間文字列
            /// </summary>
            public string RepairTimeString
            {
                get
                {
                    if (RepairTime.TotalDays >= 1.0)
                    {
                        int hours = (int)Math.Floor(RepairTime.TotalHours);
                        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, RepairTime.Minutes, RepairTime.Seconds);
                    }
                    else
                        return RepairTime.ToString(@"hh\:mm\:ss");
                }
            }
            /// <summary>
            /// HP
            /// </summary>
            public NowMax HP { get; protected set; }
            /// <summary>
            /// 燃料
            /// </summary>
            public NowMax Fuel { get; protected set; }
            /// <summary>
            /// 銃弾数
            /// </summary>
            public NowMax Bullet { get; protected set; }
            /// <summary>
            /// レベル
            /// </summary>
            public int Level { get; protected set; }

            /// <summary>
            /// レベル背景色
            /// </summary>
            public Color LevelBackgroundColor { get { return GetUpgradeColor(Level, UpdateLevel); } }

            /// <summary>
            /// 改装レベル
            /// </summary>
            public int UpdateLevel { get; protected set; }
            /// <summary>
            /// 経験値
            /// </summary>
            public int Experience { get; protected set; }

            /// <summary>
            /// 次レベルまでに必要な経験値
            /// </summary>
            public int ExperienceRequiredToNextLevel { get { return GetStepupRequiredExperience(Level, Experience); } }

            /// <summary>
            /// 改造までに必要な経験値
            /// </summary>
            public int ExperienceRequiredToUpgrade { get { return GetUpgradeExperience(Experience, UpdateLevel); } }

            /// <summary>
            /// 疲労値
            /// </summary>
            public int Condition { get; protected set; }

            public Color ConditionColor { get { return GetConditionColor(Condition); } }
            /// <summary>

            /// <summary>
            /// 火力
            /// </summary>
            public NowMax Fire { get; protected set; }

            /// <summary>
            /// 雷装
            /// </summary>
            public NowMax Torpedo { get; protected set; }

            /// <summary>
            /// 装甲
            /// </summary>
            public NowMax Armor { get; protected set; }

            /// <summary>
            /// 対空
            /// </summary>
            public NowMax AntiAir { get; protected set; }

            /// <summary>
            /// 回避
            /// </summary>
            public NowMax Escape { get; protected set; }

            /// <summary>
            /// 対潜
            /// </summary>
            public NowMax AntiSubm { get; protected set; }

            /// <summary>
            /// 索敵
            /// </summary>
            public NowMax Search { get; protected set; }

            /// <summary>
            /// 幸運度
            /// </summary>
            public NowMax Lucky { get; protected set; }

            /// <summary>
            /// 航空機搭載量
            /// </summary>
            //                public int Aircraft { get; protected set; }

            /// <summary>
            /// ロック
            /// </summary>
            public bool Locked { get; set; }

            /// <summary>
            /// 射程 1:短 2:中 3:長 4:超長
            /// </summary>
            public int ShotRange { get; protected set; }

            /// <summary>
            /// 射程文字列
            /// </summary>
            public string ShotRangeString
            {
                get
                {
                    return MasterData.Item.GetShotRangeString(ShotRange);
                }
            }

            /// <summary>
            /// 船速 5:低速 10:高速
            /// </summary>
            public int Speed { get; protected set; }

            /// <summary>
            /// 船速文字列
            /// </summary>
            public string SpeedString
            {
                get
                {
                    switch (Speed)
                    {
                        case 5:
                            return "低速";
                        case 10:
                            return "高速";
                        default:
                            return string.Format("未知({0})", Speed);
                    }
                }
            }

            /// <summary>
            /// 搭載するアイテム
            /// </summary>
            public List<SlotItemInfo> SlotItem { get; protected set; }

            /// <summary>
            /// 装備スロット数
            /// </summary>
            public int SlotNum { get; protected set; }

            /// <summary>
            /// 艦隊名
            /// </summary>
            public string Fleet
            {
                get
                {
                    if (FleetNum > 0 && FleetOrder > 0)
                        return string.Format("{0}({1})", FleetName, FleetOrder);
                    else
                        return "";

                }
            }

            /// <summary>
            /// 艦隊名
            /// </summary>
            public string FleetName { get; private set; }

            /// <summary>
            /// 艦隊内序列(1-6) 0:無所属
            /// </summary>
            public int FleetOrder { get; private set; }

            /// <summary>
            /// 艦隊番号(1-4) 0:無所属
            /// </summary>
            public int FleetNum { get; private set; }

            /// <summary>
            /// 入渠ドック番号(1-4) 0:未入渠
            /// </summary>
            public int DockNum { get; private set; }

            /// <summary>
            /// 入渠ドック名
            /// </summary>
            public string DockName
            {
                get
                {
                    if (DockNum == 0)
                        return "";
                    else
                        return string.Format("ドック{0}", DockNum);
                }
            }

            /// <summary>
            /// 制空値
            /// </summary>
            public int AirSuperiorityAbility
            {
                get
                {
                    int airSuperiority = 0;
                    foreach (var slotItem in SlotItem)
                        airSuperiority += slotItem.AirSuperiorityAbility;

                    return airSuperiority;
                }
            }

            /// <summary>
            /// 航空機による索敵値
            /// </summary>
            public int AircraftSearch
            {
                get
                {
                    int airSearch = 0;
                    foreach (var slotItem in SlotItem)
                        airSearch += slotItem.AirSearch;

                    return airSearch;
                }
            }

            /// <summary>
            /// 電探による索敵値
            /// </summary>
            public int RadarSearch
            {
                get
                {
                    int radarSearch = 0;
                    foreach (var slotItem in SlotItem)
                        radarSearch += slotItem.RadarSearch;

                    return radarSearch;
                }

            }

            /// <summary>
            /// 修理時間を算出する際のパラメタ
            /// </summary>
            public int RepairTimeParam { get; protected set; }

            /// <summary>
            /// 出撃エリア(御札)
            /// </summary>
            public int SallyArea { get;protected set; }

            #endregion パラメータ

            /// <summary>
            /// 更新時刻
            /// </summary>
            public DateTime LastUpdated { get; protected set; }

            #region 背景色や経験値算出サブルーチン
            /// <summary>
            /// コンディション値に応じた色の取得
            /// 色は http://wikiwiki.jp/kancolle/?%A4%DE%A4%E1%A4%C1%A4%B7%A4%AD を参考にした
            /// </summary>
            /// <param name="condVal">コンディション値</param>
            /// <returns>System.Drawing.Color</returns>
            public static Color GetConditionColor(int condVal)
            {
                if (condVal < 20)
                    return Color.LightPink;
                if (condVal < 30)
                    return Color.Gold;
                if (condVal < 40)
                    return Color.Beige;
                if (condVal < 50)
                    return SystemColors.Window;

                return Color.LightCyan;
            }

            /// <summary>
            /// 各レベルに達するに必要な最小累積経験値を算出
            /// http://wikiwiki.jp/kancolle/?%A4%DE%A4%E1%A4%C1%A4%B7%A4%AD を参考にした
            /// </summary>
            /// <param name="nLevel">算出するレベル</param>
            /// <returns>必要な経験値</returns>
            public static int GetMinimumExperience(int nLevel)
            {
                //レベル51まで
                if (nLevel <= 51)
                    return 50 * nLevel * (nLevel - 1);
                //レベル52から61まで
                if (nLevel <= 61)
                    return 100 * nLevel * (nLevel - 51) + 127500;
                //レベル62から71まで
                if (nLevel <= 71)
                    return 50 * (nLevel - 61) * (3 * nLevel - 40) + 188500;
                //レベル72から81まで
                if (nLevel <= 81)
                    return 200 * (nLevel - 70) * (nLevel - 21) + 265000;
                //レベル82から91まで
                if (nLevel <= 91)
                    return 50 * (nLevel - 81) * (5 * nLevel - 120) + 397000;
                //レベル92から99まで
                if (nLevel <= 99)
                    switch (nLevel)
                    {
                        case 92:
                            return 584500;
                        case 93:
                            return 606500;
                        case 94:
                            return 631500;
                        case 95:
                            return 661500;
                        case 96:
                            return 701500;
                        case 97:
                            return 761500;
                        case 98:
                            return 851500;
                        case 99:
                            return 1000000;
                    }
                //レベル100はケッコンカッコカリすることで到達する
                if (nLevel == 100)
                    return 1000000;
                //レベル101からレベル111まで
                if (nLevel <= 111)
                    return 500 * (nLevel - 101) * (nLevel - 100) + 10000 + 1000000;
                //レベル111からレベル115まで
                if (nLevel <= 115)
                    return 1000 * (nLevel - 101) * (nLevel - 110) + 55000 + 1000000;
                //レベル116からレベル120まで
                if (nLevel <= 120)
                    return 500 * (nLevel - 115) * (3 * nLevel - 308) + 125000 + 1000000;
                //レベル121からレベル130まで
                if (nLevel <= 130)
                    return 1000 * (nLevel - 121) * (2 * nLevel - 205) + 290000 + 1000000;
                //レベル131からレベル140まで
                if (nLevel <= 140)
                    return 2500 * (nLevel - 131) * (nLevel - 100) + 860000 + 1000000;
                //レベル141からレベル145まで
                if (nLevel <= 145)
                    return 500 * (nLevel - 140) * (7 * nLevel - 733) + 1760000 + 1000000;
                //レベル146からレベル150まで
                if (nLevel <= 150)
                    return 1000 * (nLevel - 145) * (4 * nLevel - 421) + 2465000 + 1000000;
                //レベル151から155まで
                if(nLevel <= 155)
                    switch(nLevel)
                    {
                        case 151:
                            return 3564000;
                        case 152:
                            return 3777000;
                        case 153:
                            return 3999000;
                        case 154:
                            return 4230000;
                        case 155:
                            return 4470000;
                    }

                throw new ArgumentOutOfRangeException("Invalid Level param:" + nLevel.ToString());
            }


            /// <summary>
            /// 次レベルに至るに必要な経験値
            /// </summary>
            /// <param name="nCurrentLevel">今のレベル</param>
            /// <param name="nCurrentExp">今の経験値</param>
            /// <returns>まだ必要な経験値</returns>
            static int GetStepupRequiredExperience(int nCurrentLevel, int nCurrentExp)
            {
                if (nCurrentLevel < 150)
                    return GetMinimumExperience(nCurrentLevel + 1) - nCurrentExp;

                return 0;
            }

            /// <summary>
            /// 改造可能なレベルに達した場合に背景色を変える
            /// </summary>
            /// <param name="nCurrentLevel">今のレベル</param>
            /// <param name="nUpgradeLevel">改造可能レベル</param>
            /// <returns>色</returns>
            static Color GetUpgradeColor(int nCurrentLevel, int nUpgradeLevel)
            {
                //もう改造できないときはUpgradeLevel==0になってる
                if (nUpgradeLevel > 0 && (nCurrentLevel >= nUpgradeLevel))
                    return Color.LightGreen;

                return SystemColors.Window;

            }

            /// <summary>
            /// 改造に必要な経験値
            /// </summary>
            /// <param name="nCurrentExp">今の経験値</param>
            /// <param name="nUpgradeLv">改造レベル</param>
            /// <returns></returns>
            static int GetUpgradeExperience(int nCurrentExp, int nUpgradeLv)
            {
                if (nUpgradeLv == 0)
                    return 0;

                int nUpgradeExp = GetMinimumExperience(nUpgradeLv) - nCurrentExp;

                return nUpgradeExp > 0 ? nUpgradeExp : 0;
            }


            #endregion 背景色や経験値算出サブルーチン

            /// <summary>
            /// コピーコンストラクタ
            /// </summary>
            /// <param name="org">元</param>
            public Info(Info org)
            {
                ShipNameId = org.ShipNameId;
                ShipSortNo = org.ShipSortNo;

                ShipName = org.ShipName;
                ShipNameYomi = org.ShipNameYomi;

                ShipId = org.ShipId;
                RepairTime = org.RepairTime;
                ShipTypeId = org.ShipTypeId;
                ShipType = org.ShipType;
                Speed = org.Speed;

                HP = new NowMax(org.HP);
                Fuel = new NowMax(org.Fuel);
                Bullet = new NowMax(org.Bullet);
                Experience = org.Experience;
                Level = org.Level;
                UpdateLevel = org.UpdateLevel;
                Condition = org.Condition;

                Escape = new NowMax(org.Escape);
                Fire = new NowMax(org.Fire);
                Lucky = new NowMax(org.Lucky);
                Torpedo = new NowMax(org.Torpedo);
                Search = new NowMax(org.Search);
                Armor = new NowMax(org.Armor);
                AntiAir = new NowMax(org.AntiAir);
                AntiSubm = new NowMax(org.AntiSubm);

                ShotRange = org.ShotRange;
                Locked = org.Locked;

                FleetName = org.FleetName;
                FleetOrder = org.FleetOrder;
                FleetNum = org.FleetNum;
                DockNum = org.DockNum;

                SlotItem = new List<SlotItemInfo>();
                foreach (var it in org.SlotItem)
                    SlotItem.Add(new SlotItemInfo(it));
                SlotNum = org.SlotNum;

                AppliedSlotItemInfo = org.AppliedSlotItemInfo;
                RepairTimeParam = org.RepairTimeParam;
                SallyArea = org.SallyArea;

                LastUpdated = org.LastUpdated;

            }

            /// <summary>
            /// IClonableインタフェイス実装
            /// </summary>
            /// <returns>複製されたオブジェクト</returns>
            public virtual object Clone()
            {
                return new Info(this);
            }

            /// <summary>
            /// APIの返すパラメータはアイテムにより強化された値を含んでいるので、
            /// 強化された分を減算して艦娘の生パラメタを算出する
            /// </summary>
            /// <param name="itemInfo"></param>
            void TrimItemParameters(Item.Info itemInfo)
            {
                AntiAir.Now -= itemInfo.対空;
                if (AntiAir.Now < 0)
                    AntiAir.Now = 0;

                Armor.Now -= itemInfo.装甲;
                if (Armor.Now < 0)
                    Armor.Now = 0;

                Fire.Now -= itemInfo.火力;
                if (Fire.Now < 0)
                    Fire.Now = 0;

                Torpedo.Now -= itemInfo.雷撃;
                if (Torpedo.Now < 0)
                    Torpedo.Now = 0;
                
                AntiSubm.Now -= itemInfo.対潜;
                if(AntiSubm.Now < 0)
                    AntiSubm.Now = 0;

                Search.Now -= itemInfo.索敵;
                if(Search.Now < 0)
                    Search.Now = 0;

                Escape.Now -= itemInfo.砲撃回避;
                if(Escape.Now < 0)
                    Escape.Now = 0;


            }

            /// <summary>
            /// 装備情報を反映させる
            /// </summary>
            /// <param name="itemMember"></param>
            public void ApplySlotItemData(Item itemMember)
            {
                if (AppliedSlotItemInfo)
                    return;

                //                    Debug.WriteLine("ApplySlotItemData:" + ShipName);
                foreach (var it2 in SlotItem)
                {
                    if (it2.ID > 0)
                    {
                        var item = itemMember.GetItem(it2.ID);
                        if (item == null)
                        {
                            //                                Debug.WriteLine(string.Format("ItemID:{0} info not found", it2.ID));
                            continue;
                        }

                        //                            Debug.WriteLine(string.Format("ItemID:{0} -> {1}", it2.ID,item.ToString()));
                        it2.Info = item;
                        //アイテムにより加算されたパラメータを消す
                        TrimItemParameters(item);
                        AppliedSlotItemInfo = true;
                    }
                }
                Refresh();
            }

            /// <summary>
            /// 装備情報が反映されたかどうか
            /// </summary>
            public bool AppliedSlotItemInfo { get; set; }

            /// <summary>
            /// 修復に必要な燃料
            /// http://wikiwiki.jp/kancolle/?%C6%FE%B5%F4#w5a2818b 参照
            /// </summary>
            public int RepairFuel
            {
                get
                {
                    return (int)Math.Floor(Fuel.Max * 0.2 * HP.Diff * 0.16);
                }
            }

            /// <summary>
            /// 修復に必要な鋼材
            /// http://wikiwiki.jp/kancolle/?%C6%FE%B5%F4#w5a2818b
            /// </summary>
            public int RepairSteel
            {
                get
                {
                    return (int)Math.Floor(Fuel.Max * 0.2 * HP.Diff * 0.3);
                }
            }

            /// <summary>
            /// 修理時間を算出するレベル由来パラメタ
            /// http://wikiwiki.jp/kancolle/?%C6%FE%B5%F4#i43fa2e3
            /// </summary>
            /// <param name="level"></param>
            /// <returns></returns>
            private static int RepairLVParam(int level)
            {
                if (level < 12)
                    return level * 10;

                return (int)(Math.Floor(Math.Sqrt(level - 11)) * 10 + 50) + level * 5;
            }

            /// <summary>
            /// 修理時間(秒数)を算出
            /// http://wikiwiki.jp/kancolle/?%C6%FE%B5%F4#i43fa2e3
            /// </summary>
            /// <param name="level">レベル</param>
            /// <param name="hp_diff">HP減少分</param>
            /// <param name="param">艦種別パラメタ</param>
            /// <returns></returns>
            public static double CalcRepairTime(int level, int hp_diff, int param)
            {
                return RepairLVParam(level) * param * hp_diff * 0.5 + 30;
            }

            /// <summary>
            /// 修理時間をLvから算出した値に補正
            /// http://wikiwiki.jp/kancolle/?%C6%FE%B5%F4#i43fa2e3
            /// </summary>
            public void UpdateRepairTime()
            {
                long repairTime = (long)CalcRepairTime(Level, HP.Diff, RepairTimeParam);
                var ts = new TimeSpan(TimeSpan.TicksPerSecond * repairTime);
                Debug.WriteLine(string.Format("UpdateRepairTime:{0}(Lv{1}) HPDiff:{2} Time:{3}",
                    ShipName, Level, HP.Diff, ts));
                RepairTime = ts;
            }

            /// /kcsapi/api_get_member/ship2
            /// /kcsapi/api_get_member/ship3
            public Info(KCB.api_get_member.ApiDataShip it, MasterData.Ship shipMaster)
            {
                SlotItem = new List<SlotItemInfo>();
                HP = new NowMax();
                Fuel = new NowMax();
                Bullet = new NowMax();

                Escape = new NowMax();
                Fire = new NowMax();
                Lucky = new NowMax();
                Torpedo = new NowMax();
                Search = new NowMax();
                Armor = new NowMax();
                AntiAir = new NowMax();
                AntiSubm = new NowMax();

                Update(it, shipMaster);
            }

            /// /kcsapi/api_get_member/ship_deck 
            public void Update(KCB.api_get_member.ApiDataShip it, MasterData.Ship shipMaster)
            {
                ShipNameId = it.api_ship_id;
                ShipSortNo = it.api_sortno;

                var shipData = shipMaster.LookupShipMaster(ShipNameId);

                ShipName = shipData.Name;

                //                    Debug.WriteLine("Load2:" + ShipName);
                ShipId = it.api_id;
                RepairTime = new TimeSpan((long)it.api_ndock_time * TimeSpan.TicksPerMillisecond);
                ShipTypeId = shipData.ShipTypeId;
                ShipType = shipData.ShipTypeName;
                ShipNameYomi = shipData.Yomi;
                RepairTimeParam = shipData.RepairTimeParam;
                Speed = shipData.Speed;

                HP.Update(it.api_nowhp, it.api_maxhp);
                Fuel.Update(it.api_fuel, shipData.MaxFuel);
                Bullet.Update(it.api_bull, shipData.MaxBullet);

                //                    Debug.WriteLine(string.Format("ID:{0} Exp:{1},{2},{3}", ShipId, (int)it.api_exp[0], (int)it.api_exp[1], (int)it.api_exp[2]));

                Experience = (int)it.api_exp[0];
                Level = (int)it.api_lv;
                UpdateLevel = (int)shipData.AfterLV;
                Condition = (int)it.api_cond;

                Escape.Update(it.api_kaihi[0], it.api_kaihi[1]);
                Fire.Update(it.api_karyoku[0], it.api_karyoku[1]);
                Lucky.Update(it.api_lucky[0], it.api_lucky[1]);
                Torpedo.Update(it.api_raisou[0], it.api_raisou[1]);
                Search.Update(it.api_sakuteki[0], it.api_sakuteki[1]);
                Armor.Update(it.api_soukou[0], it.api_soukou[1]);
                AntiAir.Update(it.api_taiku[0], it.api_taiku[1]);
                AntiSubm.Update(it.api_taisen[0], it.api_taisen[1]);

                ShotRange = (int)it.api_leng;
                Locked = (int)it.api_locked == 1;

                SlotItem.Clear();
                for (int n = 0; n < it.api_slotnum; n++)
                {
                    int item_id = (int)it.api_slot[n];
                    //                        if (item_id == -1)
                    //                            continue;

                    SlotItem.Add(new SlotItemInfo(item_id, (int)it.api_onslot[n]));
                }
                SlotNum = shipData.SlotNum;
                AppliedSlotItemInfo = false;
                SallyArea = it.api_sally_area;

                Refresh();
            }

            /// <summary>
            /// 最終更新時刻を更新
            /// </summary>
            public void Refresh()
            {
                LastUpdated = DateTime.Now;
            }

        }

        /// <summary>
        /// アイテムデータを艦娘一覧に反映する.アイテムにより加算された火力などを減算
        /// </summary>
        /// <param name="itemData">アイテム情報</param>
        public void ApplySlotItemData(Item memberItem)
        {
            foreach (var it in _shipDic)
            {
                it.Value.ApplySlotItemData(memberItem);
            }
        }

        /// <summary>
        /// 修理ドック情報を反映
        /// </summary>
        /// <param name="dockData">ドック情報</param>
        public void UpdateNDockInfo(Dock dockData)
        {
            foreach (var it in _shipDic)
            {
                it.Value.UpdateNDock(dockData.GetNDockNum(it.Key));
            }
        }

        /// <summary>
        /// 艦隊情報を反映
        /// </summary>
        /// <param name="deckData">艦隊情報</param>
        public void UpdateDeckInfo(Deck deckData)
        {
            foreach (var it in _shipDic)
            {
                var inf = deckData.GetShipDeckData(it.Key);
                it.Value.UpdateDeckInfo(inf);
            }
        }

        /// <summary>
        /// 現状の艦娘一覧を返す。Listは呼ぶ度に作られる
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Info> ShipList
        {
            get
            {
                var retList = new List<Info>();
                lock (_shipDic)
                {
                    foreach (var it in _shipDic)
                        retList.Add(it.Value);
                }
                return retList;
            }
        }

        /// <summary>
        /// 艦船の修復情報を反映
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns>高速修復の時trueを返す</returns>
        public bool RepairShip(IDictionary<string, string> queryParam)
        {
            string ship_id = queryParam["api_ship_id"];
            string highspeed = queryParam["api_highspeed"];
            var ship = GetShip(ship_id);

            if (highspeed == "1")
            {
                Debug.WriteLine("Using FastRepair");

                //即座にHPがMAXに戻る。NDockはいじらなくてよい
                ship.HP.Now = ship.HP.Max;
                ship.Refresh();
                return true;
            }

            return false;

        }

        /// <summary>
        /// 指定した艦娘の修理が完了した
        /// </summary>
        /// <param name="ship_id">艦娘ID</param>
        public void RepairShip(int ship_id)
        {
            var ship = GetShip(ship_id);

            ship.HP.Now = ship.HP.Max;
            ship.Refresh();
            ship.UpdateNDock(0);
        }

        /// <summary>
        /// 戦闘による損傷を反映する
        /// </summary>
        /// <param name="result"></param>
        public void ApplyBattleResult(BattleResult.Result result)
        {
            //戦闘が一度も解析されていないとき
            if (result == null)
                return;

            foreach (var it in result.Friend.Ships)
            {
                if (!it.Valid)
                    continue;

                var ship = GetShip(it.ShipID);

                //同じ艦娘の情報を複数スレッドから同時にいじる可能性を潰す
                lock (ship)
                {
                    ship.HP.Now = it.CurrentHP;
                    ship.UpdateRepairTime();
                    ship.Refresh();
                }
            }
        }

        /// <summary>
        /// 装備スロット順の入れ替えを反映
        /// </summary>
        /// <param name="shipId">装備スロット入れ替え対象</param>
        /// <param name="slotItemOrder">入れ替え後のスロット順番</param>
        public int UpdateSlotItemOrder(string shipId_s, List<int> slotItemOrder)
        {
            var ship = GetShip(shipId_s);
            if (ship == null)
                return -1 ;

            lock (ship)
            {
                Dictionary<int, Info.SlotItemInfo> dic = ship.SlotItem.ToDictionary(n => n.ID);
                ship.SlotItem.Clear();

                foreach (var id in slotItemOrder)
                {
                    if (id == -1)
                        continue;

                    ship.SlotItem.Add(dic[id]);
                    ship.Refresh();
                }
            }

            return ship.ShipId;
        }
    }
}
