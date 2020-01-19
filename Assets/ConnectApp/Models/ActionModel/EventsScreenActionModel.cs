using System;
using ConnectApp.Models.State;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class EventsScreenActionModel : BaseActionModel {
        public Action<string, EventType> pushToEventDetail;
        public Action startFetchEventOngoing;
        public Action startFetchEventCompleted;
        public Action startFetchHomeEvent;
        public Action clearEventOngoing;
        public Action clearEventCompleted;
        public Func<int, string, IPromise> fetchEvents;
        public Func<int, IPromise> fetchHomeEvents;
    }
}