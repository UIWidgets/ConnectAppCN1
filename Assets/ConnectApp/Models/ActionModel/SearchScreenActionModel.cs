using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class SearchScreenActionModel {
        public Action mainRouterPop;
        public Action<string> pushToArticleDetail;
        public Func<string, int, IPromise> searchArticle;
        public Action fetchPopularSearch;
        public Action clearSearchArticleResult;
        public Action<string> saveSearchHistory;
        public Action<string> deleteSearchHistory;
        public Action deleteAllSearchHistory;
    }
}