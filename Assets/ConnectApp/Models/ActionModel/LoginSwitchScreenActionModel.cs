using System;

namespace ConnectApp.Models.ActionModel
{
    public class LoginSwitchScreenActionModel
    {
        public Action mainRouterPop;
        public Action<string> loginByWechatAction;
        public Action loginRouterPushToUnityBind;
    }
}