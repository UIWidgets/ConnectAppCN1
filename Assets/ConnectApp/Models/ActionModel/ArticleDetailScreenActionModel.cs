using System;
using ConnectApp.Components;
using ConnectApp.Models.Model;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ArticleDetailScreenActionModel : BaseActionModel {
        public Action<string> openUrl;
        public Action<string> playVideo;
        public Action browserImage;
        public Action<string> pushToArticleDetail;
        public Action<string> pushToUserDetail;
        public Action<string> pushToTeamDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action startFetchArticleDetail;
        public Func<string, IPromise> fetchArticleDetail;
        public Func<string, string, IPromise> fetchArticleComments;
        public Func<string, IPromise> likeArticle;
        public Func<Message, IPromise> likeComment;
        public Func<Message, IPromise> removeLikeComment;
        public Func<string, string, string, string, IPromise> sendComment;
        public Action startFollowUser;
        public Func<string, IPromise> followUser;
        public Action startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
        public Action startFollowTeam;
        public Func<string, IPromise> followTeam;
        public Action startUnFollowTeam;
        public Func<string, IPromise> unFollowTeam;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
    }
}