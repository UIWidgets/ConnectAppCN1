using ConnectApp.api;
using ConnectApp.models;
using ConnectApp.redux.actions;
using UnityEngine;

namespace ConnectApp.redux {
    public static class Middleware {
        public static Middleware<AppState> Create() {
            return (store) => (next) => (bAction) => {
                if (bAction is EventsRequestAction action) {
                    var api = new API();
                    api.FetchEvents(pageNumber: action.pageNumber)
                        .Then(events => {
                            Debug.Log(events);
                            store.Dispatch(new EventsResponseAction {
                                events = events
                            });
                        })
                        .Catch(error => { Debug.Log(error); });
                }

                return next(bAction);
            };
        }
    }
}