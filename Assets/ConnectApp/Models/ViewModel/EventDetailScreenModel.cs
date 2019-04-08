using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel {
    public class EventDetailScreenModel : IEquatable<EventDetailScreenModel> {
        public string eventId;
        public EventType eventType;
        public string currOldestMessageId;
        public bool isLoggedIn;
        public bool eventDetailLoading;
        public bool joinEventLoading;
        public bool showChatWindow;
        public string channelId;
        public List<string> messageList;
        public bool messageLoading;
        public bool hasMore;
        public Dictionary<string, Dictionary<string, Message>> channelMessageDict;
        public Dictionary<string, IEvent> eventsDict;

        public bool Equals(EventDetailScreenModel other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(eventId, other.eventId) && eventType == other.eventType && string.Equals(currOldestMessageId, other.currOldestMessageId) && isLoggedIn == other.isLoggedIn && eventDetailLoading == other.eventDetailLoading && joinEventLoading == other.joinEventLoading && showChatWindow == other.showChatWindow && string.Equals(channelId, other.channelId) && Equals(messageList, other.messageList) && messageLoading == other.messageLoading && hasMore == other.hasMore && Equals(channelMessageDict, other.channelMessageDict) && Equals(eventsDict, other.eventsDict);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EventDetailScreenModel) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = (eventId != null ? eventId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) eventType;
                hashCode = (hashCode * 397) ^ (currOldestMessageId != null ? currOldestMessageId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ isLoggedIn.GetHashCode();
                hashCode = (hashCode * 397) ^ eventDetailLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ joinEventLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ showChatWindow.GetHashCode();
                hashCode = (hashCode * 397) ^ (channelId != null ? channelId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (messageList != null ? messageList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ messageLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ hasMore.GetHashCode();
                hashCode = (hashCode * 397) ^ (channelMessageDict != null ? channelMessageDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (eventsDict != null ? eventsDict.GetHashCode() : 0);
                return hashCode;
            }
        }
    }    
}