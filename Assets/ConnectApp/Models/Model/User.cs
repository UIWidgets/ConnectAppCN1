using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class User {
        public string id;
        public string slug;
        public string type;
        public string username;
        public string fullName;
        public string name;
        public string title;
        public string avatar;
        public string coverImage;
        public string description;
        public int? articleCount;
        public int? followCount;
        public int? followingUsersCount;
        public int? followingTeamsCount;
        public int? likeCount;
        public int? appArticleLikedCount;
        public List<User> followingUsers;
        public bool? followingUsersHasMore;
        public List<User> followers;
        public bool? followersHasMore;
        public List<Team> followingTeams;
        public bool? followingTeamsHasMore;
        public List<Following> followings;
        public bool? followingsHasMore;
        public List<string> articleIds;
        public bool? articlesHasMore;
        public List<string> likeArticleIds;
        public bool? likeArticlesHasMore;
        public int? likeArticlesPage;
        public Dictionary<string, JobRole> jobRoleMap;
        public List<string> jobRoleIds;
        public bool? followUserLoading;
        public List<string> badges;
        public string errorCode;

        public User copyWith(
            string id = null,
            string type = null,
            string username = null,
            string fullName = null,
            string name = null,
            string title = null,
            string avatar = null,
            string coverImage = null,
            string description = null,
            int? articleCount = null,
            int? followCount = null,
            int? followingUsersCount = null,
            int? followingTeamsCount = null,
            int? likeCount = null,
            int? appArticleLikedCount = null,
            List<User> followingUsers = null,
            bool? followingUsersHasMore = null,
            List<User> followers = null,
            bool? followersHasMore = null,
            List<Team> followingTeams = null,
            bool? followingTeamsHasMore = null,
            List<Following> followings = null,
            bool? followingsHasMore = null,
            List<string> articleIds = null,
            bool? articlesHasMore = null,
            List<string> likeArticleIds = null,
            bool? likeArticlesHasMore = null,
            int? likeArticlesPage = null,
            Dictionary<string, JobRole> jobRoleMap = null,
            List<string> jobRoleIds = null,
            bool? followUserLoading = null,
            List<string> badges = null,
            string errorCode = null
        ) {
            return new User {
                id = id ?? this.id,
                type = type ?? this.type,
                username = username ?? this.username,
                fullName = fullName ?? this.fullName,
                name = name ?? this.name,
                title = title ?? this.title,
                avatar = avatar ?? this.avatar,
                coverImage = coverImage ?? this.coverImage,
                description = description ?? this.description,
                articleCount = articleCount ?? this.articleCount,
                followCount = followCount ?? this.followCount,
                followingUsersCount = followingUsersCount ?? this.followingUsersCount,
                followingTeamsCount = followingTeamsCount ?? this.followingTeamsCount,
                likeCount = likeCount ?? this.likeCount,
                // 0 or null covering the fields is prohibited
                appArticleLikedCount = appArticleLikedCount != null && appArticleLikedCount > 0 ? appArticleLikedCount : this.appArticleLikedCount,
                followingUsers = followingUsers ?? this.followingUsers,
                followingUsersHasMore = followingUsersHasMore ?? this.followingUsersHasMore,
                followers = followers ?? this.followers,
                followersHasMore = followersHasMore ?? this.followersHasMore,
                followingTeams = followingTeams ?? this.followingTeams,
                followingTeamsHasMore = followingTeamsHasMore ?? this.followingTeamsHasMore,
                followings = followings ?? this.followings,
                followingsHasMore = followingsHasMore ?? this.followingsHasMore,
                articleIds = articleIds ?? this.articleIds,
                articlesHasMore = articlesHasMore ?? this.articlesHasMore,
                likeArticleIds = likeArticleIds ?? this.likeArticleIds,
                likeArticlesHasMore = likeArticlesHasMore ?? this.likeArticlesHasMore,
                likeArticlesPage = likeArticlesPage ?? this.likeArticlesPage,
                jobRoleMap = jobRoleMap ?? this.jobRoleMap,
                jobRoleIds = jobRoleIds ?? this.jobRoleIds,
                followUserLoading = followUserLoading ?? this.followUserLoading,
                badges = badges ?? this.badges,
                errorCode = errorCode ?? this.errorCode
            };
        }

        public User Merge(User other) {
            if (null == other) {
                return this;
            }

            return this.copyWith(
                id: other.id,
                type: other.type,
                username: other.username,
                fullName: other.fullName,
                name: other.name,
                title: other.title,
                avatar: other.avatar,
                coverImage: other.coverImage,
                description: other.description,
                articleCount: other.articleCount,
                followCount: other.followCount,
                followingUsersCount: other.followingUsersCount,
                followingTeamsCount: other.followingTeamsCount,
                likeCount: other.likeCount,
                appArticleLikedCount: other.appArticleLikedCount,
                followingUsers: other.followingUsers,
                followingUsersHasMore: other.followingUsersHasMore,
                followers: other.followers,
                followersHasMore: other.followersHasMore,
                followingTeams: other.followingTeams,
                followingTeamsHasMore: other.followingTeamsHasMore,
                followings: other.followings,
                followingsHasMore: other.followingsHasMore,
                articleIds: other.articleIds,
                articlesHasMore: other.articlesHasMore,
                likeArticleIds: other.likeArticleIds,
                likeArticlesHasMore: other.likeArticlesHasMore,
                likeArticlesPage: other.likeArticlesPage,
                jobRoleMap: other.jobRoleMap,
                jobRoleIds: other.jobRoleIds,
                followUserLoading: other.followUserLoading,
                badges: other.badges,
                errorCode: other.errorCode
            );
        }
    }

    [Serializable]
    public class UserLicense {
        public string userId;
        public string license;
    }

    [Serializable]
    public class Following {
        public string id;
        public string userId;
        public string type;
        public string followeeId;
        public DateTime createdTime;
    }
}