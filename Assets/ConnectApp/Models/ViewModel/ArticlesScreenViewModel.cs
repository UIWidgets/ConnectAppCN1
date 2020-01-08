using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class ArticlesScreenViewModel {
        public bool articlesLoading;
        public bool followArticlesLoading;
        public bool followingLoading;
        public List<string> recommendArticleIds;
        public List<string> followArticleIds;
        public List<string> hotArticleIds;
        public List<Following> followings;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, UserLicense> userLicenseDict;
        public Dictionary<string, Team> teamDict;
        public Dictionary<string, bool> likeMap;
        public Dictionary<string, bool> followMap;
        public List<string> blockArticleList;
        public bool hottestHasMore;
        public bool followArticleHasMore;
        public bool hotArticleHasMore;
        public int hosttestOffset;
        public int hotArticlePage;
        public bool isLoggedIn;
        public string currentUserId;
        public bool showFirstEgg;
        public bool feedHasNew;
        public string beforeTime;
        public string afterTime;
        public int currentTabBarIndex;
        public bool nationalDayEnabled;
        public int selectedIndex;
    }
}