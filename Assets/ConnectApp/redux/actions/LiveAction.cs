using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class LiveAction : BaseAction {
    }

    public class FetchEventDetailAction : RequestAction {
        public string eventId;
    }

    public class FetchEventDetailSuccessAction : BaseAction {
        public IEvent eventObj;
    }

    public class ClearLiveInfoAction : BaseAction {
    }
}