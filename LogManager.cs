using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Data;

namespace KCB2.Log
{
    /// <summary>
    /// ログ保存用基礎パラメタ
    /// </summary>
    public interface ILogItemBase
    {
        string MemberID { get; set; }

        /// <summary>
        /// ログダウンロード時のクラス
        /// </summary>
        string LogClass { get; }
    }

    /// <summary>
    /// CSVログ保存用インタフェイス
    /// </summary>
    public interface ICSVLog : ILogItemBase
    {
        /// <summary>
        /// CSV出力
        /// </summary>
        string CSV { get; }
    }

    /// <summary>
    /// ログエクスポート用インタフェイス
    /// </summary>
    public interface ILogExporter
    {
        string ExportLogEntry(LogManager.ExportLogDataType exportType);
    }

    /// <summary>
    /// Google Spreadsheet用のインタフェイス
    /// </summary>
    public interface IGSpreadLog : ILogItemBase
    {
        /// <summary>
        /// 登録するためのKey-Valueを取得
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> KeyValue { get; }
    }
}

namespace KCB2.LogManager
{
    /// <summary>
    /// ログエクスポート形式
    /// </summary>
    public enum ExportLogDataType
    {
        CSV,
    };

    /// <summary>
    /// ログマネージャ
    /// </summary>
    public class LogManager
    {
        FormMain _mainFrm;

        public LogManager(FormMain mainFrm)
        {
            _mainFrm = mainFrm;
        }

        /// <summary>
        /// ステータスバーのメッセージを更新する
        /// </summary>
        /// <param name="format">フォーマット</param>
        /// <param name="args">引数</param>
        void UpdateStatusMessage(string format, params object[] args)
        {
            ///form側でInvokeしてるので考慮不要
            _mainFrm.UpdateStatus(format, args);
        }

        /// <summary>
        /// メンバーID
        /// </summary>
        public string MemberID { get; protected set; }

        string _storeType = "";

        /// <summary>
        /// 利用するログストア
        /// </summary>
        public LogStore.LogStore LogStore
        {
            get { return _logStore; }
            set {
                if (value == null)
                    _logStore = new LogStore.LogStore();
                else
                    _logStore = value;
            }
        }

        /// <summary>
        /// ログストア.
        /// デフォルトは何もしないストアを定義
        /// </summary>
        LogStore.LogStore _logStore = new LogStore.LogStore();

        /// <summary>
        /// ログを読み込む
        /// </summary>
        /// <param name="memberId">ログを読み込むユーザのMember ID</param>
        /// <returns>成功すればtrue</returns>
        public bool LoadLog(string memberId)
        {
            if (MemberID == memberId && _storeType == _logStore.StoreType)
            {
                Debug.WriteLine("Log Already downloaded:" + memberId);
                return false;
            }

            _bLoadingMaterialInfo = true;
            _storeType = _logStore.StoreType;
            _clearLog();

            MemberID = memberId;

            if (_frmLog.InvokeRequired)
                _frmLog.Invoke((System.Windows.Forms.MethodInvoker)(() =>
                _frmLog.Text = string.Format("記録(Store:{0} ID:{1})", _storeType, memberId)));
            else
                _frmLog.Text = string.Format("記録(Store:{0} ID:{1})", _storeType, memberId);

            _logStore.LoadBattleResult(memberId, _battleLog, (bool bSucceeded)  => {
                UpdateStatusMessage("戦闘結果ログの読み込みに{0}しました", bSucceeded ? "成功" : "失敗");
                _vlvBattleLog.UpdateList();
            });
            _logStore.LoadCreateItemInfo(memberId, _createItemLog, (bool bSucceeded)  => {
                UpdateStatusMessage("開発ログの読み込みに{0}しました", bSucceeded ? "成功" : "失敗");
                _vlvCreateItem.UpdateList();
            });
            _logStore.LoadCreateShipInfo(memberId, _createShipLog, (bool bSucceeded) => {
                UpdateStatusMessage("建造ログの読み込みに{0}しました", bSucceeded ? "成功" : "失敗");
                _vlvCreateShip.UpdateList();
            });
            _logStore.LoadMissionResult(memberId, _missionLog, (bool bSucceeded) => {
                UpdateStatusMessage("遠征ログの読み込みに{0}しました", bSucceeded ? "成功" : "失敗");
                _vlvMissionResult.UpdateList();
            });
            _logStore.LoadMaterialChange(memberId, _materialsLog,
                (LogData.MaterialChangeInfo info) =>
                {
                    _addMaterialRow(info.SetRowData(_createMaterialNewRow()));
                    if (_dtLastMaterialUploaded < info.Date)
                        _dtLastMaterialUploaded = info.Date;
                }, (bool bSucceeded) =>
                {
                    UpdateStatusMessage("資源推移の読み込みに{0}しました", bSucceeded ? "成功" : "失敗");
                    _vlvMaterialChange.UpdateList(); UpdateMaterialChart();
                    _bLoadingMaterialInfo = false;
                });

            return true;
        }



        /// <summary>
        /// 戦闘結果の追加
        /// </summary>
        /// <param name="info">戦闘結果データ</param>
        public void AddBattleResult(LogData.BattleResultInfo info)
        {
            if (info == null)
            {
                Debug.WriteLine("invalid BattleResult(null)");
                return;
            }

            lock (((ICollection)_battleLog).SyncRoot)
            {
                info.Number = _battleLog.Count + 1;
                _battleLog.Add(info);
            }
            _logStore.AddBattleResult(info, (bool bSucceeded) =>
                {
                    if(!(Properties.Settings.Default.SuppressSuceedWriteLog && bSucceeded))
                    UpdateStatusMessage("戦闘結果の書き込みに{0}しました", bSucceeded ? "成功" : "失敗");
                });
            _vlvBattleLog.UpdateList();
        }

        /// <summary>
        /// 遠征結果の追加
        /// </summary>
        /// <param name="info">遠征結果</param>
        public void AddMissionResult(LogData.MissionResultInfo info)
        {
            if (info == null)
            {
                Debug.WriteLine("invalid MissionResult(null)");
                return;
            }

            lock (((ICollection)_missionLog).SyncRoot)
            {
                info.Number = _missionLog.Count + 1;
                _missionLog.Add(info);
            }
            _logStore.AddMissionResult(info, (bool bSucceeded) =>
            {
                if (!(Properties.Settings.Default.SuppressSuceedWriteLog && bSucceeded))
                    UpdateStatusMessage("遠征結果の書き込みに{0}しました", bSucceeded ? "成功" : "失敗");
            });
            _vlvMissionResult.UpdateList();
        }

        /// <summary>
        /// 艦船建造結果の追加
        /// </summary>
        /// <param name="infoList">艦船建造情報</param>
        public virtual void AddCreateShipResult(IEnumerable<LogData.CreateShipInfo> infoList)
        {
            foreach (var info in infoList)
            {
                //そんなに多いアイテムはないはず(最大４つ)なのでforeach内でlock呼ぶ
                lock (((ICollection)_createShipLog).SyncRoot)
                {
                    info.Number = _createShipLog.Count + 1;
                    _createShipLog.Add(info);
                }
                _logStore.AddCreateShipInfo(info, (bool bSucceeded) =>
                {
                    if (!(Properties.Settings.Default.SuppressSuceedWriteLog && bSucceeded))
                        UpdateStatusMessage("建造結果の書き込みに{0}しました", bSucceeded ? "成功" : "失敗");
                });
            }
            _vlvCreateShip.UpdateList();
        }

        /// <summary>
        /// 装備開発結果の追加
        /// </summary>
        /// <param name="info">装備開発情報</param>
        public void AddCreateItemResult(LogData.CreateItemInfo info)
        {
            if (info == null)
            {
                Debug.WriteLine("invalid CreateItem(null)");
                return;
            }

            lock (((ICollection)_createItemLog).SyncRoot)
            {
                info.Number = _createItemLog.Count + 1;
                _createItemLog.Add(info);
            }
            _logStore.AddCreateItemInfo(info, (bool bSucceeded) =>
            {
                if (!(Properties.Settings.Default.SuppressSuceedWriteLog && bSucceeded))
                    UpdateStatusMessage("開発結果の書き込みに{0}しました", bSucceeded ? "成功" : "失敗");
            });
            _vlvCreateItem.UpdateList();
        }

        protected DateTime _dtLastMaterialUploaded = DateTime.MinValue;
        volatile bool _bLoadingMaterialInfo = false;

        /// <summary>
        /// 資源推移情報の追加
        /// </summary>
        /// <param name="info">資源情報</param>
        public bool AddMaterialsChangeResult(LogData.MaterialChangeInfo info)
        {
            if (info == null)
            {
                Debug.WriteLine("invalid MaterialChange(null)");
                return false;
            }

            DateTime now = DTNow;

            if (_bLoadingMaterialInfo)
            {
                Debug.WriteLine("MaterialChange still loading...");
                return false;
            }

            Debug.WriteLine(string.Format("LastMod:{0} Now:{1}",
                _dtLastMaterialUploaded, now));

            if (_dtLastMaterialUploaded == DateTime.MinValue
                || (_dtLastMaterialUploaded < now))
            {
                Debug.WriteLine("Update MaterialsInfo");
                info.Date = now;

                lock (((ICollection)_materialsLog).SyncRoot)
                {
                    //リストが空の時例外を投げるらしい。
                    if (_materialsLog.Count > 0)
                        info.PrevItem = _materialsLog.Last();

                    _materialsLog.Add(info);
                }
                _addMaterialRow(info.SetRowData(_createMaterialNewRow()));

                UpdateMaterialChart();

                _vlvMaterialChange.UpdateList();
                _dtLastMaterialUploaded = now;
                _logStore.AddMaterialChange(info, (bool bSucceeded) =>
                {
                    if (!(Properties.Settings.Default.SuppressSuceedWriteLog && bSucceeded))
                        UpdateStatusMessage("資源推移の書き込みに{0}しました", bSucceeded ? "成功" : "失敗");
                });
                return true;
            }
            return false;
        }

        /// <summary>
        /// ログを保存する
        /// </summary>
        /// <returns>成功すればtrue</returns>
        public bool SaveLog()
        {
            _logStore.SaveBattleResult(_battleLog);
            _logStore.SaveCreateItemInfo(_createItemLog);
            _logStore.SaveCreateShipInfo(_createShipLog);
            _logStore.SaveMaterialChange(_materialsLog);
            _logStore.SaveMissionResult(_missionLog);

            return true;
        }

        #region 保持しているデータ

        /* リストはジョブキュースレッド(データの追加)とUIスレッド(リストビューへの参照)から同時にアクセスされる。
         * 
         * lock (((ICollection)_createItemLog).SyncRoot) を使って適切にロックしないと
         * なんかのはずみでおかしなことが起きうる。
         * 
         */

        protected List<LogData.CreateShipInfo> _createShipLog = new List<LogData.CreateShipInfo>();
        protected List<LogData.CreateItemInfo> _createItemLog = new List<LogData.CreateItemInfo>();
        protected List<LogData.BattleResultInfo> _battleLog = new List<LogData.BattleResultInfo>();
        protected List<LogData.MissionResultInfo> _missionLog = new List<LogData.MissionResultInfo>();
        protected List<LogData.MaterialChangeInfo> _materialsLog = new List<LogData.MaterialChangeInfo>();

        protected VirtualListViewManager<LogData.CreateShipInfo> _vlvCreateShip
            = new VirtualListViewManager<LogData.CreateShipInfo>(true);

        protected VirtualListViewManager<LogData.CreateItemInfo> _vlvCreateItem
            = new VirtualListViewManager<LogData.CreateItemInfo>(true);

        protected VirtualListViewManager<LogData.BattleResultInfo> _vlvBattleLog
            = new VirtualListViewManager<LogData.BattleResultInfo>(true);

        protected VirtualListViewManager<LogData.MissionResultInfo> _vlvMissionResult
            = new VirtualListViewManager<LogData.MissionResultInfo>(true);

        protected VirtualListViewManager<LogData.MaterialChangeInfo> _vlvMaterialChange
            = new VirtualListViewManager<LogData.MaterialChangeInfo>(true);

        System.Windows.Forms.DataVisualization.Charting.Chart _ctMaterial;

#endregion

        /// <summary>
        /// ログクリア
        /// </summary>
        protected void _clearLog()
        {
            _createItemLog.Clear();
            _vlvCreateItem.UpdateList();

            _createShipLog.Clear();
            _vlvCreateShip.UpdateList();

            _battleLog.Clear();
            _vlvBattleLog.UpdateList();

            _missionLog.Clear();
            _vlvMissionResult.UpdateList();

            _materialsLog.Clear();
            _vlvMaterialChange.UpdateList();

            _clearMaterialRow();
        }

        FormLog _frmLog;

        /// <summary>
        /// ログコントロールにアタッチ
        /// </summary>
        /// <param name="lvCreateShip"></param>
        /// <param name="lvCreateItem"></param>
        /// <param name="lvBattleResult"></param>
        /// <param name="lvMissionResult"></param>
        /// <param name="lvMaterialChange"></param>
        public void AttachLogControls(
            System.Windows.Forms.ListView lvCreateShip,
            System.Windows.Forms.ListView lvCreateItem,
            System.Windows.Forms.ListView lvBattleResult,
            System.Windows.Forms.ListView lvMissionResult,
            System.Windows.Forms.ListView lvMaterialChange,
            System.Windows.Forms.DataVisualization.Charting.Chart ctMaterial,
            FormLog frmLog)
        {
            _vlvBattleLog.Attach(lvBattleResult, _battleLog);
            _vlvCreateItem.Attach(lvCreateItem, _createItemLog);
            _vlvCreateShip.Attach(lvCreateShip, _createShipLog);
            _vlvMaterialChange.Attach(lvMaterialChange, _materialsLog);
            _vlvMissionResult.Attach(lvMissionResult, _missionLog);

            _ctMaterial = ctMaterial;
            _frmLog = frmLog;
        }

        /// <summary>
        /// 朝五時に日付の変わる日付が入ったDateTimeを返します
        /// 時間は全て00:00:00
        /// </summary>
        public static DateTime DTNow
        {
            get
            {
                DateTime dtNow = DateTime.Now;
                DateTime dtUpdate = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day,
                    0, 0, 0);

                //もし午前五時以前だったら一日戻す
                if (dtNow.Hour < 5)
                    dtUpdate = dtUpdate.AddDays(-1);

                return dtUpdate;
            }
        }

        /// <summary>
        /// 表データの初期化
        /// </summary>
        /// <returns>データセット</returns>
        public DataSet InitMaterialDataTable()
        {
            Debug.Assert(dsMaterial == null);

            DataTable dt = new DataTable("MatTable");
            dsMaterial = new DataSet();

            dt.Columns.Add("日付", typeof(DateTime));
            dt.Columns.Add("燃料", typeof(int));
            dt.Columns.Add("弾薬", typeof(int));
            dt.Columns.Add("鋼材", typeof(int));
            dt.Columns.Add("ボーキサイト", typeof(int));

            dsMaterial.Tables.Add(dt);

            return dsMaterial;
        }
        DataSet dsMaterial = null;

        /// <summary>
        /// データクリア
        /// </summary>
        protected void _clearMaterialRow()
        {
            Debug.Assert(dsMaterial != null);
            Debug.Assert(dsMaterial.Tables.Count > 0);
            dsMaterial.Tables[0].Clear();
        }

        /// <summary>
        /// 行要素追加
        /// </summary>
        /// <param name="newRow">行要素</param>
        protected void _addMaterialRow(DataRow newRow)
        {
            Debug.Assert(dsMaterial != null);
            dsMaterial.Tables[0].Rows.Add(newRow);
        }

        /// <summary>
        /// 新行生成
        /// </summary>
        /// <returns>新行</returns>
        protected DataRow _createMaterialNewRow()
        {
            Debug.Assert(dsMaterial != null);
            return dsMaterial.Tables[0].NewRow();
        }

        protected void UpdateMaterialChart()
        {
            if (_ctMaterial.InvokeRequired)
                _ctMaterial.Invoke((System.Windows.Forms.MethodInvoker)
                    (() => _ctMaterial.DataBind()));
            else
                _ctMaterial.DataBind();
        }

        /// <summary>
        /// 指定したインデクスの戦闘結果を取得
        /// </summary>
        /// <param name="index">インデクス</param>
        /// <returns></returns>
        public LogData.BattleResultInfo GetBattleResultItem(int index)
        {
            return _vlvBattleLog.GetItem(index);
        }

        /// <summary>
        /// 建造情報のエクスポート
        /// </summary>
        /// <param name="sw">書き出すStreamWriter</param>
        /// <param name="dataType">書き出す形式</param>
        /// <returns>成功すればtrue</returns>
        public bool ExportCreateShipLog(System.IO.StreamWriter sw,ExportLogDataType dataType)
        {
            if (dataType != ExportLogDataType.CSV)
                return false;

            lock (((ICollection)_createShipLog).SyncRoot)
            {
                foreach(Log.ILogExporter it in _createShipLog)
                {
                    sw.WriteLine(it.ExportLogEntry(dataType));
                }
            }

            return true;
        }
        /// <summary>
        /// 開発結果のエクスポート
        /// </summary>
        /// <param name="sw">書き出すStreamWriter</param>
        /// <param name="dataType">書き出す形式</param>
        /// <returns>成功すればtrue</returns>
        public bool ExportCreateItemLog(System.IO.StreamWriter sw, ExportLogDataType dataType)
        {
            if (dataType != ExportLogDataType.CSV)
                return false;

            lock (((ICollection)_createItemLog).SyncRoot)
            {
                foreach (Log.ILogExporter it in _createItemLog)
                {
                    sw.WriteLine(it.ExportLogEntry(dataType));
                }
            }

            return true;
        }

        /// <summary>
        /// 戦闘結果のエクスポート
        /// </summary>
        /// <param name="sw">書き出すStreamWriter</param>
        /// <param name="dataType">書き出す形式</param>
        /// <returns>成功すればtrue</returns>
        public bool ExportBattleLog(System.IO.StreamWriter sw, ExportLogDataType dataType)
        {
            if (dataType != ExportLogDataType.CSV)
                return false;

            lock (((ICollection)_battleLog).SyncRoot)
            {
                foreach (Log.ILogExporter it in _battleLog)
                {
                    sw.WriteLine(it.ExportLogEntry(dataType));
                }
            }

            return true;
        }

        /// <summary>
        /// 遠征結果のエクスポート
        /// </summary>
        /// <param name="sw">書き出すStreamWriter</param>
        /// <param name="dataType">書き出す形式</param>
        /// <returns>成功すればtrue</returns>
        public bool ExportMissionLog(System.IO.StreamWriter sw, ExportLogDataType dataType)
        {
            if (dataType != ExportLogDataType.CSV)
                return false;

            lock (((ICollection)_missionLog).SyncRoot)
            {
                foreach (Log.ILogExporter it in _missionLog)
                {
                    sw.WriteLine(it.ExportLogEntry(dataType));
                }
            }

            return true;
        }

        /// <summary>
        /// 資源情報のエクスポート
        /// </summary>
        /// <param name="sw">書き出すStreamWriter</param>
        /// <param name="dataType">書き出す形式</param>
        /// <returns>成功すればtrue</returns>
        public bool ExportMaterialsLog(System.IO.StreamWriter sw, ExportLogDataType dataType)
        {
            if (dataType != ExportLogDataType.CSV)
                return false;

            lock (((ICollection)_materialsLog).SyncRoot)
            {
                foreach (Log.ILogExporter it in _materialsLog)
                {
                    sw.WriteLine(it.ExportLogEntry(dataType));
                }
            }

            return true;
        }
    }
}


