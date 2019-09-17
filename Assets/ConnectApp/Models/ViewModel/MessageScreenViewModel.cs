using System;
using System.Collections.Generic;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Models.ViewModel {
    public class MessageScreenViewModel {
        public bool notificationLoading;
        public int page;
        public int pageTotal;
        public List<Notification> notifications;
        public List<User> mentions;
        public Dictionary<string, User> userDict;
        public List<ChannelView> joinedChannels;
        public List<ChannelView> popularChannels;
        public List<ChannelView> publicChannels;
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
        public string lastMessageId;
        public ChannelMessageView lastMessage;
        public List<string> messageIds;
        public int unread = 0;
        public bool isTop = false;
        public bool joined = false;
        public bool atMe = false;
        public bool atAll = false;
        public bool hasMore = false;
        public bool hasMoreNew = false;
        public List<string> memberIds;

        public static ChannelView fromChannel(Channel channel) {
            return new ChannelView {
                atAll = channel?.lastMessage?.mentionEveryone ?? false,
                memberIds = new List<string>(),
                id = channel?.id,
                groupId = channel?.groupId,
                thumbnail = channel?.thumbnail,
                name = channel?.name,
                topic = channel?.topic,
                memberCount = channel?.memberCount ?? 0,
                isMute = channel?.isMute ?? false,
                live = channel?.live ?? false,
                lastMessageId = channel?.lastMessage?.id,
                lastMessage = ChannelMessageView.fromChannelMessage(channel?.lastMessage),
                messageIds = new List<string>()
            };
        }

        public void updateFromChannel(Channel channel) {
            this.atAll = this.atAll || (channel?.lastMessage?.mentionEveryone ?? false);
            this.id = channel?.id ?? this.id;
            this.groupId = channel?.groupId ?? this.groupId;
            this.thumbnail = channel?.thumbnail ?? this.thumbnail;
            this.name = channel?.name ?? this.name;
            this.topic = channel?.topic ?? this.topic;
            this.memberCount = channel?.memberCount ?? this.memberCount;
            this.isMute = channel?.isMute ?? this.isMute;
            this.live = channel?.live ?? this.live;
            this.lastMessage = channel?.lastMessage == null
                ? this.lastMessage
                : ChannelMessageView.fromChannelMessage(channel.lastMessage);
            this.lastMessageId = channel?.lastMessage?.id ?? this.lastMessageId;
        }

        public void handleUnreadMessage(ChannelMessageView message, string userId) {
            for (int k = 0; k < message.mentions.Count; k++) {
                if (message.mentions[k].id == userId) {
                    this.atMe = true;
                }
            }

            if (message.mentionEveryone) {
                this.atAll = true;
            }

            this.unread += 1;
        }
    }

    public enum ChannelMessageType {
        text,
        image,
        file,
        embed
    }

    public class ChannelMessageView {
        public string id;
        public long nonce;
        public string channelId;
        public User author;
        public DateTime time;
        public string timeString;
        public ChannelMessageType type = ChannelMessageType.text;
        public string content;
        public long fileSize;
        public List<Attachment> attachments;
        public bool mentionEveryone;
        public List<User> mentions;
        public bool starred;
        public List<string> replyMessageIds;
        public List<string> lowerMessageIds;
        public List<User> replyUsers;
        public List<User> lowerUsers;
        public bool pending;
        public bool deleted;
        public List<Reaction> reactions;
        public List<Embed> embeds;

        public static ChannelMessageView fromPushMessage(SocketResponseMessageData message) {
            if (message == null) {
                return new ChannelMessageView();
            }
            
            ChannelMessageType type = message.content != null || message.attachments.Count == 0
                ? message.embeds.Count == 0 ? ChannelMessageType.text : ChannelMessageType.embed
                : message.attachments[0].contentType.StartsWith("image")
                    ? ChannelMessageType.image
                    : ChannelMessageType.file;
            string content = message.content;
            switch (type) {
                case ChannelMessageType.text:
                case ChannelMessageType.embed:
                    content = content ?? "";
                    break;
                case ChannelMessageType.image:
                    content = CImageUtils.SizeTo200ImageUrl(message.attachments[0].url);
                    break;
                case ChannelMessageType.file:
                    content = message.attachments[0].filename;
                    break;
            }

            return new ChannelMessageView {
                id = message.id,
                nonce = string.IsNullOrEmpty(message.nonce) ? 0 : Convert.ToInt64(message.nonce, 16),
                channelId = message.channelId,
                author = message.author,
                content = content,
                fileSize = type == ChannelMessageType.file ? message.attachments[0].size : 0,
                time = DateConvert.DateTimeFromNonce(message.nonce),
                attachments = message.attachments,
                type = type,
                mentionEveryone = message.mentionEveryone,
                mentions = message.mentions,
                starred = message.starred,
                replyMessageIds = message.replyMessageIds,
                lowerMessageIds = message.lowerMessageIds,
                replyUsers = message.replyUsers,
                lowerUsers = message.lowerUsers,
                pending = message.pending,
                deleted = message.deletedTime != null,
                embeds = message.embeds,
                reactions = message.reactions
            };
        }

        public static ChannelMessageView fromChannelMessage(ChannelMessage message) {
            if (message == null) {
                return new ChannelMessageView();
            }
            ChannelMessageType type = message.content != null || message.attachments.Count == 0
                ? message.embeds.Count == 0 ? ChannelMessageType.text : ChannelMessageType.embed
                : message.attachments[0].contentType.StartsWith("image")
                    ? ChannelMessageType.image
                    : ChannelMessageType.file;
            string content = message.content;
            switch (type) {
                case ChannelMessageType.text:
                case ChannelMessageType.embed:
                    content = content ?? "";
                    break;
                case ChannelMessageType.image:
                    content = CImageUtils.SizeTo200ImageUrl(message.attachments[0].url);
                    break;
                case ChannelMessageType.file:
                    content = message.attachments[0].filename;
                    break;
            }

            return new ChannelMessageView {
                id = message.id,
                nonce = string.IsNullOrEmpty(message.nonce) ? 0 : Convert.ToInt64(message.nonce, 16),
                channelId = message.channelId,
                author = message.author,
                content = content,
                fileSize = type == ChannelMessageType.file ? message.attachments[0].size : 0,
                time = DateConvert.DateTimeFromNonce(message.nonce),
                attachments = message.attachments,
                type = type,
                mentionEveryone = message.mentionEveryone,
                mentions = message.mentions,
                starred = message.starred,
                replyMessageIds = message.replyMessageIds,
                lowerMessageIds = message.lowerMessageIds,
                replyUsers = message.replyUsers,
                lowerUsers = message.lowerUsers,
                pending = message.pending,
                deleted = message.deletedTime != null,
                embeds = message.embeds,
                reactions = message.reactions
            };
        }
    }
}