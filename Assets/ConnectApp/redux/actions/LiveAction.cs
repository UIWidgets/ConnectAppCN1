using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class LiveAction : BaseAction {
    }

    public class FetchEventDetailAction : RequestAction {
        public string eventId;
    }

    public class FetchEventDetailSuccessAction : BaseAction {
        public LiveInfo liveInfo;
    }

    public class ClearLiveInfoAction : BaseAction {
    }
}