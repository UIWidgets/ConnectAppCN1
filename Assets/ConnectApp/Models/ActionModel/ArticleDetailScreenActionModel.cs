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
        public Action<string> likeArticle;
        public Action<string> likeComment;
        public Action<string> removeLikeComment;
        public Action<string, string, string, string> sendComment;
    }
}