using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class UserDetailScreenViewModel {
        public bool userLoading;
        public bool userArticleLoading;
        public bool userFavoriteLoading;
        public Dictionary<string, List<string>> favoriteTagIdDict;
        public bool userFavoriteHasMore;
        public User user;
        public Dictionary<string, UserLicense> userLicenseDict;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, FavoriteTag> favoriteTagDict;
        public Dictionary<string, bool> followMap;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public string currentUserId;
        public bool isLoggedIn;
    }
}