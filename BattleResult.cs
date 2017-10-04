using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KCB2.BattleResult
{
    
    class Manager
    {

        Result _battleResult = null;
        bool _practice = false;

        /// <summary>
        /// 戦闘に要する概算秒数
        /// </summary>
        const int InitialPhase = 3;
        const int SearchPhase = 7;
        const int SupportTime = 2;
        const int AirBattle = 3;
        const int HouRai = 2;
        const int HouRaiSubmarine = 2;
        const int Torpedo = 3;
        const int CriticalHitTime = 4;

        /// <summary>
        /// /kcsapi/api_req_sortie/battle
        /// 戦闘開始
        /// </summary>
        /// <param name="JSON"></param>
        /// <param name="practice">演習の時true</param>
        /// <param name="_memberShip"></param>
        /// <param name="_memberDeck"></param>
        /// <param name="_masterShip"></param>
        public int ProcessBattle(string JSON,bool practice, MemberData.Ship _memberShip,
            MemberData.Deck _memberDeck, MasterData.Ship _masterShip,MasterData.Item _masterItem)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_sortie.Battle>(JSON);
            if (json == null)
                return 0;
            if (json.api_result != 1)
                return 0;
            _practice = practice;

            var result = json.api_data;
            var deck_info = _memberDeck.GetFleet(result.api_dock_id);

            int estimatedBattleTime = InitialPhase;

            initializeShipData(result, deck_info, _memberShip, _masterShip);

            //索敵フェイズ
            if (_battleResult.Friend.SearchResult != Result.FleetState.SearchResultType.索敵なし)
                estimatedBattleTime += SearchPhase;

            //支援艦隊が来た
            if (result.api_support_flag > 0 && result.api_support_info != null)
            {
                //航空支援(flag=1)
                if (result.api_support_info.api_support_airatack != null)
                {
                    Debug.WriteLine("航空支援(味方から敵へ)");
                    getRaiDamage(result.api_support_info.api_support_airatack.api_stage3.api_edam,
                        _battleResult.Enemy.Ships);
                    estimatedBattleTime += SupportTime;
                }
                //砲雷撃支援(flag=2/雷撃,3/砲撃)
                if (result.api_support_info.api_support_hourai != null)
                {
                    Debug.WriteLine("砲雷支援(味方から敵へ)");
                    getRaiDamage(result.api_support_info.api_support_hourai.api_damage,
                        _battleResult.Enemy.Ships);
                    estimatedBattleTime += SupportTime;
                }
            }

            //開幕対潜
            if (result.api_opening_taisen_flag == 1 && result.api_opening_taisen != null)
            {
                Debug.WriteLine("開幕対潜攻撃");
                estimatedBattleTime += getHougekiDamage(result.api_opening_taisen, _battleResult, _masterItem);
            }

            // 開幕雷撃
            if (result.api_opening_flag == 1 && result.api_opening_atack != null)
            {
                Debug.WriteLine("開幕雷撃(敵から味方へ)");
                estimatedBattleTime += 
                    getRaiDamage(result.api_opening_atack.api_fdam, _battleResult.Friend.Ships);

                Debug.WriteLine("開幕雷撃(味方から敵へ)");
                getRaiDamage(result.api_opening_atack.api_edam, _battleResult.Enemy.Ships);

            }
            estimatedBattleTime += Torpedo;

            // 航空戦
            if (result.api_stage_flag[2] == 1 && result.api_kouku.api_stage3 != null)
            {
                Debug.WriteLine("航空戦(敵から味方へ)");
                estimatedBattleTime +=
                    getRaiDamage(result.api_kouku.api_stage3.api_fdam, _battleResult.Friend.Ships);

                Debug.WriteLine("航空戦(味方から敵へ)");
                getRaiDamage(result.api_kouku.api_stage3.api_edam, _battleResult.Enemy.Ships);

            }
            estimatedBattleTime += AirBattle;

            // 砲撃1順目
            if (result.api_hourai_flag[0] == 1 && result.api_hougeki1 != null)
            {
                Debug.WriteLine("砲撃1順目");
                estimatedBattleTime += getHougekiDamage(result.api_hougeki1, _battleResult, _masterItem);
            }

            // 砲撃2順目
            if (result.api_hourai_flag[1] == 1 && result.api_hougeki2 != null)
            {
                Debug.WriteLine("砲撃2順目");
                estimatedBattleTime += getHougekiDamage(result.api_hougeki2, _battleResult, _masterItem);

            }

            // 砲撃3順目
            if (result.api_hourai_flag[2] == 1 && result.api_hougeki3 != null)
            {
                Debug.WriteLine("砲撃3順目");
                estimatedBattleTime += getHougekiDamage(result.api_hougeki3, _battleResult, _masterItem);
            }

            // 雷撃
            if (result.api_hourai_flag[3] == 1 && result.api_raigeki != null)
            {
                Debug.WriteLine("雷撃(敵から味方へ)");
                estimatedBattleTime += 
                    getRaiDamage(result.api_raigeki.api_fdam, _battleResult.Friend.Ships);

                Debug.WriteLine("雷撃(味方から敵へ)");
                getRaiDamage(result.api_raigeki.api_edam, _battleResult.Enemy.Ships);
            }
            estimatedBattleTime += Torpedo;

            //夜戦の場合は待つ必要がないから0を返す。
            return result.api_midnight_flag != 0 ? estimatedBattleTime : 0;
        }

        /// <summary>
        /// /kcsapi/api_req_battle_midnight/battle
        /// 夜戦開始
        /// </summary>
        /// <param name="JSON"></param>
        public void SwitchNightBattle(string JSON, MasterData.Item _masterItem)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_sortie.Battle>(JSON);
            if (json == null)
                return;
            if (json.api_result != 1)
                return;

            getHougekiDamage(json.api_data.api_hougeki, _battleResult, _masterItem);
        }

        /// <summary>
        /// /kcsapi/api_req_battle_midnight/sp_midnight
        /// 開幕夜戦
        /// </summary>
        /// <param name="JSON"></param>
        /// <param name="_memberShip"></param>
        /// <param name="_memberDeck"></param>
        /// <param name="_masterShip"></param>
        public void ProcessNightBattle(string JSON, MemberData.Ship _memberShip,
            MemberData.Deck _memberDeck, MasterData.Ship _masterShip, MasterData.Item _masterItem)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_sortie.Battle>(JSON);
            if (json == null)
                return;
            if (json.api_result != 1)
                return;
            var deck_info = _memberDeck.GetFleet(json.api_data.api_deck_id);

            initializeShipData(json.api_data, deck_info, _memberShip, _masterShip);
            getHougekiDamage(json.api_data.api_hougeki, _battleResult,_masterItem);

        }

        /// <summary>
        /// /kcsapi/api_req_sortie/battleresult
        /// 戦闘終了(戦闘が解析されていなければnull)
        /// </summary>
        /// <returns></returns>
        public Result GetBattleResult()
        {
            if (_battleResult == null)
                return null;

            return new Result(_battleResult);
        }

        /// <summary>
        /// /kcsapi/api_req_map/start
        /// 戦闘を開始するので過去の情報を削除
        /// </summary>
        public void StartBattle()
        {
            _battleResult = null;
        }

        /// <summary>
        /// 内部状態の初期化
        /// </summary>
        /// <param name="result">戦闘結果</param>
        /// <param name="_memberShip"></param>
        /// <param name="fleetInfo">艦隊情報</param>
        /// <param name="_masterShip"></param>
        private void initializeShipData(KCB.api_req_sortie.Battle.ApiData result,
            MemberData.Deck.Fleet fleetInfo, MemberData.Ship _memberShip, MasterData.Ship _masterShip)
        {
            Debug.WriteLine("戦闘開始/艦隊番号：" + fleetInfo.Num.ToString());
            _battleResult = new Result(result.api_midnight_flag == 1,_practice);

            //戦闘形態を取得する


            _battleResult.Friend.Formation = (Result.FleetState.FormationType)result.api_formation[0];
            _battleResult.Enemy.Formation = (Result.FleetState.FormationType)result.api_formation[1];
            if (result.api_search != null)
            {
                _battleResult.Friend.SearchResult = (Result.FleetState.SearchResultType)result.api_search[0];
                _battleResult.Enemy.SearchResult = (Result.FleetState.SearchResultType)result.api_search[1];
            }
            else
            {
                _battleResult.Friend.SearchResult = Result.FleetState.SearchResultType.索敵なし;
                _battleResult.Enemy.SearchResult = Result.FleetState.SearchResultType.索敵なし;
            }

            _battleResult.MatchState = (Result.MatchType)result.api_formation[2];

            Debug.WriteLine(string.Format("自陣形:{0} {1}\n敵陣形:{2} {3}\n交戦形態:{4}\n夜戦:{5}",
                 _battleResult.Friend.Formation,_battleResult.Friend.SearchResult,
                 _battleResult.Enemy.Formation,_battleResult.Enemy.SearchResult,
                 _battleResult.MatchState, _battleResult.HasNightBattle));


            ///味方艦隊の情報を取得する
            for (int i = 0; i < fleetInfo.Member.Count; i++)
            {
                var ship_id = fleetInfo.Member[i];
                var ship = _memberShip.GetShip(ship_id);
                var info = _battleResult.Friend.Ships[i];

                info.Initialize(ship_id,ship.ShipNameId,ship.ShipName,ship.ShipTypeId,
                    result.api_maxhps[i + 1],result.api_nowhps[i + 1]);

                Debug.WriteLine(string.Format("No.{0} ID:{1} Name:{2} HP:{3}", i + 1,
                    ship_id, ship.ShipName,ship.HP.ToString()));
            }

            ///敵艦隊の情報を取得する
            for (int i = 0; i < 6; i++)
            {
                int ship_id = result.api_ship_ke[i + 1];
                if (ship_id < 0)
                    continue;

                var ship = _masterShip.LookupShipMaster(ship_id);
                var info = _battleResult.Enemy.Ships[i];
                string shipName;
                if(_practice)
                    shipName = ship.Name;
                else
                    shipName = string.Format("{0}{1}", ship.Name, ship.Yomi);


                info.Initialize(-1,ship_id,shipName,ship.ShipTypeId,
                    result.api_maxhps[i + 7],result.api_nowhps[i + 7]);

                Debug.WriteLine(string.Format("No.{0} Name:{1} HP:{2}/{3}",
                    i + 1, shipName, result.api_maxhps[i + 7], result.api_nowhps[i + 7]));
            }

        }

        /// <summary>
        /// 雷撃/航空攻撃の結果処理
        /// </summary>
        /// <param name="damages"></param>
        /// <param name="param"></param>
        private int getRaiDamage(List<double> damages, Result.FleetState.ShipState[] param)
        {
            if (damages == null)
                return 0;

            int bCritical = 0;

            for (int i = 0; i < 6; i++)
            {
                double damage = damages[i + 1];

                Debug.WriteLine(String.Format("[{0}] HP:{1} -> {2} ({3})",
                    i + 1, param[i].CurrentHP, param[i].CurrentHP - Math.Floor(damage), damage));

                bCritical += param[i].AddDamage(damage) ? 1: 0;
            }

            return bCritical > 0 ? CriticalHitTime : 0;
        }

        /// <summary>
        /// 砲雷撃戦の結果処理
        /// </summary>
        /// <param name="hougeki"></param>
        /// <param name="result"></param>
        private int getHougekiDamage(KCB.api_req_sortie.Battle.ApiData.ApiHougeki hougeki,
            Result result, MasterData.Item _masterItem)
        {
            if (hougeki == null)
                return 0;

            //長さが一致しない
            if (hougeki.api_damage.Count != hougeki.api_df_list.Count)
                return 0;

            int battleTime = 0;

            //攻撃先のHPを減算
            for (int i = 1; i < hougeki.api_damage.Count; i++)
            {
                var targets = (Newtonsoft.Json.Linq.JArray)hougeki.api_df_list[i];
                var damages = (Newtonsoft.Json.Linq.JArray)hougeki.api_damage[i];
                var weapons = (Newtonsoft.Json.Linq.JArray)hougeki.api_si_list[i];
                int from = hougeki.api_at_list[i];

                for (int j = 0; j < targets.Count; j++)
                {
                    int target = (int)targets[j];
                    double damage = (double)damages[j];
                    int weapon_id = (int)weapons[j];

                    var weapon = _masterItem.GetItemParam(weapon_id);

                    if(weapon == null)
                        Debug.WriteLine(string.Format("砲撃:{0}->{1} ダメージ:{2} 装備:なし", from, target, damage));
                    else
                        Debug.WriteLine(string.Format("砲撃:{0}->{1} ダメージ:{2} 装備:{3}", from, target, damage
                            , weapon.Name));

                    if (target > 0 && target <= 6)
                    {
                        var it = result.Friend.Ships[target - 1];

                        //敵→味方
                        var critical = it.AddDamage(damage);
                        if (critical)
                            battleTime += CriticalHitTime;

                        if(it.Submarine)
                            battleTime += HouRaiSubmarine;
                    }
                    else if (target > 0 && target <= 12)
                    {
                        var it = result.Enemy.Ships[target - 7];
                        //味方→敵
                        it.AddDamage(damage);

                        if (it.Submarine)
                            battleTime += HouRaiSubmarine;
                    }

                    battleTime += HouRai;
                }

            }
            return battleTime;
        }
    }

    /// <summary>
    /// 戦闘結果情報
    /// </summary>
    public class Result
    {
        public enum MatchType
        {
            同航戦 = 1,
            反航戦 = 2,
            Ｔ字有利 = 3,
            Ｔ字不利 = 4,
        }

        public bool HasNightBattle { get; private set; }

        /// <summary>
        /// 戦闘形態
        /// </summary>
        public MatchType MatchState;

        /// <summary>
        /// 制空権(api_disp_seiku)
        /// </summary>
        public enum AirSuperityType
        {
            航空互角 = 0,
            制空権確保 = 1,
            航空優勢 = 2,
            航空劣勢 = 3,
            制空権喪失 = 4,
        }

        /// <summary>
        /// 艦隊情報
        /// </summary>
        public class FleetState
        {
            /// <summary>
            /// 艦娘情報
            /// </summary>
            public class ShipState
            {
                public ShipState()
                {
                    ShipID = -1;
                    ShipNameID = -1;
                    ShipTypeID = -1;
                    InitialHP = 0;
                    DamageTotal = 0;
                    ShipName = "";
                }

                /// <summary>
                /// 初期化
                /// </summary>
                /// <param name="ship_id">艦娘UID(敵の場合は-1)</param>
                /// <param name="ship_name_id">艦船ID(存在しない場合は-1)</param>
                /// <param name="max_hp">艦娘の最大HP</param>
                /// <param name="now_hp">戦闘開始時HP</param>
                public void Initialize(int ship_id, int ship_name_id,string ship_name
                    ,int ship_type,int max_hp, int now_hp)
                {
                    ShipID = ship_id;
                    ShipNameID = ship_name_id;
                    ShipName = ship_name;
                    ShipTypeID = ship_type;
                    MaxHP = max_hp;
                    InitialHP = now_hp;
                }

                public ShipState(ShipState orig)
                {
                    this.ShipID = orig.ShipID;
                    this.ShipNameID = orig.ShipNameID;
                    this.ShipTypeID = orig.ShipTypeID;
                    this.InitialHP = orig.InitialHP;
                    this.MaxHP = orig.MaxHP;
                    this.DamageTotal = orig.DamageTotal;
                    this.ShipName = orig.ShipName;
                }

                /// <summary>
                /// 艦娘UID(敵の場合は-1)
                /// </summary>
                public int ShipID { get; private set; }
                /// <summary>
                /// 艦船ID(存在しない場合は-1)
                /// </summary>
                public int ShipNameID { get; private set; }
                /// <summary>
                /// 艦船名
                /// </summary>
                public string ShipName { get; private set; }
                /// <summary>
                /// 艦船種別ID
                /// </summary>
                public int ShipTypeID { get; private set; }
                /// <summary>
                /// 艦娘の最大HP
                /// </summary>
                public int MaxHP { get; private set; }
                /// <summary>
                /// 戦闘開始時HP
                /// </summary>
                public int InitialHP { get; private set; }
                /// <summary>
                /// 戦闘終了時HP
                /// </summary>
                public int CurrentHP
                {
                    get
                    {
                        int HP = InitialHP - DamageTotal;
                        if (HP < 0)
                            HP = 0;
                        return HP;
                    }
                }
                /// <summary>
                /// 与えられたダメージ合計(単純加算なので艦HPを越えうる)
                /// </summary>
                public int DamageTotal { get; private set; }

                /// <summary>
                /// 削られたHP
                /// </summary>
                public int TrimmedHP
                {
                    get
                    {
                        if (DamageTotal > InitialHP)
                            return InitialHP;

                        return DamageTotal;
                    }
                }

                /// <summary>
                /// 潜水艦ならtrue
                /// </summary>
                public bool Submarine
                {
                    get
                    {
                        if (ShipTypeID == 13 || ShipTypeID == 14)
                            return true;
                        else
                            return false;
                    }
                }

                /// <summary>
                /// ダメージ加算
                /// </summary>
                /// <param name="damage">与ダメージ</param>
                /// <returns>クリティカルヒット食らったエフェクトが出そうなときtrue</returns>
                public bool AddDamage(double ddamage)
                {
                    //ダメージの小数点以下は旗艦をかばうフラグの様子
                    int damage = (int)Math.Floor(ddamage);

                    DamageTotal += damage;

                    ///無傷→中破/大破のときだけtrueを返したい

                    if ((double)InitialHP / MaxHP > 0.5
                        && (double)CurrentHP / MaxHP <= 0.5)
                        return true;
                    else
                        return false;
                }

                /// <summary>
                /// 有効ならばtrue
                /// </summary>
                public bool Valid { get { return ShipNameID != -1; } }

                public override string ToString()
                {
                    if (!Valid)
                        return "(無効)";

                    return string.Format("[UID:{0},NID:{1}({2})] {3}/{4}(Before:{5},Damage:{6})",
                        ShipID, ShipNameID,ShipName, CurrentHP, MaxHP, InitialHP, DamageTotal);
                }
            }

            /// <summary>
            /// 索敵結果(api_search[0] api_search[1]は敵側？)
            /// </summary>
            public enum SearchResultType
            {
                /// <summary>
                /// 索敵機による索敵が成功した
                /// </summary>
                索敵機成功 = 1,
                /// <summary>
                /// 索敵機による索敵は成功したが、未帰還機あり
                /// </summary>
                索敵機成功未帰還 = 2,
                /// <summary>
                /// 索敵機による索敵に失敗し、未帰還機あり
                /// </summary>
                索敵機失敗未帰還 = 3,
                /// <summary>
                /// 自力での索敵に失敗
                /// </summary>
                索敵失敗 = 4,
                /// <summary>
                /// 自力での索敵に成功
                /// </summary>
                索敵成功 = 5,
                /// <summary>
                /// 索敵フェイズなし
                /// </summary>
                索敵なし = 6,
            }

            /// <summary>
            /// 策敵結果
            /// </summary>
            public SearchResultType SearchResult;

            /// <summary>
            /// 艦娘の状態
            /// </summary>
            public ShipState[] Ships;

            /// <summary>
            /// 艦隊隊形
            /// </summary>
            public FormationType Formation;

            public enum FormationType
            {
                単縦陣 = 1,
                複縦陣 = 2,
                輪形陣 = 3,
                梯形陣 = 4,
                単横陣 = 5,

                //何故か文字で来る…。
                連合対潜警戒 = 11,
                連合前方警戒 = 12,
                連合輪形陣 = 13,
                連合戦闘隊形 = 14,
            };

            public FleetState()
            {
                Ships = new ShipState[] {
                    new ShipState(), new ShipState(),
                    new ShipState(), new ShipState(),
                    new ShipState(), new ShipState()
                };
            }

            public FleetState(FleetState orig)
            {
                Formation = orig.Formation;
                SearchResult = orig.SearchResult;
                Ships = new ShipState[] {
                    new ShipState(orig.Ships[0]),new ShipState(orig.Ships[1]),
                    new ShipState(orig.Ships[2]),new ShipState(orig.Ships[3]),
                    new ShipState(orig.Ships[4]),new ShipState(orig.Ships[5])
                };
            }

            
            /// <summary>
            /// 勝敗判定用ダメージ率
            /// </summary>
            public double DamageRate
            {
                get
                {
                    int totalInitialHP = 0, totalTrimmedHP = 0;
                    foreach (var it in Ships)
                    {
                        totalInitialHP += it.InitialHP;
                        totalTrimmedHP += it.TrimmedHP;
                    }

                    double rate = (double)totalTrimmedHP / totalInitialHP * 100;
                    rate = Math.Round(rate, 2, MidpointRounding.AwayFromZero);

                    Debug.WriteLine(string.Format("戦果ゲージ: {0} , 削られたHP:{1} 開始時HP:{2}",
                        rate, totalTrimmedHP, totalInitialHP));

                    return rate;
                }
            }

            /// <summary>
            /// 艦隊にいる艦船数
            /// </summary>
            public int TotalShipCount
            {
                get
                {
                    int count = 0;
                    foreach (var it in Ships)
                    {
                        if (it.Valid)
                            count++;
                    }
                    return count;
                }
            }

            /// <summary>
            /// 沈んでいない艦船数
            /// </summary>
            public int TotalShipsAlive
            {
                get
                {
                    int count = 0;
                    foreach (var it in Ships)
                    {
                        if (it.Valid && it.CurrentHP > 0)
                            count++;
                    }
                    return count;
                }
            }

            /// <summary>
            /// 撃沈された艦が存在するか
            /// </summary>
            public bool HasSinkedShip
            {
                get
                {
                    return (SinkedShip > 0);
                }
            }

            /// <summary>
            /// 沈んだ隻数
            /// </summary>
            public int SinkedShip
            {
                get
                {
                    return TotalShipCount - TotalShipsAlive;
                }
            }

            /// <summary>
            /// 半分以上の艦が沈んだか
            /// </summary>
            public bool IsMajoritySinked
            {
                get
                {
                    int alive = TotalShipsAlive;
                    int count = TotalShipCount;
                    Debug.WriteLine(string.Format("IsMajoritySinked -> {0}/{1}", alive, count));
                    switch (count)
                    {
                        case 6: // 4隻以上撃沈(2隻以下生存)
                        case 5: // 3隻以上撃沈(2隻以下生存)
                        case 4: // 2隻以上撃沈(2隻以下生存)
                            return alive <= 2;
                        case 3: // 2隻以上轟沈(1隻以下生存)
                        case 2: // 1隻以上轟沈(1隻以下生存)
                            return alive <= 1;
                        case 1: // 1隻轟沈(生存艦なし)
                            return alive <= 0;
                        default:
                            throw new IndexOutOfRangeException("Ships count missing");
                    }
                }
            }

            
        }

        /// <summary>
        /// 敵艦隊の戦闘情報
        /// </summary>
        public FleetState Enemy;

        /// <summary>
        /// 自艦隊の戦闘情報
        /// </summary>
        public FleetState Friend;

            /* http://wikiwiki.jp/kancolle/?%BE%A1%CD%F8%BE%F2%B7%EF
             * http://sorohai.blog49.fc2.com/blog-entry-602.html
             * http://wakaranpos.blog33.fc2.com/blog-entry-487.html
             * http://kannkore.com/system/victory-condition/
             * http://kancolle.doorblog.jp/archives/33338525.html
             */

        

        /// <summary>
        /// 戦闘結果ステート
        /// </summary>
        public enum BattleResultState
        {
            /// <summary>
            /// 完全勝利S
            /// </summary>
            SS = 0,
            /// <summary>
            /// S勝利
            /// </summary>
            S = 1,
            /// <summary>
            /// A勝利
            /// </summary>
            A = 2,
            /// <summary>
            /// 戦術的勝利B
            /// </summary>
            B = 3,
            /// <summary>
            /// 戦術的敗北C
            /// </summary>
            C = 4,
            /// <summary>
            /// 敗北D
            /// </summary>
            D = 5,
            /// <summary>
            /// 敗北E
            /// </summary>
            E = 6,
            /// <summary>
            /// 未確定(情報がないとき)
            /// </summary>
            Undefined = 99,
        }

        public static string BattleResultStateShortString(BattleResultState state)
        {
            switch (state)
            {
                case BattleResultState.SS: return "S";
                case BattleResultState.S: return "S";
                case BattleResultState.A: return "A";
                case BattleResultState.B: return "B";
                case BattleResultState.C: return "C";
                case BattleResultState.D: return "D";
                case BattleResultState.E: return "E";
                default: throw new IndexOutOfRangeException("Unknown state:" + state.ToString());
            }
        }

        public static string BattleResultStateString(BattleResultState state)
        {
            switch (state)
            {
                case BattleResultState.SS: return "完全勝利S";
                case BattleResultState.S: return "勝利S";
                case BattleResultState.A: return "勝利A";
                case BattleResultState.B: return "戦術的勝利B";
                case BattleResultState.C: return "戦術的敗北C";
                case BattleResultState.D: return "敗北D";
                case BattleResultState.E: return "敗北E";
                default: throw new IndexOutOfRangeException("Unknown state:" + state.ToString());
            }
        }

        /// <summary>
        /// 現状の戦闘結果
        /// </summary>
        public BattleResultState BattleState
        {
            get
            {
                //敵にダメージを与えていなければD,E敗北
                if (Enemy.DamageRate == 0)
                {
                    //味方が沈められていればE？
                    if (Friend.HasSinkedShip)
                        return BattleResultState.E;

                    return BattleResultState.D;
                }

                int state = (int)BattleResultState.SS;

                if (Enemy.TotalShipsAlive == 0)
                {
                    //敵を壊滅させていればS/SS
                    if (Friend.DamageRate > 0)
                        state = (int)BattleResultState.S;
                }
                else
                {
                    if (Enemy.IsMajoritySinked)
                    {
                        //過半数の敵を沈めた
                        state = (int)BattleResultState.A;
                    }
                    else
                    {
                        if (Enemy.Ships[0].CurrentHP == 0)
                        {
                            //敵旗艦を沈めた
                            state = (int)BattleResultState.B;
                        }
                        else
                        {
                            if (Enemy.DamageRate > Friend.DamageRate * 2.5)
                            {
                                //戦果ゲージが2.5倍超え
                                state = (int)BattleResultState.B;
                            }
                            else if (Enemy.DamageRate >= Friend.DamageRate)
                            {
                                //戦果ゲージが1倍超え
                                state = (int)BattleResultState.C;
                            }
                            else
                            {
                                state = (int)BattleResultState.D;
                            }
                        }
                    }
                }
                //轟沈艦があればB勝利以下にする
                if (Friend.HasSinkedShip)
                {
                    state++;
                    if (state > (int)BattleResultState.B)
                        state = (int)BattleResultState.B;
                }

                return (BattleResultState)state;
            }
        }


        /// <summary>
        /// 演習の時true
        /// </summary>
        public bool Practice { get; private set; }

        public Result(bool bHasNightBattle,bool practice)
        {
            Friend = new FleetState();
            Enemy = new FleetState();
            HasNightBattle = bHasNightBattle;
            Practice = practice;
        }

        public Result(Result orig)
        {
            MatchState = orig.MatchState;
            HasNightBattle = orig.HasNightBattle;
            Friend = new FleetState(orig.Friend);
            Enemy = new FleetState(orig.Enemy);
            Practice = orig.Practice;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("自陣形:{0} {1}\r\n敵陣形:{2} {3}\r\n交戦形態:{4}\r\n夜戦:{5}\r\n",
                 Friend.Formation, Friend.SearchResult,
                 Enemy.Formation, Enemy.SearchResult,
                 MatchState, HasNightBattle);

            sb.AppendFormat("*自軍状態:{0} {1} ゲージ{2}% \r\n",Friend.Formation.ToString(),Friend.SearchResult.ToString(),Enemy.DamageRate);
            for (int i = 0; i < 6; i++)
            {
                if (!Friend.Ships[i].Valid)
                    continue;
                sb.AppendFormat("{0}:{1}\r\n", i + 1, Friend.Ships[i].ToString());
            }

            sb.AppendFormat("*敵軍状態:{0} {1} ゲージ{2}%\r\n",Enemy.Formation.ToString(),Enemy.SearchResult.ToString(),Friend.DamageRate);
            for (int i = 0; i < 6; i++)
            {
                if (!Enemy.Ships[i].Valid)
                    continue;
                sb.AppendFormat("{0}:{1}\r\n", i + 1, Enemy.Ships[i].ToString());
            }

            return sb.ToString();
        }
    }
}
