using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace KCB2.MemberData
{
    /// <summary>
    /// /kcsapi/api_get_member/questlist
    /// /kcsapi/api_req_quest/clearitemget
    /// 任務
    /// </summary>
    public class Quest
    {
        /// <summary>
        /// 任務一覧
        /// </summary>
        ConcurrentDictionary<int, Info> _quest_list = new ConcurrentDictionary<int, Info>();

        /// <summary>
        /// 任務一覧を最後に取得した日時
        /// </summary>
        DateTime _dtQuestLastUpdated = DateTime.Now;

        /// <summary>
        /// 任務一覧を取ってくる /kcsapi/api_get_member/questlist
        /// </summary>
        /// <param name="JSON">JSON</param>
        /// <returns>処理に成功したらtrue</returns>
        public bool UpdateQuest(string JSON)
        {
            //                var json = DynamicJson.Parse(JSON);
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.QuestList>(JSON);
            if ((int)json.api_result != 1)
                return false;

            //朝五時を越えて最初の更新時に任務一覧をクリアする
            DateTime dtNow = DateTime.Now;
            DateTime dtFiveOClock = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 5, 0, 0);

            if (_dtQuestLastUpdated < dtFiveOClock && dtNow >= dtFiveOClock)
                _quest_list.Clear();
            _dtQuestLastUpdated = dtNow;

            //ページめくる度に任務一覧の部分情報だけ降って来るので、それを集める。
            ConcurrentDictionary<int, Info> new_quest_list = new ConcurrentDictionary<int, Info>();
            foreach (var pair in _quest_list)
            {
                if (pair.Value.State == 1)
                    continue;
                new_quest_list[pair.Key] = pair.Value;
            }
            _quest_list = new_quest_list;

            if (json.api_data == null)
                return false;
            if (json.api_data.api_list == null)
                return false;

            /*
            foreach (dynamic item in (object[])json.api_data.api_list)
            {
                //何故かリストの中に数値「-1」が入っているので、それをはじく
                Type typItem = item.GetType();
                if (typItem != typeof(Codeplex.Data.DynamicJson))
                    continue;

                int id = (int)item.api_no;
                _quest_list[id] = new Info(item);
            }
             */
            foreach (object item in json.api_data.api_list)
            {
                //何故かリストの中に数値「-1」が入っているので、それをはじく
                Newtonsoft.Json.Linq.JObject it = item as Newtonsoft.Json.Linq.JObject;
                if (it == null)
                    continue;

                var obj = it.ToObject<KCB.api_get_member.QuestList.ApiData.ApiList>();
                _quest_list[obj.api_no] = new Info(obj);
            }


            return true;
        }

        /// <summary>
        /// 任務達成 /kcsapi/api_req_quest/clearitemget
        /// </summary>
        /// <param name="quest_id_str">達成任務ID文字列</param>
        public void QuestClear(string quest_id_str)
        {
            int quest_id = int.Parse(quest_id_str);
            Debug.WriteLine(string.Format("Quest ID.{0} Cleared", quest_id));

            Info ret;
            _quest_list.TryRemove(quest_id, out ret);
        }

        /// <summary>
        /// 現在有効な任務一覧をオブジェクトを複製した上で返す
        /// </summary>
        /// <returns>任務一覧</returns>
        public IEnumerable<Info> GetActiveQuests()
        {
            List<Info> retList = new List<Info>();
            foreach (var it in _quest_list)
            {
                ///スレッド間をデータが渡るのでオブジェクトを複製して渡す
                if (it.Value.State == 2 || it.Value.State == 3)
                    retList.Add(new Info(it.Value));
            }
            retList.Sort();
#if DEBUG
            foreach (var it in retList)
                Debug.WriteLine("Quests:" + it.ToString());
#endif

            return retList;
        }

        /// <summary>
        /// クエスト情報のクリア
        /// </summary>
        public void Clear()
        {
            _quest_list.Clear();
        }

        /// <summary>
        /// クエスト内容を保持するクラス
        /// </summary>
        public class Info : IComparable<Info>
        {

            /// <summary>
            /// 任務ID
            /// </summary>
            public int Id { get; private set; }

            /// <summary>
            /// 任務状態。1=未選択 2=選択済み 3=達成
            /// </summary>
            public int State { get; private set; }
            public string StateString
            {
                get
                {
                    switch (State)
                    {
                        case 1:
                            return "□"; // 未選択
                        case 2:
                            return "☑"; // 選択済み
                        case 3:
                            return "■"; // 達成
                        default:
                            throw new ArgumentOutOfRangeException("Unknown State:" + State.ToString());
                    }
                }
            }
            /// <summary>
            /// 任務名
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// 説明
            /// </summary>
            public string Description { get; private set; }

            /// <summary>
            /// 50%以上進捗してると1 80%越えで2 それ以外は0
            /// </summary>
            public int ProgressFlag { get; private set; }

            public string ProgressMsg
            {
                get
                {
                    switch (ProgressFlag)
                    {
                        case 0:
                            return "";
                        case 1:
                            return "≧50%";
                        case 2:
                            return "≧80%";
                        default:
                            throw new ArgumentOutOfRangeException("Unknown Progess Flag:" + ProgressFlag.ToString());
                    }
                }
            }

            /*
             * 10/23メンテまでは生値が出てた
            /// <summary>
            /// 達成状況
            /// </summary>
            public string[] NowCount { get; set; }
            /// <summary>
            /// 目標値
            /// </summary>
            public string[] ClearCount { get; set; }
             */

            /*
            public Info(dynamic json)
            {
                Id = (int)json.api_no;
                Name = json.api_title;
                Description = json.api_detail;
                State = (int)json.api_state;
                ProgressFlag = (int)json.api_progress_flag;
            }*/

            public Info(KCB.api_get_member.QuestList.ApiData.ApiList json)
            {
                Id = (int)json.api_no;
                Name = json.api_title;
                Description = json.api_detail;
                State = (int)json.api_state;
                ProgressFlag = (int)json.api_progress_flag;
            }

            public Info(Info org)
            {
                Id = org.Id;
                Name = org.Name;
                Description = org.Description;
                State = org.State;
                ProgressFlag = org.ProgressFlag;
            }

            public int CompareTo(Info it)
            {
                return Id - it.Id;
            }

            public override string ToString()
            {
                return string.Format("No.{0} Name:{1} State;{2} Prog:{3}",
                    Id, Name, StateString, ProgressMsg);
            }
        }


    }
}
