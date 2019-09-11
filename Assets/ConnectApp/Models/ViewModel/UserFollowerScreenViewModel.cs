using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class UserFollowerScreenViewModel {
        public string userId;
        public bool followerLoading;
        public List<User> followers;
        public bool followersHasMore;
        public int userOffset;
        public Dictionary<string, User> userDict;
        public Dictionary<string, UserLicense> userLicenseDict;
        public Dictionary<string, bool> followMap;
        public string currentUserId;
        public bool isLoggedIn;
    }
}