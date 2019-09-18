using System;
using System.Collections.Generic;

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
        public int commentCount;
        public Thumbnail thumbnail;
        public DateTime updatedTime;
        public DateTime createdTime;
        public DateTime publishedTime;
        public DateTime lastPublishedTime;
        public string type;
        public string body;
        public bool like;
        public List<string> projectIds;
        public string channelId;
        public Dictionary<string, ContentMap> contentMap;
        public Dictionary<string, VideoSliceMap> videoSliceMap;
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
            int? commentCount = null,
            Thumbnail thumbnail = null,
            DateTime? updatedTime = null,
            DateTime? createdTime = null,
            DateTime? publishedTime = null,
            DateTime? lastPublishedTime = null,
            string type = null,
            string body = null,
            bool? like = null,
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
                commentCount = commentCount ?? this.commentCount,
                thumbnail = thumbnail ?? this.thumbnail,
                updatedTime = updatedTime ?? this.updatedTime,
                createdTime = createdTime ?? this.createdTime,
                publishedTime = publishedTime ?? this.publishedTime,
                lastPublishedTime = lastPublishedTime ?? this.lastPublishedTime,
                type = type ?? this.type,
                body = body ?? this.body,
                like = like ?? this.like,
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
                commentCount: other.commentCount,
                thumbnail: other.thumbnail,
                updatedTime: other.updatedTime,
                createdTime: other.createdTime,
                publishedTime: other.publishedTime,
                lastPublishedTime: other.lastPublishedTime,
                type: other.type,
                body: other.body,
                like: other.like,
                projectIds: other.projectIds,
                channelId: other.channelId,
                contentMap: other.contentMap,
                currOldestMessageId: other.currOldestMessageId,
                hasMore: other.hasMore,
                isNotFirst: other.isNotFirst
            );
        }
    }
}