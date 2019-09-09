using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelMembersScreenViewModel {
        public Channel channel;
        public HashSet<string> followed;
    }
}