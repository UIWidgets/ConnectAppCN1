using System;

namespace ConnectApp.models {
    [Serializable]
    public class SettingState : IEquatable<SettingState> {
        public bool hasReviewUrl { get; set; }
        public bool fetchReviewUrlLoading { get; set; }
        public string reviewUrl { get; set; }

        public bool Equals(SettingState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return hasReviewUrl == other.hasReviewUrl && fetchReviewUrlLoading == other.fetchReviewUrlLoading && string.Equals(reviewUrl, other.reviewUrl);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SettingState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = hasReviewUrl.GetHashCode();
                hashCode = (hashCode * 397) ^ fetchReviewUrlLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (reviewUrl != null ? reviewUrl.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}