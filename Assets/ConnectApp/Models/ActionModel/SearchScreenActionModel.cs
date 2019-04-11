using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class SearchScreenActionModel {
        public Action mainRouterPop;
        public Action<string> pushToArticleDetail;
        public Action startSearchArticle;
        public Func<string, int, IPromise> searchArticle;
        public Func<IPromise> fetchPopularSearch;
        public Action clearSearchArticleResult;
        public Action<string> saveSearchHistory;
        public Action<string> deleteSearchHistory;
        public Action deleteAllSearchHistory;
    }
}