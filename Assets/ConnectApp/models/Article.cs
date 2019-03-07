using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    
    [Serializable]
    public class Article
    {
        public string id;
        public string title;
        public string subTitle;
        public string description;
        public string userId;
        public string type;
        public string publishedTime;
        public string bodyPlain;
        public int viewCount;
        public Thumbnail thumbnail;
    }

}