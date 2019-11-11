using System;

namespace ConnectApp.Models.ActionModel {
    public class SettingScreenActionModel : BaseActionModel {
        public Action<string> openUrl;
        public Action<string> mainRouterPushTo;
        public Action<bool> updateVibrate;
        public Action clearCache;
        public Action logout;
    }
}