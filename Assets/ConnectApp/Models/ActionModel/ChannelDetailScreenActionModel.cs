using System;
using ConnectApp.Components;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ChannelDetailScreenActionModel : BaseActionModel {
        public Action<string> pushToUserDetail;
        public Action pushToChannelMembers;
        public Action pushToChannelIntroduction;
        public Func<IPromise> fetchMembers;
        public Action joinChannel;
        public Action leaveChannel;
        public Action<bool> updateTop;
        public Action<bool> updateMute;
        public Action<string> copyText;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
    }
}