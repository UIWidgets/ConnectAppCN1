using System;

namespace ConnectApp.Models.ActionModel {
    public class ChannelMentionScreenActionModel : BaseActionModel {
        public Action<string> chooseMentionConfirm;
        public Action chooseMentionCancel;
    }
}