using System;

namespace ConnectApp.Models.ActionModel {
    public class ChannelMentionScreenActionModel : BaseActionModel {
        public Action<string, string> chooseMentionConfirm;
        public Action chooseMentionCancel;
        public Action startLoadingMention;
    }
}