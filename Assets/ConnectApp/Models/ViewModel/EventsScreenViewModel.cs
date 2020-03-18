using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class EventsScreenViewModel {
        public bool eventOngoingLoading;
        public bool eventCompletedLoading;
        public bool homeEventLoading;
        public List<string> ongoingEvents;
        public List<string> completedEvents;
        public List<string> homeEvents;
        public int ongoingEventTotal;
        public int completedEventTotal;
        public int homeEventPageNumber;
        public bool homeEventHasMore;
        public int currentTabBarIndex;
        public Dictionary<string, IEvent> eventsDict;
        public Dictionary<string, Place> placeDict;
    }
}