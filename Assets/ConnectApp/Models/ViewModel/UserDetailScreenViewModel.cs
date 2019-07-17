using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class UserDetailScreenViewModel {
        public string userId;
        public bool userLoading;
        public bool userArticleLoading;
        public User user;
        public Dictionary<string, bool> followMap;
        public int articleOffset;
        public string currentUserId;
        public bool isLoggedIn;
    }
}