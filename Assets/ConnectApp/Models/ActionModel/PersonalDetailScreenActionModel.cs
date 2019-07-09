using System;
using ConnectApp.Components;
using ConnectApp.screens;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class PersonalDetailScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Action<string, ReportType> pushToReport;
        public Action<string> pushToBlock;
        public Action<string> pushToFollowingUser;
        public Action<string> pushToFollowerUser;
        public Action<string> pushToEditPersonalInfo;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Action startFetchPersonal;
        public Func<IPromise> fetchPersonal;
        public Func<int, IPromise> fetchPersonalArticle;
        public Action startFollowUser;
        public Func<IPromise> followUser;
        public Action startUnFollowUser;
        public Func<IPromise> unFollowUser;
    }
}