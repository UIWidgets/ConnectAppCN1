using System;
using ConnectApp.components;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ArticleDetailScreenActionModel : BaseActionModel {
        public Action<string> openUrl;
        public Action<string> pushToArticleDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action startFetchArticleDetail;
        public Func<string, IPromise> fetchArticleDetail;
        public Func<string, string, IPromise> fetchArticleComments;
        public Func<string, IPromise> likeArticle;
        public Func<string, IPromise> likeComment;
        public Func<string, IPromise> removeLikeComment;
        public Func<string, string, string, string, IPromise> sendComment;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
    }
}