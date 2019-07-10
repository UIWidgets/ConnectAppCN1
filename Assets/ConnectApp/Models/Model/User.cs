using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class User {
        public string id;
        public string type;
        public string username;
        public string fullName;
        public string name;
        public string title;
        public string avatar;
        public string coverImage;
        public string description;
        public int followCount;
        public int followingCount;
        public List<User> followings;
        public bool followingsHasMore;
        public List<User> followers;
        public bool followersHasMore;
        public List<Article> articles;
        public bool articlesHasMore;
        public Dictionary<string, JobRole> jobRoleMap;
        public List<string> jobRoleIds;
    }
}