using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class ClearMyFutureEventsAction : RequestAction {
    }

    public class StartFetchMyFutureEventsAction : RequestAction {
    }

    public class FetchMyFutureEventsSuccessAction : BaseAction {
        public List<string> eventIds;
        public int pageNumber;
        public int total;
    }

    public class FetchMyFutureEventsFailureAction : BaseAction {
    }

    public class ClearMyPastEventsAction : RequestAction {
    }

    public class StartFetchMyPastEventsAction : RequestAction {
    }

    public class FetchMyPastEventsSuccessAction : BaseAction {
        public List<string> eventIds;
        public int pageNumber;
        public int total;
    }

    public class FetchMyPastEventsFailureAction : BaseAction {
    }

    public static partial class Actions {
        public static object fetchMyFutureEvents(int pageNumber, string mode) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return MineApi.FetchMyFutureEvents(pageNumber: pageNumber, mode: mode)
                    .Then(eventsResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = eventsResponse.userMap});
                        dispatcher.dispatch(new PlaceMapAction {placeMap = eventsResponse.placeMap});
                        var eventIds = new List<string>();
                        var eventMap = new Dictionary<string, IEvent>();
                        eventsResponse.events.items.ForEach(eventObj => {
                            eventIds.Add(item: eventObj.id);
                            eventMap.Add(key: eventObj.id, value: eventObj);
                        });
                        dispatcher.dispatch(new EventMapAction {eventMap = eventMap});
                        dispatcher.dispatch(new FetchMyFutureEventsSuccessAction {
                            eventIds = eventIds,
                            pageNumber = pageNumber,
                            total = eventsResponse.events.total
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchMyFutureEventsFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchMyPastEvents(int pageNumber, string mode) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return MineApi.FetchMyPastEvents(pageNumber: pageNumber, mode: mode)
                    .Then(eventsResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = eventsResponse.userMap});
                        dispatcher.dispatch(new PlaceMapAction {placeMap = eventsResponse.placeMap});
                        var eventIds = new List<string>();
                        var eventMap = new Dictionary<string, IEvent>();
                        eventsResponse.events.items.ForEach(eventObj => {
                            eventIds.Add(item: eventObj.id);
                            eventMap.Add(key: eventObj.id, value: eventObj);
                        });
                        dispatcher.dispatch(new EventMapAction {eventMap = eventMap});
                        dispatcher.dispatch(new FetchMyPastEventsSuccessAction {
                            eventIds = eventIds,
                            pageNumber = pageNumber,
                            total = eventsResponse.events.total
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchMyPastEventsFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }
    }
}