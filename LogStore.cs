using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Collections;
using System.Net;
using System.Diagnostics;
using System.Collections.Specialized;
//using Codeplex.Data;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace KCB2.LogStore
{
    /// <summary>
    /// 何もしないログストア
    /// </summary>
    public class LogStore
    {
        public virtual string StoreType { get { return "Null"; } }

        public delegate void MaterialDataLoadPostProcess(LogData.MaterialChangeInfo newData);
        public delegate void LogDownloadCompleted(bool bSucceeded);
        public delegate void LogUploadCompleted(bool bSucceeded);

        #region 読み込み
        public virtual bool LoadCreateShipInfo(string MemberID,IList<LogData.CreateShipInfo> createShipLog,
            LogDownloadCompleted finishCallback) { finishCallback(true);  return true; }
        public virtual bool LoadCreateItemInfo(string MemberID, IList<LogData.CreateItemInfo> createItemLog,
            LogDownloadCompleted finishCallback) { finishCallback(true); return true; }
        public virtual bool LoadBattleResult(string MemberID, IList<LogData.BattleResultInfo> battleLog,
            LogDownloadCompleted finishCallback) { finishCallback(true); return true; }
        public virtual bool LoadMissionResult(string MemberID, IList<LogData.MissionResultInfo> missionLog,
            LogDownloadCompleted finishCallback) { finishCallback(true); return true; }
        public virtual bool LoadMaterialChange(string MemberID, IList<LogData.MaterialChangeInfo> materialsLog,
            MaterialDataLoadPostProcess postCallback, LogDownloadCompleted finishCallback)
            { finishCallback(true); return true; }
        #endregion

        #region 更新
        public virtual bool AddCreateShipInfo(LogData.CreateShipInfo info, LogUploadCompleted finishCallback)
            { finishCallback(true); return true; }
        public virtual bool AddCreateItemInfo(LogData.CreateItemInfo info, LogUploadCompleted finishCallback)
            { finishCallback(true); return true; }
        public virtual bool AddBattleResult(LogData.BattleResultInfo info, LogUploadCompleted finishCallback)
            { finishCallback(true); return true; }
        public virtual bool AddMissionResult(LogData.MissionResultInfo info, LogUploadCompleted finishCallback)
            { finishCallback(true); return true; }
        public virtual bool AddMaterialChange(LogData.MaterialChangeInfo info, LogUploadCompleted finishCallback)
            { finishCallback(true); return true; }
        #endregion

        #region 保存
        public virtual bool SaveCreateShipInfo(IList<LogData.CreateShipInfo> createShipLog) { return false; }
        public virtual bool SaveCreateItemInfo(IList<LogData.CreateItemInfo> createItemLog) { return false; }
        public virtual bool SaveBattleResult(IList<LogData.BattleResultInfo> battleLog) { return false; }
        public virtual bool SaveMissionResult(IList<LogData.MissionResultInfo> missionLog) { return false; }
        public virtual bool SaveMaterialChange(IList<LogData.MaterialChangeInfo> materialsLog) { return false; }
        #endregion
    }

    /// <summary>
    /// CSVに保存するログストア
    /// </summary>
    public class CSVLogStore : LogStore
    {
        public override string StoreType { get { return "CSV"; } }

        const string createship_filename = "create_ship.csv";
        const string createitem_filename = "create_item.csv";
        const string battlelog_filename = "battle.csv";
        const string mission_filename = "mission.csv";
        const string material_filename = "materials.csv";

        /// <summary>
        /// 読みだすファイルのフルパスを取得
        /// </summary>
        /// <param name="memberId">member id</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns></returns>
        string GetFilePath(string memberId,string fileName)
        {
            string logRootDir = Properties.Settings.Default.LogStoreDir;
            if(logRootDir.Length == 0)
                logRootDir = "./";

            string logDir = Path.Combine(logRootDir, memberId);
            string fullPath = Path.Combine(logDir, fileName);
            if (!File.Exists(logDir))
            {
                try
                {
                    Directory.CreateDirectory(logDir);
                }
                catch (Exception ex)
                {
                    string err = string.Format("ログディレクトリの作成に失敗しました\nDir:{0}\nException:{1}",
                        logDir,ex.ToString());
                    Debug.WriteLine(err);
                    System.Windows.Forms.MessageBox.Show(err);

                    return null;
                }
            }
            return fullPath;
        }

        #region 読み込み

        public override bool LoadCreateShipInfo(string MemberID, IList<LogData.CreateShipInfo> createShipLog,
            LogDownloadCompleted finishCallback)
        {
            string file = GetFilePath(MemberID,createship_filename);
            if (file == null)
            {
                finishCallback(false);
                return false;
            }

            if (!File.Exists(file))
            {
                finishCallback(false);
                return false;
            }

            using (TextFieldParser parser = new TextFieldParser(file,
                System.Text.Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                lock (((ICollection)createShipLog).SyncRoot)
                {
                    while (!parser.EndOfData)
                    {
                        string[] row = parser.ReadFields();
                        var info = new LogData.CreateShipInfo(row);
                        info.Number = createShipLog.Count + 1;
                        info.MemberID = MemberID;
                        createShipLog.Add(info);
                    }
                }

            }
            finishCallback(true);
            return true;
        }
        public override bool LoadCreateItemInfo(string MemberID, IList<LogData.CreateItemInfo> createItemLog,
            LogDownloadCompleted finishCallback)
        {
            string file = GetFilePath(MemberID, createitem_filename);
            if (file == null)
            {
                finishCallback(false);
                return false;
            }

            if (!File.Exists(file))
            {
                finishCallback(false);
                return false;
            }

            using (TextFieldParser parser = new TextFieldParser(file,
                System.Text.Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                lock (((ICollection)createItemLog).SyncRoot)
                {
                    while (!parser.EndOfData)
                    {
                        string[] row = parser.ReadFields();
                        var info = new LogData.CreateItemInfo(row);
                        info.Number = createItemLog.Count + 1;
                        info.MemberID = MemberID;
                        createItemLog.Add(info);
                    }
                }
                finishCallback(true);
                return true;
            }
        }
        public override bool LoadBattleResult(string MemberID, IList<LogData.BattleResultInfo> battleLog,
            LogDownloadCompleted finishCallback)
        {
            string file = GetFilePath(MemberID, battlelog_filename);
            if (file == null)
            {
                finishCallback(false);
                return false;
            }

            if (!File.Exists(file))
            {
                finishCallback(false);
                return false;
            }

            using (TextFieldParser parser = new TextFieldParser(file,
                System.Text.Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                lock (((ICollection)battleLog).SyncRoot)
                {
                    while (!parser.EndOfData)
                    {
                        string[] row = parser.ReadFields();
                        var info = new LogData.BattleResultInfo(row);
                        info.Number = battleLog.Count + 1;
                        info.MemberID = MemberID;
                        battleLog.Add(info);
                    }
                }
            }

            finishCallback(true);
            return true;
        }
        public override bool LoadMissionResult(string MemberID, IList<LogData.MissionResultInfo> missionLog,
            LogDownloadCompleted finishCallback)
        {
            string file = GetFilePath(MemberID, mission_filename);
            if (file == null)
            {
                finishCallback(false);
                return false;
            }

            if (!File.Exists(file))
            {
                finishCallback(false);
                return false;
            }

            using (TextFieldParser parser = new TextFieldParser(file,
                System.Text.Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                lock (((ICollection)missionLog).SyncRoot)
                {
                    while (!parser.EndOfData)
                    {
                        string[] row = parser.ReadFields();
                        var info = new LogData.MissionResultInfo(row);
                        info.Number = missionLog.Count + 1;
                        info.MemberID = MemberID;
                        missionLog.Add(info);
                    }
                }
            }
            finishCallback(true);
            return true;
        }
        public override bool LoadMaterialChange(string MemberID, IList<LogData.MaterialChangeInfo> materialsLog,
            MaterialDataLoadPostProcess postCallback, LogDownloadCompleted finishCallback)
        {
            string file = GetFilePath(MemberID, material_filename);
            if (file == null)
            {
                finishCallback(false);
                return false;
            }

            if (!File.Exists(file))
            {
                finishCallback(false);
                return false;
            }

            using (TextFieldParser parser = new TextFieldParser(file,
                System.Text.Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                lock (((ICollection)materialsLog).SyncRoot)
                {
                    while (!parser.EndOfData)
                    {
                        string[] row = parser.ReadFields();
                        var info = new LogData.MaterialChangeInfo(row);
                        if (materialsLog.Count > 0)
                            info.PrevItem = materialsLog.Last();
                        info.Number = materialsLog.Count + 1;
                        info.MemberID = MemberID;
                        materialsLog.Add(info);
                        postCallback(info);
                    }
                }
            }
            finishCallback(true);
            return true;
        }

#endregion

        #region 書き込み
        public override bool SaveCreateShipInfo(IList<LogData.CreateShipInfo> createShipLog)
        {
            if (createShipLog.Count == 0)
                return false;
            string file = GetFilePath(createShipLog[0].MemberID,createship_filename);
            if (file == null)
                return false;

            using (var sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                lock (((ICollection)createShipLog).SyncRoot)
                {
                    foreach (var it in createShipLog)
                    {
                        sw.WriteLine(it.CSV);
                    }
                }
            }
            return true;
        }
        public override bool SaveCreateItemInfo(IList<LogData.CreateItemInfo> createItemLog)
        {
            if (createItemLog.Count == 0)
                return false;
            string file = GetFilePath(createItemLog[0].MemberID, createitem_filename);
            if (file == null)
                return false;

            using (var sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                lock (((ICollection)createItemLog).SyncRoot)
                {
                    foreach (var it in createItemLog)
                    {
                        sw.WriteLine(it.CSV);
                    }
                }
            }
            return true;
        }
        public override bool SaveBattleResult(IList<LogData.BattleResultInfo> battleLog)
        {
            if (battleLog.Count == 0)
                return false;
            string file = GetFilePath(battleLog[0].MemberID, battlelog_filename);
            if (file == null)
                return false;

            using (var sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                foreach (var it in battleLog)
                {
                    sw.WriteLine(it.CSV);
                }
            }
            return true;
        }
        public override bool SaveMissionResult(IList<LogData.MissionResultInfo> missionLog)
        {
            if (missionLog.Count == 0)
                return false;
            string file = GetFilePath(missionLog[0].MemberID, mission_filename);
            if (file == null)
                return false;

            using (var sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                lock (((ICollection)missionLog).SyncRoot)
                {
                    foreach (var it in missionLog)
                    {
                        sw.WriteLine(it.CSV);
                    }
                }
            }
            return true;
        }
        public override bool SaveMaterialChange(IList<LogData.MaterialChangeInfo> materialsLog)
        {
            if (materialsLog.Count == 0)
                return false;
            string file = GetFilePath(materialsLog[0].MemberID, material_filename);
            if (file == null)

                return false; using (var sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                lock (((ICollection)materialsLog).SyncRoot)
                {
                    foreach (var it in materialsLog)
                    {
                        sw.WriteLine(it.CSV);
                    }
                }
            }
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Google Spreadsheetに保存するログストア
    /// </summary>
    public class GSpreadLogStore : LogStore
    {
        public override string StoreType { get { return "GoogleSpreadsheet"; } }

        static GSpread.Job.SpreadsheetJobQueue _jobq = new GSpread.Job.SpreadsheetJobQueue();

        /// <summary>
        /// スプレッドシートアクセスAPIラッパ
        /// </summary>
        GSpread.SpreadSheetWrapper _wrapper;

        public GSpreadLogStore(GSpread.SpreadSheetWrapper _wrapper)
        {
            this._wrapper = _wrapper;
        }

        #region 読み出し
        public override bool LoadCreateShipInfo(string MemberID, IList<LogData.CreateShipInfo> createShipLog,
            LogDownloadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(MemberID,_wrapper));
            _jobq.Add(new GSpread.Job.LoadCreateShipEntry(MemberID, _wrapper, createShipLog, finishCallback));
            return true;
        }
        public override bool LoadCreateItemInfo(string MemberID, IList<LogData.CreateItemInfo> createItemLog,
            LogDownloadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.LoadCreateItemEntry(MemberID, _wrapper, createItemLog, finishCallback));
            return true;
        }
        public override bool LoadBattleResult(string MemberID, IList<LogData.BattleResultInfo> battleLog,
            LogDownloadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.LoadBattleResultEntry(MemberID, _wrapper, battleLog, finishCallback));
            return true;
        }
        public override bool LoadMissionResult(string MemberID, IList<LogData.MissionResultInfo> missionLog,
            LogDownloadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.LoadMissionResultEntry(MemberID, _wrapper, missionLog, finishCallback));
            return true;
        }

        public override bool LoadMaterialChange(string MemberID, IList<LogData.MaterialChangeInfo> materialsLog,
            MaterialDataLoadPostProcess postCallback, LogDownloadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.LoadMaterialChangeEntry(MemberID, _wrapper, materialsLog, postCallback,
                finishCallback));
            return true;
        }
        #endregion


        #region 書き込み
        public override bool AddCreateShipInfo(LogData.CreateShipInfo info, LogUploadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(info.MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.AddCreateShipEntry(_wrapper, info, finishCallback));
            return true;
        }
        public override bool AddCreateItemInfo(LogData.CreateItemInfo info, LogUploadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(info.MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.AddCreateItemEntry(_wrapper, info, finishCallback));
            return true;
        }
        public override bool AddBattleResult(LogData.BattleResultInfo info, LogUploadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(info.MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.AddBattleResultEntry(_wrapper, info, finishCallback));
            return true;
        }
        public override bool AddMissionResult(LogData.MissionResultInfo info, LogUploadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(info.MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.AddMissionResultEntry(_wrapper, info, finishCallback));
            return true;
        }
        public override bool AddMaterialChange(LogData.MaterialChangeInfo info, LogUploadCompleted finishCallback)
        {
            _jobq.Add(new GSpread.Job.CreateIfNotExistSheet(info.MemberID, _wrapper));
            _jobq.Add(new GSpread.Job.AddMaterialChangeEntry(_wrapper, info, finishCallback));
            return true;
        }

        #endregion
    }
}
