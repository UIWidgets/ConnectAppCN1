using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class TeamDetailScreenViewModel {
        public string teamId;
        public bool teamLoading;
        public bool teamArticleLoading;
        public Team team;
        public Dictionary<string, List<Article>> teamArticleDict;
        public bool teamArticleHasMore;
        public int teamArticleOffset;
        public bool isLoggedIn;
    }
}