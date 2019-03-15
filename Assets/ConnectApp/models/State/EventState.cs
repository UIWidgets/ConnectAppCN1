using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    public enum EventType {
        onLine,
        Offline
    }
    
    [Serializable]
    public class EventState {
        public bool eventsLoading { get; set; }
        public List<string> events { get; set; }
        public Dictionary<string, IEvent> eventDict { get; set; }

        public bool eventDetailLoading { get; set; }
        public string detailId { get; set; }
        public EventType eventType { get; set; }
        public bool showChatWindow { get; set; }
        public bool openChatWindow { get; set; }
    }
}