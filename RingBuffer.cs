using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KCB2
{
    public class RingBuffer<T> : IEnumerable<T>
    {
        /// <summary>
        /// データを覚えておく配列
        /// </summary>
        private T[] _buffer;

        /// <summary>
        /// 最新のデータが有る場所
        /// </summary>
        private int _pos = -1;

        /// <summary>
        /// Enum中にアイテム追加されたことを検出
        /// </summary>
        private long _revision = 0 ;

        /// <summary>
        /// リング長
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// リング中に存在するデータ数。最大Capacityまで
        /// </summary>
        private int _avail = 0;

        /// <summary>
        /// データ末尾。iter用
        /// </summary>
        private int _tail = 0;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="capacity">収納可能な</param>
        public RingBuffer(int capacity)
        {
            _buffer = new T[capacity];
            Capacity = capacity;
        }

        /// <summary>
        /// リングにアイテムを追加
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            int npos = (_pos + 1) % Capacity;
            _buffer[npos] = item;
            _pos = npos;

            if (_avail < Capacity)
                _avail++;
            else
                _tail = (_tail + 1) % Capacity;
            
            _revision++;
        }

        /// <summary>
        /// バッファをクリア
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < Capacity; i++)
                _buffer[i] = default(T);
            _pos = -1;
            _avail = 0;
            _tail = 0;
            _revision++;
        }

        /// <summary>
        /// 中身を列挙する
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            long revision = _revision;
            for (int i = 0; i < _avail ; i++)
            {
                if (revision != _revision)
                    throw new InvalidOperationException("列挙中にデータが変更された");
                int index = (_tail + i ) % Capacity;
                yield return _buffer[index];
            }
        }

        /// <summary>
        /// 非GenericなGetEnumeratorの実装
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        { return this.GetEnumerator(); }

    }
}
