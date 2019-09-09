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

    public class MainNavigatorPushReplaceSplashAction : BaseAction {
    }

    public class MainNavigatorPushReplaceMainAction : BaseAction {
    }
    
    public class MainNavigatorPushToNotificationAction : BaseAction {
    }
    
    public class MainNavigatorPushToDiscoverChannelsAction : BaseAction {
    }
    
    public class MainNavigatorPushToChannelDetailAction : BaseAction {
    }

    public class MainNavigatorPushToChannelMembersAction : BaseAction {
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
        public bool isSlug = false;
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
        public bool isSlug = false;
    }

    public class MainNavigatorPushToTeamFollowerAction : BaseAction {
        public string teamId;
    }

    public class MainNavigatorPushToTeamMemberAction : BaseAction {
        public string teamId;
    }

    public class MainNavigatorPushToVideoPlayerAction : BaseAction {
        public string url;
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

    public class CopyTextAction : BaseAction {
        public string text = "";
    }

    public class MainNavigatorPushToWebViewAction : BaseAction {
        public string url = "";
    }
    
    public class EnterRealityAction : BaseAction {
    }

    public class MainNavigatorPushToQRScanLoginAction : BaseAction {
        public string token = "";
    }
}