using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace KCB2
{
    /// <summary>
    /// 戦闘中の艦娘のHPを管理する
    /// </summary>
    public class ShipStatusManager
    {
        /// <summary>
        /// 艦娘情報
        /// </summary>
        public class ShipStatus : IComparable<ShipStatus>
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="info">艦隊情報</param>
            public ShipStatus(KCB.api_get_member.ApiDataShip it,int order,MasterData.Ship masterShip,MemberData.Item memberItem)
            {

                Order = order;
                Name = masterShip.LookupShipMaster(it.api_ship_id).Name;

                Condition = it.api_cond;
                HP = new MemberData.Ship.Info.NowMax(it.api_nowhp,it.api_maxhp);

                DamageControl = false;
                foreach(var item in it.api_slot)
                {
                    if (item < 0)
                        continue;
                    var item_info = memberItem.GetItem(item);
                    if (item_info == null)
                        continue;
                    if (item_info.TypeNum == 14)
                    {
                        DamageControl = true;
                        break;
                    }
                }
            }

            /// <summary>
            /// 艦隊内序列
            /// </summary>
            public int Order { get; private set; }

            /// <summary>
            /// 艦船名
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// コンディション
            /// </summary>
            public int Condition { get; private set; }

            /// <summary>
            /// HP
            /// </summary>
            public MemberData.Ship.Info.NowMax HP { get; private set; }

            /// <summary>
            /// IComparable<ShipStatus>の実装
            /// </summary>
            /// <param name="it"></param>
            /// <returns></returns>
            public int CompareTo(ShipStatus it)
            {
                return Order - it.Order;
            }

            /// <summary>
            /// 独自のToString
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return string.Format("No.{0} Name:{1} Cond.{2} HP:{3} HasDamecon:{4}", Order, Name, Condition, HP,DamageControl);
            }

            /// <summary>
            /// ダメコンの有無
            /// </summary>
            public bool DamageControl { get; private set; }


            /// <summary>
            /// リストビューアイテムを取得
            /// </summary>
            /// <returns></returns>
            public System.Windows.Forms.ListViewItem GetLVItem()
            {
                var it = new System.Windows.Forms.ListViewItem(new string[]
                {
                    string.Format("{0}){1}",Order / 6 + 1,Order % 6 + 1),
                    DamageControl ? "○" : " ",
                    Name,
                    Condition.ToString(),
                    string.Format("{0}({1})",HP.ToString(),HP.Ratio.ToString("0.00%")),
                });

                it.UseItemStyleForSubItems = false;
                it.SubItems[3].BackColor = MemberData.Ship.Info.GetConditionColor(Condition);
                it.SubItems[4].BackColor = HP.BackgroundColor;

                return it;
            }
        }

        /// <summary>
        /// 艦娘一覧 _shipList[0] は使わない(1オリジン)
        /// </summary>
        List<ShipStatus> _shipList = new List<ShipStatus>();

        /// <summary>
        /// 戦闘終了後にupdateされたか
        /// </summary>
        bool bUpdateCompleted = false;

        bool bBattle = false;

        /// <summary>
        /// 戦闘したか
        /// </summary>
        public void StartBattle()
        {
            bBattle = true;
        }

        /// <summary>
        /// 戦闘終了
        /// /kcsapi/api_req_sortie/battleresult
        /// </summary>
        public void FinishBattle()
        {
            bUpdateCompleted = false;
        }

        /// <summary>
        /// 出撃終了
        /// api_port
        /// </summary>
        public void FinishSortie()
        {
            bBattle = false;
        }

        /// <summary>
        /// 艦船の状態を更新する。shipDataは艦隊情報が更新されていると仮定している
        /// ジョブキュースレッドから呼ばれる
        /// /kcsapi/api_get_member/ship_deck
        /// </summary>
        /// <param name="shipData"></param>
        public void UpdateShipStatus(KCB.api_get_member.Ship_Deck shipDeck,MasterData.Ship masterShip,MemberData.Item memberItem)
        {
            Debug.WriteLine("Updating ShipStatus");

            Dictionary<int, int> shipOrder = new Dictionary<int, int>();
            foreach (var it in shipDeck.api_data.api_deck_data)
            {
                int id = it.api_id - 1;
                int order = 0;
                foreach(var it2 in it.api_ship)
                {
                    if (it2 < 0)
                        continue;

                    shipOrder[it2] = order + id * 6;
                    order++;
                }
            }
            
            lock(((IList)_shipList).SyncRoot)
            {
                _shipList.Clear();

                foreach(var it in shipDeck.api_data.api_ship_data)
                        _shipList.Add(new ShipStatus(it,shipOrder[it.api_id],masterShip,memberItem));

                _shipList.Sort();
                 bUpdateCompleted = true;
            }

#if DEBUG
            foreach(var it in _shipList)
            {
                Debug.WriteLine(it);
            }
#endif

        }

        /// <summary>
        /// 艦隊のHPをチェックする。UIスレッドから呼ばれる。
        /// /kcsapi/api_req_map/start /kcsapi/api_req_map/next のBeforeRequestで呼ばれると思ってる
        /// </summary>
        /// <param name="fleetNum">出撃艦隊ID(出撃時のみ) 進撃時は0を入れる</param>
        /// <returns>続行するときはtrue</returns>
        public bool CheckFleetCondition()
        {
            if (Properties.Settings.Default.HPThreshold == 0)
            {
                Debug.WriteLine("HP checking disabled");
                return true;
            }

            int waitCount = 0;
            while(true)
            {
                //一度も戦闘していなければステータスは取得されていないので何もしない
                if (!bBattle)
                {
                    Debug.WriteLine("一度も戦闘していないのでステータスは見ない");
                    return true;
                }
                //出撃先を決めた後 或いは艦船ステータスの更新後ならチェックして返す。
                if (bUpdateCompleted)
                    return _checkFleetCondition();

                Debug.WriteLine("update not completed.yield and sleep 50ms");
                System.Threading.Thread.Yield();
                System.Threading.Thread.Sleep(50);
                waitCount++;

                //4秒以上経っても更新されなかった場合、なにかおかしいとみなす。
                if (50 *waitCount > 4000)
                    break;
            }

            if (System.Windows.Forms.MessageBox.Show("艦船のステータスを取得できません。進撃を続行しますか？",
                "KCBr2", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                return true;

            return false;
        }

        /// <summary>
        /// HPチェックをする実体。UIスレッドから呼ばれる
        /// </summary>
        /// <param name="fleetNum">艦隊番号</param>
        /// <returns></returns>
        bool _checkFleetCondition()
        {
            Debug.WriteLine(string.Format("CheckFleetCondition: threshold:{0}%"
                , Properties.Settings.Default.HPThreshold));

            using (FormBadShipStatus dlg = new FormBadShipStatus())
            {
                //チェック開始
                lock (((IList)_shipList).SyncRoot)
                {
                    //旗艦しかいないのでチェックしない
                    if (_shipList.Count == 1)
                    {
                        Debug.WriteLine("旗艦しかいないのでチェックしない");
                        return true;
                    }

                    bool bFound = false;
                    foreach (var ship in _shipList)
                    {
                        Debug.WriteLine(string.Format("Checking:{0}", ship));
                        if (ship.Order % 6 == 0)
                        {
                            Debug.WriteLine("旗艦はチェックしない");
                            continue;
                        }
                        Debug.WriteLine(string.Format("Check {0} <=> {1}", ship.HP.Percent,
                            Properties.Settings.Default.HPThreshold));
                        if (ship.HP.Percent <= Properties.Settings.Default.HPThreshold)
                        {
                            bFound = true;
                            break;
                        }
                    }

                    //全員チェックを越えた
                    if (bFound == false)
                    {
                        Debug.WriteLine("AllShip Status Green");
                        return true;
                    }

                    //この段階でアイテムが登録される
                    dlg.ShipList = _shipList;
                }

                //続行するならtrue
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    return true;
                }

                //中断させる
                return false;
            }
        }
    }
}
