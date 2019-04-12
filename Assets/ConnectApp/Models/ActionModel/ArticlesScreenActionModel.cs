using System;
using RSG;
using ConnectApp.screens;

namespace ConnectApp.Models.ActionModel {
    public class ArticlesScreenActionModel {
        public Action pushToSearch;
        public Action<string> pushToArticleDetail;
        public Action<string, ReportType> pushToReport;
        public Action startFetchArticles;
        public Func<int, IPromise> fetchArticles;
    }
}