using System.Collections.Generic;
using ConnectApp.Models.State;
using ConnectApp.screens;

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

    public class MainNavigatorPushToChannelAction : BaseAction {
        public string channelId;
        public bool pushReplace = false;
    }

    public class MainNavigatorPushToChannelDetailAction : BaseAction {
        public string channelId;
    }

    public class MainNavigatorPushToChannelShareAction : BaseAction {
        public string channelId;
    }

    public class MainNavigatorPushToChannelMembersAction : BaseAction {
        public string channelId;
    }

    public class MainNavigatorPushToChannelIntroductionAction : BaseAction {
        public string channelId;
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

    public class MainNavigatorPushToUserLikeAction : BaseAction {
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
        public string url;
        public bool needUpdate;
        public int limitSeconds;
    }

    public class MainNavigatorPushToReportAction : BaseAction {
        public string reportId;
        public ReportType reportType;
    }

    public class MainNavigatorPushToReactionsDetailAction : BaseAction {
        public string messageId;
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
        public bool landscape = false;
        public bool fullscreen = false;
        public bool showOpenInBrowser = true;
    }

    public class EnterRealityAction : BaseAction {
    }

    public class MainNavigatorPushToQRScanLoginAction : BaseAction {
        public string token = "";
    }

    public class MainNavigatorPushToFavoriteDetailAction : BaseAction {
        public string tagId = "";
        public string userId = "";
        public FavoriteType type;
    }

    public class MainNavigatorPushToEditFavoriteAction : BaseAction {
        public string tagId = "";
    }

    public class MainNavigatorPushToPhotoViewAction : BaseAction {
        public List<string> urls;
        public string url;
        public bool useCachedNetworkImage = true;
        public Dictionary<string, byte[]> imageData;
    }

    public class MainNavigatorPushToChannelMentionAction : BaseAction {
        public string channelId;
    }

    public class MainNavigatorPushToLeaderBoardDetailAction : BaseAction {
        public string id;
        public LeaderBoardType type = LeaderBoardType.collection;
    }

    public class MainNavigatorPushToLeaderBoardAction : BaseAction {
        public int initIndex = 0;
    }

    public class MainNavigatorPushToGameDetailAction : BaseAction {
        public string gameId;
    }
}