using System.Collections.Generic;

namespace ConnectApp.Models.ViewModel {
    public class DiscoverChannelsScreenViewModel {
        public List<ChannelView> publicChannels;
        public List<ChannelView> joinedChannels;
        public int page;
    }
}