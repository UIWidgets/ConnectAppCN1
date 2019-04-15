using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class LoginSwitchScreenActionModel : BaseActionModel {
        public Func<string, IPromise> loginByWechatAction;
        public Action loginRouterPushToUnityBind;
    }
}