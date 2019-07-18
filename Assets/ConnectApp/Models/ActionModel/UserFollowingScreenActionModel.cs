using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class UserFollowingScreenActionModel : BaseActionModel  {
        public Action<string> pushToUserDetail;
        public Action<string> pushToTeamDetail;
        public Action startFetchFollowingUser;
        public Action startFetchFollowingTeam;
        public Func<int, IPromise> fetchFollowingUser;
        public Func<int, IPromise> fetchFollowingTeam;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
        public Action<string> startFollowTeam;
        public Func<string, IPromise> followTeam;
        public Action<string> startUnFollowTeam;
        public Func<string, IPromise> unFollowTeam;
        public Action startSearchFollowingUser;
        public Func<string, int, IPromise> searchFollowingUser;
        public Action clearSearchFollowingResult;
    }
}