using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel
{
    public class EventsScreenModel : IEquatable<EventsScreenModel>
    {
        public bool eventsLoading;
        public List<string> ongoingEvents;
        public List<string> completedEvents;
        public int ongoingEventTotal;
        public int completedEventTotal;
        public Dictionary<string, IEvent> eventsDict;

        public bool Equals(EventsScreenModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return eventsLoading == other.eventsLoading && Equals(ongoingEvents, other.ongoingEvents) && Equals(completedEvents, other.completedEvents) && ongoingEventTotal == other.ongoingEventTotal && completedEventTotal == other.completedEventTotal && Equals(eventsDict, other.eventsDict);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EventsScreenModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = eventsLoading.GetHashCode();
                hashCode = (hashCode * 397) ^ (ongoingEvents != null ? ongoingEvents.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (completedEvents != null ? completedEvents.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ongoingEventTotal;
                hashCode = (hashCode * 397) ^ completedEventTotal;
                hashCode = (hashCode * 397) ^ (eventsDict != null ? eventsDict.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}