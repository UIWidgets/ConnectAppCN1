using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;
using ConnectApp.Utils;

namespace ConnectApp.Models.ViewModel {
    public class MessageScreenViewModel {
        public bool notificationLoading;
        public int page;
        public int pageTotal;
        public List<Notification> notifications;
        public List<User> mentions;
        public Dictionary<string, User> userDict;
        public List<ChannelView> channelInfo;
        public List<ChannelView> popularChannelInfo;
        public List<ChannelView> discoverChannelInfo;
    }

    public class ChannelView {
        public string id;
        public string groupId;
        public string thumbnail;
        public string name;
        public string topic;
        public int memberCount;
        public bool isMute;
        public bool live;
        public ChannelMessageView lastMessage;
        public List<ChannelMessageView> messages;
        public int unread = 0;
        public bool isTop = false;
        public bool joined = false;
        public bool atMe = false;
        public bool atAll = false;
        public List<User> members;
        public int numAdmins;

        public static ChannelView fromChannel(Channel channel) {
            return new ChannelView {
                atAll = channel?.lastMessage?.content?.Contains("@all") ?? false,
                members = new List<User>(),
                id = channel?.id,
                groupId = channel?.groupId,
                thumbnail = channel?.thumbnail,
                name = channel?.name,
                topic = channel?.topic,
                memberCount = channel?.memberCount ?? 0,
                isMute = channel?.isMute ?? false,
                live = channel?.live ?? false,
                lastMessage = ChannelMessageView.fromChannelMessage(channel?.lastMessage),
                messages = new List<ChannelMessageView>()
            };
        }
    }

    public enum ChannelMessageType {
        text,
        image,
        file
    }

    public class ChannelMessageView {
        public string id;
        public string nonce;
        public string channelId;
        public User author;
        public DateTime time;
        public string timeString;
        public ChannelMessageType type = ChannelMessageType.text;
        public string content;
        public long fileSize;

        public static ChannelMessageView fromChannelMessage(ChannelMessage message) {
            return new ChannelMessageView {
                id = message?.id,
                nonce = message?.nonce,
                channelId = message?.channelId,
                author = message?.author,
                content = message?.content,
                fileSize = 0,
                time = DateConvert.DateTimeFromNonce(message?.nonce),
                type = ChannelMessageType.text,
            };
        }
    }
}