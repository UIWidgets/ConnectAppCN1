using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
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
        public bool? isPublic;
        public string mode;
        public TimeMap begin;
        public string placeId;
        public string place;
        public string address;
        public int? participantsCount;
        public int? onlineMemberCount;
        public int? recordWatchCount;
        public DateTime? createdTime;
        public string shortDescription;
        public bool? userIsCheckedIn;
        public string channelId;
        public List<User> hosts;
        public string content;
        public Dictionary<string, ContentMap> contentMap;
        public bool? isNotFirst; //加载详情后 置为true
        public string record;
        public float recordDuration;
        public Dictionary<string, UserLicense> userLicenseMap;

        IEvent copyWith(
            string id = null,
            User user = null,
            string userId = null,
            string title = null,
            string avatar = null,
            string background = null,
            string type = null,
            string typeParam = null,
            string mode = null,
            TimeMap begin = null,
            string placeId = null,
            string place = null,
            string address = null,
            int? participantsCount = null,
            int? onlineMemberCount = null,
            int? recordWatchCount = null,
            DateTime? createdTime = null,
            string shortDescription = null,
            bool? userIsCheckedIn = null,
            string channelId = null,
            List<User> hosts = null,
            string content = null,
            Dictionary<string, ContentMap> contentMap = null,
            bool? isNotFirst = null,
            string record = null,
            float? recordDuration = null,
            Dictionary<string, UserLicense> userLicenseMap = null
        ) {
            return new IEvent {
                id = id ?? this.id,
                user = user ?? this.user,
                userId = userId ?? this.userId,
                title = title ?? this.title,
                avatar = avatar ?? this.avatar,
                background = background ?? this.background,
                type = type ?? this.type,
                typeParam = typeParam ?? this.typeParam,
                mode = mode ?? this.mode,
                begin = begin ?? this.begin,
                placeId = placeId ?? this.placeId,
                place = place ?? this.place,
                address = address ?? this.address,
                participantsCount = participantsCount ?? this.participantsCount,
                onlineMemberCount = onlineMemberCount ?? this.onlineMemberCount,
                recordWatchCount = recordWatchCount ?? this.recordWatchCount,
                createdTime = createdTime ?? this.createdTime,
                shortDescription = shortDescription ?? this.shortDescription,
                userIsCheckedIn = userIsCheckedIn ?? this.userIsCheckedIn,
                channelId = channelId ?? this.channelId,
                hosts = hosts ?? this.hosts,
                content = content ?? this.content,
                contentMap = contentMap ?? this.contentMap,
                isNotFirst = isNotFirst ?? this.isNotFirst,
                record = record ?? this.record,
                recordDuration = recordDuration ?? this.recordDuration,
                userLicenseMap = userLicenseMap ?? this.userLicenseMap
            };
        }

        public IEvent Merge(IEvent other) {
            if (null == other) {
                return this;
            }

            return this.copyWith(
                id: other.id,
                user: other.user,
                userId: other.userId,
                title: other.title,
                avatar: other.avatar,
                background: other.background,
                type: other.type,
                typeParam: other.typeParam,
                mode: other.mode,
                begin: other.begin,
                placeId: other.placeId,
                place: other.place,
                address: other.address,
                participantsCount: other.participantsCount,
                onlineMemberCount: other.onlineMemberCount,
                recordWatchCount: other.recordWatchCount,
                createdTime: other.createdTime,
                shortDescription: other.shortDescription,
                userIsCheckedIn: other.userIsCheckedIn,
                channelId: other.channelId,
                hosts: other.hosts,
                content: other.content,
                contentMap: other.contentMap,
                isNotFirst: other.isNotFirst,
                record: other.record,
                recordDuration: other.recordDuration,
                userLicenseMap: other.userLicenseMap
            );
        }
    }

    [Serializable]
    public class TimeMap {
        public string startTime;
        public string endTime;
    }
}