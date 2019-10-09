using System;

namespace ConnectApp.Models.ActionModel {
    public class ChannelDetailScreenActionModel : BaseActionModel {
        public Action<string> pushToUserDetail;
        public Action pushToChannelMembers;
        public Action pushToChannelIntroduction;
        public Action joinChannel;
        public Action leaveChannel;
        public Action<bool> updateTop;
    }
}
