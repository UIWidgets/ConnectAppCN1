using ConnectApp.api;
using ConnectApp.models;
using ConnectApp.redux.actions;
using UnityEngine;

namespace ConnectApp.redux.reducers {
    public static class AppReducer {
        public static AppState Reduce(AppState state, object bAction) {
            switch (bAction) {
                case AddCountAction action: {
                    state.Count += action.number;
                    PlayerPrefs.SetInt("count", state.Count);
                    PlayerPrefs.Save();
                    break;
                }
                case LoginChangeEmailAction action: {
                    state.LoginState.email = action.changeText;
                    break;
                }
                case LoginChangePasswordAction action: {
                    state.LoginState.password = action.changeText;
                    break;
                }
                case ChatWindowShowAction action: {
                    state.LiveState.showChatWindow = action.show;
                    break;
                }
                case ChatWindowStatusAction action: {
                    state.LiveState.openChatWindow = action.status;
                    break;
                }
                case NavigatorToLiveAction action: {
                    state.LiveState.detailId = action.eventId;
                    break;
                }
                case ClearLiveInfoAction action: {
                    state.LiveState.liveInfo = null;
                    break;
                }
                case LoginByEmailAction action: {
                    state.LoginState.loading = true;
                    var email = state.LoginState.email;
                    var password = state.LoginState.password;
                    LoginApi.LoginByEmail(email, password)
                        .Then(loginInfo => {
                            StoreProvider.store.Dispatch(new LoginByEmailSuccessAction {loginInfo = loginInfo});
                        })
                        .Catch(error => {
                            state.LoginState.loading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case LoginByEmailSuccessAction action: {
                    state.LoginState.loading = false;
                    state.LoginState.loginInfo = action.loginInfo;
                    state.LoginState.isLoggedIn = true;
                    break;
                }
                case FetchArticlesAction action: {
                    ArticleApi.FetchArticles(action.pageNumber)
                        .Then(() => { StoreProvider.store.Dispatch(new FetchArticleSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchArticleSuccessAction action: {
                    break;
                }
                case FetchArticleDetailAction action: {
                    ArticleApi.FetchArticleDetail(action.articleId)
                        .Then(() => { StoreProvider.store.Dispatch(new FetchArticleDetailSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchArticleDetailSuccessAction action: {
                    break;
                }
                case FetchEventsAction action: {
                    state.EventsLoading = true;
                    EventApi.FetchEvents(action.pageNumber)
                        .Then(events => {
                            StoreProvider.store.Dispatch(new FetchEventsSuccessAction {events = events});
                        })
                        .Catch(error => {
                            state.EventsLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchEventsSuccessAction action: {
                    state.EventsLoading = false;
                    state.Events = action.events;
                    break;
                }
                case FetchEventDetailAction action: {
                    state.LiveState.loading = true;
                    EventApi.FetchLiveDetail(action.eventId)
                        .Then(liveInfo => {
                            StoreProvider.store.Dispatch(new FetchEventDetailSuccessAction {liveInfo = liveInfo});
                        })
                        .Catch(error => {
                            state.LiveState.loading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchEventDetailSuccessAction action: {
                    state.LiveState.loading = false;
                    state.LiveState.liveInfo = action.liveInfo;
                    break;
                }
                case FetchNotificationsAction action: {
                    NotificationApi.FetchNotifications(action.pageNumber)
                        .Then(() => { StoreProvider.store.Dispatch(new FetchNotificationsSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchNotificationsSuccessAction action: {
                    break;
                }
            }

            return state;
        }
    }
}