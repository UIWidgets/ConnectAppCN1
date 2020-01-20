using System;
using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class ArticleMapAction : BaseAction {
        public Dictionary<string, Article> articleMap;
    }

    public class StartFetchArticlesAction : RequestAction {
    }

    public class FetchArticleSuccessAction : BaseAction {
        public List<Article> articleList;
        public bool hottestHasMore;
        public int offset;
        public bool feedHasNew;
        public List<string> homeSliderIds;
        public List<string> homeTopCollectionIds;
        public List<string> homeCollectionIds;
        public List<string> homeBloggerIds;
        public string searchSuggest;
        public string dailySelectionId;
        public DateTime? leaderBoardUpdatedTime;
    }

    public class FetchArticleFailureAction : BaseAction {
    }

    public class StartFetchFollowArticlesAction : RequestAction {
    }

    public class FetchFollowArticleSuccessAction : BaseAction {
        public List<Feed> feeds;
        public bool feedHasNew;
        public bool feedIsFirst;
        public bool feedHasMore;
        public List<HottestItem> hotItems;
        public bool hotHasMore;
        public int hotPage;
        public int pageNumber;
    }

    public class FetchFollowArticleFailureAction : BaseAction {
    }

    public class StartFetchArticleDetailAction : RequestAction {
    }

    public class FetchArticleDetailSuccessAction : BaseAction {
        public Project articleDetail;
        public string articleId;
    }

    public class FetchArticleDetailFailureAction : BaseAction {
    }

    public class SaveArticleHistoryAction : BaseAction {
        public Article article;
    }

    public class DeleteArticleHistoryAction : BaseAction {
        public string articleId;
    }

    public class DeleteAllArticleHistoryAction : BaseAction {
    }

    public class FetchArticleCommentsSuccessAction : BaseAction {
        public string channelId;
        public List<string> itemIds;
        public Dictionary<string, Message> messageItems;
        public string currOldestMessageId;
        public bool hasMore;
        public bool isRefreshList;
    }

    public class LikeArticleSuccessAction : BaseAction {
        public string articleId;
        public int likeCount;
    }

    public class FavoriteArticleSuccessAction : BaseAction {
        public List<Favorite> favorites;
        public string articleId;
    }

    public class UnFavoriteArticleSuccessAction : BaseAction {
        public Favorite favorite;
        public string articleId;
    }

    public class BlockArticleAction : RequestAction {
        public string articleId;
    }

    public class LikeCommentSuccessAction : BaseAction {
        public Message message;
    }

    public class RemoveLikeCommentSuccessAction : BaseAction {
        public Message message;
    }

    public class SendCommentSuccessAction : BaseAction {
        public Message message;
        public string articleId;
        public string channelId;
        public string parentMessageId;
        public string upperMessageId;
    }

    public static partial class Actions {
        public static object fetchArticles(string userId, int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                var articleOffset = getState().articleState.recommendArticleIds.Count;
                if (offset != 0 && offset != articleOffset) {
                    offset = articleOffset;
                }

                return ArticleApi.FetchArticles(userId: userId, offset: offset)
                    .Then(articlesResponse => {
                        var articleList = new List<Article>();
                        articlesResponse.hottests.ForEach(item => {
                            if (articlesResponse.projectMap.ContainsKey(key: item.itemId)) {
                                var article = articlesResponse.projectMap[key: item.itemId];
                                articleList.Add(item: article);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = articlesResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = articlesResponse.teamMap});
                        dispatcher.dispatch(new FollowMapAction {followMap = articlesResponse.followMap});
                        dispatcher.dispatch(new LikeMapAction {likeMap = articlesResponse.likeMap});
                        var homeSliderIds = new List<string>();
                        var homeTopCollectionIds = new List<string>();
                        var homeCollectionIds = new List<string>();
                        var homeBloggerIds = new List<string>();
                        if (articlesResponse.rankData != null) {
                            // 轮播图
                            var homeSlider = articlesResponse.rankData.homeSlider ?? new HomeSlider();
                            dispatcher.dispatch(new RankListAction {rankList = homeSlider.rankList});
                            (homeSlider.rankList ?? new List<RankData>()).ForEach(rankData => {
                                homeSliderIds.Add(item: rankData.id);
                            });

                            // 推荐榜单
                            var homeTopCollection = articlesResponse.rankData.homeTopCollection ?? new HomeCollection();
                            dispatcher.dispatch(new RankListAction {rankList = homeTopCollection.rankList});
                            dispatcher.dispatch(new FavoriteTagMapAction {
                                favoriteTagMap = homeTopCollection.favoriteTagMap
                            });
                            dispatcher.dispatch(new FavoriteTagArticleMapAction {
                                favoriteTagArticleMap = homeTopCollection.favoriteTagArticleMap
                            });
                            dispatcher.dispatch(new CollectedTagMapAction {
                                collectedTagMap = homeTopCollection.collectedTagMap
                            });
                            (homeTopCollection.rankList ?? new List<RankData>()).ForEach(rankData => {
                                homeTopCollectionIds.Add(item: rankData.id);
                            });

                            // 榜单
                            var homeCollection = articlesResponse.rankData.homeCollection ?? new HomeCollection();
                            dispatcher.dispatch(new RankListAction {rankList = homeCollection.rankList});
                            dispatcher.dispatch(new FavoriteTagMapAction {
                                favoriteTagMap = homeCollection.favoriteTagMap
                            });
                            dispatcher.dispatch(new FavoriteTagArticleMapAction {
                                favoriteTagArticleMap = homeCollection.favoriteTagArticleMap
                            });
                            dispatcher.dispatch(new CollectedTagMapAction {
                                collectedTagMap = homeCollection.collectedTagMap
                            });
                            (homeCollection.rankList ?? new List<RankData>()).ForEach(rankData => {
                                homeCollectionIds.Add(item: rankData.id);
                            });

                            // 推荐博主
                            var homeBlogger = articlesResponse.rankData.homeBlogger ?? new FetchBloggerResponse();
                            dispatcher.dispatch(new RankListAction {rankList = homeBlogger.rankList});
                            dispatcher.dispatch(new UserMapAction {userMap = homeBlogger.userFullMap});
                            dispatcher.dispatch(new FollowMapAction {followMap = homeBlogger.followMap});
                            dispatcher.dispatch(new UserLicenseMapAction {userLicenseMap = homeBlogger.userLicenseMap});
                            (homeBlogger.rankList ?? new List<RankData>()).ForEach(rankData => {
                                homeBloggerIds.Add(item: rankData.id);
                            });
                        }

                        dispatcher.dispatch(new FetchArticleSuccessAction {
                            offset = offset,
                            hottestHasMore = articlesResponse.hottestHasMore,
                            articleList = articleList,
                            feedHasNew = articlesResponse.feedHasNew,
                            homeSliderIds = homeSliderIds,
                            homeTopCollectionIds = homeTopCollectionIds,
                            homeCollectionIds = homeCollectionIds,
                            homeBloggerIds = homeBloggerIds,
                            searchSuggest = articlesResponse.rankData?.searchSuggest,
                            dailySelectionId = articlesResponse.rankData?.dailySelectionId,
                            leaderBoardUpdatedTime = articlesResponse.rankData.leaderboardUpdatedTime
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchArticleFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchFollowArticles(int pageNumber, string beforeTime, string afterTime, bool isFirst,
            bool isHot) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.FetchFollowArticles(pageNumber: pageNumber, beforeTime: beforeTime,
                        afterTime: afterTime, isFirst: isFirst, isHot: isHot)
                    .Then(followArticlesResponse => {
                        dispatcher.dispatch(new ArticleMapAction
                            {articleMap = followArticlesResponse.projectSimpleMap});
                        dispatcher.dispatch(new UserMapAction {userMap = followArticlesResponse.userMap});
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = followArticlesResponse.userLicenseMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = followArticlesResponse.teamMap});
                        dispatcher.dispatch(new FollowMapAction {followMap = followArticlesResponse.followMap});
                        dispatcher.dispatch(new LikeMapAction {likeMap = followArticlesResponse.likeMap});
                        dispatcher.dispatch(new FetchFollowArticleSuccessAction {
                            feeds = followArticlesResponse.feeds,
                            feedHasNew = followArticlesResponse.feedHasNew,
                            feedIsFirst = followArticlesResponse.feedIsFirst,
                            feedHasMore = followArticlesResponse.feedHasMore,
                            hotItems = followArticlesResponse.hotItems,
                            hotHasMore = followArticlesResponse.hotHasMore,
                            hotPage = followArticlesResponse.hotPage,
                            pageNumber = pageNumber
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchFollowArticleFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object fetchArticleComments(string channelId, string currOldestMessageId = "") {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.FetchArticleComments(channelId: channelId, currOldestMessageId: currOldestMessageId)
                    .Then(responseComments => {
                        var itemIds = new List<string>();
                        var messageItems = new Dictionary<string, Message>();
                        var userMap = new Dictionary<string, User>();
                        responseComments.items.ForEach(message => {
                            itemIds.Add(item: message.id);
                            messageItems[key: message.id] = message;
                            if (userMap.ContainsKey(key: message.author.id)) {
                                userMap[key: message.author.id] = message.author;
                            }
                            else {
                                userMap.Add(key: message.author.id, value: message.author);
                            }
                        });
                        responseComments.parents.ForEach(message => {
                            if (messageItems.ContainsKey(key: message.id)) {
                                messageItems[key: message.id] = message;
                            }
                            else {
                                messageItems.Add(key: message.id, value: message);
                            }

                            if (userMap.ContainsKey(key: message.author.id)) {
                                userMap[key: message.author.id] = message.author;
                            }
                            else {
                                userMap.Add(key: message.author.id, value: message.author);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = responseComments.userLicenseMap});
                        dispatcher.dispatch(new FetchArticleCommentsSuccessAction {
                            channelId = channelId,
                            itemIds = itemIds,
                            messageItems = messageItems,
                            isRefreshList = false,
                            hasMore = responseComments.hasMore,
                            currOldestMessageId = responseComments.currOldestMessageId
                        });
                    })
                    .Catch(onRejected: Debuger.LogError);
            });
        }

        public static object FetchArticleDetail(string articleId, bool isPush = false) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.FetchArticleDetail(articleId: articleId, isPush: isPush)
                    .Then(articleDetailResponse => {
                        var itemIds = new List<string>();
                        var messageItems = new Dictionary<string, Message>();
                        var userMap = new Dictionary<string, User>();
                        articleDetailResponse.project.comments.items.ForEach(message => {
                            itemIds.Add(item: message.id);
                            messageItems[key: message.id] = message;
                            if (userMap.ContainsKey(key: message.author.id)) {
                                userMap[key: message.author.id] = message.author;
                            }
                            else {
                                userMap.Add(key: message.author.id, value: message.author);
                            }
                        });
                        articleDetailResponse.project.comments.parents.ForEach(message => {
                            if (messageItems.ContainsKey(key: message.id)) {
                                messageItems[key: message.id] = message;
                            }
                            else {
                                messageItems.Add(key: message.id, value: message);
                            }

                            if (userMap.ContainsKey(key: message.author.id)) {
                                userMap[key: message.author.id] = message.author;
                            }
                            else {
                                userMap.Add(key: message.author.id, value: message.author);
                            }
                        });
                        articleDetailResponse.project.comments.uppers.ForEach(message => {
                            if (messageItems.ContainsKey(key: message.id)) {
                                messageItems[key: message.id] = message;
                            }
                            else {
                                messageItems.Add(key: message.id, value: message);
                            }

                            if (userMap.ContainsKey(key: message.author.id)) {
                                userMap[key: message.author.id] = message.author;
                            }
                            else {
                                userMap.Add(key: message.author.id, value: message.author);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = userMap});
                        dispatcher.dispatch(new UserLicenseMapAction
                            {userLicenseMap = articleDetailResponse.project.userLicenseMap});
                        dispatcher.dispatch(new FetchArticleCommentsSuccessAction {
                            channelId = articleDetailResponse.project.channelId,
                            itemIds = itemIds,
                            messageItems = messageItems,
                            isRefreshList = true,
                            hasMore = articleDetailResponse.project.comments.hasMore,
                            currOldestMessageId = articleDetailResponse.project.comments.currOldestMessageId
                        });

                        dispatcher.dispatch(new UserMapAction {
                            userMap = articleDetailResponse.project.userMap
                        });
                        dispatcher.dispatch(new UserMapAction {
                            userMap = articleDetailResponse.project.mentionUsers
                        });
                        dispatcher.dispatch(new TeamMapAction {
                            teamMap = articleDetailResponse.project.teamMap
                        });
                        dispatcher.dispatch(new FollowMapAction {followMap = articleDetailResponse.project.followMap});
                        dispatcher.dispatch(new FetchArticleDetailSuccessAction {
                            articleDetail = articleDetailResponse.project,
                            articleId = articleId
                        });
                        dispatcher.dispatch(new SaveArticleHistoryAction {
                            article = articleDetailResponse.project.projectData
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchArticleDetailFailureAction());
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object likeArticle(string articleId, int addCount = 1) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                dispatcher.dispatch(new LikeArticleSuccessAction {
                    articleId = articleId,
                    likeCount = addCount
                });
                return ArticleApi.LikeArticle(articleId: articleId, addCount: addCount)
                    .Then(() => { })
                    .Catch(_ => { });
            });
        }

        public static object favoriteArticle(string articleId, List<string> tagIds) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(
                child: new CustomLoadingDialog(
                    message: "操作中"
                )
            );
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.FavoriteArticle(articleId: articleId, tagIds: tagIds)
                    .Then(favoriteArticleResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        CustomDialogUtils.showToast("操作成功", iconData: Icons.sentiment_satisfied);
                        dispatcher.dispatch(new FavoriteArticleSuccessAction {
                            favorites = favoriteArticleResponse,
                            articleId = articleId
                        });
                        AnalyticsManager.AnalyticsFavoriteArticle(articleId: articleId, favoriteTagIds: tagIds);
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object unFavoriteArticle(string articleId, string favoriteId) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            CustomDialogUtils.showCustomDialog(
                child: new CustomLoadingDialog(
                    message: "取消收藏中"
                )
            );
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.UnFavoriteArticle(favoriteId: favoriteId)
                    .Then(unFavoriteArticleResponse => {
                        CustomDialogUtils.hiddenCustomDialog();
                        CustomDialogUtils.showToast("取消收藏成功", iconData: Icons.sentiment_satisfied);
                        dispatcher.dispatch(new UnFavoriteArticleSuccessAction {
                            favorite = unFavoriteArticleResponse,
                            articleId = articleId
                        });
                        AnalyticsManager.AnalyticsUnFavoriteArticle(favoriteId: favoriteId);
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        Debuger.LogError(message: error);
                    });
            });
        }

        public static object likeComment(Message message) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                CustomDialogUtils.showToast("点赞成功", iconData: Icons.sentiment_satisfied);
                dispatcher.dispatch(new LikeCommentSuccessAction {message = message});

                return ArticleApi.LikeComment(commentId: message.id)
                    .Then(mess => { })
                    .Catch(error => { });
            });
        }

        public static object removeLikeComment(Message message) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", iconData: Icons.sentiment_dissatisfied);
                return null;
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                CustomDialogUtils.showToast("已取消点赞", iconData: Icons.sentiment_satisfied);
                dispatcher.dispatch(new RemoveLikeCommentSuccessAction {message = message});
                return ArticleApi.RemoveLikeComment(commentId: message.id)
                    .Then(mess => { })
                    .Catch(error => { });
            });
        }

        public static object sendComment(string articleId, string channelId, string content, string nonce,
            string parentMessageId, string upperMessageId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.SendComment(
                        channelId: channelId,
                        content: content,
                        nonce: nonce,
                        parentMessageId: parentMessageId,
                        upperMessageId: upperMessageId
                    )
                    .Then(message => {
                        CustomDialogUtils.hiddenCustomDialog();
                        if (message.deleted) {
                            if (parentMessageId.isNotEmpty()) {
                                CustomDialogUtils.showToast("此条评论已被删除", iconData: Icons.sentiment_dissatisfied);
                            }
                        }
                        else {
                            CustomDialogUtils.showToast("发送成功", iconData: Icons.sentiment_satisfied);
                        }

                        dispatcher.dispatch(new SendCommentSuccessAction {
                            message = message,
                            articleId = articleId,
                            channelId = channelId,
                            parentMessageId = parentMessageId,
                            upperMessageId = upperMessageId
                        });
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        CustomDialogUtils.showToast("发送失败", iconData: Icons.sentiment_dissatisfied);
                    });
            });
        }
    }
}