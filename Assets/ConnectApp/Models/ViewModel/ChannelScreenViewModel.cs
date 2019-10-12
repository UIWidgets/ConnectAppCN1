using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelScreenViewModel {
        public ChannelView channel;
        public List<ChannelMessageView> messages;
        public List<ChannelMessageView> newMessages;
        public int newMessageCount;
        public string me;
        public bool messageLoading;
        public bool socketConnected;
    }
}