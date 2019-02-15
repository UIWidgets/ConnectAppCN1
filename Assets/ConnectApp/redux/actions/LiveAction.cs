using System;
using ConnectApp.models;

namespace ConnectApp.redux.actions
{
    public class LiveAction : BaseAction
    {
    }

    public class LiveRequestAction : RequestAction
    {
        public string eventId;
    }

    [Serializable]
    public class LiveResponseAction : ResponseAction
    {
        public LiveInfo liveInfo;
        public string eventId;
    }
}