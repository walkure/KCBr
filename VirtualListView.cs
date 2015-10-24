using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;

namespace KCB2
{
    /// <summary>
    /// 仮想リストビュー管理クラスに対応するアイテムが実装するインタフェイス
    /// </summary>
    public interface IVirtualListViewItem
    {
        /// <summary>
        /// リストビューアイテム生成
        /// </summary>
        /// <returns></returns>
        ListViewItem CreateVirtualListViewItem();
    }

    /// <summary>
    /// 仮想リストビュー管理クラス
    /// </summary>
    /// <typeparam name="TLogInfoClass">IVirtualListViewItemを実装するリストビューアイテム情報クラス。</typeparam>
    public class VirtualListViewManager<TLogInfoClass> where TLogInfoClass : IVirtualListViewItem
    {
        System.Windows.Forms.ListView _lvTarget = null;
        IList<TLogInfoClass> _itemList = null;
        bool _bReverse;

        public VirtualListViewManager() { _bReverse = false; }

        /// <summary>
        /// 表示順をリストの順番と逆転するかどうか指定してオブジェクトを生成
        /// </summary>
        /// <param name="bReverse">逆転して表示する場合はtrue</param>
        public VirtualListViewManager(bool bReverse) { _bReverse = bReverse; }

        /// <summary>
        /// リストビューを仮想化してアイテムリストと結びつける
        /// </summary>
        /// <param name="lvTarget">対象のリストビュー</param>
        /// <param name="itemList">アイテムリスト</param>
        /// <returns></returns>
        public bool Attach(ListView lvTarget, List<TLogInfoClass> itemList)
        {
            if (lvTarget == null)
                throw new ArgumentNullException("lvTarget must be valid object");

            if (lvTarget.VirtualMode)
            {
                Debug.WriteLine("AlreadyVLMode");
                return false;
            }
            if (_lvTarget != null)
            {
                Debug.WriteLine("AlreadyAttached");
                return false;
            }
            lvTarget.VirtualMode = true;
            lvTarget.RetrieveVirtualItem += RetrieveVirtualItem;
            lvTarget.CacheVirtualItems += CacheVirtualItems;

            if (itemList != null)
                
                lvTarget.VirtualListSize = itemList.Count;
            else
                lvTarget.VirtualListSize = 0;

            _itemList = itemList; 
            _lvTarget = lvTarget;

            return true;
        }

        /// <summary>
        /// リストビューに取り敢えずアタッチし、あとからリストを与える
        /// </summary>
        /// <param name="lvTarget">アタッチするListView</param>
        /// <returns></returns>
        public bool Attach(ListView lvTarget)
        {
            return Attach(lvTarget, null);
        }

        /// <summary>
        /// 関連付けされているリストビューを外す
        /// </summary>
        /// <returns></returns>
        public bool Detach()
        {
            if (_lvTarget == null)
            {
                Debug.WriteLine("AlreadyDetached");
                return false;
            }

            _lvTarget.VirtualMode = false;
            _lvTarget.RetrieveVirtualItem -= RetrieveVirtualItem;
            _lvTarget.CacheVirtualItems -= CacheVirtualItems;

            _lvTarget = null;
            _itemList = null;

            _firstItem = -1;
            _itemCache = null;
            return true;
        }

        /// <summary>
        /// 新しいリストを与える
        /// </summary>
        /// <param name="newItemList">新しいリスト</param>
        public void UpdateList(IList<TLogInfoClass> newItemList)
        {
            /*
             * 更新したのでキャッシュをクリア。これを忘れるとキャッシュの再ビルドが
             * 呼ばれるまで追加アイテムが表示されない
             */

            _firstItem = -1;
            _itemCache = null;

            if (newItemList != null)
                _itemList = newItemList;

            int itemCount = 0;
            if(_itemList != null)
                itemCount = _itemList.Count;

            /*
             * 要Invoke
             * どうもVirtualListSize.setを叩くと処理返す前にRetrieveVirtualItemが呼ばれる様子
             * なので、if全体をlockでくるむとアイテム追加時にデッドロックする。
             *  -> UpdateList()はワーカースレッド、RetrieveVirtualItemはUIスレッド
             */
            if (_lvTarget.InvokeRequired)
                _lvTarget.Invoke((MethodInvoker)(() =>
                {
                    _lvTarget.VirtualListSize = itemCount;
/*                    if (itemCount > 0)
                    {
                        int startIndex = _lvTarget.TopItem == null ? 0 : _lvTarget.TopItem.Index;
                        int endIndex = Math.Min(startIndex + 100, itemCount - 1);
                        _lvTarget.RedrawItems(startIndex, endIndex, true);
                    }*/
                }));
            else
            {
                _lvTarget.VirtualListSize = itemCount;
            }

        }

        /// <summary>
        /// リスト数が更新されたことを伝える
        /// </summary>
        public void UpdateList()
        {
            UpdateList(null);
        }

        /// <summary>
        /// 指定インデクス範囲のアイテムを再描画
        /// </summary>
        /// <param name="startIndex">開始インデックス(-1で最末尾)</param>
        /// <param name="endIndex">終了インデックス(-1で最末尾)</param>
        /// <param name="invalidateOnly">無効にした後、再描画しない場合true</param>
        public void RedrawItem(int startIndex, int endIndex,bool invalidateOnly)
        {
            if (startIndex == -1)
                startIndex = _itemList.Count -1;

            if (endIndex == -1)
                endIndex = _itemList.Count - 1;

            int _startIndex = GetIndex(startIndex);
            int _endIndex = GetIndex(endIndex);
            if (_startIndex > _endIndex)
            {
                int tmp = _startIndex;
                _startIndex = _endIndex;
                _endIndex = tmp;
            }

            Debug.WriteLine(string.Format("RedrawItems({0}/{1}.{2}/{3})",
                startIndex,_startIndex,endIndex,_endIndex));

            if (_lvTarget.InvokeRequired)
                _lvTarget.Invoke((MethodInvoker)(()
                    => {
                        _lvTarget.RedrawItems(_startIndex, _endIndex, invalidateOnly);
//                    _lvTarget.Invalidate();
                }));
            else
            {
                _lvTarget.RedrawItems(_startIndex, _endIndex, invalidateOnly);
//                _lvTarget.Invalidate();

            }
        }

        /// <summary>
        /// リストビューをリフレッシュ
        /// </summary>
        public void Refresh()
        {
            ///キャッシュはクリア
            _firstItem = -1;
            _itemCache = null;

            if (_lvTarget.InvokeRequired)
                _lvTarget.Invoke((MethodInvoker)(() => _lvTarget.Refresh()));
            else
                _lvTarget.Refresh();

        }

        #region 仮想リストビューの実装

        /// <summary>
        /// リストビューアイテムのキャッシュ
        /// </summary>
        ListViewItem[] _itemCache = null;

        /// <summary>
        /// リストビューアイテムキャッシュ先頭アイテムのインデックス
        /// </summary>
        int _firstItem = -1;


        /// <summary>
        /// 仮想リストビューのキャッシュを再構築する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            if (_itemList == null)
            {
                Debug.WriteLine("CacheVirtualItems itemList not defined");
                return;
            }
            if (_itemList.Count == 0)
            {
                Debug.WriteLine("CacheVirtualItems itemList.Count == 0");
                return;
            }

            //            Debug.WriteLine(string.Format("CacheVirtualImages start:{0} end:{1} first:{2}",
            //                e.StartIndex, e.EndIndex, _firstItem));


            if (_itemCache != null && e.StartIndex >= _firstItem &&
                e.EndIndex <= _firstItem + _itemCache.Length)
            {
                return;
            }

            _firstItem = e.StartIndex;
            int cacheLength = e.EndIndex - e.StartIndex + 1;
            _itemCache = new ListViewItem[cacheLength];

            int itemIndex = 0;
            lock (((ICollection)_itemList).SyncRoot)
            {
                for (int i = 0; i < cacheLength; i++)
                {
                    itemIndex = (i + _firstItem);

                    _itemCache[i] = _itemList[GetIndex(itemIndex)].CreateVirtualListViewItem();
                }
            }
        }

        /// <summary>
        /// 仮想リストビューのアイテムを取得
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            Debug.Assert(_itemList != null);
            Debug.Assert(_itemList.Count > 0) ;

            if (_itemCache != null && e.ItemIndex >= _firstItem && e.ItemIndex < _firstItem + _itemCache.Length)
            {
//                Debug.WriteLine(string.Format("RetrieveVirtualItem index:{0} from cache", e.ItemIndex));
                e.Item = _itemCache[e.ItemIndex - _firstItem];
            }
            else
            {
                lock (((ICollection)_itemList).SyncRoot)
                {
//                    Debug.WriteLine(string.Format("RetrieveVirtualItem index:{0} create", e.ItemIndex));

                    if (_itemList.Count > e.ItemIndex)
                        e.Item = _itemList[GetIndex(e.ItemIndex)].CreateVirtualListViewItem();
                    else
                        Debug.WriteLine(string.Format("RetrieveVirtualItem list={0} <= index={1}",
                            _itemList.Count,e.ItemIndex));
                }
            }
        }

        #endregion

        /// <summary>
        /// インデックス取得
        /// </summary>
        /// <param name="itemIndex">インデックス</param>
        /// <returns>正/逆したインデックス</returns>
        int GetIndex(int itemIndex)
        {
            ///_itemListの中身が空っぽの時は想定していない
            if (_bReverse)
                return _itemList.Count - itemIndex - 1;
            return itemIndex;
        }

        /// <summary>
        /// 指定したインデックスのアイテムを取得
        /// </summary>
        /// <param name="itemIndex">インデックス</param>
        /// <returns></returns>
        public TLogInfoClass GetItem(int itemIndex)
        {
            lock (((ICollection)_itemList).SyncRoot)
            {
                return _itemList[GetIndex(itemIndex)];
            }
        }
    }


}
