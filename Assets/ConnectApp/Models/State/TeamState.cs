using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class TeamState {
        public Dictionary<string, Team> teamDict { get; set; }
    }
}