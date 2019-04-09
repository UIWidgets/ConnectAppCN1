using System;
using ConnectApp.components;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class EventDetailScreenActionModel {
        public Action mainRouterPop;
        public Action pushToLogin;
        public Func<string, IPromise> fetchEventDetail;
        public Action<string> joinEvent;
        public Action<bool> showChatWindow;
        public Func<string, string, bool, IPromise> fetchMessages;
        public Action<string, string, string, string> sendMessage;
        public Action<ShareType, string, string, string, string> share;
    }
}