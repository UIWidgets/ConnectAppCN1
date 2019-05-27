using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class SettingScreenActionModel : BaseActionModel {
        public Action<string> openUrl;
        public Action<string> mainRouterPushTo;
        public Action clearCache;
        public Action logout;
    }
}