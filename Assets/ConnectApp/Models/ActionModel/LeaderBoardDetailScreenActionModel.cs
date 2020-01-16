using System;
using ConnectApp.Components;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class LeaderBoardDetailScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Action startFetchDetailList;
        public Func<int, IPromise> fetchDetailList;
        public Func<string, string, IPromise> collectFavoriteTag;
        public Func<string, string, IPromise> cancelCollectFavoriteTag;
        public Action<string> startFollowUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> followUser;
        public Func<string, IPromise> unFollowUser;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
    }
}