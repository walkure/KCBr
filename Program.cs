using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace KCB2
{
    static class Program
    {
        static public Mutex _mutex;

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            _mutex = new Mutex(true, "KCB2 existing flag", out createdNew);
            if (createdNew == false)
            {
                //ミューテックスの初期所有権が付与されなかったときは
                //すでに起動していると判断して終了
                MessageBox.Show("多重起動を検出しました。終了します", "KCB2");
                return;
            }
            //mutexを非シグナル状態へ
            _mutex.WaitOne();

            int workerThreads,completionPortThreads;
            System.Threading.ThreadPool.GetMaxThreads(out workerThreads,out completionPortThreads);
            System.Threading.ThreadPool.SetMaxThreads(workerThreads+50, completionPortThreads);

#if !DEBUG
            try
#endif
            {
#if !DEBUG
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
                System.AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
#endif
                bool bPortable = false;
                foreach (var argv in args)
                switch (argv)
                {
                    case "init":
                        Properties.Settings.Default.Reset();
                        break;
                    case "portable":
                        bPortable = true;
                        break;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain(bPortable));
            }
#if !DEBUG
            catch (Exception exc)
            {
                String message = exc.ToString();
                System.Diagnostics.Debug.WriteLine("Unexpected Exception:"+message);
                MessageBox.Show(message, "Unexpected Exception");
            }
            finally{
#endif
            _mutex.Dispose();
#if !DEBUG
            }
#endif
        }

#if !DEBUG
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                //エラーメッセージを表示する
                Exception exc = (Exception)e.ExceptionObject;
                String message = exc.ToString();
                System.Diagnostics.Debug.WriteLine("Unhandled Exception\n"+message);
                MessageBox.Show(message, "Unhandled Exception");
            }
            finally
            {
                //アプリケーションを終了する
                Environment.Exit(1);
            }
        }
#endif

    }
}
