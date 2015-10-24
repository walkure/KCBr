using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace KCB2.MemberData
{
    /// <summary>
    /// ドック
    /// /kcsapi/api_get_member/ndock 
    /// /kcsapi/api_get_member/kdock 
    /// </summary>
    public class Dock
    {
        /// <summary>
        /// UNIX EPOCH(1970年1月1日午前零時 UTC)
        /// </summary>
        static DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// ドック情報
        /// </summary>
        abstract public class DockInfo
        {
            /// <summary>
            /// 艦船名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 種別
            /// </summary>
            public string Type { get; set; }
            /// <summary>
            /// 艦船ID
            /// </summary>
            public int ShipID { get; set; }
            /// <summary>
            /// 終了時刻
            /// </summary>
            public DateTime Finish { get; set; }
            /// <summary>
            /// ドック番号
            /// </summary>
            public int Order { get; protected set; }
            /// <summary>
            /// 空き
            /// </summary>
            public abstract bool Vacant { get; set; }
        }

        #region 修理ドック

        /// <summary>
        /// 修理ドックの状況
        /// </summary>
        ConcurrentDictionary<int, int> _ndockList = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// /kcsapi/api_get_member/ndock 修理ドック
        /// </summary>
        /// <param name="JSON">JSON</param>
        /// <param name="shipData">艦娘情報</param>
        /// <returns>処理に成功したらtrue</returns>
        /// 
        public bool UpdateRepairNDock(string JSON, Ship shipData)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.NDock>(JSON);
            if ((int)json.api_result != 1)
                return false;

            return UpdateRepairNDock(json.api_data, shipData);
        }


        ///
        public bool UpdateRepairNDock(List<KCB.api_get_member.NDock.ApiData> jsons, Ship shipData)
        {
            _ndockList.Clear();

            foreach (var data in jsons)
            {
                int ship_id = (int)data.api_ship_id;
                int dock_id = (int)data.api_id;

                lock (_ndock)
                {
                    var targetDock = _ndock[dock_id - 1];

                    if (ship_id == 0)
                        targetDock.Vacant = true;
                    else
                    {
                        _ndockList[ship_id] = dock_id;

                        var ship = shipData.GetShip(ship_id);
                        targetDock.Vacant = false;
                        targetDock.ShipID = ship_id;
                        targetDock.Name = ship.ShipName;
                        targetDock.Type = ship.ShipType;
                        targetDock.Finish
                            = _epoch.AddMilliseconds(data.api_complete_time).ToLocalTime();
                    }
                }
            }

#if DEBUG
            for (int i = 0; i < _ndock.Count(); i++)
                Debug.WriteLine(_ndock[i]);
#endif
            return true;
        }

        NDock[] _ndock = new NDock[] { new NDock(1), new NDock(2)
                , new NDock(3), new NDock(4), };

        /// <summary>
        /// 修復ドックの現状
        /// </summary>
        public IEnumerable<NDock> RepairDock { get { return _ndock; } }

        /// <summary>
        /// 修復ドック番号取得
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        public int GetNDockNum(int shipId)
        {
            if (_ndockList.ContainsKey(shipId))
                return _ndockList[shipId];
            return 0;
        }

        /// <summary>
        /// 途中で高速修復を使った
        /// </summary>
        /// <param name="ndock_id_s">使ったドック番号文字</param>
        /// <returns>出渠した艦娘のID</returns>
        public int UseFastRepair(string ndock_id_s)
        {
            int ndock_id = int.Parse(ndock_id_s);
            int ship_id;

            lock (_ndock)
            {
                var ndock = _ndock[ndock_id - 1];
                ndock.Vacant = true;
                ship_id = ndock.ShipID;
            }

            return ship_id;
        }

        /// <summary>
        /// 修理ドック情報
        /// </summary>
        public class NDock : DockInfo
        {
            bool _vacant;
            public override bool Vacant
            {
                get { return _vacant; }
                set
                {
                    _vacant = value;
                    if (_vacant)
                    {
                        Name = "";
                        Type = "";
                    }
                }
            }

            public NDock(int _order)
            {
                Order = _order;
                Name = "";
                Type = "";
                Vacant = true;
            }

            public override string ToString()
            {
                if (Vacant)
                    return string.Format("NDock({0}) vacant", Order);
                else
                    return string.Format("NDock({0}) {1}({2}) {3}", Order, Name, Type, Finish);
            }
        }

        #endregion

        #region 建造ドック
        /// <summary>
        /// /kcsapi/api_get_member/kdock 建造ドック
        /// </summary>
        /// <param name="JSON">JSON</param>
        /// <param name="shipMaster">艦娘マスタ</param>
        /// <returns></returns>
        public bool UpdateBuildKDock(string JSON, MasterData.Ship shipMaster)
        {
            //                var json = DynamicJson.Parse(JSON);
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.KDock>(JSON);
            if ((int)json.api_result != 1)
                return false;

            UpdateBuildKDock(json.api_data, shipMaster);

#if DEBUG
            for (int i = 0; i < _ndock.Count(); i++)
                Debug.WriteLine(_ndock[i]);
#endif
            return true;
        }

        public bool UpdateBuildKDock(List<KCB.api_get_member.KDock.ApiData> json, MasterData.Ship shipMaster)
        {
            foreach (var data in json)
            {
                int ship_id = (int)data.api_created_ship_id;
                int dock_id = (int)data.api_id;

                lock (_kdock)
                {
                    KDock targetDock = _kdock[dock_id - 1];
                    if (ship_id == 0)
                        targetDock.Vacant = true;
                    else
                    {
                        var ship = shipMaster.LookupShipMaster(ship_id);
                        targetDock.Vacant = false;
                        targetDock.ShipID = ship_id;
                        targetDock.Name = ship.Name;
                        targetDock.Type = ship.ShipTypeName;
                        targetDock.Finish
                            = _epoch.AddMilliseconds(data.api_complete_time).ToLocalTime();
                        targetDock.UpdateMaterialParam(data);
                    }
                }
            }
            return true;
        }

        KDock[] _kdock = new KDock[] { new KDock(1), new KDock(2)
                , new KDock(3), new KDock(4) };

        /// <summary>
        /// 建造ドック一覧
        /// </summary>
        public IEnumerable<KDock> BuildDock { get { return _kdock; } }

        /// <summary>
        /// 指定ドックの情報を取り出す
        /// </summary>
        /// <param name="kdock_id">ドックID(1-4)</param>
        /// <returns></returns>
        public KDock GetKDock(int kdock_id)
        {
            return _kdock[kdock_id - 1];
        }

        /// <summary>
        /// 高速建造
        /// </summary>
        /// <param name="kdock_id_s"></param>
        /// <returns></returns>
        public bool UseBurner(string kdock_id_s)
        {
            int kdock_id;
            if (!int.TryParse(kdock_id_s, out kdock_id))
                return false;

            var kdock = GetKDock(kdock_id);

            kdock.Finish = _epoch;
            return true;
        }

        /// <summary>
        /// KDockのロック用
        /// </summary>
        public object KDockLock { get { return _kdock; } }

        /// <summary>
        /// 建造ドック情報
        /// </summary>
        public class KDock : DockInfo
        {
            public int Fuel { get; private set; }
            public int Ammo { get; private set; }
            public int Steel { get; private set; }
            public int Bauxite { get; private set; }
            public int Dev { get; private set; }

            /// <summary>
            /// 建造パラメタをクリア
            /// </summary>
            public void ClearMaterialParam()
            {
                Fuel = 0;
                Ammo = 0;
                Steel = 0;
                Bauxite = 0;
                Dev = 0;
            }

            /// <summary>
            /// 建造パラメタを設定
            /// </summary>
            /// <param name="data">パラメタ</param>
            public void UpdateMaterialParam(KCB.api_get_member.KDock.ApiData data)
            {
                Fuel = data.api_item1;
                Ammo = data.api_item2;
                Steel = data.api_item3;
                Bauxite = data.api_item4;
                Dev = data.api_item5;
            }

            bool _vacant;

            public override bool Vacant
            {
                get { return _vacant; }
                set
                {
                    _vacant = value;
                    if (_vacant)
                    {
                        Name = "";
                        Type = "";
                    }
                }
            }

            public KDock(int _order)
            {
                Order = _order;
                Name = "";
                Type = "";
                Vacant = true;
            }

            public override string ToString()
            {
                if (Vacant)
                    return string.Format("KDock({0}) vacant", Order);
                else
                    return string.Format("KDock({0}) {1}({2}) {3} Fuel:{4} Steel:{5} Bullet:{6} Bauxite:{7} Dev:{8} ",
                        Order, Name, Type, Finish, Fuel, Steel, Ammo, Bauxite, Dev);
            }

        }

        #endregion

    }
}
