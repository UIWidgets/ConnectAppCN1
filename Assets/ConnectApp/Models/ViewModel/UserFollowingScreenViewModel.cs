using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class UserFollowingScreenViewModel {
        public string userId;
        public bool followingLoading;
        public bool searchFollowingLoading;
        public List<User> followings;
        public List<User> searchFollowings;
        public string searchFollowingKeyword;
        public bool searchFollowingHasMore;
        public bool followingsHasMore;
        public int userOffset;
        public Dictionary<string, User> userDict;
        public Dictionary<string, bool> followMap;
        public string currentUserId;
        public bool isLoggedIn;
    }
}