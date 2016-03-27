using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCB2
{
    public partial class FormVolume : Form
    {
        MixerAPI _mixer = null;

        public FormVolume()
        {
            InitializeComponent();
        }

        private void FormVolume_Load(object sender, EventArgs e)
        {
            try
            {
                _mixer = new MixerAPI();
            }
            catch (MixerAPI.MixerException ex)
            {
                System.Diagnostics.Debug.WriteLine("Cannot open Mixer device" + ex.ToString());
                Close();
                return;
            }


            //何故か勝手に大きさ変えられてしまう
            Size = new System.Drawing.Size(82, 162);
            cbMute.Checked = _mixer.Mute;
            tbVolume.Value = (int)_mixer.Volume;

            toolTip1.SetToolTip(tbVolume,string.Format("音量:{0}",tbVolume.Value));
            toolTip1.SetToolTip(cbMute, cbMute.Checked ? "ミュートしています"
                                                       : "ミュートしていません");

        }
        private void cbMute_CheckedChanged(object sender, EventArgs e)
        {
            tbVolume.Enabled = !cbMute.Checked;
            _mixer.Mute = cbMute.Checked;
            toolTip1.SetToolTip(cbMute, cbMute.Checked ? "ミュートしています"
                                                       : "ミュートしていません");
        }

        private void FormVolume_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        private void tbVolume_Scroll(object sender, EventArgs e)
        {
            _mixer.Volume = (uint)tbVolume.Value;
            toolTip1.SetToolTip(tbVolume,string.Format("音量:{0}",tbVolume.Value));
        }

        private void FormVolume_FormClosed(object sender, FormClosedEventArgs e)
        {
            //ミキサーが開けなかった時はnullで飛んでくる
            if (_mixer != null)
            {
                _mixer.Dispose();
                _mixer = null;
            }
        }


    }
}
