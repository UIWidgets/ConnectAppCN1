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
        public User[] hosts;
    }
}