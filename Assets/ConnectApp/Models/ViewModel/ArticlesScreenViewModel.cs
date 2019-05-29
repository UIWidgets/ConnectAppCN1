using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ArticlesScreenViewModel {
        public bool articlesLoading;
        public List<string> articleList;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public List<string> blockArticleList;
        public bool hottestHasMore;
        public int hosttestOffset;
        public bool isLoggedIn;
    }
}