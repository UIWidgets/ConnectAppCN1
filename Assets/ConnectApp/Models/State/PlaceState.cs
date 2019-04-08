using System;
using System.Collections.Generic;
using RSG;

namespace ConnectApp.models {
    [Serializable]
    public class PlaceState : IEquatable<PlaceState> {
        public Dictionary<string, Place> placeDict { get; set; }

        public bool Equals(PlaceState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(placeDict, other.placeDict);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlaceState) obj);
        }

        public override int GetHashCode() {
            return (placeDict != null ? placeDict.GetHashCode() : 0);
        }
    }
}