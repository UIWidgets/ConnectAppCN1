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
    }

    [Serializable]
    public class ChannelMessage {
        public string id;
        public string type;
        public string nonce;
        public string channelId;
        public string content;
        public User author;
        public List<Attachment> attachments;
        public bool mentionEveryone;
        public List<User> mentions;
        public bool starred;
        public List<string> replyMessageIds;
        public List<string> lowerMessageIds;
        public List<User> replyUsers;
        public List<User> lowerUsers;
        public List<Reaction> reactions;
        public bool pending;
        public string deletedTime;
    }

    [Serializable]
    public class Attachment {
        public string id;
        public string url;
        public string filename;
        public string contentType;
        public int width;
        public int height;
        public int size;
        public int duration;
    }
}