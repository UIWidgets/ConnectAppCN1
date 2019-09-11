using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    public class ChannelState {
        public List<Channel> publicChannels;
        public int publicChannelCurrentPage;
        public List<int> publicChannelPages;
        public int publicChannelTotal;
    }
}