using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class ArticleState {
        public bool articlesLoading { get; set; }
        public bool followArticlesLoading { get; set; }
        public bool articleDetailLoading { get; set; }
        public List<string> recommendArticleIds { get; set; }
        public Dictionary<string, List<string>> followArticleIdDict { get; set; }
        public Dictionary<string, List<string>> hotArticleIdDict { get; set; }
        public bool hottestHasMore { get; set; }
        public bool followArticleHasMore { get; set; }
        public bool hotArticleHasMore { get; set; }
        public int hotArticlePage { get; set; }
        public bool feedHasNew { get; set; }
        public bool feedIsFirst { get; set; }
        public string beforeTime { get; set; }
        public string afterTime { get; set; }
        public Dictionary<string, Article> articleDict { get; set; }
        public Dictionary<string, UserArticle> userArticleDict { get; set; }
        public List<Article> articleHistory { get; set; }
        public List<string> blockArticleList { get; set; }
        public List<string> homeSliderIds { get; set; }
        public List<string> homeTopCollectionIds { get; set; }
        public List<string> homeCollectionIds { get; set; }
        public List<string> homeBloggerIds { get; set; }
        public string searchSuggest { get; set; }
        public string dailySelectionId { get; set; }
        public DateTime? leaderBoardUpdatedTime { get; set; }
        public string recommendLastRefreshArticleId { get; set; }
        public bool recommendHasNewArticle { get; set; }
    }
}