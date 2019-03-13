using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class FetchArticlesResponse {
        public List<Article> items;
        public Dictionary<string, User> userMap;
    }

    [Serializable]
    public class FetchArticleDetailResponse {
        public Project project;
        
    }
    
    [Serializable]
    public class NotificationResponse {
        public int unreadCount;
        public int unseenCount;
        public string current;
        public string next;
        public int total;
        public int page;
        public int pageTotal;
        public List<Notification> results;
    }
}