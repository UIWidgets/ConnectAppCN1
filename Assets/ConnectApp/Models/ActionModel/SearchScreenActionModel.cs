using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class SearchScreenActionModel : BaseActionModel {
        public Action<string> pushToArticleDetail;
        public Action<string> pushToUserDetail;
        public Action<string> pushToTeamDetail;
        public Action<string> startSearchArticle;
        public Action startSearchUser;
        public Action startSearchTeam;
        public Func<string, int, IPromise> searchArticle;
        public Func<string, int, IPromise> searchUser;
        public Func<string, int, IPromise> searchTeam;
        public Func<IPromise> fetchPopularSearch;
        public Action clearSearchResult;
        public Action<string> saveSearchArticleHistory;
        public Action<string> deleteSearchArticleHistory;
        public Action deleteAllSearchArticleHistory;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
        public Action<string> startFollowTeam;
        public Func<string, IPromise> followTeam;
        public Action<string> startUnFollowTeam;
        public Func<string, IPromise> unFollowTeam;
    }
}