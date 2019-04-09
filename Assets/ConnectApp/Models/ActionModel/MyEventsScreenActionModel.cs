using System;
using ConnectApp.models;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class MyEventsScreenActionModel {
        public Action mainRouterPop;
        public Action<string, EventType> pushToEventDetail;
        public Func<int, IPromise> fetchMyFutureEvents;
        public Func<int, IPromise> fetchMyPastEvents;
    }
}