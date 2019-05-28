using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class NotificationState {
        public bool loading { get; set; }
        public int pageTotal { get; set; }
        public List<Notification> notifications { get; set; }
    }
}