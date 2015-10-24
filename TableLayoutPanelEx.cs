using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace KCB2
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TableLayoutPanelEx
    {
        public static int GetRowFromPoint(this TableLayoutPanel tableControl,
            System.Drawing.Point pt)
        {
            int[] rows = tableControl.GetRowHeights();
            //横列は気にしないnode
            //int[] cols = tableLayoutPanel1.GetColumnWidths();

            int row = -1;

            int bottom = 0;
            int top = 0;

            int y = pt.Y;

            for (int n = 0; n < rows.Count(); n++)
            {
                if (n > 0)
                    bottom += rows[n - 1];
                top += rows[n];

                //                System.Diagnostics.Debug.WriteLine(string.Format("{0} < {1} < {2}", bottom, y, top));
                if (bottom <= y && y < top)
                {
                    row = n;
                    break;
                }
            }

            return row;
        }

        public static void DoubleBuffer(this TableLayoutPanel tableControl, bool bUseDoubleBuffer)
        {
            Type typTarget = typeof(TableLayoutPanel);
            System.Reflection.PropertyInfo propInfo =
                typTarget.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);
            propInfo.SetValue(tableControl, bUseDoubleBuffer, null);
        }

    }
}
