using System;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Notification {
        public string id;
        public string userId;
        public string read;
        public string seen;
        public string type;
        public NotificationData data;
        public DateTime createdTime;
        public DateTime updatedTime;
    }

    [Serializable]
    public class NotificationData {
        public string id;
        public string fullname;
        public string avatarWithCDN;
        public string teamAvatarWithCDN;
        public string projectId;
        public string projectTitle;
        public string role;
        public string userId;
        public string username;
        public string comment;
        public string parentComment;
        public string upperComment;
        public string teamName;
        public string teamId;
        public string upperMessageId;
        public string parentMessageId;
    }
}