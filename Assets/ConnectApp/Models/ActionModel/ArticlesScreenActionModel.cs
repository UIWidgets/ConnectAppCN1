using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ArticlesScreenActionModel {
        public Action pushToSearch;
        public Action<string> pushToArticleDetail;
        public Action startFetchArticles;
        public Func<int, IPromise> fetchArticles;
    }
}