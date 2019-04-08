using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class NotificationState : IEquatable<NotificationState> {
        public bool loading { get; set; }
        public int total { get; set; }
        public List<Notification> notifications { get; set; }

        public bool Equals(NotificationState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return loading == other.loading && total == other.total && Equals(notifications, other.notifications);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NotificationState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = loading.GetHashCode();
                hashCode = (hashCode * 397) ^ total;
                hashCode = (hashCode * 397) ^ (notifications != null ? notifications.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}