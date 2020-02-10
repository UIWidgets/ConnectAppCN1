using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class BloggerScreenActionModel : BaseActionModel {
        public Action<string> pushToUserDetail;
        public Action startFetchBlogger;
        public Func<int, IPromise> fetchBlogger;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
    }
}