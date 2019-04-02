using System;
using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.models;
using ConnectApp.redux.actions;
using ConnectApp.screens;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.redux.reducers {
    public static class AppReducer {
        private const string _searchHistoryKey = "searchHistoryKey";
        private const string _articleHistoryKey = "articleHistoryKey";
        private const string _eventHistoryKey = "eventHistoryKey";

        private static List<string> _nonce = new List<string>();

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
                case LoginByEmailAction action: {
                    state.loginState.loading = true;
                    var email = state.loginState.email;
                    var password = state.loginState.password;
                    LoginApi.LoginByEmail(email, password)
                        .Then(loginInfo => {
                            StoreProvider.store.Dispatch(new LoginByEmailSuccessAction {
                                loginInfo = loginInfo,
                                context = action.context
                            });
                        })
                        .Catch(error => {
                            Debug.Log(error);
                            StoreProvider.store.Dispatch(new LoginByEmailFailedAction {context = action.context});
                        });
                    break;
                }
                case LoginByEmailSuccessAction action: {
                    state.loginState.loading = false;
                    state.loginState.loginInfo = action.loginInfo;
                    var user = new User {
                        id = state.loginState.loginInfo.userId,
                        fullName = state.loginState.loginInfo.userFullName,
                        avatar = state.loginState.loginInfo.userAvatar
                    };
                    var dict = new Dictionary<string, User> {
                        {user.id, user}
                    };
                    StoreProvider.store.Dispatch(new UserMapAction {userMap = dict});
                    state.loginState.isLoggedIn = true;
                    StoreProvider.store.Dispatch(new MainNavigatorPopAction()).Then(() =>
                        StoreProvider.store.Dispatch(new CleanEmailAndPasswordAction()));
                    break;
                }
                case LoginByEmailFailedAction action: {
                    state.loginState.loading = false;
                    var customSnackBar = new CustomSnackBar(
                        "邮箱或密码不正确，请稍后再试。",
                        new TimeSpan(0, 0, 0, 2)
                    );
                    customSnackBar.show(action.context);
                    break;
                }
                case LogoutAction action: {
                    state.loginState.loginInfo = new LoginInfo();
                    state.loginState.isLoggedIn = false;
                    StoreProvider.store.Dispatch(new MainNavigatorPopAction());
                    break;
                }
                case CleanEmailAndPasswordAction action: {
                    state.loginState.email = "";
                    state.loginState.password = "";
                    break;
                }
                case CreateUnityIdUrlAction action: {
                    LoginApi.FetchCreateUnityIdUrl()
                        .Then(url => { StoreProvider.store.Dispatch(new OpenUrlAction {url = url}); })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchArticlesAction action: {
                    state.articleState.articlesLoading = true;
                    ArticleApi.FetchArticles(action.pageNumber)
                        .Then(articlesResponse => {
                            StoreProvider.store.Dispatch(new UserMapAction
                                {userMap = articlesResponse.userMap});
                            StoreProvider.store.Dispatch(new TeamMapAction {teamMap = articlesResponse.teamMap});
                            StoreProvider.store.Dispatch(new FetchArticleSuccessAction {
                                pageNumber = action.pageNumber, articleList = articlesResponse.items,
                                total = articlesResponse.total
                            });
                        })
                        .Catch(error => {
                            state.articleState.articlesLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchArticleSuccessAction action: {
                    if (action.pageNumber == 1) state.articleState.articleList.Clear();
                    var articleList = state.articleState.articleList;
                    foreach (var article in action.articleList) {
                        articleList.Add(article.id);
                        if (!state.articleState.articleDict.ContainsKey(article.id))
                            state.articleState.articleDict.Add(article.id, article);
                    }

                    state.articleState.pageNumber = action.pageNumber;
                    state.articleState.articleTotal = action.total;
                    state.articleState.articlesLoading = false;
                    break;
                }
                case FetchArticleDetailAction action: {
                    state.articleState.articleDetailLoading = true;
                    ArticleApi.FetchArticleDetail(action.articleId)
                        .Then(articleDetailResponse => {
                            if (articleDetailResponse.project.comments.items.Count > 0)
                                StoreProvider.store.Dispatch(new FetchArticleCommentsSuccessAction {
                                    channelId = articleDetailResponse.project.channelId,
                                    commentsResponse = articleDetailResponse.project.comments,
                                    isRefreshList = true
                                });

                            StoreProvider.store.Dispatch(new UserMapAction {
                                userMap = articleDetailResponse.project.userMap
                            });
                            StoreProvider.store.Dispatch(new TeamMapAction {
                                teamMap = articleDetailResponse.project.teamMap
                            });
                            StoreProvider.store.Dispatch(new FetchArticleDetailSuccessAction {
                                articleDetail = articleDetailResponse.project
                            });
                        })
                        .Catch(error => {
                            state.articleState.articleDetailLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchArticleDetailSuccessAction action: {
                    state.articleState.articleDetailLoading = false;
                    var article = action.articleDetail.projectData;
                    article.like = action.articleDetail.like;
                    article.edit = action.articleDetail.edit;
                    article.projects = action.articleDetail.projects;
                    article.channelId = action.articleDetail.channelId;
                    article.contentMap = action.articleDetail.contentMap;
                    article.hasMore = action.articleDetail.comments.hasMore;
                    article.currOldestMessageId = action.articleDetail.comments.currOldestMessageId;
                    var dict = state.articleState.articleDict;
                    if (dict.ContainsKey(article.id))
                        state.articleState.articleDict[article.id] = article;
                    else
                        state.articleState.articleDict.Add(article.id, article);

                    StoreProvider.store.Dispatch(new SaveArticleHistoryAction {
                        article = article
                    });
                    break;
                }
                case SaveArticleHistoryAction action: {
                    var articleHistory = PlayerPrefs.GetString(_articleHistoryKey);
                    var articleHistoryList = new List<Article>();
                    if (articleHistory.isNotEmpty())
                        articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
                    articleHistoryList.RemoveAll(item => item.id == action.article.id);
                    articleHistoryList.Insert(0, action.article);
                    state.articleState.articleHistory = articleHistoryList;
                    var newArticleHistory = JsonConvert.SerializeObject(articleHistoryList);
                    PlayerPrefs.SetString(_articleHistoryKey, newArticleHistory);
                    PlayerPrefs.Save();
                    break;
                }
                case GetArticleHistoryAction action: {
                    var articleHistory = PlayerPrefs.GetString(_articleHistoryKey);
                    var articleHistoryList = new List<Article>();
                    if (articleHistory.isNotEmpty())
                        articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
                    state.articleState.articleHistory = articleHistoryList;
                    break;
                }
                case DeleteArticleHistoryAction action: {
                    var articleHistory = PlayerPrefs.GetString(_articleHistoryKey);
                    var articleHistoryList = new List<Article>();
                    if (articleHistory.isNotEmpty())
                        articleHistoryList = JsonConvert.DeserializeObject<List<Article>>(articleHistory);
                    articleHistoryList.RemoveAll(item => item.id == action.articleId);
                    state.articleState.articleHistory = articleHistoryList;
                    var newArticleHistory = JsonConvert.SerializeObject(articleHistoryList);
                    PlayerPrefs.SetString(_articleHistoryKey, newArticleHistory);
                    PlayerPrefs.Save();
                    break;
                }
                case DeleteAllArticleHistoryAction action: {
                    state.articleState.articleHistory = new List<Article>();
                    PlayerPrefs.DeleteKey(_articleHistoryKey);
                    break;
                }
                case LikeArticleAction action: {
                    ArticleApi.LikeArticle(action.articleId)
                        .Then(() => {
                            StoreProvider.store.Dispatch(new LikeArticleSuccessAction() {articleId = action.articleId});
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case LikeArticleSuccessAction action: {
                    if (state.articleState.articleDict.ContainsKey(action.articleId))
                        state.articleState.articleDict[action.articleId].like = true;

                    break;
                }
                case FetchArticleCommentsAction action: {
                    ArticleApi.FetchArticleComments(action.channelId, action.currOldestMessageId)
                        .Then(responseComments => {
                            StoreProvider.store.Dispatch(new FetchArticleCommentsSuccessAction {
                                channelId = action.channelId,
                                commentsResponse = responseComments,
                                isRefreshList = false
                            });
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case FetchArticleCommentsSuccessAction action: {
                    var channelMessageList = new Dictionary<string, List<string>>();
                    var channelMessageDict = new Dictionary<string, Dictionary<string, Message>>();
                    var itemIds = new List<string>();
                    var messageItem = new Dictionary<string, Message>();

                    var userMap = new Dictionary<string, User>();
                    action.commentsResponse.items.ForEach(message => {
                        itemIds.Add(message.id);
                        messageItem[message.id] = message;
                        if (userMap.ContainsKey(message.author.id))
                            userMap[message.author.id] = message.author;
                        else
                            userMap.Add(message.author.id, message.author);
                    });
                    // action.commentsResponse.parents.ForEach(message => { messageItem[message.id] = message; });
                    channelMessageList.Add(action.channelId, itemIds);
                    channelMessageDict.Add(action.channelId, messageItem);
                    StoreProvider.store.Dispatch(new UserMapAction {
                        userMap = userMap
                    });

                    if (action.channelId.isNotEmpty())
                        foreach (var dict in state.articleState.articleDict)
                            if (dict.Value.channelId == action.channelId) {
                                dict.Value.hasMore = action.hasMore;
                                dict.Value.currOldestMessageId = action.currOldestMessageId;
                            }

                    foreach (var keyValuePair in channelMessageList)
                        if (state.messageState.channelMessageList.ContainsKey(keyValuePair.Key)) {
                            var oldList = state.messageState.channelMessageList[keyValuePair.Key];
                            if (action.isRefreshList) oldList.Clear();
                            oldList.AddRange(keyValuePair.Value);
                            state.messageState.channelMessageList[keyValuePair.Key] = oldList;
                        }
                        else {
                            state.messageState.channelMessageList.Add(keyValuePair.Key, keyValuePair.Value);
                        }

                    foreach (var keyValuePair in channelMessageDict)
                        if (state.messageState.channelMessageDict.ContainsKey(keyValuePair.Key)) {
                            var oldDict = state.messageState.channelMessageDict[keyValuePair.Key];
                            var newDict = keyValuePair.Value;
                            foreach (var valuePair in newDict)
                                if (oldDict.ContainsKey(valuePair.Key))
                                    oldDict[valuePair.Key] = valuePair.Value;
                                else
                                    oldDict.Add(valuePair.Key, valuePair.Value);
                            state.messageState.channelMessageDict[keyValuePair.Key] = oldDict;
                        }
                        else {
                            state.messageState.channelMessageDict.Add(keyValuePair.Key, keyValuePair.Value);
                        }

                    break;
                }
                case LikeCommentAction action: {
                    ArticleApi.LikeComment(action.messageId)
                        .Then((message) => {
                            StoreProvider.store.Dispatch(new LikeCommentSuccessAction() {message = message});
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case LikeCommentSuccessAction action: {
                    state.messageState.channelMessageDict[action.message.channelId][action.message.id] = action.message;
                    break;
                }
                case RemoveLikeCommentAction action: {
                    ArticleApi.RemoveLikeComment(action.messageId)
                        .Then((message) => {
                            StoreProvider.store.Dispatch(new RemoveLikeSuccessAction() {message = message});
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case RemoveLikeSuccessAction action: {
                    state.messageState.channelMessageDict[action.message.channelId][action.message.id] = action.message;
                    break;
                }
                case SendCommentAction action: {
                    ArticleApi.SendComment(action.channelId, action.content, action.nonce, action.parentMessageId)
                        .Then((message) => {
                            StoreProvider.store.Dispatch(new SendCommentSuccessAction() {
                                message = message
                            });
                        })
                        .Catch(error => { Debug.Log(error); });
                    break;
                }
                case SendCommentSuccessAction action: {
                    if (state.messageState.channelMessageList.ContainsKey(action.message.channelId)) {
                        var list = state.messageState.channelMessageList[action.message.channelId];
                        list.Insert(0, action.message.id);
                        state.messageState.channelMessageList[action.message.channelId] = list;
                    }
                    else {
                        state.messageState.channelMessageList.Add(action.message.channelId,
                            new List<string> {action.message.id});
                    }

                    if (state.messageState.channelMessageDict.ContainsKey(action.message.channelId)) {
                        var dict = state.messageState.channelMessageDict[action.message.channelId];
                        dict.Add(action.message.id, action.message);
                        state.messageState.channelMessageDict[action.message.channelId] = dict;
                    }
                    else {
                        state.messageState.channelMessageDict.Add(
                            action.message.channelId,
                            new Dictionary<string, Message> {
                                {action.message.id, action.message}
                            }
                        );
                    }

                    break;
                }
                case FetchEventsAction action: {
                    state.eventState.eventsLoading = true;
                    EventApi.FetchEvents(action.pageNumber, action.tab)
                        .Then(eventsResponse => {
                            StoreProvider.store.Dispatch(new UserMapAction {userMap = eventsResponse.userMap});
                            StoreProvider.store.Dispatch(new PlaceMapAction {placeMap = eventsResponse.placeMap});
                            StoreProvider.store.Dispatch(new FetchEventsSuccessAction {
                                    events = eventsResponse.events.items, total = eventsResponse.events.total,
                                    tab = action.tab, pageNumber = action.pageNumber
                                }
                            );
                        })
                        .Catch(error => {
                            state.eventState.eventsLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchEventsSuccessAction action: {
                    state.eventState.eventsLoading = false;
                    if (action.tab == "ongoing") {
                        state.eventState.pageNumber = action.pageNumber;
                        state.eventState.ongoingEventTotal = action.total;
                    }
                    else {
                        state.eventState.completedPageNumber = action.pageNumber;
                        state.eventState.completedEventTotal = action.total;
                    }


                    if (action.pageNumber == 1) {
                        if (action.tab == "ongoing")
                            state.eventState.ongoingEvents.Clear();
                        else
                            state.eventState.completedEvents.Clear();
                    }

                    action.events.ForEach(eventObj => {
                        if (action.tab == "ongoing") {
                            if (!state.eventState.ongoingEvents.Contains(eventObj.id))
                                state.eventState.ongoingEvents.Add(eventObj.id);
                        }
                        else {
                            if (!state.eventState.completedEvents.Contains(eventObj.id))
                                state.eventState.completedEvents.Add(eventObj.id);
                        }

                        if (state.eventState.eventsDict.ContainsKey(eventObj.id))
                            state.eventState.eventsDict[eventObj.id] = eventObj;
                        else
                            state.eventState.eventsDict.Add(eventObj.id, eventObj);
                    });
                    break;
                }
                case FetchEventDetailAction action: {
                    state.eventState.eventDetailLoading = true;
                    EventApi.FetchEventDetail(action.eventId)
                        .Then(eventObj => {
                            var isLoggedIn = StoreProvider.store.state.loginState.isLoggedIn;
                            if (isLoggedIn)
                                StoreProvider.store.Dispatch(new FetchMessagesAction
                                    {channelId = eventObj.channelId, isFirstLoad = true});
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
                    state.eventState.channelId = action.eventObj.channelId;
                    var userMap = new Dictionary<string, User> {
                        {action.eventObj.user.id, action.eventObj.user}
                    };
                    action.eventObj.hosts.ForEach(host => {
                        if (userMap.ContainsKey(host.id))
                            userMap[host.id] = host;
                        else
                            userMap.Add(host.id, host);
                    });
                    StoreProvider.store.Dispatch(new UserMapAction {userMap = userMap});
                    if (state.eventState.eventsDict.ContainsKey(action.eventObj.id))
                        state.eventState.eventsDict[action.eventObj.id] = action.eventObj;
                    else
                        state.eventState.eventsDict.Add(action.eventObj.id, action.eventObj);
                    StoreProvider.store.Dispatch(new SaveEventHistoryAction {eventObj = action.eventObj});
                    break;
                }
                case SaveEventHistoryAction action: {
                    var eventHistory = PlayerPrefs.GetString(_eventHistoryKey);
                    var eventHistoryList = new List<IEvent>();
                    if (eventHistory.isNotEmpty())
                        eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
                    eventHistoryList.RemoveAll(item => item.id == action.eventObj.id);
                    eventHistoryList.Insert(0, action.eventObj);
                    state.eventState.eventHistory = eventHistoryList;
                    var newEventHistory = JsonConvert.SerializeObject(eventHistoryList);
                    PlayerPrefs.SetString(_eventHistoryKey, newEventHistory);
                    PlayerPrefs.Save();
                    break;
                }
                case GetEventHistoryAction action: {
                    var eventHistory = PlayerPrefs.GetString(_eventHistoryKey);
                    var eventHistoryList = new List<IEvent>();
                    if (eventHistory.isNotEmpty())
                        eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
                    state.eventState.eventHistory = eventHistoryList;
                    break;
                }
                case DeleteEventHistoryAction action: {
                    var eventHistory = PlayerPrefs.GetString(_eventHistoryKey);
                    var eventHistoryList = new List<IEvent>();
                    if (eventHistory.isNotEmpty())
                        eventHistoryList = JsonConvert.DeserializeObject<List<IEvent>>(eventHistory);
                    eventHistoryList.RemoveAll(item => item.id == action.eventId);
                    state.eventState.eventHistory = eventHistoryList;
                    var newEventHistory = JsonConvert.SerializeObject(eventHistoryList);
                    PlayerPrefs.SetString(_eventHistoryKey, newEventHistory);
                    PlayerPrefs.Save();
                    break;
                }
                case DeleteAllEventHistoryAction action: {
                    state.eventState.eventHistory = new List<IEvent>();
                    PlayerPrefs.DeleteKey(_eventHistoryKey);
                    break;
                }
                case JoinEventAction action: {
                    state.eventState.joinEventLoading = true;
                    EventApi.JoinEvent(action.eventId)
                        .Then(eventId => {
                            StoreProvider.store.Dispatch(new JoinEventSuccessAction {eventId = action.eventId});
                        })
                        .Catch(error => {
                            state.eventState.joinEventLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case JoinEventSuccessAction action: {
                    state.eventState.joinEventLoading = false;
                    var eventObj = state.eventState.eventsDict[action.eventId];
                    eventObj.userIsCheckedIn = true;
                    state.eventState.eventsDict[action.eventId] = eventObj;
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
                    var oldResults = action.notificationResponse.results;
                    if (oldResults != null && oldResults.Count > 0)
                        oldResults.ForEach(item => {
                            var data = item.data;
                            var user = new User {
                                id = data.userId,
                                fullName = data.fullname
                            };
                            StoreProvider.store.Dispatch(new UserMapAction {
                                userMap = new Dictionary<string, User> {
                                    {data.userId, user}
                                }
                            });
                        });
                    if (action.pageNumber == 1) {
                        state.notificationState.notifications = action.notificationResponse.results;
                    }
                    else {
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
                        .Then(events => {
                            StoreProvider.store.Dispatch(new FetchMyFutureEventsSuccessAction {
                                events = events,
                                pageNumber = action.pageNumber
                            });
                        })
                        .Catch(error => {
                            state.mineState.futureListLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchMyFutureEventsSuccessAction action: {
                    state.mineState.futureListLoading = false;
                    if (action.events != null && action.events.Count > 0)
                        action.events.ForEach(item => {
                            var userMap = new Dictionary<string, User> {
                                {item.user.id, item.user}
                            };
                            StoreProvider.store.Dispatch(new UserMapAction {userMap = userMap});
                        });
                    if (action.pageNumber == 0) {
                        state.mineState.futureEventsList = action.events;
                    }
                    else {
                        var results = state.mineState.futureEventsList;
                        results.AddRange(action.events);
                        state.mineState.futureEventsList = results;
                    }

                    break;
                }
                case FetchMyPastEventsAction action: {
                    state.mineState.pastListLoading = true;
                    MineApi.FetchMyPastEvents(action.pageNumber)
                        .Then(events => {
                            StoreProvider.store.Dispatch(new FetchMyPastEventsSuccessAction {
                                events = events,
                                pageNumber = action.pageNumber
                            });
                        })
                        .Catch(error => {
                            state.mineState.pastListLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchMyPastEventsSuccessAction action: {
                    state.mineState.pastListLoading = false;
                    if (action.events != null && action.events.Count > 0)
                        action.events.ForEach(item => {
                            var userMap = new Dictionary<string, User> {
                                {item.user.id, item.user}
                            };
                            StoreProvider.store.Dispatch(new UserMapAction {userMap = userMap});
                        });
                    if (action.pageNumber == 0) {
                        state.mineState.pastEventsList = action.events;
                    }
                    else {
                        var results = state.mineState.pastEventsList;
                        results.AddRange(action.events);
                        state.mineState.pastEventsList = results;
                    }

                    break;
                }
                case FetchMessagesAction action: {
                    StoreProvider.store.state.messageState.messageLoading = true;
                    MessageApi.FetchMessages(action.channelId, action.currOldestMessageId)
                        .Then(messagesResponse => {
                            StoreProvider.store.Dispatch(new FetchMessagesSuccessAction {
                                isFirstLoad = action.isFirstLoad,
                                channelId = action.channelId,
                                messages = messagesResponse.items,
                                hasMore = messagesResponse.hasMore,
                                currOldestMessageId = messagesResponse.currOldestMessageId
                            });
                        })
                        .Catch(error => {
                            StoreProvider.store.state.messageState.messageLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case FetchMessagesSuccessAction action: {
                    StoreProvider.store.state.messageState.messageLoading = false;
                    if (action.messages != null && action.messages.Count > 0) {
                        var channelMessageList = state.messageState.channelMessageList;
                        var channelMessageDict = state.messageState.channelMessageDict;
                        var messageIds = new List<string>();
                        if (channelMessageList.ContainsKey(action.channelId) && !action.isFirstLoad)
                            messageIds = channelMessageList[action.channelId];
                        var messageDict = new Dictionary<string, Message>();
                        if (channelMessageDict.ContainsKey(action.channelId) && !action.isFirstLoad)
                            messageDict = channelMessageDict[action.channelId];

                        var userMap = new Dictionary<string, User>();
                        action.messages.ForEach(message => {
                            if (message.deletedTime == null && message.type == "normal") {
                                if (messageIds.Contains(message.id)) messageIds.Remove(message.id);
                                messageIds.Add(message.id);

                                if (messageDict.ContainsKey(message.id))
                                    messageDict[message.id] = message;
                                else
                                    messageDict.Add(message.id, message);
                            }

                            if (userMap.ContainsKey(message.author.id))
                                userMap[message.author.id] = message.author;
                            else
                                userMap.Add(message.author.id, message.author);
                        });

                        if (channelMessageList.ContainsKey(action.channelId))
                            channelMessageList[action.channelId] = messageIds;
                        else
                            channelMessageList.Add(action.channelId, messageIds);
                        if (channelMessageDict.ContainsKey(action.channelId))
                            channelMessageDict[action.channelId] = messageDict;
                        else
                            channelMessageDict.Add(action.channelId, messageDict);
                        StoreProvider.store.Dispatch(new UserMapAction {
                            userMap = userMap
                        });
                        state.messageState.channelMessageList = channelMessageList;
                        state.messageState.channelMessageDict = channelMessageDict;
                        state.messageState.hasMore = action.hasMore;
                        state.messageState.currOldestMessageId = action.currOldestMessageId;
                    }

                    break;
                }
                case SendMessageAction action: {
                    StoreProvider.store.state.messageState.sendMessageLoading = true;
                    MessageApi.SendMessage(action.channelId, action.content, action.nonce, action.parentMessageId)
                        .Then(sendMessageResponse => {
                            StoreProvider.store.Dispatch(new SendMessageSuccessAction {
                                channelId = action.channelId,
                                content = action.content,
                                nonce = action.nonce
                            });
                        })
                        .Catch(error => {
                            StoreProvider.store.state.messageState.sendMessageLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case SendMessageSuccessAction action: {
                    var channelMessageList = state.messageState.channelMessageList;
                    var channelMessageDict = state.messageState.channelMessageDict;
                    var messageIds = new List<string>();
                    if (channelMessageList.ContainsKey(action.channelId))
                        messageIds = channelMessageList[action.channelId];
                    var messageDict = new Dictionary<string, Message>();
                    if (channelMessageDict.ContainsKey(action.channelId))
                        messageDict = channelMessageDict[action.channelId];

                    messageIds.Insert(0, action.nonce);
                    if (channelMessageList.ContainsKey(action.channelId))
                        channelMessageList[action.channelId] = messageIds;
                    else
                        channelMessageList.Add(action.channelId, messageIds);

                    var previewMsg = new Message {
                        id = action.nonce,
                        content = action.content,
                        author = new User {
                            id = StoreProvider.store.state.loginState.loginInfo.userId,
                            fullName = StoreProvider.store.state.loginState.loginInfo.userFullName
                        }
                    };
                    _nonce.Add(action.nonce);
                    if (messageDict.ContainsKey(action.nonce))
                        messageDict[action.nonce] = previewMsg;
                    else
                        messageDict.Add(action.nonce, previewMsg);
                    state.messageState.channelMessageList = channelMessageList;
                    state.messageState.channelMessageDict = channelMessageDict;
                    StoreProvider.store.state.messageState.sendMessageLoading = false;

                    break;
                }
                case UserMapAction action: {
                    foreach (var keyValuePair in action.userMap)
                        if (state.userState.userDict.ContainsKey(keyValuePair.Key))
                            state.userState.userDict[keyValuePair.Key] = keyValuePair.Value;
                        else
                            state.userState.userDict.Add(keyValuePair.Key, keyValuePair.Value);
                    break;
                }
                case TeamMapAction action: {
                    foreach (var keyValuePair in action.teamMap)
                        if (state.teamState.teamDict.ContainsKey(keyValuePair.Key))
                            state.teamState.teamDict[keyValuePair.Key] = keyValuePair.Value;
                        else
                            state.teamState.teamDict.Add(keyValuePair.Key, keyValuePair.Value);
                    break;
                }
                case PlaceMapAction action: {
                    foreach (var keyValuePair in action.placeMap)
                        if (state.placeState.placeDict.ContainsKey(keyValuePair.Key))
                            state.placeState.placeDict[keyValuePair.Key] = keyValuePair.Value;
                        else
                            state.placeState.placeDict.Add(keyValuePair.Key, keyValuePair.Value);
                    break;
                }
                case SearchArticleAction action: {
                    state.searchState.loading = true;
                    SearchApi.SearchArticle(action.keyword, action.pageNumber)
                        .Then(searchResponse => {
                            StoreProvider.store.Dispatch(new SearchArticleSuccessAction {
                                keyword = action.keyword,
                                pageNumber = action.pageNumber,
                                searchResponse = searchResponse
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
                    StoreProvider.store.Dispatch(new UserMapAction {
                        userMap = action.searchResponse.userMap
                    });
                    StoreProvider.store.Dispatch(new TeamMapAction {teamMap = action.searchResponse.teamMap});
                    state.searchState.currentPage = action.searchResponse.currentPage;
                    state.searchState.pages = action.searchResponse.pages;
                    if (action.pageNumber == 0) {
                        state.searchState.searchArticles = action.searchResponse.projects;
                    }
                    else {
                        var searchArticles = state.searchState.searchArticles;
                        searchArticles.AddRange(action.searchResponse.projects);
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
                    if (searchHistory.isNotEmpty())
                        searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
                    state.searchState.searchHistoryList = searchHistoryList;
                    break;
                }
                case SaveSearchHistoryAction action: {
                    var searchHistory = PlayerPrefs.GetString(_searchHistoryKey);
                    var searchHistoryList = new List<string>();
                    if (searchHistory.isNotEmpty())
                        searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
                    if (searchHistoryList.Contains(action.keyword)) searchHistoryList.Remove(action.keyword);
                    searchHistoryList.Insert(0, action.keyword);
                    if (searchHistoryList.Count > 5) searchHistoryList.RemoveRange(5, searchHistoryList.Count - 5);
                    state.searchState.searchHistoryList = searchHistoryList;
                    var newSearchHistory = JsonConvert.SerializeObject(searchHistoryList);
                    PlayerPrefs.SetString(_searchHistoryKey, newSearchHistory);
                    PlayerPrefs.Save();
                    break;
                }
                case DeleteSearchHistoryAction action: {
                    var searchHistory = PlayerPrefs.GetString(_searchHistoryKey);
                    var searchHistoryList = new List<string>();
                    if (searchHistory.isNotEmpty())
                        searchHistoryList = JsonConvert.DeserializeObject<List<string>>(searchHistory);
                    if (searchHistoryList.Contains(action.keyword)) searchHistoryList.Remove(action.keyword);
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
                case MainNavigatorPushToArticleDetailAction action: {
                    if (action.ArticleId != null)
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new ArticleDetailScreen(articleId: action.ArticleId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                ))
                        );
                    break;
                }
                case MainNavigatorPushToEventDetailAction action: {
                    if (action.EventId != null)
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new EventDetailScreen(eventId: action.EventId, eventType: action.EventType),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                ))
                        );
                    break;
                }
                case MainNavigatorPushToAction action: {
                    Router.navigator.pushNamed(action.RouteName);
                    break;
                }
                case MainNavigatorPopAction action: {
                    for (var i = 0; i < action.Index; i++)
                        if (Router.navigator.canPop())
                            Router.navigator.pop();
                    break;
                }
                case LoginNavigatorPushToBindUintyAction action: {
                    LoginScreen.navigator.push(new PageRouteBuilder(
                        pageBuilder: (context, animation, secondaryAnimation) =>
                            new BindUnityScreen(fromPage: action.FromPage),
                        transitionsBuilder: (context1, animation, secondaryAnimation, child) => new PushPageTransition(
                            routeAnimation: animation,
                            child: child
                        ))
                    );
                    break;
                }
                case LoginNavigatorPushToAction action: {
                    LoginScreen.navigator.pushNamed(action.RouteName);
                    break;
                }
                case LoginNavigatorPopAction action: {
                    for (var i = 0; i < action.Index; i++)
                        if (LoginScreen.navigator.canPop())
                            LoginScreen.navigator.pop();
                    break;
                }
                case OpenUrlAction action: {
                    Application.OpenURL(action.url);
                    break;
                }
                case SettingReviewUrlAction action: {
                    StoreProvider.store.state.settingState.reviewLoading = true;
                    SettingApi.FetchReviewUrl(action.platform, action.store)
                        .Then(url => { StoreProvider.store.Dispatch(new SettingReviewUrlSuccessAction {url = url}); })
                        .Catch(error => {
                            StoreProvider.store.state.settingState.reviewLoading = false;
                            Debug.Log(error);
                        });
                    break;
                }
                case SettingReviewUrlSuccessAction action: {
                    StoreProvider.store.state.settingState.reviewLoading = false;
                    StoreProvider.store.state.settingState.reviewUrl = action.url;
                    break;
                }
            }

            return state;
        }
    }
}