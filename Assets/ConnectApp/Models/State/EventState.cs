using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    public enum EventType {
        onLine,
        offline
    }

    [Serializable]
    public class EventState : IEquatable<EventState> {
        public bool eventsLoading { get; set; }
        public List<string> ongoingEvents { get; set; }
        public int ongoingEventTotal { get; set; }
        public List<string> completedEvents { get; set; }
        public int completedEventTotal { get; set; }
        public int pageNumber { get; set; }
        public int completedPageNumber { get; set; }
        public Dictionary<string, IEvent> eventsDict { get; set; }
        public List<IEvent> eventHistory { get; set; }
        public bool eventDetailLoading { get; set; }
        public bool joinEventLoading { get; set; }
        public bool showChatWindow { get; set; }
        public bool openChatWindow { get; set; }
        public string channelId { get; set; }

        public bool Equals(EventState other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return eventsLoading == other.eventsLoading && Equals(ongoingEvents, other.ongoingEvents) && ongoingEventTotal == other.ongoingEventTotal && Equals(completedEvents, other.completedEvents) && completedEventTotal == other.completedEventTotal && pageNumber == other.pageNumber && completedPageNumber == other.completedPageNumber && Equals(eventsDict, other.eventsDict) && Equals(eventHistory, other.eventHistory) && eventDetailLoading == other.eventDetailLoading && joinEventLoading == other.joinEventLoading && showChatWindow == other.showChatWindow && openChatWindow == other.openChatWindow && string.Equals(channelId, other.channelId);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EventState) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = eventsLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (ongoingEvents != null ? ongoingEvents.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ongoingEventTotal;
                hashCode = (hashCode * 397) ^ (completedEvents != null ? completedEvents.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ completedEventTotal;
                hashCode = (hashCode * 397) ^ pageNumber;
                hashCode = (hashCode * 397) ^ completedPageNumber;
                hashCode = (hashCode * 397) ^ (eventsDict != null ? eventsDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (eventHistory != null ? eventHistory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ eventDetailLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ joinEventLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ showChatWindow.GetHashCode();
                hashCode = (hashCode * 397) ^ openChatWindow.GetHashCode();
                hashCode = (hashCode * 397) ^ (channelId != null ? channelId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}