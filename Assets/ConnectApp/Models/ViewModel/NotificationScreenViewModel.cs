using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel {
    public class NotificationScreenViewModel {
        public bool notificationLoading;
        public int total;
        public List<Notification> notifications;
        public Dictionary<string, User> userDict;
    }
}