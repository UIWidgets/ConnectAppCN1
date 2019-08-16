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
        public Action<string> pushToUserFollowing;
        public Action<string> pushToUserDetail;
        public Action<string> pushToTeamDetail;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
        public Action<string> startFollowTeam;
        public Func<string, IPromise> followTeam;
        public Action<string> startUnFollowTeam;
        public Func<string, IPromise> unFollowTeam;
        public Func<string, string, string, string, string, IPromise> sendComment;
        public Func<string, IPromise> likeArticle;
        public Action startFetchArticles;
        public Func<int, IPromise> fetchArticles;
        public Action startFetchFollowing;
        public Func<int, IPromise> fetchFollowing;
        public Action startFetchFollowArticles;
        public Func<int, IPromise> fetchFollowArticles;
        public Func<IPromise> fetchReviewUrl;
        
        public Action pushToReality;
    }
}