using System.Collections.Generic;
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
                    state.loginState.email = action.changeText;
                    break;
                }
                case LoginChangePasswordAction action: {
                    state.loginState.password = action.changeText;
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
                    state.loginState.loading = true;
                    var email = state.loginState.email;
                    var password = state.loginState.password;
                    LoginApi.LoginByEmail(email, password)
                        .Then(loginInfo => {
                            StoreProvider.store.Dispatch(new LoginByEmailSuccessAction {loginInfo = loginInfo});
                        })
                        .Catch(error => {
                            state.loginState.loading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case LoginByEmailSuccessAction action: {
                    state.loginState.loading = false;
                    state.loginState.loginInfo = action.loginInfo;
                    state.loginState.isLoggedIn = true;
                    break;
                }
                case FetchArticlesAction action:
                {
                    state.ArticleState.ArticlesLoading = true;
                    ArticleApi.FetchArticles(action.pageNumber)
                        .Then((articlesResponse) =>
                        {
                            var articleList = new List<string>();
                            var articleDict = new Dictionary<string, Article>();
                            articlesResponse.items.ForEach((item) =>
                            {
                                articleList.Add(item.id);
                                articleDict.Add(item.id,item);
                            });
                            StoreProvider.store.Dispatch(new FetchArticleSuccessAction{ ArticleDict = articleDict,ArticleList = articleList});
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchArticleSuccessAction action:
                {
                    state.ArticleState.ArticleList = action.ArticleList;
                    state.ArticleState.ArticleDict = action.ArticleDict;
                    state.ArticleState.ArticlesLoading = false;
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