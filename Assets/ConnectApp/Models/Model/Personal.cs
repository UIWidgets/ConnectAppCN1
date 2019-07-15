using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Personal {
        public User user;
        public int followingCount;
        public List<User> followings;
        public bool followingsHasMore;
        public List<User> followers;
        public bool followersHasMore;
        public List<Article> articles;
        public bool articlesHasMore;
        public string currentUserId;
        public Dictionary<string, JobRole> jobRoleMap;
    }
}