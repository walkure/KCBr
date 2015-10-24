using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

//using Codeplex.Data;
using Newtonsoft.Json;

///ログ情報を生成するクラス
namespace KCB2.LogGenerator
{
    /// <summary>
    /// 戦闘結果
    /// /kcsapi/api_req_map/start
    ///  /kcsapi/api_req_sortie/battleresult
    /// </summary>
    public class BattleResult
    {
        /// <summary>
        /// 戦闘中の艦隊番号
        /// </summary>
        int battleFleetID = 0;

        /// <summary>
        /// 戦闘開始 /kcsapi/api_req_map/start
        /// </summary>
        /// <param name="req"></param>
        public void Start(IDictionary<string, string> req)
        {
            Debug.WriteLine(string.Format(
                   "StartBattle(Log) Form:{0} Deck:{1} ID:{2} No:{3}",
                       req["api_formation_id"], req["api_deck_id"],
                       req["api_maparea_id"], req["api_mapinfo_no"]));
            battleFleetID = int.Parse(req["api_deck_id"]);
        }

        /// <summary>
        /// 戦闘終了 /kcsapi/api_req_sortie/battleresult
        /// </summary>
        /// <param name="JSON"></param>
        public LogData.BattleResultInfo Finish(string JSON, MemberData.Ship shipData,
            MemberData.Deck deckData,MasterData.Ship shipMaster,MemberData.Basic basicData)
        {
//            var json = DynamicJson.Parse(JSON);
            var json = JsonConvert.DeserializeObject<KCB.api_req_sortie.BattleResult>(JSON);
            if ((int)json.api_result != 1)
                return null;

            var data = json.api_data;

            LogData.BattleResultInfo result = new LogData.BattleResultInfo();
            result.MemberID = basicData.MemberID;
            result.Date = DateTime.Now;
            result.AreaName = data.api_quest_name;
            result.Rank = data.api_win_rank;

            //敵情報
            result.Foe.DeckName = data.api_enemy_info.api_deck_name;
            Debug.WriteLine("FoeDeckName:" + result.Foe.DeckName);
//            double[] enemylist = (double[])data.api_ship_id;

//            for (int i = 1; i < enemylist.Count(); i++)
            for (int i = 1; i < data.api_ship_id.Count(); i++)
            {
                int enemyID = data.api_ship_id[i];
                if (enemyID <= 0)
                {
                    result.Foe.ShipList[i - 1] = "";
                    continue;
                }

                //何故かeliteとかflagの文字は読みに入ってる
                var t = shipMaster.LookupShipMaster(enemyID);
                if (t.Yomi.Length == 1)
                    result.Foe.ShipList[i - 1] = t.Name;
                else
                    result.Foe.ShipList[i - 1] = string.Format("{0}({1})", t.Name, t.Yomi);

                Debug.WriteLine(string.Format("ShipList({0})={1}", i - 1, result.Foe.ShipList[i - 1]));
            }

            //自軍情報
            var fleet = deckData.GetFleet(battleFleetID);
            result.Friend.DeckName = fleet.Name;
            Debug.WriteLine("FriendDeckName:" + result.Friend.DeckName);
            for (int i = 0; i < fleet.Member.Count(); i++)
            {
                if (fleet.Member[i] <= 0)
                {
                    result.Friend.ShipList[i] = "";
                    continue;
                }

                string header = "";
                ///ロスト艦艇
                if (data.api_lost_flag != null && (int)data.api_lost_flag[i + 1] != 0)
                    header = "x";

                //MVPは1オリジン
                if ((int)data.api_mvp == i + 1)
                    header = "*";

                var t = shipData.GetShip(fleet.Member[i]);
                result.Friend.ShipList[i] = string.Format("{0}{1}(Lv{2} HP{3})",
                    header, t.ShipName, t.Level, t.HP);
                Debug.WriteLine(string.Format("ShipList({0})={1}", i, result.Friend.ShipList[i]));
            }

            ///ドロップ
//            if (data.IsDefined("api_get_ship"))
            if(data.api_get_ship != null)
                result.ShipDropped = string.Format("{0}({1})", data.api_get_ship.api_ship_name,
                    data.api_get_ship.api_ship_type);
            Debug.WriteLine("GetShip:" + result.ShipDropped);

            return result;
        }

    }

    /// <summary>
    /// 遠征結果
    /// /kcsapi/api_req_mission/result
    /// </summary>
    public class MissionResult
    {
        /// <summary>
        /// 遠征結果のログ情報を生成
        /// </summary>
        /// <param name="JSON"></param>
        /// <param name="shipList"></param>
        /// <returns></returns>
        public LogData.MissionResultInfo CreateResult(string JSON, MemberData.Ship shipList,MemberData.Basic basicData)
        {
//            var json = DynamicJson.Parse(JSON);
            var json = JsonConvert.DeserializeObject<KCB.api_req_mission.Result>(JSON);
            if ((int)json.api_result != 1)
                return null;

            var result = json.api_data;
            if (result == null)
                return null;

            LogData.MissionResultInfo res = new LogData.MissionResultInfo();
            res.MemberID = basicData.MemberID;
            res.AreaName = result.api_maparea_name;
            res.QuestName = result.api_quest_name;
            res.Date = DateTime.Now;
            res.Succeeded = (int)result.api_clear_result;

            //失敗したとき、api_get_material = -1で帰ってくる
//            Type typMaterial = result.api_get_material.GetType();
//            if (typMaterial != typeof(Codeplex.Data.DynamicJson))
//            if(result.api_get_material is Codeplex.Data.DynamicJson)
//                res.Material = new LogData.Material((double[])result.api_get_material);

            if (result.api_get_material is Newtonsoft.Json.Linq.JArray)
                res.Material = new LogData.Material((Newtonsoft.Json.Linq.JArray)result.api_get_material);
            else
                res.Material = new LogData.Material();

            if (result.api_useitem_flag[0] != 0)
            {
                res.Item.Add(
                    string.Format("{0}x{1}", itemName((int)result.api_useitem_flag[0]
                       , result.api_get_item1.api_useitem_name),
                    (int)result.api_get_item1.api_useitem_count)
                );
            }

            if (result.api_useitem_flag[1] != 0)
            {
                res.Item.Add(
                    string.Format("{0}x{1}", itemName((int)result.api_useitem_flag[1]
                     , result.api_get_item2.api_useitem_name),
                    (int)result.api_get_item2.api_useitem_count)
                );
            }

//            double[] ships = (double[]) result.api_ship_id;
//            for (int n = 1; n < ships.Count(); n++)
            for (int n = 1; n < result.api_ship_id.Count(); n++)
            {
                var it = shipList.GetShip(result.api_ship_id[n]);
                res.Member.Add(string.Format("{0}({1}Lv{2})", it.ShipName, it.ShipType, it.Level));
                Debug.WriteLine("Member:" + res.Member.Last());
            }

            Debug.WriteLine("result:" + res.ToString());

            return res;


        }

        /// <summary>
        /// アイテム名を確定させる
        /// </summary>
        /// <param name="id">アイテムID</param>
        /// <param name="name">アイテム名が来た時</param>
        /// <returns></returns>
        string itemName(int id, string name)
        {
            if (name != null && name != "")
                return name;

            switch (id)
            {
                case 1:
                    return "高速修復材";
                case 2:
                    return "高速建造材";
                case 3:
                    return "開発資材";
                case 10:
                    return "家具箱(小)";
                case 11:
                    return "家具箱(中)";
                case 12:
                    return "家具箱(大)";
                default:
                    return string.Format("ID:{0}", id);
            }
        }

    }

    /// <summary>
    /// 艦娘建造
    /// kcsapi/api_req_kousyou/createship
    /// /kcsapi/api_get_member/kdock
    /// </summary>
    public class CreateShip
    {
        /// <summary>
        /// 建造開始API叩かれたドック
        /// </summary>
        bool[] kdock_build = new bool[4] { false,false,false,false};

        /// <summary>
        /// /kcsapi/api_req_kousyou/createship
        /// 建造開始
        /// </summary>
        /// <param name="dock_id_str"> api_kdock_id </param>
        public void Start(string dock_id_str)
        {
            int dock_id = int.Parse(dock_id_str);

            kdock_build[dock_id - 1] = true;
            Debug.WriteLine(string.Format("KDock{0} starting!", dock_id));
        }

        // 

        /// <summary>
        /// 建造記録を生成
        /// /kcsapi/api_get_member/kdock
        /// </summary>
        /// <param name="dockDat"></param>
        /// <param name="basicDat"></param>
        /// <param name="deckDat"></param>
        /// <param name="shipDat"></param>
        /// <returns></returns>
        public IEnumerable<LogData.CreateShipInfo> CreateLogData(MemberData.Dock dockDat,
            MemberData.Basic basicDat,MemberData.Deck deckDat,MemberData.Ship shipDat)
        {
            List<LogData.CreateShipInfo> retList = new List<LogData.CreateShipInfo>();
            for (int n = 0; n < kdock_build.Count(); n++)
            {
                ///既に建造記録を生成しているか、建造していないドックを飛ばす
                if (!kdock_build[n])
                    continue;

                var secretary = shipDat.GetShip(deckDat.Secretary);
                var info = new LogData.CreateShipInfo();
                lock (dockDat.KDockLock)
                {
                    var kdock = dockDat.GetKDock(n + 1);

                    info.Resource = new LogData.Material(kdock);
                    info.ShipType = kdock.Type;
                    info.ShipName = kdock.Name;
                    info.OfficeLv = basicDat.Level;
                    info.SecretaryShip = string.Format("{0}(Lv{1})", secretary.ShipName, secretary.Level);
                    info.Date = DateTime.Now;
                    info.MemberID = basicDat.MemberID;
                }

                retList.Add(info);

                Debug.WriteLine("KDockLog:" + info.ToString());

                kdock_build[n] = false;
            }

            if (retList.Count == 0)
                return null;

            return retList;
        }

    }

    /// <summary>
    /// 装備建造
    /// /kcsapi/api_req_kousyou/createitem
    /// </summary>
    public class CreateItem
    {
        public LogData.CreateItemInfo CreateLogData(string responseJson,IDictionary<string,string> queryParam, MasterData.Item itemData,
            MemberData.Deck deckDat, MemberData.Ship shipDat,MemberData.Basic basicDat)
        {
//            var json = DynamicJson.Parse(oSession.ResponseJSON);
            var json = JsonConvert.DeserializeObject<KCB.api_req_kousyou.CreateItem>(responseJson);
            if ((int)json.api_result != 1)
                return null;

            var info = new LogData.CreateItemInfo(queryParam, json.api_data);

            info.OfficeLv = basicDat.Level;
            var secretary = shipDat.GetShip(deckDat.Secretary);
            info.SecretaryShip = string.Format("{0}(Lv{1})", secretary.ShipName,
                secretary.Level);
            info.MemberID = basicDat.MemberID;

            if (info.Succeess)
            {
                var it = itemData.GetItemParam(info.ItemNameID);
                info.ItemName = it.Name;
                info.ItemType = it.Type;
            }
            return info;
        }

    }

    /// <summary>
    /// 資源
    /// /kcsapi/api_get_member/material
    /// </summary>
    public class MaterialChange
    {
        public LogData.MaterialChangeInfo RecordMaterial(MemberData.Material matDat,MemberData.Basic basicData)
        {
            var info = new LogData.MaterialChangeInfo(matDat, basicData);
            return info;
        }
    }
}
