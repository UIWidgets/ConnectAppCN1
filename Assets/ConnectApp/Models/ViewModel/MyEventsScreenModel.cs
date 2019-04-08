using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel {
    public class MyEventsScreenModel : IEquatable<MyEventsScreenModel> {
        public List<IEvent> futureEventsList;
        public List<IEvent> pastEventsList;
        public bool futureListLoading;
        public bool pastListLoading;
        public int futureEventTotal;
        public int pastEventTotal;

        public bool Equals(MyEventsScreenModel other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(futureEventsList, other.futureEventsList) && Equals(pastEventsList, other.pastEventsList) && futureListLoading == other.futureListLoading && pastListLoading == other.pastListLoading && futureEventTotal == other.futureEventTotal && pastEventTotal == other.pastEventTotal;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MyEventsScreenModel) obj);
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