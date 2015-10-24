using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KCB2.MemberData
{
    /// <summary>
    /// 資材情報
    /// /kcsapi/api_port/port
    /// /kcsapi/api_req_hokyu/charge
    /// /kcsapi/api_req_nyukyo/start
    /// </summary>
    public class Material
    {
        /// <summary>
        /// 燃料
        /// </summary>
        public int Fuel { get; private set; }
        /// <summary>
        /// 弾薬
        /// </summary>
        public int Ammo { get; private set; }
        /// <summary>
        /// 鋼材
        /// </summary>
        public int Steel { get; private set; }
        /// <summary>
        /// ボーキ
        /// </summary>
        public int Bauxite { get; private set; }

        /// <summary>
        /// 高速建造材
        /// </summary>
        public int FastBuild { get; private set; }
        /// <summary>
        /// バケツ
        /// </summary>
        public int FastRepair { get; private set; }
        /// <summary>
        /// 開発資材
        /// </summary>
        public int Developer { get; private set; }

        /// <summary>
        /// 改修資材
        /// </summary>
        public int Updater { get; private set; }

        public Material()
        {
            Fuel = -1;
            Ammo = -1;
            Steel = -1;
            FastBuild = -1;
            FastRepair = -1;
            Developer = -1;
            Updater = -1;
        }

        /// <summary>
        /// /kcsapi/api_get_member/material
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool Update(String JSON)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_get_member.Material>(JSON);
            if ((int)json.api_result != 1)
                return false;

            return Update(json.api_data);
        }


        public bool Update(List<KCB.api_get_member.Material.ApiData> jsons)
        {
            foreach (var item in jsons)
            {
                switch ((int)item.api_id)
                {
                    case 1:
                        Fuel = item.api_value;
                        break;
                    case 2:
                        Ammo = item.api_value;
                        break;
                    case 3:
                        Steel = item.api_value;
                        break;
                    case 4:
                        Bauxite = item.api_value;
                        break;
                    case 5:
                        FastBuild = item.api_value;
                        break;
                    case 6:
                        FastRepair = item.api_value;
                        break;
                    case 7:
                        Developer = item.api_value;
                        break;
                    case 8:
                        Updater = item.api_value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("unknown item ID");
                }
            }

            Debug.WriteLine(string.Format("Fuel:{0} Ammo:{1} Steel:{2} Bauxite:{3} FastRepair:{4} FastBuild:{5} Dev:{6} Upd:{7}",
                Fuel, Ammo, Steel, Bauxite, FastRepair, FastBuild, Developer, Updater));

            return true;
        }

        /// <summary>
        /// /kcsapi/api_req_hokyu/charge
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns>ボーキ消費量</returns>
        public int UpdateOnCharge(string JSON)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_hokyu.Charge>(JSON);
            if ((int)json.api_result != 1)
                return 0;

            var dat = json.api_data.api_material;

            Fuel = dat[0];
            Ammo = dat[1];
            Steel = dat[2];
            Bauxite = dat[3];

            return (int)json.api_data.api_use_bou;
        }

        /// <summary>
        /// /kcsapi/api_req_kousyou/createitem
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns>開発成功していればtrue そうでなければfalse</returns>
        public bool UpdateOnCreateItem(string JSON)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kousyou.CreateItem>(JSON);
            if ((int)json.api_result != 1)
                return false;

            var dat = json.api_data.api_material;

            Fuel = dat[0];
            Ammo = dat[1];
            Steel = dat[2];
            Bauxite = dat[3];
            FastBuild = dat[4];
            FastRepair = dat[5];
            Developer = dat[6];
            Updater = dat[7];

            return json.api_data.api_create_flag == 1;
        }

        /// <summary>
        /// /kcsapi/api_req_kousyou/destroyitem2
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool UpdateOnDestroyItem2(string JSON)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kousyou.DestroyItem2>(JSON);
            if ((int)json.api_result != 1)
                return false;

            var dat = json.api_data.api_get_material;

            Fuel += dat[0];
            Ammo += dat[1];
            Steel += dat[2];
            Bauxite += dat[3];

            return true;
        }

        /// <summary>
        /// /kcsapi/api_req_kousyou/destroyship
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns></returns>
        public bool UpdateOnDestroyShip(string JSON)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kousyou.DestroyShip>(JSON);
            if ((int)json.api_result != 1)
                return false;

            var dat = json.api_data.api_material;

            Fuel = dat[0];
            Ammo = dat[1];
            Steel = dat[2];
            Bauxite = dat[3];

            return true;
        }

        public bool UpdateOnRemodelSlotItem(string JSON)
        {
            var json = JsonConvert.DeserializeObject<KCB.api_req_kousyou.RemodelSlot>(JSON);
            if ((int)json.api_result != 1)
                return false;

            var dat = json.api_data.api_after_material;
            if (dat == null)
                return false;

            Fuel = dat[0];
            Ammo = dat[1];
            Steel = dat[2];
            Bauxite = dat[3];

            FastBuild = dat[4];
            FastRepair = dat[5];
            Developer = dat[6];
            Updater = dat[7];

            return true;
        }

        /// <summary>
        /// /kcsapi/api_req_nyukyo/start
        /// 入渠に伴う資材消費を反映させる
        /// </summary>
        /// <param name="api_highspeed"></param>
        public void UpdateOnNDockStart(IDictionary<string, string> queryParam, MemberData.Ship _memberShip)
        {
            string ship_id = queryParam["api_ship_id"];
            string highspeed = queryParam["api_highspeed"];
            var ship = _memberShip.GetShip(ship_id);

            if (highspeed == "1")
            {
                Debug.WriteLine("Using FastRepair");
                FastRepair--;
            }

            //修理に要した資材を引く
            Fuel -= ship.RepairFuel;
            Steel -= ship.RepairSteel;
        }

        /// <summary>
        /// /kcsapi/api_req_nyukyo/speedchange
        /// </summary>
        public void UseFastRepair()
        {
            FastRepair--;
        }
    }
}
