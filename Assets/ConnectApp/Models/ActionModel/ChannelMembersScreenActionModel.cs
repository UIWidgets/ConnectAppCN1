using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ChannelMembersScreenActionModel : BaseActionModel {
        public Action fetchMembers;
        public Action<string> startFollowUser;
        public Func<string, IPromise> followUser;
        public Action<string> startUnFollowUser;
        public Func<string, IPromise> unFollowUser;
    }
}