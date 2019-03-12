using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class Article {
        public string id;
        public string slug;
        public string teamId;
        public string ownerType;
        public string title;
        public string userId;
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
    }
    
    [Serializable]
    public class HeaderImage
    {
        public string url;
    }

}