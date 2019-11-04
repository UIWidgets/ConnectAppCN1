using System;

namespace ConnectApp.Models.ActionModel {
    public class ChannelMentionScreenActionModel : BaseActionModel {
        public Action<string, string> chooseMentionConfirm;
        public Action chooseMentionCancel;
        public Action startLoadingMention;
        public Action<string> startQueryMention;
        public Action<string> updateMentionQuery;
        public Action clearQueryMention;
    }
}