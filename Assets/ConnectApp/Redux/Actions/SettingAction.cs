using Unity.UIWidgets.widgets;

namespace ConnectApp.redux.actions {
    public class SettingReviewUrlAction : RequestAction {
        public string platform;
        public string store;
    }

    public class SettingReviewUrlSuccessAction : BaseAction {
        public string url;
    }

    public class SettingClearCacheAction : BaseAction {
    }
}