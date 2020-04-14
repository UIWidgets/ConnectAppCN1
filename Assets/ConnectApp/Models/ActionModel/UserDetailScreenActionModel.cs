using System;
using ConnectApp.Components;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class UserDetailScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Action<string, string> pushToFavoriteDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action<string, int> pushToUserFollowing;
        public Action<string> pushToUserFollower;
        public Action<string> pushToEditPersonalInfo;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Action startFetchUserProfile;
        public Func<IPromise> fetchUserProfile;
        public Action startFetchUserArticle;
        public Func<string, int, IPromise> fetchUserArticle;
        public Action startFetchUserFavorite;
        public Func<string, int, IPromise> fetchUserFavorite;
        public Action startFetchUserFollowFavorite;
        public Func<string, int, IPromise> fetchUserFollowFavorite;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
        public Func<string, IPromise> deleteFavoriteTag;
        public Action<string> pushToCreateFavorite;
        public Action<bool> blockUserAction;
    }
}