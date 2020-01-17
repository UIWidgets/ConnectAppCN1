using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class SearchScreenViewModel {
        public bool searchArticleLoading;
        public bool searchUserLoading;
        public bool searchTeamLoading;
        public string searchKeyword;
        public string searchSuggest;
        public List<string> searchArticleIds;
        public List<string> searchUserIds;
        public List<string> searchTeamIds;
        public int searchArticleCurrentPage;
        public List<int> searchArticlePages;
        public bool searchUserHasMore;
        public bool searchTeamHasMore;
        public Dictionary<string, bool> followMap;
        public List<string> searchArticleHistoryList;
        public List<string> searchUserHistoryList;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, User> userDict;
        public Dictionary<string, UserLicense> userLicenseDict;
        public Dictionary<string, Team> teamDict;
        public List<PopularSearch> popularSearchArticleList;
        public List<PopularSearch> popularSearchUserList;
        public List<string> blockArticleList;
        public string currentUserId;
        public bool isLoggedIn;
    }
}