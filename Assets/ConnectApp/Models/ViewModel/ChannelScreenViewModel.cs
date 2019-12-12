using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelScreenViewModel {
        public ChannelView channel;
        public string channelId;
        public List<ChannelMessageView> messages;
        public List<ChannelMessageView> newMessages;
        public User me;
        public bool channelInfoLoading;
        public bool messageLoading;
        public bool socketConnected;
        public bool networkConnected;
        public bool dismissNoNetworkBanner;
        public bool mentionAutoFocus;
        public string mentionUserId;
        public string mentionUserName;
        public List<ChannelMember> mentionSuggestion;
        public bool hasChannel;
        public ChannelMessageView waitingMessage;
        public ChannelMessageView sendingMessage;
        public Dictionary<string, UserLicense> userLicenseDict;
    }
}