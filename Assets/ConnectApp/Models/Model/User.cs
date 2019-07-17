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
        public int? followCount;
        public int? followingCount;
        public List<User> followings;
        public bool followingsHasMore;
        public List<User> followers;
        public bool followersHasMore;
        public List<Article> articles;
        public bool articlesHasMore;
        public Dictionary<string, JobRole> jobRoleMap;
        public List<string> jobRoleIds;
        public bool followUserLoading;

        User copyWith(
            string id = null,
            string type = null,
            string username = null,
            string fullName = null,
            string name = null,
            string title = null,
            string avatar = null,
            string coverImage = null,
            string description = null,
            int? followCount = null,
            int? followingCount = null,
            List<User> followings = null,
            bool? followingsHasMore = null,
            List<User> followers = null,
            bool? followersHasMore = null,
            List<Article> articles = null,
            bool? articlesHasMore = null,
            Dictionary<string, JobRole> jobRoleMap = null,
            List<string> jobRoleIds = null,
            bool? followUserLoading = null
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
                followCount = followCount ?? this.followCount,
                followingCount = followingCount ?? this.followingCount,
                followings = followings ?? this.followings,
                followingsHasMore = followingsHasMore ?? this.followingsHasMore,
                followers = followers ?? this.followers,
                followersHasMore = followersHasMore ?? this.followersHasMore,
                articles = articles ?? this.articles,
                articlesHasMore = articlesHasMore ?? this.articlesHasMore,
                jobRoleMap = jobRoleMap ?? this.jobRoleMap,
                jobRoleIds = jobRoleIds ?? this.jobRoleIds,
                followUserLoading = followUserLoading ?? this.followUserLoading
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
                followCount: other.followCount,
                followingCount: other.followingCount,
                followings: other.followings,
                followingsHasMore: other.followingsHasMore,
                followers: other.followers,
                followersHasMore: other.followersHasMore,
                articles: other.articles,
                articlesHasMore: other.articlesHasMore,
                jobRoleMap: other.jobRoleMap,
                jobRoleIds: other.jobRoleIds,
                followUserLoading: other.followUserLoading
            );
        }
    }
}