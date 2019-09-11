using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Channel {
        public string id;
        public string groupId;
        public string thumbnail;
        public string imageUrl;
        public string name;
        public string topic;
        public int memberCount;
        public bool isMute;
        public bool live;
        public ChannelMessage lastMessage;
//        public bool isHot;
//        public string latestMessage;
//        public string time;
//        public int unread = 0;
//        public string introduction;
//        public bool isTop = false;
//        public bool silenced = false;
//        public bool joined = false;
//        public bool atMe = false;
//        public bool atAll = false;
//        public List<User> members;
//        public int numAdmins;
    }

    [Serializable]
    public class ChannelMessage {
        public string id;
        public string nonce;
        public string channelId;
        public string content;
        public User author;
        // public User sender;
        // public DateTime time;
        // public ChannelMessageType type = ChannelMessageType.text;
        // public long fileSize;
    }
}