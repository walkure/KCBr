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
    public partial class EnemyFleetList : UserControl
    {
        public class FleetInfo
        {
            public FleetInfo(BattleResult.Result.FleetState state)
            {
                Ships = new ShipInfo[] {
                    new ShipInfo(state.Ships[0]),new ShipInfo(state.Ships[1]),new ShipInfo(state.Ships[2]),
                    new ShipInfo(state.Ships[3]),new ShipInfo(state.Ships[4]),new ShipInfo(state.Ships[5]),
                };

            }

            public ShipInfo[] Ships;

            public class ShipInfo
            {
                public string Name { get; private set; }
                public int MaxHP { get; private set; }
                public int CurrentHP { get; private set; }

                public ShipInfo(BattleResult.Result.FleetState.ShipState state)
                {
                    if (!state.Valid)
                    {
                        Name = "";
                        return;
                    }

                    Name = state.ShipName;
                    MaxHP = state.MaxHP;
                    CurrentHP = state.CurrentHP;

                    
                }
            }

        }

        public FleetInfo Fleet
        {
            set
            {
                var fleet = value;

                lblShip1.Text = fleet.Ships[0].Name;
                lblHP1.Text = HPString(fleet.Ships[0]);
                lblHP1.BackColor = HPColor(fleet.Ships[0]);

                lblShip2.Text = fleet.Ships[1].Name;
                lblHP2.Text = HPString(fleet.Ships[1]);
                lblHP2.BackColor = HPColor(fleet.Ships[1]);

                lblShip3.Text = fleet.Ships[2].Name;
                lblHP3.Text = HPString(fleet.Ships[2]);
                lblHP3.BackColor = HPColor(fleet.Ships[2]);

                lblShip4.Text = fleet.Ships[3].Name;
                lblHP4.Text = HPString(fleet.Ships[3]);
                lblHP4.BackColor = HPColor(fleet.Ships[3]);

                lblShip5.Text = fleet.Ships[4].Name;
                lblHP5.Text = HPString(fleet.Ships[4]);
                lblHP5.BackColor = HPColor(fleet.Ships[4]);

                lblShip6.Text = fleet.Ships[5].Name;
                lblHP6.Text = HPString(fleet.Ships[5]);
                lblHP6.BackColor = HPColor(fleet.Ships[5]);

            }
        }

        public string BattleStatus
        {
            set
            {
                lblBattleStatus.Text = "戦果予想："+value;
            }
        }

        string HPString(FleetInfo.ShipInfo info)
        {
            if (info.Name == "")
                return "";

            double ratio = (double)info.CurrentHP / info.MaxHP;
            return string.Format("{0}/{1}({2})", info.CurrentHP, info.MaxHP, ratio.ToString("0.00%"));
        }

        Color HPColor(FleetInfo.ShipInfo info)
        {
            if (info.Name == "")
                return SystemColors.Control;

            double ratio = (double)info.CurrentHP / info.MaxHP;
            if(ratio <= 0)
                return Color.LightBlue;
            if (ratio <= 0.25)
                return Color.LightPink;
            if (ratio < 0.5)
                return Color.Gold;
            if (ratio < 0.75)
                return Color.Beige;

            return Color.LightGreen;
        }

        public EnemyFleetList()
        {
            InitializeComponent();
        }
    }
}
