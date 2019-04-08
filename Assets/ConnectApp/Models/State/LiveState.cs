using System;

namespace ConnectApp.models {
    [Serializable]
    public class LiveState : IEquatable<LiveState> {
        public bool loading;
        public string detailId;
        public bool showChatWindow;
        public bool openChatWindow;

        public bool Equals(LiveState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return loading == other.loading && string.Equals(detailId, other.detailId) && showChatWindow == other.showChatWindow && openChatWindow == other.openChatWindow;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LiveState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = loading.GetHashCode();
                hashCode = (hashCode * 397) ^ (detailId != null ? detailId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ showChatWindow.GetHashCode();
                hashCode = (hashCode * 397) ^ openChatWindow.GetHashCode();
                return hashCode;
            }
        }
    }
}