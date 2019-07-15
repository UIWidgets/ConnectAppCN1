using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Utils;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class StartFetchArticlesAction : RequestAction {
    }

    public class FetchArticleSuccessAction : BaseAction {
        public List<Article> articleList;
        public bool hottestHasMore;
        public int offset;
    }

    public class FetchArticleFailureAction : BaseAction {
    }

    public class StartFetchArticleDetailAction : RequestAction {
    }

    public class FetchArticleDetailSuccessAction : BaseAction {
        public Project articleDetail;
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

    public class StartFetchArticleCommentsAction : RequestAction {
    }

    public class FetchArticleCommentsSuccessAction : BaseAction {
        public string channelId;
        public List<string> itemIds;
        public Dictionary<string, Message> messageItems;
        public string currOldestMessageId;
        public bool hasMore;
        public bool isRefreshList;
    }

    public class LikeArticleAction : RequestAction {
        public string articleId;
    }

    public class LikeArticleSuccessAction : BaseAction {
        public string articleId;
    }

    public class BlockArticleAction : RequestAction {
        public string articleId;
    }

    public class StartLikeCommentAction : RequestAction {
    }

    public class LikeCommentSuccessAction : BaseAction {
        public Message message;
    }

    public class LikeCommentFailureAction : BaseAction {
    }

    public class StartRemoveLikeCommentAction : RequestAction {
        public string messageId;
    }

    public class RemoveLikeCommentSuccessAction : BaseAction {
        public Message message;
    }

    public class StartSendCommentAction : RequestAction {
        public string channelId;
        public string content;
        public string nonce;
        public string parentMessageId = "";
    }

    public class SendCommentSuccessAction : BaseAction {
        public Message message;
        public string articleId;
    }

    public static partial class Actions {
        public static object fetchArticles(int offset) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.FetchArticles(offset)
                    .Then(articlesResponse => {
                        var articleList = new List<Article>();
                        articlesResponse.hottests.ForEach(item => {
                            if (articlesResponse.projectMap.ContainsKey(item.itemId)) {
                                var article = articlesResponse.projectMap[item.itemId];
                                articleList.Add(article);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {userMap = articlesResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = articlesResponse.teamMap});
                        dispatcher.dispatch(new FetchArticleSuccessAction {
                            offset = offset,
                            hottestHasMore = articlesResponse.hottestHasMore,
                            articleList = articleList
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchArticleFailureAction());
                        Debug.Log(error);
                    });
            });
        }

        public static object fetchArticleComments(string channelId, string currOldestMessageId = "") {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.FetchArticleComments(channelId, currOldestMessageId)
                    .Then(responseComments => {
                        var itemIds = new List<string>();
                        var messageItems = new Dictionary<string, Message>();
                        var userMap = new Dictionary<string, User>();
                        responseComments.items.ForEach(message => {
                            itemIds.Add(message.id);
                            messageItems[message.id] = message;
                            if (userMap.ContainsKey(message.author.id)) {
                                userMap[message.author.id] = message.author;
                            }
                            else {
                                userMap.Add(message.author.id, message.author);
                            }
                        });
                        dispatcher.dispatch(new UserMapAction {
                            userMap = userMap
                        });
                        var hasMore = responseComments.hasMore;
                        var lastCommentId = responseComments.currOldestMessageId;
                        dispatcher.dispatch(new FetchArticleCommentsSuccessAction {
                            channelId = channelId,
                            itemIds = itemIds,
                            messageItems = messageItems,
                            isRefreshList = false,
                            hasMore = hasMore,
                            currOldestMessageId = lastCommentId
                        });
                    })
                    .Catch(Debug.Log);
            });
        }

        public static object FetchArticleDetail(string articleId, bool isPush = false) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.FetchArticleDetail(articleId, isPush)
                    .Then(articleDetailResponse => {
                        if (articleDetailResponse.project.comments.items.Count > 0) {
                            var itemIds = new List<string>();
                            var messageItems = new Dictionary<string, Message>();
                            var userMap = new Dictionary<string, User>();
                            articleDetailResponse.project.comments.items.ForEach(message => {
                                itemIds.Add(message.id);
                                messageItems[message.id] = message;
                                if (userMap.ContainsKey(message.author.id)) {
                                    userMap[message.author.id] = message.author;
                                }
                                else {
                                    userMap.Add(message.author.id, message.author);
                                }
                            });
                            dispatcher.dispatch(new UserMapAction {
                                userMap = userMap
                            });
                            dispatcher.dispatch(new FetchArticleCommentsSuccessAction {
                                channelId = articleDetailResponse.project.channelId,
                                itemIds = itemIds,
                                messageItems = messageItems,
                                isRefreshList = true,
                                hasMore = articleDetailResponse.project.comments.hasMore,
                                currOldestMessageId = articleDetailResponse.project.comments.currOldestMessageId
                            });
                        }

                        dispatcher.dispatch(new UserMapAction {
                            userMap = articleDetailResponse.project.userMap
                        });
                        dispatcher.dispatch(new TeamMapAction {
                            teamMap = articleDetailResponse.project.teamMap
                        });
                        dispatcher.dispatch(new FetchArticleDetailSuccessAction {
                            articleDetail = articleDetailResponse.project
                        });
                        dispatcher.dispatch(new SaveArticleHistoryAction {
                            article = articleDetailResponse.project.projectData
                        });
                    })
                    .Catch(error => {
//                        dispatcher.dispatch(new FetchArticleDetailFailureAction());
                        Debug.Log(error);
                    });
            });
        }

        public static object likeArticle(string articleId) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", Icons.sentiment_dissatisfied);
                return null;
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                CustomDialogUtils.showToast("点赞成功", Icons.sentiment_satisfied);
                dispatcher.dispatch(new LikeArticleSuccessAction {articleId = articleId});
                return ArticleApi.LikeArticle(articleId)
                    .Then(() => { })
                    .Catch(_ => { });
            });
        }

        public static object likeComment(Message message) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", Icons.sentiment_dissatisfied);
                return null;
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                CustomDialogUtils.showToast("点赞成功", Icons.sentiment_satisfied);
                dispatcher.dispatch(new LikeCommentSuccessAction {message = message});

                return ArticleApi.LikeComment(message.id)
                    .Then(mess => { })
                    .Catch(error => { });
            });
        }

        public static object removeLikeComment(Message message) {
            if (HttpManager.isNetWorkError()) {
                CustomDialogUtils.showToast("请检查网络", Icons.sentiment_dissatisfied);
                return null;
            }

            return new ThunkAction<AppState>((dispatcher, getState) => {
                CustomDialogUtils.showToast("已取消点赞", Icons.sentiment_satisfied);
                dispatcher.dispatch(new RemoveLikeCommentSuccessAction {message = message});
                return ArticleApi.RemoveLikeComment(message.id)
                    .Then(mess => { })
                    .Catch(error => { });
            });
        }

        public static object sendComment(string articleId, string channelId, string content, string nonce,
            string parentMessageId) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ArticleApi.SendComment(channelId, content, nonce, parentMessageId)
                    .Then(message => {
                        CustomDialogUtils.hiddenCustomDialog();
                        CustomDialogUtils.showToast("发送成功", Icons.sentiment_satisfied);
                        dispatcher.dispatch(new SendCommentSuccessAction {
                            message = message,
                            articleId = articleId
                        });
                    })
                    .Catch(error => {
                        CustomDialogUtils.hiddenCustomDialog();
                        CustomDialogUtils.showToast("发送失败", Icons.sentiment_dissatisfied);
                    });
            });
        }
    }
}