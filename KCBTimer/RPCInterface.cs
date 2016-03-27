using System;
using System.ServiceModel;

namespace KCB.RPC
{
    [ServiceContract]
    public interface IUpdateNotification
    {
        [OperationContract]
        void UpdateParameters(string memberID, int dockMax, int deckMax);

        [OperationContract]
        void UpdateNDock(int dockNum, string shipName, DateTime finishTime);

        [OperationContract]
        void UpdateMission(int fleetNum, string fleetName, string missionName,
            DateTime finishTime);

        [OperationContract]
        void UpdateConditionTimer(int fleetNum, string fleetName, DateTime finishTime);

        [OperationContract]
        void ShutdownTimer();

        [OperationContract]
        void ShowPreferenceForm();

        [OperationContract]
        void FinishBattle(string type);
    }
}
