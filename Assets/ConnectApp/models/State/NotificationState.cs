using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class NotificationState {
        public bool loading { get; set; }
        public int unreadCount { get; set; }
        public int unseenCount { get; set; }
        public string current { get; set; }
        public string next { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int pageTotal { get; set; }
        public List<NotificationResult> results { get; set; }
    }
}