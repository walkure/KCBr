using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft;
using System.Reflection;
using Newtonsoft.Json;

namespace KCB
{

    namespace api_req_member
    {
        [Obfuscation(Exclude = true)]
        public class GetIncentive
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_count { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    }

    namespace api_get_member
    {
        /// <summary>
        /// 基本情報
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class Basic
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public string api_member_id { get; set; }
                public string api_nickname { get; set; }
                public string api_nickname_id { get; set; }
                public int api_active_flag { get; set; }
                public long api_starttime { get; set; }
                public int api_level { get; set; }
                public int api_rank { get; set; }
                public int api_experience { get; set; }
                public object api_fleetname { get; set; }
                public string api_comment { get; set; }
                public string api_comment_id { get; set; }
                public int api_max_chara { get; set; }
                public int api_max_slotitem { get; set; }
                public int api_max_kagu { get; set; }
                public int api_playtime { get; set; }
                public int api_tutorial { get; set; }
                public List<int> api_furniture { get; set; }
                public int api_count_deck { get; set; }
                public int api_count_kdock { get; set; }
                public int api_count_ndock { get; set; }
                public int api_fcoin { get; set; }
                public int api_st_win { get; set; }
                public int api_st_lose { get; set; }
                public int api_ms_count { get; set; }
                public int api_ms_success { get; set; }
                public int api_pt_win { get; set; }
                public int api_pt_lose { get; set; }
                public int api_pt_challenged { get; set; }
                public int api_pt_challenged_win { get; set; }
                public int api_firstflag { get; set; }
                public int api_tutorial_progress { get; set; }
                public List<int> api_pvp { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    
        /// <summary>
        /// 入渠情報
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class NDock
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_member_id { get; set; }
                /// <summary>
                /// ドック番号
                /// </summary>
                public int api_id { get; set; }
                public int api_state { get; set; }
                /// <summary>
                /// 艦船ユニークID
                /// </summary>
                public int api_ship_id { get; set; }
                public long api_complete_time { get; set; }
                public string api_complete_time_str { get; set; }
                public int api_item1 { get; set; }
                public int api_item2 { get; set; }
                public int api_item3 { get; set; }
                public int api_item4 { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public List<ApiData> api_data { get; set; }
        }

        /// <summary>
        /// 資材一覧
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class Material
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_member_id { get; set; }
                public int api_id { get; set; }
                public int api_value { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public List<ApiData> api_data { get; set; }
        }

        /// <summary>
        /// Ship2とShip3で使われる port getshipでも使う ship_deckでも使う
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class ApiDataShip
        {
            /// <summary>
            /// ユーザ内ユニーク艦船ID
            /// </summary>
            public int api_id { get; set; }
            public int api_sortno { get; set; }
            /// <summary>
            /// 艦船種別ID
            /// </summary>
            public int api_ship_id { get; set; }
            public int api_lv { get; set; }
            public List<int> api_exp { get; set; }
            public int api_nowhp { get; set; }
            public int api_maxhp { get; set; }
            public int api_leng { get; set; }
            /// <summary>
            /// スロットについてる装備id一覧
            /// </summary>
            public List<int> api_slot { get; set; }
            /// <summary>
            /// スロットごとの艦載機数
            /// </summary>
            public List<int> api_onslot { get; set; }
            public List<int> api_kyouka { get; set; }
            public int api_backs { get; set; }
            public int api_fuel { get; set; }
            public int api_bull { get; set; }
            /// <summary>
            /// スロット数(0,1,2,3,4)
            /// </summary>
            public int api_slotnum { get; set; }
            public int api_ndock_time { get; set; }
            public List<int> api_ndock_item { get; set; }
            public int api_srate { get; set; }
            public int api_cond { get; set; }
            public List<int> api_karyoku { get; set; }
            public List<int> api_raisou { get; set; }
            public List<int> api_taiku { get; set; }
            public List<int> api_soukou { get; set; }
            public List<int> api_kaihi { get; set; }
            public List<int> api_taisen { get; set; }
            public List<int> api_sakuteki { get; set; }
            public List<int> api_lucky { get; set; }
            public int api_locked { get; set; }
            public int api_sally_area { get; set; }
        }

        /// <summary>
        /// 所属艦隊と遠征情報(Ship2,3とDeckで使われる) ship_deckでも使う
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class ApiDataDeck
        {
            public int api_member_id { get; set; }
            public int api_id { get; set; }
            public string api_name { get; set; }
            public string api_name_id { get; set; }
            public List<long> api_mission { get; set; }
            public string api_flagship { get; set; }
            public List<int> api_ship { get; set; }
        }
        
        /// <summary>
        /// 艦隊情報　/kcsapi/api_get_member/deck
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class Deck
        {
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public List<ApiDataDeck> api_data { get; set; }
        }
        
        /// <summary>
        /// 艦艇作成 /kcsapi/api_get_member/kdock
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class KDock
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
//                public int api_member_id { get; set; }
                public int api_id { get; set; }
                public int api_state { get; set; }
                public int api_created_ship_id { get; set; }
                public long api_complete_time { get; set; }
                public string api_complete_time_str { get; set; }
                public int api_item1 { get; set; }
                public int api_item2 { get; set; }
                public int api_item3 { get; set; }
                public int api_item4 { get; set; }
                public int api_item5 { get; set; }
            }

            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public List<ApiData> api_data { get; set; }
        }

        /// <summary>
        /// 任務一覧 /kcsapi/api_get_member/questlist
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class QuestList
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                //api_listの最後に-1が入ることがあるので前述の構造だとパースに失敗することがある。狂気
                [Obfuscation(Exclude = true)]
                public class ApiList
                {
                    public int api_no { get; set; }
                    public int api_category { get; set; }
                    public int api_type { get; set; }
                    public int api_state { get; set; }
                    public string api_title { get; set; }
                    public string api_detail { get; set; }
                    public List<int> api_get_material { get; set; }
                    public int api_bonus_flag { get; set; }
                    public int api_progress_flag { get; set; }
                }
                public int api_count { get; set; }
                public int api_page_count { get; set; }
                public int api_disp_page { get; set; }
                public List<object> api_list { get; set; } // 基本的にはApiListなんだが、最後に-1が入ることがある。
                public int api_exec_count { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        /// <summary>
        /// 所有艦船更新情報
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class Ship2
        {
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public List<ApiDataShip> api_data { get; set; }
            public List<ApiDataDeck> api_data_deck { get; set; }
        }

        /// <summary>
        /// Ship3
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class Ship3
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                [Obfuscation(Exclude = true)]
                public class ApiSlotData
                {
                    public object api_slottype1 { get; set; }
                    public object api_slottype2 { get; set; }
                    public object api_slottype3 { get; set; }
                    public object api_slottype4 { get; set; }
                    public object api_slottype5 { get; set; }
                    public object api_slottype6 { get; set; }
                    public object api_slottype7 { get; set; }
                    public object api_slottype8 { get; set; }
                    public object api_slottype9 { get; set; }
                    public object api_slottype10 { get; set; }
                    public object api_slottype11 { get; set; }
                    public object api_slottype12 { get; set; }
                    public object api_slottype13 { get; set; }
                    public object api_slottype14 { get; set; }
                    public object api_slottype15 { get; set; }
                    public object api_slottype16 { get; set; }
                    public object api_slottype17 { get; set; }
                    public object api_slottype18 { get; set; }
                    public object api_slottype19 { get; set; }
                    public object api_slottype20 { get; set; }
                    public object api_slottype21 { get; set; }
                    public object api_slottype22 { get; set; }
                    public object api_slottype23 { get; set; }
                    public object api_slottype24 { get; set; }
                    public object api_slottype25 { get; set; }
                    public object api_slottype26 { get; set; }
                    public object api_slottype27 { get; set; }
                    public object api_slottype28 { get; set; }
                    public object api_slottype29 { get; set; }
                    public object api_slottype30 { get; set; }
                    public object api_slottype31 { get; set; }
                    public object api_slottype32 { get; set; }
                    public object api_slottype33 { get; set; }
                    public object api_slottype34 { get; set; }
                    public object api_slottype35 { get; set; }
                    public object api_slottype36 { get; set; }
                    public object api_slottype37 { get; set; }
                    public object api_slottype38 { get; set; }
                    public object api_slottype39 { get; set; }
                    public object api_slottype40 { get; set; }
                    public object api_slottype41 { get; set; }
                    public object api_slottype42 { get; set; }
                    public object api_slottype43 { get; set; }
                    public object api_slottype44 { get; set; }
                    public object api_slottype45 { get; set; }
                    public object api_slottype46 { get; set; }
                }

                public List<ApiDataShip> api_ship_data { get; set; }
                public List<ApiDataDeck> api_deck_data { get; set; }
                public ApiSlotData api_slot_data { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class Slot_Item
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_id { get; set; }
                public int api_slotitem_id { get; set; }
                public int api_locked { get; set; }
                public int api_level { get; set; }
                public int? api_alv { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public List<ApiData> api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class Ship_Deck
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public List<ApiDataShip> api_ship_data { get; set; }
                public List<ApiDataDeck> api_deck_data { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class RequireInfo
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                [Obfuscation(Exclude = true)]
                public class ApiBasic
                {
                    public int api_member_id { get; set; }
                    public int api_firstflag { get; set; }
                }

                [Obfuscation(Exclude = true)]
                public class ApiUnsetslot
                {
                    public List<int> api_slottype1 { get; set; }
                    public List<int> api_slottype2 { get; set; }
                    public List<int> api_slottype3 { get; set; }
                    public List<int> api_slottype4 { get; set; }
                    public List<int> api_slottype5 { get; set; }
                    public List<int> api_slottype6 { get; set; }
                    public List<int> api_slottype7 { get; set; }
                    public List<int> api_slottype8 { get; set; }
                    public List<int> api_slottype9 { get; set; }
                    public List<int> api_slottype10 { get; set; }
                    public List<int> api_slottype11 { get; set; }
                    public List<int> api_slottype12 { get; set; }
                    public List<int> api_slottype13 { get; set; }
                    public List<int> api_slottype14 { get; set; }
                    public List<int> api_slottype15 { get; set; }
                    public int api_slottype16 { get; set; }
                    public List<int> api_slottype17 { get; set; }
                    public List<int> api_slottype18 { get; set; }
                    public List<int> api_slottype19 { get; set; }
                    public int api_slottype20 { get; set; }
                    public List<int> api_slottype21 { get; set; }
                    public List<int> api_slottype22 { get; set; }
                    public List<int> api_slottype23 { get; set; }
                    public int api_slottype24 { get; set; }
                    public List<int> api_slottype25 { get; set; }
                    public List<int> api_slottype26 { get; set; }
                    public List<int> api_slottype27 { get; set; }
                    public List<int> api_slottype28 { get; set; }
                    public List<int> api_slottype29 { get; set; }
                    public int api_slottype30 { get; set; }
                    public List<int> api_slottype31 { get; set; }
                    public List<int> api_slottype32 { get; set; }
                    public List<int> api_slottype33 { get; set; }
                    public int api_slottype34 { get; set; }
                    public List<int> api_slottype35 { get; set; }
                    public List<int> api_slottype36 { get; set; }
                    public List<int> api_slottype37 { get; set; }
                    public int api_slottype38 { get; set; }
                    public List<int> api_slottype39 { get; set; }
                    public int api_slottype40 { get; set; }
                    public int api_slottype41 { get; set; }
                    public int api_slottype42 { get; set; }
                    public List<int> api_slottype43 { get; set; }
                    public List<int> api_slottype44 { get; set; }
                    public int api_slottype45 { get; set; }
                    public int api_slottype46 { get; set; }
                }

                [Obfuscation(Exclude = true)]
                public class ApiUseitem
                {
                    public int api_id { get; set; }
                    public int api_count { get; set; }
                }

                [Obfuscation(Exclude = true)]
                public class ApiFurniture
                {
                    public int api_id { get; set; }
                    public int api_furniture_type { get; set; }
                    public int api_furniture_no { get; set; }
                    public int api_furniture_id { get; set; }
                }

                public ApiBasic api_basic { get; set; }
                public List<Slot_Item.ApiData> api_slot_item { get; set; }
                public ApiUnsetslot api_unsetslot { get; set; }
                public List<KDock.ApiData> api_kdock { get; set; }
                public List<ApiUseitem> api_useitem { get; set; }
                public List<ApiFurniture> api_furniture { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

    }

    namespace api_req_sortie
    {
        [Obfuscation(Exclude = true)]
        public class BattleResult
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                [Obfuscation(Exclude = true)]
                public class ApiEnemyInfo
                {
                    public string api_level { get; set; }
                    public string api_rank { get; set; }
                    public string api_deck_name { get; set; }
                }

                [Obfuscation(Exclude = true)]
                public class ApiGetShip
                {
                    public int api_ship_id { get; set; }
                    public string api_ship_type { get; set; }
                    public string api_ship_name { get; set; }
                    public string api_ship_getmes { get; set; }
                }
                public List<int> api_ship_id { get; set; }
                public string api_win_rank { get; set; }
                public int api_get_exp { get; set; }
                public int api_mvp { get; set; }
                public int api_member_lv { get; set; }
                public int api_member_exp { get; set; }
                public int api_get_base_exp { get; set; }
                public List<int> api_get_ship_exp { get; set; }
                public List<List<int>> api_get_exp_lvup { get; set; }
                public int api_dests { get; set; }
                public int api_destsf { get; set; }
                public List<int> api_lost_flag { get; set; }
                public string api_quest_name { get; set; }
                public int api_quest_level { get; set; }
                public ApiEnemyInfo api_enemy_info { get; set; }
                public int api_first_clear { get; set; }
                public List<int> api_get_flag { get; set; }
                public ApiGetShip api_get_ship { get; set; }
                public int api_get_eventflag { get; set; }
            }

            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class Battle
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                [Obfuscation(Exclude = true)]
                public class ApiKouku
                {
                    [Obfuscation(Exclude = true)]
                    public class ApiStage1
                    {
                        public int api_f_count { get; set; }
                        public int api_f_lostcount { get; set; }
                        public int api_e_count { get; set; }
                        public int api_e_lostcount { get; set; }
                        public int api_disp_seiku { get; set; }
                        public List<int> api_touch_plane { get; set; }
                    }
                    [Obfuscation(Exclude = true)]
                    public class ApiStage2
                    {
                        public int api_f_count { get; set; }
                        public int api_f_lostcount { get; set; }
                        public int api_e_count { get; set; }
                        public int api_e_lostcount { get; set; }
                    }
                    [Obfuscation(Exclude = true)]
                    public class ApiStage3
                    {
                        public List<int> api_frai_flag { get; set; }
                        public List<int> api_erai_flag { get; set; }
                        public List<int> api_fbak_flag { get; set; }
                        public List<int> api_ebak_flag { get; set; }
                        public List<int> api_fcl_flag { get; set; }
                        public List<int> api_ecl_flag { get; set; }
                        /// <summary>
                        /// 航空戦の自艦隊ダメージ
                        /// </summary>
                        public List<double> api_fdam { get; set; }
                        /// <summary>
                        /// 航空戦の敵艦隊ダメージ
                        /// </summary>
                        public List<double> api_edam { get; set; }
                    }

                    public List<List<int>> api_plane_from { get; set; }
                    public ApiStage1 api_stage1 { get; set; }
                    public ApiStage2 api_stage2 { get; set; }
                    public ApiStage3 api_stage3 { get; set; }
                }
                [Obfuscation(Exclude = true)]
                public class ApiOpeningAtack
                {
                    public List<int> api_frai { get; set; }
                    public List<int> api_erai { get; set; }
                    /// <summary>
                    /// 開幕雷撃の自艦隊ダメージ
                    /// </summary>
                    public List<double> api_fdam { get; set; }
                    public List<double> api_edam { get; set; }
                    public List<int> api_fydam { get; set; }
                    public List<int> api_eydam { get; set; }
                    public List<int> api_fcl { get; set; }
                    public List<int> api_ecl { get; set; }
                }
                [Obfuscation(Exclude = true)]
                public class ApiHougeki
                {
                    /// <summary>
                    /// 砲撃する艦のorder(1-12)
                    /// </summary>
                    public List<int> api_at_list { get; set; }
                    /// <summary>
                    /// 攻撃形態 0が普通。2は連撃。3はカットインらしい
                    /// </summary>
                    public List<int> api_at_type { get; set; }
                    /// <summary>
                    /// 砲撃される艦のorder(1-12)
                    /// 連撃することがあるので、JArrayが入ってる
                    /// </summary>
                    public List<object> api_df_list { get; set; }
                    /// <summary>
                    /// 攻撃する際の装備(int)
                    /// 連撃することがあるので、JArrayが入ってる
                    /// 航空戦および該当装備がない(艦娘の自力攻撃)の時：-1
                    /// </summary>
                    public List<object> api_si_list { get; set; }
                    /// <summary>
                    /// 攻撃がクリティカルかどうか 2でcriticalっぽい
                    /// </summary>
                    public List<object> api_cl_list { get; set; }
                    /// <summary>
                    /// 砲撃された艦へのダメージ(double)
                    /// 連撃することがあるので、JArrayが入ってる
                    /// </summary>
                    public List<object> api_damage { get; set; }
                }
                [Obfuscation(Exclude = true)]
                public class ApiRaigeki
                {
                    public List<int> api_frai { get; set; }
                    public List<int> api_erai { get; set; }
                    public List<double> api_fdam { get; set; }
                    public List<double> api_edam { get; set; }
                    public List<int> api_fydam { get; set; }
                    public List<int> api_eydam { get; set; }
                    public List<int> api_fcl { get; set; }
                    public List<int> api_ecl { get; set; }
                }
                [Obfuscation(Exclude = true)]
                public class ApiSupportInfo
                {
                    [Obfuscation(Exclude = true)]
                    public class ApiSupportAiratack
                    {
                        [Obfuscation(Exclude = true)]
                        public class ApiStage1
                        {
                            public int api_f_count { get; set; }
                            public int api_f_lostcount { get; set; }
                            public int api_e_count { get; set; }
                            public int api_e_lostcount { get; set; }
                        }
                        [Obfuscation(Exclude = true)]
                        public class ApiStage2
                        {
                            public int api_f_count { get; set; }
                            public int api_f_lostcount { get; set; }
                        }
                        [Obfuscation(Exclude = true)]
                        public class ApiStage3
                        {
                            public List<int> api_erai_flag { get; set; }
                            public List<int> api_ebak_flag { get; set; }
                            public List<int> api_ecl_flag { get; set; }
                            /// <summary>
                            /// 敵へのダメージ[1-6]
                            /// </summary>
                            public List<double> api_edam { get; set; }
                        }

                        /// <summary>
                        /// 艦隊番号
                        /// </summary>
                        public int api_deck_id { get; set; }
                        /// <summary>
                        /// 艦娘UID
                        /// </summary>
                        public List<int> api_ship_id { get; set; }
                        /// <summary>
                        /// 中破してるかどうか？
                        /// </summary>
                        public List<int> api_undressing_flag { get; set; }
                        public List<int> api_stage_flag { get; set; }
                        public List<List<int>> api_plane_from { get; set; }
                        public ApiStage1 api_stage1 { get; set; }
                        public ApiStage2 api_stage2 { get; set; }
                        public ApiStage3 api_stage3 { get; set; }
                    }
                    [Obfuscation(Exclude = true)]
                    public class ApiSupportHourai
                    {
                        /// <summary>
                        /// 支援に来た艦隊番号
                        /// </summary>
                        public int api_deck_id { get; set; }
                        /// <summary>
                        /// 支援に来た艦娘UID
                        /// </summary>
                        public List<int> api_ship_id { get; set; }
                        public List<int> api_undressing_flag { get; set; }
                        public List<int> api_cl_list { get; set; }
                        /// <summary>
                        /// 敵へのダメージ[1-6]
                        /// </summary>
                        public List<double> api_damage { get; set; }
                    }

                    public ApiSupportAiratack api_support_airatack { get; set; }
                    public ApiSupportHourai api_support_hourai { get; set; }
                }

                /// <summary>
                /// 通常昼戦時の艦隊番号
                /// </summary>
                public int api_dock_id { get; set; }
                /// <summary>
                /// 通常昼戦以外(夜戦など)の艦隊番号
                /// </summary>
                public int api_deck_id { get; set; }
                public List<int> api_ship_ke { get; set; }
                public List<int> api_ship_lv { get; set; }
                /// <summary>
                /// 戦闘開始時のHP
                /// </summary>
                public List<int> api_nowhps { get; set; }
                /// <summary>
                /// 最大のHP(修理後の？)
                /// </summary>
                public List<int> api_maxhps { get; set; }
                /// <summary>
                /// 夜戦に突入可能かどうか
                /// </summary>
                public int api_midnight_flag { get; set; }
                /// <summary>
                /// 支援艦隊が来たかどうか
                /// 0:来なかった/支援なし 1:航空支援 2:砲撃支援 3:雷撃支援 
                /// </summary>
                public int api_support_flag { get; set; }
                /// <summary>
                /// 支援艦隊が来た場合の攻撃情報
                /// </summary>
                public ApiSupportInfo api_support_info { get; set; }
                public List<List<int>> api_eSlot { get; set; }
                public List<List<int>> api_eKyouka { get; set; }
                public List<List<int>> api_fParam { get; set; }
                public List<List<int>> api_eParam { get; set; }
                public List<int> api_search { get; set; }
                public List<int> api_formation { get; set; }
                public List<int> api_stage_flag { get; set; }
                /// <summary>
                /// 航空戦。航空戦がないときでもnullではないっぽい。夜戦の時はnull
                /// </summary>
                public ApiKouku api_kouku { get; set; }
                /// <summary>
                /// 開幕雷撃があれば1
                /// </summary>
                public int api_opening_flag { get; set; }
                /// <summary>
                /// 開幕雷撃情報。なければnull
                /// </summary>
                public ApiOpeningAtack api_opening_atack { get; set; }
                /// <summary>
                /// 昼戦の攻撃有無 
                /// 0:砲撃1順目 1:砲撃2順目 2:砲撃3順目 3:雷撃
                /// 存在すれば1 夜戦の時はnullになる(エントリが存在しない)
                /// </summary>
                public List<int> api_hourai_flag { get; set; }
                /// <summary>
                /// 昼戦砲撃1順目
                /// </summary>
                public ApiHougeki api_hougeki1 { get; set; }
                /// <summary>
                /// 昼戦砲撃2順目
                /// </summary>
                public ApiHougeki api_hougeki2 { get; set; }
                /// <summary>
                /// 昼戦砲撃3順目
                /// </summary>
                public ApiHougeki api_hougeki3 { get; set; }
                /// <summary>
                /// 昼戦雷撃戦
                /// </summary>
                public ApiRaigeki api_raigeki { get; set; }
                /// <summary>
                /// 夜戦砲撃戦 昼戦時は存在しない(nullになる)
                /// </summary>
                public ApiHougeki api_hougeki { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    }

    namespace api_req_kousyou
    {
        [Obfuscation(Exclude = true)]
        public class CreateItem
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_create_flag { get; set; }
                public int api_shizai_flag { get; set; }
                public api_get_member.Slot_Item.ApiData api_slot_item { get; set; }
                public List<int> api_material { get; set; }
                public int api_type3 { get; set; }
                public List<int> api_unsetslot { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class CreateShip
        {
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class DestroyItem2
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public List<int> api_get_material { get; set; }
            }

            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class GetShip
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_id { get; set; }
                public int api_ship_id { get; set; }
                public List<api_get_member.KDock.ApiData> api_kdock { get; set; }
                public api_get_member.ApiDataShip api_ship { get; set; }
                public List<api_get_member.Slot_Item.ApiData> api_slotitem { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }

        }

        [Obfuscation(Exclude = true)]
        public class DestroyShip
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public List<int> api_material { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class RemodelSlot
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                [Obfuscation(Exclude = true)]
                public class ApiAfterSlot
                {
                    public int api_id { get; set; }
                    public int api_slotitem_id { get; set; }
                    public int api_locked { get; set; }
                    public int api_level { get; set; }
                }
                public int api_remodel_flag { get; set; }
                public List<int> api_remodel_id { get; set; }
                public List<int> api_after_material { get; set; }
                public string api_voice_id { get; set; }
                public ApiAfterSlot api_after_slot { get; set; }
                public List<int> api_use_slot_id { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    }

    namespace api_req_mission
    {
        [Obfuscation(Exclude = true)]
        public class Result
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                [Obfuscation(Exclude = true)]
                public class ApiGetItem
                {
                    public int api_useitem_count { get; set; }
                    public int api_useitem_id { get; set; }
                    public string api_useitem_name { get; set; }
                }
                public List<int> api_ship_id { get; set; }
                public int api_clear_result { get; set; }
                public int api_get_exp { get; set; }
                public int api_member_lv { get; set; }
                public int api_member_exp { get; set; }
                public List<int> api_get_ship_exp { get; set; }
                public List<List<int>> api_get_exp_lvup { get; set; }
                public string api_maparea_name { get; set; }
                public string api_detail { get; set; }
                public string api_quest_name { get; set; }
                public int api_quest_level { get; set; }
                /*失敗した時、api_get_material はint64(-1)で帰ってくる。
                そうでなければNewtonsoft.Json.Linq.JArray
                 */
                public object api_get_material { get; set; }
                public List<int> api_useitem_flag { get; set; }
                public ApiGetItem api_get_item1 { get; set; }
                public ApiGetItem api_get_item2 { get; set; }
            }

            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class ReturnInstruction
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                // 3,遠征ID,帰還時間？,0
                public List<long> api_mission { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

    }

    namespace api_req_hokyu
    {
        /// <summary>
        /// /kcsapi/api_req_hokyu/charge
        /// </summary>
        [Obfuscation(Exclude = true)]
        public class Charge
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                [Obfuscation(Exclude = true)]
                public class ApiShip
                {
                    public int api_id { get; set; }
                    public int api_fuel { get; set; }
                    public int api_bull { get; set; }
                    public List<int> api_onslot { get; set; }
                }
                public List<ApiShip> api_ship { get; set; }
                public List<int> api_material { get; set; }
                public int api_use_bou { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    }

    namespace api_req_practice
    {
        [Obfuscation(Exclude = true)]
        public class BattleResult
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public List<int> api_ship_id { get; set; }
                public string api_win_rank { get; set; }
                public int api_get_exp { get; set; }
                public int api_member_lv { get; set; }
                public int api_member_exp { get; set; }
                public int api_get_base_exp { get; set; }
                public int api_mvp { get; set; }
                public List<int> api_get_ship_exp { get; set; }
                public List<List<int>> api_get_exp_lvup { get; set; }
                public int api_dests { get; set; }
                public int api_destsf { get; set; }
                public object api_enemy_info { get; set; }
            }

            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    }

    namespace api_req_kaisou
    {
        [Obfuscation(Exclude = true)]
        public class Powerup
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_powerup_flag { get; set; }
                public api_get_member.ApiDataShip api_ship { get; set; }
                public List<api_get_member.ApiDataDeck> api_deck { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class Lock
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_locked { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    }

    [Obfuscation(Exclude = true)]
    public class api_start2
    {
        [Obfuscation(Exclude = true)]
        public class ApiData
        {
            [Obfuscation(Exclude = true)]
            public class ApiMstShip
            {
                public int api_id { get; set; }
                public int api_sortno { get; set; }
                public string api_name { get; set; }
                public string api_yomi { get; set; }
                public int api_stype { get; set; }
                public int api_afterlv { get; set; }
                public string api_aftershipid { get; set; }
                public List<int> api_taik { get; set; }
                public List<int> api_souk { get; set; }
                public List<int> api_houg { get; set; }
                public List<int> api_raig { get; set; }
                public List<int> api_tyku { get; set; }
                public List<int> api_luck { get; set; }
                public int api_soku { get; set; }
                public int api_leng { get; set; }
                public int api_slot_num { get; set; }
                public List<int> api_maxeq { get; set; }
                public int api_buildtime { get; set; }
                public List<int> api_broken { get; set; }
                public List<int> api_powup { get; set; }
                public int api_backs { get; set; }
                public string api_getmes { get; set; }
                public int api_afterfuel { get; set; }
                public int api_afterbull { get; set; }
                public int api_fuel_max { get; set; }
                public int api_bull_max { get; set; }
                public int api_voicef { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstShipgraph
            {
                public int api_id { get; set; }
                public int api_sortno { get; set; }
                public string api_filename { get; set; }
                public List<string> api_version { get; set; }
                public List<int> api_boko_n { get; set; }
                public List<int> api_boko_d { get; set; }
                public List<int> api_kaisyu_n { get; set; }
                public List<int> api_kaisyu_d { get; set; }
                public List<int> api_kaizo_n { get; set; }
                public List<int> api_kaizo_d { get; set; }
                public List<int> api_map_n { get; set; }
                public List<int> api_map_d { get; set; }
                public List<int> api_ensyuf_n { get; set; }
                public List<int> api_ensyuf_d { get; set; }
                public List<int> api_ensyue_n { get; set; }
                public List<int> api_battle_n { get; set; }
                public List<int> api_battle_d { get; set; }
                public List<int> api_weda { get; set; }
                public List<int> api_wedb { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstSlotitemEquiptype
            {
                public int api_id { get; set; }
                public string api_name { get; set; }
                public int api_show_flg { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstStype
            {
                [Obfuscation(Exclude = true)]
                public class ApiEquipType
                {
                    [JsonProperty("1")]
                    public int Type1 { get; set; }
                    [JsonProperty("2")]
                    public int Type2 { get; set; }
                    [JsonProperty("3")]
                    public int Type3 { get; set; }
                    [JsonProperty("4")]
                    public int Type4 { get; set; }
                    [JsonProperty("5")]
                    public int Type5 { get; set; }
                    [JsonProperty("6")]
                    public int Type6 { get; set; }
                    [JsonProperty("7")]
                    public int Type7 { get; set; }
                    [JsonProperty("8")]
                    public int Type8 { get; set; }
                    [JsonProperty("9")]
                    public int Type9 { get; set; }
                    [JsonProperty("10")]
                    public int Type10 { get; set; }

                    [JsonProperty("11")]
                    public int Type11 { get; set; }
                    [JsonProperty("12")]
                    public int Type12 { get; set; }
                    [JsonProperty("13")]
                    public int Type13 { get; set; }
                    [JsonProperty("14")]
                    public int Type14 { get; set; }
                    [JsonProperty("15")]
                    public int Type15 { get; set; }
                    [JsonProperty("16")]
                    public int Type16 { get; set; }
                    [JsonProperty("17")]
                    public int Type17 { get; set; }
                    [JsonProperty("18")]
                    public int Type18 { get; set; }
                    [JsonProperty("19")]
                    public int Type19 { get; set; }
                    [JsonProperty("20")]
                    public int Type20 { get; set; }

                    [JsonProperty("21")]
                    public int Type21 { get; set; }
                    [JsonProperty("22")]
                    public int Type22 { get; set; }
                    [JsonProperty("23")]
                    public int Type23 { get; set; }
                    [JsonProperty("24")]
                    public int Type24 { get; set; }
                    [JsonProperty("25")]
                    public int Type25 { get; set; }
                    [JsonProperty("26")]
                    public int Type26 { get; set; }
                    [JsonProperty("27")]
                    public int Type27 { get; set; }
                    [JsonProperty("28")]
                    public int Type28 { get; set; }
                    [JsonProperty("29")]
                    public int Type29 { get; set; }
                    [JsonProperty("30")]
                    public int Type30 { get; set; }

                    [JsonProperty("31")]
                    public int Type31 { get; set; }
                    [JsonProperty("32")]
                    public int Type32 { get; set; }
                    [JsonProperty("33")]
                    public int Type33 { get; set; }
                    [JsonProperty("34")]
                    public int Type34 { get; set; }
                    [JsonProperty("35")]
                    public int Type35 { get; set; }
                    [JsonProperty("36")]
                    public int Type36 { get; set; }
                    [JsonProperty("37")]
                    public int Type37 { get; set; }
                    [JsonProperty("38")]
                    public int Type38 { get; set; }
                    [JsonProperty("39")]
                    public int Type39 { get; set; }
                    [JsonProperty("40")]
                    public int Type40 { get; set; }

                    [JsonProperty("41")]
                    public int Type41 { get; set; }
                    [JsonProperty("42")]
                    public int Type42 { get; set; }
                    [JsonProperty("43")]
                    public int Type43 { get; set; }
                    [JsonProperty("44")]
                    public int Type44 { get; set; }
                    [JsonProperty("45")]
                    public int Type45 { get; set; }
                    [JsonProperty("46")]
                    public int Type46 { get; set; }
                    [JsonProperty("47")]
                    public int Type47 { get; set; }
                    [JsonProperty("48")]
                    public int Type48 { get; set; }
                    [JsonProperty("49")]
                    public int Type49 { get; set; }
                    [JsonProperty("50")]
                    public int Type50 { get; set; }

                    [JsonProperty("51")]
                    public int Type51 { get; set; }
                    [JsonProperty("52")]
                    public int Type52 { get; set; }
                    [JsonProperty("53")]
                    public int Type53 { get; set; }
                    [JsonProperty("54")]
                    public int Type54 { get; set; }
                    [JsonProperty("55")]
                    public int Type55 { get; set; }
                    [JsonProperty("56")]
                    public int Type56 { get; set; }
                    [JsonProperty("57")]
                    public int Type57 { get; set; }
                    [JsonProperty("58")]
                    public int Type58 { get; set; }
                    [JsonProperty("59")]
                    public int Type59 { get; set; }
                    [JsonProperty("60")]
                    public int Type60 { get; set; }

                    [JsonProperty("61")]
                    public int Type61 { get; set; }
                    [JsonProperty("62")]
                    public int Type62 { get; set; }
                    [JsonProperty("63")]
                    public int Type63 { get; set; }
                    [JsonProperty("64")]
                    public int Type64 { get; set; }
                    [JsonProperty("65")]
                    public int Type65 { get; set; }
                    [JsonProperty("66")]
                    public int Type66 { get; set; }
                    [JsonProperty("67")]
                    public int Type67 { get; set; }
                    [JsonProperty("68")]
                    public int Type68 { get; set; }
                    [JsonProperty("69")]
                    public int Type69 { get; set; }
                    [JsonProperty("70")]
                    public int Type70 { get; set; }

                    [JsonProperty("71")]
                    public int Type71 { get; set; }
                    [JsonProperty("72")]
                    public int Type72 { get; set; }
                    [JsonProperty("73")]
                    public int Type73 { get; set; }
                    [JsonProperty("74")]
                    public int Type74 { get; set; }
                    [JsonProperty("75")]
                    public int Type75 { get; set; }
                    [JsonProperty("76")]
                    public int Type76 { get; set; }
                    [JsonProperty("77")]
                    public int Type77 { get; set; }
                    [JsonProperty("78")]
                    public int Type78 { get; set; }
                    [JsonProperty("79")]
                    public int Type79 { get; set; }
                    [JsonProperty("80")]
                    public int Type80 { get; set; }

                    [JsonProperty("81")]
                    public int Type81 { get; set; }
                    [JsonProperty("82")]
                    public int Type82 { get; set; }
                    [JsonProperty("83")]
                    public int Type83 { get; set; }
                    [JsonProperty("84")]
                    public int Type84 { get; set; }
                    [JsonProperty("85")]
                    public int Type85 { get; set; }
                    [JsonProperty("86")]
                    public int Type86 { get; set; }
                    [JsonProperty("87")]
                    public int Type87 { get; set; }
                    [JsonProperty("88")]
                    public int Type88 { get; set; }
                    [JsonProperty("89")]
                    public int Type89 { get; set; }
                    [JsonProperty("90")]
                    public int Type90 { get; set; }

                    [JsonProperty("91")]
                    public int Type91 { get; set; }
                    [JsonProperty("92")]
                    public int Type92 { get; set; }
                    [JsonProperty("93")]
                    public int Type93 { get; set; }
                    [JsonProperty("94")]
                    public int Type94 { get; set; }

                }

                public int api_id { get; set; }
                public int api_sortno { get; set; }
                public string api_name { get; set; }
                public int api_scnt { get; set; }
                public int api_kcnt { get; set; }
                public ApiEquipType api_equip_type { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstSlotitem
            {
                public int api_id { get; set; }
                public int api_sortno { get; set; }
                public string api_name { get; set; }
                public List<int> api_type { get; set; }
                public int api_taik { get; set; }
                public int api_souk { get; set; }
                public int api_houg { get; set; }
                public int api_raig { get; set; }
                public int api_soku { get; set; }
                public int api_baku { get; set; }
                public int api_tyku { get; set; }
                public int api_tais { get; set; }
                public int api_atap { get; set; }
                public int api_houm { get; set; }
                public int api_raim { get; set; }
                public int api_houk { get; set; }
                public int api_raik { get; set; }
                public int api_bakk { get; set; }
                public int api_saku { get; set; }
                public int api_sakb { get; set; }
                public int api_luck { get; set; }
                public int api_leng { get; set; }
                public int api_rare { get; set; }
                public List<int> api_broken { get; set; }
                public string api_info { get; set; }
                public string api_usebull { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstFurniture
            {
                public int api_id { get; set; }
                public int api_type { get; set; }
                public int api_no { get; set; }
                public string api_title { get; set; }
                public string api_description { get; set; }
                public int api_rarity { get; set; }
                public int api_price { get; set; }
                public int api_saleflg { get; set; }
                public int api_season { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstFurnituregraph
            {
                public int api_id { get; set; }
                public int api_type { get; set; }
                public int api_no { get; set; }
                public string api_filename { get; set; }
                public string api_version { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstUseitem
            {
                public int api_id { get; set; }
                public int api_usetype { get; set; }
                public int api_category { get; set; }
                public string api_name { get; set; }
                public List<string> api_description { get; set; }
                public int api_price { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstPayitem
            {
                public int api_id { get; set; }
                public int api_type { get; set; }
                public string api_name { get; set; }
                public string api_description { get; set; }
                public List<int> api_item { get; set; }
                public int api_price { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstItemShop
            {
                public List<int> api_cabinet_1 { get; set; }
                public List<int> api_cabinet_2 { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstMaparea
            {
                public int api_id { get; set; }
                public string api_name { get; set; }
                public int api_type { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstMapinfo
            {
                public int api_id { get; set; }
                public int api_maparea_id { get; set; }
                public int api_no { get; set; }
                public string api_name { get; set; }
                public int api_level { get; set; }
                public string api_opetext { get; set; }
                public string api_infotext { get; set; }
                public List<int> api_item { get; set; }
                public object api_max_maphp { get; set; }
                public int? api_required_defeat_count { get; set; }
                public List<int> api_sally_flag { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstMapbgm
            {
                public int api_id { get; set; }
                public int api_maparea_id { get; set; }
                public int api_no { get; set; }
                public List<int> api_map_bgm { get; set; }
                public List<int> api_boss_bgm { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstMapcell
            {
                public int api_map_no { get; set; }
                public int api_maparea_id { get; set; }
                public int api_mapinfo_no { get; set; }
                public int api_id { get; set; }
                public int api_no { get; set; }
                public int api_color_no { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstMission
            {
                public int api_id { get; set; }
                public int api_maparea_id { get; set; }
                public string api_name { get; set; }
                public string api_details { get; set; }
                public int api_time { get; set; }
                public int api_difficulty { get; set; }
                public double api_use_fuel { get; set; }
                public double api_use_bull { get; set; }
                public List<int> api_win_item1 { get; set; }
                public List<int> api_win_item2 { get; set; }
                public int api_return_flag { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstConst
            {
                [Obfuscation(Exclude = true)]
                public class ApiParallelQuestMax
                {
                    public string api_string_value { get; set; }
                    public int api_int_value { get; set; }
                }

                [Obfuscation(Exclude = true)]
                public class ApiDpflagQuest
                {
                    public string api_string_value { get; set; }
                    public int api_int_value { get; set; }
                }

                [Obfuscation(Exclude = true)]
                public class ApiBokoMaxShips
                {
                    public string api_string_value { get; set; }
                    public int api_int_value { get; set; }
                }
                public ApiParallelQuestMax api_parallel_quest_max { get; set; }
                public ApiDpflagQuest api_dpflag_quest { get; set; }
                public ApiBokoMaxShips api_boko_max_ships { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstShipupgrade
            {
                public int api_id { get; set; }
                public int api_current_ship_id { get; set; }
                public int api_original_ship_id { get; set; }
                public int api_upgrade_type { get; set; }
                public int api_upgrade_level { get; set; }
                public int api_drawing_count { get; set; }
                public int api_catapult_count { get; set; }
                public int api_sortno { get; set; }
            }

            [Obfuscation(Exclude = true)]
            public class ApiMstBgm
            {
                public int api_id { get; set; }
                public string api_name { get; set; }
            }
            public List<ApiMstShip> api_mst_ship { get; set; }
            public List<ApiMstShipgraph> api_mst_shipgraph { get; set; }
            public List<ApiMstSlotitemEquiptype> api_mst_slotitem_equiptype { get; set; }
            public List<int> api_mst_equip_exslot { get; set; }
            public List<ApiMstStype> api_mst_stype { get; set; }
            public List<ApiMstSlotitem> api_mst_slotitem { get; set; }
            public List<ApiMstFurniture> api_mst_furniture { get; set; }
            public List<ApiMstFurnituregraph> api_mst_furnituregraph { get; set; }
            public List<ApiMstUseitem> api_mst_useitem { get; set; }
            public List<ApiMstPayitem> api_mst_payitem { get; set; }
            public ApiMstItemShop api_mst_item_shop { get; set; }
            public List<ApiMstMaparea> api_mst_maparea { get; set; }
            public List<ApiMstMapinfo> api_mst_mapinfo { get; set; }
            public List<ApiMstMapbgm> api_mst_mapbgm { get; set; }
            public List<ApiMstMapcell> api_mst_mapcell { get; set; }
            public List<ApiMstMission> api_mst_mission { get; set; }
            public ApiMstConst api_mst_const { get; set; }
            public List<ApiMstShipupgrade> api_mst_shipupgrade { get; set; }
            public List<ApiMstBgm> api_mst_bgm { get; set; }
            public int api_register_status { get; set; }
        }
        public int api_result { get; set; }
        public string api_result_msg { get; set; }
        public ApiData api_data { get; set; }
    }

    namespace api_port
    {
        [Obfuscation(Exclude = true)]
        public class Port
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                // basic微妙に違う(api_large_dockがある)
                [Obfuscation(Exclude = true)]
                public class ApiBasic
                {
                    public string api_member_id { get; set; }
                    public string api_nickname { get; set; }
                    public string api_nickname_id { get; set; }
                    public int api_active_flag { get; set; }
                    public long api_starttime { get; set; }
                    public int api_level { get; set; }
                    public int api_rank { get; set; }
                    public int api_experience { get; set; }
                    public object api_fleetname { get; set; }
                    public string api_comment { get; set; }
                    public string api_comment_id { get; set; }
                    public int api_max_chara { get; set; }
                    public int api_max_slotitem { get; set; }
                    public int api_max_kagu { get; set; }
                    public int api_playtime { get; set; }
                    public int api_tutorial { get; set; }
                    public List<int> api_furniture { get; set; }
                    public int api_count_deck { get; set; }
                    public int api_count_kdock { get; set; }
                    public int api_count_ndock { get; set; }
                    public int api_fcoin { get; set; }
                    public int api_st_win { get; set; }
                    public int api_st_lose { get; set; }
                    public int api_ms_count { get; set; }
                    public int api_ms_success { get; set; }
                    public int api_pt_win { get; set; }
                    public int api_pt_lose { get; set; }
                    public int api_pt_challenged { get; set; }
                    public int api_pt_challenged_win { get; set; }
                    public int api_firstflag { get; set; }
                    public int api_tutorial_progress { get; set; }
                    public List<int> api_pvp { get; set; }
                    public int api_large_dock { get; set; }
                }
                [Obfuscation(Exclude = true)]
                public class ApiLog
                {
                    public int api_no { get; set; }
                    public string api_type { get; set; }
                    public string api_state { get; set; }
                    public string api_message { get; set; }
                }

                public List<api_get_member.Material.ApiData> api_material { get; set; }
                public List<api_get_member.ApiDataDeck> api_deck_port { get; set; }
                public List<api_get_member.NDock.ApiData> api_ndock { get; set; }
                public List<api_get_member.ApiDataShip> api_ship { get; set; }
                public ApiBasic api_basic { get; set; }
                public List<ApiLog> api_log { get; set; }

                /// <summary>
                /// 母港BGMのID
                /// </summary>
                public int api_p_bgm_id {get;set;}

                /// <summary>
                /// 同時受託可能任務数
                /// </summary>
                public int api_parallel_quest_count { get; set; }

            }

            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    }

    namespace api_req_hensei
    {
        [Obfuscation(Exclude = true)]
        public class Lock
        {
            [Obfuscation(Exclude = true)]
            public class ApiData
            {
                public int api_locked { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }

        [Obfuscation(Exclude = true)]
        public class PresetSelect
        {
            public class ApiData
            {
                [Obfuscation(Exclude = true)]
                public int api_member_id { get; set; }
                public int api_id { get; set; }
                public string api_name { get; set; }
                public string api_name_id { get; set; }
                public List<int> api_mission { get; set; }
                public string api_flagship { get; set; }
                public List<int> api_ship { get; set; }
            }
            public int api_result { get; set; }
            public string api_result_msg { get; set; }
            public ApiData api_data { get; set; }
        }
    }
}

