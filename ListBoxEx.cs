using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace KCB2
{
    public class ListBoxEx : ListBox
    {
        /*
         * 背景色をオーナードローする
         *
         */


        /// <summary>
        /// リストボックスに表示されるアイテム
        /// </summary>
        public class ListBoxItem
        {
            public ListBoxItem()
            {
                Text = "";
                ToolTip = null;
                BackColor = SystemColors.Window;
                Color = SystemColors.WindowText;
            }

            /// <summary>
            /// 非選択時の背景色
            /// </summary>
            public Color BackColor { get; set; }
            /// <summary>
            /// 非選択時の文字色
            /// </summary>
            public Color Color { get; set; }
            /// <summary>
            /// 文字列
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// ツールチップ
            /// </summary>
            public string ToolTip { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        public ListBoxEx() : base()
        {
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;

            _currentItemSet = false;
            _toolTipDisplayed = false;
            _toolTipDisplayTimer = new Timer();
            _toolTip = new ToolTip();

            _toolTipDisplayTimer.Interval = SystemInformation.MouseHoverTime;
            _toolTipDisplayTimer.Tick += _toolTipDisplayTimer_Tick;
        }

        /// <summary>
        /// 背景色・文字色を変更するオーナードロー
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            e.DrawBackground();
            if (e.Index > -1 && Items.Count > e.Index)
            {
                ListBoxItem item = Items[e.Index] as ListBoxItem;
                Brush b = null;

                if(e.State.HasFlag(DrawItemState.Selected))
                    //選ばれたアイテムは色をシステム規定で塗る。
                    b = new SolidBrush(e.ForeColor);
                else
                {
                    //選ばれなかったアイテム

                    // 分けるの面倒なのでSystemBrushesは使わない。SystemBrushedをdisposeしたら何が起きる？
                    if (item == null)
                        b = new SolidBrush(e.ForeColor);
                    else
                        b = new SolidBrush(item.Color);

                    //背景を塗りつぶす
                    if (item != null)
                        using (Brush bb = new SolidBrush(item.BackColor))
                            e.Graphics.FillRectangle(bb, e.Bounds);
                }


                string txt = Items[e.Index].ToString();

                e.Graphics.DrawString(txt, e.Font, b, e.Bounds);

                b.Dispose();
            }

            e.DrawFocusRectangle();

        }

        public override void Refresh()
        {
            // Create a Graphics object to use when determining the size of the largest item in the ListBox.
            Graphics g = CreateGraphics();

            // Determine the size for HorizontalExtent using the MeasureString method using the last item in the list. 
            int hzSize = 0;

            foreach(var item in Items)
                hzSize = Math.Max(hzSize,(int)g.MeasureString(item.ToString(), Font).Width);

            //微妙に足らない。
            hzSize += 4;
            // Set the HorizontalExtent property.
            HorizontalExtent = hzSize;

            base.Refresh();
        }

        /*
         * 
         * tooltip表示
         * http://www.codeproject.com/Articles/457444/Listbox-Control-with-Tooltip-for-Each-Item
         * より。
         * 
         */

        int _currentItem;
        bool _currentItemSet;
        bool _toolTipDisplayed;
        Timer _toolTipDisplayTimer;
        ToolTip _toolTip;

        void _toolTipDisplayTimer_Tick(object sender, EventArgs e)
        {
            // Display tooltip text since the mouse has hovered over an item
            if (!_toolTipDisplayed && _currentItem != ListBox.NoMatches &&
                        _currentItem < this.Items.Count)
            {
                ListBoxItem toolTipDisplayer = this.Items[_currentItem] as ListBoxItem;
                if (toolTipDisplayer != null)
                {
                    _toolTip.SetToolTip(this, toolTipDisplayer.ToolTip);
                    _toolTipDisplayed = true;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Get the item that the mouse is currently over
            Point cursorPoint = Cursor.Position;
            cursorPoint = this.PointToClient(cursorPoint);
            int itemIndex = this.IndexFromPoint(cursorPoint);

            if (itemIndex == ListBox.NoMatches)
            {
                // Mouse is over empty space in the listbox so hide tooltip
                _toolTip.Hide(this);
                _currentItemSet = false;
                _toolTipDisplayed = false;
                _toolTipDisplayTimer.Stop();
            }
            else if (!_currentItemSet)
            {
                // Mouse is over a new item so start timer to display tooltip
                _currentItem = itemIndex;
                _currentItemSet = true;
                _toolTipDisplayTimer.Start();
            }
            else if (itemIndex != _currentItem)
            {
                // Mouse is over a different item so hide tooltip and restart timer
                _currentItem = itemIndex;
                _toolTipDisplayTimer.Stop();
                _toolTipDisplayTimer.Start();
                _toolTip.Hide(this);
                _toolTipDisplayed = false;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _currentItemSet = false;
            _toolTipDisplayed = false;
            _toolTipDisplayTimer.Stop();
        }
    }
}
