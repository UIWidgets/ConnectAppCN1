using System;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ActionModel {
    public class ChannelMentionScreenActionModel : BaseActionModel {
        public Action<string, string, ChannelMember> chooseMentionConfirm;
        public Action chooseMentionCancel;
        public Action startLoadingMention;
        public Action<string> startQueryMention;
        public Action<string> updateMentionQuery;
        public Action clearQueryMention;
    }
}