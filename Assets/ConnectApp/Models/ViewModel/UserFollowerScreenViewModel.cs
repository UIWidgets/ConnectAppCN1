using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class UserFollowerScreenViewModel {
        public string userId;
        public bool followerLoading;
        public bool followUserLoading;
        public List<User> followers;
        public bool followersHasMore;
        public int userOffset;
        public Dictionary<string, bool> followMap;
        public string currentFollowId;
        public string currentUserId;
        public bool isLoggedIn;
    }
}