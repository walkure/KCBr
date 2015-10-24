using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;

namespace KCB
{

    /// <summary>
    /// WebBrowserコントロールのスクリーンショット
    /// http://passing.breeze.cc/mt/archives/2005/09/web.html
    /// http://musi-chan.at.webry.info/200605/article_11.html
    /// http://homepage2.nifty.com/nonnon/SoftSample/CS.NET/SampleWebBitmap.html
    /// </summary>
    public class WebBrowserEx : WebBrowser, COM.IServiceProviderForIInternetSecurityManager,
        COM.IInternetSecurityManager
    {
        const int DVASPECT_CONTENT = 1;

        // ole32.dllのOleDrawを使用する(DllImport定義)
        [DllImport("ole32.dll")]
        extern static int OleDraw(
            IntPtr pUnknown,
            int dwAspect,
            IntPtr hdcDraw,
            ref Rectangle lprcBounds);

        /// <summary>
        /// WebBrowserのスクリーンショットを取る
        /// </summary>
        /// <returns>取得したビットマップ</returns>
        public Bitmap GetScreenShot()
        {
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                IntPtr hDC = g.GetHdc();
                IntPtr pUnknown = Marshal.GetIUnknownForObject(ActiveXInstance);
                try
                {
                    /* 
                     * OleDrawのrectに収まるよう、IViewObject::Draw内で縮小されてしまうので 
                     * 一部分のみのスクリーンショットは取れない。
                     */

                    OleDraw(pUnknown, DVASPECT_CONTENT, hDC, ref rect);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("OleDraw throws exception:" + ex.ToString());
                    return null;
                }
                finally
                {
                    g.ReleaseHdc(hDC);
                    Marshal.Release(pUnknown);
                }
            }
            return bmp;
        }

        [ComImport, Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E")]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        public interface IWebBrowser2
        {
            object Application { get; }
            object Document { get; set; }
        }

        [ComImport,GuidAttribute("6d5140c1-7436-11ce-8034-00aa006009fa"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),ComVisible(false)]
        public interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        }

        public static Guid IWebBrowser2_GUID = new Guid("D30C1661-CDAF-11D0-8A3E-00C04FC9E26E");
        public static Guid IWebBrowserApp_GUID = new Guid("0002DF05-0000-0000-C000-000000000046");

        /// <summary>
        /// http://www.divakk.co.jp/aoyagi/csharp_tips_wbzone.html
        /// </summary>

        #region IServiceProviderForIInternetSecurityManager メンバ
        int COM.IServiceProviderForIInternetSecurityManager.QueryService(ref Guid guidService, ref Guid riid, out COM.IInternetSecurityManager ppvObject)
        {
            ppvObject = null;
            if (guidService == COM.IID_IInternetSecurityManager)
            {
                ppvObject = this as COM.IInternetSecurityManager;
                return COM.S_OK;
            }
            return COM.E_NOINTERFACE;
        }
        #endregion

        #region IInternetSecurityManager メンバ
        int COM.IInternetSecurityManager.SetSecuritySite(COM.IInternetSecurityMgrSite pSite)
        {
            return COM.INET_E_DEFAULT_ACTION;
        }

        int COM.IInternetSecurityManager.GetSecuritySite(COM.IInternetSecurityMgrSite pSite)
        {
            return COM.INET_E_DEFAULT_ACTION;
        }

        int COM.IInternetSecurityManager.MapUrlToZone(String pwszUrl, out int pdwZone, int dwFlags)
        {
            pdwZone = 0;
            return COM.INET_E_DEFAULT_ACTION;
        }

        private const string m_strSecurity = "None:localhost+My Computer";
        int COM.IInternetSecurityManager.GetSecurityId(string pwszUrl, IntPtr pbSecurityId, ref uint pcbSecurityId, uint dwReserved)
        {
            byte[] by = System.Text.Encoding.ASCII.GetBytes(m_strSecurity);
            Marshal.Copy(by, 0, pbSecurityId, by.Length);
            pcbSecurityId = (uint)m_strSecurity.Length;

            //これでいけるっぽい。どんなゾーンになるの？？？？
            //これだとXPマシンで凍ったりした
//            pcbSecurityId = 0;
            return COM.S_OK;
        }

        int COM.IInternetSecurityManager.ProcessUrlAction(String pwszUrl, int dwAction, out byte pPolicy, int cbPolicy, byte pContext, int cbContext, int dwFlags, int dwReserved)
        {
            pPolicy = 0;
            return COM.INET_E_DEFAULT_ACTION;
        }

        int COM.IInternetSecurityManager.QueryCustomPolicy(String pwszUrl, ref Guid guidKey, byte ppPolicy, int pcbPolicy, byte pContext, int cbContext, int dwReserved)
        {
            return COM.INET_E_DEFAULT_ACTION;
        }

        int COM.IInternetSecurityManager.SetZoneMapping(int dwZone, String lpszPattern, int dwFlags)
        {
            return COM.INET_E_DEFAULT_ACTION;
        }

        int COM.IInternetSecurityManager.GetZoneMappings(int dwZone, out IEnumString  ppenumString, int dwFlags)
        {
            ppenumString = null;
            return COM.INET_E_DEFAULT_ACTION;
        }
        #endregion


        public struct INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        };

    }
}
