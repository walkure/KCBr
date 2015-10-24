using System;
using System.ServiceModel;

namespace KCB2.IPC
{
    [ServiceContract]
    public interface IUpdateNotification
    {
        [OperationContract]
        void UpdateNDock(int dockNum, string shipName, DateTime finishTime);

        [OperationContract]
        void UpdateMission(int fleetNum, string fleetName, string missionName,
            DateTime finishTime);
    }
}
