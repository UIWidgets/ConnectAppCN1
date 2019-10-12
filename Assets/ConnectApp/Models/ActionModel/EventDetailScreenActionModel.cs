using System;
using ConnectApp.Components;
using ConnectApp.Models.State;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class EventDetailScreenActionModel : BaseActionModel {
        public Action<string> pushToUserDetail;
        public Action<string> openUrl;
        public Action<string> browserImage;
        public Action<string> copyText;
        public Action startFetchEventDetail;
        public Func<string, EventType, IPromise> fetchEventDetail;
        public Action startJoinEvent;
        public Func<string, IPromise> joinEvent;
        public Action<bool> showChatWindow;
        public Action startFetchMessages;
        public Func<string, string, bool, IPromise> fetchMessages;
        public Action startSendMessage;
        public Func<string, string, string, string, IPromise> sendMessage;
        public Func<ShareType, string, string, string, string, string, IPromise> shareToWechat;
    }
}