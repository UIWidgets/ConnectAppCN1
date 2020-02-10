using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class LeaderBoardScreenActionModel : BaseActionModel {
        public Action<string> pushToDetailAction;
        public Action<string> pushToUserDetail;
        public Action startFetchCollection;
        public Func<int, IPromise> fetchCollection;
        public Action startFetchColumn;
        public Func<int, IPromise> fetchColumn;
        public Action startFetchBlogger;
        public Func<int, IPromise> fetchBlogger;
    }
}