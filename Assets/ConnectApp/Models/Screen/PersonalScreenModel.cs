using System;

namespace ConnectApp.Models.Screen
{
    public class PersonalScreenModel : IEquatable<PersonalScreenModel>{
        public bool isLoggedIn;
        public string userId;
        public string userFullName;

        public bool Equals(PersonalScreenModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return isLoggedIn == other.isLoggedIn && string.Equals(userId, other.userId) && string.Equals(userFullName, other.userFullName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PersonalScreenModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = isLoggedIn.GetHashCode();
                hashCode = (hashCode * 397) ^ (userId != null ? userId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (userFullName != null ? userFullName.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}