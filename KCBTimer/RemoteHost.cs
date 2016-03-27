using System;
using System.Windows.Forms;
using System.ServiceModel;

namespace KCBTimer
{
    /// <summary>
    /// WCFのRPCエンドポイント
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class RemoteHost : KCB.RPC.IUpdateNotification
    {
        public KCBTimer.Form1 _parent;

        public RemoteHost(KCBTimer.Form1 parentForm) { _parent = parentForm; }
        public RemoteHost()
        {
            _parent = null;
            foreach (Form form in Application.OpenForms)
            {
                if (form is KCBTimer.Form1)
                {
                    _parent = (KCBTimer.Form1)form;
                    break;
                }
            }
        }

        public void UpdateNDock(int dockNum, string shipName, DateTime finishTime)
        {
            _parent.UpdateNDock(dockNum, shipName, finishTime);
        }

        public void UpdateMission(int fleetNum, string fleetName, string missionName,
            DateTime finishTime)
        {
            _parent.UpdateMission(fleetNum, fleetName, missionName, finishTime);
        }

        public void UpdateParameters(string memberID,int dockMax, int deckMax)
        {
            _parent.UpdateParameters(memberID,dockMax, deckMax);
        }

        public void ShutdownTimer()
        {
            _parent.ShutdownApplication();
        }

        public void ShowPreferenceForm()
        {
            _parent.ShowPreference();
        }

        public void UpdateConditionTimer(int fleetNum, string fleetName, DateTime finishTime)
        {
            _parent.UpdateConditionTimer(fleetNum, fleetName, finishTime);
        }

        public void FinishBattle(string type)
        {
            TimerHandlerListViewItem.PlaySound(Properties.Settings.Default.NotifySound);
            _parent.ShowBaloonMessage("終了通知", string.Format("{0}が終了しました",type));
            //not implemented yet
        }
    }
}
