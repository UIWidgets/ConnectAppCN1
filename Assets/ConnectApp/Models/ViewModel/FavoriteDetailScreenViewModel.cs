using System.Collections.Generic;
using ConnectApp.Models.Model;
using ConnectApp.screens;

namespace ConnectApp.Models.ViewModel {
    public class FavoriteDetailScreenViewModel {
        public string userId;
        public string tagId;
        public FavoriteType type;
        public bool favoriteDetailLoading;
        public List<string> favoriteDetailArticleIds;
        public List<string> myFavoriteIds;
        public int favoriteArticleOffset;
        public bool favoriteArticleHasMore;
        public bool isLoggedIn;
        public FavoriteTag favoriteTag;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public Dictionary<string, string> collectChangeMap;
        public string currentUserId;
        public bool isCollect;
        public bool collectLoading;
    }
}