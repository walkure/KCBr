using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace KCB2
{
    class BrowserSetting
    {
        const string FeatureControl = @"Software\Microsoft\Internet Explorer\MAIN\FeatureControl\";
        const string BrowserEmurationKey = "FEATURE_BROWSER_EMULATION";
        const string GPURenderingKey = "FEATURE_GPU_RENDERING";

        const string BrowserEmuration = FeatureControl + BrowserEmurationKey;
        const string GPURendering = FeatureControl+GPURenderingKey;

        /// <summary>
        /// 実行プロセス名を取得
        /// </summary>
        static public string ProcessName
        {
            get
            {
                string name;
                using (var proc = Process.GetCurrentProcess())
                {
                    name = proc.MainModule.ModuleName;
                }
                Debug.WriteLine("ProcessName:" + name);
                return name;
            }
        }

        /// <summary>
        /// エミュレーションするIEのバージョン
        /// </summary>
        public enum BrowserVer
        {
            IE11_FORCE = 11001,
            IE11 = 11000,
            IE10_FORCE = 10001,
            IE10 = 10000,
            IE9_FORCE = 9999,
            IE9 = 9000,
            IE8_FORCE = 8888,
            IE8 = 8000,
            IE7 = 7000,
        }

        /// <summary>
        /// WebBrowserコントロールがエミュレーションするIEのバージョン
        /// </summary>
        static public BrowserVer EmurateBrowser
        {
            get
            {
                try
                {
                    var key = Registry.CurrentUser.OpenSubKey(BrowserEmuration);
                    if (key == null)
                        return BrowserVer.IE7;

                    int emuVer = (int)key.GetValue(ProcessName, (int)BrowserVer.IE7);

                    key.Close();

                    return (BrowserVer)emuVer;
                }
                catch (Exception ex)
                {
                    string msg = "ブラウザバージョンの取得に失敗しました\n" + ex.ToString();
                    Debug.WriteLine(msg);
                    MessageBox.Show(msg);

                    return BrowserVer.IE7; 
                }
            }

            set
            {
                try
                {
                    var key = Registry.CurrentUser.CreateSubKey(BrowserEmuration);

                    //IE7は標準なので、エントリを削除する
                    if (value == BrowserVer.IE7)
                    {
                        if (key.GetValue(ProcessName) != null)
                            key.DeleteValue(ProcessName);
                        key.Close();
                        return;
                    }

                    key.SetValue(ProcessName, (int)value, RegistryValueKind.DWord);
                    key.Close();
                }
                catch (Exception ex)
                {
                    string msg = "ブラウザバージョンの設定に失敗しました\n" + ex.ToString();
                    Debug.WriteLine(msg);
                    MessageBox.Show(msg);
                }
            }
        }

        /// <summary>
        /// GPUレンダリングの設定
        /// </summary>
        public static bool EnableGPURendering
        {
            get
            {
                try
                {
                    var key = Registry.CurrentUser.OpenSubKey(GPURendering);
                    if (key == null)
                        return false;

                    var val = key.GetValue(ProcessName);
                    if (val == null)
                    {
                        key.Close();
                        return false;
                    }

                    int enable = (int)val;

                    key.Close();

                    return enable == 1;
                }
                catch (Exception ex)
                {
                    string msg = "GPUレンダリング設定の読み込みに失敗しました\n" + ex.ToString();
                    Debug.WriteLine(msg);
                    MessageBox.Show(msg);
                    return false;
                }
            }

            set
            {
                try
                {
                    var key = Registry.CurrentUser.CreateSubKey(GPURendering);
                    if (value == false)
                    {
                        if (key.GetValue(ProcessName) != null)
                            key.DeleteValue(ProcessName);
                        key.Close();
                        return;
                    }

                    key.SetValue(ProcessName, 1, RegistryValueKind.DWord);
                    key.Close();
                }
                catch (Exception ex)
                {
                    string msg = "GPUレンダリング設定の書き込みに失敗しました\n" + ex.ToString();
                    Debug.WriteLine(msg);
                    MessageBox.Show(msg);
                }
            }
        }

    }
}
