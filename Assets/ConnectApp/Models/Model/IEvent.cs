using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class IEvent {
        public string id;
        public User user;
        public string userId;
        public string title;
        public string avatar;
        public string background;
        public string type;
        public string typeParam;
        public bool isPublic;
        public string mode;
        public TimeMap begin;
        public string placeId;
        public string place;
        public string address;
        public int participantsCount;
        public int onlineMemberCount;
        public int recordWatchCount;
        public DateTime createdTime;
        public string shortDescription;
        public bool userIsCheckedIn;
        public string channelId;
        public List<User> hosts;
        public string content;
        public Dictionary<string, ContentMap> contentMap;
        public bool isNotFirst; //加载详情后 置位true
        public string record;
        public float recordDuration;
    }

    [Serializable]
    public class TimeMap {
        public string startTime;
        public string endTime;
    }
}