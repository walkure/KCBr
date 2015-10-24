using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

//using Codeplex.Data;

namespace KCB2
{
    public partial class  SessionProcessor : KCB.JobQueue<SessionData>
    {
        FormMain _parent;
        ShipStatusManager _statusManager;
        BattleResult.Manager _battleResultManager;
        public SessionProcessor(FormMain parent,ShipStatusManager statusManager)
            : base()
        {
            _parent = parent;
            _statusManager = statusManager;
            _battleResultManager = new BattleResult.Manager();
            DuringCombat = false;

        }

        /// <summary>
        /// 戦闘中ならtrue
        /// </summary>
        public bool DuringCombat { get; private set; }

        #region パラメータハンドル定義
        MasterData.Ship _masterShip = new MasterData.Ship();
        MasterData.Item _masterItem = new MasterData.Item();
        MasterData.Mission _masterMission = new MasterData.Mission();

        MemberData.Basic _memberBasic = new MemberData.Basic();
        MemberData.Material _memberMaterial = new MemberData.Material();
        public MemberData.Deck _memberDeck = new MemberData.Deck();
        public MemberData.Ship _memberShip = new MemberData.Ship();
        MemberData.Item _memberItem = new MemberData.Item();
        MemberData.Dock _memberDock = new MemberData.Dock();
        MemberData.Quest _memberQuest = new MemberData.Quest();

        LogGenerator.BattleResult _logBattle = new LogGenerator.BattleResult();
        LogGenerator.MissionResult _logMission = new LogGenerator.MissionResult();
        LogGenerator.CreateShip _logCreateShip = new LogGenerator.CreateShip();
        LogGenerator.CreateItem _logCreateItem = new LogGenerator.CreateItem();
        LogGenerator.MaterialChange _logMaterial = new LogGenerator.MaterialChange();

        #endregion

        /// <summary>
        /// ウィンドウタイトルを更新する
        /// </summary>
        void UpdateWindowTitle()
        {
            StringBuilder title = new StringBuilder(_memberBasic.WindowTitle);
            int ships = _memberShip.Count;
            int items = _memberItem.Count;
            if (ships == 0)
                title.AppendFormat(" 最大艦娘数:{0}", _memberBasic.MaxShip);
            else
                title.AppendFormat(" 艦娘:{0}/{1}", ships, _memberBasic.MaxShip);

            if (items == 0)
                title.AppendFormat(" 最大装備数:{0}", _memberBasic.MaxItem);
            else
                title.AppendFormat(" 装備:{0}/{1}", items, _memberBasic.MaxItem);

            _parent.UpdateWindowTitle(title.ToString());
        }

        /// <summary>
        /// 詳細なログをステータスに吐く
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void UpdateDetailStatus(string format, params object[] args)
        {
            if(Properties.Settings.Default.VerboseStatus)
                _parent.UpdateStatus(format, args);
        }


        /// <summary>
        /// ジョブ処理ハンドラ
        /// </summary>
        /// <param name="oSession"></param>
        override protected void processJob(SessionData oSession)
        {
#if !DEBUG
            try
#endif
            {
                _parent.AddJSONLog(oSession);
                Dispatch(oSession);

            }
#if !DEBUG
            catch (Exception ex)
            {
                string fn = string.Format("./{0}.log", DateTime.Now.Ticks.ToString());
                Debug.WriteLine("SaveSession:" + fn + ".session");
                oSession.SaveSession(fn + ".session");

                using (var sw = new System.IO.StreamWriter(fn, true, Encoding.UTF8))
                {
                    sw.WriteLine("Exception:{0}\r\nJSON:{1}\r\n",
                        ex.ToString(), SessionData.DecodeJSON(oSession.ResponseJSON));
                }

                string msg = string.Format("JSON Parse failed.\n\nException:{0}\r\nSession saved:{1}",
                    ex.ToString(), fn);
                Debug.WriteLine(msg);
                System.Windows.Forms.MessageBox.Show(msg);
            }
#endif

        }
        
        /// <summary>
        /// API URLに応じた処理先へディスパッチ
        /// </summary>
        /// <param name="oSession"></param>
        void Dispatch(SessionData oSession)
        {
            Debug.WriteLine(string.Format("Process:{0}\nJSON:{1}\n",
                oSession.PathQuery, oSession.ResponseJSON));

            string responseJson = oSession.ResponseJSON;
            IDictionary<string, string> queryParam = oSession.QueryParam;

            #region ディスパッチ
            switch (oSession.PathQuery)
            {
                case "/kcsapi/api_start2":
                    Start2(responseJson);
                    break;

                case "/kcsapi/api_req_member/get_incentive":
                    _parent.UpdateStatus("ゲームを開始しました");
                    break;

                case "/kcsapi/api_get_member/basic":
                    Basic(responseJson);
                    break;

                case "/kcsapi/api_get_member/slot_item":
                    Slot_Item(responseJson);
                    break;

                case "/kcsapi/api_get_member/kdock":
                    KDock(responseJson);
                    break;

                case "/kcsapi/api_port/port":
                    DuringCombat = false;
                    Port(responseJson);
                    break;

                case "/kcsapi/api_get_member/ship2":
                    Ship2(responseJson);
                    break;

                case "/kcsapi/api_get_member/ship3":
                    Ship3(responseJson,queryParam);
                    break;

                case "/kcsapi/api_get_member/ship_deck":
                    ShipDeck(responseJson, queryParam);
                    break;

                case "/kcsapi/api_req_hokyu/charge":
                    Charge(responseJson);
                    break;

                case "/kcsapi/api_req_hensei/change":
                    DeckMemberChange(queryParam);
                    break;

                case "/kcsapi/api_req_hensei/lock":
                    LockShip(responseJson, queryParam["api_ship_id"]);
                    break;

                case "/kcsapi/api_get_member/questlist":
                    UpdateQuestList(responseJson);
                    break;

                case "/kcsapi/api_req_quest/stop":
                    UpdateDetailStatus("任務を解除しました");
                    break;

                case "/kcsapi/api_req_quest/clearitemget":
                    _memberQuest.QuestClear(queryParam["api_quest_id"]);
                    UpdateDetailStatus("任務を達成しました");
                    break;

                case "/kcsapi/api_req_mission/start":

                    _memberDeck.StartMission(queryParam, _masterMission);
                    _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);

                    UpdateDetailStatus("遠征を開始しました");
                    break;

                case "/kcsapi/api_req_mission/result":
                    MissionResult(responseJson);
                    break;

                case "/kcsapi/api_req_kaisou/slotset":
                    UpdateDetailStatus("装備を変更しました");
                    break;

                case "/kcsapi/api_req_sortie/battleresult":
                    //2014夏イベントでの連合艦隊戦闘結果
                case "/kcsapi/api_req_combined_battle/battleresult":
                    BattleResult(responseJson,_battleResultManager.GetBattleResult());
                    break;

                case "/kcsapi/api_req_map/start":
                    _logBattle.Start(oSession.QueryParam);
                    _battleResultManager.StartBattle();
                    UpdateDetailStatus("出撃しました");
                    break;

                case "/kcsapi/api_req_sortie/battle":
                    DuringCombat = true;
                    int battleWait = _battleResultManager.ProcessBattle(oSession.ResponseJSON,
                        _memberShip,_memberDeck,_masterShip,_masterItem);
                    _statusManager.StartBattle();

                    UpdateDetailStatus("戦闘を開始しました",battleWait);
                    _parent.BeginWaitForNightBattle(battleWait,_battleResultManager.GetBattleResult());
                    break;

                case "/kcsapi/api_req_battle_midnight/battle":
                    MidnightBattle(oSession.ResponseJSON);
                    break;

                case "/kcsapi/api_req_battle_midnight/sp_midnight":
                    _battleResultManager.ProcessNightBattle(oSession.ResponseJSON, _memberShip,
                        _memberDeck, _masterShip, _masterItem);
                    UpdateDetailStatus("夜戦を開始しました");
                    break;

//                case "/kcsapi/api_req_sortie/night_to_day":
//                    UpdateDetailStatus("昼戦へ移行しました");
//                    break;

                case "/kcsapi/api_req_map/next":
                    UpdateDetailStatus("進撃しました");
                    break;

                case "/kcsapi/api_get_member/practice":
                    UpdateDetailStatus("演習相手候補を取得しました");
                    break;

                case "/kcsapi/api_req_member/get_practice_enemyinfo":
                    UpdateDetailStatus("演習相手の艦隊編成を取得しました");
                    break;

                case "/kcsapi/api_req_practice/battle":
                    DuringCombat = true;
                    UpdateDetailStatus("演習を開始しました");
                    break;

                case "/kcsapi/api_req_practice/midnight_battle":
                    UpdateDetailStatus("夜戦演習へ移行しました");
                    break;

                case "/kcsapi/api_req_practice/battle_result":
                    PracticeResult(responseJson);
                    break;

                case "/kcsapi/api_req_kaisou/powerup":
                    PowerUp(responseJson,queryParam);
                    break;

                case "/kcsapi/api_req_kaisou/remodeling":
                    UpdateDetailStatus("改造しました");
                    break;

                case "/kcsapi/api_req_kaisou/lock":
                    LockSlotItem(responseJson, queryParam["api_slotitem_id"]);
                    break;

                case "/kcsapi/api_req_member/updatedeckname":
                    UpdateDeckName(queryParam);
                    break;

                case "/kcsapi/api_get_member/ndock":
                    UpdateRepairNDock(responseJson);
                    break;

                case "/kcsapi/api_req_kousyou/createitem":
                    CreateItem(responseJson,queryParam);
                    break;

                case "/kcsapi/api_get_member/material":
                    UpdateMaterial(responseJson);
                    break;

                case "/kcsapi/api_req_kousyou/destroyitem2":
                    DestoryItem2(responseJson, queryParam);
                    break;


                case "/kcsapi/api_req_kousyou/createship":
                    _logCreateShip.Start(queryParam["api_kdock_id"]);
                    UpdateDetailStatus("艦娘建造を開始しました");
                    break;

                case "/kcsapi/api_req_kousyou/getship":
                    GetNewShip(responseJson);
                    break;

                case "/kcsapi/api_get_member/deck":
                    UpdateDeck(responseJson);
                    break;

                case "/kcsapi/api_req_kousyou/destroyship":
                    DestroyShip(responseJson, queryParam);
                    break;

                case "/kcsapi/api_req_kousyou/createship_speedchange":
                    SpeedChangeKDock(queryParam);
                    break;

                case "/kcsapi/api_req_nyukyo/start":
                    NDockStart(responseJson, queryParam);
                    break;

                case "/kcsapi/api_req_nyukyo/speedchange":
                    NDockSpeedChange(queryParam["api_ndock_id"]);

                    break;

                    /* 連合艦隊の戦闘
                     * 2014夏イベントで開催。取り敢えず解析はしない。
                     */

                case "/kcsapi/api_req_combined_battle/airbattle":
                    _statusManager.StartBattle();
                    UpdateDetailStatus("航空戦を開始しました");
                    break;

                case "/kcsapi/api_req_combined_battle/battle":
                    _statusManager.StartBattle();
                    UpdateDetailStatus("戦闘を開始しました");
                    break;

                case "/kcsapi/api_req_combined_battle/midnight_battle":
                    UpdateDetailStatus("夜戦へ移行しました");
                    break;

                case "/kcsapi/api_req_combined_battle/sp_midnight":
                    UpdateDetailStatus("夜戦を開始しました");
                    break;

                    /* 2014 秋イベでの水上聯合艦隊戦闘 */
                case "/kcsapi/api_req_combined_battle/battle_water":
                    _statusManager.StartBattle();
                    UpdateDetailStatus("戦闘を開始しました");
                    break;

                case "/kcsapi/api_req_kousyou/remodel_slotlist":
                    UpdateDetailStatus("改修可能な装備の一覧を取得しました");
                    break;

                case "/kcsapi/api_req_kousyou/remodel_slotlist_detail":
                    UpdateDetailStatus("改修対象装備の情報を取得しました");
                    break;

                case "/kcsapi/api_req_kousyou/remodel_slot":
                    RemodelSlotItem(responseJson);
                    break;

                case "/kcsapi/api_req_furniture/set_portbgm":
                    UpdateDetailStatus("母港BGMを変更しました");
                    break;

                    //無視するAPI勢
                    
                case "/kcsapi/api_get_member/unsetslot":
                case "/kcsapi/api_get_member/useitem":
                case "/kcsapi/api_get_member/furniture":
                case "/kcsapi/api_get_member/mapinfo":
                case "/kcsapi/api_get_member/mapcell":
                case "/kcsapi/api_get_member/payitem":
                case "/kcsapi/api_get_member/picture_book":
                case "/kcsapi/api_get_member/record":
                case "/kcsapi/api_get_member/mission":
                case "/kcsapi/api_get_member/sortie_conditions":

                case "/kcsapi/api_req_member/payitemuse":
                case "/kcsapi/api_req_member/itemuse":
                case "/kcsapi/api_req_member/updatecomment":
                case "/kcsapi/api_req_member/itemuse_cond":

                case "/kcsapi/api_req_furniture/change":
                case "/kcsapi/api_req_furniture/buy":
                case "/kcsapi/api_req_furniture/music_list":
                case "/kcsapi/api_req_furniture/music_play":

                case "/kcsapi/api_req_hensei/combined":

                case "/kcsapi/api_req_mission/return_instruction":

                case "/kcsapi/api_req_ranking/getlist":

                case "/kcsapi/api_req_quest/start":

                case "/kcsapi/api_req_kaisou/unsetslot_all":
                case "/kcsapi/api_req_kaisou/marriage":

                case "/kcsapi/api_req_map/select_eventmap_rank":

                    break;

                default:
                    _parent.UpdateStatus("未知のAPIを検出しました");
                    LogUnknownAPI(oSession);
                    break;

            }
            #endregion

/*
            var before = GC.GetTotalMemory(false);
            //GC起動
            GC.Collect();
            var after = GC.GetTotalMemory(false);

            Debug.WriteLine(string.Format("GC {0} -> {1} ({2})", before, after, before - after));
 */
        }

        /// <summary>
        /// 未知のAPIを保存する
        /// </summary>
        /// <param name="oSession"></param>
        private void LogUnknownAPI(SessionData oSession)
        {
            string fn = string.Format("./{0}.unknown.session", DateTime.Now.Ticks.ToString());
            Debug.WriteLine(string.Format("UnknownAPI:{0} save {1}",oSession.PathQuery,fn));
            oSession.SaveSession(fn);
        }

    }
}
