using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;

namespace KCB2.MasterData
{



    /// <summary>
    /// 遠征情報
    /// </summary>
    public class Mission
    {
        ConcurrentDictionary<int, Item> _missionList = new ConcurrentDictionary<int, Item>();
        public bool UpdateMission(List<KCB.api_start2.ApiData.ApiMstMission> missions)
        {
            foreach (var data in missions)
            {
                _missionList[(int)data.api_id] = new Item(data);
            }

            return true;
        }

        /// <summary>
        /// 遠征情報取得
        /// </summary>
        /// <param name="mission_id"></param>
        /// <returns>id=0でnull</returns>
        public Item GetMissionInfo(int mission_id)
        {
            if (_missionList.ContainsKey(mission_id))
                return _missionList[mission_id];

            if (mission_id != 0)
                return new Item(mission_id);
            else
                return null;
        }

        public class Item
        {
            public int MissionID { get; private set; }
            public int MapArea { get; private set; }
            public string Name { get; private set; }
            public string Detail { get; private set; }
            public int MinutesRequired { get; private set; }
            public int Diffculty { get; private set; }
            public double FuelUse { get; private set; }
            public double BulletUse { get; private set; }

            public Item(KCB.api_start2.ApiData.ApiMstMission json)
            {
                MissionID = (int)json.api_id;
                MapArea = (int)json.api_maparea_id;
                Name = json.api_name;
                Detail = json.api_details;
                MinutesRequired = (int)json.api_time;
                Diffculty = (int)json.api_difficulty;
                FuelUse = (double)json.api_use_fuel;
                BulletUse = (double)json.api_use_bull;
            }

            public Item(int nID)
            {
                MissionID = nID;
                Name = "";
                Detail = "";
            }
        }
    }
}
