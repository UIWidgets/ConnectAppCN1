using System;
using RSG;

namespace ConnectApp.Models.ActionModel
{
    public class LoginSwitchScreenActionModel
    {
        public Action mainRouterPop;
        public Func<string, IPromise> loginByWechatAction;
        public Action loginRouterPushToUnityBind;
    }
}