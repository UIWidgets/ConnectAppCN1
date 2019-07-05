using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class FollowingUserScreenActionModel : BaseActionModel  {
        public Action<string> pushToPersonalDetail;
        public Action startFetchFollowing;
        public Func<int, IPromise> fetchFollowing;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
        public Action startSearchFollowing;
        public Func<string, int, IPromise> searchFollowing;
        public Action clearSearchFollowingResult;
    }
}