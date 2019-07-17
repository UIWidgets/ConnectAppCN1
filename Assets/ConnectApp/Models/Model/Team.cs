using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Team {
        public string id;
        public string avatar;
        public string name;
        public string coverImage;
        public TeamStats stats;
        public List<User> followers;
        public bool? followersHasMore;
        public List<Article> articles;
        public bool? articlesHasMore;
        public bool? followTeamLoading;

        Team copyWith(
            string id = null,
            string avatar = null,
            string name = null,
            string coverImage = null,
            TeamStats stats = null,
            List<User> followers = null,
            bool? followersHasMore = null,
            List<Article> articles = null,
            bool? articlesHasMore = null,
            bool? followTeamLoading = null
        ) {
            return new Team {
                id = id ?? this.id,
                avatar = avatar ?? this.avatar,
                name = name ?? this.name,
                coverImage = coverImage ?? this.coverImage,
                stats = stats ?? this.stats,
                followers = followers ?? this.followers,
                followersHasMore = followersHasMore ?? this.followersHasMore,
                articles = articles ?? this.articles,
                articlesHasMore = articlesHasMore ?? this.articlesHasMore,
                followTeamLoading = followTeamLoading ?? this.followTeamLoading
            };
        }

        public Team Merge(Team other) {
            if (null == other) {
                return this;
            }

            return this.copyWith(
                id: other.id,
                avatar: other.avatar,
                name: other.name,
                coverImage: other.coverImage,
                stats: other.stats,
                followers: other.followers,
                followersHasMore: other.followersHasMore,
                articles: other.articles,
                articlesHasMore: other.articlesHasMore,
                followTeamLoading: other.followTeamLoading
            );
        }
    }

    [Serializable]
    public class TeamStats {
        public int followCount;
    }
}