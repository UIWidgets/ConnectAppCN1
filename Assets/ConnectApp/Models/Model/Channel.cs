using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Channel {
        public string imageUrl;
        public string name;
        public bool isHot;
        public string latestMessage;
        public string time;
        public int unread = 0;
        public string introduction;
        public bool isTop = false;
        public bool silenced = false;
        public bool joined = false;
        public bool atMe = false;
        public bool atAll = false;
        public List<User> members;
        public int numAdmins;
    }

    public enum ChannelMessageType {
        text,
        image,
        file
    }

    [Serializable]
    public class ChannelMessage {
        public User sender;
        public DateTime time;
        public ChannelMessageType type;
        public string content;
    }
}