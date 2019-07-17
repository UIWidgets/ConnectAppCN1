using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class ArticleState {
        public bool articlesLoading { get; set; }
        public bool followArticlesLoading { get; set; }
        public bool articleDetailLoading { get; set; }
        public List<string> articleList { get; set; }
        public Dictionary<string, List<string>> followArticleDict { get; set; }
        public Dictionary<string, List<string>> hotArticleDict { get; set; }
        public bool hottestHasMore { get; set; }
        public bool followArticleHasMore { get; set; }
        public bool hotArticleHasMore { get; set; }
        public Dictionary<string, Article> articleDict { get; set; }
        public List<Article> articleHistory { get; set; }
        public List<string> blockArticleList { get; set; }
    }
}