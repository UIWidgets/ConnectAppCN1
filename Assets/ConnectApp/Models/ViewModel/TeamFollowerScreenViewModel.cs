using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class TeamFollowerScreenViewModel {
        public string teamId;
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