using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class NotificationState {
        public bool loading { get; set; }
        public int page { get; set; }
        public int pageTotal { get; set; }
        public List<Notification> notifications { get; set; }
        public List<User> mentions { get; set; }
    }
}