using System;

namespace ConnectApp.models {
    [Serializable]
    public class Reaction {
        public string id;
        public string type;
        public string messageId;
        public string channelId;
        public User user;
        public DateTime updatedTime;
    }
}