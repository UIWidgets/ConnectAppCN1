using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class NotificationScreenActionModel : BaseActionModel {
        public Action startFetchNotifications;
        public Func<int, IPromise> fetchNotifications;
        public Func<IPromise> fetchMakeAllSeen;
        public Action<string> pushToArticleDetail;
        public Action<string> pushToUserDetail;
        public Action<string> pushToTeamDetail;
    }
}