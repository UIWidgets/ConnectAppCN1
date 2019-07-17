using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class TeamState {
        public bool teamLoading { get; set; }
        public bool teamArticleLoading { get; set; }
        public bool followerLoading { get; set; }
        public Dictionary<string, Team> teamDict { get; set; }
    }
}