using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using KCB;

namespace KCB2
{
    public partial class FormShipDetail : Form
    {
        public MemberData.Ship.Info Info { get; set; }

        ImageList _imgSlotItem;

        public FormShipDetail(ImageList imgSlotItem)
        {
            Info = null;
            _imgSlotItem = imgSlotItem;
            InitializeComponent();
        }

        private void FormShipDetail_Load(object sender, EventArgs e)
        {
            if (Info == null)
                Close();

            lvStatus.Columns[1].Text = string.Format("{0} Lv.{1} {2}exp"
                , Info.ShipName, Info.Level, Info.Experience);

            //string listItem = "火力,対空,装甲,雷装,回避,索敵,対潜,HP,燃料,弾薬,射程,速力,運,装備";
//            string listItem = "火力,対空,装甲,雷装,HP,燃料,装備";
            string listItem = Properties.Settings.Default.ShipDetailItem;

            var itemParams = new LVItemParamsFactory(Info,_imgSlotItem);
            foreach (var itemType in listItem.Split(','))
            {
                Debug.WriteLine(string.Format("addItem type:{0}", itemType));
                itemParams.AddLVItem(itemType, lvStatus.Items);
            }

            lvStatus.LoadColumnWithOrder(Properties.Settings.Default.ShipDetailColumnWidth);

        }

        /// <summary>
        /// リストビューアイテムを生成するクラス
        /// </summary>
        class LVItemParamsFactory
        {
            MemberData.Ship.Info _info;
            ImageList _imgSlotItem;

            public LVItemParamsFactory(MemberData.Ship.Info shipInfo, ImageList imgSlotItem)
            {
                _pwupParams = new SlotItemPowerupParams(shipInfo.SlotItem);
                _imgSlotItem = imgSlotItem;
                _info = shipInfo;
            }

            /// <summary>
            /// 指定アイテムを追加する
            /// </summary>
            /// <param name="itemType">リストアイテム種別</param>
            /// <param name="lvItems">追加するリストビューアイテムコレクション</param>
            public void AddLVItem(string itemType, ListView.ListViewItemCollection lvItems)
            {
                switch (itemType)
                {
                    case "火力":
                    case "対空":
                    case "装甲":
                    case "雷装":
                    case "回避":
                    case "索敵":
                    case "対潜":
                    case "HP":
                    case "燃料":
                    case "弾薬":
                    case "運":
                    case "速力":
                    case "射程":
                        var lvit = GetLVItem(itemType);
                        if (lvit != null)
                            lvItems.Add(lvit);
                        return;
                    case "装備":
                        AddSlotItems(lvItems);
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(string.Format("not defined type:[{0}]",itemType));
//                        return;

                }

            }

            /// <summary>
            /// 指定名のパラメタを取得
            /// </summary>
            /// <param name="itemType">パラメタ名称</param>
            /// <returns>文字列</returns>
            ListViewItem GetLVItem(string itemType)
            {
                switch (itemType)
                {
                    case "火力":
                        return new NowMaxPaddingLVItem("火力", _info.Fire, _pwupParams.火力);
                    case "対空":
                        return new NowMaxPaddingLVItem("対空", _info.AntiAir, _pwupParams.対空);
                    case "装甲":
                        return new NowMaxPaddingLVItem("装甲", _info.Armor, _pwupParams.装甲);
                    case "雷装":
                        return new NowMaxPaddingLVItem("雷装", _info.Torpedo, _pwupParams.雷撃);

                    case "回避":
                        return new NowMaxPaddingMinusLVItem("回避", _info.Escape, _pwupParams.回避);
                    case "索敵":
                        return new NowMaxPaddingMinusLVItem("索敵", _info.Search, _pwupParams.索敵);
                    case "対潜":
                        return new NowMaxPaddingMinusLVItem("対潜", _info.AntiSubm, _pwupParams.対潜);

                    case "HP":
                        return new HPLVItem(_info);

                    case "燃料":
                        return new NowMaxColorLVItem("燃料", _info.Fuel);
                    case "弾薬":
                        return new NowMaxColorLVItem("弾薬", _info.Bullet);

                    case "運":
                        return new ListViewItem(new string[] { "運", _info.Lucky.ToString() });
                    case "速力":
                        return new ListViewItem(new string[] { "速力", _info.SpeedString });
                    case "射程":
                        return new ListViewItem(new string[] { "射程", _info.ShotRangeString });

                }

                throw new ArgumentOutOfRangeException(string.Format("not defined type2:[{0}]", itemType));

            }

            /// <summary>
            /// 装備情報を追加
            /// </summary>
            /// <param name="lvItems"></param>
            void AddSlotItems(ListView.ListViewItemCollection lvItems)
            {
                for (int n = 0; n < _info.SlotNum; n++)
                {
                    string name = string.Format("装備{0}", n + 1);

                    //有効スロットの場合
                    if (_info.SlotItem.Count > n)
                    {
                        //艦載機の場合
                        if (_info.SlotItem[n].Count > 0)
                        {
                            name = string.Format("装備{0} x{1}", n + 1, _info.SlotItem[n].Count);
                        }
                        
                        lvItems.Add(new SlotItemLVItem(name,_info.SlotItem[n],_imgSlotItem));

                    }
                    else
                    {   
                        lvItems.Add(new ListViewItem(new string[] { name, "(装備不可)" }));
                    }
                }

            }

            /// <summary>
            /// 現在値/最大値(+加算分)
            /// </summary>
            class NowMaxPaddingLVItem : ListViewItem
            {
                public NowMaxPaddingLVItem(string title,MemberData.Ship.Info.NowMax nowMax, int padding)
                {
                    Text = title;
                    if (padding > 0)
                        SubItems.Add(string.Format("{0} (+{1})", nowMax.ToString(), padding));
                    else if(padding < 0)
                        SubItems.Add(string.Format("{0} ({1})", nowMax.ToString(), padding));
                    else
                        SubItems.Add(nowMax.ToString());
                }
            }

            /// <summary>
            /// 現在値-加算分/最大値　(+加算分)
            /// </summary>
            class NowMaxPaddingMinusLVItem : ListViewItem
            {
                public NowMaxPaddingMinusLVItem(string title, MemberData.Ship.Info.NowMax nowMax, int padding)
                {
                    Text = title;

                    MemberData.Ship.Info.NowMax nowMax2 = new MemberData.Ship.Info.NowMax(nowMax);
                    nowMax2.Now -= padding;

                    if (padding > 0)
                        SubItems.Add(string.Format("{0} (+{1})", nowMax2.ToString(), padding));
                    else if (padding < 0)
                        SubItems.Add(string.Format("{0} ({1})", nowMax2.ToString(), padding));
                    else
                        SubItems.Add(nowMax.ToString());
                }
            }

            /// <summary>
            /// 現在値/最大値 +背景色
            /// </summary>
            class NowMaxColorLVItem : ListViewItem
            {
                public NowMaxColorLVItem(string title, MemberData.Ship.Info.NowMax nowMax)
                {
                    Text = title;
                    UseItemStyleForSubItems = false;
                    SubItems.Add(nowMax.ToString());
                    SubItems[1].BackColor = nowMax.BackgroundColor;
                }
            }

            /// <summary>
            /// HP(入渠時間)
            /// </summary>
            class HPLVItem : ListViewItem
            {
                public HPLVItem(MemberData.Ship.Info shipInfo)
                {
                    Text = "HP";
                    UseItemStyleForSubItems = false;

                    string valstr = shipInfo.HP.ToString();
                    if (shipInfo.RepairTime.TotalSeconds > 0)
                        valstr += string.Format(" (入渠所要:{0})", shipInfo.RepairTimeString);

                    SubItems.Add(valstr);
                    SubItems[1].BackColor = shipInfo.HP.BackgroundColor;

                }
            }

            /// <summary>
            /// 装備
            /// </summary>
            class SlotItemLVItem : ListViewItem
            {
                public SlotItemLVItem(string name,MemberData.Ship.Info.SlotItemInfo slotItem,ImageList imgSlotItem)
                {
                    Text = name;
                    SubItems.Add(new SlotItemNameLVSubItem(slotItem, imgSlotItem));
                }

                /// <summary>
                /// 装備種別アイコンを描画する装備名サブアイテム
                /// </summary>
                class SlotItemNameLVSubItem : ListViewItem.ListViewSubItem, IOwnerDrawLVSubItem
                {
//                    ImageList imgSlotItem;
                    int slotItemIndex;
                    Image imageNormal, imageSelected;
                    public SlotItemNameLVSubItem(MemberData.Ship.Info.SlotItemInfo slotItem,ImageList imgSlotItem)
                    {
//                        this.imgSlotItem = imgSlotItem;

                        imageNormal = new Bitmap(16, 16);
                        imageSelected = new Bitmap(16, 16);

                        if (slotItem == null)
                        {
                            Text = "(未装備)";
                            slotItemIndex = 0;
                            
                        }
                        else
                        {
                            if(slotItem.Level > 0)
                                Text = string.Format("[{1}]{0}", slotItem.Name  ,slotItem.Level);
                            else
                                Text = slotItem.Name;

                            slotItemIndex = slotItem.TypeNum;
                            if (slotItemIndex < 0)
                                slotItemIndex = 0;
                        }

                        DrawSlotItemImage(imgSlotItem, imageNormal, SystemBrushes.Window, slotItemIndex);
                        DrawSlotItemImage(imgSlotItem, imageSelected, SystemBrushes.Highlight, slotItemIndex);

                    }

                    /// <summary>
                    /// バッファに装備アイテム画像を描画する
                    /// </summary>
                    /// <param name="iconImageList"></param>
                    /// <param name="canvas"></param>
                    /// <param name="backBrush"></param>
                    /// <param name="itemIndex"></param>
                    void DrawSlotItemImage(ImageList iconImageList, Image canvas, Brush backBrush,int itemIndex)
                    {
                        using (Graphics g = Graphics.FromImage(canvas))
                        {
                            g.FillRectangle(backBrush,
                                new Rectangle(new Point(0, 0), canvas.Size));

                            //装着されてるスロット かつ 装備情報読み込み済み
                            if (itemIndex < iconImageList.Images.Count)
                                iconImageList.Draw(g, new Point(1, 0), itemIndex);
                            else
                                iconImageList.Draw(g, new Point(1, 0), 0);
                            
                        }
                    }

                    /// <summary>
                    /// サブアイテム描画
                    /// </summary>
                    /// <param name="e"></param>
                    public void DrawSubItem(DrawListViewSubItemEventArgs e)
                    {

                        Brush textBrush;
                        e.Graphics.SetClip(e.Bounds);


                        if (e.ItemState.HasFlag(ListViewItemStates.Selected))
                        {
                            e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                            e.Graphics.DrawImage(imageSelected, e.Bounds.X,e.Bounds.Y);
                            textBrush = SystemBrushes.HighlightText;
                        }
                        else
                        {
                            e.DrawBackground();
                            e.Graphics.DrawImage(imageNormal, e.Bounds.X, e.Bounds.Y);
                            textBrush = SystemBrushes.WindowText;
                        }

//                        imgSlotItem.Draw(e.Graphics, new Point(e.Bounds.X + 1, e.Bounds.Y), slotItemIndex);

                        using (StringFormat sf = new StringFormat())
                        {
                            sf.LineAlignment = StringAlignment.Center;
//                            sf.Trimming = StringTrimming.EllipsisCharacter;
                            sf.FormatFlags = StringFormatFlags.NoWrap;

                            e.Graphics.DrawString(Text, SystemFonts.DialogFont, textBrush,
                                new RectangleF(e.Bounds.X + 17, e.Bounds.Y, e.Bounds.Width 
                                    - 16, e.Bounds.Height), sf);
                        }

                    }
                }
            }

            SlotItemPowerupParams _pwupParams;

            /// <summary>
            /// 装備による加算分を算出するクラス
            /// </summary>
            class SlotItemPowerupParams
            {
                public int 火力 { get; private set; }
                public int 対空 { get; private set; }
                public int 装甲 { get; private set; }
                public int 雷撃 { get; private set; }
                public int 回避 { get; private set; }
                public int 索敵 { get; private set; }
                public int 対潜 { get; private set; }

                public SlotItemPowerupParams(IEnumerable<MemberData.Ship.Info.SlotItemInfo> slotItemList)
                {
                    火力 = 0;
                    対空 = 0;
                    装甲 = 0;
                    雷撃 = 0;
                    回避 = 0;
                    索敵 = 0;
                    対潜 = 0;

                    foreach (var slotItem in slotItemList)
                    {
                        var item = slotItem.Info;
                        if (item == null)
                            return;

                        火力 += item.火力;
                        対空 += item.対空;
                        装甲 += item.装甲;
                        雷撃 += item.雷撃;
                        回避 += item.砲撃回避;
                        索敵 += item.索敵;
                        対潜 += item.対潜;
                    }
                }
            }
        }

        #region ウィンドウ状態変化系
        private void FormShipDetail_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        private void lvStatus_MouseDown(object sender, MouseEventArgs e)
        {
            //右クリックでサイズ可変化
            if (e.Button != System.Windows.Forms.MouseButtons.Right)
                return;

            Point curLoc = Location;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            Location = curLoc;
        }

        private void FormShipDetail_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.ShipDetailSize = lvStatus.Size;
            Properties.Settings.Default.ShipDetailColumnWidth = lvStatus.SaveColumnWithOrder();
        }
        #endregion


    }
}
