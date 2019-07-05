using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class FollowingUserScreenViewModel {
        public string personalId;
        public bool followingLoading;
        public bool searchFollowingLoading;
        public bool followUserLoading;
        public List<User> followings;
        public List<User> searchFollowings;
        public string searchFollowingKeyword;
        public bool searchFollowingHasMore;
        public bool followingsHasMore;
        public int userOffset;
        public Dictionary<string, bool> followMap;
        public string currentFollowId;
        public string currentUserId;
        public bool isLoggedIn;
    }
}