using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class PersonalScreenActionModel {
        public Action<string> mainRouterPushTo;
        public Action pushToNotifications;
        public Action<string> pushToUserDetail;
        public Action<string, int> pushToUserFollowing;
        public Action<string> pushToUserFollower;
        public Action<string> pushToUserLike;
        public Func<string, IPromise> fetchUserProfile;
    }
}