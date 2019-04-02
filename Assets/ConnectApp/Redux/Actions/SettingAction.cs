namespace ConnectApp.redux.actions {
    public class SettingReviewUrlAction : RequestAction {
        public string platform;
        public string store;
    }
    
    public class SettingReviewUrlSuccessAction : RequestAction {
        public string url;
    }
}