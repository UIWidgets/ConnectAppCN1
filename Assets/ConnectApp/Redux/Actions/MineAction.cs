using ConnectApp.Api;
using ConnectApp.Models.Api;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class ClearMyFutureEventsAction : RequestAction {
    }

    public class StartFetchMyFutureEventsAction : RequestAction {
    }

    public class FetchMyFutureEventsSuccessAction : BaseAction {
        public FetchEventsResponse eventsResponse;
        public int pageNumber;
    }

    public class FetchMyFutureEventsFailureAction : BaseAction {
    }

    public class ClearMyPastEventsAction : RequestAction {
    }

    public class StartFetchMyPastEventsAction : RequestAction {
    }

    public class FetchMyPastEventsSuccessAction : BaseAction {
        public FetchEventsResponse eventsResponse;
        public int pageNumber;
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
                        dispatcher.dispatch(new FetchMyFutureEventsSuccessAction
                            {eventsResponse = eventsResponse, pageNumber = pageNumber});
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
                        dispatcher.dispatch(new FetchMyPastEventsSuccessAction {
                            eventsResponse = eventsResponse,
                            pageNumber = pageNumber
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