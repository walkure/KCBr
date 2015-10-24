using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace KCB2
{
    public partial class MissionList : UserControl
    {
        public MissionList()
        {
            InitializeComponent();
            tableLayoutPanel1.DoubleBuffer(true);
        }

        /// <summary>
        /// 開放されてる艦隊を有効化
        /// </summary>
        /// <param name="count">艦隊数(1-4)</param>
        public void SetActiveDeck(int count)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => _setActiveDeckControltext(count)));
            else
                _setActiveDeckControltext(count);
        }

        void _setActiveDeckControltext(int count)
        {
            lblFleetName4.ForeColor = SystemColors.Control;
            lblTime4.ForeColor = SystemColors.Control;
            lblMission4.ForeColor = SystemColors.Control;

            lblFleetName3.ForeColor = SystemColors.Control;
            lblTime3.ForeColor = SystemColors.Control;
            lblMission3.ForeColor = SystemColors.Control;

            lblFleetName2.ForeColor = SystemColors.Control;
            lblTime2.ForeColor = SystemColors.Control;
            lblMission2.ForeColor = SystemColors.Control;

            switch (count)
            {
                case 4:
                    lblFleetName4.ForeColor = SystemColors.ControlText;
                         lblTime4.ForeColor = SystemColors.ControlText;
                      lblMission4.ForeColor = SystemColors.ControlText;
                    goto case 3;
                case 3:
                    lblFleetName3.ForeColor = SystemColors.ControlText;
                         lblTime3.ForeColor = SystemColors.ControlText;
                      lblMission3.ForeColor = SystemColors.ControlText;
                    goto case 2;
                case 2:
                    lblFleetName2.ForeColor = SystemColors.ControlText;
                         lblTime2.ForeColor = SystemColors.ControlText;
                      lblMission2.ForeColor = SystemColors.ControlText;
                    break;
            }
        }

        /// <summary>
        /// 遠征一覧を更新
        /// </summary>
        /// <param name="deckList"></param>
        public void UpdateMissionList(IEnumerable<MemberData.Deck.Fleet> deckList,
            MemberData.Ship shipData)
        {
            Mission[] mission = new Mission[] { new Mission(), new Mission(), new Mission() };
            if (deckList == null)
                return;

            lock (deckList)
            {
                foreach (var it in deckList)
                {
                    switch (it.Num)
                    {
                        case 1:
                            continue;
                        case 2:
                        case 3:
                        case 4:
                            mission[it.Num - 2].Name = it.Mission;
                            mission[it.Num - 2].Detail = it.MissionDetail;
                            mission[it.Num - 2].Finish = it.MissionFinish;
                            mission[it.Num - 2].DeckName = it.Name;
                            mission[it.Num - 2].Num = it.MissionNum;
                            mission[it.Num - 2].SupplyStatus = CheckFleetStatus(it.Member, shipData);
                            break;
                        default:
                            return;
                    }
                }
            }

            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => updateMissionView(mission)));
            else
                updateMissionView(mission);
        }

        [Flags]
        enum SupplyStatusFlags
        {
            None = 0x00,
            Fuel = 0x01,
            Bullet = 0x02,
            Both = Bullet | Fuel,
        };

        /// <summary>
        /// 艦隊の燃料と弾薬の補給状況を調べる
        /// </summary>
        /// <param name="fleetMember">艦隊メンバー</param>
        /// <param name="shipData">艦娘ステート一覧</param>
        /// <returns>どの燃料が消費されているか</returns>
        SupplyStatusFlags CheckFleetStatus(IEnumerable<int> fleetMember, MemberData.Ship shipData)
        {
            SupplyStatusFlags status = SupplyStatusFlags.None;

            foreach (var ship_id in fleetMember)
            {
                var dat = shipData.GetShip(ship_id);
                if (!dat.Fuel.Full)
                {
                    Debug.WriteLine("Fuel not filled");
                    status |= SupplyStatusFlags.Fuel;
                }

                if (!dat.Bullet.Full)
                {
                    Debug.WriteLine("Bullet not filled");
                    status |= SupplyStatusFlags.Bullet;
                }
            }

            return status;
        }

        /// <summary>
        /// 遠征情報をコントロールへ更新する
        /// </summary>
        /// <param name="mission">遠征情報</param>
        void updateMissionView(Mission[] mission)
        {
            System.Diagnostics.Debug.WriteLine("Deck2:" + mission[0].DeckName 
                + " Mission:" + mission[0].Name);
            updateMissionViewParams(mission[0], lblFleetName2, lblMission2, lblTime2);

            System.Diagnostics.Debug.WriteLine("Deck3:" + mission[1].DeckName
                + " Mission:" + mission[1].Name);
            updateMissionViewParams(mission[1], lblFleetName3, lblMission3, lblTime3);

            System.Diagnostics.Debug.WriteLine("Deck4:" + mission[2].DeckName
                + " Mission:" + mission[2].Name);
            updateMissionViewParams(mission[2], lblFleetName4, lblMission4, lblTime4);
        }

        /// <summary>
        /// 表示を更新する
        /// </summary>
        /// <param name="mission">遠征情報</param>
        /// <param name="lblFleetName">艦隊名</param>
        /// <param name="lblMission">遠征名</param>
        /// <param name="lblTime">所要時間</param>
        void updateMissionViewParams(Mission mission,Label lblFleetName,Label lblMission
            ,CountdownLabel lblTime)
        {
            lblFleetName.Text = mission.DeckName;
            lblFleetName.BackColor = mission.SupplyStatusColor;
            toolTip1.SetToolTip(lblFleetName, mission.SupplyStatusText);

            if (mission.Valid)
            {
                lblMission.Text = mission.Name;
                toolTip1.SetToolTip(lblMission, mission.Detail);
                lblTime.FinishTime = mission.Finish;
                toolTip1.SetToolTip(lblTime, mission.Finish.ToString());

            }
            else
            {
                lblMission.Text = "";
                toolTip1.SetToolTip(lblMission, null);
                lblTime.Valid = false;
                toolTip1.SetToolTip(lblTime, null);
            }
        }


        /// <summary>
        /// 艦隊情報パラメタ
        /// </summary>
        class Mission
        {
            /// <summary>
            /// 遠征名
            /// </summary>
            public string Name { get;set;}

            /// <summary>
            /// 艦隊番号
            /// </summary>
            public int Num { get; set; }

            /// <summary>
            /// 遠征詳細
            /// </summary>
            public string Detail { get; set; }

            /// <summary>
            /// 終了時刻
            /// </summary>
            public DateTime Finish { get;set;}

            /// <summary>
            /// 艦隊名
            /// </summary>
            public string DeckName { get; set; }

            /// <summary>
            /// 補給状況
            /// </summary>
            public SupplyStatusFlags SupplyStatus { get; set; }

            public Mission() { Num = 0; }

            /// <summary>
            /// 遠征が有効かどうか
            /// </summary>
            public bool Valid
            {
                get
                {
                    if (Num == 0)
                        return false;

                    return true;
                }
            }

            /// <summary>
            /// 補給状況から色を取ってくる
            /// </summary>
            public Color SupplyStatusColor
            {
                get
                {
                    switch (SupplyStatus)
                    {
                        case SupplyStatusFlags.Both:
                            return Color.HotPink;
                        case SupplyStatusFlags.Bullet:
                            return Color.MistyRose;
                        case SupplyStatusFlags.Fuel:
                            return Color.LightPink;
                        default:
                            return SystemColors.Control;
                    }
                }
            }

            /// <summary>
            /// 補給状況のテキスト
            /// </summary>
            public string SupplyStatusText
            {
                get
                {
                    switch (SupplyStatus)
                    {
                        case SupplyStatusFlags.Both:
                            return "燃料と弾薬が補給されていません";
                        case SupplyStatusFlags.Bullet:
                            return "弾薬が補給されていません";
                        case SupplyStatusFlags.Fuel:
                            return "燃料が補給されていません";
                        default:
                            return "燃料と弾薬は補給済みです";
                    }
                }
            }

        }

    }
}
