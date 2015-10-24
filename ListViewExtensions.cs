using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace KCB
{
    /// <summary>
    /// リストビューの拡張メソッド
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ListViewExtensions
    {
        #region リストビューの矢印描画
        /// <summary>
        /// http://stackoverflow.com/questions/254129/how-to-i-display-a-sort-arrow-in-the-header-of-a-list-view-column-using-c
        /// </summary>
        /// 
        [StructLayout(LayoutKind.Sequential)]
        public struct HDITEM
        {
            public Mask mask;
            public int cxy;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public Format fmt;
            public IntPtr lParam;
            // _WIN32_IE >= 0x0300 
            public int iImage;
            public int iOrder;
            // _WIN32_IE >= 0x0500
            public uint type;
            public IntPtr pvFilter;
            // _WIN32_WINNT >= 0x0600
            public uint state;

            [Flags]
            public enum Mask
            {
                Format = 0x4,       // HDI_FORMAT
            };

            [Flags]
            public enum Format
            {
                SortDown = 0x200,   // HDF_SORTDOWN
                SortUp = 0x400,     // HDF_SORTUP
            };
        };

        public const int LVM_FIRST = 0x1000;
        public const int LVM_GETHEADER = LVM_FIRST + 31;

        public const int HDM_FIRST = 0x1200;
        public const int HDM_GETITEM = HDM_FIRST + 11;
        public const int HDM_SETITEM = HDM_FIRST + 12;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, ref HDITEM lParam);

        /// <summary>
        /// カラムヘッダにソート矢印を描画
        /// </summary>
        /// <param name="listViewControl"></param>
        /// <param name="columnIndex"></param>
        /// <param name="order"></param>
        public static void SetSortIcon(this ListView listViewControl,
            int columnIndex, SortOrder order)
        {
            IntPtr columnHeader = SendMessage(listViewControl.Handle,
                LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
            for (int columnNumber = 0; columnNumber <= listViewControl.Columns.Count - 1; columnNumber++)
            {
                var columnPtr = new IntPtr(columnNumber);
                var item = new HDITEM
                {
                    mask = HDITEM.Mask.Format
                };

                if (SendMessage(columnHeader, HDM_GETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                if (order != SortOrder.None && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            item.fmt &= ~HDITEM.Format.SortDown;
                            item.fmt |= HDITEM.Format.SortUp;
                            break;
                        case SortOrder.Descending:
                            item.fmt &= ~HDITEM.Format.SortUp;
                            item.fmt |= HDITEM.Format.SortDown;
                            break;
                    }
                }
                else
                {
                    item.fmt &= ~HDITEM.Format.SortDown & ~HDITEM.Format.SortUp;
                }

                if (SendMessage(columnHeader, HDM_SETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }
            }
        }

        #endregion

        #region リストビューのカラム幅保存/反映
        /// <summary>
        /// リストビューのカラム幅及び配置を読み込み
        /// </summary>
        /// <param name="listViewControl"></param>
        /// <param name="config"></param>
        public static void LoadColumnWithOrder(this ListView listViewControl, string config)
        {
            Debug.WriteLine("LoadColumnWithOrder format->" + config+"<");

            if (config == null || config.Length == 0)
                return;

            string[] cols = config.Split(new char[] { ' ' });

            //カラム数が変わってたら保存されてたデータは無視する
            if (cols.Count() != listViewControl.Columns.Count)
            {
                Debug.WriteLine(string.Format("columncount data:{0} actually:{1} data ignored",
                    cols.Count(), listViewControl.Columns.Count));
                return;
            }

            for(int i = 0 ; i < cols.Count() ; i++)
            {
                string[] k_v = cols[i].Split(new char[] { ':' });
                Debug.Assert(k_v.Count() == 2,"invalid k-v");
                int index = int.Parse(k_v[0]);
                int width = int.Parse(k_v[1]);

                ColumnHeader hdr = listViewControl.Columns[i];
                hdr.DisplayIndex = index;
                hdr.Width = width;
            }
        }

        /// <summary>
        /// リストビューのカラム幅及び配置を保存する文字列を生成
        /// </summary>
        /// <param name="listViewControl"></param>
        /// <returns></returns>
        public static string SaveColumnWithOrder(this ListView listViewControl)
        {
            List<string> hdrs = new List<string>();
            foreach (ColumnHeader hdr in listViewControl.Columns)
            {
                hdrs.Add(string.Format("{0}:{1}", hdr.DisplayIndex, hdr.Width));
            }
            string retval = string.Join(" ", hdrs.ToArray());
            Debug.WriteLine("SaveColumnWithOrder ->" + retval+"<");

            return retval;
        }
#endregion

        /// <summary>
        /// ダブルバッファリングプロパティを変更
        /// </summary>
        /// <param name="listViewControl"></param>
        /// <param name="bUseDoubleBuffer"></param>
        public static void DoubleBuffer(this ListView listViewControl, bool bUseDoubleBuffer)
        {
            Type typListView = typeof(ListView);
            System.Reflection.PropertyInfo propInfo =
                typListView.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);
            propInfo.SetValue(listViewControl, bUseDoubleBuffer, null);
        }

    }
}
