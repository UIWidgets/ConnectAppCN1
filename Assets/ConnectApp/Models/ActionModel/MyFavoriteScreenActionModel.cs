using System;
using System.Collections.Generic;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class MyFavoriteScreenActionModel : BaseActionModel {
        public Action startFetchMyFavorite;
        public Func<int, IPromise> fetchMyFavorite;
        public Func<List<string>, IPromise> favoriteArticle;
        public Func<string, IPromise> deleteFavoriteTag;
        public Action<string, string> pushToFavoriteDetail;
        public Action<string> pushToCreateFavorite;
    }
}