using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class MyFavoriteScreenViewModel {
        public bool myFavoriteLoading;
        public bool myFollowFavoriteLoading;
        public List<string> myFavoriteIds;
        public List<string> myFollowFavoriteIds;
        public bool myFavoriteHasMore;
        public bool myFollowFavoriteHasMore;
        public string currentUserId;
        public Article article;
        public Dictionary<string, FavoriteTag> favoriteTagDict;
    }
}