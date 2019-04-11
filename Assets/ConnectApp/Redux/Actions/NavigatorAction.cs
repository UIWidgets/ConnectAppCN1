using ConnectApp.models;
using ConnectApp.screens;
using Unity.UIWidgets.widgets;

namespace ConnectApp.redux.actions {
    public class MainNavigatorPushToAction : BaseAction {
        public string routeName;
    }
    
    public class MainNavigatorPushToRouteAction : BaseAction {
        public Route route;
    }

    public class MainNavigatorPushToEventDetailAction : BaseAction {
        public string eventId;
        public EventType eventType;
    }

    public class MainNavigatorPushToArticleDetailAction : BaseAction {
        public string articleId;
    }
    
    public class MainNavigatorPushToVideoPlayerAction : BaseAction {
        public string videoUrl;
    }

    public class LoginNavigatorPushToBindUnityAction : BaseAction {
    }

    public class LoginNavigatorPushToAction : BaseAction {
        public string routeName;
    }

    public class MainNavigatorPopAction : BaseAction {
        public int index = 1;
    }

    public class LoginNavigatorPopAction : BaseAction {
        public int index = 1;
    }

    public class OpenUrlAction : BaseAction {
        public string url = "";
    }

    public static partial class Actions {
//        public static object popFromBindUnityScreen() {
//            return 
//        }
    }
}