using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class MineState : IEquatable<MineState> {
        public List<IEvent> futureEventsList { get; set; }
        public List<IEvent> pastEventsList { get; set; }
        public bool futureListLoading { get; set; }
        public bool pastListLoading { get; set; }
        public int futureEventTotal { get; set; }
        public int pastEventTotal { get; set; }

        public bool Equals(MineState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(futureEventsList, other.futureEventsList) && Equals(pastEventsList, other.pastEventsList) && futureListLoading == other.futureListLoading && pastListLoading == other.pastListLoading && futureEventTotal == other.futureEventTotal && pastEventTotal == other.pastEventTotal;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MineState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (futureEventsList != null ? futureEventsList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (pastEventsList != null ? pastEventsList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ futureListLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ pastListLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ futureEventTotal;
                hashCode = (hashCode * 397) ^ pastEventTotal;
                return hashCode;
            }
        }
    }
}