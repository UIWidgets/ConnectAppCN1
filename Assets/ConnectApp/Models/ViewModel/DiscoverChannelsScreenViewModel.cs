using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class DiscoverChannelsScreenViewModel {
        public List<ChannelView> publicChannels;
        public Dictionary<string, string> lastMessageMap;
        public int page;
        public bool hasMore;
    }
}