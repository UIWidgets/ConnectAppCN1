using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    public enum EventType {
        onLine,
        offline
    }

    [Serializable]
    public class EventState {
        public bool eventsLoading { get; set; }
        public List<string> events { get; set; }
        public Dictionary<string, IEvent> eventDict { get; set; }
        public List<string> completedEvents { get; set; }
        public Dictionary<string, IEvent> completedEventDict { get; set; }
        public List<IEvent> eventHistory { get; set; }
        public bool eventDetailLoading { get; set; }
        public bool joinEventLoading { get; set; }
        public string detailId { get; set; }
        public EventType eventType { get; set; }
        public bool showChatWindow { get; set; }
        public bool openChatWindow { get; set; }
    }
}