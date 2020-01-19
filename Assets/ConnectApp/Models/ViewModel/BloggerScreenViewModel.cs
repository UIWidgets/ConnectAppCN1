using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class BloggerScreenViewModel {
        public bool bloggerLoading;
        public List<string> bloggerIds;
        public int bloggerPageNumber;
        public bool bloggerHasMore;
        public bool isLoggedIn;
        public string currentUserId;
        public Dictionary<string, User> userDict;
        public Dictionary<string, bool> followMap;
        public Dictionary<string, UserLicense> userLicenseDict;
    }
}