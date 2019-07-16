using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class TeamDetailScreenViewModel {
        public string teamId;
        public bool teamLoading;
        public bool teamArticleLoading;
        public bool followTeamLoading;
        public Team team;
        public Dictionary<string, List<Article>> teamArticleDict;
        public bool teamArticleHasMore;
        public int teamArticleOffset;
        public Dictionary<string, bool> followMap;
        public string currentUserId;
        public bool isLoggedIn;
    }
}