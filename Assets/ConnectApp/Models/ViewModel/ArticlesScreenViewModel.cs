using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ArticlesScreenViewModel {
        public bool articlesLoading;
        public bool followArticlesLoading;
        public bool followingLoading;
        public bool followTeamLoading;
        public List<string> articleList;
        public List<string> followArticleList;
        public List<string> hotArticleList;
        public List<User> followingList;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public Dictionary<string, bool> likeMap;
        public Dictionary<string, bool> followMap;
        public List<string> blockArticleList;
        public bool hottestHasMore;
        public bool followArticleHasMore;
        public bool hotArticleHasMore;
        public int hosttestOffset;
        public bool isLoggedIn;
        public string currentUserId;
    }
}