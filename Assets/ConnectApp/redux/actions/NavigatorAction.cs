using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class NavigatorToEventDetailAction : BaseAction {
        public string eventId;
        public EventType eventType;
    }

    public class NavigatorToArticleDetailAction : BaseAction {
        public string detailId;
    }
}