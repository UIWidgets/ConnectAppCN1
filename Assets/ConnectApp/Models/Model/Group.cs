using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Group {
        public string id;
        public string avatar;
        public string name;
        public string slug;
        public string userId;
        public string coverImage;
        public string createdTime;
        public List<string> categoryIds;
        public string description;
        public List<string> preferredLanguages;
        public List<string> tags;
        public string privacy;
        public string postPermission;
        public string joinPermission;
        public string placeId;
        public int actions;
        public int unreadPosts;
        public string unityLicense;
    }

    [Serializable]
    public class GroupStats {
        public string id;
        public int membersCount;
        public int postsCount;
    }

    [Serializable]
    public class GroupMember {
        public string id;
        public string groupId;
        public string userId;
        public List<string> roles;
        public string status;
        public string email;
        public string createdTime;
        public string lastReadPostTime;
        public string emailSetting;
    }
}