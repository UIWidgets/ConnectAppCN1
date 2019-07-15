using System;
using ConnectApp.Components;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ArticlesScreenActionModel : BaseActionModel {
        public Action pushToSearch;
        public Action<string> pushToArticleDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Action startFetchArticles;
        public Func<int, IPromise> fetchArticles;
        public Func<IPromise> fetchReviewUrl;
    }
}