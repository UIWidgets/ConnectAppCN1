using System;
using ConnectApp.Components;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class FavoriteDetailScreenActionModel : BaseActionModel {
        public Action startFetchFavoriteDetail;
        public Func<string, int, IPromise> fetchFavoriteDetail;
        public Func<string, string, IPromise> unFavoriteArticle;
        public Action<string> pushToArticleDetail;
        public Action<string> pushToEditFavorite;
        public Action<string> pushToReport;
        public Action<string> pushToBlock;
        public Func<ShareType, string, string, string, string, IPromise> shareToWechat;
        public Func<string, string, IPromise> collectFavoriteTag;
        public Func<string, string, IPromise> cancelCollectFavoriteTag;
    }
}