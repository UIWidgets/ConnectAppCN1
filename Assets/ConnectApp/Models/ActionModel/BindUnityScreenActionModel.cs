using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class BindUnityScreenActionModel : BaseActionModel {
        public Action loginRouterPop;
        public Action<string> openUrl;
        public Func<IPromise> openCreateUnityIdUrl;
        public Action<string> changeEmail;
        public Action<string> changePassword;
        public Action startLoginByEmail;
        public Action clearEmailAndPassword;
        public Func<IPromise> loginByEmail;
    }
}