using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ChannelScreenActionModel : BaseActionModel {
        public Action<string> pushToUserDetail;
        public Action pushToChannelDetail;
        public Action startSendMessage;
        public Action clearUnread;
        public Action reportLeaveBottom;
        public Action reportHitBottom;
        public Action<string> openUrl;
        public Func<string, string, string, string, IPromise> sendMessage;
        public Func<string, string, string, IPromise> sendImage;
        public Func<string, string, IPromise> fetchMessages;
        public Func<IPromise> fetchMembers;
        public Action pushToChannelMention;
        public Action clearLastChannelMention;
    }
}