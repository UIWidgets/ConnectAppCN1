using System;
using ConnectApp.models;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class EventsScreenActionModel {
        public Action<string, EventType> pushToEventDetail;
        public Action StartFetchEventOngoingAction;
        public Action StartFetchEventCompletedAction;

        public Func<int, string, IPromise> fetchEvents;
    }
}