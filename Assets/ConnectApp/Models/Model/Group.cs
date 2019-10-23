using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Group {
        string id;
        string avatar;
        string name;
        string slug;
        string userId;
        string coverImage;
        string createdTime;
        List<string> categoryIds;
        string description;
        List<string> preferredLanguages;
        List<string> tags;
        string privacy;
        string postPermission;
        string joinPermission;
        string placeId;
        int actions;
        int unreadPosts;
        string unityLicense;
    }

    [Serializable]
    public class GroupStats {
        string id;
        int membersCount;
        int postsCount;
    }

    [Serializable]
    public class GroupMember {
        string id;
        string groupId;
        string userId;
        List<string> roles;
        string status;
        string email;
        string createdTime;
        string lastReadPostTime;
        string emailSetting;
    }
}