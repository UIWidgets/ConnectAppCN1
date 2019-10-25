using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelMentionScreenViewModel {
        public ChannelView channel;
        public Dictionary<string, ChannelMember> mentionSuggestions;
        public Dictionary<string, User> userDict;
        public bool mentionLoading;
        public string lastMentionQuery;
        public bool mentionSearching;
        public List<ChannelMember> queryMentions;
    }
}