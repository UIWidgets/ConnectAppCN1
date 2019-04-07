using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.Screen {
    public class NotifcationScreenModel : IEquatable<NotifcationScreenModel>
    {
        public bool notifationLoading;
        public int total;
        public List<Notification> notifications;

        public bool Equals(NotifcationScreenModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return notifationLoading == other.notifationLoading && total == other.total && Equals(notifications, other.notifications);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NotifcationScreenModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = notifationLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ total;
                hashCode = (hashCode * 397) ^ (notifications != null ? notifications.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}