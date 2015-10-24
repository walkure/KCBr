using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KCB2
{
    public partial class MaterialList : UserControl
    {
        public MaterialList()
        {
            InitializeComponent();
            tableLayoutPanel1.DoubleBuffer(true);
            /*
            lblFuel.Text = "888888";
            lblBullet.Text = "888888";
            lblSteel.Text = "888888";
            lblBauxite.Text = "888888";

            lblFastRepair.Text = "8888";
            lblFastBuild.Text = "8888";
            lblDevelop.Text = "8888";

            //*/

        }
        /// <summary>
        /// 資源情報を更新
        /// </summary>
        /// <param name="data"></param>
        public void Update(MemberData.Material data)
        {
            if(InvokeRequired)
               BeginInvoke((MethodInvoker)(() => updateMaterialView(data)));
            else
                updateMaterialView(data);
        }

        void updateMaterialView(MemberData.Material data)
        {
            lblFuel.Text = data.Fuel.ToString();
            lblBullet.Text = data.Ammo.ToString();
            lblSteel.Text = data.Steel.ToString();
            lblBauxite.Text = data.Bauxite.ToString();

            lblFastRepair.Text = data.FastRepair.ToString();
            lblFastBuild.Text = data.FastBuild.ToString();
            lblDevelop.Text = data.Developer.ToString();
            lblUpdater.Text = data.Updater.ToString();
        }
    }
}
