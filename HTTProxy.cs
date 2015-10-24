using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Threading;

namespace KCB2
{
    public class HTTProxy : IDisposable
    {
        #region プロキシ設定
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet,
            int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        struct INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        }

        /// <summary>
        /// WinInetプロセスプロキシ設定
        /// </summary>
        /// <param name="strProxy">プロキシ設定文字列 http=http-proxy:8080 https=https-proxy:8081</param>
        /// <param name="strProxyBypass">プロキシ不使用設定文字列</param>
        public static void SetProcessProxy(string strProxy,string strProxyBypass)
        {
            /*
             * Listing proxy servers.
             * https://msdn.microsoft.com/en-us/library/windows/desktop/aa383996(v=vs.85).aspx
             * 
             */
            const int INTERNET_OPTION_PROXY = 38;
            const int INTERNET_OPEN_TYPE_PROXY = 3;
            INTERNET_PROXY_INFO proxyInfo;

            // Filling in structure
            proxyInfo.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
            proxyInfo.proxy = Marshal.StringToHGlobalAnsi(strProxy);
            proxyInfo.proxyBypass = Marshal.StringToHGlobalAnsi(strProxyBypass);

            // Allocating memory
            IntPtr pProxyInfo = Marshal.AllocCoTaskMem(Marshal.SizeOf(proxyInfo));

            DebugOut("Update Proxy Settings proxy:[{0}] proxyBypass:[{1}]", strProxy, strProxyBypass);

            // Converting structure to IntPtr
            Marshal.StructureToPtr(proxyInfo, pProxyInfo, true);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY,pProxyInfo, Marshal.SizeOf(proxyInfo));
            Marshal.FreeCoTaskMem(pProxyInfo);
        }
        #endregion

        /// <summary>
        /// セッション情報
        /// </summary>
        public class SessionInfo
        {
            public  SessionInfo()
            {
                Ignore = false;
                BypassServerRequest = false;
                Response = new HTTPResponse();
                Request = new HTTPRequest();
            }

            /// <summary>
            /// リクエストURI
            /// </summary>
            public Uri Uri { get; set; }
            public HTTPRequest Request { get; set; }
            /// <summary>
            /// レスポンス情報
            /// </summary>
            public HTTPResponse Response { get; set; }
            /// <summary>
            /// 当該レスポンスを処理しない(AfterSessionCompletedが呼ばれない)
            /// </summary>
            public bool Ignore { get; set; }
            /// <summary>
            /// サーバへリクエストを投げず、現在設定されているHTTPResponseをクライアントへ返す
            /// </summary>
            public bool BypassServerRequest { get; set; }
            
            /// <summary>
            /// HTTPレスポンス情報

            /// </summary>
            public class HTTPResponse
            {
                public HTTPResponse()
                {
                    StatusCode = 200;
                    ProtocolVersion = HttpVersion.Version10;
                    ContentType = "text/plain;charset=utf-8";
                    ContentBody = null;
                    _statusDescription = null;
                }

                /// <summary>
                /// 現在のHTTPレスポンスをHttpListenerResponseへ設定
                /// </summary>
                /// <param name="res"></param>
                public void UpdateResponse(HttpListenerResponse res)
                {
                    ContentType = res.ContentType;
                    StatusCode = res.StatusCode;
                    StatusDescription = res.StatusDescription;
                    ProtocolVersion = res.ProtocolVersion;
                }

                /// <summary>
                /// HTTPステータスコード
                /// </summary>
                public int StatusCode { get;set;}
                /// <summary>
                /// Content-Type
                /// </summary>
                public string ContentType { get;set;}
                /// <summary>
                /// HTTPプロトコルバージョン
                /// </summary>
                public Version ProtocolVersion;
                string _statusDescription;
                /// <summary>
                /// HTTPステータス情報、nullを設定するとステータスコードから引っ張った文字になる
                /// </summary>
                public string StatusDescription
                {
                    get
                    {
                        if (_statusDescription != null)
                            return _statusDescription;

                        switch (StatusCode)
                        {
                            case 200:
                                return "OK";
                            case 502:
                                return "Bad Gateway";
                            default:
                                return string.Format("Unknown({0})",StatusCode);
                        }
                    }
                    set { _statusDescription = value; }
                }

                /// <summary>
                /// レスポンスボディ
                /// </summary>
                public byte[] ContentBody { get; set; }
                /// <summary>
                /// レスポンスボディをUTF-8文字列と見なして処理
                /// </summary>
                public string ContentString
                {
                    get
                    {
                        if (ContentBody == null)
                            return "";

                        return Encoding.UTF8.GetString(ContentBody);
                    }
                    set { ContentBody = Encoding.UTF8.GetBytes(value); }
                }
            }

            /// <summary>
            /// HTTPリクエスト情報
            /// </summary>
            public class HTTPRequest
            {
                public HTTPRequest()
                {
                    Body = null;
                }

                public byte[] Body { get; set; }

                public string String
                {
                    get
                    {
                        if (Body == null)
                            return "";
                        return Uri.UnescapeDataString(Encoding.UTF8.GetString(Body));
                    }
                    set
                    {
                        Body = Encoding.UTF8.GetBytes(Uri.EscapeDataString(value));
                    }
                }

            }
        }
        public delegate void HTTProxyCallbackHandler(SessionInfo info);

        /// <summary>
        /// セッションが終了した際に呼ばれる
        /// </summary>
        public event HTTProxyCallbackHandler AfterSessionCompleted;

        /// <summary>
        /// プロキシがリクエストを受信し、サーバへ中継する前に呼ばれる
        /// </summary>
        public event HTTProxyCallbackHandler BeforeRequest;

        private IWebProxy _proxy = new WebProxy();

        /// <summary>
        /// サーバへリクエストを中継する際に使うプロキシ。nullでプロキシなし
        /// </summary>
        public IWebProxy UpstreamProxy
        {
            get
            {
                return _proxy;
            }
            set
            {
                if (value == null)
                    _proxy = new WebProxy();
                else
                    _proxy = value;
            }
        }

        /*
         * http://d.hatena.ne.jp/wwwcfe/20081228/1230470881
         * HttpListenerを使った簡単プロクシ
         * 
         */


        /*
         * Windows7等ではnetshを使ってprefixを設定しないといけない
         * http://ivis-mynikki.blogspot.jp/2011/02/nethttp.html
         * http://www.moonmile.net/blog/archives/6406
         * http://stackoverflow.com/questions/4019466/httplistener-access-denied
         * 
         * #netsh http add urlacl url=http://127.0.0.1:8881/ user=everyone
         */
        private HttpListener _listener = null;

        /// <summary>
        /// プロキシをスタートする
        /// </summary>
        /// <param name="port">listenするポート</param>
        /// <returns>失敗するとfalse</returns>
        public bool Start(int port)
        {
            if (!HttpListener.IsSupported)
            {
                DebugOut("Not supported platform");
                return false;
            }

            if (_listener != null)
            {
                DebugOut("Already started.");
                return false;
            }

            _listener = new HttpListener();
            string prefix = string.Format("http://{0}:{1}/", IPAddress.Loopback, port);
            _listener.Prefixes.Add(prefix);
            DebugOut("Add HttpListener prefix:{0}",prefix);
            try
            {
                _listener.Start();
            }
            catch(HttpListenerException ex)
            {
                DebugOut("Exception at starting HttpListener\n{0}", ex.ToString());
                return false;
            }


            //リクエストの到着を待つ
            _listener.BeginGetContext(OnHTTPRequest, _listener);

            return true;

        }

        /// <summary>
        /// 停止する
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if (_listener == null)
                return false;

            Dispose();
            return true;
        }

        /// <summary>
        /// オブジェクトを破棄
        /// </summary>
        public void Dispose()
        {
            if (_listener != null)
            {
                if(_listener.IsListening)
                    _listener.Stop();
                _listener.Close();
                _listener = null;
            }
        }

        /// <summary>
        /// リクエストが到着するたびに別スレッドで呼ばれる
        /// </summary>
        /// <param name="ar">非同期情報</param>
        private void OnHTTPRequest(IAsyncResult ar)
        {
            HttpListener listener = ar.AsyncState as HttpListener;
            if (!listener.IsListening)
            {
                DebugOut("not listening.");
                return;
            }

            //次のリクエストに備える
            listener.BeginGetContext(OnHTTPRequest, listener);

            HttpListenerContext ctx = null;
            try
            {
                //リクエストを処理
                ctx = _listener.EndGetContext(ar);
                if (ctx != null)
                {
                    HandleHTTPRequest(ctx);
                    ctx.Response.Close();
                }
            }
            catch (Exception ex)
            {
                DebugOut("Exception {0}\n{1}", ctx.Request.RawUrl, ex.ToString());
                if (ctx != null)
                    ctx.Response.Abort();
            }
        }

        /// <summary>
        /// HTTPリクエストを処理する
        /// </summary>
        /// <param name="ctx"></param>
        private void HandleHTTPRequest(HttpListenerContext ctx)
        {
            SessionInfo info = new SessionInfo();

            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse res = ctx.Response;

            // どこから接続されたかと、加工されていないアドレス
//            DebugOut("{3}) UserHost={0} Method={1} Request={2}",
//                req.UserHostAddress,req.HttpMethod, req.RawUrl,Thread.CurrentThread.ManagedThreadId);

            info.Uri = new Uri(req.RawUrl);

            HttpWebRequest webRequest = CreateHttpWebRequest(req);

            // ボディあったら送受信
            if (req.HasEntityBody) // リクエストのボディがある (POST とか)
            {
                if (AfterSessionCompleted != null && !info.Ignore)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        TeeStream(req.InputStream, webRequest.GetRequestStream(),ms);
                        info.Request.Body = ms.ToArray();
                    }
                }
                else
                {
                    CopyStream(req.InputStream, webRequest.GetRequestStream());
                }
            }

            if (BeforeRequest != null)
                BeforeRequest(info);

            //サーバへのリクエストを投げずクライアントへレスポンスを返すよう設定された。
            if (info.BypassServerRequest)
            {
                DebugOut("marked as bypassing server request");
                SendResponse(ctx.Response, info.Response);

                // AfterSessionCompleted は呼ばない

                return;
            }


            // レスポンス取得
            HttpWebResponse webResponse = null;
            string exMsg = "";
            try
            {
                webResponse = webRequest.GetResponse() as HttpWebResponse;
            }
            catch (WebException e)
            {
                webResponse = e.Response as HttpWebResponse;
                DebugOut("WebException {0} / GetResponse \n{1}",req.RawUrl, e.ToString());
                exMsg = e.Message;
            }

            // 失敗したら502を返す
            if (webResponse == null)
            {
                SendResponse(ctx.Response, new SessionInfo.HTTPResponse()
                {
                    StatusCode = 502,
                    ContentString = string.Format("Cannot connect {0} \n {1}",req.RawUrl,exMsg),
                });
                return;
            }

            SetHttpListenerResponse(ctx, webResponse);

            if (AfterSessionCompleted != null && !info.Ignore)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        TeeStream(webResponse.GetResponseStream(), res.OutputStream, ms);
                        webResponse.Close();
                        info.Response.UpdateResponse(res);
                        info.Response.ContentBody = ms.ToArray();
                    }
                    catch
                    {
                        DebugOut("Exception Tee: {0}" , req.RawUrl);
                        throw;
                    }
                }
                AfterSessionCompleted(info);
            }
            else
            {
                try
                {
                    CopyStream(webResponse.GetResponseStream(), res.OutputStream);
                    webResponse.Close();
                }
                catch
                {
                    DebugOut("Exception Relay: "+req.RawUrl);
                    throw;
                }
          }
        }

        /// <summary>
        /// HttpListenerRequestからWebRequestを生成
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private HttpWebRequest CreateHttpWebRequest(HttpListenerRequest req)
        {
            HttpWebRequest webRequest = WebRequest.Create(req.RawUrl) as HttpWebRequest;

            //リクエスト情報を設定
            webRequest.Method = req.HttpMethod;
            webRequest.ProtocolVersion = req.ProtocolVersion;
            webRequest.Proxy = _proxy;
//            webRequest.Timeout = 5000;
            webRequest.ContentType = req.ContentType;
            webRequest.UserAgent = req.UserAgent;

            if (req.UrlReferrer != null)
                webRequest.Referer = req.UrlReferrer.OriginalString;

            if(req.ContentLength64 > 0)
                webRequest.ContentLength = req.ContentLength64;
            
            //リダイレクトをブラウザで処理させる
            webRequest.AllowAutoRedirect = false;

            //gzip/deflateをプロキシ内で伸張する
            webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            // HttpWebRequest の制限がきついのでヘッダごとに対応
            foreach(var name in req.Headers.AllKeys)
            {
                string value = req.Headers[name];

                switch (name.ToLower())
                {
                    case "content-length":
                    case "content-type":
                    case "referer":
                    case "user-agent":
                    case "host":
                        //すでに設定済み
                        break;
                    case "accept":
                        webRequest.Accept = value;
                        break;
                    case "connection":
                    case "proxy-connection":
                        webRequest.KeepAlive = req.KeepAlive; // TODO: keepalive の取得はここで行う。
                        break;
                    case "if-modified-since":
                        webRequest.IfModifiedSince = DateTime.Parse(value);
                        break;
                    default:
                        try
                        {
                            //DebugOut(" ReqHeader[{0}]:[{1}]", name, value);
                            webRequest.Headers.Add(name, value);
                        }
                        catch(Exception ex)
                        {
                            DebugOut("Exception [{0}] [{1}]->[{2}]\n{3}",
                                req.RawUrl,name,value,ex.ToString());
                        }
                        break;
                }
            }

            return webRequest;
        }

        /// <summary>
        /// WebResponseをHttpListenerResponseへ設定する
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="webResponse"></param>
        void SetHttpListenerResponse(HttpListenerContext ctx, HttpWebResponse webResponse)
        {
            HttpListenerResponse res = ctx.Response;
            res.ProtocolVersion = webResponse.ProtocolVersion;
            res.StatusCode = (int)webResponse.StatusCode;
            res.StatusDescription = webResponse.StatusDescription;
            if (webResponse.ContentLength > 0)
                res.ContentLength64 = webResponse.ContentLength;
            if (webResponse.ContentType != null && webResponse.ContentType != "")
                res.ContentType = webResponse.ContentType;

            foreach (var name in webResponse.Headers.AllKeys)
            {
                string value = webResponse.Headers[name];

                switch (name.ToLower())
                {
                    case "content-length":
                    case "content-type":
                    case "keep-alive":
                        break;
                    case "transfer-encoding":
                        res.SendChunked = value.ToLower().IndexOf("chunked") >= 0 ? true : false;
                        break;
                    case "location":
                        res.RedirectLocation = value;
                        break;
                    case "connection":
                        if (value.ToLower() == "keep-alive")
                            res.KeepAlive = true;
                        break;
                    default:
                        try
                        {
                            //DebugOut(" ResHeader[{0}]:{1}", name, value);
                            res.Headers.Add(name, value);
                        }
                        catch (Exception ex)
                        {
                            DebugOut("Exception[{0}] [{1}]->[{2}]\n{3}",
                                ctx.Request.RawUrl, name, value, ex.ToString());
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// ストリームをコピー
        /// </summary>
        /// <param name="src">コピー元</param>
        /// <param name="dst">コピー先</param>
        private void CopyStream(Stream src, Stream dst)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ( (bytesRead = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                if(dst.CanWrite)
                    dst.Write(buffer, 0, bytesRead);
            }

            src.Close();
            dst.Close();
        }

        /// <summary>
        /// ストリームを二カ所へコピー
        /// </summary>
        /// <param name="src">コピー元</param>
        /// <param name="dst1">コピー先1</param>
        /// <param name="dst2">コピー先2</param>
        private void TeeStream(Stream src, Stream dst1, Stream dst2)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ( (bytesRead = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dst1.Write(buffer, 0, bytesRead);
                dst2.Write(buffer, 0, bytesRead);
            }

            src.Close();
            dst1.Close();
            dst2.Close();
        }

        /// <summary>
        /// HttpListenerResponeへレスポンスを書き出す
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="response"></param>
        private void SendResponse(HttpListenerResponse ctx, SessionInfo.HTTPResponse response)
        {
            ctx.StatusCode = response.StatusCode;
            ctx.StatusDescription = response.StatusDescription;
            ctx.ProtocolVersion = response.ProtocolVersion;
            ctx.ContentType = response.ContentType;
            ctx.KeepAlive = false;

            if (response.ContentBody != null)
            {
                ctx.ContentLength64 = response.ContentBody.Length;
                ctx.OutputStream.Write(response.ContentBody, 0, response.ContentBody.Length);
                ctx.OutputStream.Close();
            }
            else
            {
                ctx.ContentLength64 = 0;
            }
            DebugOut("SendResponse");
        }

        /// <summary>
        /// デバッグログ出力関数
        /// </summary>
        /// <param name="format">フォーマット</param>
        /// <param name="args">引数</param>
        [System.Diagnostics.Conditional("DEBUG")]
        private static void DebugOut(string format, params object[] args)
        {
            var st = new System.Diagnostics.StackTrace(false);
            string name = st.GetFrame(1).GetMethod().Name;
            System.Diagnostics.Debug.WriteLine(string.Format("HTTProxy::{0} {1}", name, string.Format(format, args)));
        }
    }
}
