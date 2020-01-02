using System;
using ConnectApp.Components;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class UserLikeArticleScreenActionModel : BaseActionModel {
        public Action startFetchUserLikeArticle;
        public Func<int, IPromise> fetchUserLikeArticle;
        public Action<string> pushToArticleDetail;
        public Action<string> pushToReport;
        public Action<string> pushToBlock;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
    }
}