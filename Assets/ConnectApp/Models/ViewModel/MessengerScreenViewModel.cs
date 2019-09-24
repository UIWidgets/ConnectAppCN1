using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Models.ViewModel {
    public class MessengerScreenViewModel {
        public int discoverPage;
        public int currentTabBarIndex;
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
        public int mentioned = 0;
        public bool isTop = false;
        public bool joined = false;
        public bool atMe = false;
        public bool atAll = false;
        public bool hasMore = false;
        public bool hasMoreNew = false;
        public bool upToDate = true;
        public List<string> memberIds;
        public int memberOffset;
        public Dictionary<string, bool> memberFolloweeMap;
        public float offsetToBottom;
        public float offsetToTop;
        public bool atBottom;

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
        
        public static ChannelView fromNormalChannelLite(NormalChannelLite channel) {
            return new ChannelView {
                atAll = false,
                memberIds = new List<string>(),
                id = channel?.id,
                groupId = channel?.groupId,
                thumbnail = channel?.thumbnail,
                name = channel?.name,
                topic = channel?.topic,
                memberCount = channel?.memberCount ?? 0,
                isMute = channel?.isMute ?? false,
                live = channel?.live ?? false,
                lastMessageId = channel?.lastMessageId,
                messageIds = new List<string>()
            };
        }

        public void updateFromNormalChannelLite(NormalChannelLite channel) {
            this.id = channel?.id ?? this.id;
            this.groupId = channel?.groupId ?? this.groupId;
            this.thumbnail = channel?.thumbnail ?? this.thumbnail;
            this.name = channel?.name ?? this.name;
            this.topic = channel?.topic ?? this.topic;
            this.memberCount = channel?.memberCount ?? this.memberCount;
            this.isMute = channel?.isMute ?? this.isMute;
            this.live = channel?.live ?? this.live;
            this.lastMessageId = channel?.lastMessageId ?? this.lastMessageId;
        }

        public void handleUnreadMessage(ChannelMessageView message, string userId) {
            bool atMe = false, atAll = false;
            for (int k = 0; k < message.mentions.Count; k++) {
                if (message.mentions[k].id == userId) {
                    atMe = true;
                }
            }

            if (message.mentionEveryone) {
                atAll = true;
                atMe = true;
            }

            if (atAll || atMe) {
                this.mentioned += 1;
            }

            this.atAll = this.atAll || atAll;
            this.atMe = this.atMe || atMe;

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
        public ChannelMessageType type = ChannelMessageType.text;
        public string content;
        public long fileSize = 0;
        public List<Attachment> attachments;
        public bool mentionEveryone = false;
        public List<User> mentions;
        public bool starred = false;
        public List<string> replyMessageIds;
        public List<string> lowerMessageIds;
        public List<User> replyUsers;
        public List<User> lowerUsers;
        public bool pending = false;
        public bool deleted = false;
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
        
        public static ChannelMessageView fromChannelMessageLite(ChannelMessageLite message) {
            if (message == null) {
                return new ChannelMessageView();
            }
            ChannelMessageType type = message.content != null || message.attachments.Count == 0
                ? ChannelMessageType.text
                : message.attachments[0].contentType.StartsWith("image")
                    ? ChannelMessageType.image
                    : ChannelMessageType.file;
            string content = message.content;
            switch (type) {
                case ChannelMessageType.text:
                    break;
                case ChannelMessageType.image:
                    content = message.attachments[0].url;
                    break;
                case ChannelMessageType.file:
                    content = message.attachments[0].filename;
                    break;
            }
            long nonce = string.IsNullOrEmpty(message.nonce) ? 0 : Convert.ToInt64(message.nonce, 16);

            return new ChannelMessageView {
                id = message.id,
                nonce = nonce,
                channelId = message.channelId,
                author = new User {
                    id = message.author.id
                },
                content = content,
                fileSize = type == ChannelMessageType.file ? message.attachments[0].size : 0,
                time = DateConvert.DateTimeFromNonce(message.nonce),
                attachments = message.attachments,
                type = type,
                mentionEveryone = message.mentionEveryone,
                mentions = message.mentions?.Select(user => new User{id = user.id}).ToList()
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
                    break;
                case ChannelMessageType.embed:
                    content = content ?? "";
                    break;
                case ChannelMessageType.image:
                    content = message.attachments[0].url;
                    break;
                case ChannelMessageType.file:
                    content = message.attachments[0].filename;
                    break;
            }
            long nonce = string.IsNullOrEmpty(message.nonce) ? 0 : Convert.ToInt64(message.nonce, 16);

            return new ChannelMessageView {
                id = message.id,
                nonce = nonce,
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