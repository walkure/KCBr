using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCB2
{
    public class ListViewEx : ListView
    {
        public ListViewEx()
        {
            OwnerDraw = true;
            DoubleBuffered = true;
            DrawSubItem += this_DrawSubItem;
            DrawColumnHeader += this_DrawColumnHeader;
        }

        void this_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            //これがないと描画されない。
            e.DrawDefault = true;
        }

        void this_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.SubItem is IOwnerDrawLVSubItem)
                //描画をカスタマイズするサブアイテムの場合は描画を呼ぶ
                ((IOwnerDrawLVSubItem)e.SubItem).DrawSubItem(e);
            else
                //そうでなければシステムに描画を任せる
                e.DrawDefault = true;
        }

    }

    public interface IOwnerDrawLVSubItem
    {
        /// <summary>
        /// サブアイテム描画ハンドラ
        /// </summary>
        /// <param name="e"></param>
        void DrawSubItem(DrawListViewSubItemEventArgs e);
    }

}
