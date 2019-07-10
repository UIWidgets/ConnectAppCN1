using System.Collections.Generic;
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
                    state.searchState.searchArticleHistoryList =
                        HistoryManager.searchArticleHistoryList(userId: action.loginInfo.userId);
                    state.articleState.blockArticleList =
                        HistoryManager.blockArticleList(userId: action.loginInfo.userId);
                    EventBus.publish(sName: EventBusConstant.login_success, new List<object>());
                    break;
                }

                case LoginByEmailFailureAction _: {
                    state.loginState.loading = false;
                    break;
                }

                case LoginByWechatAction _: {
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
                    state.searchState.searchArticleHistoryList =
                        HistoryManager.searchArticleHistoryList(userId: action.loginInfo.userId);
                    state.articleState.blockArticleList =
                        HistoryManager.blockArticleList(userId: action.loginInfo.userId);
                    EventBus.publish(sName: EventBusConstant.login_success, new List<object>());
                    break;
                }

                case LoginByWechatFailureAction _: {
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
                    state.searchState.searchArticleHistoryList = HistoryManager.searchArticleHistoryList();
                    state.articleState.blockArticleList = HistoryManager.blockArticleList();
                    break;
                }

                case CleanEmailAndPasswordAction _: {
                    state.loginState.email = "";
                    state.loginState.password = "";
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
                            state.messageState.channelMessageList[key: keyValuePair.Key] = oldList;
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
                            var oldUser = userDict[key: keyValuePair.Key];
                            var newUser = keyValuePair.Value;
                            newUser.followingCount = oldUser.followingCount;
                            newUser.followings = oldUser.followings;
                            newUser.followingsHasMore = oldUser.followingsHasMore;
                            newUser.followers = oldUser.followers;
                            newUser.followersHasMore = oldUser.followersHasMore;
                            newUser.articles = oldUser.articles;
                            newUser.articlesHasMore = oldUser.articlesHasMore;
                            newUser.jobRoleMap = oldUser.jobRoleMap;
                            userDict[key: keyValuePair.Key] = newUser;
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

                case FollowMapAction action: {
                    var followDict = state.followState.followDict;
                    Dictionary<string, bool> followMap = followDict.ContainsKey(key: action.userId)
                        ? followDict[key: action.userId]
                        : new Dictionary<string, bool>();
                    foreach (var keyValuePair in action.followMap) {
                        if (!followMap.ContainsKey(key: keyValuePair.Key)) {
                            followMap.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                        }
                    }

                    if (action.userId.isNotEmpty() && followDict.ContainsKey(key: action.userId)) {
                        followDict[key: action.userId] = followMap;
                    }
                    else {
                        followDict.Add(action.userId, followMap);
                    }
                    state.followState.followDict = followDict;
                    break;
                }

                case PopularSearchArticleSuccessAction action: {
                    state.popularSearchState.popularSearchArticles = action.popularSearchArticles;
                    break;
                }
                
                case PopularSearchUserSuccessAction action: {
                    state.popularSearchState.popularSearchUsers = action.popularSearchUsers;
                    break;
                }

                case StartSearchArticleAction action: {
                    state.searchState.searchArticleLoading = true;
                    state.searchState.keyword = action.keyword;
                    break;
                }

                case SearchArticleSuccessAction action: {
                    state.searchState.searchArticleLoading = false;
                    state.searchState.keyword = action.keyword;
                    state.searchState.searchArticleCurrentPage = action.searchArticleResponse.currentPage;
                    state.searchState.searchArticlePages = action.searchArticleResponse.pages;
                    if (state.searchState.searchArticles.ContainsKey(key: action.keyword)) {
                        if (action.pageNumber == 0) {
                            state.searchState.searchArticles[key: action.keyword] = action.searchArticleResponse.projects;
                        }
                        else {
                            var searchArticles = state.searchState.searchArticles[key: action.keyword];
                            searchArticles.AddRange(collection: action.searchArticleResponse.projects);
                            state.searchState.searchArticles[key: action.keyword] = searchArticles;
                        }
                    }
                    else {
                        state.searchState.searchArticles.Add(action.keyword, action.searchArticleResponse.projects);
                    }

                    break;
                }

                case SearchArticleFailureAction action: {
                    state.searchState.searchArticleLoading = false;
                    state.searchState.keyword = action.keyword;
                    break;
                }

                case ClearSearchResultAction _: {
                    state.searchState.keyword = "";
                    state.searchState.searchArticles = new Dictionary<string, List<Article>>();
                    state.searchState.searchUsers = new Dictionary<string, List<User>>();
                    break;
                }

                case SaveSearchArticleHistoryAction action: {
                    var searchArticleHistoryList = HistoryManager.saveSearchArticleHistoryList(keyword: action.keyword,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.searchState.searchArticleHistoryList = searchArticleHistoryList;
                    break;
                }

                case DeleteSearchArticleHistoryAction action: {
                    var searchArticleHistoryList = HistoryManager.deleteSearchArticleHistoryList(keyword: action.keyword,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.searchState.searchArticleHistoryList = searchArticleHistoryList;
                    break;
                }

                case DeleteAllSearchArticleHistoryAction _: {
                    state.searchState.searchArticleHistoryList = new List<string>();
                    HistoryManager.deleteAllSearchArticleHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    break;
                }
                
                case StartSearchUserAction _: {
                    state.searchState.searchUserLoading = true;
                    break;
                }

                case SearchUserSuccessAction action: {
                    state.searchState.searchUserLoading = false;
                    state.searchState.keyword = action.keyword;
                    state.searchState.searchUserHasMore = action.hasMore;
                    if (state.searchState.searchArticles.ContainsKey(key: action.keyword)) {
                        if (action.pageNumber == 1) {
                            state.searchState.searchUsers[key: action.keyword] = action.users;
                        }
                        else {
                            var searchUsers = state.searchState.searchUsers[key: action.keyword];
                            searchUsers.AddRange(collection: action.users);
                            state.searchState.searchUsers[key: action.keyword] = searchUsers;
                        }
                    }
                    else {
                        state.searchState.searchUsers.Add(action.keyword, action.users);
                    }

                    break;
                }

                case SearchUserFailureAction action: {
                    state.searchState.searchUserLoading = false;
                    state.searchState.keyword = action.keyword;
                    break;
                }
                
                case StartSearchFollowingAction _: {
                    state.searchState.searchFollowingLoading = true;
                    break;
                }

                case SearchFollowingSuccessAction action: {
                    state.searchState.searchFollowingLoading = false;
                    state.searchState.searchFollowingKeyword = action.keyword;
                    state.searchState.searchFollowingHasMore = action.hasMore;
                    if (action.pageNumber == 0) {
                        state.searchState.searchFollowings = action.users;
                    }
                    else {
                        var searchUsers = state.searchState.searchFollowings;
                        searchUsers.AddRange(collection: action.users);
                        state.searchState.searchFollowings = searchUsers;
                    }

                    break;
                }

                case SearchFollowingFailureAction action: {
                    state.searchState.searchFollowingLoading = false;
                    state.searchState.searchFollowingKeyword = action.keyword;
                    break;
                }
                
                case ClearSearchFollowingResultAction _: {
                    state.searchState.searchFollowingKeyword = "";
                    state.searchState.searchFollowings = new List<User>();
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
                                )
                            )
                        );
                    }

                    break;
                }

                case MainNavigatorPushToUserDetailAction action: {
                    if (action.userId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new UserDetailScreenConnector(userId: action.userId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                            )
                        );
                    }

                    break;
                }

                case MainNavigatorPushToUserFollowingAction action: {
                    if (action.userId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new UserFollowingScreenConnector(userId: action.userId), 
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                            )
                        );
                    }

                    break;
                }

                case MainNavigatorPushToUserFollowerAction action: {
                    if (action.userId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new UserFollowerScreenConnector(userId: action.userId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                            )
                        );
                    }

                    break;
                }

                case MainNavigatorPushToEditPersonalInfoAction action: {
                    if (action.userId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new EditPersonalInfoScreenConnector(personalId: action.userId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                            )
                        );
                    }

                    break;
                }

                case MainNavigatorPushToTeamDetailAction action: {
                    if (action.teamId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new TeamDetailScreenConnector(teamId: action.teamId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                            )
                        );
                    }

                    break;
                }

                case MainNavigatorPushToTeamFollowerAction action: {
                    if (action.teamId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new TeamFollowerScreenConnector(teamId: action.teamId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                            )
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
                                )
                            )
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
                    state.searchState.searchArticleHistoryList = new List<string>();
                    HistoryManager.deleteAllArticleHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    HistoryManager.deleteAllEventHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    HistoryManager.deleteAllSearchArticleHistory(state.loginState.isLoggedIn
                        ? state.loginState.loginInfo.userId
                        : null);
                    break;
                }

                case StartFetchUserProfileAction _: {
                    state.userState.userLoading = true;
                    break;
                } 

                case FetchUserProfileSuccessAction action: {
                    state.userState.userLoading = false;
                    if (!state.userState.userDict.ContainsKey(key: action.userId)) {
                        state.userState.userDict.Add(key: action.userId, value: action.user);
                    }
                    else {
                        state.userState.userDict[key: action.userId] = action.user;
                    }
                    break;
                } 

                case FetchUserProfileFailureAction _: {
                    state.userState.userLoading = false;
                    break;
                } 

                case StartFetchUserArticleAction _: {
                    state.userState.userArticleLoading = true;
                    break;
                } 

                case FetchUserArticleSuccessAction action: {
                    state.userState.userArticleLoading = false;
                    if (state.userState.userDict.ContainsKey(key: action.userId)) {
                        var user = state.userState.userDict[key: action.userId];
                        Debug.Log(user.followCount);
                        Debug.Log(user.followingCount);
                        Debug.Log(user.id);
                        user.articlesHasMore = action.hasMore;
                        if (action.offset == 0) {
                            user.articles = action.articles;
                        }
                        else {
                            var articles = user.articles;
                            articles.AddRange(collection: action.articles);
                            user.articles = articles;
                        }
                        Debug.Log(user.followCount);
                        Debug.Log(user.followingCount);
                        Debug.Log(user.id);
                        state.userState.userDict[key: action.userId] = user;
                    }
                    else {
                        var user = new User {
                            articlesHasMore = action.hasMore
                        };
                        if (action.offset == 0) {
                            user.articles = action.articles;
                        }
                        else {
                            var articles = user.articles;
                            articles.AddRange(collection: action.articles);
                            user.articles = articles;
                        }
                        state.userState.userDict.Add(key: action.userId, value: user);
                    }
                    break;
                }

                case FetchUserArticleFailureAction _: {
                    state.userState.userArticleLoading = false;
                    break;
                } 

                case StartFetchFollowUserAction action: {
                    state.userState.followUserLoading = true;
                    state.userState.currentFollowId = action.followUserId;
                    break;
                }

                case FetchFollowUserSuccessAction action: {
                    state.userState.followUserLoading = false;
                    if (state.followState.followDict.ContainsKey(key: action.currentUserId)) {
                        var followMap = state.followState.followDict[key: action.currentUserId];
                        if (!followMap.ContainsKey(key: action.followUserId)) {
                            followMap.Add(key: action.followUserId, value: action.success);
                        }
                        state.followState.followDict[key: action.currentUserId] = followMap;
                    }
                    if (state.userState.userDict.ContainsKey(key: action.currentUserId)) {
                        var user = state.userState.userDict[key: action.currentUserId];
                        user.followingCount += 1;
                        state.userState.userDict[key: action.currentUserId] = user;
                    }
                    if (state.userState.userDict.ContainsKey(key: action.followUserId)) {
                        var user = state.userState.userDict[key: action.followUserId];
                        user.followCount += 1;
                        state.userState.userDict[key: action.followUserId] = user;
                    }
                    break;
                } 

                case FetchFollowUserFailureAction _: {
                    state.userState.followUserLoading = false;
                    break;
                }

                case StartFetchUnFollowUserAction action: {
                    state.userState.followUserLoading = true;
                    state.userState.currentFollowId = action.unFollowUserId;
                    break;
                } 

                case FetchUnFollowUserSuccessAction action: {
                    state.userState.followUserLoading = false;
                    if (state.followState.followDict.ContainsKey(key: action.currentUserId)) {
                        var followMap = state.followState.followDict[key: action.currentUserId];
                        if (followMap.ContainsKey(key: action.unFollowUserId)) {
                            followMap.Remove(key: action.unFollowUserId);
                        }
                        state.followState.followDict[key: action.currentUserId] = followMap;
                    }
                    if (state.userState.userDict.ContainsKey(key: action.currentUserId)) {
                        var user = state.userState.userDict[key: action.currentUserId];
                        user.followingCount -= 1;
                        state.userState.userDict[key: action.currentUserId] = user;
                    }
                    if (state.userState.userDict.ContainsKey(key: action.unFollowUserId)) {
                        var user = state.userState.userDict[key: action.unFollowUserId];
                        user.followCount -= 1;
                        state.userState.userDict[key: action.unFollowUserId] = user;
                    }
                    break;
                } 

                case FetchUnFollowUserFailureAction _: {
                    state.userState.followUserLoading = false;
                    break;
                }

                case StartFetchFollowingAction _: {
                    state.userState.followingLoading = true;
                    break;
                }

                case FetchFollowingSuccessAction action: {
                    state.userState.followingLoading = false;
                    if (state.userState.userDict.ContainsKey(key: action.userId)) {
                        var user = state.userState.userDict[key: action.userId];
                        user.followingsHasMore = action.followingsHasMore;
                        if (action.offset == 0) {
                            user.followings = action.followings;
                        }
                        else {
                            var followings = user.followings;
                            followings.AddRange(collection: action.followings);
                            user.followings = followings;
                        }
                        state.userState.userDict[key: action.userId] = user;
                    }
                    break;
                }

                case FetchFollowingFailureAction _: {
                    state.userState.followingLoading = false;
                    break;
                }

                case StartFetchFollowerAction _: {
                    state.userState.followerLoading = true;
                    break;
                } 

                case FetchFollowerSuccessAction action: {
                    state.userState.followerLoading = false;
                    if (state.userState.userDict.ContainsKey(key: action.userId)) {
                        var user = state.userState.userDict[key: action.userId];
                        user.followersHasMore = action.followersHasMore;
                        if (action.offset == 0) {
                            user.followers = action.followers;
                        }
                        else {
                            var followers = user.followers;
                            followers.AddRange(collection: action.followers);
                            user.followers = followers;
                        }
                        state.userState.userDict[key: action.userId] = user;
                    }
                    break;
                } 

                case FetchFollowerFailureAction _: {
                    state.userState.followerLoading = false;
                    break;
                }

                case ChangePersonalFullNameAction action: {
                    state.userState.fullName = action.fullName;
                    break;
                }

                case ChangePersonalTitleAction action: {
                    state.userState.title = action.title;
                    break;
                }

                case ChangePersonalRoleAction action: {
                    state.userState.jobRole = action.jobRole;
                    break;
                }

                case CleanPersonalInfoAction _: {
                    state.userState.fullName = "";
                    state.userState.title = "";
                    state.userState.jobRole = new JobRole();
                    break;
                }

                case EditPersonalInfoSuccessAction action: {
                    if (state.userState.userDict.ContainsKey(key: action.user.id)) {
                        var oldUser = state.userState.userDict[key: action.user.id];
                        var newUser = action.user;
                        newUser.followingCount = oldUser.followingCount;
                        newUser.followings = oldUser.followings;
                        newUser.followingsHasMore = oldUser.followingsHasMore;
                        newUser.followers = oldUser.followers;
                        newUser.followersHasMore = oldUser.followersHasMore;
                        newUser.articles = oldUser.articles;
                        newUser.articlesHasMore = oldUser.articlesHasMore;
                        state.userState.userDict[key: action.user.id] = newUser;
                    }
                    break;
                }

                case StartFetchTeamAction _: {
                    state.teamState.teamLoading = true;
                    break;
                } 

                case FetchTeamSuccessAction action: {
                    state.teamState.teamLoading = false;
                    if (!state.teamState.teamDict.ContainsKey(key: action.teamId)) {
                        state.teamState.teamDict.Add(key: action.teamId, value: action.team);
                    }
                    else {
                        state.teamState.teamDict[key: action.teamId] = action.team;
                    }
                    break;
                } 

                case FetchTeamFailureAction _: {
                    state.teamState.teamLoading = false;
                    break;
                } 

                case StartFetchTeamArticleAction _: {
                    state.teamState.teamArticleLoading = true;
                    break;
                } 

                case FetchTeamArticleSuccessAction action: {
                    state.teamState.teamArticleLoading = false;
                    state.teamState.teamArticleHasMore = action.hasMore;
                    if (state.teamState.teamArticleDict.ContainsKey(key: action.teamId)) {
                        if (action.offset == 0) {
                            state.teamState.teamArticleDict[key: action.teamId] = action.articles;
                        }
                        else {
                            var teamArticles = state.teamState.teamArticleDict[key: action.teamId];
                            teamArticles.AddRange(collection: action.articles);
                            state.teamState.teamArticleDict[key: action.teamId] = teamArticles;
                        }
                    }
                    else {
                        if (action.offset == 0) {
                            state.teamState.teamArticleDict.Add(key: action.teamId, value: action.articles);
                        }
                    }
                    break;
                } 

                case FetchTeamArticleFailureAction _: {
                    state.teamState.teamArticleLoading = false;
                    break;
                }

                case StartFetchTeamFollowerAction _: {
                    state.teamState.teamFollowerLoading = true;
                    break;
                }

                case FetchTeamFollowerSuccessAction action: {
                    state.teamState.teamFollowerLoading = false;
                    state.teamState.teamFollowerHasMore = action.followersHasMore;
                    if (state.teamState.teamFollowerDict.ContainsKey(key: action.teamId)) {
                        if (action.offset == 0) {
                            state.teamState.teamFollowerDict[key: action.teamId] = action.followers;
                        }
                        else {
                            var teamFollowers = state.teamState.teamFollowerDict[key: action.teamId];
                            teamFollowers.AddRange(collection: action.followers);
                            state.teamState.teamFollowerDict[key: action.teamId] = teamFollowers;
                        }
                    }
                    else {
                        if (action.offset == 0) {
                            state.teamState.teamFollowerDict.Add(key: action.teamId, value: action.followers);
                        }
                    }
                    break;
                }

                case FetchTeamFollowerFailureAction _: {
                    state.teamState.teamFollowerLoading = false;
                    break;
                }
            }

            return state;
        }
    }
}