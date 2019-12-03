using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelMentionScreenViewModel {
        public Dictionary<string, ChannelMember> channelMembers;
        public string me;
        public ChannelMember currentMember;
        public List<ChannelMember> mentionSuggestions;
        public Dictionary<string, User> userDict;
        public bool mentionLoading;
        public string lastMentionQuery;
        public bool mentionSearching;
        public List<ChannelMember> queryMentions;
    }
}