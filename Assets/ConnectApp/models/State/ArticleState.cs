using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class ArticleState {
        public bool ArticlesLoading { get; set; }
        public bool ArticleDetailLoading { get; set; }
        public List<Article> Articles { get; set; }
        public Article ArticleDetail { get; set; }
    }
}