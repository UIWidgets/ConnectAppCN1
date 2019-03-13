using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class Notification {
        public int unreadCount;
        public int unseenCount;
        public string current;
        public string next;
        public int total;
        public int page;
        public int pageTotal;
        public List<NotificationResult> results;
    }
    
    [Serializable]
    public class NotificationResult {
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
        public string projectId;
        public string projectTitle;
        public string role;
        public string userId;
        public string username;
    }
}