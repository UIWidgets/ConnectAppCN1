using System;
using ConnectApp.Models.State;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class MyEventsScreenActionModel : BaseActionModel {
        public Action<string, EventType> pushToEventDetail;
        public Action clearMyFutureEvents;
        public Action startFetchMyFutureEvents;
        public Func<int, IPromise> fetchMyFutureEvents;
        public Action clearMyPastEvents;
        public Action startFetchMyPastEvents;
        public Func<int, IPromise> fetchMyPastEvents;
    }
}