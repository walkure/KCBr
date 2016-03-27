using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace KCB2
{
    /// <summary>
    /// セッションデータ
    /// </summary>
    public abstract class SessionData
    {
        /// <summary>
        /// レスポンス
        /// </summary>
        protected string ResponseBody { get; set; }
        /// <summary>
        /// レスポンスのMIMEType
        /// </summary>
        public string ResponseMimeType { get; protected set; }
        /// <summary>
        /// レスポンスJSON
        /// </summary>
        public string ResponseJSON
        {
            get
            {
                //JSONの開始位置を検索
                int jsonStart = ResponseBody.IndexOf('{');
                //JSONの終了位置を検索
                int jsonEnd = ResponseBody.LastIndexOf('}');
                
                //正しいJSONの形ではない。
                if (jsonStart == -1 || jsonEnd == -1)
                    return "";

                return ResponseBody.Substring(jsonStart, jsonEnd - jsonStart + 1);
            }
        }

        /// <summary>
        /// リクエスト
        /// </summary>
        protected string RequestBody { get; set; }

        /// <summary>
        /// リクエストパス
        /// </summary>
        public string PathQuery { get; protected set; }

        /// <summary>
        /// POSTクエリパラメタ
        /// </summary>
        public IDictionary<string, string> QueryParam
        {
            get
            {
                return ParsePostQuery(RequestBody);
            }
        }

        /// <summary>
        /// セッションを保存
        /// </summary>
        /// <param name="fileName">保存するファイル名</param>
        abstract public void SaveSession(string FileName);

        /// <summary>
        /// セッションを保存する実装
        /// </summary>
        /// <param name="fileName">保存するファイル名</param>
        protected void saveSessionImpl(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                sw.WriteLine("SessionType:{0}", SessionType);
                sw.WriteLine("PathQuery:{0}", PathQuery);
                sw.WriteLine("Request:{0}", RequestBody);
                sw.WriteLine("Response:{0}", ResponseBody);

                string JSON = ResponseJSON;
                if (JSON.Length > 0)
                {
                    sw.WriteLine("JSON:{0}", DecodeJSON(JSON));
                }
            }
        }

        /// <summary>
        /// セッションの種別
        /// </summary>
        public string SessionType { get; protected set; }

        /// <summary>
        /// エスケープされたJSONをデコードする
        /// </summary>
        /// <param name="raw">JSON</param>
        /// <returns>デコードされたJSON</returns>
        public static string DecodeJSON(string raw)
        {
            if (raw == null)
                return "";
            if (raw == "")
                return "";

            String decoded = Regex.Replace(raw, "\\\\u(?<code>[0-9a-fA-F]{4})",
            m =>
            {
                int code = Convert.ToInt32(m.Groups["code"].Value, 16);
                return ((char)code).ToString();
            });

            return decoded;

        }

        /// <summary>
        /// リクエストクエリの解析
        /// </summary>
        /// <param name="query">POSTクエリ文字列</param>
        /// <returns></returns>
        public static IDictionary<string, string> ParsePostQuery(string query)
        {
            var ret = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(query))
                return ret;

            var req = Uri.UnescapeDataString(query);
            var kv_pair = req.Split(new char[] { '&' });

            foreach (var it in kv_pair)
            {
                var k_v = it.Split(new char[] { '=' });
                var key = k_v[0].Trim(new char[] { '?', ' ' });
                var val = k_v[1];//.Trim();
                Debug.WriteLine(string.Format("PostQuery Key:[{0}] Value:[{1}]",
                    key, val));
                Debug.Assert(k_v.Length == 2);
                ret[key] = val;
            }

            return ret;
        }
        /// <summary>
        /// リクエストクエリの解析
        /// </summary>
        /// <param name="query">POSTクエリ バイト列</param>
        /// <returns></returns>
        public static IDictionary<string, string> ParsePostQuery(byte[] query)
        {
            string req = Encoding.ASCII.GetString(query);
            return ParsePostQuery(req);
        }



    }

    /// <summary>
    /// デバッグ用ダミーセッション
    /// </summary>
    public class DummySessionData : SessionData
    {

        public DummySessionData(string req, string res, string path, string mime)
        {
            RequestBody = req;
            ResponseBody = res;
            PathQuery = path;
            ResponseMimeType = mime;

            SessionType = "Dummy";

        }

        public override void SaveSession(string fileName)
        {
            Debug.WriteLine("DummySession save fileName:"+fileName);
        }
    }

    /// <summary>
    /// HTTProxyを使ったセッション
    /// </summary>
    public class HTTProxySessionData : SessionData
    {
        public HTTProxySessionData(HTTProxy.SessionInfo info)
        {
            RequestBody = info.Request.String;
            ResponseBody = info.Response.ContentString;
            PathQuery = info.Uri.PathAndQuery;
            ResponseMimeType = info.Response.ContentType;

            SessionType = "HTTProxy";
        }

        public override void SaveSession(string fileName)
        {
            saveSessionImpl(fileName);
        }
    }

}
