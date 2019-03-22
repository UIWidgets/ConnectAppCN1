using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class FetchArticlesResponse {
        public List<Article> items;
        public Dictionary<string, User> userMap;
        public Dictionary<string, Team> teamMap;
    }

    [Serializable]
    public class FetchArticleDetailResponse {
        public Project project;
    }

    [Serializable]
    public class FetchNotificationResponse {
        public int unreadCount;
        public int unseenCount;
        public string current;
        public string next;
        public int total;
        public int page;
        public int pageTotal;
        public List<Notification> results;
    }

    [Serializable]
    public class FetchSearchResponse {
        public List<Article> projects;
        public Dictionary<string, User> userMap;
        public Dictionary<string, Team> teamMap;
    }
}