using System;
using ConnectApp.components;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class EventDetailScreenActionModel {
        public Action mainRouterPop;
        public Action pushToLogin;
        public Action<string> openUrl;
        public Action startFetchEventDetail;
        public Func<string, IPromise> fetchEventDetail;
        public Action startJoinEvent;
        public Func<string, IPromise> joinEvent;
        public Action<bool> showChatWindow;
        public Func<string, string, bool, IPromise> fetchMessages;
        public Func<string, string, string, string, IPromise> sendMessage;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
    }
}