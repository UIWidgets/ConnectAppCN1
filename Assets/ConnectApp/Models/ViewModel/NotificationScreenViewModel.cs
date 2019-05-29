using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class NotificationScreenViewModel {
        public bool notificationLoading;
        public int pageTotal;
        public List<Notification> notifications;
        public Dictionary<string, User> userDict;
    }
}