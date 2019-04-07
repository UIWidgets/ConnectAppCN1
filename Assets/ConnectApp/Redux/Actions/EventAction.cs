using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.models;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class FetchEventsAction : RequestAction {
        public int pageNumber = 0;
        public string tab;
    }
    
    public class FetchEventsSuccessAction : BaseAction {
        public FetchEventsResponse eventsResponse;
        public int pageNumber = 0;
        public string tab;
    }

    public class FetchEventDetailAction : RequestAction {
        public string eventId;
    }

    public class FetchEventDetailSuccessAction : BaseAction {
        public IEvent eventObj;
    }
    
    public class FetchEventDetailFailedAction : BaseAction {
    }

    public class GetEventHistoryAction : BaseAction {
    }

    public class SaveEventHistoryAction : BaseAction {
        public IEvent eventObj;
    }

    public class DeleteEventHistoryAction : BaseAction {
        public string eventId;
    }

    public class DeleteAllEventHistoryAction : BaseAction {
    }

    public class JoinEventAction : RequestAction {
        public string eventId;
    }

    public class JoinEventSuccessAction : BaseAction {
        public string eventId;
    }
    
    public static partial class Actions {
        public static object fetchEvents(int pageNumber, string tab)
        {
            return new ThunkAction<AppState>((dispatcher, getState) => {                
                return EventApi.FetchEvents(pageNumber, tab)
                    .Then(eventsResponse => {
                        dispatcher.dispatch(new FetchEventsSuccessAction {
                                eventsResponse = eventsResponse, 
                                tab = tab,
                                pageNumber = pageNumber
                            }
                        );
                    })
                    .Catch(Debug.Log);
            });
        }
    }
}