using System;

namespace ConnectApp.Models.Screen
{
    public class SettingScreenModel : IEquatable<SettingScreenModel>
    {
        public bool anonymous;
        public bool hasReviewUrl;
        public string reviewUrl;

        public bool Equals(SettingScreenModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return anonymous == other.anonymous && hasReviewUrl == other.hasReviewUrl && string.Equals(reviewUrl, other.reviewUrl);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SettingScreenModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = anonymous.GetHashCode();
                hashCode = (hashCode * 397) ^ hasReviewUrl.GetHashCode();
                hashCode = (hashCode * 397) ^ (reviewUrl != null ? reviewUrl.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}