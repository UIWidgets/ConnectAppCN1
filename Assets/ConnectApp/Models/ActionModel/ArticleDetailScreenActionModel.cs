using System;
using ConnectApp.Components;
using ConnectApp.Models.Model;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ArticleDetailScreenActionModel : BaseActionModel {
        public Action<string> openUrl;
        public Action<string, bool, int> playVideo;
        public Action<string> browserImage;
        public Action<string> pushToArticleDetail;
        public Action<string> pushToUserDetail;
        public Action<string> pushToTeamDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action<string> blockUser;
        public Action startFetchArticleDetail;
        public Func<string, IPromise> fetchArticleDetail;
        public Func<string, string, IPromise> fetchArticleComments;
        public Func<string, int, IPromise> likeArticle;
        public Func<Message, IPromise> likeComment;
        public Func<Message, IPromise> removeLikeComment;
        public Func<string, string, string, string, string, IPromise> sendComment;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
        public Action<string> startFollowTeam;
        public Func<string, IPromise> followTeam;
        public Action<string> startUnFollowTeam;
        public Func<string, IPromise> unFollowTeam;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
    }
}