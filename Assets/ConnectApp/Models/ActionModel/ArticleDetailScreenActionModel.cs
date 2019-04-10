using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ArticleDetailScreenActionModel {
        public Action mainRouterPop;
        public Action pushToLogin;
        public Action<string> pushToArticleDetail;
        public Action startFetchArticleDetail;
        public Func<string, IPromise> fetchArticleDetail;
        public Func<string, string, IPromise> fetchArticleComments;
        public Func<string, IPromise> likeArticle;
        public Func<string, IPromise> likeComment;
        public Func<string, IPromise> removeLikeComment;
        public Func<string, string, string, string, IPromise> sendComment;
    }
}