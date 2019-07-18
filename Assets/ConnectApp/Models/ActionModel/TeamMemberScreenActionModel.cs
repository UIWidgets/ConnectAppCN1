using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class TeamMemberScreenActionModel : BaseActionModel {
        public Action<string> pushToUserDetail;
        public Action startFetchMember;
        public Func<int, IPromise> fetchMember;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
    }
}