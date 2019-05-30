using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class SearchScreenViewModel {
        public bool searchLoading;
        public string searchKeyword;
        public List<Article> searchArticles;
        public int currentPage;
        public List<int> pages;
        public List<string> searchHistoryList;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public List<PopularSearch> popularSearchList;
        public List<string> blockArticleList;
    }
}