using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class MyFavoriteScreenViewModel {
        public bool myFavoriteLoading;
        public List<string> myFavoriteIds;
        public bool myFavoriteHasMore;
        public string currentUserId;
        public Article article;
        public Dictionary<string, FavoriteTag> favoriteTagDict;
    }
}