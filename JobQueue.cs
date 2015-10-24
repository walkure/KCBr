using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace KCB
{
    /// <summary>
    /// スレッドプールにワーカースレッドを放り込んで、ジョブキューの中身をもぐもぐさせる
    /// 
    /// http://d.hatena.ne.jp/zarchis/20101015
    /// </summary>
    /// <typeparam name="T">ジョブキューへ放り込まれる仕事内容</typeparam>
    public abstract class JobQueue<T> : IDisposable
    {
        /// <summary>
        /// ジョブを管理するFIFO
        /// </summary>
        Queue<T> _jobs = new Queue<T>();
        /// <summary>
        /// 動作中はtrue
        /// </summary>
        volatile bool _running = true;
        /// <summary>
        /// 暇な時はスレッドを寝かせるイベント。初期は寝てる
        /// </summary>
        ManualResetEvent _ev = new ManualResetEvent(false);

        /// <summary>
        /// メッセージ・キューのコンストラクタ。スレッドプールにワーカーを投入
        /// </summary>
        public JobQueue()
        {
            Debug.WriteLine("Begin JobQueue jobclass:" + typeof(T).Name);
            ThreadPool.QueueUserWorkItem(_execute);
        }

        /// <summary>
        /// ワーカー関数
        /// </summary>
        /// <param name="obj">スレッド引数(未使用)</param>
        void _execute(object param)
        {
            while (_ev.WaitOne())
            {
                if (!_running)
                    break;

                T job = default(T);
                lock (_jobs)
                {
                    if (_jobs.Count > 0)
                        job = _jobs.Dequeue();
                    else
                        _ev.Reset();
                }
                if (job != null)
                    processJob(job);
            }
        }

        /// <summary>
        /// IDispose インタフェイス
        /// </summary>
        virtual public void Dispose()
        {
            _running = false;
            _ev.Set();
        }

        /// <summary>
        /// ジョブを投入
        /// </summary>
        /// <param name="job">投入するセッション</param>
        public void Add(T job)
        {
            if (!_running)
                throw new Exception("jobqueue already shutdown.");

            lock (_jobs)
            {
                _jobs.Enqueue(job);
                _ev.Set();
            }
        }

        /// <summary>
        /// ジョブを実行するハンドラ
        /// </summary>
        /// <param name="job">ジョブ内容</param>
        abstract protected void processJob(T job);
    }
}
