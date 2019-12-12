using System;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Reaction {
        public string id;
        public string type;
        public string messageId;
        public string channelId;
        public User user;
        public DateTime updatedTime;
        public int count;
        public string likeImage;
    }
}