using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class UserState : IEquatable<UserState> {
        public Dictionary<string, User> userDict { get; set; }

        public bool Equals(UserState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(userDict, other.userDict);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserState) obj);
        }

        public override int GetHashCode() {
            return (userDict != null ? userDict.GetHashCode() : 0);
        }
    }
}