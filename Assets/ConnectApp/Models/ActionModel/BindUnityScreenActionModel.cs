using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class BindUnityScreenActionModel {
        public Action mainRouterPop;
        public Action loginRouterPop;
        public Action<string> openUrl;
        public Func<IPromise> openCreateUnityIdUrl;
        public Action<string> changeEmail;
        public Action<string> changePassword;
        public Func<IPromise> loginByEmail;
    }
}