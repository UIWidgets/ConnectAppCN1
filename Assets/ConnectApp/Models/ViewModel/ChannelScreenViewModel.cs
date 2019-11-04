using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelScreenViewModel {
        public ChannelView channel;
        public List<ChannelMessageView> messages;
        public List<ChannelMessageView> newMessages;
        public int newMessageCount;
        public User me;
        public bool messageLoading;
        public bool socketConnected;
        public bool netWorkConnected;
        public bool mentionAutoFocus;
        public string mentionUserId;
        public string mentionUserName;
        public Dictionary<string, ChannelMember> mentionSuggestion;
        public bool hasChannel;
        public bool channelError;
        public ChannelMessageView waitingMessage;
        public ChannelMessageView sendingMessage;
        public Dictionary<string, UserLicense> userLicenseDict;
    }
}