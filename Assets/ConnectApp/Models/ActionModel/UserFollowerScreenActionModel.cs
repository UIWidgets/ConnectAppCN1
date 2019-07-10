using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class UserFollowerScreenActionModel : BaseActionModel {
        public Action<string> pushToUserDetail;
        public Action startFetchFollower;
        public Func<int, IPromise> fetchFollower;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
    }
}