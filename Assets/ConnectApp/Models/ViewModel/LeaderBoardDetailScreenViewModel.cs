using System.Collections.Generic;
using ConnectApp.Models.Model;
using ConnectApp.screens;

namespace ConnectApp.Models.ViewModel {
    public class LeaderBoardDetailScreenViewModel {
        public RankData rankData;
        public LeaderBoardType type;
        public string tagId;
        public string currentUserId;
        public List<string> articleList;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, FavoriteTag> favoriteTagDict;
        public Dictionary<string, FavoriteTagArticle> favoriteTagArticleDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public Dictionary<string, UserArticle> userArticleDict;
        public bool isCollected;
        public bool isFollowed;
        public bool isLoggedIn;
        public bool hasMore;
        public bool loading;
        public bool collectLoading;
        public bool isHost;
    }
}