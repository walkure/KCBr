using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KCBTimer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public bool TcpMode;

        private void Form2_Load(object sender, EventArgs e)
        {
            tbDockSound.Text = Properties.Settings.Default.DockOutSound;
            tbMissionSound.Text = Properties.Settings.Default.MissionFinishSound;
            tbCondSound.Text = Properties.Settings.Default.CondSound;
            cbUseNetTcp.Checked = Properties.Settings.Default.NetTcp;

            var ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(
               System.Reflection.Assembly.GetExecutingAssembly().Location);
            labelName.Text = ver.ProductName;
            labelRevision.Text = ver.ProductVersion;
            labelDescription.Text = ver.Comments;
            labelCopyright.Text = ver.LegalCopyright;

            if (TcpMode)
                labelModeText.Text = "(TCPモードで動作中)";
            else
                labelModeText.Text = "";

            tbNotifySound.Text = Properties.Settings.Default.NotifySound;

        }

        private void btBrowseDock_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "WAVE File(*.wav)|*.wav|All Files(*.*)|*.*";
            ofd.Title = "出渠通知用WAVファイルを選んでください";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            tbDockSound.Text = ofd.FileName;
        }

        private void btTestDock_Click(object sender, EventArgs e)
        {
            soundPlay(tbDockSound.Text);
        }

        private void btBrowseMission_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "WAVE File(*.wav)|*.wav|All Files(*.*)|*.*";
            ofd.Title = "遠征終了通知用WAVファイルを選んでください";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            tbMissionSound.Text = ofd.FileName;
        }

        private void btTestMission_Click(object sender, EventArgs e)
        {
            soundPlay(tbMissionSound.Text);
        }
        
        private void btBrowseCond_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "WAVE File(*.wav)|*.wav|All Files(*.*)|*.*";
            ofd.Title = "コンディション回復通知用WAVファイルを選んでください";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            tbCondSound.Text = ofd.FileName;

        }

        private void btTestCond_Click(object sender, EventArgs e)
        {
            soundPlay(tbCondSound.Text);
        }

        private void btNotify_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "WAVE File(*.wav)|*.wav|All Files(*.*)|*.*";
            ofd.Title = "戦闘終了通知用WAVファイルを選んでください";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            tbNotifySound.Text = ofd.FileName;

        }

        private void btTestNotify_Click(object sender, EventArgs e)
        {
            soundPlay(tbNotifySound.Text);
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DockOutSound = tbDockSound.Text;
            Properties.Settings.Default.MissionFinishSound = tbMissionSound.Text;
            Properties.Settings.Default.CondSound = tbCondSound.Text;
            Properties.Settings.Default.NetTcp = cbUseNetTcp.Checked;
            Properties.Settings.Default.NotifySound = tbNotifySound.Text;

            DialogResult = DialogResult.OK;
            Close();

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        void soundPlay(string path)
        {
            try
            {
                var player = new System.Media.SoundPlayer(path);
                player.Play();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show("ファイルが見つかりませんでした。");
                Debug.WriteLine("SoundPlayer throws FileNotFoundException:" + ex.ToString());
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("ファイルのフォーマットが不正です。");
                Debug.WriteLine("SoundPlayer throws InvalidOperationException:" + ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("再生に失敗しました。");
                Debug.WriteLine("SoundPlayer throws InvalidOperationException:" + ex.ToString());
            }
        }

    }
}
