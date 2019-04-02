using ConnectApp.models;
using ConnectApp.screens;

namespace ConnectApp.redux.actions {
    public class MainNavigatorPushToAction : BaseAction {
        public string RouteName;
    }

    public class MainNavigatorPushToEventDetailAction : BaseAction {
        public string EventId;
        public EventType EventType;
    }

    public class MainNavigatorPushToArticleDetailAction : BaseAction {
        public string ArticleId;
    }

    public class LoginNavigatorPushToBindUintyAction : BaseAction {
        public FromPage FromPage;
    }

    public class LoginNavigatorPushToAction : BaseAction {
        public string RouteName;
    }

    public class MainNavigatorPopAction : BaseAction {
        public int Index = 1;
    }

    public class LoginNavigatorPopAction : BaseAction {
        public int Index = 1;
    }

    public class OpenUrlAction : BaseAction {
        public string url = "";
    }
}