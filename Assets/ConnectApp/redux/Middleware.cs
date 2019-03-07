using ConnectApp.api;
using ConnectApp.models;
using ConnectApp.redux.actions;
using UnityEngine;

namespace ConnectApp.redux {
    public static class Middleware {
        public static Middleware<AppState> Create() {
            return (store) => (next) => (bAction) => {
                if (bAction is EventsRequestAction action) {
                    API.FetchEvents(action.pageNumber)
                        .Then(events => {
//                            Debug.Log(events);
                            store.Dispatch(new EventsResponseAction {
                                events = events
                            });
                        })
                        .Catch(error => { Debug.Log(error); });
                }
                else if (bAction is LiveRequestAction liveRequestAction) {
                    API.FetchLiveDetail(liveRequestAction.eventId)
                        .Then(liveInfo => {
//                            Debug.Log(liveInfo);
                            store.Dispatch(new LiveResponseAction() {
                                liveInfo = liveInfo,
                                eventId = liveRequestAction.eventId
                            });
                        })
                        .Catch(error => { Debug.Log(error); });
                }
                else if (bAction is LoginByEmailAction loginByEmailAction) {
                    var email = store.state.LoginState.email;
                    var password = store.state.LoginState.password;
                    API.LoginByEmail(email, password)
                        .Then(loginInfo => {
                            Debug.Log(loginInfo);
                            store.Dispatch(new LoginResponseAction {
                                loginInfo = loginInfo
                            });
                        })
                        .Catch(error => { Debug.Log(error); });
                }

                return next(bAction);
            };
        }
    }
}