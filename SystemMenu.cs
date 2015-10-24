using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace KCB2
{
    public class SystemMenu
    {
        [StructLayout(LayoutKind.Sequential)]
        struct MENUITEMINFO
        {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public IntPtr dwItemData;
            public string dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition,
          [In] ref MENUITEMINFO lpmii);


        const uint MFT_SEPARATOR = 0x00000800;
        const uint MFT_STRING = 0x00000000;

        const uint MIIM_FTYPE = 0x00000100;
        const uint MIIM_STRING = 0x00000040;
        const uint MIIM_ID = 0x00000002;

        const uint WM_SYSCOMMAND = 0x0112;

        IntPtr _hSysMenu;

        public SystemMenu(System.Windows.Forms.Form form)
        {
            _hSysMenu = GetSystemMenu(form.Handle, false);
        }

        public bool InsertMenuItem(uint menuId,string name,uint position)
        {
            MENUITEMINFO it = new MENUITEMINFO();
            it.cbSize = (uint)Marshal.SizeOf(it);
            it.fMask = MIIM_STRING | MIIM_ID;
            it.wID = menuId;
            it.dwTypeData = name;
            return InsertMenuItem(_hSysMenu, position, true, ref it);
        }

        public bool InsertSeparator(uint position)
        {
            MENUITEMINFO it = new MENUITEMINFO();
            it.cbSize = (uint)Marshal.SizeOf(it);
            it.fMask = MIIM_FTYPE;
            it.fType = MFT_SEPARATOR;
            return InsertMenuItem(_hSysMenu, position, true, ref it);
        }

        public static uint GetSysMenuId(System.Windows.Forms.Message msg)
        {
            if (msg.Msg != WM_SYSCOMMAND)
                return 0;

            return (uint)(msg.WParam.ToInt32() & 0xffff);
        }
    }
}
