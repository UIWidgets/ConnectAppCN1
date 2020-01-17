using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class MyEventsScreenViewModel {
        public List<string> futureEventIds;
        public List<string> pastEventIds;
        public bool futureListLoading;
        public bool pastListLoading;
        public int futureEventTotal;
        public int pastEventTotal;
        public Dictionary<string, IEvent> eventsDict;
        public Dictionary<string, Place> placeDict;
    }
}