using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelMentionScreenViewModel {
        public ChannelView channel;
        public Dictionary<string, ChannelMember> mentionSuggestions;
        public bool mentionLoading;
    }
}