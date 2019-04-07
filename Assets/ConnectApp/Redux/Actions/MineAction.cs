using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.models;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class FetchMyFutureEventsAction : RequestAction {
        public int pageNumber = 0;
    }

    public class FetchMyFutureEventsSuccessAction : BaseAction {
        public FetchEventsResponse eventsResponse;
        public int pageNumber;
    }

    public class FetchMyPastEventsAction : RequestAction {
    }

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
                        dispatcher.dispatch(new FetchMyFutureEventsSuccessAction
                            {eventsResponse = eventsResponse, pageNumber = pageNumber});
                    })
                    .Catch(Debug.Log);
            });
        }
        
        public static object fetchMyPastEvents(int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                dispatcher.dispatch(new FetchMyPastEventsAction());
                return MineApi.FetchMyPastEvents(pageNumber)
                    .Then(eventsResponse => {
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