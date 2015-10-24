using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;

namespace KCB
{
    /// <summary>
    /// http://www.divakk.co.jp/aoyagi/COM.cs.txt
    /// から不要な部分を削除
    /// </summary>
    /// 
    public class COM
    {
        public const int S_OK = unchecked((int)0x00000000);
        public const int S_FALSE = unchecked((int)0x00000001);
        public const int E_NOINTERFACE = unchecked((int)0x80004002);
        public const int INET_E_DEFAULT_ACTION = unchecked((int)0x800C0011);

        public static Guid IID_IProfferService = new Guid("cb728b20-f786-11ce-92ad-00aa00a74cd0");
        public static Guid SID_SProfferService = new Guid("cb728b20-f786-11ce-92ad-00aa00a74cd0");
        public static Guid IID_IInternetSecurityManager = new Guid("79eac9ee-baf9-11ce-8c82-00aa004ba90b");

        [ComImport,
        GuidAttribute("6d5140c1-7436-11ce-8034-00aa006009fa"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)]
        public interface IServiceProvider
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        }

        [ComImport,
        GuidAttribute("cb728b20-f786-11ce-92ad-00aa00a74cd0"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)]
        public interface IProfferService
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ProfferService(ref Guid guidService, COM.IServiceProviderForIInternetSecurityManager psp, ref int cookie);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int RevokeService(int cookie);
        }


        [ComImport,
        GuidAttribute("6d5140c1-7436-11ce-8034-00aa006009fa"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)]
        public interface IServiceProviderForIInternetSecurityManager
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out COM.IInternetSecurityManager ppvObject);
        }

        [ComImport,
        GuidAttribute("79eac9ed-baf9-11ce-8c82-00aa004ba90b"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)]
        public interface IInternetSecurityMgrSite
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetWindow(out IntPtr hwnd);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int EnableModeless([In, MarshalAs(UnmanagedType.Bool)] Boolean fEnable);
        }


        [ComImport, GuidAttribute("79eac9ee-baf9-11ce-8c82-00aa004ba90b"),
        InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),
        ComVisible(false)]
        public interface IInternetSecurityManager
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetSecuritySite([In] IInternetSecurityMgrSite pSite);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetSecuritySite([Out] IInternetSecurityMgrSite pSite);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int MapUrlToZone([In, MarshalAs(UnmanagedType.LPWStr)] String pwszUrl, out int pdwZone, int dwFlags);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
//            int GetSecurityId([MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, [MarshalAs(UnmanagedType.LPArray)] byte[] pbSecurityId, ref uint pcbSecurityId, uint dwReserved);
            int GetSecurityId([MarshalAs(UnmanagedType.LPWStr)] string pwszUrl, IntPtr pbSecurityId, ref uint pcbSecurityId, uint dwReserved);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int ProcessUrlAction([In, MarshalAs(UnmanagedType.LPWStr)] String pwszUrl, int dwAction, out byte pPolicy, int cbPolicy, byte pContext, int cbContext, int dwFlags, int dwReserved);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryCustomPolicy([In, MarshalAs(UnmanagedType.LPWStr)] String pwszUrl, ref Guid guidKey, byte ppPolicy, int pcbPolicy, byte pContext, int cbContext, int dwReserved);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int SetZoneMapping(int dwZone, [In, MarshalAs(UnmanagedType.LPWStr)] String lpszPattern, int dwFlags);

            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetZoneMappings(int dwZone, out IEnumString ppenumString, int dwFlags);
        }
    }
}
