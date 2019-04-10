using System.Collections.Generic;
using System.Diagnostics;
using ConnectApp.api;
using ConnectApp.models;
using RSG;
using Unity.UIWidgets.Redux;
using Debug = UnityEngine.Debug;

namespace ConnectApp.redux.actions {
    public class StartFetchArticlesAction : RequestAction {
        public int pageNumber = 0;
    }

    public class FetchArticleSuccessAction : BaseAction {
        public int pageNumber = 0;
        public List<Article> articleList;
        public int total;
    }
    public class FetchArticleFailureAction : BaseAction {}

    public class StartFetchArticleDetailAction : RequestAction {}

    public class FetchArticleDetailSuccessAction : BaseAction {
        public Project articleDetail;
    }
    
    public class FetchArticleDetailFailureAction : BaseAction {}

    public class SaveArticleHistoryAction : BaseAction {
        public Article article;
    }

    public class DeleteArticleHistoryAction : BaseAction {
        public string articleId;
    }

    public class DeleteAllArticleHistoryAction : BaseAction {
    }

    public class StartFetchArticleCommentsAction : RequestAction {
        public string channelId;
        public string currOldestMessageId = "";
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

    public class StartLikeCommentAction : RequestAction {
        public string messageId;
    }

    public class LikeCommentSuccessAction : BaseAction {
        public Message message;
    }
    
    public class LikeCommentFailureAction : BaseAction {}

    public class StartRemoveLikeCommentAction : RequestAction {
        public string messageId;
    }

    public class RemoveLikeSuccessAction : BaseAction {
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
    }

    public static partial class Actions {
        
        public static object fetchArticles(int pageNumber)
        {
            return new ThunkAction<AppState>((dispatcher, getState) =>
            {
                return ArticleApi.FetchArticles(pageNumber)
                    .Then(articlesResponse => {
                        dispatcher.dispatch(new UserMapAction {userMap = articlesResponse.userMap});
                        dispatcher.dispatch(new TeamMapAction {teamMap = articlesResponse.teamMap});
                        dispatcher.dispatch(new FetchArticleSuccessAction {
                            pageNumber = pageNumber, 
                            articleList = articlesResponse.items,
                            total = articlesResponse.total
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
                            if (userMap.ContainsKey(message.author.id))
                                userMap[message.author.id] = message.author;
                            else
                                userMap.Add(message.author.id, message.author);
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
        
        public static object FetchArticleDetail(string articleId)
        {
            return new ThunkAction<AppState>((dispatcher, getState) => {                
                return ArticleApi.FetchArticleDetail(articleId)
                    .Then(articleDetailResponse => {
                        if (articleDetailResponse.project.comments.items.Count > 0) {
                            var itemIds = new List<string>();
                            var messageItems = new Dictionary<string, Message>();
                            var userMap = new Dictionary<string, User>();
                            articleDetailResponse.project.comments.items.ForEach(message => {
                                itemIds.Add(message.id);
                                messageItems[message.id] = message;
                                if (userMap.ContainsKey(message.author.id))
                                    userMap[message.author.id] = message.author;
                                else
                                    userMap.Add(message.author.id, message.author);
                            });
                            dispatcher.dispatch(new UserMapAction {
                                userMap = userMap
                            });
                            dispatcher.dispatch(new FetchArticleCommentsSuccessAction {
                                channelId = articleDetailResponse.project.channelId,
                                itemIds = itemIds,
                                messageItems = messageItems,
                                isRefreshList = true
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
                        dispatcher.dispatch(new FetchArticleDetailFailureAction()); 
                        Debug.Log(error);
                    });
            });
        }
        
        public static object likeArticle(string articleId)
        {
            return new ThunkAction<AppState>((dispatcher, getState) =>
            {
                return ArticleApi.LikeArticle(articleId)
                    .Then(() => {
                        dispatcher.dispatch(new LikeArticleSuccessAction {articleId = articleId});
                    })
                    .Catch(Debug.Log);
            });
        }
        
        public static object likeComment(string messageId)
        {
            return new ThunkAction<AppState>((dispatcher, getState) =>
            {
                return ArticleApi.LikeComment(messageId)
                    .Then((message) => {
                        dispatcher.dispatch(new LikeCommentSuccessAction {message = message});
                    })
                    .Catch(error => { Debug.Log(error); });
            });
        }
        
        public static object removeLikeComment(string messageId)
        {
            return new ThunkAction<AppState>((dispatcher, getState) =>
            {
                return ArticleApi.RemoveLikeComment(messageId)
                    .Then((message) => {
                        dispatcher.dispatch(new RemoveLikeSuccessAction() {message = message});
                    })
                    .Catch(error => { Debug.Log(error); });
            });
        }
        
        public static object sendComment(string channelId, string content, string nonce, string parentMessageId)
        {
            return new ThunkAction<AppState>((dispatcher, getState) =>
            {
                return ArticleApi.SendComment(channelId, content, nonce, parentMessageId)
                    .Then((message) => {
                        dispatcher.dispatch(new SendCommentSuccessAction {
                            message = message
                        });
                    })
                    .Catch(error => { Debug.Log(error); });
            });
        }
    }
}