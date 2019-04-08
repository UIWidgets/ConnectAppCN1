using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel {
    public class HistoryScreenModel : IEquatable<HistoryScreenModel> {
        public List<IEvent> eventHistory;
        public List<Article> articleHistory;

        public bool Equals(HistoryScreenModel other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(eventHistory, other.eventHistory) && Equals(articleHistory, other.articleHistory);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HistoryScreenModel) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((eventHistory != null ? eventHistory.GetHashCode() : 0) * 397) ^ (articleHistory != null ? articleHistory.GetHashCode() : 0);
            }
        }
    }
}