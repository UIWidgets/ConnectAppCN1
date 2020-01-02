using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class MessengerScreenViewModel {
        public bool channelLoading;
        public string myUserId;
        public int currentTabBarIndex;
        public int page;
        public bool hasMore;
        public bool socketConnected;
        public bool networkConnected;
        public bool dismissNoNetworkBanner;
        public Dictionary<string, string> lastMessageMap;
        public List<ChannelView> joinedChannels;
        public List<ChannelView> popularChannels;
        public List<ChannelView> publicChannels;
    }
}