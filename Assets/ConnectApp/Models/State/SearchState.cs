using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class SearchState {
        public bool searchArticleLoading { get; set; }
        public bool searchUserLoading { get; set; }
        public bool searchFollowingLoading { get; set; }
        public bool searchTeamLoading { get; set; }
        public string keyword { get; set; }
        public string searchFollowingKeyword { get; set; }
        public Dictionary<string, List<string>> searchArticleIdDict { get; set; }
        public Dictionary<string, List<string>> searchUserIdDict { get; set; }
        public Dictionary<string, List<string>> searchTeamIdDict { get; set; }
        public List<User> searchFollowings { get; set; }
        public int searchArticleCurrentPage { get; set; }
        public List<int> searchArticlePages { get; set; }
        public bool searchUserHasMore { get; set; }
        public bool searchTeamHasMore { get; set; }
        public bool searchFollowingHasMore { get; set; }
        public List<string> searchArticleHistoryList { get; set; }
    }

    [Serializable]
    public class PopularSearchState {
        public List<PopularSearch> popularSearchArticles { get; set; }
        public List<PopularSearch> popularSearchUsers { get; set; }
    }
}