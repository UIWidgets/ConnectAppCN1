using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class SearchScreenViewModel {
        public bool searchArticleLoading;
        public bool searchUserLoading;
        public bool followUserLoading;
        public string searchKeyword;
        public Dictionary<string, List<Article>> searchArticles;
        public Dictionary<string, List<User>> searchUsers;
        public int searchArticleCurrentPage;
        public List<int> searchArticlePages;
        public bool searchUserHasMore;
        public Dictionary<string, bool> followMap;
        public List<string> searchArticleHistoryList;
        public List<string> searchUserHistoryList;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public List<PopularSearch> popularSearchArticleList;
        public List<PopularSearch> popularSearchUserList;
        public List<string> blockArticleList;
        public string currentFollowId;
        public string currentUserId;
        public bool isLoggedIn;
    }
}