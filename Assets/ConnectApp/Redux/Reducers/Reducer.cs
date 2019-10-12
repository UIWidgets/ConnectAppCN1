using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Main;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.redux.actions;
using ConnectApp.Reality;
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
                    state.articleState.feedHasNew = true;
                    state.articleState.articleHistory =
                        HistoryManager.articleHistoryList(userId: action.loginInfo.userId);
                    state.eventState.eventHistory = HistoryManager.eventHistoryList(userId: action.loginInfo.userId);
                    state.searchState.searchArticleHistoryList =
                        HistoryManager.searchArticleHistoryList(userId: action.loginInfo.userId);
                    state.articleState.blockArticleList =
                        HistoryManager.blockArticleList(userId: action.loginInfo.userId);
                    break;
                }

                case LoginByEmailFailureAction _: {
                    state.loginState.loading = false;
                    break;
                }

                case LoginByWechatSuccessAction action: {
                    state.loginState.loading = false;
                    state.loginState.loginInfo = action.loginInfo;
                    state.loginState.isLoggedIn = true;
                    state.articleState.feedHasNew = true;
                    state.articleState.articleHistory =
                        HistoryManager.articleHistoryList(userId: action.loginInfo.userId);
                    state.eventState.eventHistory = HistoryManager.eventHistoryList(userId: action.loginInfo.userId);
                    state.searchState.searchArticleHistoryList =
                        HistoryManager.searchArticleHistoryList(userId: action.loginInfo.userId);
                    state.articleState.blockArticleList =
                        HistoryManager.blockArticleList(userId: action.loginInfo.userId);
                    break;
                }

                case LoginByWechatFailureAction _: {
                    state.loginState.loading = false;
                    break;
                }

                case LogoutAction _: {
                    EventBus.publish(sName: EventBusConstant.logout_success, new List<object>());
                    HistoryManager.deleteHomeAfterTime(state.loginState.loginInfo.userId);
                    HttpManager.clearCookie();
                    state.loginState.loginInfo = new LoginInfo();
                    state.loginState.isLoggedIn = false;
                    UserInfoManager.clearUserInfo();
                    state.articleState.articleHistory = HistoryManager.articleHistoryList();
                    state.eventState.eventHistory = HistoryManager.eventHistoryList();
                    state.searchState.searchArticleHistoryList = HistoryManager.searchArticleHistoryList();
                    state.articleState.blockArticleList = HistoryManager.blockArticleList();
                    state.favoriteState.favoriteTagIds = new List<string>();
                    state.favoriteState.favoriteTagDict = new Dictionary<string, FavoriteTag>();
                    state.favoriteState.favoriteDetailArticleIdDict = new Dictionary<string, List<string>>();
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
                        state.articleState.recommendArticleIds.Clear();
                    }

                    foreach (var article in action.articleList) {
                        state.articleState.recommendArticleIds.Add(item: article.id);
                        if (!state.articleState.articleDict.ContainsKey(key: article.id)) {
                            state.articleState.articleDict.Add(key: article.id, value: article);
                        }
                        else {
                            var oldArticle = state.articleState.articleDict[key: article.id];
                            state.articleState.articleDict[key: article.id] = oldArticle.Merge(other: article);
                        }
                    }

                    state.articleState.feedHasNew = action.feedHasNew;
                    state.articleState.hottestHasMore = action.hottestHasMore;
                    state.articleState.articlesLoading = false;
                    break;
                }

                case FetchArticleFailureAction _: {
                    state.articleState.articlesLoading = false;
                    break;
                }

                case StartFetchFollowArticlesAction _: {
                    state.articleState.followArticlesLoading = true;
                    break;
                }

                case FetchFollowArticleSuccessAction action: {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    if (currentUserId.isNotEmpty()) {
                        if (action.feeds != null && action.feeds.Count > 0) {
                            var followArticleIds = new List<string>();
                            action.feeds.ForEach(feed => {
                                if (feed.itemIds != null && feed.itemIds.Count > 0) {
                                    followArticleIds.Add(feed.itemIds[0]);
                                }
                            });
                            if (state.articleState.followArticleIdDict.ContainsKey(key: currentUserId)) {
                                if (action.pageNumber == 1) {
                                    state.articleState.beforeTime = action.feeds.last().actionTime;
                                    state.articleState.afterTime = action.feeds.first().actionTime;
                                    if (state.loginState.isLoggedIn) {
                                        HistoryManager.saveHomeAfterTime(afterTime: state.articleState.afterTime,
                                            userId: state.loginState.loginInfo.userId);
                                    }

                                    state.articleState.followArticleIdDict[key: currentUserId] = followArticleIds;
                                }
                                else {
                                    state.articleState.beforeTime = action.feeds.last().actionTime;
                                    var projectIds = state.articleState.followArticleIdDict[key: currentUserId];
                                    projectIds.AddRange(collection: followArticleIds);
                                    state.articleState.followArticleIdDict[key: currentUserId] = projectIds;
                                }
                            }
                            else {
                                state.articleState.beforeTime = action.feeds.last().actionTime;
                                state.articleState.afterTime = action.feeds.first().actionTime;
                                if (state.loginState.isLoggedIn) {
                                    HistoryManager.saveHomeAfterTime(afterTime: state.articleState.afterTime,
                                        userId: state.loginState.loginInfo.userId);
                                }

                                state.articleState.followArticleIdDict.Add(key: currentUserId, value: followArticleIds);
                            }
                        }

                        var hotArticleIds = new List<string>();
                        foreach (var hotItem in action.hotItems) {
                            hotArticleIds.Add(item: hotItem.itemId);
                        }

                        if (state.articleState.hotArticleIdDict.ContainsKey(key: currentUserId)) {
                            if (action.pageNumber == 1) {
                                state.articleState.hotArticleIdDict[key: currentUserId] = hotArticleIds;
                            }
                            else {
                                var hotIds = state.articleState.hotArticleIdDict[key: currentUserId];
                                hotIds.AddRange(collection: hotArticleIds);
                                state.articleState.hotArticleIdDict[key: currentUserId] = hotIds;
                            }
                        }
                        else {
                            state.articleState.hotArticleIdDict.Add(key: currentUserId, value: hotArticleIds);
                        }

                        state.articleState.feedHasNew = action.feedHasNew;
                        state.articleState.feedIsFirst = action.feedIsFirst;
                        state.articleState.followArticleHasMore = action.feedHasMore;
                        state.articleState.hotArticleHasMore = action.hotHasMore;
                        state.articleState.hotArticlePage = action.hotPage;
                    }

                    state.articleState.followArticlesLoading = false;
                    break;
                }

                case FetchFollowArticleFailureAction _: {
                    state.articleState.followArticlesLoading = false;
                    break;
                }

                case StartFetchArticleDetailAction _: {
                    state.articleState.articleDetailLoading = true;
                    break;
                }

                case FetchArticleDetailSuccessAction action: {
                    state.articleState.articleDetailLoading = false;
                    var relatedArticles = action.articleDetail.projects.FindAll(item => item.type == "article");
                    var projectIds = new List<string>();
                    relatedArticles.ForEach(project => {
                        projectIds.Add(item: project.id);
                        if (!state.articleState.articleDict.ContainsKey(key: project.id)) {
                            state.articleState.articleDict.Add(key: project.id, value: project);
                        }
                        else {
                            var oldArticle = state.articleState.articleDict[key: project.id];
                            state.articleState.articleDict[key: project.id] = oldArticle.Merge(other: project);
                        }
                    });
                    var article = action.articleDetail.projectData;
                    article.like = action.articleDetail.like;
                    article.projectIds = projectIds;
                    article.channelId = action.articleDetail.channelId;
                    article.contentMap = action.articleDetail.contentMap;
                    article.hasMore = action.articleDetail.comments.hasMore;
                    article.favorite = action.articleDetail.favorite;
                    article.isNotFirst = true;
                    article.currOldestMessageId = action.articleDetail.comments.currOldestMessageId;
                    article.videoSliceMap = action.articleDetail.videoSliceMap;
                    article.videoPosterMap = action.articleDetail.videoPosterMap;
                    var dict = state.articleState.articleDict;
                    if (dict.ContainsKey(key: article.id)) {
                        state.articleState.articleDict[key: article.id] = article;
                    }
                    else {
                        state.articleState.articleDict.Add(key: article.id, value: article);
                    }

                    if (!article.id.Equals(value: action.articleId)) {
                        if (dict.ContainsKey(key: action.articleId)) {
                            state.articleState.articleDict[key: action.articleId] = article;
                        }
                        else {
                            state.articleState.articleDict.Add(key: action.articleId, value: article);
                        }
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
                        var article = state.articleState.articleDict[key: action.articleId];
                        article.like = true;
                        article.likeCount += 1;
                        state.articleState.articleDict[key: action.articleId] = article;
                    }

                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    if (currentUserId.isNotEmpty() && state.likeState.likeDict.ContainsKey(key: currentUserId)) {
                        var likeMap = state.likeState.likeDict[key: currentUserId];
                        if (!likeMap.ContainsKey(key: action.articleId)) {
                            likeMap.Add(key: action.articleId, true);
                        }

                        state.likeState.likeDict[key: currentUserId] = likeMap;
                    }

                    break;
                }

                case FavoriteArticleSuccessAction action: {
                    if (state.articleState.articleDict.ContainsKey(key: action.articleId)) {
                        var article = state.articleState.articleDict[key: action.articleId];
                        article.favorite = action.favorite;
                        state.articleState.articleDict[key: action.articleId] = article;
                    }

                    if (state.favoriteState.favoriteTagDict.ContainsKey(key: action.favorite.tagId)) {
                        var favoriteTag = state.favoriteState.favoriteTagDict[key: action.favorite.tagId];
                        var statistics = favoriteTag.stasitics ?? new Statistics {count = 0};
                        statistics.count += 1;
                        favoriteTag.stasitics = statistics;
                        state.favoriteState.favoriteTagDict[key: action.favorite.tagId] = favoriteTag;
                    }

                    if (state.favoriteState.favoriteDetailArticleIdDict.ContainsKey(key: action.favorite.tagId)) {
                        var favoriteDetailArticleIds =
                            state.favoriteState.favoriteDetailArticleIdDict[key: action.favorite.tagId];
                        favoriteDetailArticleIds.Insert(0, item: action.articleId);
                        state.favoriteState.favoriteDetailArticleIdDict[key: action.favorite.tagId] =
                            favoriteDetailArticleIds;
                    }

                    break;
                }

                case UnFavoriteArticleSuccessAction action: {
                    if (state.articleState.articleDict.ContainsKey(key: action.articleId)) {
                        var article = state.articleState.articleDict[key: action.articleId];
                        article.favorite = null;
                        state.articleState.articleDict[key: action.articleId] = article;
                    }

                    if (state.favoriteState.favoriteTagDict.ContainsKey(key: action.favorite.tagId)) {
                        var favoriteTag = state.favoriteState.favoriteTagDict[key: action.favorite.tagId];
                        var statistics = favoriteTag.stasitics ?? new Statistics {count = 1};
                        statistics.count -= 1;
                        favoriteTag.stasitics = statistics;
                        state.favoriteState.favoriteTagDict[key: action.favorite.tagId] = favoriteTag;
                    }

                    if (state.favoriteState.favoriteDetailArticleIdDict.ContainsKey(key: action.favorite.tagId)) {
                        var favoriteDetailArticleIds =
                            state.favoriteState.favoriteDetailArticleIdDict[key: action.favorite.tagId];
                        favoriteDetailArticleIds.Remove(item: action.articleId);
                        state.favoriteState.favoriteDetailArticleIdDict[key: action.favorite.tagId] =
                            favoriteDetailArticleIds;
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
                    var user = new User {id = state.loginState.loginInfo.userId};
                    var reaction = new Reaction {user = user};
                    action.message.reactions.Add(item: reaction);
                    state.messageState.channelMessageDict[key: action.message.channelId][key: action.message.id] =
                        action.message;
                    break;
                }

                case LikeCommentFailureAction _: {
                    break;
                }

                case StartRemoveLikeCommentAction _: {
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

                case StartSendCommentAction _: {
                    break;
                }

                case SendCommentSuccessAction action: {
                    if (action.message.deleted) {
                        break;
                    }

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

                    if (state.articleState.articleDict.ContainsKey(key: action.articleId)) {
                        var article = state.articleState.articleDict[key: action.articleId];
                        article.commentCount += 1;
                        state.articleState.articleDict[key: action.articleId] = article;
                    }

                    if (state.messageState.channelMessageDict.ContainsKey(key: action.channelId)) {
                        var messageDict = state.messageState.channelMessageDict[key: action.channelId];
                        if (action.upperMessageId.isNotEmpty()) {
                            if (messageDict.ContainsKey(key: action.upperMessageId)) {
                                var message = messageDict[key: action.upperMessageId];
                                (message.lowerMessageIds ?? new List<string>()).Add(item: action.message.id);
                                messageDict[key: action.upperMessageId] = message;
                            }
                        }
                        else {
                            if (messageDict.ContainsKey(key: action.parentMessageId)) {
                                var message = messageDict[key: action.parentMessageId];
                                (message.replyMessageIds ?? new List<string>()).Add(item: action.message.id);
                                messageDict[key: action.parentMessageId] = message;
                            }
                        }

                        state.messageState.channelMessageDict[key: action.channelId] = messageDict;
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
                            var oldEventObj = state.eventState.eventsDict[key: eventObj.id];
                            state.eventState.eventsDict[key: eventObj.id] = oldEventObj.Merge(eventObj);
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
                        var oldEvent = state.eventState.eventsDict[key: action.eventObj.id];
                        state.eventState.eventsDict[key: action.eventObj.id] = oldEvent.Merge(action.eventObj);
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
                    state.notificationState.page = action.page;
                    state.notificationState.pageTotal = action.pageTotal;
                    if (action.pageNumber == 1) {
                        state.notificationState.notifications = action.notifications;
                        state.notificationState.mentions = action.mentions;
                    }
                    else {
                        var notifications = state.notificationState.notifications;
                        var mentions = state.notificationState.mentions;
                        if (action.pageNumber <= action.pageTotal) {
                            notifications.AddRange(collection: action.notifications);
                        }

                        foreach (var user in action.mentions) {
                            if (!mentions.Contains(item: user)) {
                                mentions.Add(item: user);
                            }
                        }

                        state.notificationState.notifications = notifications;
                        state.notificationState.mentions = mentions;
                    }

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
                    var items = action.eventsResponse.events.items;
                    if (action.pageNumber == 1) {
                        state.mineState.futureEventsList = items;
                    }
                    else {
                        if (state.mineState.futureEventsList.Count < action.eventsResponse.events.total) {
                            var results = state.mineState.futureEventsList;
                            results.AddRange(collection: items);
                            state.mineState.futureEventsList = results;
                        }
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
                        if (state.mineState.pastEventsList.Count < action.eventsResponse.events.total) {
                            var results = state.mineState.pastEventsList;
                            results.AddRange(collection: offlineItems);
                            state.mineState.pastEventsList = results;
                        }
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

                case ArticleMapAction action: {
                    if (action.articleMap != null && action.articleMap.isNotEmpty()) {
                        var articleDict = state.articleState.articleDict;
                        foreach (var keyValuePair in action.articleMap) {
                            if (articleDict.ContainsKey(key: keyValuePair.Key)) {
                                var oldArticle = articleDict[key: keyValuePair.Key];
                                articleDict[key: keyValuePair.Key] = oldArticle.Merge(other: keyValuePair.Value);
                            }
                            else {
                                articleDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                            }
                        }

                        state.articleState.articleDict = articleDict;
                    }

                    break;
                }

                case UserMapAction action: {
                    if (action.userMap != null && action.userMap.isNotEmpty()) {
                        var userDict = state.userState.userDict;
                        foreach (var keyValuePair in action.userMap) {
                            if (userDict.ContainsKey(key: keyValuePair.Key)) {
                                var oldUser = userDict[key: keyValuePair.Key];
                                userDict[key: keyValuePair.Key] = oldUser.Merge(other: keyValuePair.Value);
                            }
                            else {
                                userDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                            }
                        }

                        state.userState.userDict = userDict;
                    }

                    break;
                }

                case UserLicenseMapAction action: {
                    if (action.userLicenseMap != null && action.userLicenseMap.isNotEmpty()) {
                        var userLicenseDict = state.userState.userLicenseDict;
                        foreach (var keyValuePair in action.userLicenseMap) {
                            if (userLicenseDict.ContainsKey(key: keyValuePair.Key)) {
                                userLicenseDict[key: keyValuePair.Key] = keyValuePair.Value;
                            }
                            else {
                                userLicenseDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                            }
                        }

                        state.userState.userLicenseDict = userLicenseDict;
                    }

                    break;
                }

                case TeamMapAction action: {
                    if (action.teamMap != null && action.teamMap.isNotEmpty()) {
                        var teamDict = state.teamState.teamDict;
                        foreach (var keyValuePair in action.teamMap) {
                            if (teamDict.ContainsKey(key: keyValuePair.Key)) {
                                var oldTeam = teamDict[key: keyValuePair.Key];
                                if (oldTeam.isDetail ?? false) {
                                    var newTeam = oldTeam.Merge(other: keyValuePair.Value);
                                    var stats = newTeam.stats ?? new TeamStats();
                                    teamDict[key: keyValuePair.Key] = newTeam.copyWith(
                                        stats: stats.copyWith(membersCount: oldTeam.stats?.membersCount)
                                    );
                                }
                                else {
                                    var newTeam = oldTeam.Merge(other: keyValuePair.Value);
                                    var stats = newTeam.stats ?? new TeamStats();
                                    teamDict[key: keyValuePair.Key] = newTeam.copyWith(
                                        stats: stats.copyWith(membersCount: 0)
                                    );
                                }
                            }
                            else {
                                teamDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                            }
                        }

                        state.teamState.teamDict = teamDict;
                    }

                    break;
                }

                case PlaceMapAction action: {
                    if (action.placeMap != null && action.placeMap.isNotEmpty()) {
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
                    }

                    break;
                }

                case FollowMapAction action: {
                    if (action.followMap != null && action.followMap.isNotEmpty()) {
                        var userId = state.loginState.loginInfo.userId ?? "";
                        if (userId.isNotEmpty()) {
                            var followDict = state.followState.followDict;
                            Dictionary<string, bool> followMap = followDict.ContainsKey(key: userId)
                                ? followDict[key: userId]
                                : new Dictionary<string, bool>();
                            foreach (var keyValuePair in action.followMap) {
                                if (!followMap.ContainsKey(key: keyValuePair.Key)) {
                                    followMap.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                                }
                            }

                            if (followDict.ContainsKey(key: userId)) {
                                followDict[key: userId] = followMap;
                            }
                            else {
                                followDict.Add(key: userId, value: followMap);
                            }

                            state.followState.followDict = followDict;
                        }
                    }

                    break;
                }

                case LikeMapAction action: {
                    if (action.likeMap != null && action.likeMap.isNotEmpty()) {
                        var userId = state.loginState.loginInfo.userId ?? "";
                        if (userId.isNotEmpty()) {
                            var likeDict = state.likeState.likeDict;
                            Dictionary<string, bool> likeMap = likeDict.ContainsKey(key: userId)
                                ? likeDict[key: userId]
                                : new Dictionary<string, bool>();
                            foreach (var keyValuePair in action.likeMap) {
                                if (!likeMap.ContainsKey(key: keyValuePair.Key)) {
                                    likeMap.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                                }
                            }

                            if (likeDict.ContainsKey(key: userId)) {
                                likeDict[key: userId] = likeMap;
                            }
                            else {
                                likeDict.Add(key: userId, value: likeMap);
                            }

                            state.likeState.likeDict = likeDict;
                        }
                    }

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
                    state.searchState.searchArticleCurrentPage = action.currentPage;
                    state.searchState.searchArticlePages = action.pages;
                    var articleIds = new List<string>();
                    (action.searchArticles ?? new List<Article>()).ForEach(searchArticle => {
                        articleIds.Add(item: searchArticle.id);
                        if (!state.articleState.articleDict.ContainsKey(key: searchArticle.id)) {
                            state.articleState.articleDict.Add(key: searchArticle.id, value: searchArticle);
                        }
                        else {
                            var oldArticle = state.articleState.articleDict[key: searchArticle.id];
                            state.articleState.articleDict[key: searchArticle.id] =
                                oldArticle.Merge(other: searchArticle);
                        }
                    });

                    if (state.searchState.searchArticleIdDict.ContainsKey(key: action.keyword)) {
                        if (action.pageNumber == 0) {
                            state.searchState.searchArticleIdDict[key: action.keyword] = articleIds;
                        }
                        else {
                            var searchArticleIds = state.searchState.searchArticleIdDict[key: action.keyword];
                            searchArticleIds.AddRange(collection: articleIds);
                            state.searchState.searchArticleIdDict[key: action.keyword] = searchArticleIds;
                        }
                    }
                    else {
                        state.searchState.searchArticleIdDict.Add(key: action.keyword, value: articleIds);
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
                    state.searchState.searchArticleIdDict = new Dictionary<string, List<string>>();
                    state.searchState.searchUserIdDict = new Dictionary<string, List<string>>();
                    state.searchState.searchTeamIdDict = new Dictionary<string, List<string>>();
                    break;
                }

                case SaveSearchArticleHistoryAction action: {
                    var searchArticleHistoryList = HistoryManager.saveSearchArticleHistoryList(keyword: action.keyword,
                        state.loginState.isLoggedIn ? state.loginState.loginInfo.userId : null);
                    state.searchState.searchArticleHistoryList = searchArticleHistoryList;
                    break;
                }

                case DeleteSearchArticleHistoryAction action: {
                    var searchArticleHistoryList = HistoryManager.deleteSearchArticleHistoryList(
                        keyword: action.keyword,
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
                    if (state.searchState.searchUserIdDict.ContainsKey(key: action.keyword)) {
                        if (action.pageNumber == 1) {
                            state.searchState.searchUserIdDict[key: action.keyword] = action.searchUserIds;
                        }
                        else {
                            var searchUserIds = state.searchState.searchUserIdDict[key: action.keyword] ??
                                                new List<string>();
                            searchUserIds.AddRange(collection: action.searchUserIds);
                            state.searchState.searchUserIdDict[key: action.keyword] = searchUserIds;
                        }
                    }
                    else {
                        state.searchState.searchUserIdDict.Add(key: action.keyword, value: action.searchUserIds);
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
                    if (action.pageNumber == 1) {
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

                case StartSearchTeamAction _: {
                    state.searchState.searchTeamLoading = true;
                    break;
                }

                case SearchTeamSuccessAction action: {
                    state.searchState.searchTeamLoading = false;
                    state.searchState.keyword = action.keyword;
                    state.searchState.searchTeamHasMore = action.hasMore;
                    if (state.searchState.searchTeamIdDict.ContainsKey(key: action.keyword)) {
                        if (action.pageNumber == 1) {
                            state.searchState.searchTeamIdDict[key: action.keyword] = action.searchTeamIds;
                        }
                        else {
                            var searchTeamIds = state.searchState.searchTeamIdDict[key: action.keyword] ??
                                                new List<string>();
                            searchTeamIds.AddRange(collection: action.searchTeamIds);
                            state.searchState.searchTeamIdDict[key: action.keyword] = searchTeamIds;
                        }
                    }
                    else {
                        state.searchState.searchTeamIdDict.Add(key: action.keyword, value: action.searchTeamIds);
                    }

                    break;
                }

                case SearchTeamFailureAction action: {
                    state.searchState.searchTeamLoading = false;
                    state.searchState.keyword = action.keyword;
                    break;
                }

                case MainNavigatorPushReplaceSplashAction _: {
                    Router.navigator.pushReplacement(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new SplashPage(),
                            transitionDuration: TimeSpan.FromMilliseconds(600),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new FadeTransition( //使用渐隐渐入过渡, 
                                    opacity: animation,
                                    child: child
                                )
                        )
                    );
                    break;
                }

                case MainNavigatorPushReplaceMainAction _: {
                    Router.navigator.pushReplacement(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new MainScreen(),
                            transitionDuration: TimeSpan.FromMilliseconds(600),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new FadeTransition( //使用渐隐渐入过渡, 
                                    opacity: animation,
                                    child: child
                                )
                        )
                    );
                    break;
                }

                case MainNavigatorPushToChannelAction action: {
                    state.channelState.channelDict[action.channelId].unread = 0;
                    state.channelState.channelDict[action.channelId].mentioned = 0;
                    state.channelState.channelDict[action.channelId].atAll = false;
                    state.channelState.channelDict[action.channelId].atMe = false;
                    state.channelState.updateTotalMention();
                    Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new ChannelScreenConnector(action.channelId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                        )
                    );
                    break;
                }

                case MainNavigatorPushToChannelDetailAction action: {
                    Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new ChannelDetailScreenConnector(action.channelId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                        )
                    );
                    break;
                }

                case MainNavigatorPushToChannelMembersAction action: {
                    Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new ChannelMembersScreenConnector(action.channelId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                        )
                    );
                    break;
                }

                case MainNavigatorPushToChannelIntroductionAction action: {
                    Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new ChannelIntroductionScreenConnector(action.channelId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                        )
                    );
                    break;
                }

                case MainNavigatorPushToArticleDetailAction action: {
                    if (action.articleId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                                pageBuilder: (context, animation, secondaryAnimation) =>
                                    new ArticleDetailScreenConnector(articleId: action.articleId,
                                        isPush: action.isPush),
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
                                    new UserDetailScreenConnector(userId: action.userId, isSlug: action.isSlug),
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
                                    new UserFollowingScreenConnector(userId: action.userId,
                                        initialPage: action.initialPage),
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
                                    new TeamDetailScreenConnector(teamId: action.teamId, isSlug: action.isSlug),
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

                case MainNavigatorPushToTeamMemberAction action: {
                    if (action.teamId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                                pageBuilder: (context, animation, secondaryAnimation) =>
                                    new TeamMemberScreenConnector(teamId: action.teamId),
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

                case MainNavigatorPushToVideoPlayerAction action: {
                    if (action.url != null) {
                        Router.navigator.push(new PageRouteBuilder(
                                pageBuilder: (context, animation, secondaryAnimation) =>
                                    new VideoViewScreen(action.url, action.needUpdate, action.limitSeconds),
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

                case MainNavigatorPushToQRScanLoginAction action: {
                    if (action.token != null) {
                        Router.navigator.push(new PageRouteBuilder(
                                pageBuilder: (context, animation, secondaryAnimation) =>
                                    new QRScanLoginScreenConnector(token: action.token),
                                transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                    new ModalPageTransition(
                                        routeAnimation: animation,
                                        child: child
                                    )
                            )
                        );
                    }

                    break;
                }

                case MainNavigatorPushToFavoriteDetailAction action: {
                    if (action.tagId != null) {
                        Router.navigator.push(new PageRouteBuilder(
                                pageBuilder: (context, animation, secondaryAnimation) =>
                                    new FavoriteDetailScreenConnector(tagId: action.tagId, userId: action.userId),
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

                case MainNavigatorPushToEditFavoriteAction action: {
                    Router.navigator.push(new PageRouteBuilder(
                            pageBuilder: (context, animation, secondaryAnimation) =>
                                new EditFavoriteScreenConnector(tagId: action.tagId),
                            transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                )
                        )
                    );

                    break;
                }

                case MainNavigatorPushToPhotoViewAction action: {
                    if (action.url.isNotEmpty() && action.urls.isNotEmpty() && action.urls.Contains(action.url)) {
                        var index = action.urls.IndexOf(action.url);
                        Router.navigator.push(new PageRouteBuilder(
                                pageBuilder: (context, animation, secondaryAnimation) =>
                                    new PhotoView(action.urls, index: index,
                                        useCachedNetworkImage: action.useCachedNetworkImage),
                                transitionsBuilder: (context1, animation, secondaryAnimation, child) =>
                                    new ModalPageTransition(
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
                    if (!state.userState.userDict.ContainsKey(key: action.user.id)) {
                        state.userState.userDict.Add(key: action.user.id, value: action.user);
                    }
                    else {
                        var oldUser = state.userState.userDict[key: action.user.id];
                        state.userState.userDict[key: action.user.id] = oldUser.Merge(other: action.user);
                    }

                    if (action.userId != action.user.id) {
                        if (state.userState.slugDict.ContainsKey(action.userId)) {
                            state.userState.slugDict[action.userId] = action.user.id;
                        }
                        else {
                            state.userState.slugDict.Add(action.userId, action.user.id);
                        }
                    }


                    break;
                }

                case FetchUserProfileFailureAction action: {
                    state.userState.userLoading = false;
                    if (!state.userState.userDict.ContainsKey(key: action.userId)) {
                        var user = new User {
                            errorCode = action.errorCode
                        };
                        state.userState.userDict.Add(key: action.userId, value: user);
                    }
                    else {
                        var user = state.userState.userDict[key: action.userId];
                        user.errorCode = action.errorCode;
                        state.userState.userDict[key: action.userId] = user;
                    }

                    break;
                }

                case StartFetchUserArticleAction _: {
                    state.userState.userArticleLoading = true;
                    break;
                }

                case FetchUserArticleSuccessAction action: {
                    state.userState.userArticleLoading = false;
                    var articleIds = new List<string>();
                    action.articles.ForEach(article => {
                        articleIds.Add(item: article.id);
                        if (!state.articleState.articleDict.ContainsKey(key: article.id)) {
                            state.articleState.articleDict.Add(key: article.id, value: article);
                        }
                        else {
                            var oldArticle = state.articleState.articleDict[key: article.id];
                            state.articleState.articleDict[key: article.id] = oldArticle.Merge(other: article);
                        }
                    });
                    if (state.userState.userDict.ContainsKey(key: action.userId)) {
                        var user = state.userState.userDict[key: action.userId];
                        user.articlesHasMore = action.hasMore;
                        if (action.pageNumber == 1) {
                            user.articleIds = articleIds;
                        }
                        else {
                            var userArticleIds = user.articleIds;
                            userArticleIds.AddRange(collection: articleIds);
                            user.articleIds = userArticleIds;
                        }

                        state.userState.userDict[key: action.userId] = user;
                    }
                    else {
                        var user = new User {articlesHasMore = action.hasMore};
                        if (action.pageNumber == 1) {
                            user.articleIds = articleIds;
                        }
                        else {
                            var userArticleIds = user.articleIds;
                            userArticleIds.AddRange(collection: articleIds);
                            user.articleIds = userArticleIds;
                        }

                        state.userState.userDict.Add(key: action.userId, value: user);
                    }

                    break;
                }

                case FetchUserArticleFailureAction _: {
                    state.userState.userArticleLoading = false;
                    break;
                }

                case StartFollowUserAction action: {
                    if (state.userState.userDict.ContainsKey(key: action.followUserId)) {
                        var user = state.userState.userDict[key: action.followUserId];
                        user.followUserLoading = true;
                        state.userState.userDict[key: action.followUserId] = user;
                    }

                    break;
                }

                case FollowUserSuccessAction action: {
                    if (state.userState.userDict.ContainsKey(key: action.followUserId)) {
                        var user = state.userState.userDict[key: action.followUserId];
                        user.followUserLoading = false;
                        state.userState.userDict[key: action.followUserId] = user;
                    }

                    if (state.followState.followDict.ContainsKey(key: action.currentUserId)) {
                        var followMap = state.followState.followDict[key: action.currentUserId];
                        if (!followMap.ContainsKey(key: action.followUserId)) {
                            followMap.Add(key: action.followUserId, value: action.success);
                        }

                        state.followState.followDict[key: action.currentUserId] = followMap;
                    }
                    else {
                        var followMap = new Dictionary<string, bool>();
                        if (!followMap.ContainsKey(key: action.followUserId)) {
                            followMap.Add(key: action.followUserId, value: action.success);
                        }

                        state.followState.followDict.Add(key: action.currentUserId, value: followMap);
                    }

                    if (state.userState.userDict.ContainsKey(key: action.currentUserId)) {
                        var user = state.userState.userDict[key: action.currentUserId];
                        user.followingUsersCount += 1;
                        state.userState.userDict[key: action.currentUserId] = user;
                    }

                    if (state.userState.userDict.ContainsKey(key: action.followUserId)) {
                        var user = state.userState.userDict[key: action.followUserId];
                        user.followCount += 1;
                        state.userState.userDict[key: action.followUserId] = user;
                    }

                    EventBus.publish(sName: EventBusConstant.follow_user, new List<object>());

                    break;
                }

                case FollowUserFailureAction action: {
                    if (state.userState.userDict.ContainsKey(key: action.followUserId)) {
                        var user = state.userState.userDict[key: action.followUserId];
                        user.followUserLoading = false;
                        state.userState.userDict[key: action.followUserId] = user;
                    }

                    break;
                }

                case StartUnFollowUserAction action: {
                    if (state.userState.userDict.ContainsKey(key: action.unFollowUserId)) {
                        var user = state.userState.userDict[key: action.unFollowUserId];
                        user.followUserLoading = true;
                        state.userState.userDict[key: action.unFollowUserId] = user;
                    }

                    break;
                }

                case UnFollowUserSuccessAction action: {
                    if (state.userState.userDict.ContainsKey(key: action.unFollowUserId)) {
                        var user = state.userState.userDict[key: action.unFollowUserId];
                        user.followUserLoading = false;
                        state.userState.userDict[key: action.unFollowUserId] = user;
                    }

                    if (state.followState.followDict.ContainsKey(key: action.currentUserId)) {
                        var followMap = state.followState.followDict[key: action.currentUserId];
                        if (followMap.ContainsKey(key: action.unFollowUserId)) {
                            followMap.Remove(key: action.unFollowUserId);
                        }

                        state.followState.followDict[key: action.currentUserId] = followMap;
                    }

                    if (state.userState.userDict.ContainsKey(key: action.currentUserId)) {
                        var user = state.userState.userDict[key: action.currentUserId];
                        user.followingUsersCount -= 1;
                        state.userState.userDict[key: action.currentUserId] = user;
                    }

                    if (state.userState.userDict.ContainsKey(key: action.unFollowUserId)) {
                        var user = state.userState.userDict[key: action.unFollowUserId];
                        user.followCount -= 1;
                        state.userState.userDict[key: action.unFollowUserId] = user;
                    }

                    EventBus.publish(sName: EventBusConstant.follow_user, new List<object>());

                    break;
                }

                case UnFollowUserFailureAction action: {
                    if (state.userState.userDict.ContainsKey(key: action.unFollowUserId)) {
                        var user = state.userState.userDict[key: action.unFollowUserId];
                        user.followUserLoading = false;
                        state.userState.userDict[key: action.unFollowUserId] = user;
                    }

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
                        user.followingsHasMore = action.followingHasMore;
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

                case StartFetchFollowingUserAction _: {
                    state.userState.followingUserLoading = true;
                    break;
                }

                case FetchFollowingUserSuccessAction action: {
                    state.userState.followingUserLoading = false;
                    if (state.userState.userDict.ContainsKey(key: action.userId)) {
                        var user = state.userState.userDict[key: action.userId];
                        user.followingUsersHasMore = action.followingUsersHasMore;
                        if (action.offset == 0) {
                            user.followingUsers = action.followingUsers;
                        }
                        else {
                            var followingUsers = user.followingUsers;
                            followingUsers.AddRange(collection: action.followingUsers);
                            user.followingUsers = followingUsers;
                        }

                        state.userState.userDict[key: action.userId] = user;
                    }

                    break;
                }

                case FetchFollowingUserFailureAction _: {
                    state.userState.followingUserLoading = false;
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

                case StartFetchFollowingTeamAction _: {
                    state.userState.followingTeamLoading = true;
                    break;
                }

                case FetchFollowingTeamSuccessAction action: {
                    state.userState.followingTeamLoading = false;
                    if (state.userState.userDict.ContainsKey(key: action.userId)) {
                        var user = state.userState.userDict[key: action.userId];
                        user.followingTeamsHasMore = action.followingTeamsHasMore;
                        if (action.offset == 0) {
                            user.followingTeams = action.followingTeams;
                        }
                        else {
                            var followingTeams = user.followingTeams;
                            followingTeams.AddRange(collection: action.followingTeams);
                            user.followingTeams = followingTeams;
                        }

                        state.userState.userDict[key: action.userId] = user;
                    }

                    break;
                }

                case FetchFollowingTeamFailureAction _: {
                    state.userState.followingTeamLoading = false;
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
                        state.userState.userDict[key: action.user.id] = oldUser.Merge(action.user);
                    }

                    break;
                }

                case UpdateAvatarSuccessAction action: {
                    var userId = state.loginState.loginInfo.userId;
                    var user = state.userState.userDict[userId];
                    user.avatar = action.avatar;
                    state.userState.userDict[userId] = user;
                    state.loginState.loginInfo.userAvatar = action.avatar;
                    UserInfoManager.saveUserInfo(state.loginState.loginInfo);
                    break;
                }

                case StartFetchTeamAction _: {
                    state.teamState.teamLoading = true;
                    break;
                }

                case FetchTeamSuccessAction action: {
                    state.teamState.teamLoading = false;
                    var team = action.team.copyWith(isDetail: true);
                    if (!state.teamState.teamDict.ContainsKey(key: action.team.id)) {
                        state.teamState.teamDict.Add(key: action.team.id, value: team);
                    }
                    else {
                        var oldTeam = state.teamState.teamDict[key: action.team.id];
                        state.teamState.teamDict[key: action.team.id] = oldTeam.Merge(other: team);
                    }

                    if (action.teamId != action.team.id) {
                        if (state.teamState.slugDict.ContainsKey(action.teamId)) {
                            state.teamState.slugDict[action.teamId] = action.team.id;
                        }
                        else {
                            state.teamState.slugDict.Add(action.teamId, action.team.id);
                        }
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
                    var articleIds = new List<string>();
                    action.articles.ForEach(article => {
                        articleIds.Add(item: article.id);
                        if (!state.articleState.articleDict.ContainsKey(key: article.id)) {
                            state.articleState.articleDict.Add(key: article.id, value: article);
                        }
                        else {
                            var oldArticle = state.articleState.articleDict[key: article.id];
                            state.articleState.articleDict[key: article.id] = oldArticle.Merge(other: article);
                        }
                    });
                    if (state.teamState.teamDict.ContainsKey(key: action.teamId)) {
                        var team = state.teamState.teamDict[key: action.teamId];
                        team.articlesHasMore = action.hasMore;
                        if (action.pageNumber == 1) {
                            team.articleIds = articleIds;
                        }
                        else {
                            var teamArticleIds = team.articleIds;
                            teamArticleIds.AddRange(collection: articleIds);
                            team.articleIds = teamArticleIds;
                        }

                        state.teamState.teamDict[key: action.teamId] = team;
                    }
                    else {
                        var team = new Team {articlesHasMore = action.hasMore};
                        if (action.pageNumber == 1) {
                            team.articleIds = articleIds;
                        }
                        else {
                            var teamArticleIds = team.articleIds;
                            teamArticleIds.AddRange(collection: articleIds);
                            team.articleIds = teamArticleIds;
                        }

                        state.teamState.teamDict.Add(key: action.teamId, value: team);
                    }

                    break;
                }

                case FetchTeamArticleFailureAction _: {
                    state.teamState.teamArticleLoading = false;
                    break;
                }

                case StartFetchTeamFollowerAction _: {
                    state.teamState.followerLoading = true;
                    break;
                }

                case FetchTeamFollowerSuccessAction action: {
                    state.teamState.followerLoading = false;
                    if (state.teamState.teamDict.ContainsKey(key: action.teamId)) {
                        var team = state.teamState.teamDict[key: action.teamId];
                        team.followersHasMore = action.followersHasMore;
                        if (action.offset == 0) {
                            team.followers = action.followers;
                        }
                        else {
                            var followers = team.followers;
                            followers.AddRange(collection: action.followers);
                            team.followers = followers;
                        }

                        state.teamState.teamDict[key: action.teamId] = team;
                    }

                    break;
                }

                case FetchTeamFollowerFailureAction _: {
                    state.teamState.followerLoading = false;
                    break;
                }

                case StartFetchTeamMemberAction _: {
                    state.teamState.memberLoading = true;
                    break;
                }

                case FetchTeamMemberSuccessAction action: {
                    state.teamState.memberLoading = false;
                    if (state.teamState.teamDict.ContainsKey(key: action.teamId)) {
                        var team = state.teamState.teamDict[key: action.teamId];
                        team.membersHasMore = action.membersHasMore;
                        if (action.pageNumber == 1) {
                            team.members = action.members;
                        }
                        else {
                            var members = team.members;
                            members.AddRange(collection: action.members);
                            team.members = members;
                        }

                        state.teamState.teamDict[key: action.teamId] = team;
                    }

                    break;
                }

                case FetchTeamMemberFailureAction _: {
                    state.teamState.memberLoading = false;
                    break;
                }

                case StartFetchFollowTeamAction action: {
                    if (state.teamState.teamDict.ContainsKey(key: action.followTeamId)) {
                        var team = state.teamState.teamDict[key: action.followTeamId];
                        team.followTeamLoading = true;
                        state.teamState.teamDict[key: action.followTeamId] = team;
                    }

                    break;
                }

                case FetchFollowTeamSuccessAction action: {
                    if (state.teamState.teamDict.ContainsKey(key: action.followTeamId)) {
                        var team = state.teamState.teamDict[key: action.followTeamId];
                        team.followTeamLoading = false;
                        state.teamState.teamDict[key: action.followTeamId] = team;
                    }

                    if (state.followState.followDict.ContainsKey(key: action.currentUserId)) {
                        var followMap = state.followState.followDict[key: action.currentUserId];
                        if (!followMap.ContainsKey(key: action.followTeamId)) {
                            followMap.Add(key: action.followTeamId, value: action.success);
                        }

                        state.followState.followDict[key: action.currentUserId] = followMap;
                    }
                    else {
                        var followMap = new Dictionary<string, bool>();
                        if (!followMap.ContainsKey(key: action.followTeamId)) {
                            followMap.Add(key: action.followTeamId, value: action.success);
                        }

                        state.followState.followDict.Add(key: action.currentUserId, value: followMap);
                    }

                    if (state.userState.userDict.ContainsKey(key: action.currentUserId)) {
                        var user = state.userState.userDict[key: action.currentUserId];
                        user.followingUsersCount += 1;
                        state.userState.userDict[key: action.currentUserId] = user;
                    }

                    if (state.teamState.teamDict.ContainsKey(key: action.followTeamId)) {
                        var team = state.teamState.teamDict[key: action.followTeamId];
                        if (team.stats != null) {
                            team.stats.followCount += 1;
                            state.teamState.teamDict[key: action.followTeamId] = team;
                        }
                    }

                    EventBus.publish(sName: EventBusConstant.follow_user, new List<object>());

                    break;
                }

                case FetchFollowTeamFailureAction action: {
                    if (state.teamState.teamDict.ContainsKey(key: action.followTeamId)) {
                        var team = state.teamState.teamDict[key: action.followTeamId];
                        team.followTeamLoading = false;
                        state.teamState.teamDict[key: action.followTeamId] = team;
                    }

                    break;
                }

                case StartFetchUnFollowTeamAction action: {
                    if (state.teamState.teamDict.ContainsKey(key: action.unFollowTeamId)) {
                        var team = state.teamState.teamDict[key: action.unFollowTeamId];
                        team.followTeamLoading = true;
                        state.teamState.teamDict[key: action.unFollowTeamId] = team;
                    }

                    break;
                }

                case FetchUnFollowTeamSuccessAction action: {
                    if (state.teamState.teamDict.ContainsKey(key: action.unFollowTeamId)) {
                        var team = state.teamState.teamDict[key: action.unFollowTeamId];
                        team.followTeamLoading = false;
                        state.teamState.teamDict[key: action.unFollowTeamId] = team;
                    }

                    if (state.followState.followDict.ContainsKey(key: action.currentUserId)) {
                        var followMap = state.followState.followDict[key: action.currentUserId];
                        if (followMap.ContainsKey(key: action.unFollowTeamId)) {
                            followMap.Remove(key: action.unFollowTeamId);
                        }

                        state.followState.followDict[key: action.currentUserId] = followMap;
                    }

                    if (state.userState.userDict.ContainsKey(key: action.currentUserId)) {
                        var user = state.userState.userDict[key: action.currentUserId];
                        user.followingUsersCount -= 1;
                        state.userState.userDict[key: action.currentUserId] = user;
                    }

                    if (state.teamState.teamDict.ContainsKey(key: action.unFollowTeamId)) {
                        var team = state.teamState.teamDict[key: action.unFollowTeamId];
                        if (team.stats != null) {
                            team.stats.followCount -= 1;
                            state.teamState.teamDict[key: action.unFollowTeamId] = team;
                        }
                    }

                    EventBus.publish(sName: EventBusConstant.follow_user, new List<object>());

                    break;
                }

                case FetchUnFollowTeamFailureAction action: {
                    if (state.teamState.teamDict.ContainsKey(key: action.unFollowTeamId)) {
                        var team = state.teamState.teamDict[key: action.unFollowTeamId];
                        team.followTeamLoading = false;
                        state.teamState.teamDict[key: action.unFollowTeamId] = team;
                    }

                    break;
                }

                case ChangeFeedbackTypeAction action: {
                    state.feedbackState.feedbackType = action.type;
                    break;
                }

                case StartFeedbackAction _: {
                    state.feedbackState.loading = true;
                    break;
                }

                case FeedbackSuccessAction _: {
                    state.feedbackState.loading = false;
                    break;
                }

                case FeedbackFailureAction _: {
                    state.feedbackState.loading = false;
                    break;
                }

                case StartFetchFavoriteTagAction _: {
                    state.favoriteState.favoriteTagLoading = true;
                    break;
                }

                case FetchFavoriteTagSuccessAction action: {
                    if (action.offset == 0) {
                        state.favoriteState.favoriteTagIds = new List<string>();
                    }

                    action.favoriteTags.ForEach(favoriteTag => {
                        state.favoriteState.favoriteTagIds.Add(item: favoriteTag.id);
                        if (state.favoriteState.favoriteTagDict.ContainsKey(key: favoriteTag.id)) {
                            state.favoriteState.favoriteTagDict[key: favoriteTag.id] = favoriteTag;
                        }
                        else {
                            state.favoriteState.favoriteTagDict.Add(key: favoriteTag.id, value: favoriteTag);
                        }
                    });

                    state.favoriteState.favoriteTagHasMore = action.hasMore;
                    state.favoriteState.favoriteTagLoading = false;
                    break;
                }

                case FetchFavoriteTagFailureAction _: {
                    state.favoriteState.favoriteTagLoading = false;
                    break;
                }

                case StartFetchFavoriteDetailAction _: {
                    state.favoriteState.favoriteDetailLoading = true;
                    break;
                }

                case FetchFavoriteDetailSuccessAction action: {
                    if (action.tagMap != null && action.tagMap.isNotEmpty()) {
                        var favoriteTagDict = state.favoriteState.favoriteTagDict;
                        foreach (var keyValuePair in action.tagMap) {
                            if (favoriteTagDict.ContainsKey(key: keyValuePair.Key)) {
                                favoriteTagDict[key: keyValuePair.Key] = keyValuePair.Value;
                            }
                            else {
                                favoriteTagDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                            }
                        }

                        state.favoriteState.favoriteTagDict = favoriteTagDict;
                    }

                    if (action.projectSimpleMap != null && action.projectSimpleMap.isNotEmpty()) {
                        var articleDict = state.articleState.articleDict;
                        foreach (var keyValuePair in action.projectSimpleMap) {
                            if (articleDict.ContainsKey(key: keyValuePair.Key)) {
                                var oldArticle = articleDict[key: keyValuePair.Key];
                                articleDict[key: keyValuePair.Key] = oldArticle.Merge(other: keyValuePair.Value);
                            }
                            else {
                                articleDict.Add(key: keyValuePair.Key, value: keyValuePair.Value);
                            }
                        }

                        state.articleState.articleDict = articleDict;
                    }

                    var favoriteDetailArticleIdDict = state.favoriteState.favoriteDetailArticleIdDict;
                    var tagId = action.tagId.isNotEmpty() ? action.tagId : $"{action.userId}all";
                    var favoriteDetailArticleIds = favoriteDetailArticleIdDict.ContainsKey(key: tagId)
                        ? favoriteDetailArticleIdDict[key: tagId]
                        : new List<string>();
                    if (action.offset == 0) {
                        favoriteDetailArticleIds.Clear();
                    }

                    if (action.favorites != null && action.favorites.isNotEmpty()) {
                        var articleDict = state.articleState.articleDict;
                        action.favorites.ForEach(favorite => {
                            favoriteDetailArticleIds.Add(item: favorite.itemId);
                            if (articleDict.ContainsKey(key: favorite.itemId)) {
                                var article = articleDict[key: favorite.itemId];
                                article.favorite = favorite;
                                articleDict[key: favorite.itemId] = article;
                            }
                        });

                        state.articleState.articleDict = articleDict;
                    }

                    if (favoriteDetailArticleIdDict.ContainsKey(key: tagId)) {
                        favoriteDetailArticleIdDict[key: tagId] = favoriteDetailArticleIds;
                    }
                    else {
                        favoriteDetailArticleIdDict.Add(key: tagId, value: favoriteDetailArticleIds);
                    }

                    state.favoriteState.favoriteDetailArticleIdDict = favoriteDetailArticleIdDict;
                    state.favoriteState.favoriteDetailHasMore = action.hasMore;
                    state.favoriteState.favoriteDetailLoading = false;
                    break;
                }

                case FetchFavoriteDetailFailureAction _: {
                    state.favoriteState.favoriteDetailLoading = false;
                    break;
                }

                case CreateFavoriteTagSuccessAction action: {
                    if (!state.favoriteState.favoriteTagIds.Contains(item: action.favoriteTag.id)) {
                        if (state.favoriteState.favoriteTagIds.Count <= 1) {
                            state.favoriteState.favoriteTagIds.Add(item: action.favoriteTag.id);
                        }
                        else {
                            state.favoriteState.favoriteTagIds.Insert(1, item: action.favoriteTag.id);
                        }
                    }

                    if (!state.favoriteState.favoriteTagDict.ContainsKey(key: action.favoriteTag.id)) {
                        state.favoriteState.favoriteTagDict.Add(key: action.favoriteTag.id, value: action.favoriteTag);
                    }

                    break;
                }

                case EditFavoriteTagSuccessAction action: {
                    if (state.favoriteState.favoriteTagDict.ContainsKey(key: action.favoriteTag.id)) {
                        state.favoriteState.favoriteTagDict[key: action.favoriteTag.id] = action.favoriteTag;
                    }
                    else {
                        state.favoriteState.favoriteTagDict.Add(key: action.favoriteTag.id, value: action.favoriteTag);
                    }

                    break;
                }

                case DeleteFavoriteTagSuccessAction action: {
                    if (state.favoriteState.favoriteTagIds.Contains(item: action.favoriteTag.id)) {
                        state.favoriteState.favoriteTagIds.Remove(item: action.favoriteTag.id);
                    }

                    if (state.favoriteState.favoriteTagDict.ContainsKey(key: action.favoriteTag.id)) {
                        state.favoriteState.favoriteTagDict.Remove(key: action.favoriteTag.id);
                    }

                    break;
                }

                case InitEggsAction action: {
                    state.serviceConfigState.showFirstEgg = action.firstEgg;
                    break;
                }

                case ScanEnabledAction action: {
                    state.serviceConfigState.scanEnabled = action.scanEnabled;
                    break;
                }

                case NationalDayEnabledAction action: {
                    state.serviceConfigState.nationalDayEnabled = action.nationalDayEnabled;
                    break;
                }

                case EnterRealityAction _: {
                    // Enter Reality
                    RealityManager.TriggerSwitch();
                    break;
                }

                case FetchChannelsSuccessAction action: {
                    action.discoverList.ForEach(discover => {
                        if (!state.channelState.publicChannels.Contains(item: discover)) {
                            state.channelState.publicChannels.Add(item: discover);
                        }
                    });

                    state.channelState.discoverPage = action.discoverPage;
                    state.channelState.joinedChannels = action.joinedList;
                    foreach (var channelId in action.joinedList) {
                        action.joinedChannelMap[key: channelId] = true;
                    }

                    foreach (var entry in action.channelMap) {
                        state.channelState.updateChannel(channel: entry.Value);
                        state.channelState.channelDict[key: entry.Key].joined =
                            action.joinedChannelMap.ContainsKey(key: entry.Key);
                    }

                    state.channelState.channelTop = ChannelTopManager.getChannelTop();
                    break;
                }

                case FetchChannelsFailureAction _: {
                    state.channelState.socketConnected = false;
                    break;
                }

                case StartFetchChannelMessageAction _: {
                    state.channelState.messageLoading = true;
                    break;
                }

                case FetchChannelMessagesSuccessAction action: {
                    var channel = state.channelState.channelDict[key: action.channelId];
                    if (channel.messageIds == null || (action.after == null && action.before == null)) {
                        channel.messageIds = new List<string>();
                    }

                    channel.hasMore = action.hasMore;
                    channel.hasMoreNew = action.hasMoreNew;

                    if (action.after != null || channel.messageIds.isEmpty()) {
                        D.assert(channel.messageIds.isEmpty() || channel.messageIds.last() == action.after);
                        for (var i = action.messages.Count - 1; i >= 0; i--) {
                            var channelMessage = ChannelMessageView.fromChannelMessage(action.messages[i]);
                            state.channelState.messageDict[channelMessage.id] = channelMessage;
                            channel.messageIds.Add(channelMessage.id);
                            if (CStringUtils.HexToLong(channelMessage.id) >
                                CStringUtils.HexToLong(channel.lastMessage.id)) {
                                channel.lastMessage = channelMessage;
                                channel.lastMessageId = channelMessage.id;
                            }
                        }
                    }
                    else if (action.before != null) {
                        D.assert(channel.messageIds.first() == action.before);
                        int to = action.messages[0].id == action.before ? 1 : 0;
                        for (var i = action.messages.Count - 1; i >= to; i--) {
                            var channelMessage = ChannelMessageView.fromChannelMessage(action.messages[i]);
                            state.channelState.messageDict[channelMessage.id] = channelMessage;
                            channel.oldMessageIds.Add(channelMessage.id);
                        }
                    }

                    state.channelState.messageLoading = false;
                    state.channelState.updateTotalMention();
                    if (channel.atBottom) {
                        channel.clearUnread();
                    }

                    break;
                }

                case FetchChannelMessagesFailureAction _: {
                    state.channelState.messageLoading = false;
                    state.channelState.socketConnected = false;
                    break;
                }

                case MergeNewChannelMessages action: {
                    var channel = state.channelState.channelDict[action.channelId];
                    channel.messageIds.AddRange(channel.newMessageIds);
                    channel.newMessageIds = new List<string>();
                    break;
                }

                case MergeOldChannelMessages action: {
                    var channel = state.channelState.channelDict[action.channelId];
                    channel.oldMessageIds.AddRange(channel.messageIds);
                    channel.messageIds = channel.oldMessageIds;
                    channel.oldMessageIds = new List<string>();
                    break;
                }

                case StartSendChannelMessageAction action: {
                    var channel = state.channelState.channelDict[action.channelId];
                    channel.sendingMessage = true;
                    break;
                }

                case SendChannelMessageSuccessAction action: {
                    var channel = state.channelState.channelDict[action.channelId];
                    channel.sendingMessage = false;
                    channel.sentMessageSuccess = true;
                    break;
                }

                case SendChannelMessageFailureAction action: {
                    var channel = state.channelState.channelDict[action.channelId];
                    channel.sendingMessage = false;
                    channel.sentMessageFailed = true;
                    break;
                }

                case ClearSentChannelMessage action: {
                    var channel = state.channelState.channelDict[key: action.channelId];
                    channel.sentMessageSuccess = false;
                    channel.sentMessageFailed = false;
                    break;
                }

                case FetchChannelMemberSuccessAction action: {
                    var channel = state.channelState.channelDict[key: action.channelId];
                    if (channel.messageIds == null) {
                        channel.memberIds = new List<string>();
                    }

                    action.members.ForEach(channelMember => {
                        channel.membersDict[key: channelMember.id] = channelMember;
                        if (!channel.memberIds.Contains(item: channelMember.id)) {
                            channel.memberIds.Add(item: channelMember.id);
                        }

                        channel.memberCount = action.total;
                        channel.memberOffset = action.offset;
                    });

                    break;
                }

                case StartJoinChannelAction action: {
                    var channel = state.channelState.channelDict[key: action.channelId];
                    channel.joinLoading = true;
                    break;
                }

                case JoinChannelSuccessAction action: {
                    var channel = state.channelState.channelDict[key: action.channelId];
                    channel.joined = true;
                    channel.joinLoading = false;
                    channel.memberCount++;
                    if (!channel.memberIds.Contains(item: state.loginState.loginInfo.userId)) {
                        channel.memberIds.Add(state.loginState.loginInfo.userId);
                        channel.membersDict[state.loginState.loginInfo.userId] = new ChannelMember {
                            channelId = action.channelId,
                            id = state.loginState.loginInfo.userId,
                            role = "member",
                            user = new User {
                                fullName = state.loginState.loginInfo.userFullName,
                                avatar = state.loginState.loginInfo.userAvatar,
                                id = state.loginState.loginInfo.userId
                            }
                        };
                    }

                    if (!state.channelState.joinedChannels.Contains(item: action.channelId)) {
                        state.channelState.joinedChannels.Add(item: action.channelId);
                    }

                    break;
                }

                case JoinChannelFailureAction action: {
                    var channel = state.channelState.channelDict[key: action.channelId];
                    channel.joinLoading = false;
                    break;
                }

                case LeaveChannelSuccessAction action: {
                    var channel = state.channelState.channelDict[key: action.channelId];
                    channel.joined = false;
                    channel.memberIds.Remove(state.loginState.loginInfo.userId);
                    channel.memberCount -= 1;
                    state.channelState.joinedChannels.Remove(item: action.channelId);
                    break;
                }

                case SaveMessagesToDBSuccessAction _: {
                    break;
                }

                case LoadMessagesFromDBSuccessAction action: {
                    if (!state.channelState.channelDict.TryGetValue(action.channelId, out var channel)) {
                        break;
                    }

                    if (action.before == -1) {
                        for (int i = action.messages.Count - 1; i >= 0; i--) {
                            var message = action.messages[i];
                            if (!channel.messageIds.Contains(message.id)) {
                                channel.messageIds.Add(message.id);
                            }

                            state.channelState.messageDict[message.id] = message;
                        }
                    }
                    else {
                        var messageIds = new List<string>();
                        for (int i = action.messages.Count - 1; i >= 0; i--) {
                            var message = action.messages[i];
                            if (!channel.messageIds.Contains(message.id)) {
                                messageIds.Add(message.id);
                            }

                            state.channelState.messageDict[message.id] = message;
                        }

                        messageIds.AddRange(channel.messageIds);
                        channel.messageIds = messageIds;
                    }

                    state.channelState.channelTop = ChannelTopManager.getChannelTop();
                    state.channelState.messageLoading = false;

                    break;
                }

                case SaveReadyStateToDBSuccessAction _: {
                    break;
                }

                case LoadReadyStateFromDBSuccessAction action: {
                    state.channelState.updateSessionReadyData(action.data);
                    break;
                }

                case ClearChannelUnreadAction action: {
                    var channel = state.channelState.channelDict[action.channelId];
                    channel.clearUnread();
                    state.channelState.updateTotalMention();
                    break;
                }

                case ChannelScreenHitBottom action: {
                    var channel = state.channelState.channelDict[action.channelId];
                    channel.clearUnread();
                    channel.atBottom = true;
                    state.channelState.updateTotalMention();
                    break;
                }

                case ChannelScreenLeaveBottom action: {
                    var channel = state.channelState.channelDict[action.channelId];
                    channel.atBottom = false;
                    break;
                }

                case UpdateChannelTopAction action: {
                    state.channelState.channelTop[action.channelId] = action.value;
                    ChannelTopManager.saveChannelTop(state.channelState.channelTop);
                    break;
                }

                case PushReadyAction action: {
                    state.channelState.updateSessionReadyData(action.readyData);
                    state.channelState.updateTotalMention();
                    break;
                }

                case PushNewMessageAction action: {
                    var message = action.messageData;
                    if (!state.channelState.channelDict.ContainsKey(message.channelId)) {
                        break;
                    }

                    var channel = state.channelState.channelDict[message.channelId];
                    //ignore duplicated message
                    if (!channel.messageIds.Contains(message.id) && !channel.newMessageIds.Contains(message.id) &&
                        !channel.oldMessageIds.Contains(message.id)) {
                        var channelMessage = ChannelMessageView.fromPushMessage(message);
                        state.channelState.messageDict[channelMessage.id] = channelMessage;
                        if (channel.atBottom) {
                            channel.messageIds.Add(channelMessage.id);
                        }
                        else {
                            channel.newMessageIds.Add(channelMessage.id);
                        }

                        channel.lastMessageId = channelMessage.id;
                        channel.lastMessage = channelMessage;
                        if ((!state.loginState.isLoggedIn ||
                             channelMessage.author.id != state.loginState.loginInfo.userId) &&
                            !channel.atBottom) {
                            channel.handleUnreadMessage(channelMessage, state.loginState.loginInfo.userId);
                        }
                    }

                    state.channelState.updateTotalMention();
                    break;
                }

                case PushModifyMessageAction action: {
                    var message = action.messageData;
                    if (!state.channelState.channelDict.ContainsKey(message.channelId)) {
                        break;
                    }

                    var channel = state.channelState.channelDict[message.channelId];

                    var channelMessage = ChannelMessageView.fromPushMessage(message);
                    state.channelState.messageDict[channelMessage.id] = channelMessage;

                    //insert new if not exists yet
                    if (!channel.messageIds.Contains(message.id) && !channel.newMessageIds.Contains(message.id)) {
                        channel.messageIds.Add(channelMessage.id);
                    }

                    break;
                }

                case PushDeleteMessageAction action: {
                    var message = action.messageData;
                    if (!state.channelState.channelDict.ContainsKey(message.channelId)) {
                        break;
                    }

                    var channel = state.channelState.channelDict[message.channelId];
                    var messageId = message.id;
                    if (state.channelState.messageDict.ContainsKey(messageId)) {
                        state.channelState.messageDict.Remove(messageId);

                        if (channel.messageIds.Contains(messageId)) {
                            channel.messageIds.Remove(messageId);
                        }
                    }

                    break;
                }

                case PushPresentUpdateAction action: {
                    foreach (var channel in state.channelState.channelDict.Values) {
                        if (channel.membersDict.ContainsKey(action.presentUpdateData.userId)) {
                            channel.membersDict[action.presentUpdateData.userId].presenceStatus =
                                action.presentUpdateData.status;
                        }
                    }

                    break;
                }

                case PushChannelAddMemberAction action: {
                    if (state.channelState.channelDict.ContainsKey(action.memberData.channelId)) {
                        ChannelView channel = state.channelState.channelDict[action.memberData.channelId];
                        if (channel.membersDict.ContainsKey(action.memberData.id)) {
                            channel.membersDict[action.memberData.id]
                                .updateFromSocketResponseChannelMemberChangeData(action.memberData);
                        }
                        else {
                            channel.membersDict[action.memberData.id] =
                                ChannelMember.fromSocketResponseChannelMemberChangeData(action.memberData);
                        }

                        if (!channel.memberIds.Contains(action.memberData.id)) {
                            channel.memberIds.Add(action.memberData.id);
                            channel.memberCount++;
                        }
                    }

                    break;
                }

                case PushChannelRemoveMemberAction action: {
                    if (state.channelState.channelDict.ContainsKey(action.memberData.channelId)) {
                        ChannelView channel = state.channelState.channelDict[action.memberData.channelId];
                        channel.memberIds.Remove(action.memberData.id);
                        channel.memberCount--;
                    }

                    break;
                }

                case SocketConnectStateAction action: {
                    state.channelState.socketConnected = action.connected;
                    break;
                }

                case SwitchTabBarIndexAction action: {
                    state.tabBarState.currentTabIndex = action.index;

                    break;
                }
            }

            return state;
        }
    }
}