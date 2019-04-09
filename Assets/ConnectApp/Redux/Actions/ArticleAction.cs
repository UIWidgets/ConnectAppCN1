using System.Collections.Generic;
using System.Diagnostics;
using ConnectApp.api;
using ConnectApp.models;
using RSG;
using Unity.UIWidgets.Redux;
using Debug = UnityEngine.Debug;

namespace ConnectApp.redux.actions {
    public class FetchArticlesAction : RequestAction {
        public int pageNumber = 0;
    }

    public class FetchArticleSuccessAction : BaseAction {
        public int pageNumber = 0;
        public List<Article> articleList;
        public int total;
    }
    public class FetchArticleFailureAction : BaseAction {
    }

    public class FetchArticleDetailAction : RequestAction {
        public string articleId;
    }

    public class FetchArticleDetailSuccessAction : BaseAction {
        public Project articleDetail;
    }
    
    public class FetchArticleDetailFailureAction : BaseAction {}

    public class GetArticleHistoryAction : BaseAction {
    }

    public class SaveArticleHistoryAction : BaseAction {
        public Article article;
    }

    public class DeleteArticleHistoryAction : BaseAction {
        public string articleId;
    }

    public class DeleteAllArticleHistoryAction : BaseAction {
    }

    public class FetchArticleCommentsAction : RequestAction {
        public string channelId;
        public string currOldestMessageId = "";
    }

    public class FetchArticleCommentsSuccessAction : BaseAction {
        public string channelId;
        public FetchCommentsResponse commentsResponse;
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

    public class LikeCommentAction : RequestAction {
        public string messageId;
    }

    public class LikeCommentSuccessAction : BaseAction {
        public Message message;
    }

    public class RemoveLikeCommentAction : RequestAction {
        public string messageId;
    }

    public class RemoveLikeSuccessAction : BaseAction {
        public Message message;
    }

    public class SendCommentAction : RequestAction {
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
            return new ThunkAction<AppState>((dispatcher, getState) => {                
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
                        var hasMore = responseComments.hasMore;
                        var lastCommentId = responseComments.currOldestMessageId;
                        dispatcher.dispatch(new FetchArticleCommentsSuccessAction {
                            channelId = channelId,
                            commentsResponse = responseComments,
                            isRefreshList = false,
                            hasMore = hasMore,
                            currOldestMessageId = lastCommentId
                        });
                    })
                    .Catch(Debug.Log);
            });
        }
        
        public static object fetchArticleDetatil(string articleId)
        {
            return new ThunkAction<AppState>((dispatcher, getState) => {                
                return ArticleApi.FetchArticleDetail(articleId)
                    .Then(articleDetailResponse => {
                        if (articleDetailResponse.project.comments.items.Count > 0)
                            dispatcher.dispatch(new FetchArticleCommentsSuccessAction {
                                channelId = articleDetailResponse.project.channelId,
                                commentsResponse = articleDetailResponse.project.comments,
                                isRefreshList = true
                            });
                        dispatcher.dispatch(new UserMapAction {
                            userMap = articleDetailResponse.project.userMap
                        });
                        dispatcher.dispatch(new TeamMapAction {
                            teamMap = articleDetailResponse.project.teamMap
                        });
                        dispatcher.dispatch(new FetchArticleDetailSuccessAction {
                            articleDetail = articleDetailResponse.project
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchArticleDetailFailureAction()); 
                        Debug.Log(error);
                    });
            });
        }
    }
}