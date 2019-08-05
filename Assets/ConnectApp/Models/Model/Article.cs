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
        public string description;
        public string subTitle;
        public bool published;
        public int viewCount;
        public int likeCount;
        public int commentCount;
        public Thumbnail thumbnail;
        public DateTime updatedTime;
        public DateTime createdTime;
        public DateTime publishedTime;
        public DateTime lastPublishedTime;
        public string type;
        public List<string> contentIds;
        public string bodyPlain;
        public string body;
        public HeaderImage headerImage;
        public bool like;
        public bool edit;
        public List<Article> projects;
        public List<string> projectIds;
        public string channelId;
        public Dictionary<string, ContentMap> contentMap;
        public string currOldestMessageId;
        public bool hasMore;
        public bool isNotFirst; //加载详情后置位true
    }

    [Serializable]
    public class HeaderImage {
        public string url;
    }
}