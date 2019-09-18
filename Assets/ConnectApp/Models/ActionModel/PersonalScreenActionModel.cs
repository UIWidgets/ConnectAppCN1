using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class PersonalScreenActionModel {
        public Action<string> mainRouterPushTo;
        public Action<string> pushToUserDetail;
        public Func<string, IPromise> fetchUserProfile;
    }
}