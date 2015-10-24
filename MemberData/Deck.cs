using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KCB2.MemberData
{
    /// <summary>
    /// 艦隊情報
    /// /kcsapi/api_port/port
    /// /kcsapi/api_req_member/updatedeckname
    /// /kcsapi/api_req_hensei/change
    /// /kcsapi/api_get_member/deck
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// UNIX EPOCH(1970年1月1日午前零時 UTC)
        /// </summary>
        static DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 艦隊一覧
        /// </summary>
        List<Fleet> _deck = new List<Fleet>();
        public int Secretary { get; private set; }

        /// <summary>
        /// 艦娘IDをキーとした艦隊情報
        /// </summary>
        ConcurrentDictionary<int, ShipDeckData> _deckMember = new ConcurrentDictionary<int, ShipDeckData>();

        /// <summary>
        /// 艦隊情報を更新。生JSON受け
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool UpdateDeck(string JSON, MasterData.Mission masterMission)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.Deck>(JSON);
            if ((int)json.api_result != 1)
                return false;

            return UpdateDeck(json.api_data, masterMission);
        }

        /// <summary>
        /// 艦隊情報を更新。member/ship3 port対応
        /// 一分艦隊の情報だけ降ってくる場合はまだ非対応。
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool UpdateDeck(List<KCB.api_get_member.ApiDataDeck> json, MasterData.Mission masterMission)
        {
            lock (_deck)
            {
                _deck.Clear();
                foreach (var item in json)
                {
                    _deck.Add(new Fleet(item, masterMission));

                    //第一艦隊の秘書官が旗艦
                    if(item.api_id == 1)
                    {
                        Secretary = item.api_ship[0];
                        Debug.WriteLine("SecretaryShip:" + Secretary.ToString());
                    }
                }
                _deck.Sort();
            }

            UpdateShipDeckDataList(masterMission);

            return true;
        }

        /// <summary>
        /// 艦隊一覧が更新されたので艦隊メンバーハッシュを更新
        /// </summary>
        /// <param name="masterMission">任務マスター</param>
        void UpdateShipDeckDataList(MasterData.Mission masterMission)
        {
            _deckMember.Clear();
            lock (_deck)
            {
                foreach (var it in _deck)
                {
                    Debug.WriteLine("Deck:" + it.ToString());

                    for (int n = 0; n < it.Member.Count; n++)
                    {
                        _deckMember[it.Member[n]] = new ShipDeckData(it.Name, it.Num, it.MissionNum, n + 1, masterMission);
                    }
                }
            }

        }

        /// <summary>
        /// 指定した艦娘IDの艦隊情報を返す
        /// </summary>
        /// <param name="shipID">艦娘ID</param>
        /// <returns>情報 無所属ならnull</returns>
        public ShipDeckData GetShipDeckData(int shipID)
        {
            if (_deckMember.ContainsKey(shipID))
                return _deckMember[shipID];
            return null;
        }

        /// <summary>
        /// 艦隊メンバー変更
        /// /kcsapi/api_req_hensei/change
        /// </summary>
        /// <param name="req">リクエスト</param>
        /// <param name="memberShip">艦隊メンバー情報</param>
        public void ChangeDeckMember(IDictionary<string, string> req, Ship memberShip, MasterData.Mission masterMission)
        {
            //操作する艦隊番号(1-4)
            string deck_id_s = req["api_id"];
            //投入/削除する位置(0-5) -1で旗艦以外全員外す
            string ship_index_s = req["api_ship_idx"];
            //メンバー(-1で該当位置のメンバー削除)
            string ship_id_s = req["api_ship_id"];

            int deck_id = int.Parse(deck_id_s);
            int ship_index = int.Parse(ship_index_s);
            int ship_id = int.Parse(ship_id_s);

            Debug.WriteLine(string.Format("Deck:{0} Order:{1} Ship:{2}", deck_id, ship_index, ship_id));

            //deckの操作を開始するのでlockする
            lock (_deck)
            {
                if (ship_id > 0)
                {
                    //メンバーを入れ替える

                    //秘書艦入れ替えた場合
                    if (deck_id == 1 && ship_index == 0)
                    {
                        Debug.WriteLine(string.Format("Change secretary {0} -> {1}", Secretary, ship_id));
                        Secretary = ship_id;
                    }

                    //既に艦隊にいる艦娘の場合は書き換える。
                    ShipDeckData ship1 = GetShipDeckData(ship_id);
                    if (ship1 != null)
                    {
                        //指定した船が現在いる場所を取得
                        int deck = ship1.Num;
                        int order = ship1.Order - 1;
                        Debug.WriteLine(string.Format("OldDeck:{0} OldOrder:{1}", deck, order));
                        Debug.WriteLine(string.Format("ShipDeck{0}.Count:{1} ShipIndex:{2}",
                            deck_id - 1, _deck[deck_id - 1].Member.Count, ship_index));

                        if (_deck[deck_id - 1].Member.Count > ship_index)
                        {
                            //挿入先が既に存在していれば、入れ替え
                            int currentShipId = _deck[deck_id - 1].Member[ship_index];
                            ShipDeckData ship2 = GetShipDeckData(currentShipId);

                            int ndeck = ship2.Num;
                            int norder = ship2.Order - 1;
                            Debug.WriteLine(string.Format("SwapMember ndeck:{0} norder:{1} currentShipId:{2}",
                                ndeck, norder, currentShipId));

                            //Fleetを入れ替え
                            _deck[deck_id - 1].Member[ship_index] = _deck[deck - 1].Member[order];
                            _deck[deck - 1].Member[order] = currentShipId;

                        }
                        else
                        {
                            //指定した艦隊に追加

                            //挿入先が空白なので、挿入元から指定情報を削除
                            Debug.WriteLine(string.Format("Removeat deck{0} order:{1}", deck - 1, order));
                            _deck[deck - 1].Member.RemoveAt(order);

                            //挿入先へ艦娘IDを挿入
                            Debug.WriteLine(string.Format("Insert deck{0} index:{1} id:{2}", deck_id - 1, ship_index, ship_id));
                            _deck[deck_id - 1].Member.Insert(ship_index, ship_id);
                        }

                    }
                    else
                    {
                        //新規参入
                        _deckMember[ship_id] = new ShipDeckData(_deck[deck_id - 1].Name, deck_id,
                            _deck[deck_id - 1].MissionNum, ship_index + 1, masterMission);

                        if (_deck[deck_id - 1].Member.Count > ship_index)
                        {
                            //すでにある場所へ挿入する場合
                            int currentShipId = _deck[deck_id - 1].Member[ship_index];
                            _deck[deck_id - 1].Member[ship_index] = ship_id;

                        }
                        else
                        {
                            //何もいない場所へ挿入する場合
                            _deck[deck_id - 1].Member.Insert(ship_index, ship_id);
                        }

                    }

                }
                else
                {
                    //メンバーを削除
                    if (ship_index >= 0)
                        _removeShip(ship_index, deck_id);
                    else
                    {
                        //旗艦以外全員外す
                        if (_deck[deck_id - 1].Member.Count > 1)
                        {
                            int member = _deck[deck_id - 1].Member.Count - 1;

                            //後ろから消す
                            for (int idx = member; idx > 0; idx--)
                                _removeShip(idx, deck_id);
                        }
                    }
                }

            }//lock

            //deckが更新されたので反映させる
            UpdateShipDeckDataList(masterMission);

        }

        /// <summary>
        /// 指定deckの指定index 艦船をdeckから削除
        /// </summary>
        /// <param name="ship_index">艦船index(0-5)</param>
        /// <param name="deck_id">艦隊番号(1-4)</param>
        void _removeShip(int ship_index, int deck_id)
        {
            int targetShipId = _deck[deck_id - 1].Member[ship_index];
            Debug.WriteLine(string.Format("Remove Ship fleet:{0} order:{1} ID:{2}", deck_id, ship_index, targetShipId));

            _deck[deck_id - 1].Member.RemoveAt(ship_index);

        }

        /// <summary>
        /// 艦隊名更新
        /// /kcsapi/api_req_member/updatedeckname
        /// </summary>
        /// <param name="req"></param>
        public void UpdateDeckName(IDictionary<string, string> req)
        {
            string deck_id_s = req["api_deck_id"];
            string deck_name = req["api_name"];

            int deck_id = int.Parse(deck_id_s);

            Debug.WriteLine(string.Format("UpdateDeckName({0}) -> {1}", deck_id, deck_name));

            //艦隊リスト更新
            lock (_deck)
            {
                foreach (var deck in _deck)
                {
                    if (deck.Num == deck_id)
                        deck.Name = deck_name;
                }
            }


            //艦娘リスト更新
            foreach (var ship in _deckMember)
            {
                if (ship.Value.Num == deck_id)
                    ship.Value.Name = deck_name;
            }
        }

        /// <summary>
        /// 艦隊一覧を返す(要lock)
        /// </summary>
        public IEnumerable<Fleet> DeckList
        {
            get
            {
                return _deck;
            }
        }

        /// <summary>
        /// 指定番号の艦隊情報を返す
        /// </summary>
        /// <param name="fleetNum">艦隊番号(1-4)</param>
        /// <returns>艦隊情報。失敗したらnull</returns>
        public Fleet GetFleet(int fleetNum)
        {
            Fleet retVal = null;
            lock (_deck)
            {
                foreach (var it in _deck)
                {
                    if (it.Num == fleetNum)
                    {
                        retVal = it;
                        break;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// 遠征開始しました
        /// </summary>
        /// <param name="req"></param>
        public void StartMission(IDictionary<string, string> req, MasterData.Mission masterMission)
        {
            /*
             *[api_mission_id] Value:[5]
             *   [api_deck_id] Value:[3]
             */

            //TODO 遠征開始したのでUIを更新する

        }

        /// <summary>
        /// 艦隊情報(艦隊単位)
        /// </summary>
        public class Fleet : IComparable<Fleet>
        {
            /// <summary>
            /// 艦隊名
            /// </summary>
            public string Name { get; internal set; }

            /// <summary>
            /// 艦隊番号(1-4)
            /// </summary>
            public int Num { get; private set; }

            /// <summary>
            /// 遠征番号
            /// </summary>
            public int MissionNum { get; private set; }

            /// <summary>
            /// 遠征名称
            /// </summary>
            public string MissionName { get; private set; }

            /// <summary>
            /// 遠征名
            /// </summary>
            public string Mission
            {
                get
                {
                    if (MissionNum > 0)
                    {
                        if (MissionName.Length > 0)
                            return string.Format("No.{0} {1}", MissionNum, MissionName);
                        return string.Format("No.{0}", MissionNum);
                    }
                    return "";
                }
            }

            /// <summary>
            /// 遠征内容
            /// </summary>
            public string MissionDetail { get; internal set; }

            /// <summary>
            /// 遠征終了時刻
            /// </summary>
            public DateTime MissionFinish { get; private set; }

            /// <summary>
            /// 艦隊メンバ
            /// </summary>
            public List<int> Member { get; private set; }

            public Fleet(KCB.api_get_member.ApiDataDeck json, MasterData.Mission masterMission)
            {
                Name = json.api_name;
                Num = json.api_id;
                MissionNum = (int)json.api_mission[1];
                MissionFinish = _epoch.AddMilliseconds(json.api_mission[2]).ToLocalTime();

                var mission = masterMission.GetMissionInfo(MissionNum);

                if (mission == null)
                {
                    MissionName = "";
                    MissionDetail = "";
                }
                else
                {
                    MissionName = mission.Name;
                    MissionDetail = mission.Detail;
                }


                List<int> shipList = new List<int>();

                foreach (var ship in json.api_ship)
                {
                    if (ship == -1)
                        continue;

                    shipList.Add(ship);
                }
                Member = shipList;

            }

            public override string ToString()
            {
                return string.Format("Fleet({0}) Name:{1} Mission:{2}({3}) -> {4} Member:{5}",
                    Num, Name, Mission, MissionNum, MissionFinish, string.Join(",", Member));
            }

            public int CompareTo(Fleet it)
            {
                return Num - it.Num;
            }
        }

        /// <summary>
        /// 艦隊情報(艦娘単位)
        /// </summary>
        public class ShipDeckData
        {
            /// <summary>
            /// 艦隊名
            /// </summary>
            public string Name { get; internal set; }
            /// <summary>
            /// 艦隊番号(1-4)
            /// </summary>
            public int Num { get; private set; }
            /// <summary>
            /// 遠征番号
            /// </summary>
            public int MissionNum { get; private set; }
            /// <summary>
            /// 遠征名称
            /// </summary>
            public string MissionName { get; internal set; }

            /// <summary>
            /// 遠征名
            /// </summary>
            public string Mission
            {
                get
                {
                    if (MissionNum > 0)
                    {
                        if (MissionName.Length > 0)
                            return MissionName;
                        return string.Format("No.{0} {1}", MissionNum, MissionName);
                    }
                    return "";
                }
            }

            /// <summary>
            /// 艦隊内序列(1-6)
            /// </summary>
            public int Order { get; private set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="_name">艦隊名</param>
            /// <param name="_num">艦隊番号(1-4)</param>
            /// <param name="_missionNum">遠征番号</param>
            /// <param name="_order">艦隊内序列(1-6)</param>
            /// <param name="masterMission"></param>
            public ShipDeckData(string _name, int _num, int _missionNum, int _order, MasterData.Mission masterMission)
            {
                Name = _name; Num = _num; MissionNum = _missionNum; Order = _order;

                var mission = masterMission.GetMissionInfo(MissionNum);

                if (mission == null)
                    MissionName = "";
                else
                    MissionName = mission.Name;
            }

            /// <summary>
            /// 艦隊内位置を変更
            /// </summary>
            /// <param name="deck_id">艦隊番号(1-4)</param>
            /// <param name="ship_order">艦隊内位置(1-6)</param>
            public void UpdateShipPosition(int deck_id, int ship_order)
            {
                Debug.Assert(deck_id >= 1 && deck_id <= 4);
                Debug.Assert(ship_order >= 1 && ship_order <= 6);
                Debug.WriteLine(string.Format("UpdateShipPosition Num:{0}->{1} Order:{2}->{3}",
                    Num, deck_id, Order, ship_order));
                Num = deck_id;
                Order = ship_order;
            }
        }
    }

}
