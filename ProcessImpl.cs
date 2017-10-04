using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace KCB2
{
    public partial class SessionProcessor
    {
        void Start2(string responseJson)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_start2>(responseJson);

            if ((int)json.api_result != 1)
                return;

            //まず装備を読み込む
            _masterItem.LoadItemMaster(json.api_data.api_mst_slotitem,json.api_data.api_mst_slotitem_equiptype);

            ///先に艦船種別を読んでおく。
            _masterShip.LoadShipType(json.api_data.api_mst_stype);
            _masterShip.LoadShipMaster(json.api_data.api_mst_ship, _masterItem);

            //任務を読む
            _masterMission.UpdateMission(json.api_data.api_mst_mission);

            _parent.UpdateMasterData(_masterShip, _masterItem);

            //夜戦判定画面表示時に猫ってリロードした際、判定画面を消去されないので消しておく
            _parent.EndWaitForNightBattle();

            UpdateDetailStatus("マスタ情報を読み込みました");
        }

        void Basic(string responseJson)
        {
            _memberBasic.Update(responseJson);

            _parent.UpdateMaxShipItemValue(_memberBasic.MaxShip, _memberBasic.MaxItem);
            _parent.UpdateDockCount(_memberBasic.KDock, _memberBasic.NDock);
            _parent.UpdateDeckCount(_memberBasic.Deck);
            _parent.UpdateBasicInfo(_memberBasic);

            _parent.UpdateMemberID(_memberBasic.MemberID);

            UpdateWindowTitle();
            UpdateDetailStatus("鎮守府情報を更新しました");
        }

        /// <summary>
        /// /kcsapi/api_get_member/slot_item
        /// </summary>
        /// <param name="responseJson"></param>
        void Slot_Item(string responseJson)
        {
            _memberItem.UpdateItem(responseJson, _masterItem);

            _memberItem.UpdateItemOwnerShip(_memberShip);
            _memberShip.ApplySlotItemData(_memberItem);

            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateItemList(_memberItem.ItemList);
            _parent.UpdateShipList(_memberShip.ShipList);

            UpdateWindowTitle();

            UpdateDetailStatus("装備一覧を更新しました");
        }

        void KDock(string responseJson)
        {
            _memberDock.UpdateBuildKDock(responseJson, _masterShip);
            var kit = _logCreateShip.CreateLogData(_memberDock, _memberBasic
                , _memberDeck, _memberShip);

            if (kit != null)
                _parent.AddCreateShipResult(kit);
            _parent.UpdateBuildDock(_memberDock);

            UpdateDetailStatus("建造ドック情報を更新しました");
        }

        /// <summary>
        /// /kcs_api/api_port/port
        /// </summary>
        /// <param name="responseJson"></param>
        void Port(string responseJson)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_port.Port>(responseJson);
            if ((int)json.api_result != 1)
                return;

            _memberBasic.UpdatePort(json.api_data.api_basic);
            _memberMaterial.Update(json.api_data.api_material);

            //deck
            _memberDeck.UpdateDeck(json.api_data.api_deck_port, _masterMission);

            _memberShip.LoadShipInfo(json.api_data.api_ship, _masterShip, 0);
            _memberDock.UpdateRepairNDock(json.api_data.api_ndock, _memberShip);

            _memberShip.UpdateNDockInfo(_memberDock);

            //material
            _parent.UpdateMaterialData(_memberMaterial);
            var matret = _logMaterial.RecordMaterial(_memberMaterial, _memberBasic);
            _parent.AddMaterialsChange(matret);


            //ndock
            _memberShip.UpdateNDockInfo(_memberDock);


            //ship
            _memberShip.ApplySlotItemData(_memberItem);
            _memberShip.UpdateDeckInfo(_memberDeck);

            //basic
            _parent.UpdateMaxShipItemValue(_memberBasic.MaxShip, _memberBasic.MaxItem);
            _parent.UpdateDockCount(_memberBasic.KDock, _memberBasic.NDock);
            _parent.UpdateDeckCount(_memberBasic.Deck);
            _parent.UpdateBasicInfo(_memberBasic);

            _parent.UpdateMemberID(_memberBasic.MemberID);
            UpdateWindowTitle();

            _parent.UpdateRepairDock(_memberDock);
            _parent.UpdateShipListDock(_memberDock);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateShipList(_memberShip.ShipList);
            _parent.UpdateItemOwner(_memberShip.ItemOwnerList, _memberItem.SlotItemTypeDic);

            _statusManager.FinishSortie();

            UpdateDetailStatus("母港情報を更新しました");
        }

        void Ship2(string responseJson)
        {
            _memberShip.LoadShip2(responseJson, _masterShip, _memberDeck,_masterMission);

            _memberShip.UpdateNDockInfo(_memberDock);

            /* APIはship2->slotitem->deckの順に呼ばれる
                * ship2ではdeck相当のデータも返しているので処理している
                */
            _memberShip.ApplySlotItemData(_memberItem);
            _memberShip.UpdateDeckInfo(_memberDeck);

            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateShipList(_memberShip.ShipList);

            _parent.UpdateItemOwner(_memberShip.ItemOwnerList, _memberItem.SlotItemTypeDic);

            UpdateWindowTitle();

            UpdateDetailStatus("艦娘状況(2)を更新しました");

        }

        /// <summary>
        /// /kcsapi/api_get_member/ship3
        /// </summary>
        /// <param name="responseJson"></param>
        /// <param name="queryParam"></param>
        void Ship3(string responseJson, IDictionary<string, string> queryParam)
        {
            _memberShip.LoadShip3(responseJson, _masterShip,_masterMission, _memberDeck, queryParam);

            _memberShip.UpdateNDockInfo(_memberDock);

            /* APIはship3->slot_itemの順に呼ばれる
             * ship3ではdeck相当のデータも返しているので処理している
             */
            _memberShip.ApplySlotItemData(_memberItem);
            _memberShip.UpdateDeckInfo(_memberDeck);

            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateShipList(_memberShip.ShipList);

            _parent.UpdateItemOwner(_memberShip.ItemOwnerList, _memberItem.SlotItemTypeDic);

            UpdateWindowTitle();

            UpdateDetailStatus("艦娘状況(3)を更新しました");
        }

        void Charge(string responseJson)
        {
            int baux_charge = _memberMaterial.UpdateOnCharge(responseJson);

            //補給したことを反映する
            _memberShip.Charge(responseJson);
            _parent.UpdateMaterialData(_memberMaterial);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateShipList(_memberShip.ShipList);

            if (baux_charge > 0)
                UpdateDetailStatus("補給、ボーキサイトを{0}補給しました", baux_charge);
            else
                UpdateDetailStatus("補給しました");

        }

        void DeckMemberChange(IDictionary<string, string> queryParam)
        {
            _memberDeck.ChangeDeckMember(queryParam, _memberShip,_masterMission);
            _memberShip.UpdateDeckInfo(_memberDeck);

            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateShipList(_memberShip.ShipList);

            UpdateDetailStatus("艦隊編成を組み替えました");
        }

        void UpdateQuestList(string responseJson)
        {
            _memberQuest.UpdateQuest(responseJson);
            _parent.UpdateQuestList(_memberQuest.GetActiveQuests());

            UpdateDetailStatus("任務情報を更新しました");
        }

        void MissionResult(string responseJson)
        {
            var minfo = _logMission.CreateResult(responseJson, _memberShip, _memberBasic);
            _parent.AddMissionResult(minfo);

            //            string fn = string.Format("./{0}.log.session", DateTime.Now.Ticks.ToString());
            //            oSession.SaveSession(fn);

            UpdateDetailStatus("遠征を終了しました");
            return;
        }

        /// <summary>
        /// /kcsapi/api_req_sortie/battleresult
        /// /kcsapi/api_req_combined_battle/battleresult
        /// </summary>
        /// <param name="responseJson"></param>
        /// <param name="battleResult"></param>
        void BattleResult(string responseJson,BattleResult.Result battleResult)
        {
#if DEBUG
            if (battleResult != null)
                Debug.WriteLine(battleResult.ToString());
            else
                Debug.WriteLine("BattleResult is null");
#endif

            _parent.EndWaitForNightBattle();

            var binfo = _logBattle.Finish(responseJson,
                _memberShip, _memberDeck, _masterShip, _memberBasic);
            _parent.AddBattleResult(binfo);

            _statusManager.FinishBattle();

            //戦闘で受けた友軍ダメージを反映する
            _memberShip.ApplyBattleResult(battleResult);

            //推測戦闘結果
            if (battleResult != null)
            {
                var st = battleResult.BattleState;
                var st2 = KCB2.BattleResult.Result.BattleResultStateString(st);

                //HP減少分をUIへ反映
                _parent.UpdateShipList(_memberShip.ShipList);
                _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);

                if (binfo.ShipDropped.Length > 0 && !Properties.Settings.Default.HideDroppedShip)
                    UpdateDetailStatus("評価{0}({2})で戦闘を終了、{1}がドロップしました",
                        binfo.Rank, binfo.ShipDropped, st2);
                else
                    UpdateDetailStatus("評価{0}({1})で戦闘を終了しました", binfo.Rank, st2);

                if (binfo.Rank != KCB2.BattleResult.Result.BattleResultStateShortString(st))
                {
                    string fn = string.Format("./{0}.missing.battleresult", DateTime.Now.Ticks.ToString());
                    Debug.WriteLine("BattleResult Missing log:" + fn);

                    using (var sw = new System.IO.StreamWriter(fn, true, Encoding.UTF8))
                    {
                        sw.WriteLine("Result(Official):{0}\r\nResult(Self):{1}\r\n\r\n{2}",
                            binfo.Rank, st2, battleResult.ToString());
                    }

                }
            }
            else
            {
                //戦闘解析データがない場合は評価しない。
                _parent.UpdateShipList(_memberShip.ShipList);
                _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);

                if (binfo.ShipDropped.Length > 0 && !Properties.Settings.Default.HideDroppedShip)
                    UpdateDetailStatus("評価{0}で戦闘を終了、{1}がドロップしました",
                        binfo.Rank, binfo.ShipDropped);
                else
                    UpdateDetailStatus("評価{0}で戦闘を終了しました", binfo.Rank);
            }
            _parent.NotifyFinishBattle("戦闘");
        }

        void UpdateDeckName(IDictionary<string, string> queryParam)
        {
            _memberDeck.UpdateDeckName(queryParam);

            _memberShip.UpdateDeckInfo(_memberDeck);

            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateShipList(_memberShip.ShipList);
            UpdateDetailStatus("艦隊名を変更しました");
        }

        void CreateItem(string responseJson, IDictionary<string, string> queryParam)
        {

            _memberMaterial.UpdateOnCreateItem(responseJson);
            _memberItem.AddNewSlotItem(responseJson, _masterItem);

            _parent.UpdateMaterialData(_memberMaterial);
            _parent.UpdateItemList(_memberItem.ItemList);

            var citem = _logCreateItem.CreateLogData(responseJson, queryParam, _masterItem, _memberDeck, _memberShip, _memberBasic);

            if (citem != null)
            {
                _parent.AddCreateItemResult(citem);

                if (citem.Succeess)
                {
                    UpdateDetailStatus("装備開発に成功、{0}を入手しました", citem.ItemName);
                }
                else
                    UpdateDetailStatus("装備開発に失敗しました");
            }
            else
                UpdateDetailStatus("装備開発を試行しました");

            UpdateWindowTitle();

        }

        void PowerUp(string responseJson, IDictionary<string, string> queryParam)
        {
            bool bPwupResult = _memberShip.Powerup(responseJson,queryParam, _memberShip,
                _memberDeck, _memberItem, _masterShip, _masterMission);

            _parent.UpdateShipList(_memberShip.ShipList);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateItemList(_memberItem.ItemList);
            _parent.UpdateItemOwner(_memberShip.ItemOwnerList, _memberItem.SlotItemTypeDic);

            UpdateDetailStatus("近代化改修に{0}しました",
                bPwupResult ? "成功" : "失敗");

            UpdateWindowTitle();
        }

        void UpdateRepairNDock(string responseJson)
        {
            _memberDock.UpdateRepairNDock(responseJson, _memberShip);
            _memberShip.UpdateNDockInfo(_memberDock);

            _parent.UpdateRepairDock(_memberDock);
            _parent.UpdateShipListDock(_memberDock);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            UpdateDetailStatus("修復ドック情報を更新しました");
        }

        void UpdateMaterial(string responseJson)
        {
            _memberMaterial.Update(responseJson);

            _parent.UpdateMaterialData(_memberMaterial);
            var matret2 = _logMaterial.RecordMaterial(_memberMaterial, _memberBasic);
            _parent.AddMaterialsChange(matret2);
            UpdateDetailStatus("資源情報を更新しました");

        }

        void DestoryItem2(string responseJson, IDictionary<string, string> queryParam)
        {
            _memberMaterial.UpdateOnDestroyItem2(responseJson);
            _memberItem.DestoryItem(queryParam["api_slotitem_ids"]);

            _parent.UpdateMaterialData(_memberMaterial);
            _parent.UpdateItemList(_memberItem.ItemList);
            UpdateWindowTitle();

        }

        void PracticeResult(string responseJson)
        {
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject
                <KCB.api_req_practice.BattleResult>(responseJson);

            string result;

            if (json == null)
                result = "(JSON解析失敗)";

            if (json.api_result != 1)
                result = "(APIエラー)";

            result = json.api_data.api_win_rank;

            UpdateDetailStatus("評価{0}で演習を終了しました", result);
            _parent.EndWaitForNightBattle();
            _parent.NotifyFinishBattle("演習");
        }

        void GetNewShip(string responseJson)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kousyou.GetShip>(responseJson);
            if ((int)json.api_result != 1)
                return;

            _memberDock.UpdateBuildKDock(json.api_data.api_kdock,_masterShip);
            _memberItem.AddNewSlotItems(json.api_data.api_slotitem, _masterItem);
            _memberShip.AddShip(json.api_data.api_ship,_masterShip);

            _memberItem.UpdateItemOwnerShip(_memberShip);
            _memberShip.ApplySlotItemData(_memberItem);

            _parent.UpdateBuildDock(_memberDock);
            _parent.UpdateItemList(_memberItem.ItemList);
            _parent.UpdateShipList(_memberShip.ShipList);
            UpdateWindowTitle();

            UpdateDetailStatus("艦船を建造終了しました");

        }

        void UpdateDeck(string responseJson)
        {
            _memberDeck.UpdateDeck(responseJson,_masterMission);
            _memberShip.UpdateDeckInfo(_memberDeck);

            _parent.UpdateShipListDeck(_memberDeck);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            UpdateDetailStatus("艦隊情報を更新しました");
        }

        void DestroyShip(string responseJson, IDictionary<string, string> queryParam)
        {
            _memberMaterial.UpdateOnDestroyShip(responseJson);
            
            int ship_id = int.Parse(queryParam["api_ship_id"]);

            var ship = _memberShip.GetShip(ship_id);
            //装備がついていれば削除
            foreach (var it in ship.SlotItem)
            {
                _memberItem.DestoryItem(it.ID);
            }

            //艦船をリストから削除
            _memberShip.DestoryShip(ship_id);

            _parent.UpdateShipList(_memberShip.ShipList);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateItemList(_memberItem.ItemList);

            UpdateWindowTitle();

            UpdateDetailStatus("艦船を解体しました");
        }

        void NDockStart(string responseJson, IDictionary<string, string> queryParam)
        {

            _memberMaterial.UpdateOnNDockStart(queryParam, _memberShip);
            bool bFastRepair = _memberShip.RepairShip(queryParam);

            _parent.UpdateMaterialData(_memberMaterial);
            if (bFastRepair)
            {
                _parent.UpdateShipList(_memberShip.ShipList);
                _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            }
        }

        void NDockSpeedChange(string ndock_id)
        {
            _memberMaterial.UseFastRepair();

            int ship_id = _memberDock.UseFastRepair(ndock_id);
            _memberShip.RepairShip(ship_id);

            _parent.UpdateMaterialData(_memberMaterial);
            _parent.UpdateShipList(_memberShip.ShipList);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateRepairDock(_memberDock);

        }

        void LockShip(string responseJson, string ship_id_s)
        {
            int ship_id;
            if (!int.TryParse(ship_id_s, out ship_id))
                return;

            var result = _memberShip.Lock(responseJson, ship_id);
            if (result == MemberData.APIResponse.Error)
                return;

            bool bLock = result == MemberData.APIResponse.True;

            _parent.UpdateShipLock(ship_id,bLock);
            UpdateDetailStatus("艦娘をロック{0}しました", bLock ? "" : "解除");
        }

        void LockSlotItem(string responseJson, string slotitem_id_s)
        {
            int slotitem_id;
            if (!int.TryParse(slotitem_id_s, out slotitem_id))
                return;

            var result = _memberItem.Lock(responseJson, slotitem_id);
            if (result == MemberData.APIResponse.Error)
                return;

            bool bLock = result == MemberData.APIResponse.True;

            _parent.UpdateSlotItemLock(slotitem_id, bLock);
            UpdateDetailStatus("装備をロック{0}しました", bLock ? "" : "解除");
        }

        void RemodelSlotItem(string responseJson)
        {
            _memberMaterial.UpdateOnRemodelSlotItem(responseJson);
            _parent.UpdateMaterialData(_memberMaterial);

            bool result = _memberItem.RemodelSlotItem(responseJson,_masterItem);
            _parent.UpdateItemList(_memberItem.ItemList);
            UpdateWindowTitle();

            UpdateDetailStatus("装備の改修に{0}しました", result ? "成功" : "失敗");
        }

        void MidnightBattle(string responseJson)
        {
            //昼戦の結果を反映
            _memberShip.ApplyBattleResult(_battleResultManager.GetBattleResult());
            _parent.UpdateShipList(_memberShip.ShipList);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);

            //夜戦の状況を取得する
            _battleResultManager.SwitchNightBattle(responseJson, _masterItem);
            _parent.EndWaitForNightBattle();
            UpdateDetailStatus("夜戦へ移行しました");

        }

        void ShipDeck(string responseJson, IDictionary<string,string> queryParam)
        {
            //情報を更新する艦隊ID 連合艦隊の時は「"1,2"」の形式
            string deck_id = queryParam["api_deck_rid"];

            var json = JsonConvert.DeserializeObject<KCB.api_get_member.Ship_Deck>(responseJson);
            if (json.api_result != 1)
            {
                return;
            }

            //艦娘情報を更新
            _memberShip.UpdateShipsInfo(json.api_data.api_ship_data, _masterShip);
            _memberShip.ApplySlotItemData(_memberItem);
            _parent.UpdateShipList(_memberShip.ShipList);

            /*
             * "出撃中"の艦隊deck情報も降ってきている。
             * 轟沈した場合は更新しないといけないのでは？
             * MemberData.Deck.UpdateDeck の現在の実装だと一部艦隊の情報だけ
             * 降ってくる場合に未対応なので実装変えないといけないね。
            */
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);


            _statusManager.UpdateShipStatus(json,_masterShip,_memberItem);
            UpdateDetailStatus("第{0}艦隊の情報を更新しました",deck_id);
        }

        /// <summary>
        /// 建造ドックのバーナー使用
        /// </summary>
        /// <param name="queryParam"></param>
        void SpeedChangeKDock(IDictionary<string, string> queryParam)
        {
            string kdock_id_s = queryParam["api_kdock_id"];
            string highspeed_s = queryParam["api_highspeed"];

            if (highspeed_s != "1")
                return;

            _memberDock.UseBurner(kdock_id_s);
            _parent.UpdateBuildDock(_memberDock);
        }

        /// <summary>
        /// 設定済み編成の読み込み
        /// </summary>
        /// <param name="queryParam"></param>
        /// <param name="responseJson"></param>
        void LoadPresetDeck(IDictionary<string, string> queryParam,string responseJson)
        {

            _memberDeck.LoadPresetDeck(queryParam, responseJson,_masterMission);

            _memberShip.UpdateDeckInfo(_memberDeck);

            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateShipList(_memberShip.ShipList);


            UpdateDetailStatus("スロット{1}の編成記録を第{0}艦隊へ展開させました", queryParam["api_deck_id"], queryParam["api_preset_no"]);
        }

        /// <summary>
        /// api_get_member/require_info
        /// </summary>
        /// <param name="responseJson">JSON</param>
        void RequireInfo(string responseJson)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.RequireInfo>(responseJson);

            if (json.api_result != 1)
                return;

            //basic
            _memberBasic.UpdateRequireInfo(json.api_data.api_basic);

            //slot_item
            _memberItem.UpdateItem(json.api_data.api_slot_item, _masterItem);
            _memberItem.UpdateItemOwnerShip(_memberShip);
            _memberShip.ApplySlotItemData(_memberItem);

            UpdateDetailStatus("装備一覧を更新しました");
            UpdateWindowTitle();

            //kdock
            _memberDock.UpdateBuildKDock(json.api_data.api_kdock, _masterShip);
            var kit = _logCreateShip.CreateLogData(_memberDock, _memberBasic
                , _memberDeck, _memberShip);

            if (kit != null)
                _parent.AddCreateShipResult(kit);
            _parent.UpdateBuildDock(_memberDock);
            _parent.UpdateDeckMemberList(_memberShip, _memberDeck.DeckList);
            _parent.UpdateItemList(_memberItem.ItemList);
            _parent.UpdateShipList(_memberShip.ShipList);
        }

        /// <summary>
        /// /kcsapi/api_req_kaisou/slot_deprive
        /// </summary>
        /// <param name="queryParam"></param>
        /// <param name="responseJson"></param>
        /// <param name="_memberShip"></param>
        /// <param name="_memberItem"></param>
        void PullOutSlotItem(IDictionary<string, string> queryParam, string responseJson,
            MemberData.Ship _memberShip, MemberData.Item _memberItem)
        {
            int srcShipId,dstShipId;

            if(!int.TryParse(queryParam["api_unset_ship"],out srcShipId) ||
                !int.TryParse(queryParam["api_set_ship"], out dstShipId) )
                return;

            var json = JsonConvert.DeserializeObject<KCB.api_req_kaisou.SlotDeprive>(responseJson);
            if (json.api_result != 1)
                return;

            // 装備装着先変更
            _memberShip.LoadShipInfo(new List<KCB.api_get_member.ApiDataShip>() { json.api_data.api_ship_data.api_unset_ship }
                , _masterShip, json.api_data.api_ship_data.api_unset_ship.api_id);
            _memberShip.LoadShipInfo(new List<KCB.api_get_member.ApiDataShip>() { json.api_data.api_ship_data.api_set_ship }
                , _masterShip, json.api_data.api_ship_data.api_set_ship.api_id);

            _memberItem.UpdateItemOwnerShip(_memberShip);

            _memberShip.ApplySlotItemData(_memberItem);
            _memberShip.UpdateDeckInfo(_memberDeck);

            UpdateDetailStatus("装備を引き抜き装着しました");

            _parent.UpdateSlotItemInfo(json.api_data.api_ship_data.api_unset_ship.api_id);
            _parent.UpdateSlotItemInfo(json.api_data.api_ship_data.api_set_ship.api_id);

        }

        /// <summary>
        /// /kcsapi/api_req_kaisou/slot_exchange_index
        /// </summary>
        /// <param name="queryParam"></param>
        /// <param name="responseJson"></param>
        /// <param name="_memberShip"></param>
        void ChangeSlotItemIndex(string shipId_s, string responseJson,
            MemberData.Ship _memberShip)
        {

            // 装備順番入れ替え後のスロット情報
            var json = JsonConvert.DeserializeObject<KCB.api_req_kaisou.SlotExchangeIndex>(responseJson);
            if (json.api_result != 1)
                return;

            // スロット入れ替えを反映
            var ship_id = _memberShip.UpdateSlotItemOrder(shipId_s,json.api_data.api_slot);
            if (ship_id < 0)
                return;

            UpdateDetailStatus("スロット入れ替えを反映しました");

            _parent.UpdateSlotItemInfo(ship_id);
        }
    }
}
