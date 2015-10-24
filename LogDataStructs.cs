using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Codeplex.Data;

namespace KCB2.LogData
{
    /// <summary>
    /// ログ
    /// </summary>
    public interface ILogItem : Log.ICSVLog, IVirtualListViewItem, Log.ILogExporter , Log.IGSpreadLog
    { }
    static class LogUtil
    {
        /// <summary>
        /// チェックサムを生成
        /// </summary>
        /// <param name="key">共有キー</param>
        /// <param name="values">値</param>
        /// <returns>MD5チェックサム</returns>
        public static string GenerateCheckSum(string key, params string[] values)
        {
            StringBuilder sb = new StringBuilder(key);
            System.Diagnostics.Debug.WriteLine("GenerateChecksum key:" + key);
            foreach (string val in values)
            {
                System.Diagnostics.Debug.WriteLine("AppendValue:" + val);
                sb.Append(val);
            }
            System.Security.Cryptography.MD5 ctx = System.Security.Cryptography.MD5.Create();
            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] sum = ctx.ComputeHash(data);
            ctx.Clear();

            string sum16 = BitConverter.ToString(sum).ToLower().Replace("-", "");
            System.Diagnostics.Debug.WriteLine("Checksum:" + sum16);
            return sum16;
        }

    }

    /// <summary>
    /// 資源
    /// </summary>
    public class Material
    {
        /// <summary>
        /// リクエストクエリから作成
        /// </summary>
        /// <param name="param">リクエストクエリ</param>
        public Material(IDictionary<string, string> param)
        {
            Fuel = int.Parse(param["api_item1"]);
            Ammo = int.Parse(param["api_item2"]);
            Steel = int.Parse(param["api_item3"]);
            Bauxite = int.Parse(param["api_item4"]);
            if (param.ContainsKey("api_item5"))
                Dev = int.Parse(param["api_item5"]);
            else
                Dev = 1;

        }

        /// <summary>
        /// 文字列から生成
        /// </summary>
        /// <param name="Fuel_"></param>
        /// <param name="Ammo_"></param>
        /// <param name="Steel_"></param>
        /// <param name="Bauxite_"></param>
        public Material(string Fuel_, string Ammo_, string Steel_, string Bauxite_)
        {
            Fuel = int.Parse(Fuel_);
            Ammo = int.Parse(Ammo_);
            Steel = int.Parse(Steel_);
            Bauxite = int.Parse(Bauxite_);
            Dev = 1;
        }

        public Material(string Fuel_, string Ammo_, string Steel_, string Bauxite_,string Devs_)
        {
            Fuel = int.Parse(Fuel_);
            Ammo = int.Parse(Ammo_);
            Steel = int.Parse(Steel_);
            Bauxite = int.Parse(Bauxite_);
            Dev = int.Parse(Devs_);
        }

        public Material(MemberData.Dock.KDock dockInfo)
        {
            Fuel = dockInfo.Fuel;
            Ammo = dockInfo.Ammo;
            Steel = dockInfo.Steel;
            Bauxite = dockInfo.Bauxite;
            Dev = dockInfo.Dev;
        }

        public Material(Newtonsoft.Json.Linq.JArray param)
        {
            Fuel = (int)param[0];
            Ammo = (int)param[1];
            Steel = (int)param[2];
            Bauxite = (int)param[3];
        }

        /// <summary>
        /// 値から作成
        /// </summary>
        /// <param name="param"></param>
        public Material(double[] param)
        {
            Fuel = (int)param[0];
            Ammo = (int)param[1];
            Steel = (int)param[2];
            Bauxite = (int)param[3];
            if (param.Length > 4)
                Dev = (int)param[4];
            else
                Dev = 1;

        }

        public Material(int[] param)
        {
            Fuel = param[0];
            Ammo = param[1];
            Steel = param[2];
            Bauxite = param[3];
            if (param.Length > 4)
                Dev = param[4];
            else
                Dev = 1;
        }

        public Material(string[] param)
        {
            Fuel = int.Parse(param[0]);
            Ammo = int.Parse(param[1]);
            Steel = int.Parse(param[2]);
            Bauxite = int.Parse(param[3]);
            if (param.Length > 4)
                Dev = int.Parse(param[4]);
            else
                Dev = 1;
        }

        public Material()
        {
            Fuel = 0;
            Ammo = 0;
            Steel = 0;
            Bauxite = 0;
            Dev = 0;
        }

        public override string ToString()
        {
            return string.Format("Fuel:{0} Ammo:{1} Steel:{2} Bauxite:{3} Dev:{4}", Fuel, Ammo, Steel, Bauxite,Dev);
        }

        public string CSV
        {
            get
            {
                return string.Format("{0},{1},{2},{3},{4}", Fuel, Ammo, Steel, Bauxite,Dev);
            }
        }

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
        /// 開発資材
        /// </summary>
        public int Dev { get; private set; }
        /// <summary>
        /// 改修資材
        /// </summary>
        public int Updater { get; private set; }
    }

    /// <summary>
    /// 艦娘作成
    /// </summary>
    public class CreateShipInfo : ILogItem
    {
        public string MemberID { get; set; }

        /// <summary>
        /// 資源
        /// </summary>
        public Material Resource;

        /// <summary>
        /// 建造艦名
        /// </summary>
        public string ShipName = "";
        /// <summary>
        /// 建造艦種別
        /// </summary>
        public string ShipType;

        /// <summary>
        /// 司令部レベル
        /// </summary>
        public int OfficeLv;
        /// <summary>
        /// 秘書艦
        /// </summary>
        public string SecretaryShip { get; set; }

        /// <summary>
        /// 建造時間
        /// </summary>
        public DateTime Date;
        /// <summary>
        /// リスト序列
        /// </summary>
        public int Number { get; set; }

        public override string ToString()
        {
            return string.Format("CreateShip({6}) Rc:{0},{1},{2},{3} -> {4} ({5})",
                Resource.Fuel, Resource.Ammo, Resource.Steel, Resource.Bauxite,
                ShipName,SecretaryShip,Date);
        }

        public class LVItem : System.Windows.Forms.ListViewItem
        {
            public CreateShipInfo Info { get; set; }
        }

        public System.Windows.Forms.ListViewItem CreateVirtualListViewItem()
        {
            LVItem lvit = new LVItem();
            lvit.Text = Number.ToString();
            lvit.SubItems.Add(Date.ToString());
            lvit.SubItems.Add(ShipName);
            lvit.SubItems.Add(ShipType);
            lvit.SubItems.Add(Resource.Fuel.ToString());
            lvit.SubItems.Add(Resource.Ammo.ToString());
            lvit.SubItems.Add(Resource.Steel.ToString());
            lvit.SubItems.Add(Resource.Bauxite.ToString());
            lvit.SubItems.Add(Resource.Dev.ToString());
            lvit.SubItems.Add(SecretaryShip);
            lvit.SubItems.Add(OfficeLv.ToString());
            lvit.Info = this;

            return lvit;
        }
        

        public string CSV
        {
            get
            {
                return string.Format("\"{0}\",\"{1}\",\"{2}\",{3},{4},{5},{6},{7},\"{8}\",{9}",
                    Date.ToString(), ShipName, ShipType, Resource.Fuel, Resource.Ammo,
                        Resource.Steel, Resource.Bauxite, Resource.Dev, SecretaryShip, OfficeLv);
            }
        }

        public string ExportLogEntry(KCB2.LogManager.ExportLogDataType exportType)
        {
            if (exportType == LogManager.ExportLogDataType.CSV)
                return CSV;

            throw new NotSupportedException("Unknown log export type:" + exportType.ToString());
        }

        public CreateShipInfo(string[] row)
        {
            Date = DateTime.Parse(row[0]);
            ShipName = row[1];
            ShipType = row[2];
            Resource = new Material(row[3], row[4], row[5], row[6] , row[7]);
            SecretaryShip = row[8];
            OfficeLv = int.Parse(row[9]);
        }

        public CreateShipInfo()
        { Number = 0; }

        public string LogClass  { get { return "ship";  }  }

        public IDictionary<string, string> KeyValue
        {
            get
            {
                Dictionary<string, string> ret = new Dictionary<string, string>();
                ret.Add("date", Date.ToString());
                ret.Add("name", ShipName);
                ret.Add("type", ShipType);
                ret.Add("fuel", Resource.Fuel.ToString());
                ret.Add("ammo", Resource.Ammo.ToString());
                ret.Add("steel", Resource.Steel.ToString());
                ret.Add("bauxite", Resource.Bauxite.ToString());
                ret.Add("develop", Resource.Dev.ToString());
                ret.Add("secretary", SecretaryShip);
                ret.Add("level", OfficeLv.ToString());

                return ret;
            }
        }

        public CreateShipInfo(IDictionary<string, string> row)
        {
            Date = DateTime.Parse(row["date"]);
            ShipName = row["name"];
            ShipType = row["type"];
            Resource = new LogData.Material(row["fuel"], row["ammo"], row["steel"],
                row["bauxite"], row["develop"]);
            OfficeLv = int.Parse(row["level"]);
            SecretaryShip = row["secretary"];
        }

        public static string[] SpreadsheetColumnTitle()
        {
            return new string[]{"date", "name", "type", "fuel", "ammo", "steel", "bauxite", "develop",
                "secretary", "level"};
        }
    }

    /// <summary>
    /// アイテム作成
    /// </summary>
    public class CreateItemInfo : ILogItem
    {
        public string MemberID { get; set; }

        public Material Resource;
        public int OfficeLv;
        public int ItemNameID { get; private set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public bool Succeess { get; private set; }
        public string SecretaryShip { get; set; }
        public DateTime Date { get; set; }
        public int Number { get; set; }

        /// <summary>
        /// レスポンスから
        /// </summary>
        /// <param name="req">リクエストパラメタ</param>
        /// <param name="json">レスポンス情報</param>
        public CreateItemInfo(IDictionary<string, string> req, KCB.api_req_kousyou.CreateItem.ApiData json)
//        public CreateItemInfo(IDictionary<string, string> req, dynamic json)
        {
            Succeess = false;
            Number = 0;
            ItemNameID = 0;
            ItemType = "";
            ItemName = "(失敗)";
            Date = DateTime.Now;
            Resource = new Material(req);
            if ((int)json.api_create_flag == 0)
                return;

            Succeess = true;
            ItemNameID = (int)json.api_slot_item.api_slotitem_id;
        }

        public override string ToString()
        {
            if (Succeess)
                return string.Format("CreateItem(Success:{7}): Rc:{0},{1},{2},{3} -> {4} by:{5} Lv:{6}",
                    Resource.Fuel, Resource.Ammo, Resource.Steel, Resource.Bauxite,
                    ItemName, SecretaryShip, OfficeLv,Date);
            else
                return string.Format("CreateItem(Failure:{6}): Rc:{0},{1},{2},{3} by:{4} Lv:{5}",
                    Resource.Fuel, Resource.Ammo, Resource.Steel, Resource.Bauxite,
                    SecretaryShip, OfficeLv,Date);
        }

        public class LVItem : System.Windows.Forms.ListViewItem
        {
            public CreateItemInfo Info { get; set; }
        }

        public System.Windows.Forms.ListViewItem CreateVirtualListViewItem()
        {
            LVItem lvit = new LVItem();
            lvit.Text = Number.ToString();
            lvit.SubItems.Add(Date.ToString());
            lvit.SubItems.Add(ItemName);
            lvit.SubItems.Add(ItemType);
            lvit.SubItems.Add(Resource.Fuel.ToString());
            lvit.SubItems.Add(Resource.Ammo.ToString());
            lvit.SubItems.Add(Resource.Steel.ToString());
            lvit.SubItems.Add(Resource.Bauxite.ToString());
            lvit.SubItems.Add(SecretaryShip);
            lvit.SubItems.Add(OfficeLv.ToString());
            lvit.Info = this;

            return lvit;
        }

        public string CSV
        {
            get
            {
                return string.Format("\"{0}\",\"{1}\",\"{2}\",{3},{4},{5},{6},\"{7}\",{8}",
                    Date.ToString(), ItemName, ItemType, Resource.Fuel, Resource.Ammo,
                        Resource.Steel, Resource.Bauxite, SecretaryShip, OfficeLv);
            }
        }

        public string ExportLogEntry(KCB2.LogManager.ExportLogDataType exportType)
        {
            if (exportType == LogManager.ExportLogDataType.CSV)
                return CSV;

            throw new NotSupportedException("Unknown log export type:" + exportType.ToString());
        }

        public CreateItemInfo()
        {
            Number = 0;
        }

        public CreateItemInfo(string[] row)
        {
            Date = DateTime.Parse(row[0]);
            ItemName = row[1];
            ItemType = row[2];
            Resource = new Material(row[3], row[4], row[5], row[6]);
            SecretaryShip = row[7];
            OfficeLv = int.Parse(row[8]);
            Succeess = IsSuceeded(ItemName);
        }

        public string LogClass { get { return "item"; } }

        bool IsSuceeded(string itemName)
        {
            if (itemName == "(Failed)" || itemName == "(失敗)")
                return false;
            return true;
        }

        public IDictionary<string, string> KeyValue
        {
            get
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("date", Date.ToString());
                row.Add("name", ItemName);
                row.Add("type", ItemType);
                row.Add("fuel", Resource.Fuel.ToString());
                row.Add("ammo", Resource.Ammo.ToString());
                row.Add("steel", Resource.Steel.ToString());
                row.Add("bauxite", Resource.Bauxite.ToString());
                row.Add("secretary", SecretaryShip);
                row.Add("level", OfficeLv.ToString());
                return row;
            }
        }

        public CreateItemInfo(IDictionary<string, string> row)
        {
            Date = DateTime.Parse(row["date"]);
            ItemName = row["name"];
            ItemType = row["type"];
            Resource = new LogData.Material(row["fuel"], row["ammo"], row["steel"],
                row["bauxite"]);
            OfficeLv = int.Parse(row["level"]);
            SecretaryShip = row["secretary"];
        }

        public static string[] SpreadsheetColumnTitle()
        {
            return new string[] {"date", "name", "type", "fuel", "ammo", "steel", "bauxite",
                "secretary", "level" };
        }
        
    }

    /// <summary>
    /// 戦闘結果
    /// </summary>
    public class BattleResultInfo : ILogItem
    {
        public string MemberID { get; set; }

        /// <summary>
        /// 戦闘海域名
        /// </summary>
        public string AreaName { get; set; }

        public DateTime Date { get; set; }
        public int Number { get; set; }

        /// <summary>
        /// 勝敗ランク
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// 戦闘艦隊情報
        /// </summary>
        public class Fleet
        {
            public string[] ShipList = { "", "", "", "", "", "" };
            public string DeckName = "";

            /// <summary>
            /// 文字列フォーマッタ
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("({0}),", DeckName);
                foreach (var it in ShipList)
                {
                    if (it.Length != 0)
                        sb.AppendFormat("{0},", it);
                }

                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }

            /// <summary>
            /// CSV形式データ
            /// </summary>
            /// <returns></returns>
            public string ToCSVString()
            {
                StringBuilder sb = new StringBuilder(DeckName);
                sb.Append(",");
                foreach (var it in ShipList)
                {
                    if (it.Length != 0)
                        sb.AppendFormat("{0},", it);
                }

                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }

            public Fleet(){ }

            public Fleet(string line)
            {
                string[] param = line.Split(',');
                Init(param);
            }

            /// <summary>
            /// TextParserから
            /// </summary>
            /// <param name="param"></param>
            public Fleet(string[] param)
            {
                Init(param);
            }

            /// <summary>
            /// 初期化
            /// </summary>
            /// <param name="param">トークン分割されたパラメタ</param>
            void Init(string[] param)
            {
                DeckName = param[0];
                for (int i = 1; i < param.Count(); i++)
                    ShipList[i - 1] = param[i];
            }


        };

        /// <summary>
        /// 艦隊情報
        /// </summary>
        public Fleet Friend;
        public Fleet Foe;

        /// <summary>
        /// ドロップ
        /// </summary>
        public string ShipDropped = "";

        public override string ToString()
        {
            return string.Format("BattleResult({5}) Area:{0} Rank:{1} Fr:[{2}] Foe:[{3}] Drop:{4}",
                AreaName, Rank, Friend, Foe, ShipDropped,Date);

        }

        /// <summary>
        /// リストビューに追加するカスタムアイテム
        /// </summary>
        public class LVItem : System.Windows.Forms.ListViewItem
        {
            public BattleResultInfo Info { get; set; }
        }

        /// <summary>
        /// カスタマイズされたリストビューアイテムを生成
        /// </summary>
        /// <returns></returns>
        public System.Windows.Forms.ListViewItem CreateVirtualListViewItem()
        {
            LVItem lvit = new LVItem();
            lvit.Text = Number.ToString();
            lvit.SubItems.Add(Date.ToString());
            lvit.SubItems.Add(AreaName);
            lvit.SubItems.Add(Friend.ToString());
            lvit.SubItems.Add(Foe.ToString());
            lvit.SubItems.Add(Rank);
            lvit.SubItems.Add(ShipDropped);
            lvit.Info = this;

            return lvit;
        }

        /// <summary>
        /// CSV形式を取得
        /// </summary>
        public string CSV
        {
            get
            {
                return string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"",
                    Date.ToString(), AreaName, Friend.ToCSVString(), Foe.ToCSVString(), Rank,
                        ShipDropped);
            }
        }

        public string ExportLogEntry(KCB2.LogManager.ExportLogDataType exportType)
        {
            if (exportType == LogManager.ExportLogDataType.CSV)
                return CSV;

            throw new NotSupportedException("Unknown log export type:" + exportType.ToString());
        }

        public BattleResultInfo()
        {
            Friend = new Fleet();
            Foe = new Fleet();
        }

        /// <summary>
        /// トークン分割された文字列から生成するコンストラクタ
        /// </summary>
        /// <param name="row"></param>
        public BattleResultInfo(string[] row)
        {
            Date = DateTime.Parse(row[0]);
            AreaName = row[1];
            Friend = new Fleet(row[2]);
            Foe = new Fleet(row[3]);
            Rank = row[4];
            ShipDropped = row[5];
        }

        public string LogClass { get { return "battle"; } }

        public IDictionary<string, string> KeyValue
        {
            get
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("date", Date.ToString());
                row.Add("area", AreaName);
                row.Add("friend", Friend.ToCSVString());
                row.Add("foe", Foe.ToCSVString());
                row.Add("result", Rank);
                row.Add("drop", ShipDropped);
                return row;
            }
        }

        public BattleResultInfo(IDictionary<string, string> row)
        {
            Date = DateTime.Parse(row["date"]);
            AreaName = row["area"];
            Friend = new LogData.BattleResultInfo.Fleet(row["friend"]);
            Foe = new LogData.BattleResultInfo.Fleet(row["foe"]);
            Rank = row["result"];
            ShipDropped = row["drop"];
        }

        public static string[] SpreadsheetColumnTitle()
        {
            return new string[] { "date", "area", "friend", "foe", "result", "drop" };
        }
    }

    /// <summary>
    /// 遠征結果
    /// </summary>
    public class MissionResultInfo : ILogItem
    {
        public string MemberID { get; set; }

        public DateTime Date = DateTime.Now;
        public int Number = 0;
        public List<String> Member = new List<string>();
        public string AreaName = "";
        public string QuestName = "";
        public int Succeeded = 0;
        public Material Material;
        public List<string> Item = new List<string>();

        string SucceededMsg
        {
            get
            {
                switch (Succeeded)
                {
                    case 0:
                        return "☓";
                    case 1:
                        return "○";
                    case 2:
                        return "◎";
                    default:
                        return Succeeded.ToString();
                }
            }
        }

        public class LVItem : System.Windows.Forms.ListViewItem
        {
            public MissionResultInfo Info { get; set; }
        }

        public System.Windows.Forms.ListViewItem CreateVirtualListViewItem()
        {
            LVItem lvit = new LVItem();
            lvit.Text = Number.ToString();
            lvit.SubItems.Add(Date.ToString());
            lvit.SubItems.Add(AreaName);
            lvit.SubItems.Add(QuestName);
            lvit.SubItems.Add(string.Join(",", Member));
            lvit.SubItems.Add(SucceededMsg);
            lvit.SubItems.Add(Material.Fuel.ToString());
            lvit.SubItems.Add(Material.Ammo.ToString());
            lvit.SubItems.Add(Material.Steel.ToString());
            lvit.SubItems.Add(Material.Bauxite.ToString());
            lvit.SubItems.Add(string.Join(",", Item));
            lvit.Info = this;

            return lvit;
        }

        public override string ToString()
        {
            if (Material == null)
                return string.Format("{6} Num:{0} Member:{1} Area:{2} Quest:{3} Suceeded:{4} Item:{5}",
                    Number, string.Join(",", Member), AreaName, QuestName, Succeeded, string.Join(",", Item),Date);
            else
                return string.Format("{7} Num:{0} Member:{1} Area:{2} Quest:{3} Suceeded:{4} Material:{5} Item:{6}",
                    Number, string.Join(",", Member), AreaName, QuestName, Succeeded, Material, string.Join(",", Item),Date);

        }

        public string CSV
        {
            get
            {
                return string.Format("\"{0}\",\"{1}\",\"{2}\",{3},{4},{5},{6},{7},\"{8}\",\"{9}\"",
                    Date.ToString(), AreaName, QuestName, Material.Fuel, Material.Ammo,
                        Material.Steel, Material.Bauxite, Succeeded, string.Join(",", Member), string.Join(",", Item));
            }
        }

        public string ExportLogEntry(KCB2.LogManager.ExportLogDataType exportType)
        {
            if (exportType == LogManager.ExportLogDataType.CSV)
                return CSV;

            throw new NotSupportedException("Unknown log export type:" + exportType.ToString());
        }


        public MissionResultInfo()
        { Number = 0; }

        public MissionResultInfo(string[] row)
        {
            Number = 0;
            Date = DateTime.Parse(row[0]);
            AreaName = row[1];
            QuestName = row[2];
            Material = new Material(row[3], row[4], row[5], row[6]);
            Succeeded = int.Parse(row[7]);
            var m = row[8].Split(',');
            Member.AddRange(m);
            Item.AddRange(row[9].Split(','));
        }

        public string LogClass { get { return "mission"; } }

        public IDictionary<string, string> KeyValue
        {
            get
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("date", Date.ToString());
                row.Add("area", AreaName);
                row.Add("mission", QuestName);
                row.Add("fuel", Material.Fuel.ToString());
                row.Add("ammo", Material.Ammo.ToString());
                row.Add("steel", Material.Steel.ToString());
                row.Add("bauxite", Material.Bauxite.ToString());
                row.Add("result", Succeeded.ToString());
                row.Add("member", string.Join(",", Member));
                row.Add("reward", string.Join(",", Item));

                return row;
            }
        }

        public MissionResultInfo(IDictionary<string, string> row)
        {
            Date = DateTime.Parse(row["date"]);
            AreaName = row["area"];
            QuestName = row["mission"];
            Material = new Material(row["fuel"], row["ammo"], row["steel"], row["bauxite"]);
            Succeeded = int.Parse(row["result"]);
            var m = row["member"].Split(',');
            Member.AddRange(m);
            Item.AddRange(row["reward"].Split(','));
        }

        public static string[] SpreadsheetColumnTitle()
        {
            return new string[] { "date", "area", "mission", "fuel", "ammo", "steel", "bauxite",
                "result", "member", "reward"};
        }

    }

    /// <summary>
    /// 資源推移記録
    /// </summary>
    public class MaterialChangeInfo : ILogItem
    {
        public string MemberID { get; set; }


        /// <summary>
        /// 記録日時
        /// </summary>
        public DateTime Date = DateTime.Now;
        /// <summary>
        /// ID
        /// </summary>
        public int Number { get; set; }
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
        /// 開発資材
        /// </summary>
        public int FastBuild { get; private set; }
        /// <summary>
        /// 高速修復
        /// </summary>
        public int FastRepair { get; private set; }
        /// <summary>
        /// 開発
        /// </summary>
        public int Devel { get; private set; }

        /// <summary>
        /// 改修資材
        /// </summary>
        public int Updater { get; private set; }

        /// <summary>
        /// 変化算出用の一つ前のアイテム
        /// </summary>
        public MaterialChangeInfo PrevItem = null;

        public string LogClass { get { return "material"; } }

        public class LVItem : System.Windows.Forms.ListViewItem
        {
            public MaterialChangeInfo Info { get; set; }
        }

        public System.Windows.Forms.ListViewItem CreateVirtualListViewItem()
        {
            LVItem lvit = new LVItem();
            lvit.Text = Date.ToLongDateString();
            if (PrevItem == null)
            {
                lvit.SubItems.Add(Fuel.ToString());
                lvit.SubItems.Add(Ammo.ToString());
                lvit.SubItems.Add(Steel.ToString());
                lvit.SubItems.Add(Bauxite.ToString());
                lvit.SubItems.Add(FastRepair.ToString());
                lvit.SubItems.Add(FastBuild.ToString());
                lvit.SubItems.Add(Devel.ToString());
                lvit.SubItems.Add(Updater.ToString());
            }
            else
            {
                int diff = Fuel - PrevItem.Fuel;
                lvit.SubItems.Add(string.Format("{0}{1:(+#);(-#); }", Fuel, diff));

                diff = Ammo - PrevItem.Ammo;
                lvit.SubItems.Add(string.Format("{0}{1:(+#);(-#); }", Ammo, diff));

                diff = Steel - PrevItem.Steel;
                lvit.SubItems.Add(string.Format("{0}{1:(+#);(-#); }", Steel, diff));

                diff = Bauxite - PrevItem.Bauxite;
                lvit.SubItems.Add(string.Format("{0}{1:(+#);(-#); }", Bauxite, diff));

                diff = FastRepair - PrevItem.FastRepair;
                lvit.SubItems.Add(string.Format("{0}{1:(+#);(-#); }", FastRepair, diff));

                diff = FastBuild - PrevItem.FastBuild;
                lvit.SubItems.Add(string.Format("{0}{1:(+#);(-#); }", FastBuild, diff));

                diff = Devel - PrevItem.Devel;
                lvit.SubItems.Add(string.Format("{0}{1:(+#);(-#); }", Devel, diff));

                diff = Updater - PrevItem.Updater;
                lvit.SubItems.Add(string.Format("{0}{1:(+#);(-#); }", Updater, diff));
            }

 
            lvit.Info = this;

            return lvit;
        }

        public MaterialChangeInfo(string[] row)
        {
            Date = DateTime.Parse(row[0]);
            Fuel = int.Parse(row[1]);
            Ammo = int.Parse(row[2]);
            Steel = int.Parse(row[3]);
            Bauxite = int.Parse(row[4]);
            FastRepair = int.Parse(row[5]);
            FastBuild = int.Parse(row[6]);
            Devel = int.Parse(row[7]);
            if(row.Length > 8)
                Updater = int.Parse(row[8]);
        }

        /// <summary>
        /// API資源情報から情報をもらうコンストラクタ
        /// </summary>
        /// <param name="memberMaterial"></param>
        /// <param name="basicData"></param>
        public MaterialChangeInfo(MemberData.Material memberMaterial, MemberData.Basic basicData)
        {
            Fuel = memberMaterial.Fuel;
            Ammo = memberMaterial.Ammo;
            Steel = memberMaterial.Steel;
            Bauxite = memberMaterial.Bauxite;
            FastRepair = memberMaterial.FastRepair;
            FastBuild = memberMaterial.FastBuild;
            Devel = memberMaterial.Developer;
            Updater = memberMaterial.Updater;

            MemberID = basicData.MemberID;

        }

        /// <summary>
        /// KeyValue(Google Spreadsheet)からデータを貰うコンストラクタ
        /// </summary>
        /// <param name="row"></param>
        public MaterialChangeInfo(IDictionary<string, string> row)
        {
            Date = DateTime.Parse(row["date"]);
            Fuel = int.Parse(row["fuel"]);
            Ammo = int.Parse(row["ammo"]);
            Steel = int.Parse(row["steel"]);
            Bauxite = int.Parse(row["bauxite"]);
            FastRepair = int.Parse(row["fastrepair"]);
            FastBuild = int.Parse(row["fastdevel"]);
            Devel = int.Parse(row["develop"]);

            //後から追加したので空白の場合がありえる。
            if (row.ContainsKey("updater"))
                Updater = TryParse(row["updater"]);
        }


        public override string ToString()
        {
            return string.Format("{8} Number:{0} Fuel:{1} Ammo:{2} Steel:{3} Bauxite:{4} FRepair:{5} FBuild:{6} Dev:{7} Upd:{9}",
                Number, Fuel, Ammo,Steel, Bauxite
                     ,FastRepair,FastBuild,Devel,Date,Updater);
        }

        public string CSV
        {
            get
            {
                return string.Format("\"{0}\",{1},{2},{3},{4},{5},{6},{7},{8}",
                     Date.ToString(), Fuel, Ammo,Steel, Bauxite
                     ,FastRepair,FastBuild,Devel,Updater);
            }
        }

        public string ExportLogEntry(KCB2.LogManager.ExportLogDataType exportType)
        {
            if (exportType == LogManager.ExportLogDataType.CSV)
                return CSV;

            throw new NotSupportedException("Unknown log export type:" + exportType.ToString());
        }


        public System.Data.DataRow SetRowData(System.Data.DataRow newRow)
        {
            newRow[0] = Date;
            newRow[1] = Fuel;
            newRow[2] = Ammo;
            newRow[3] = Steel;
            newRow[4] = Bauxite;

            return newRow;
        }

        public IDictionary<string, string> KeyValue
        {
            get
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                row.Add("date",Date.ToString());
                row.Add("fuel", Fuel.ToString());
                row.Add("ammo",Ammo.ToString());
                row.Add("steel",Steel.ToString());
                row.Add("bauxite",Bauxite.ToString());
                row.Add("fastrepair",FastRepair.ToString());
                row.Add("fastdevel",FastBuild.ToString());
                row.Add("develop", Devel.ToString());
                row.Add("updater", Updater.ToString());
                return row;
            }
        }

        int TryParse(string str)
        {
            int result;
            if (int.TryParse(str, out result))
                return result;

            return 0;
        }

        public static string[] SpreadsheetColumnTitle()
        {
            return new string[] {"date", "fuel", "ammo", "steel", "bauxite", "fastrepair",
                "fastdevel", "develop","updater" };
        }
    }

}
