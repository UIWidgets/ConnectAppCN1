using System;
using System.Collections.Generic;
using ConnectApp.Models.Api;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Article {
        public string id;
        public string slug;
        public string teamId;
        public string ownerType;
        public string title;
        public string userId;
        public string fullName;
        public string subTitle;
        public int viewCount;
        public int likeCount;
        public int appLikeCount;
        public int commentCount;
        public Thumbnail thumbnail;
        public DateTime updatedTime;
        public DateTime createdTime;
        public DateTime publishedTime;
        public DateTime lastPublishedTime;
        public string type;
        public string bodyType;
        public string body;
        public string markdownBody;
        public string markdownPreviewBody;
        public bool? like;
        public int? appCurrentUserLikeCount;
        public List<Favorite> favorites;
        public List<string> projectIds;
        public string channelId;
        public Dictionary<string, ContentMap> contentMap;
        public Dictionary<string, VideoSliceMap> videoSliceMap;
        public Dictionary<string, string> videoPosterMap;
        public string currOldestMessageId;
        public bool hasMore;
        public bool isNotFirst;

        Article copyWith(
            string id = null,
            string slug = null,
            string teamId = null,
            string ownerType = null,
            string title = null,
            string userId = null,
            string fullName = null,
            string subTitle = null,
            int? viewCount = null,
            int? likeCount = null,
            int? appLikeCount = null,
            int? commentCount = null,
            Thumbnail thumbnail = null,
            DateTime? updatedTime = null,
            DateTime? createdTime = null,
            DateTime? publishedTime = null,
            DateTime? lastPublishedTime = null,
            string type = null,
            string body = null,
            string bodyType = null,
            string markdownPreviewBody = null,
            bool? like = null,
            int? appCurrentUserLikeCount = null,
            List<Favorite> favorites = null,
            List<string> projectIds = null,
            string channelId = null,
            Dictionary<string, ContentMap> contentMap = null,
            string currOldestMessageId = null,
            bool? hasMore = null,
            bool? isNotFirst = null
        ) {
            return new Article {
                id = id ?? this.id,
                slug = slug ?? this.slug,
                teamId = teamId ?? this.teamId,
                ownerType = ownerType ?? this.ownerType,
                title = title ?? this.title,
                userId = userId ?? this.userId,
                fullName = fullName ?? this.fullName,
                subTitle = subTitle ?? this.subTitle,
                viewCount = viewCount ?? this.viewCount,
                likeCount = likeCount ?? this.likeCount,
                appLikeCount = appLikeCount ?? this.appLikeCount,
                commentCount = commentCount ?? this.commentCount,
                thumbnail = thumbnail ?? this.thumbnail,
                updatedTime = updatedTime ?? this.updatedTime,
                createdTime = createdTime ?? this.createdTime,
                publishedTime = publishedTime ?? this.publishedTime,
                lastPublishedTime = lastPublishedTime ?? this.lastPublishedTime,
                type = type ?? this.type,
                bodyType = bodyType ?? this.bodyType,
                body = body ?? this.body,
                markdownPreviewBody = markdownPreviewBody ?? this.markdownPreviewBody,
                like = like ?? this.like,
                appCurrentUserLikeCount = appCurrentUserLikeCount ?? this.appCurrentUserLikeCount,
                favorites = favorites ?? this.favorites,
                projectIds = projectIds ?? this.projectIds,
                channelId = channelId ?? this.channelId,
                contentMap = contentMap ?? this.contentMap,
                currOldestMessageId = currOldestMessageId ?? this.currOldestMessageId,
                hasMore = hasMore ?? this.hasMore,
                isNotFirst = isNotFirst ?? this.isNotFirst
            };
        }

        public Article Merge(Article other) {
            if (null == other) {
                return this;
            }

            return this.copyWith(
                id: other.id,
                slug: other.slug,
                teamId: other.teamId,
                ownerType: other.ownerType,
                title: other.title,
                userId: other.userId,
                fullName: other.fullName,
                subTitle: other.subTitle,
                viewCount: other.viewCount,
                likeCount: other.likeCount,
                appLikeCount: other.appLikeCount,
                commentCount: other.commentCount,
                thumbnail: other.thumbnail,
                updatedTime: other.updatedTime,
                createdTime: other.createdTime,
                publishedTime: other.publishedTime,
                lastPublishedTime: other.lastPublishedTime,
                type: other.type,
                body: other.body,
                bodyType: other.bodyType,
                other.markdownPreviewBody.isEmpty() ? this.markdownPreviewBody : other.markdownPreviewBody,
                like: other.like,
                appCurrentUserLikeCount: other.appCurrentUserLikeCount,
                favorites: other.favorites,
                projectIds: other.projectIds,
                channelId: other.channelId,
                contentMap: other.contentMap,
                currOldestMessageId: other.currOldestMessageId,
                hasMore: other.hasMore,
                isNotFirst: other.isNotFirst
            );
        }
    }

    [Serializable]
    public class Feed {
        public List<string> itemIds;
        public string actionTime;
    }

    [Serializable]
    public class HottestItem {
        public string id;
        public string itemId;
    }

    [Serializable]
    public class UserArticle {
        public int total;
        public List<Article> list;
    }

    [Serializable]
    public class HomeRankData {
        public HomeSlider homeSlider;
        public HomeCollection homeTopCollection;
        public HomeCollection homeCollection;
        public FetchBloggerResponse homeBlogger;
        public string searchSuggest;
        public string dailySelectionId;
        public DateTime? leaderboardUpdatedTime;
    }

    [Serializable]
    public class HomeSlider {
        public List<RankData> rankList;
    }

    [Serializable]
    public class HomeCollection {
        public List<RankData> rankList;
        public Dictionary<string, FavoriteTagArticle> favoriteTagArticleMap;
        public Dictionary<string, FavoriteTag> favoriteTagMap;
        public Dictionary<string, bool> collectedTagMap;
    }
}