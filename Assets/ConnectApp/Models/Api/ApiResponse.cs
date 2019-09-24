using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.Api {
    [Serializable]
    public class FetchArticlesResponse {
        public List<HottestItem> hottests;
        public Dictionary<string, Article> projectMap;
        public Dictionary<string, User> userMap;
        public Dictionary<string, Team> teamMap;
        public Dictionary<string, bool> followMap;
        public Dictionary<string, bool> likeMap;
        public bool hottestHasMore;
        public bool feedHasNew;
    }

    [Serializable]
    public class FetchFollowArticlesResponse {
        public Dictionary<string, User> userMap;
        public Dictionary<string, UserLicense> userLicenseMap;
        public Dictionary<string, Team> teamMap;
        public Dictionary<string, bool> followMap;
        public Dictionary<string, bool> likeMap;
        public Dictionary<string, Article> projectSimpleMap;
        public List<Feed> feeds;
        public bool feedHasNew;
        public bool feedIsFirst;
        public bool feedHasMore;
        public List<HottestItem> hotItems;
        public bool hotHasMore;
        public int hotPage;
    }

    [Serializable]
    public class FetchArticleDetailResponse {
        public Project project;
    }

    [Serializable]
    public class FetchEventsResponse {
        public FetchEventListResponse events;
        public Dictionary<string, User> userMap;
        public Dictionary<string, Place> placeMap;
    }

    [Serializable]
    public class FetchEventListResponse {
        public List<IEvent> items;
        public int total;
    }

    [Serializable]
    public class FetchNotificationResponse {
        public int page;
        public int pageTotal;
        public List<Notification> results;
        public Dictionary<string, User> userMap;
    }

    [Serializable]
    public class FetchSearchArticleResponse {
        public List<Article> projects;
        public Dictionary<string, User> userMap;
        public Dictionary<string, Team> teamMap;
        public Dictionary<string, Place> placeMap;
        public Dictionary<string, bool> likeMap;
        public int currentPage;
        public List<int> pages;
    }

    [Serializable]
    public class FetchSearchUserResponse {
        public bool hasMore;
        public List<User> users;
        public Dictionary<string, UserLicense> userLicenseMap;
        public Dictionary<string, bool> followingMap;
    }

    [Serializable]
    public class FetchSearchTeamResponse {
        public List<Team> teams;
        public Dictionary<string, bool> followingMap;
        public bool hasMore;
    }

    [Serializable]
    public class FetchSocketUrlResponse {
        public string url;
    }

    [Serializable]
    public class FetchCommentsResponse {
        public List<Message> items;
        public List<Message> parents;
        public Dictionary<string, UserLicense> userLicenseMap;
        public string currOldestMessageId;
        public bool hasMore;
        public bool hasMoreNew;
    }

    [Serializable]
    public class FetchSendMessageResponse {
        public string channelId;
        public string content;
        public string nonce;
    }

    [Serializable]
    public class FetchUserProfileResponse {
        public User user;
        public Dictionary<string, bool> followMap;
        public int followingCount;
        public bool followingsHasMore;
        public bool followersHasMore;
        public int followingTeamsCount;
        public List<Team> followingTeams;
        public bool followingTeamsHasMore;
        public string currentUserId;
        public Dictionary<string, Team> teamMap;
        public Dictionary<string, Place> placeMap;
        public Dictionary<string, JobRole> jobRoleMap;
        public Dictionary<string, UserLicense> userLicenseMap;
    }

    [Serializable]
    public class FetchUserArticleResponse {
        public List<string> projectList;
        public Dictionary<string, User> userMap;
        public Dictionary<string, Team> teamMap;
        public Dictionary<string, Place> placeMap;
        public Dictionary<string, bool> likeMap;
        public Dictionary<string, bool> followMap;
        public Dictionary<string, Article> projectMap;
        public bool hasMore;
    }

    [Serializable]
    public class FetchFollowingResponse {
        public List<Following> followings;
        public bool hasMore;
        public Dictionary<string, bool> followMap;
        public Dictionary<string, User> userMap;
        public Dictionary<string, Team> teamMap;
    }

    [Serializable]
    public class FetchFollowingUserResponse {
        public List<User> followings;
        public Dictionary<string, UserLicense> userLicenseMap;
        public bool followingsHasMore;
        public Dictionary<string, bool> followMap;
    }

    [Serializable]
    public class FetchFollowerResponse {
        public List<User> followers;
        public Dictionary<string, UserLicense> userLicenseMap;
        public bool followersHasMore;
        public Dictionary<string, bool> followMap;
    }

    [Serializable]
    public class FetchFollowingTeamResponse {
        public List<Team> followingTeams;
        public bool followingTeamsHasMore;
        public Dictionary<string, bool> followMap;
        public Dictionary<string, Place> placeMap;
    }

    [Serializable]
    public class FetchEditPersonalInfoResponse {
        public User user;
        public Dictionary<string, Place> placeMap;
    }

    [Serializable]
    public class FetchTeamResponse {
        public Team team;
        public Dictionary<string, Place> placeMap;
        public Dictionary<string, bool> followMap;
    }

    [Serializable]
    public class FetchTeamArticleResponse {
        public Dictionary<string, bool> likeMap;
        public List<Article> projects;
        public Dictionary<string, UserLicense> userLicenseMap;
        public bool projectsHasMore;
    }

    [Serializable]
    public class FetchTeamMemberResponse {
        public List<Member> members;
        public Dictionary<string, User> userMap;
        public Dictionary<string, bool> followMap;
        public bool hasMore;
    }

    [Serializable]
    public class ErrorResponse {
        public string errorCode;
    }

    [Serializable]
    public class FetchInitDataResponse {
        public string VS;
        public List<bool> showEggs;
        public bool scanEnabled;
        public InitDataConfig config;
    }

    [Serializable]
    public class InitDataConfig {
        public Dictionary<string, bool> eggs;
        public bool scan;
    }

    [Serializable]
    public class UpdateAvatarResponse {
        public string avatar;
        public int profilePercent;
        public string nextStep;
    }

    [Serializable]
    public class FetchFavoriteTagsResponse {
        public List<FavoriteTag> favoriteTags;
        public bool hasMore;
    }

    [Serializable]
    public class FetchFavoriteDetailResponse {
        public Dictionary<string, User> userMap;
        public Dictionary<string, Team> teamMap;
        public Dictionary<string, FavoriteTag> tagMap;
        public Dictionary<string, Article> projectSimpleMap;
        public List<Favorite> favorites;
        public bool hasMore;
    }
}