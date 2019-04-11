using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class NotificationScreenActionModel {
        public Action startFetchNotifications;
        public Func<int, IPromise> fetchNotifications;
        public Action<string> pushToArticleDetail;
    }
}