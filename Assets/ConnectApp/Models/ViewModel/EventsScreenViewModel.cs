using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel {
    public class EventsScreenViewModel {
        public bool eventsLoading;
        public List<string> ongoingEvents;
        public List<string> completedEvents;
        public int ongoingEventTotal;
        public int completedEventTotal;
        public Dictionary<string, IEvent> eventsDict;
        public Dictionary<string, Place> placeDict;
    }
}