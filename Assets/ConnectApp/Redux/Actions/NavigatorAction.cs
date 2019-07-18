using ConnectApp.Models.State;
using ConnectApp.screens;
using Unity.UIWidgets.widgets;

namespace ConnectApp.redux.actions {
    public class MainNavigatorPushToAction : BaseAction {
        public string routeName;
    }

    public class MainNavigatorReplaceToAction : BaseAction {
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
        public bool isPush = false;
    }

    public class MainNavigatorPushToUserDetailAction : BaseAction {
        public string userId;
    }

    public class MainNavigatorPushToUserFollowingAction : BaseAction {
        public string userId;
        public int initialPage = 0;
    }

    public class MainNavigatorPushToUserFollowerAction : BaseAction {
        public string userId;
    }

    public class MainNavigatorPushToEditPersonalInfoAction : BaseAction {
        public string userId;
    }

    public class MainNavigatorPushToTeamDetailAction : BaseAction {
        public string teamId;
    }

    public class MainNavigatorPushToTeamFollowerAction : BaseAction {
        public string teamId;
    }

    public class MainNavigatorPushToTeamMemberAction : BaseAction {
        public string teamId;
    }

    public class MainNavigatorPushToVideoPlayerAction : BaseAction {
        public string videoUrl;
    }

    public class MainNavigatorPushToReportAction : BaseAction {
        public string reportId;
        public ReportType reportType;
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

    public class PlayVideoAction : BaseAction {
        public string url = "";
    }

    public class CopyTextAction : BaseAction {
        public string text = "";
    }

    public class MainNavigatorPushToWebViewAction : BaseAction {
        public string url = "";
    }

    public static partial class Actions {
//        public static object popFromBindUnityScreen() {
//            return 
//        }
    }
}