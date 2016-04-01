using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KCB2.MemberData
{
    /// <summary>
    /// アカウント情報
    /// /kcsapi/api_get_member/basic
    /// /kcsapi/api_port/port
    /// </summary>
    public class Basic
    {
        #region パラメタ定義
        /// <summary>
        /// メンバID
        /// </summary>
        public String MemberID { get; private set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Nick { get; private set; }
        /// <summary>
        /// 司令部レベル
        /// </summary>
        public int Level { get; private set; }
        /// <summary>
        /// 司令部ランク
        /// </summary>
        public int Rank { get; private set; }
        /// <summary>
        /// 提督経験値
        /// </summary>
        public int Experience { get; private set; }
        /// <summary>
        /// 艦娘最大数
        /// </summary>
        public int MaxShip { get; private set; }
        /// <summary>
        /// アイテム最大数
        /// </summary>
        public int MaxItem { get; private set; }
        /// <summary>
        /// 艦隊数
        /// </summary>
        public int Deck { get; private set; }
        /// <summary>
        /// 建造ドック数
        /// </summary>
        public int KDock { get; private set; }
        /// <summary>
        /// 修復ドック数
        /// </summary>
        public int NDock { get; private set; }
        /// <summary>
        /// 家具コイン
        /// </summary>
        public int FurnitureCoin { get; private set; }

        #endregion

        bool _initialiized = false;

        static string[] _rankText = { "", "元帥", "大将", "中将", "少将", "大佐", "中佐"
                                            , "新米中佐", "少佐", "中堅少佐", "新米少佐" };

        /// <summary>
        /// ウィンドウタイトルに設定する文字列を取得
        /// </summary>
        public string WindowTitle
        {
            get
            {
                if (!_initialiized)
                    return "KCBr2";

                return string.Format("KCBr2 >{0}< 階級:{1} 司令部レベル:{2} 提督経験値:{3}",
                    Nick, _rankText[Rank], Level, Experience);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="JSON">読み込むJSON</param>
        public bool Update(String JSON)
        {
            //                var json = DynamicJson.Parse(JSON);
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.Basic>(JSON);

            if ((int)json.api_result != 1)
                return false;

            var data = json.api_data;

            _initialiized = true;
            MemberID = data.api_member_id;
            Nick = data.api_nickname;
            Level = (int)data.api_level;
            Rank = (int)data.api_rank;
            Experience = (int)data.api_experience;
            MaxShip = (int)data.api_max_chara;
            MaxItem = (int)data.api_max_slotitem;
            Deck = (int)data.api_count_deck;
            KDock = (int)data.api_count_kdock;
            NDock = (int)data.api_count_ndock;
            FurnitureCoin = (int)data.api_fcoin;

            Debug.WriteLine(string.Format(
                "BASIC MemberID:{0} Nick:{1} Lv:{2} Rank:{3} Exp;{4} ShipMax:{5} ItemMax:{6} Deck:{7} KDock:{8} NDock:{9} FCoin:{10}",
                MemberID, Nick, Level, Rank, Experience, MaxShip, MaxItem, Deck, KDock, NDock, FurnitureCoin));

            return true;
        }

        public bool UpdatePort(KCB.api_port.Port.ApiData.ApiBasic data)
        {
            _initialiized = true;

            MemberID = data.api_member_id;
            Nick = data.api_nickname;
            Level = data.api_level;
            Rank = data.api_rank;
            Experience = data.api_experience;
            MaxShip = data.api_max_chara;
            MaxItem = data.api_max_slotitem;
            Deck = data.api_count_deck;
            KDock = data.api_count_kdock;
            NDock = data.api_count_ndock;
            FurnitureCoin = data.api_fcoin;

            Debug.WriteLine(string.Format(
                "BASIC(2) MemberID:{0} Nick:{1} Lv:{2} Rank:{3} Exp;{4} ShipMax:{5} ItemMax:{6} Deck:{7} KDock:{8} NDock:{9} FCoin:{10}",
                MemberID, Nick, Level, Rank, Experience, MaxShip, MaxItem, Deck, KDock, NDock, FurnitureCoin));

            return true;
        }

        public void UpdateRequireInfo(KCB.api_get_member.RequireInfo.ApiData.ApiBasic data)
        {
            MemberID = data.api_member_id.ToString();

            Debug.WriteLine("BASIC(3) MemberID:" + MemberID);
        }

    }

}
