using System;
using System.Collections.Generic;

namespace ConnectApp.models
{
    [Serializable]
    public class LiveInfo
    {
        public string id;
        public string background;
        public string createdTime;
        public string shortDescription;
        public string title;
        public bool live;
        public string channelId;
        public User user;
        public List<User> hosts;
        public string content;
        public Dictionary<string, dynamic> contentMap;
    }
}