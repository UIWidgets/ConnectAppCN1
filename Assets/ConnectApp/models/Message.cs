using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class Message {
        public string id;
        public string parentMessageId;
        public string channelId;
        public User author;
        public string content;
        public string nonce;
        public bool mentionEveryone;
        public List<string> replyMessageIds;
        public List<Reaction> reactions;
        public List<User> mentions;
    }
}