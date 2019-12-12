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
        public List<TeamMember> members;
        public bool? membersHasMore;
        public List<string> articleIds;
        public bool? articlesHasMore;
        public bool? followTeamLoading;
        public List<string> badges;
        public bool? isDetail;
        public string errorCode;

        public Team copyWith(
            string id = null,
            string avatar = null,
            string name = null,
            string coverImage = null,
            TeamStats stats = null,
            List<User> followers = null,
            bool? followersHasMore = null,
            List<TeamMember> members = null,
            bool? membersHasMore = null,
            List<string> articleIds = null,
            bool? articlesHasMore = null,
            bool? followTeamLoading = null,
            List<string> badges = null,
            bool? isDetail = null,
            string errorCode = null
        ) {
            return new Team {
                id = id ?? this.id,
                avatar = avatar ?? this.avatar,
                name = name ?? this.name,
                coverImage = coverImage ?? this.coverImage,
                stats = stats ?? this.stats,
                followers = followers ?? this.followers,
                followersHasMore = followersHasMore ?? this.followersHasMore,
                members = members ?? this.members,
                membersHasMore = membersHasMore ?? this.membersHasMore,
                articleIds = articleIds ?? this.articleIds,
                articlesHasMore = articlesHasMore ?? this.articlesHasMore,
                followTeamLoading = followTeamLoading ?? this.followTeamLoading,
                badges = badges ?? this.badges,
                isDetail = isDetail ?? this.isDetail,
                errorCode = errorCode ?? this.errorCode
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
                members: other.members,
                membersHasMore: other.membersHasMore,
                articleIds: other.articleIds,
                articlesHasMore: other.articlesHasMore,
                followTeamLoading: other.followTeamLoading,
                badges: other.badges,
                isDetail: other.isDetail,
                errorCode: other.errorCode
            );
        }
    }

    [Serializable]
    public class TeamStats {
        public int followCount;
        public int membersCount;

        public TeamStats copyWith(
            int? followCount = null,
            int? membersCount = null
        ) {
            return new TeamStats {
                followCount = followCount ?? this.followCount,
                membersCount = membersCount ?? this.membersCount
            };
        }

        public TeamStats Merge(TeamStats other) {
            if (null == other) {
                return this;
            }

            return this.copyWith(
                followCount: other.followCount,
                membersCount: other.membersCount
            );
        }
    }

    [Serializable]
    public class TeamMember {
        public string id;
        public string userId;
        public string status;
        public string email;
        public string invitedBy;
        public List<string> role;
    }
}