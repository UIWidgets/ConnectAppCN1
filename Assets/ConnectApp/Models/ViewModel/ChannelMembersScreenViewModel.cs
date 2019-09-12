using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelMembersScreenViewModel {
        public ChannelView channel;
        public HashSet<string> followed;
    }
}