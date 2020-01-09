using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class UserLikeArticleScreenViewModel {
        public bool likeArticleLoading;
        public List<string> likeArticleIds;
        public int likeArticlePage;
        public bool likeArticleHasMore;
        public bool isLoggedIn;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
    }
}