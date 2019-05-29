using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class EventsScreenViewModel {
        public bool eventOngoingLoading;
        public bool eventCompletedLoading;
        public List<string> ongoingEvents;
        public List<string> completedEvents;
        public int ongoingEventTotal;
        public int completedEventTotal;
        public Dictionary<string, IEvent> eventsDict;
        public Dictionary<string, Place> placeDict;
    }
}