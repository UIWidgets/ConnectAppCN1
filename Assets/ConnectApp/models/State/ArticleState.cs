using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class ArticleState {
        public bool ArticlesLoading { get; set; }
        public bool ArticleDetailLoading { get; set; }
        
        public List<string> ArticleList { get; set; }
        
        public Dictionary<string, Article> ArticleDict { get; set; }
    }
}