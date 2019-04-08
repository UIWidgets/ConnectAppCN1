using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class TeamState : IEquatable<TeamState> {
        public Dictionary<string, Team> teamDict { get; set; }

        public bool Equals(TeamState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(teamDict, other.teamDict);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TeamState) obj);
        }

        public override int GetHashCode() {
            return (teamDict != null ? teamDict.GetHashCode() : 0);
        }
    }
}