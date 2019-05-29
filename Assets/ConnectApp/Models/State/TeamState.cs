using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class TeamState {
        public Dictionary<string, Team> teamDict { get; set; }
    }
}