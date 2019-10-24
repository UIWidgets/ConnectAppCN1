using System;
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
    }
}
