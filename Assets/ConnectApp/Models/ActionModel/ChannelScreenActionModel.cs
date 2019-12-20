using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ChannelScreenActionModel : BaseActionModel {
        public Action<string> pushToUserDetail;
        public Action<string> pushToReactionsDetail;
        public Action pushToChannelDetail;
        public Action clearUnread;
        public Action reportLeaveBottom;
        public Action reportHitBottom;
        public Action popFromScreen;
        public Action ackMessage;
        public Action<string> openUrl;
        public Action<string, List<string>, Dictionary<string, byte[]>> browserImage;
        public Action<string> playVideo;
        public Func<string, string, string, string, IPromise> sendMessage;
        public Func<string, byte[], string, IPromise> sendImage;
        public Func<IPromise> fetchChannelInfo;
        public Func<string, string, IPromise> fetchMessages;
        public Func<IPromise> fetchMembers;
        public Func<IPromise> fetchMember;
        public Func<string, IPromise> deleteChannelMessage;
        public Action<ChannelMessageView> deleteLocalMessage;
        public Action pushToChannelMention;
        public Action clearLastChannelMention;
        public Action<ChannelMessageView> addLocalMessage;
        public Action<ChannelMessageView> resendMessage;
        public Action<ChannelMessageView, string> selectReactionFromMe;
        public Action<ChannelMessageView, string> cancelReactionFromMe;
    }
}