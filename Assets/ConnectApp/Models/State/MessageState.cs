using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class MessageState : IEquatable<MessageState> {
        public bool messageLoading { get; set; }
        public bool sendMessageLoading { get; set; }
        public Dictionary<string, List<string>> channelMessageList { get; set; }
        public Dictionary<string, Dictionary<string, Message>> channelMessageDict { get; set; }
        public string currOldestMessageId { get; set; }
        public bool hasMore { get; set; }

        public bool Equals(MessageState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return messageLoading == other.messageLoading && sendMessageLoading == other.sendMessageLoading && Equals(channelMessageList, other.channelMessageList) && Equals(channelMessageDict, other.channelMessageDict) && string.Equals(currOldestMessageId, other.currOldestMessageId) && hasMore == other.hasMore;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MessageState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = messageLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ sendMessageLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (channelMessageList != null ? channelMessageList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (channelMessageDict != null ? channelMessageDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (currOldestMessageId != null ? currOldestMessageId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ hasMore.GetHashCode();
                return hashCode;
            }
        }
    }
}