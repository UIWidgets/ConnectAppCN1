using System;
using ConnectApp.Components;
using ConnectApp.Models.Model;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ArticleDetailScreenActionModel : BaseActionModel {
        public Action<string> openUrl;
        public Action<string> playVideo;
        public Action<string> pushToArticleDetail;
        public Action<string> pushToPersonalDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action startFetchArticleDetail;
        public Func<string, IPromise> fetchArticleDetail;
        public Func<string, string, IPromise> fetchArticleComments;
        public Func<string, IPromise> likeArticle;
        public Func<Message, IPromise> likeComment;
        public Func<Message, IPromise> removeLikeComment;
        public Func<string, string, string, string, IPromise> sendComment;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
    }
}