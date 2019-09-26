using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;

namespace ConnectApp.Models.ViewModel {
    public class MessengerScreenViewModel {
        public int discoverPage;
        public int currentTabBarIndex;
        public List<ChannelView> joinedChannels;
        public List<ChannelView> popularChannels;
        public List<ChannelView> publicChannels;
    }
}