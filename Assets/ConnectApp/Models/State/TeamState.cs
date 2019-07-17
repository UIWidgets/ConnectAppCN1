using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class TeamState {
        public bool teamLoading { get; set; }
        public bool teamArticleLoading { get; set; }
        public bool followTeamLoading { get; set; }
        public bool teamFollowerLoading { get; set; }
        public Dictionary<string, Team> teamDict { get; set; }
        public string currentFollowId { get; set; }
        public Dictionary<string, List<Article>> teamArticleDict { get; set; }
        public Dictionary<string, List<User>> teamFollowerDict { get; set; }
        public bool teamArticleHasMore { get; set; }
        public bool teamFollowerHasMore { get; set; }
    }
}