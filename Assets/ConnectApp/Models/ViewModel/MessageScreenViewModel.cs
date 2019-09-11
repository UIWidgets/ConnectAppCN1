using System;
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
        public List<ChannelView> channelInfo;
        public List<ChannelView> popularChannelInfo;
        public List<ChannelView> discoverChannelInfo;
    }

    public class ChannelView {
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

    public class ChannelMessageView {
        public User sender;
        public DateTime time;
        public ChannelMessageType type = ChannelMessageType.text;
        public string content;
        public long fileSize;
    }
}