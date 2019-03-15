using System.Collections.Generic;
using System.Linq;
using ConnectApp.api;
using ConnectApp.models;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Newtonsoft.Json;
using UnityEngine;

namespace ConnectApp.redux.reducers {
    public static class AppReducer {
        private const string _searchHistoryKey = "searchHistoryKey";
        
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
                    state.eventState.showChatWindow = action.show;
                    break;
                }
                case ChatWindowStatusAction action: {
                    state.eventState.openChatWindow = action.status;
                    break;
                }
                case NavigatorToEventDetailAction action: {
                    state.eventState.detailId = action.eventId;
                    break;
                }
                case NavigatorToArticleDetailAction action: {
                    state.articleState.detailId = action.detailId;
                    break;
                }
                case ClearEventDetailAction action: {
                    state.eventState.detailId = null;
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
                            Debug.Log(error);
                            StoreProvider.store.Dispatch(new LoginByEmailFailedAction());
                        });
                    break;
                }
                case LoginByEmailSuccessAction action: {
                    state.loginState.loading = false;
                    state.loginState.loginInfo = action.loginInfo;
                    state.loginState.isLoggedIn = true;
                    break;
                }
                case LoginByEmailFailedAction action: {
                    state.loginState.loading = false;
                    break;
                }
                case FetchArticlesAction action: {
                    state.articleState.articlesLoading = true;
                    ArticleApi.FetchArticles(action.pageNumber)
                        .Then((articlesResponse) => {
                            var articleList = new List<string>();
                            var articleDict = new Dictionary<string, Article>();
                            articlesResponse.items.ForEach((item) => {
                                articleList.Add(item.id);
                                articleDict.Add(item.id, item);
                            });
                            StoreProvider.store.Dispatch(new UserMapAction()
                                {userMap = articlesResponse.userMap});
                            StoreProvider.store.Dispatch(new FetchArticleSuccessAction
                                {ArticleDict = articleDict, ArticleList = articleList});
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchArticleSuccessAction action: {
                    state.articleState.articleList = action.ArticleList;
                    state.articleState.articleDict = action.ArticleDict;
                    state.articleState.articlesLoading = false;
                    break;
                }
                case FetchArticleDetailAction action: {
                    state.articleState.articleDetailLoading = true;
                    ArticleApi.FetchArticleDetail(action.articleId)
                        .Then((articleDetailResponse) => {
                            
                            if( articleDetailResponse.project.comments.items.Count>0)
                            {
                                var channelId = articleDetailResponse.project.comments.items.first().channelId;
                                var channelMessageList = new Dictionary<string,List<string>>();
                                var channelMessageDict = new Dictionary<string,Dictionary<string, Message>>();
                                var itemIds = new List<string>();
                                var messageItem = new Dictionary<string,Message>();
                                articleDetailResponse.project.comments.items.ForEach((message) =>
                                {
                                    itemIds.Add(message.id);
                                    messageItem[message.id] = message;
                                });
                                channelMessageList.Add(channelId,itemIds);
                                channelMessageDict.Add(channelId,messageItem);
                            
                                StoreProvider.store.Dispatch(new FetchArticleCommentsSuccessAction()
                                {
                                    channelMessageDict = channelMessageDict,
                                    channelMessageList = channelMessageList
                                });
                            }
                            
                            StoreProvider.store.Dispatch(new UserMapAction() {
                                userMap = articleDetailResponse.project.userMap
                            });
                            StoreProvider.store.Dispatch(new FetchArticleDetailSuccessAction() {
                                articleDetail = articleDetailResponse.project
                            });
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchArticleDetailSuccessAction action: {
                    state.articleState.articleDetailLoading = false;
                    state.articleState.articleDetail = action.articleDetail;
                    break;
                }
                case LikeArticleAction action: {
                    ArticleApi.LikeArticle(action.articleId)
                        .Then(() => { StoreProvider.store.Dispatch(new LikeArticleSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case LikeArticleSuccessAction action: {
                    break;
                }
                case FetchArticleCommentsAction action: {
                    ArticleApi.FetchArticleComments(action.channelId, action.currOldestMessageId)
                        .Then((responseComments) =>
                        {
                            var channelMessageList = new Dictionary<string,List<string>>();
                            var channelMessageDict = new Dictionary<string,Dictionary<string, Message>>();
                            var itemIds = new List<string>();
                            var messageItem = new Dictionary<string,Message>();
                            responseComments.items.ForEach((message) =>
                            {
                                itemIds.Add(message.id);
                                messageItem[message.id] = message;
                            });
                            responseComments.parents.ForEach((message) =>
                            {
                                messageItem[message.id] = message;
                            });
                            channelMessageList.Add(action.channelId,itemIds);
                            channelMessageDict.Add(action.channelId,messageItem);
                            
                            StoreProvider.store.Dispatch(new FetchArticleCommentsSuccessAction()
                            {
                                channelMessageDict = channelMessageDict,
                                channelMessageList = channelMessageList
                            });
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchArticleCommentsSuccessAction action:
                {
                    foreach (var keyValuePair in action.channelMessageList)
                    {
                        if (state.messageState.channelMessageList.ContainsKey(keyValuePair.Key))
                        {
                            var oldList = state.messageState.channelMessageList[keyValuePair.Key];
                            oldList.AddRange(keyValuePair.Value);
                            state.messageState.channelMessageList[keyValuePair.Key] = oldList;
                        }
                        else
                        {
                            state.messageState.channelMessageList.Add(keyValuePair.Key,keyValuePair.Value);
                        }
                    }
                    foreach (var keyValuePair in action.channelMessageDict)
                    {
                        if (state.messageState.channelMessageDict.ContainsKey(keyValuePair.Key))
                        {
                            var oldDict = state.messageState.channelMessageDict[keyValuePair.Key];
                            var newDict = state.messageState.channelMessageDict[keyValuePair.Key];
                            foreach (var valuePair in newDict)
                            {
                                if (oldDict.ContainsKey(valuePair.Key))
                                {
                                    oldDict[valuePair.Key] = valuePair.Value;
                                }
                                else
                                {
                                    oldDict.Add(valuePair.Key,valuePair.Value);
                                }
                            }
                            state.messageState.channelMessageDict[keyValuePair.Key] = oldDict;
                        }
                        else
                        {
                            state.messageState.channelMessageDict.Add(keyValuePair.Key,keyValuePair.Value);
                        }
                    }
                    

                    break;
                }
                case LikeCommentAction action: {
                    ArticleApi.LikeComment(action.messageId)
                        .Then(() => { StoreProvider.store.Dispatch(new LikeCommentSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case LikeCommentSuccessAction action: {
                    break;
                }
                case RemoveLikeCommentAction action: {
                    ArticleApi.RemoveLikeComment(action.messageId)
                        .Then(() => { StoreProvider.store.Dispatch(new RemoveLikeSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case RemoveLikeSuccessAction action: {
                    break;
                }
                case SendCommentAction action: {
                    ArticleApi.SendComment(action.channelId, action.content, action.nonce, action.parentMessageId)
                        .Then(() => { StoreProvider.store.Dispatch(new SendCommentSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case SendCommentSuccessAction action: {
                    break;
                }
                case FetchEventsAction action: {
                    state.eventState.eventsLoading = true;
                    EventApi.FetchEvents(action.pageNumber)
                        .Then(events => {
                            StoreProvider.store.Dispatch(new FetchEventsSuccessAction {events = events});
                        })
                        .Catch(error => {
                            state.eventState.eventsLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchEventsSuccessAction action: {
                    state.eventState.eventsLoading = false;
                    action.events.ForEach(eventObj => {
                        state.eventState.events.Add(eventObj.id);
                        state.eventState.eventDict[eventObj.id] = eventObj;
                    });
                    break;
                }
                case FetchEventDetailAction action: {
                    state.eventState.eventDetailLoading = true;
                    EventApi.FetchEventDetail(action.eventId)
                        .Then(eventObj => {
                            StoreProvider.store.Dispatch(new FetchEventDetailSuccessAction {eventObj = eventObj});
                        })
                        .Catch(error => {
                            state.eventState.eventDetailLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchEventDetailSuccessAction action: {
                    state.eventState.eventDetailLoading = false;
                    state.eventState.eventDict[action.eventObj.id] = action.eventObj;
                    state.eventState.detailId = action.eventObj.id;
                    break;
                }
                case JoinEventAction action: {
                    EventApi.JoinEvent(action.eventId)
                        .Then(() => { StoreProvider.store.Dispatch(new JoinEventSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case JoinEventSuccessAction action: {
                    break;
                }
                case FetchNotificationsAction action: {
                    state.notificationState.loading = true;
                    NotificationApi.FetchNotifications(action.pageNumber)
                        .Then(notificationResponse => {
                            StoreProvider.store.Dispatch(new FetchNotificationsSuccessAction {
                                notificationResponse = notificationResponse,
                                pageNumber = action.pageNumber
                            });
                        })
                        .Catch(error => {
                            state.notificationState.loading = false;
                            Debug.Log($"{error}");
                        });
                    break;
                }
                case FetchNotificationsSuccessAction action: {
                    state.notificationState.loading = false;
                    state.notificationState.total = action.notificationResponse.total;
                    if (action.pageNumber == 1) {
                        state.notificationState.notifications = action.notificationResponse.results;
                    } else {
                        var results = state.notificationState.notifications;
                        results.AddRange(action.notificationResponse.results);
                        state.notificationState.notifications = results;
                    }
                    break;
                }
                case ReportItemAction action: {
                    ReportApi.ReportItem(action.itemId, action.itemType, action.reportContext)
                        .Then(() => { StoreProvider.store.Dispatch(new ReportItemSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case ReportItemSuccessAction action: {
                    break;
                }
                case FetchMyFutureEventsAction action: {
                    state.mineState.futureListLoading = true;
                    MineApi.FetchMyFutureEvents(action.pageNumber)
                        .Then(() => { StoreProvider.store.Dispatch(new FetchMyFutureEventsSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchMyFutureEventsSuccessAction action: {
                    state.articleState.articlesLoading = false;
                    break;
                }
                case FetchMyPastEventsAction action: {
                    state.mineState.pastListLoading = true;
                    MineApi.FetchMyPastEvents(action.pageNumber)
                        .Then(() => { StoreProvider.store.Dispatch(new FetchMyPastEventsSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchMyPastEventsSuccessAction action: {
                    state.mineState.pastListLoading = false;
                    break;
                }
                case FetchMessagesAction action: {
                    MessageApi.FetchMessages(action.channelId, action.currOldestMessageId)
                        .Then(() => { StoreProvider.store.Dispatch(new FetchMessagesSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchMessagesSuccessAction action: {
                    break;
                }
                case SendMessageAction action: {
                    MessageApi.SendMessage(action.channelId, action.content, action.nonce, action.parentMessageId)
                        .Then(() => { StoreProvider.store.Dispatch(new SendMessageSuccessAction()); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case SendMessageSuccessAction action: {
                    break;
                }
                case UserMapAction action:
                {
                    foreach (var keyValuePair in action.userMap)
                    {
                        if (state.userState.UserDict.ContainsKey(keyValuePair.Key))
                        {
                            state.userState.UserDict[keyValuePair.Key] = keyValuePair.Value;
                        }
                        else
                        {
                            state.userState.UserDict.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                    }
                    break;
                }
                case SearchArticleAction action: {
                    state.searchState.loading = true;
                    SearchApi.SearchArticle(action.keyword, action.pageNumber)
                        .Then(searchResponse => {
                            StoreProvider.store.Dispatch(new SearchArticleSuccessAction {
                                keyword = action.keyword,
                                pageNumber = action.pageNumber,
                                searchResponse = searchResponse.projects
                            });
                        })
                        .Catch(error => {
                            state.searchState.loading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case SearchArticleSuccessAction action: {
                    state.searchState.loading = false;
                    state.searchState.keyword = action.keyword;
                    if (action.pageNumber == 0) {
                        state.searchState.searchArticles = action.searchResponse;
                    } else {
                        var searchArticles = state.searchState.searchArticles;
                        searchArticles.AddRange(action.searchResponse);
                        state.searchState.searchArticles = searchArticles;
                    }

                    break;
                }
                case ClearSearchArticleAction action: {
                    state.searchState.keyword = "";
                    state.searchState.searchArticles = new List<Article>();
                    break;
                }
                case GetSearchHistoryAction action: {
                    var searchHistory = PlayerPrefs.GetString(_searchHistoryKey);
                    var searchHistoryList = new List<string>();
                    if (searchHistory.isNotEmpty()) {
                        searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
                    }
                    state.searchState.searchHistoryList = searchHistoryList;
                    break;
                }
                case SaveSearchHistoryAction action: {
                    var searchHistory = PlayerPrefs.GetString(_searchHistoryKey);
                    var searchHistoryList = new List<string>();
                    if (searchHistory.isNotEmpty()) {
                        searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
                    }
                    if (searchHistoryList.Contains(action.keyword)) {
                        searchHistoryList.Remove(action.keyword);
                    }
                    searchHistoryList.Insert(0, action.keyword);
                    if (searchHistoryList.Count > 5) {
                        searchHistoryList.RemoveRange(5, searchHistoryList.Count - 5);
                    }
                    state.searchState.searchHistoryList = searchHistoryList;
                    var newSearchHistory = JsonConvert.SerializeObject(searchHistoryList);
                    PlayerPrefs.SetString(_searchHistoryKey, newSearchHistory);
                    PlayerPrefs.Save();
                    break;
                }
                case DeleteSearchHistoryAction action: {
                    var searchHistory = PlayerPrefs.GetString(_searchHistoryKey);
                    var searchHistoryList = new List<string>();
                    if (searchHistory.isNotEmpty()) {
                        searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
                    }
                    if (searchHistoryList.Contains(action.keyword)) {
                        searchHistoryList.Remove(action.keyword);
                    }
                    state.searchState.searchHistoryList = searchHistoryList;
                    var newSearchHistory = JsonConvert.SerializeObject(searchHistoryList);
                    PlayerPrefs.SetString(_searchHistoryKey, newSearchHistory);
                    PlayerPrefs.Save();
                    break;
                }
                case DeleteAllSearchHistoryAction action: {
                    state.searchState.searchHistoryList = new List<string>();
                    PlayerPrefs.DeleteKey(_searchHistoryKey);
                    break;
                }
            }

            return state;
        }
    }
}