using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Message {
        public string id;
        public string parentMessageId;
        public string upperMessageId;
        public string channelId;
        public User author;
        public string content;
        public string nonce;
        public bool mentionEveryone;
        public List<string> replyMessageIds;
        public List<string> lowerMessageIds;
        public List<Reaction> reactions;
        public List<User> mentions;
        public string deletedTime;
        public string type;
        public bool deleted;
    }
}