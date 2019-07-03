using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Main;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.redux.actions;
using ConnectApp.screens;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;
using UnityEngine;
using EventType = ConnectApp.Models.State.EventType;

namespace ConnectApp.redux.reducers {
    public static class AppReducer {
        static readonly List<string> _nonce = new List<string>();

        public static AppState Reduce(AppState state, object bAction) {
            switch (bAction) {
                case AddCountAction action: {
                    state.Count += action.number;
                    PlayerPrefs.SetInt("count", value: state.Count);
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

                case ShowChatWindowAction action: {
                    state.eventState.showChatWindow = action.show;
                    break;
                }

                case ChatWindowStatusAction action: {
                    state.eventState.openChatWindow = action.status;
                    break;
                }

                case StartLoginByEmailAction _: {
                    state.loginState.loading = true;
                    break;
                }

                case LoginByEmailSuccessAction action: {
                    state.loginState.loading = false;
                    state.loginState.loginInfo = action.loginInfo;
                    state.loginState.isLoggedIn = true;
                    state.articleState.articleHistory =
                        HistoryManager.articleHistoryList(userId: action.loginInfo.userId);
                    state.eventState.eventHistory = HistoryManager.eventHistoryList(userId: action.loginInfo.userId);
                    state.searchState.searchHistoryList =
                        HistoryManager.searchHistoryList(userId: action.loginInfo.userId);
                    state.articleState.blockArticleList =
                        HistoryManager.blockArticleList(userId: action.loginInfo.userId);
                    EventBus.publish(sName: EventBusConstant.login_success, new List<object>());
                    break;
                }

                case LoginByEmailFailureAction _: {
                    state.loginState.loading = false;
                    break;
                }

                case LoginByWechatAction action: {
                    state.loginState.loading = true;
                    break;
                }

                case LoginByWechatSuccessAction action: {
                    state.loginState.loading = false;
                    state.loginState.loginInfo = action.loginInfo;
                    state.loginState.isLoggedIn = true;
                    state.articleState.articleHistory =
                        HistoryManager.articleHistoryList(userId: action.loginInfo.userId);
                    state.eventState.eventHistory = HistoryManager.eventHistoryList(userId: action.loginInfo.userId);
                    state.searchState.searchHistoryList =
                        HistoryManager.searchHistoryList(userId: action.loginInfo.userId);
                    EventBus.publish(sName: EventBusConstant.login_success, new List<object>());
                    break;
                }

                case LoginByWechatFailureAction action: {
                    state.loginState.loading = false;
                    break;
                }

                case LogoutAction _: {
                    HttpManager.clearCookie();
                    state.loginState.loginInfo = new LoginInfo();
                    state.loginState.isLoggedIn = false;
                    UserInfoManager.clearUserInfo();
                    state.articleState.articleHistory = HistoryManager.articleHistoryList();
                    state.eventState.eventHistory = HistoryManager.eventHistoryList();
                    state.searchState.searchHistoryList = HistoryManager.searchHistoryList();
                    state.articleState.blockArticleList = HistoryManager.blockArticleList();
                    break;
                }

                case CleanEmailAndPasswordAction action: {
                    state.loginState.email = "";
                    state.loginState.password = "";
                    break;
                }

                case JumpToCreateUnityIdAction _: {
                    break;
                }

                case StartFetchArticlesAction _: {
                    state.articleState.articlesLoading = true;
                    break;
                }

                case FetchArticleSuccessAction action: {
                    if (action.offset == 0) {
                        state.articleState.articleList.Clear();
                    }

                    foreach (var article in action.articleList) {
                        state.articleState.articleList.Add(item: article.id);
                        if (!state.articleState.articleDict.ContainsKey(key: article.id)) {
                            state.articleState.articleDict.Add(key: article.id, value: article);
                        }
                    }

                    state.articleState.hottestHasMore = action.hottestHasMore;
                    state.articleState.articlesLoading = false;
                    break;
                }

                case FetchArticleFailureAction _: {
                    state.articleState.articlesLoading = false;
                    break;
                }

                case StartFetchArticleDetailAction _: {
                    state.articleState.articleDetailLoading = true;
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
                    article.isNotFirst = true;
                    article.currOldestMessageId = action.articleDetail.comments.currOldestMessageId;
                    var dict = state.articleState.articleDict;
                    if (dict.ContainsKey(key: article.id)) {
                        state.articleState.articleDict[key: article.id] = article;
                    }
                    else {
                        state.articleState.articleDict.Add(key: article.id, value: article);
                    }

                    break;
                }

                case FetchArticleDetailFailureAction _: {
                    state.articleState.articleDetailLoading = false;
                    break;
                }

                case SaveArticleHistoryAction action: {
                    var article = action.article;
                    var fullName = "";
                    if (article.ownerType == "user") {
                        var userDict = state.userState.userDict;
                        if (article.userId != null && userDict.ContainsKey(key: article.userId)) {
                            fullName = userDict[key: article.userId].fullName;
                        }
                    }

                    if (article.ownerType == "team") {
                        var teamDict = state.teamState.teamDict;
                        if (article.teamId != null && teamDict.ContainsKey(key: article.teamId)) {
                            fullName = teamDict[key: article.teamId].name;
                        }
                    }

                    article.fullName = fullName;
                    var articleHistoryList = HistoryManager.saveArticleHistory(article: article,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.articleState.articleHistory = articleHistoryList;
                    break;
                }

                case DeleteArticleHistoryAction action: {
                    var articleHistoryList = HistoryManager.deleteArticleHistory(articleId: action.articleId,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.articleState.articleHistory = articleHistoryList;
                    break;
                }

                case DeleteAllArticleHistoryAction _: {
                    state.articleState.articleHistory = new List<Article>();
                    HistoryManager.deleteAllArticleHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    break;
                }

                case LikeArticleAction _: {
                    break;
                }

                case LikeArticleSuccessAction action: {
                    if (state.articleState.articleDict.ContainsKey(key: action.articleId)) {
                        state.articleState.articleDict[key: action.articleId].like = true;
                    }

                    break;
                }

                case BlockArticleAction action: {
                    var blockArticleList =
                        HistoryManager.saveBlockArticleList(articleId: action.articleId,
                            userId: state.loginState.loginInfo.userId);
                    state.articleState.blockArticleList = blockArticleList;
                    break;
                }

                case StartFetchArticleCommentsAction _: {
                    break;
                }

                case FetchArticleCommentsSuccessAction action: {
                    var channelMessageList = new Dictionary<string, List<string>>();
                    var channelMessageDict = new Dictionary<string, Dictionary<string, Message>>();

                    // action.commentsResponse.parents.ForEach(message => { messageItem[message.id] = message; });
                    channelMessageList.Add(key: action.channelId, value: action.itemIds);
                    channelMessageDict.Add(key: action.channelId, value: action.messageItems);

                    if (action.channelId.isNotEmpty()) {
                        foreach (var dict in state.articleState.articleDict) {
                            if (dict.Value.channelId == action.channelId) {
                                dict.Value.hasMore = action.hasMore;
                                dict.Value.currOldestMessageId = action.currOldestMessageId;
                            }
                        }
                    }

                    foreach (var keyValuePair in channelMessageList) {
                        if (state.messageState.channelMessageList.ContainsKey(key: keyValuePair.Key)) {
                            var oldList = state.messageState.channelMessageList[key: keyValuePair.Key];
                            if (action.isRefreshList) {
                                oldList.Clear();
                            }

                            oldList.AddRange(collection: keyValuePair.Value);
                            var newList = oldList.Distinct().ToList();
                            state.messageState.channelMessageList[key: keyValuePair.Key] = newList;
                        }
                        else {
                            state.messageState.channelMessageList.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                        }
                    }

                    foreach (var keyValuePair in channelMessageDict) {
                        if (state.messageState.channelMessageDict.ContainsKey(key: keyValuePair.Key)) {
                            var oldDict = state.messageState.channelMessageDict[key: keyValuePair.Key];
                            var newDict = keyValuePair.Value;
                            foreach (var valuePair in newDict) {
                                if (oldDict.ContainsKey(key: valuePair.Key)) {
                                    oldDict[key: valuePair.Key] = valuePair.Value;
                                }
                                else {
                                    oldDict.Add(key: valuePair.Key, value: valuePair.Value);
                                }
                            }

                            state.messageState.channelMessageDict[key: keyValuePair.Key] = oldDict;
                        }
                        else {
                            state.messageState.channelMessageDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                        }
                    }

                    break;
                }

                case StartLikeCommentAction _: {
                    break;
                }

                case LikeCommentSuccessAction action: {
                    var user = new User();
                    user.id = state.loginState.loginInfo.userId;
                    var reaction = new Reaction();
                    reaction.user = user;
                    action.message.reactions.Add(item: reaction);
                    state.messageState.channelMessageDict[key: action.message.channelId][key: action.message.id] =
                        action.message;
                    break;
                }

                case LikeCommentFailureAction _: {
                    break;
                }

                case StartRemoveLikeCommentAction action: {
                    break;
                }

                case RemoveLikeCommentSuccessAction action: {
                    var reactions = action.message.reactions;
                    foreach (var reaction in reactions) {
                        if (reaction.user.id == state.loginState.loginInfo.userId) {
                            action.message.reactions.Remove(item: reaction);
                            break;
                        }
                    }

                    state.messageState.channelMessageDict[key: action.message.channelId][key: action.message.id] =
                        action.message;
                    break;
                }

                case StartSendCommentAction action: {
                    break;
                }

                case SendCommentSuccessAction action: {
                    if (state.messageState.channelMessageList.ContainsKey(key: action.message.channelId)) {
                        var list = state.messageState.channelMessageList[key: action.message.channelId];
                        list.Insert(0, item: action.message.id);
                        state.messageState.channelMessageList[key: action.message.channelId] = list;
                    }
                    else {
                        state.messageState.channelMessageList.Add(key: action.message.channelId,
                            new List<string> {action.message.id});
                    }

                    if (state.messageState.channelMessageDict.ContainsKey(key: action.message.channelId)) {
                        var dict = state.messageState.channelMessageDict[key: action.message.channelId];
                        dict.Add(key: action.message.id, value: action.message);
                        state.messageState.channelMessageDict[key: action.message.channelId] = dict;
                    }
                    else {
                        state.messageState.channelMessageDict.Add(
                            key: action.message.channelId,
                            new Dictionary<string, Message> {
                                {action.message.id, action.message}
                            }
                        );
                    }

                    break;
                }

                case StartFetchEventOngoingAction _: {
                    state.eventState.eventsOngoingLoading = true;
                    break;
                }

                case StartFetchEventCompletedAction _: {
                    state.eventState.eventsCompletedLoading = true;
                    break;
                }

                case FetchEventsFailureAction action: {
                    if (action.pageNumber == 1) {
                        if (action.tab == "ongoing") {
                            state.eventState.eventsOngoingLoading = false;
                            state.eventState.pageNumber = action.pageNumber;
                        }
                        else {
                            state.eventState.eventsCompletedLoading = false;
                            state.eventState.completedPageNumber = action.pageNumber;
                        }
                    }

                    break;
                }

                case FetchEventsSuccessAction action: {
                    if (action.tab == "ongoing") {
                        state.eventState.eventsOngoingLoading = false;
                        state.eventState.pageNumber = action.pageNumber;
                        state.eventState.ongoingEventTotal = action.eventsResponse.events.total;
                    }
                    else {
                        state.eventState.eventsCompletedLoading = false;
                        state.eventState.completedPageNumber = action.pageNumber;
                        state.eventState.completedEventTotal = action.eventsResponse.events.total;
                    }

                    if (action.pageNumber == 1) {
                        if (action.tab == "ongoing") {
                            state.eventState.ongoingEvents.Clear();
                        }
                        else {
                            state.eventState.completedEvents.Clear();
                        }
                    }

                    action.eventsResponse.events.items.ForEach(eventObj => {
//                        if (eventObj.mode == "online") return;
                        if (action.tab == "ongoing") {
                            if (!state.eventState.ongoingEvents.Contains(item: eventObj.id)) {
                                state.eventState.ongoingEvents.Add(item: eventObj.id);
                            }
                        }
                        else {
                            if (!state.eventState.completedEvents.Contains(item: eventObj.id)) {
                                state.eventState.completedEvents.Add(item: eventObj.id);
                            }
                        }

                        if (state.eventState.eventsDict.ContainsKey(key: eventObj.id)) {
                            state.eventState.eventsDict[key: eventObj.id] = eventObj;
                        }
                        else {
                            state.eventState.eventsDict.Add(key: eventObj.id, value: eventObj);
                        }
                    });
                    break;
                }

                case StartFetchEventDetailAction _: {
                    state.eventState.eventDetailLoading = true;
                    break;
                }

                case FetchEventDetailSuccessAction action: {
                    state.eventState.eventDetailLoading = false;
                    state.eventState.channelId = action.eventObj.channelId;
                    if (state.eventState.eventsDict.ContainsKey(key: action.eventObj.id)) {
                        state.eventState.eventsDict[key: action.eventObj.id] = action.eventObj;
                    }
                    else {
                        state.eventState.eventsDict.Add(key: action.eventObj.id, value: action.eventObj);
                    }

                    break;
                }

                case FetchEventDetailFailedAction _: {
                    state.eventState.eventDetailLoading = false;
                    break;
                }

                case SaveEventHistoryAction action: {
                    var eventObj = action.eventObj;
                    var place = "";
                    if (eventObj.mode == "offline") {
                        var placeDict = state.placeState.placeDict;
                        if (eventObj.placeId != null && placeDict.ContainsKey(key: eventObj.placeId)) {
                            place = placeDict[key: eventObj.placeId].name;
                        }
                    }

                    eventObj.place = place;
                    if (eventObj.mode.isEmpty()) {
                        eventObj.mode = action.eventType == EventType.online ? "online" : "offline";
                    }

                    var eventHistoryList = HistoryManager.saveEventHistoryList(eventObj: eventObj,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.eventState.eventHistory = eventHistoryList;
                    break;
                }

                case DeleteEventHistoryAction action: {
                    var eventHistoryList = HistoryManager.deleteEventHistoryList(eventId: action.eventId,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.eventState.eventHistory = eventHistoryList;
                    break;
                }

                case DeleteAllEventHistoryAction action: {
                    state.eventState.eventHistory = new List<IEvent>();
                    HistoryManager.deleteAllEventHistory(
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    break;
                }

                case StartJoinEventAction _: {
                    state.eventState.joinEventLoading = true;
                    break;
                }

                case JoinEventSuccessAction action: {
                    state.eventState.joinEventLoading = false;
                    var eventObj = state.eventState.eventsDict[key: action.eventId];
                    eventObj.userIsCheckedIn = true;
                    state.eventState.eventsDict[key: action.eventId] = eventObj;
                    break;
                }

                case JoinEventFailureAction _: {
                    state.eventState.joinEventLoading = false;
                    break;
                }

                case StartFetchNotificationsAction _: {
                    state.notificationState.loading = true;
                    break;
                }

                case FetchNotificationsSuccessAction action: {
                    state.notificationState.pageTotal = action.pageTotal;
                    state.notificationState.notifications = action.notifications;
                    state.notificationState.mentions = action.mentions;
                    state.notificationState.loading = false;
                    break;
                }

                case FetchNotificationsFailureAction _: {
                    state.notificationState.loading = false;
                    break;
                }

                case StartReportItemAction _: {
                    state.reportState.loading = true;
                    break;
                }

                case ReportItemSuccessAction _: {
                    state.reportState.loading = false;
                    break;
                }

                case ReportItemFailureAction _: {
                    state.reportState.loading = false;
                    break;
                }

                case StartFetchMyFutureEventsAction _: {
                    state.mineState.futureListLoading = true;
                    break;
                }

                case FetchMyFutureEventsSuccessAction action: {
                    state.mineState.futureListLoading = false;
                    state.mineState.futureEventTotal = action.eventsResponse.events.total;
                    var offlineItems = action.eventsResponse.events.items.FindAll(item => item.mode != "online");
                    if (action.pageNumber == 1) {
                        state.mineState.futureEventsList = offlineItems;
                    }
                    else {
                        var results = state.mineState.futureEventsList;
                        results.AddRange(collection: offlineItems);
                        state.mineState.futureEventsList = results;
                    }

                    break;
                }

                case FetchMyFutureEventsFailureAction _: {
                    state.mineState.futureListLoading = false;
                    break;
                }

                case StartFetchMyPastEventsAction _: {
                    state.mineState.pastListLoading = true;
                    break;
                }

                case FetchMyPastEventsSuccessAction action: {
                    state.mineState.pastListLoading = false;
                    state.mineState.pastEventTotal = action.eventsResponse.events.total;
                    var offlineItems = action.eventsResponse.events.items;
                    if (action.pageNumber == 1) {
                        state.mineState.pastEventsList = offlineItems;
                    }
                    else {
                        var results = state.mineState.pastEventsList;
                        results.AddRange(collection: offlineItems);
                        state.mineState.pastEventsList = results;
                    }

                    break;
                }

                case FetchMyPastEventsFailureAction _: {
                    state.mineState.pastListLoading = false;
                    break;
                }

                case StartFetchMessagesAction _: {
                    state.messageState.messageLoading = true;
                    break;
                }

                case FetchMessagesSuccessAction action: {
                    if (action.messageIds != null && action.messageIds.Count > 0 &&
                        action.messageDict != null && action.messageDict.Count > 0) {
                        var channelMessageList = state.messageState.channelMessageList;
                        var channelMessageDict = state.messageState.channelMessageDict;
                        if (channelMessageList.ContainsKey(key: action.channelId)) {
                            channelMessageList[key: action.channelId] = action.messageIds;
                        }
                        else {
                            channelMessageList.Add(key: action.channelId, value: action.messageIds);
                        }

                        if (channelMessageDict.ContainsKey(key: action.channelId)) {
                            channelMessageDict[key: action.channelId] = action.messageDict;
                        }
                        else {
                            channelMessageDict.Add(key: action.channelId, value: action.messageDict);
                        }

                        state.messageState.channelMessageList = channelMessageList;
                        state.messageState.channelMessageDict = channelMessageDict;
                        state.messageState.hasMore = action.hasMore;
                        state.messageState.currOldestMessageId = action.currOldestMessageId;
                    }

                    state.messageState.messageLoading = false;
                    break;
                }

                case FetchMessagesFailureAction _: {
                    state.messageState.messageLoading = false;
                    break;
                }

                case StartSendMessageAction _: {
                    state.messageState.sendMessageLoading = true;
                    break;
                }

                case SendMessageSuccessAction action: {
                    var channelMessageList = state.messageState.channelMessageList;
                    var channelMessageDict = state.messageState.channelMessageDict;
                    var messageIds = new List<string>();
                    if (channelMessageList.ContainsKey(key: action.channelId)) {
                        messageIds = channelMessageList[key: action.channelId];
                    }

                    var messageDict = new Dictionary<string, Message>();
                    if (channelMessageDict.ContainsKey(key: action.channelId)) {
                        messageDict = channelMessageDict[key: action.channelId];
                    }

                    messageIds.Insert(0, item: action.nonce);
                    if (channelMessageList.ContainsKey(key: action.channelId)) {
                        channelMessageList[key: action.channelId] = messageIds;
                    }
                    else {
                        channelMessageList.Add(key: action.channelId, value: messageIds);
                    }

                    var previewMsg = new Message {
                        id = action.nonce,
                        content = action.content,
                        author = new User {
                            id = state.loginState.loginInfo.userId,
                            fullName = state.loginState.loginInfo.userFullName
                        }
                    };
                    _nonce.Add(item: action.nonce);
                    if (messageDict.ContainsKey(key: action.nonce)) {
                        messageDict[key: action.nonce] = previewMsg;
                    }
                    else {
                        messageDict.Add(key: action.nonce, value: previewMsg);
                    }

                    state.messageState.channelMessageList = channelMessageList;
                    state.messageState.channelMessageDict = channelMessageDict;
                    state.messageState.sendMessageLoading = false;
                    break;
                }

                case SendMessageFailureAction _: {
                    state.messageState.sendMessageLoading = false;
                    break;
                }

                case UserMapAction action: {
                    var userDict = state.userState.userDict;
                    foreach (var keyValuePair in action.userMap) {
                        if (userDict.ContainsKey(key: keyValuePair.Key)) {
                            userDict[key: keyValuePair.Key] = keyValuePair.Value;
                        }
                        else {
                            userDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                        }
                    }

                    state.userState.userDict = userDict;
                    break;
                }

                case TeamMapAction action: {
                    var teamDict = state.teamState.teamDict;
                    foreach (var keyValuePair in action.teamMap) {
                        if (teamDict.ContainsKey(key: keyValuePair.Key)) {
                            teamDict[key: keyValuePair.Key] = keyValuePair.Value;
                        }
                        else {
                            teamDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                        }
                    }

                    state.teamState.teamDict = teamDict;
                    break;
                }

                case PlaceMapAction action: {
                    var placeDict = state.placeState.placeDict;
                    foreach (var keyValuePair in action.placeMap) {
                        if (placeDict.ContainsKey(key: keyValuePair.Key)) {
                            placeDict[key: keyValuePair.Key] = keyValuePair.Value;
                        }
                        else {
                            placeDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                        }
                    }

                    state.placeState.placeDict = placeDict;
                    break;
                }

                case PopularSearchSuccessAction action: {
                    state.popularSearchState.popularSearchs = action.popularSearch;
                    break;
                }

                case StartSearchArticleAction _: {
                    state.searchState.loading = true;
                    break;
                }

                case SearchArticleSuccessAction action: {
                    state.searchState.loading = false;
                    state.searchState.keyword = action.keyword;
                    state.searchState.currentPage = action.searchResponse.currentPage;
                    state.searchState.pages = action.searchResponse.pages;
                    if (action.pageNumber == 0) {
                        state.searchState.searchArticles = action.searchResponse.projects;
                    }
                    else {
                        var searchArticles = state.searchState.searchArticles;
                        searchArticles.AddRange(collection: action.searchResponse.projects);
                        state.searchState.searchArticles = searchArticles;
                    }

                    break;
                }

                case SearchArticleFailureAction action: {
                    state.searchState.loading = false;
                    state.searchState.keyword = action.keyword;
                    break;
                }

                case ClearSearchArticleResultAction action: {
                    state.searchState.keyword = "";
                    state.searchState.searchArticles = new List<Article>();
                    break;
                }

                case SaveSearchHistoryAction action: {
                    var searchHistoryList = HistoryManager.saveSearchHistoryList(keyword: action.keyword,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.searchState.searchHistoryList = searchHistoryList;
                    break;
                }

                case DeleteSearchHistoryAction action: {
                    var searchHistoryList = HistoryManager.deleteSearchHistoryList(keyword: action.keyword,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.searchState.searchHistoryList = searchHistoryList;
                    break;
                }

                case DeleteAllSearchHistoryAction action: {
                    state.searchState.searchHistoryList = new List<string>();
                    HistoryManager.deleteAllSearchHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    break;
                }

                case MainNavigatorPushToArticleDetailAction action: {
                    if (action.articleId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new ArticleDetailScreenConnector(articleId: action.articleId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                ))
                        );
                    }

                    break;
                }

                case MainNavigatorPushToEventDetailAction action: {
                    if (action.eventId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) => {
                                if (action.eventType == EventType.offline) {
                                    return new EventOfflineDetailScreenConnector(eventId: action.eventId);
                                }

                                return new EventOnlineDetailScreenConnector(eventId: action.eventId);
                            },
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                ))
                        );
                    }

                    break;
                }

                case MainNavigatorPushToReportAction action: {
                    Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new ReportScreenConnector(reportId: action.reportId, reportType: action.reportType),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                        )
                    );

                    break;
                }

                case MainNavigatorPushToAction action: {
                    Router.navigator.pushNamed(routeName: action.routeName);
                    break;
                }
                
                case MainNavigatorReplaceToAction action: {
                    Router.navigator.pushReplacementNamed(routeName: action.routeName);
                    break;
                }

                case MainNavigatorPushToRouteAction action: {
                    Router.navigator.push(route: action.route);
                    break;
                }

                case MainNavigatorPopAction action: {
                    for (var i = 0; i < action.index; i++) {
                        if (Router.navigator.canPop()) {
                            Router.navigator.pop();
                        }
                    }

                    break;
                }

                case LoginNavigatorPushToBindUnityAction _: {
                    LoginScreen.navigator.push(new PageRouteBuilder(
                        pageBuilder: (context, animation, secondaryAnimation) =>
                            new BindUnityScreenConnector(fromPage: FromPage.login),
                        transitionsBuilder: (context1, animation, secondaryAnimation, child) => new PushPageTransition(
                            routeAnimation: animation,
                            child: child
                        ))
                    );
                    break;
                }

                case LoginNavigatorPushToAction action: {
                    LoginScreen.navigator.pushNamed(routeName: action.routeName);
                    break;
                }

                case LoginNavigatorPopAction action: {
                    for (var i = 0; i < action.index; i++) {
                        if (LoginScreen.navigator.canPop()) {
                            LoginScreen.navigator.pop();
                        }
                    }

                    break;
                }

                case OpenUrlAction action: {
                    if (action.url != null || action.url.Length > 0) {
                        Application.OpenURL(url: action.url);
                    }

                    break;
                }

                case CopyTextAction action: {
                    Clipboard.setData(new ClipboardData(text: action.text));
                    break;
                }

                case MainNavigatorPushToWebViewAction action: {
                    if (action.url != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new WebViewScreen(url: action.url),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                ))
                        );
                    }

                    break;
                }

                case PlayVideoAction action: {
                    if (action.url != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new VideoViewScreen(url: action.url),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                ))
                        );
                    }

                    break;
                }

                case FetchReviewUrlSuccessAction action: {
                    state.settingState.reviewUrl = action.url;
                    state.settingState.hasReviewUrl = action.url.isNotEmpty();
                    break;
                }

                case FetchReviewUrlFailureAction _: {
                    state.settingState.reviewUrl = "";
                    state.settingState.hasReviewUrl = false;
                    break;
                }

                case SettingClearCacheAction _: {
                    state.articleState.articleHistory = new List<Article>();
                    state.eventState.eventHistory = new List<IEvent>();
                    state.searchState.searchHistoryList = new List<string>();
                    HistoryManager.deleteAllArticleHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    HistoryManager.deleteAllEventHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    HistoryManager.deleteAllSearchHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    break;
                }
            }

            return state;
        }
    }
}