using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class IEvent {
        public string id;
        public User user;
        public string title;
        public string background;
        public int participantsCount;
        public int onlineMemberCount;
        public int recordWatchCount;
        public DateTime createdTime;
        public string shortDescription;
        public bool live;
        public string channelId;
        public List<User> hosts;
        public string content;
        public Dictionary<string, ContentMap> contentMap;
    }
}