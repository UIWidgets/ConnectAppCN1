using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class ArticleState {
        public bool articlesLoading { get; set; }
        public bool articleDetailLoading { get; set; }
        
        public List<string> articleList { get; set; }
        
        public int articleTotal { get; set; }
        public int pageNumber { get; set; }
        public Dictionary<string, Article> articleDict { get; set; }

        public List<Article> articleHistory { get; set; }
    }
}