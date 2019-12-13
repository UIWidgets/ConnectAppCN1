using System;
using System.Collections.Generic;
using ConnectApp.Models.Api;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Project {
        public Article projectData;
        public List<Article> projects;
        public Dictionary<string, ContentMap> contentMap;
        public Dictionary<string, User> userMap;
        public Dictionary<string, UserLicense> userLicenseMap;
        public Dictionary<string, VideoSliceMap> videoSliceMap;
        public Dictionary<string, string> videoPosterMap;
        public Dictionary<string, Team> teamMap;
        public Dictionary<string, bool> followMap;
        public Dictionary<string, User> mentionUsers;
        public FetchCommentsResponse comments;
        public string channelId;
        public bool like;
        public int appCurrentUserLikeCount;
        public List<Favorite> favoriteList;
        public bool edit;
    }
}