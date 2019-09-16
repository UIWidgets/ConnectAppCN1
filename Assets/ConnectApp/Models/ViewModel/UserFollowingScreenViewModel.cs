using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class UserFollowingScreenViewModel {
        public string userId;
        public int initialPage;
        public bool followingUserLoading;
        public bool searchFollowingUserLoading;
        public bool followingTeamLoading;
        public List<User> followingUsers;
        public List<User> searchFollowingUsers;
        public List<Team> followingTeams;
        public string searchFollowingKeyword;
        public bool searchFollowingUserHasMore;
        public bool followingUsersHasMore;
        public bool followingTeamsHasMore;
        public int followingUserOffset;
        public int followingTeamOffset;
        public Dictionary<string, User> userDict;
        public Dictionary<string, UserLicense> userLicenseDict;
        public Dictionary<string, Team> teamDict;
        public Dictionary<string, bool> followMap;
        public string currentUserId;
        public bool isLoggedIn;
    }
}