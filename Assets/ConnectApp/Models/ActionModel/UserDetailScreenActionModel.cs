using System;
using ConnectApp.Components;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class UserDetailScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action<string, int> pushToUserFollowing;
        public Action<string> pushToUserFollower;
        public Action<string> pushToEditPersonalInfo;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Action startFetchUserProfile;
        public Func<IPromise> fetchUserProfile;
        public Action startFetchUserArticle;
        public Func<int, IPromise> fetchUserArticle;
        public Action startFollowUser;
        public Func<IPromise> followUser;
        public Action startUnFollowUser;
        public Func<IPromise> unFollowUser;
    }
}