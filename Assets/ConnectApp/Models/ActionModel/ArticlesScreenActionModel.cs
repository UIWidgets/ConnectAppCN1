using System;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ArticlesScreenActionModel : BaseActionModel {
        public Action pushToSearch;
        public Action<string> pushToArticleDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action startFetchArticles;
        public Func<int, IPromise> fetchArticles;
        public Func<IPromise> fetchReviewUrl;
    }
}