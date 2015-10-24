using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace KCBTimer
{
    static class Program
    {
        static public Mutex _mutex;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            _mutex = new Mutex(true, "KCBTimer existing flag", out createdNew);
            if (createdNew == false)
            {
                //ミューテックスの初期所有権が付与されなかったときは
                //すでに起動していると判断して終了
                MessageBox.Show("多重起動を検出しました。終了します","KCBTimer");
                return;
            }
            //mutexを非シグナル状態へ
            _mutex.WaitOne();
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            Properties.Settings.Default.Save();

            _mutex.Dispose();
        }
    }
}
