using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Google.GData.Client;
using Google.GData.Spreadsheets;
using System.Net;

namespace KCB2.GSpread
{
    public class SpreadSheetWrapper
    {
        const string _app_name = "SpreadSheetWrapper";
        OAuth2Parameters _parameters = new OAuth2Parameters();
        SpreadsheetsService _service = null;
        GOAuth2RequestFactory _requestFactory = null;

        public SpreadSheetWrapper()
        {
            _parameters.ClientId = "471934575594.apps.googleusercontent.com";
            _parameters.ClientSecret = "Gr_MKJfuzKLYqnHCyl4m6aSP";
            _parameters.RedirectUri = "urn:ietf:wg:oauth:2.0:oob";

            /* 本来はGoogleDocs(Drive API)へのアクセス権限はいらないはず
             * スプレッドシートの作成は出来ると権限要求の説明には書いてある。
             * 
             * しかし、出来ない。Drive APIで作れとドキュメントにある。うそつき。
             */
//            parameters.Scope = "https://spreadsheets.google.com/feeds";
            _parameters.Scope = "https://spreadsheets.google.com/feeds https://www.googleapis.com/auth/drive.file";

            _requestFactory = new GOAuth2RequestFactory(null, _app_name,_parameters);

        }

        /// <summary>
        /// サービス取得。必要に応じて生成
        /// </summary>
        SpreadsheetsService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new SpreadsheetsService(_app_name);
                    _service.RequestFactory = _requestFactory;
                }
                return _service;
            }

        }

        /// <summary>
        /// 認証URLの取得
        /// </summary>
        public string AuthorizationURL
        {
            get
            {
                return OAuthUtil.CreateOAuth2AuthorizationUrl(_parameters);
            }
        }

        /// <summary>
        /// アクセストークン
        /// </summary>
        public string AccessToken
        {
            get
            {
                return _parameters.AccessToken;
            }
        }

        /// <summary>
        /// 更新トークン
        /// </summary>
        public string RefreshToken
        {
            get
            {
                return _parameters.RefreshToken;
            }
        }

        /// <summary>
        /// トークン更新日時
        /// </summary>
        public DateTime TokenExpiry
        {
            get
            {
                return _parameters.TokenExpiry;
            }
        }

        /// <summary>
        /// ネットワークアクセスプロキシ
        /// </summary>
        public System.Net.IWebProxy Proxy
        {
            get { return _requestFactory.Proxy; }
            set { _requestFactory.Proxy = OAuthUtil.Proxy = value; }
        }

        /// <summary>
        /// 認証
        /// </summary>
        /// <param name="authCode"></param>
        /// <returns></returns>
        public bool Autorize(string authCode)
        {
            Properties.Settings.Default.GSRefreshToken = "";
            _parameters.AccessCode = authCode;
            try
            {
                OAuthUtil.GetAccessToken(_parameters);
            }
            catch (System.Net.WebException ex)
            {
                //認証失敗だお
                System.Diagnostics.Debug.WriteLine("Authorize ex:" + ex.ToString());
                return false;
            }
            Properties.Settings.Default.GSRefreshToken = _parameters.RefreshToken;
            return true;
        }

        /// <summary>
        /// トークン更新
        /// </summary>
        /// <returns></returns>
        public bool Refresh()
        {
            Debug.WriteLine("Refresh AccessToken");
            _parameters.RefreshToken = Properties.Settings.Default.GSRefreshToken;
            try
            {
                OAuthUtil.RefreshAccessToken(_parameters);
            }
            catch (Exception ex)
            {
                //認証失敗だお
                System.Diagnostics.Debug.WriteLine("Refresh ex:" + ex.ToString());
                return false;
            }
            Debug.WriteLine("AccessToken Refreshed Expiry:" + _parameters.TokenExpiry.ToString());
            return true;
        }

        public SpreadsheetFeed SheetFeed
        {
            get
            {
                Debug.WriteLine("Try SpreadsheetQuery");
                SpreadsheetQuery query = new SpreadsheetQuery();
                return Service.Query(query);
            }
        }

        public ListEntry Insert(ListFeed listFeed, ListEntry listEntry)
        {
            Debug.WriteLine("Try ListInsert");

            return Service.Insert(listFeed, listEntry);

            /*
            ListEntry retVal = null;
            try
            {
                retVal = Service.Insert(listFeed, listEntry);
            }
            catch (Google.GData.Client.GDataRequestException ex)
            {
                Debug.WriteLine("ListEntry.Insert fail:" + ex.ToString());
                return null;
            }

            return retVal;*/
        }

        public WorksheetEntry Insert(WorksheetFeed wsFeed, WorksheetEntry wsEntry)
        {
            Debug.WriteLine("Try WorksheetInsert");

            return Service.Insert(wsFeed, wsEntry);
        }

        public ListFeed Query(WorksheetEntry wsEntry)
        {
            Debug.WriteLine("Try WorksheetQuery");
            AtomLink listFeedLink = wsEntry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            return Service.Query(listQuery);
        }

        public CellFeed Query(CellQuery cellQuery)
        {
            Debug.WriteLine("Try CellQuery");
            return Service.Query(cellQuery);
        }

        /// <summary>
        /// スプレッドシートを作成する。同じ名前の場合でも気にせず作成される。
        /// </summary>
        /// <param name="spreadSheetName">スプレッドシート名</param>
        /// <returns>失敗したらfalse</returns>
        public bool CreateSpreadsheet(string spreadSheetName)
        {
            // https://developers.google.com/drive/web/mime-types

            string req_json = string.Format("{{\"title\": \"{0}\",\"mimeType\": \"application/vnd.google-apps.spreadsheet\"}}", spreadSheetName);
            Debug.WriteLine("Trying to create spreadsheet:" + spreadSheetName);

            // AccessTokenが切れてるかもしれないので更新
            Refresh();

            /*
             * https://developers.google.com/drive/v2/reference/files/insert
             * metadataを送るだけなのでエンドポイントは https://www.googleapis.com/drive/v2/files
             */
            HttpWebRequest req = WebRequest.Create("https://www.googleapis.com/drive/v2/files")
                as HttpWebRequest;
            if (req == null)
                return false;

            req.ContentType = "application/json; charset=UTF-8";
            req.Method = "POST";
            req.Proxy = _requestFactory.Proxy;
            req.Headers.Add("Authorization", string.Format("Bearer {0}",_parameters.AccessToken));

            var req_by = Encoding.UTF8.GetBytes(req_json);
            req.ContentLength = req_by.Length;

            var rs = req.GetRequestStream();
            rs.Write(req_by, 0, req_by.Length);
            rs.Close();

            try
            {
                HttpWebResponse res = req.GetResponse() as HttpWebResponse;

                /*
                 * 追加したらSpreadsheetsService を作りなおさないとだめ。
                 * SpreadsheetFeedの再QueryではWorksheetFeedが取得できず例外が飛ぶ
                 */
                _service = null;
            }
            catch (WebException wex)
            {
                System.Diagnostics.Debug.WriteLine("CreateSpreadsheet Ex:" + wex.ToString());
                if (wex != null)
                {
                    using (var sr = new System.IO.StreamReader(wex.Response.GetResponseStream()))
                    {
                        Debug.WriteLine("wex:" + sr.ReadToEnd());
                    }
                }
                return false;

            }
            Debug.WriteLine("Spreadsheet created");

            return true;
        }

        /// <summary>
        /// 指定した名前のスプレッドシートを拾ってくる
        /// </summary>
        /// <param name="spreadSheetName">スプレッドシート名</param>
        /// <returns>該当するスプレッドシートが持つワークシートオブジェクト</returns>
        public WorksheetFeed GetSpreadsheet(string spreadSheetName)
        {
            Debug.WriteLine("Search Spreadsheet named:" + spreadSheetName);
            foreach (SpreadsheetEntry it in SheetFeed.Entries)
            {

                if (it.Title.Text == spreadSheetName)
                {
                    Debug.WriteLine("Spreadsheet found");
                    return it.Worksheets;
                }
            }
            Debug.WriteLine("Spreadsheet not found");
            return null;
        }

    }
}
