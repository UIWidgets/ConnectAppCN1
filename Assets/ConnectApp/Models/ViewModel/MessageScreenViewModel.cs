using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class MessageScreenViewModel {
        public bool notificationLoading;
        public int page;
        public int pageTotal;
        public List<Notification> notifications;
        public List<User> mentions;
        public Dictionary<string, User> userDict;
        public List<ChannelInfo> channelInfo;
        public List<ChannelInfo> popularChannelInfo;
        public List<ChannelInfo> discoverChannelInfo;
    }

    public class ChannelInfo {
        public string imageUrl;
        public string name;
        public int size;
        public bool isHot;
        public string latestMessage;
        public string time;
        public int unread = 0;
    }
}
