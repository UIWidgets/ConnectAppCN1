using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelScreenViewModel {
        public Channel channelInfo;
        public List<ChannelMessage> messages;
        public int newMessageCount;
        public User me;
    }
}