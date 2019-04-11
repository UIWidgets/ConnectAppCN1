using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.models;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class StartFetchMyFutureEventsAction : RequestAction {}

    public class FetchMyFutureEventsSuccessAction : BaseAction {
        public FetchEventsResponse eventsResponse;
        public int pageNumber;
    }

    public class StartFetchMyPastEventsAction : RequestAction {}

    public class FetchMyPastEventsSuccessAction : BaseAction {
        public FetchEventsResponse eventsResponse;
        public int pageNumber;
    }
    
    public class FetchMyPastEventsFailureAction : BaseAction {
        public FetchEventsResponse eventsResponse;
        public int pageNumber;
    }
    
    public static partial class Actions {
        public static object fetchMyFutureEvents(int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {                
                return MineApi.FetchMyFutureEvents(pageNumber)
                    .Then(eventsResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = eventsResponse.userMap});
                        dispatcher.dispatch(new PlaceMapAction {placeMap = eventsResponse.placeMap});
                        dispatcher.dispatch(new FetchMyFutureEventsSuccessAction
                            {eventsResponse = eventsResponse, pageNumber = pageNumber});
                    })
                    .Catch(Debug.Log);
            });
        }
        
        public static object fetchMyPastEvents(int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return MineApi.FetchMyPastEvents(pageNumber)
                    .Then(eventsResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = eventsResponse.userMap});
                        dispatcher.dispatch(new PlaceMapAction {placeMap = eventsResponse.placeMap});
                        dispatcher.dispatch(new FetchMyPastEventsSuccessAction {
                            eventsResponse = eventsResponse,
                            pageNumber = pageNumber
                        });
                    })
                    .Catch(Debug.Log);
            });
        }
        
    }
}