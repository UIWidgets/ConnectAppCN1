using System;

namespace ConnectApp.models {
    [Serializable]
    public class IEvent {
        public readonly string id;
        public User user;
        public string title;
        public string background;
        public int participantsCount;
        public int onlineMemberCount;
        public int recordWatchCount;
        public string createdTime;
        public string shortDescription;
        public bool live;
    }
}