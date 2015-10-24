using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KCB2.MemberData
{
    /// <summary>
    /// アイテム情報
    /// /kcsapi/api_get_member/slotitem
    /// </summary>
    public class Item
    {
        /// <summary>
        /// アイテムUIDをキーとする装備のハッシュ
        /// </summary>
        ConcurrentDictionary<int, Info> _itemDic = new ConcurrentDictionary<int, Info>();

        /// <summary>
        /// アイテム一覧の更新 /kcsapi/api_get_member/slot_item
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns>成功すればtrue</returns>
        public bool UpdateItem(string JSON, MasterData.Item itemMaster)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.Slot_Item>(JSON);
            if ((int)json.api_result != 1)
                return false;

            _itemDic.Clear();

            foreach (var data in json.api_data)
            {
                var item = new Info(data, itemMaster);
                _itemDic[data.api_id] = item;
                //                    Debug.WriteLine("Item:" + item.ToString());
            }
            return true;
        }

        /// <summary>
        /// /kcsapi/api_req_kousyou/createitem 
        /// </summary>
        /// <param name="JSON"></param>
        /// <param name="itemMaster"></param>
        /// <returns></returns>
        public bool AddNewSlotItem(string JSON, MasterData.Item itemMaster)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kousyou.CreateItem>(JSON);
            if (json.api_result != 1)
                return false;
            if (json.api_data.api_create_flag != 1)
                return false;

            var item = new Info(json.api_data.api_slot_item, itemMaster);
            _itemDic[json.api_data.api_slot_item.api_id] = item;

            return true;
        }

        /// <summary>
        /// /kcsapi/api_req_kousyou/getship で生成された新造艦娘の装備を一覧に追加
        /// </summary>
        /// <param name="items">api_slotitem</param>
        /// <param name="itemMaster">装備情報マスタ</param>
        public void AddNewSlotItems(List<KCB.api_get_member.Slot_Item.ApiData> items, MasterData.Item itemMaster)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                var itemInfo = new Info(item, itemMaster);
                _itemDic[item.api_id] = itemInfo;
            }
        }

        /// <summary>
        /// /kcsapi/api_req_kousyou/destroyitem2
        /// </summary>
        /// <param name="destroyItemIDs">api_slotitem_ids</param>
        /// <returns></returns>
        public bool DestoryItem(string destroyItemIDs)
        {
            foreach (var it in destroyItemIDs.Split(','))
            {
                DestoryItem(int.Parse(it));
            }

            return true;
        }

        public bool DestoryItem(int[] destoryItemIDs)
        {
            foreach (var it in destoryItemIDs)
            {
                DestoryItem(it);
            }
            return true;
        }

        public bool DestoryItem(int item_id)
        {
            Info destroyItemInfo;
            Debug.WriteLine("DestroyItem:" + item_id.ToString());
            return _itemDic.TryRemove(item_id, out destroyItemInfo);
        }

        /// <summary>
        /// 装備をロック(解除)
        /// /kcsapi/api_req_kaisou/lock
        /// </summary>
        /// <param name="json"></param>
        /// <param name="slotitem_id"></param>
        /// <returns></returns>
        public APIResponse Lock(string JSON, int slotitem_id)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kaisou.Lock>(JSON);
            if (json.api_result != 1)
                return APIResponse.Error;

            var item = GetItem(slotitem_id);
            if (item == null)
                return APIResponse.Error;

            if (json.api_data.api_locked == 1)
            {
                item.Locked = true;
                return APIResponse.True;
            }
            item.Locked = false;
            return APIResponse.False;

        }

        /// <summary>
        /// 指定した艦娘が保有している装備をリストから削除
        /// </summary>
        /// <param name="ship_id">装備をリストから削除する艦娘</param>
        public void DestroySlottedItem(int ship_id)
        {
            List<Info> targetItemList = new List<Info>();
            foreach (var it in _itemDic)
            {
                if (it.Value.Owner == null)
                    continue;

                if (it.Value.Owner.ID == ship_id)
                    targetItemList.Add(it.Value);
            }

            foreach (var it in targetItemList)
            {
                DestoryItem(it.ItemUID);
            }

        }

        /// <summary>
        /// 装備を改装
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool RemodelSlotItem(string JSON,MasterData.Item itemMaster)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kousyou.RemodelSlot>(JSON);
            if ((int)json.api_result != 1)
                return false;
            var dat = json.api_data;

            if (dat.api_remodel_flag != 1)
                return false;

            var item = GetItem(dat.api_after_slot.api_id);
            item.Remodel(dat.api_after_slot,itemMaster);

            if (dat.api_use_slot_id != null)
                DestoryItem(dat.api_use_slot_id.ToArray());

            return true;
        }

        /// <summary>
        /// 指定アイテムの情報を取得する
        /// </summary>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public Info GetItem(int item_id)
        {
            Info ret;
            if (_itemDic.TryGetValue(item_id, out ret))
                return ret;
            return null;

        }

        /// <summary>
        /// 指定アイテムの情報を取得する
        /// </summary>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public Info GetItem(string item_id_s)
        {
            int item_id;
            if (int.TryParse(item_id_s, out item_id))
                return GetItem(item_id);

            return null;
        }

        /// <summary>
        /// アイテムに所有者情報を付加する
        /// </summary>
        /// <param name="shipData"></param>
        public void UpdateItemOwnerShip(Ship shipData)
        {
            foreach (var it in _itemDic)
            {
                it.Value.Owner = shipData.GetItemOwner(it.Key);
            }
        }

        /// <summary>
        /// アイテム数
        /// </summary>
        public int Count
        {
            get
            {
                return _itemDic.Count;
            }
        }

        /// <summary>
        /// アイテム一覧。Listは呼ぶ度に作られる
        /// </summary>
        public IEnumerable<Info> ItemList
        {
            get
            {
                var retList = new List<Info>();
                foreach (var it in _itemDic)
                    retList.Add(it.Value);
                return retList;

            }
        }
        /// <summary>
        /// アイテムIDから装備種別を引っ張るハッシュを生成して返す
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, int> SlotItemTypeDic
        {
            get
            {
                Dictionary<int, int> ret = new Dictionary<int, int>();

                foreach (var it in _itemDic)
                {
                    ret[it.Key] = it.Value.SlotItemType;
                }

                return ret;
            }
        }

        /// <summary>
        /// アイテム情報
        /// </summary>
        public class Info : ICloneable
        {
            /// <summary>
            /// 装備UID
            /// </summary>
            public int ItemUID { get; private set; }
            /// <summary>
            /// 装備種類UID
            /// </summary>
            public int SlotItemType { get; private set; }
            /// <summary>
            /// 装備名
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// 装備種別文字列
            /// </summary>
            public string Type { get; private set; }
            /// <summary>
            /// 装備種別
            /// </summary>
            public int TypeNum { get; private set; }

            /// <summary>
            /// 装備詳細種別
            /// </summary>
            public int TypeDetailNum { get; private set; }

            /// <summary>
            /// 艦載機かどうか
            /// </summary>
            public bool Aircraft { get; private set; }

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
            public int 雷撃命中 { get; private set; }
            public int 雷撃回避 { get; private set; }
            public int Houk { get; private set; }
            public int 索敵 { get; private set; }
            public int 運 { get; private set; }
            public int Rare { get; private set; }
            public int 射程 { get; private set; }
            public bool Locked { get; set; }
            public int Level { get; private set; }

            public string RareString
            {
                get
                {
                    return MasterData.Item.GetRarityString(Rare);
                }
            }

            public Ship.SlotItemOwner Owner { get; set; }

            public string OwnerString
            {
                get
                {
                    if (Owner != null)
                        return Owner.ToString();
                    else
                        return "";
                }
            }

            /// <summary>
            /// 射程文字列
            /// </summary>
            public string 射程String
            {
                get
                {
                    return MasterData.Item.GetShotRangeString(射程);
                }
            }

            //                public Info(dynamic data)
            public Info(KCB.api_get_member.Slot_Item.ApiData data, MasterData.Item itemMaster)
            {
                Owner = null;
                ItemUID = data.api_id;
                SlotItemType = data.api_slotitem_id;
                Locked = data.api_locked == 1;

                if (data.api_alv > 0)
                    Level = data.api_alv;
                else
                    Level = data.api_level;

                ApplyMasterSlotItemInfo(itemMaster);
            }

            /// <summary>
            /// マスタデータから装備情報を取得して設定
            /// SlotItemTypeは設定済みと仮定
            /// </summary>
            /// <param name="itemMaster"></param>
            private void ApplyMasterSlotItemInfo(MasterData.Item itemMaster)
            {
                var itemInfo = itemMaster.GetItemParam(SlotItemType);
                SetSlotItemParam(itemInfo);
            }

            /// <summary>
            /// マスタデータの装備情報を設定
            /// </summary>
            /// <param name="itemInfo"></param>
            private void SetSlotItemParam(MasterData.Item.Param itemInfo)
            {
                if (itemInfo == null)
                    return;

                Name = itemInfo.Name;
                TypeNum = itemInfo.TypeNum;
                TypeDetailNum = itemInfo.TypeDetailNum;
                Type = itemInfo.Type;
                Aircraft = MasterData.Item.Param.IsAircraft(TypeNum);

                対空 = itemInfo.対空;
                装甲 = itemInfo.装甲;
                火力 = itemInfo.火力;
                雷撃 = itemInfo.雷撃;
                爆装 = itemInfo.爆装;
                耐久 = itemInfo.耐久;
                対潜 = itemInfo.対潜;
                砲撃命中 = itemInfo.砲撃命中;
                砲撃回避 = itemInfo.砲撃回避;
                雷撃命中 = itemInfo.雷撃命中;
                雷撃回避 = itemInfo.雷撃回避;
                索敵 = itemInfo.索敵;
                運 = itemInfo.運;
                Rare = itemInfo.Rare;
                射程 = itemInfo.射程;

                Debug.WriteLine(string.Format("装備情報設定 ID:{0} 名前:{1} 種類:{2}", ItemUID, Name, Type));
            }

            /// <summary>
            /// ToStringオーバライド
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("Owner:{19} ItemUID:{0} SlotItemType:{1} Name:{2} Type:{3} "
                    + "対空:{4} 装甲:{5} 火力:{6} 雷撃:{7} Soku:{8} 爆装:{9} 耐久:{10} 対潜:{11} 命中:{12} 回避:{13} "
                                    + "Houk:{14} 索敵:{15} 運:{16} 射程:{17} Rare:{18} Locked:{20} Lv:{21}", ItemUID, SlotItemType, Name, Type,
                                    対空, 装甲, 火力, 雷撃, Soku, 爆装, 耐久, 対潜, 砲撃命中, 砲撃回避,
                                    Houk, 索敵, 運, 射程, Rare, OwnerString, Locked, Level);
            }

            /// <summary>
            /// コピーコンストラクタ
            /// </summary>
            /// <param name="value"></param>
            public Info(Info value)
            {
                Update(value);
            }

            /// <summary>
            /// 値をアップデートする
            /// </summary>
            /// <param name="value">新しい値</param>
            public void Update(Info value)
            {
                Owner = value.Owner;
                ItemUID = value.ItemUID;
                SlotItemType = value.SlotItemType;
                Name = value.Name;
                TypeNum = value.TypeNum;
                TypeDetailNum = value.TypeDetailNum;
                Type = value.Type;
                Aircraft = value.Aircraft;
                対空 = value.対空;
                装甲 = value.装甲;
                火力 = value.火力;
                雷撃 = value.雷撃;
                Soku = value.Soku;
                爆装 = value.爆装;
                耐久 = value.耐久;
                対潜 = value.対潜;
                砲撃命中 = value.砲撃命中;
                砲撃回避 = value.砲撃回避;
                雷撃命中 = value.雷撃命中;
                雷撃回避 = value.雷撃回避;
                Houk = value.Houk;
                索敵 = value.索敵;
                運 = value.運;
                射程 = value.射程;
                Rare = value.Rare;
                Locked = value.Locked;
                Level = value.Level;
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
            /// 装備改装
            /// </summary>
            /// <param name="data">改装情報</param>
            /// <param name="itemMaster">マスタデータ</param>
            public void Remodel(KCB.api_req_kousyou.RemodelSlot.ApiData.ApiAfterSlot data,
                MasterData.Item itemMaster)
            {
                ItemUID = data.api_id;
                SlotItemType = data.api_slotitem_id;
                Locked = data.api_locked == 1;
                Level = data.api_level;

                ApplyMasterSlotItemInfo(itemMaster);
            }
        }

    }

}
