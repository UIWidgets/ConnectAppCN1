using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class TeamDetailScreenViewModel {
        public bool teamLoading;
        public bool teamArticleLoading;
        public Team team;
        public string teamId;
        public Dictionary<string, Article> articleDict;
        public Dictionary<string, bool> followMap;
        public string currentUserId;
        public bool isLoggedIn;
    }
}