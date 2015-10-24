using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Google.GData.Spreadsheets;

namespace KCB2.GSpread.Job
{
    /// <summary>
    /// ジョブインタフェイス
    /// </summary>
    public interface ISpreadsheetJob
    {
        /// <summary>
        /// ジョブ
        /// </summary>
        /// <returns></returns>
        bool Process();
    }

    /// <summary>
    /// SpreadSheetジョブキューの実装
    /// </summary>
    public class SpreadsheetJobQueue : KCB.JobQueue<ISpreadsheetJob>
    {
        override protected void processJob(ISpreadsheetJob oJob)
        {
            try
            {
                oJob.Process();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Thrown ex:" + ex.ToString());
                return;
            }
        }
    }

    /// <summary>
    /// ジョブの共通関数群とかを実装
    /// </summary>
    public class SpreadSheetJobHelper
    {
        protected SpreadSheetWrapper _wrapper;
        protected String _memberId;

        /// <summary>
        /// スプレッドシート名を取得
        /// </summary>
        protected String SpreadsheetName {
            get { return string.Format("KCB記録({0})", _memberId); }
        }

        /// <summary>
        /// 指定したワークシートを取得する
        /// </summary>
        /// <param name="workSheetName">ワークシート名</param>
        /// <returns>ワークシートエントリ</returns>
        protected WorksheetEntry GetWorksheet(string workSheetName)
        {
            Debug.WriteLine("GetSpreadsheet:" + SpreadsheetName);
            WorksheetFeed spread = _wrapper.GetSpreadsheet(SpreadsheetName);
            if (spread == null)
                return null;

            Debug.WriteLine("GetWorksheet:" + workSheetName);
            foreach (WorksheetEntry it in spread.Entries)
            {
                if (it.Title.Text == workSheetName)
                {
                    Debug.WriteLine("Worksheet found");
                    return it;
                }

            }

            return null;
        }

        /// <summary>
        /// リストを取得する
        /// </summary>
        /// <param name="wsEntry">取得する対象フィード</param>
        /// <returns>取得したリストエントリ一覧</returns>
        protected IEnumerable<IDictionary<string,string>> Query(WorksheetEntry wsEntry)
        {
            Debug.WriteLine("ListQuery start");
            ListFeed listFeed = _wrapper.Query(wsEntry);
            List<IDictionary<string, string>> retList = new List<IDictionary<string, string>>();

            foreach (ListEntry row in listFeed.Entries)
            {
//                Debug.WriteLine("ListEntry:" + row.Title.Text);
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (ListEntry.Custom element in row.Elements)
                {
//                    Debug.WriteLine(string.Format("AddIt {0}->{1}", element.LocalName, element.Value));
                    dic[element.LocalName] = element.Value;
                }
                retList.Add(dic);
            }
            return retList;
        }

        /// <summary>
        /// カラム数が変わっていたら表を拡張してカラム名を更新
        /// </summary>
        /// <param name="wsEntry"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        protected WorksheetEntry CheckColumns(WorksheetEntry wsEntry, string[] columns)
        {
            if (columns.Length > wsEntry.Cols)
            {
                wsEntry.Cols = (uint)columns.Length;

                WorksheetEntry newEntry = (WorksheetEntry)wsEntry.Update();

                /*
                 * http://stackoverflow.com/questions/22719170/updating-cell-in-google-spreadsheets-returns-error-missing-resource-version-id
                 * http://stackoverflow.com/questions/20841411/google-spreadsheets-api-c-sharp-missing-resource-version-id-on-batch-update/23438381
                 * 
                 * ETag:*をするとか、そもそもBatchよりPublishの方が分かりやすい、とか。
                 */
                CellFeed cellFeed = wsEntry.QueryCellFeed();

                uint col = 1;
                foreach(var it in columns)
                {
                    CellEntry batchEntry = cellFeed[1, col++];
                    batchEntry.InputValue = it;
                    batchEntry.Etag = "*";
                }
                cellFeed.Publish();

                return newEntry;
            }
            return wsEntry;
        }

   }

    /// <summary>
    /// 拡張メソッド
    /// </summary>
    static class MethodExtensions
    {
        /// <summary>
        /// リストにエントリを追加する
        /// </summary>
        /// <param name="name">名前(大文字小文字は同一視されるが、小文字でないとAPIが400エラーで例外投げる)</param>
        /// <param name="value">値</param>
        public static void Add(this ListEntry listRow, string name, string value)
        {
            Debug.WriteLine(string.Format("AddListRow({0},{1})", name, value));

            listRow.Elements.Add(new ListEntry.Custom() { LocalName = name, Value = value });
        }

        /// <summary>
        /// リストにエントリを追加する
        /// </summary>
        /// <param name="name">名前(大文字小文字は同一視されるが、小文字でないとAPIが400エラーで例外投げる)</param>
        /// <param name="value">値</param>
        public static void Add(this ListEntry listRow, string name, int value)
        {
            Add(listRow, name, value.ToString());
        }

        public static void Add(this ListEntry listRow, KeyValuePair<string, string> kv)
        {
            Add(listRow,kv.Key, kv.Value);
        }

        /// <summary>
        /// ワークシートを追加する
        /// </summary>
        /// <param name="name">ワークシート名</param>
        /// <param name="columns">追加するカラム数</param>
        /// <returns>作成されたワークシート</returns>
        public static WorksheetEntry CreateWorksheet(this WorksheetFeed feed,
            string name, int columns)
        {
            //現状、どっちでも動く
            WorksheetEntry worksheet = (WorksheetEntry)feed.CreateFeedEntry();
            //            WorksheetEntry worksheet = new WorksheetEntry();
            worksheet.Title.Text = name;

            ///columnsは規定数ないとCellFeed.Insertで死ぬ
            worksheet.Cols = (uint)columns;

            //rowsは勝手にListFeed.Insertで増えてく
            worksheet.Rows = 1;

            //追加する。追加された結果Entryを使って次の処理を行う
            return feed.Insert(worksheet);
        }

    }

    /// <summary>
    /// ログ保存用スプレッドシートがなければ作成するジョブ
    /// </summary>
    public class CreateIfNotExistSheet :SpreadSheetJobHelper,ISpreadsheetJob
    {
        public CreateIfNotExistSheet(string memberId, SpreadSheetWrapper wrapper)
        {
            _wrapper = wrapper;
            _memberId = memberId;
        }

        public bool Process()
        {
            Debug.WriteLine("Find Created Sheet");
            var feed = _wrapper.GetSpreadsheet(SpreadsheetName);

            if (feed != null)
            {
                Debug.WriteLine("Spreadsheet found,return");
                return false;
            }


            Debug.WriteLine("Begin CreateSheet");
            //spreadsheetを生成
            if (!_wrapper.CreateSpreadsheet(SpreadsheetName))
                return false;

            //生成したSpreadsheetを取得
            feed = _wrapper.GetSpreadsheet(SpreadsheetName);

            //worksheetを生成し、ヘッダを追加する
            string[] columns = LogData.CreateItemInfo.SpreadsheetColumnTitle();
            var sheet = feed.CreateWorksheet("CreateItem", columns.Length);
            AddNewHeader(sheet, columns);

            columns = LogData.CreateShipInfo.SpreadsheetColumnTitle();
            sheet = feed.CreateWorksheet("CreateShip", columns.Length);
            AddNewHeader(sheet, columns);

            columns = LogData.BattleResultInfo.SpreadsheetColumnTitle();
            sheet = feed.CreateWorksheet("Battle", columns.Length);
            AddNewHeader(sheet, columns);

            columns = LogData.MissionResultInfo.SpreadsheetColumnTitle();
            sheet = feed.CreateWorksheet("Mission", columns.Length);
            AddNewHeader(sheet, columns);

            columns = LogData.MaterialChangeInfo.SpreadsheetColumnTitle();
            sheet = feed.CreateWorksheet("Materials", columns.Length);
            AddNewHeader(sheet, columns);

            return true;
        }


        /// <summary>
        /// 指定したシートにヘッダを追加
        /// </summary>
        /// <param name="sheet">ヘッダを追加するワークシート</param>
        /// <param name="headers">ヘッダ</param>
        /// <returns></returns>
        bool AddNewHeader(WorksheetEntry sheet , params string[] headers)
        {
            Debug.WriteLine("AddHeader num=" + headers.Count().ToString());

            //現状、どっちでも動く
//            CellQuery cellQuery = new CellQuery(sheet.CellFeedLink);
//            CellFeed cellFeed = _wrapper.Query(cellQuery);
            CellFeed cellFeed = sheet.QueryCellFeed();

            uint column = 1;

            //InsertはBatch処理できないらしい。
            foreach (var it in headers)
            {
                Debug.WriteLine(string.Format("AddHeader name:{0} column:{1}",it,column));
                var entry = new CellEntry(1, column, it);
                column++;
                cellFeed.Insert(entry);
            }

            return true;
        }
    }

    #region 登録ジョブ
    public class AddCreateShipEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogData.CreateShipInfo _info;
        LogStore.LogStore.LogUploadCompleted _finishCallback;
        public AddCreateShipEntry(SpreadSheetWrapper wrapper, LogData.CreateShipInfo info
                        , LogStore.LogStore.LogUploadCompleted finishCallback)
        {
            _wrapper = wrapper;
            _info = info;
            _memberId = info.MemberID;
            _finishCallback = finishCallback;
        }

        public bool Process()
        {
            Debug.WriteLine("start AddCreateShipEntry");
            WorksheetEntry sheet = GetWorksheet("CreateShip");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            ListFeed listFeed = _wrapper.Query(sheet);

            //ListEntry listRow = new ListEntry();
            ListEntry row = (ListEntry)listFeed.CreateFeedEntry();
            foreach (var it in _info.KeyValue)
            {
                row.Add(it);
            }

            listFeed.Insert(row);

            _finishCallback(true);
            return true;
        }

    }
    public class AddCreateItemEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogData.CreateItemInfo _info;
        LogStore.LogStore.LogUploadCompleted _finishCallback;
        public AddCreateItemEntry(SpreadSheetWrapper wrapper, LogData.CreateItemInfo info
            , LogStore.LogStore.LogUploadCompleted finishCallback)
        {
            _wrapper = wrapper;
            _info = info;
            _memberId = info.MemberID;
            _finishCallback = finishCallback;
        }

        public bool Process()
        {
            Debug.WriteLine("start AddCreateItemEntry");
            WorksheetEntry sheet = GetWorksheet("CreateItem");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            ListFeed listFeed = _wrapper.Query(sheet);

            //ListEntry listRow = new ListEntry();
            ListEntry row = (ListEntry)listFeed.CreateFeedEntry();
            foreach (var it in _info.KeyValue)
            {
                row.Add(it);
            }

            listFeed.Insert(row);

            _finishCallback(true);
            return true;
        }

    }
    public class AddBattleResultEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogData.BattleResultInfo _info;
        LogStore.LogStore.LogUploadCompleted _finishCallback;
        public AddBattleResultEntry(SpreadSheetWrapper wrapper, LogData.BattleResultInfo info
            , LogStore.LogStore.LogUploadCompleted finishCallback)
        {
            _wrapper = wrapper;
            _info = info;
            _memberId = info.MemberID;
            _finishCallback = finishCallback;
        }

        public bool Process()
        {
            Debug.WriteLine("start AddBattleResultEntry");
            WorksheetEntry sheet = GetWorksheet("Battle");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            ListFeed listFeed = _wrapper.Query(sheet);

            //ListEntry listRow = new ListEntry();
            ListEntry row = (ListEntry)listFeed.CreateFeedEntry();
            foreach (var it in _info.KeyValue)
            {
                row.Add(it);
            }

            listFeed.Insert(row);

            _finishCallback(true);
            return false;
        }

    }
    public class AddMissionResultEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogData.MissionResultInfo _info;
        LogStore.LogStore.LogUploadCompleted _finishCallback;
        public AddMissionResultEntry(SpreadSheetWrapper wrapper, LogData.MissionResultInfo info
            , LogStore.LogStore.LogUploadCompleted finishCallback)
        {
            _wrapper = wrapper;
            _info = info;
            _memberId = info.MemberID;
            _finishCallback = finishCallback;
        }

        public bool Process()
        {
            Debug.WriteLine("start AddMissionResultEntry");
            WorksheetEntry sheet = GetWorksheet("Mission");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            ListFeed listFeed = _wrapper.Query(sheet);

            //ListEntry listRow = new ListEntry();
            ListEntry row = (ListEntry)listFeed.CreateFeedEntry();
            foreach (var it in _info.KeyValue)
            {
                row.Add(it);
            }

            listFeed.Insert(row);

            _finishCallback(true);
            return true;
        }

    }
    public class AddMaterialChangeEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogData.MaterialChangeInfo _info;
        LogStore.LogStore.LogUploadCompleted _finishCallback;
        public AddMaterialChangeEntry(SpreadSheetWrapper wrapper, LogData.MaterialChangeInfo info
            ,LogStore.LogStore.LogUploadCompleted finishCallback)
        {
            _wrapper = wrapper;
            _info = info;
            _memberId = info.MemberID;
            _finishCallback = finishCallback;
        }

        public bool Process()
        {
            Debug.WriteLine("start AddMaterialChangeEntry");
            WorksheetEntry sheet = GetWorksheet("Materials");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            sheet = CheckColumns(sheet, LogData.MaterialChangeInfo.SpreadsheetColumnTitle());

            ListFeed listFeed = _wrapper.Query(sheet);

            //ListEntry listRow = new ListEntry();
            ListEntry row = (ListEntry)listFeed.CreateFeedEntry();
            foreach (var it in _info.KeyValue)
            {
                row.Add(it);
            }

            listFeed.Insert(row);

            _finishCallback(true);
            return true;
        }

    }
#endregion

    #region 読込ジョブ
    public class LoadCreateShipEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogStore.LogStore.LogDownloadCompleted _finishCallback;
        IList<LogData.CreateShipInfo> _createShipLog;

        public LoadCreateShipEntry(string memberId, SpreadSheetWrapper wrapper, 
            IList<LogData.CreateShipInfo> createShipLog,
            LogStore.LogStore.LogDownloadCompleted finishCallback)
        {
            _memberId = memberId;
            _wrapper = wrapper;
            _finishCallback = finishCallback;
            _createShipLog = createShipLog;
        }

        public bool Process()
        {
            Debug.WriteLine("start LoadCreateShipEntry");
            WorksheetEntry sheet = GetWorksheet("CreateShip");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            var list = Query(sheet);

            lock (((ICollection)_createShipLog).SyncRoot)
            {
                foreach (var row in list)
                {
                    var it = new LogData.CreateShipInfo(row);
                    it.MemberID = _memberId;
                    it.Number = _createShipLog.Count + 1;
                    _createShipLog.Add(it);
                }
            }
            _finishCallback(true);
            return true;
        }
    }
    public class LoadCreateItemEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogStore.LogStore.LogDownloadCompleted _finishCallback;
        IList<LogData.CreateItemInfo> _createItemLog;

        public LoadCreateItemEntry(string memberId, SpreadSheetWrapper wrapper,
            IList<LogData.CreateItemInfo> createItemLog,
            LogStore.LogStore.LogDownloadCompleted finishCallback)
        {
            _memberId = memberId;
            _wrapper = wrapper;
            _finishCallback = finishCallback;
            _createItemLog = createItemLog;
        }

        public bool Process()
        {
            Debug.WriteLine("start LoadCreateItemEntry");
            WorksheetEntry sheet = GetWorksheet("CreateItem");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            var list = Query(sheet);

            lock (((ICollection)_createItemLog).SyncRoot)
            {
                foreach (var row in list)
                {
                    var it = new LogData.CreateItemInfo(row);
                    it.MemberID = _memberId;
                    it.Number = _createItemLog.Count + 1;

                    _createItemLog.Add(it);
                }
            }
            _finishCallback(true);
            return true;
        }
    }
    public class LoadBattleResultEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogStore.LogStore.LogDownloadCompleted _finishCallback;
        IList<LogData.BattleResultInfo> _battleResultLog;

        public LoadBattleResultEntry(string memberId, SpreadSheetWrapper wrapper,
            IList<LogData.BattleResultInfo> battleResultLog,
            LogStore.LogStore.LogDownloadCompleted finishCallback)
        {
            _memberId = memberId;
            _wrapper = wrapper;
            _finishCallback = finishCallback;
            _battleResultLog = battleResultLog;
        }

        public bool Process()
        {
            Debug.WriteLine("start LoadBattleResultEntry");
            WorksheetEntry sheet = GetWorksheet("Battle");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            var list = Query(sheet);

            lock (((ICollection)_battleResultLog).SyncRoot)
            {
                foreach (var row in list)
                {
                    var it = new LogData.BattleResultInfo(row);
                    it.MemberID = _memberId;
                    it.Number = _battleResultLog.Count + 1;
                    _battleResultLog.Add(it);
                }
            }
            _finishCallback(true);
            return true;
        }
    }
    public class LoadMissionResultEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogStore.LogStore.LogDownloadCompleted _finishCallback;
        IList<LogData.MissionResultInfo> _missionResultLog;

        public LoadMissionResultEntry(string memberId, SpreadSheetWrapper wrapper,
            IList<LogData.MissionResultInfo> missionResultLog,
            LogStore.LogStore.LogDownloadCompleted finishCallback)
        {
            _memberId = memberId;
            _wrapper = wrapper;
            _finishCallback = finishCallback;
            _missionResultLog = missionResultLog;
        }

        public bool Process()
        {
            Debug.WriteLine("start LoadMissionResultEntry");
            WorksheetEntry sheet = GetWorksheet("Mission");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            var list = Query(sheet);

            lock (((ICollection)_missionResultLog).SyncRoot)
            {
                foreach (var row in list)
                {
                    var it = new LogData.MissionResultInfo(row);
                    it.MemberID = _memberId;
                    it.Number = _missionResultLog.Count + 1;
                    _missionResultLog.Add(it);
                }
            }
            _finishCallback(true);
            return true;
        }
    }
    public class LoadMaterialChangeEntry : SpreadSheetJobHelper, ISpreadsheetJob
    {
        LogStore.LogStore.MaterialDataLoadPostProcess _postCallback;
        LogStore.LogStore.LogDownloadCompleted _finishCallback;
        IList<LogData.MaterialChangeInfo> _materialsChangeLog;

        public LoadMaterialChangeEntry(string memberId, SpreadSheetWrapper wrapper,
            IList<LogData.MaterialChangeInfo> materialsChangeLog,
            LogStore.LogStore.MaterialDataLoadPostProcess postCallback,
            LogStore.LogStore.LogDownloadCompleted finishCallback)
        {
            _memberId = memberId;
            _wrapper = wrapper;
            _finishCallback = finishCallback;
            _postCallback = postCallback;
            _materialsChangeLog = materialsChangeLog;
        }

        public bool Process()
        {
            Debug.WriteLine("start LoadMaterialChangeEntry");
            WorksheetEntry sheet = GetWorksheet("Materials");
            if (sheet == null)
            {
                _finishCallback(false);
                return false;
            }

            sheet = CheckColumns(sheet, LogData.MaterialChangeInfo.SpreadsheetColumnTitle());

            var list = Query(sheet);

            lock (((ICollection)_materialsChangeLog).SyncRoot)
            {
                foreach (var row in list)
                {
                    var it = new LogData.MaterialChangeInfo(row);
                    it.MemberID = _memberId;
                    it.Number = _materialsChangeLog.Count + 1;
                    if (_materialsChangeLog.Count > 0)
                        it.PrevItem = _materialsChangeLog.Last();

                    _materialsChangeLog.Add(it);

                    _postCallback(it);
                }
            }
            _finishCallback(true);
            return true;
        }
    }

#endregion
}
