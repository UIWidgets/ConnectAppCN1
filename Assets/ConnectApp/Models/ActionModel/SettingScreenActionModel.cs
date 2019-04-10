using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class SettingScreenActionModel {
        public Action mainRouterPop;
        public Action<string> openUrl;
        public Action clearCache;
        public Action logout;
    }
}