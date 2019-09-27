using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ChannelMembersScreenViewModel {
        public ChannelView channel;
        public Dictionary<string, bool> followed;
        public Dictionary<string, User> userDict;
        public List<ChannelMember> normalMembers;
        public List<ChannelMember> specialMembers;
        public bool isLoggedIn;
        public string currentUserId;
    }
}