using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Api;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Channel {
        public string id;
        public string groupId;
        public string thumbnail;
        public string imageUrl;
        public string name;
        public string topic;
        public int memberCount;
        public bool isMute;
        public bool live;
        public ChannelMessage lastMessage;
    }

    [Serializable]
    public class ChannelMessage {
        public string id;
        public string type;
        public string nonce;
        public string channelId;
        public string content;
        public User author;
        public List<Attachment> attachments;
        public bool mentionEveryone;
        public List<User> mentions;
        public bool starred;
        public List<string> replyMessageIds;
        public List<string> lowerMessageIds;
        public List<User> replyUsers;
        public List<User> lowerUsers;
        public List<Reaction> reactions;
        public List<Embed> embeds;
        public bool pending;
        public string deletedTime;
    }

    [Serializable]
    public class Attachment {
        public string id;
        public string url;
        public string filename;
        public string contentType;
        public int width;
        public int height;
        public int size;
        public int duration;
    }

    [Serializable]
    public class Embed {
        public string embedType;
        public EmbedData embedData;
    }

    [Serializable]
    public class EmbedData {
        public string description;
        public string image;
        public string name;
        public string title;
        public string url;
        public string imageUrl;
    }

    [Serializable]
    public class ChannelMember {
        public string id;
        public string channelId;
        public User user;
        public string role;
        public string stickTime;
        public bool isMute;
        public string presenceStatus;
        public bool isBanned;
        public bool kicked;
        public bool left;
        public bool guideFinished;
        public bool showTerms;

        public static ChannelMember fromSocketResponseChannelMemberChangeData(
            SocketResponseChannelMemberChangeData data) {
            return new ChannelMember {
                id = data.id,
                channelId = data.channelId,
                user = data.user,
                role = data.role,
                stickTime = data.stickTime,
                isMute = data.isMute,
                presenceStatus = null,
                isBanned = data.isBanned,
                kicked = data.kicked,
                left = data.left,
                guideFinished = data.guideFinished,
                showTerms = data.showTerms
            };
        }

        public void updateFromSocketResponseChannelMemberChangeData(
            SocketResponseChannelMemberChangeData data) {
            this.id = data.id ?? this.id;
            this.channelId = data.channelId ?? this.channelId;
            this.user = data.user ?? this.user;
            this.role = data.role ?? this.role;
            this.stickTime = data.stickTime ?? this.stickTime;
            this.isMute = data.isMute;
            this.isBanned = data.isBanned;
            this.kicked = data.kicked;
            this.left = data.left;
            this.guideFinished = data.guideFinished;
            this.showTerms = data.showTerms;
        }
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
        public List<string> oldMessageIds;
        public List<string> newMessageIds;
        public List<string> localMessageIds;
        public int unread = 0;
        public int mentioned = 0;
        public string lastReadMessageId = null;
        public bool isTop = false;
        public bool joined = false;
        public bool joinLoading = false;
        public bool atMe = false;
        public bool atAll = false;
        public bool hasMore = true;
        public bool hasMoreNew = true;
        public List<string> memberIds;
        public Dictionary<string, ChannelMember> membersDict;
        public int memberOffset;
        public bool atBottom = false;
        public bool active = false;
        public ChannelMember currentMember;
        public bool needFetchMessages;

        public static ChannelView fromChannel(Channel channel) {
            return new ChannelView {
                atAll = channel?.lastMessage?.mentionEveryone ?? false,
                memberIds = new List<string>(),
                membersDict = new Dictionary<string, ChannelMember>(),
                id = channel?.id,
                groupId = channel?.groupId,
                thumbnail = channel?.thumbnail ?? "",
                name = channel?.name ?? "",
                topic = channel?.topic,
                memberCount = channel?.memberCount ?? 0,
                isMute = channel?.isMute ?? false,
                live = channel?.live ?? false,
                lastMessageId = channel?.lastMessage?.id,
                lastMessage = ChannelMessageView.fromChannelMessage(channel?.lastMessage),
                messageIds = new List<string>(),
                localMessageIds = new List<string>(),
                oldMessageIds = new List<string>(),
                newMessageIds = new List<string>(),
                currentMember = new ChannelMember()
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
                id = channel?.id,
                groupId = channel?.groupId,
                thumbnail = channel?.thumbnail,
                name = channel?.name,
                topic = channel?.topic,
                memberCount = channel?.memberCount ?? 0,
                isMute = channel?.isMute ?? false,
                live = channel?.live ?? false,
                lastMessageId = channel?.lastMessageId,
                atAll = false,
                memberIds = new List<string>(),
                membersDict = new Dictionary<string, ChannelMember>(),
                messageIds = new List<string>(),
                localMessageIds = new List<string>(),
                oldMessageIds = new List<string>(),
                newMessageIds = new List<string>(),
                currentMember = new ChannelMember()
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

        public void updateFromSocketResponseUpdateChannelData(SocketResponseUpdateChannelData channel) {
            this.id = channel?.id ?? this.id;
            this.groupId = channel?.groupId ?? this.groupId;
            this.thumbnail = channel?.thumbnail ?? this.thumbnail;
            this.name = channel?.name ?? this.name;
            this.topic = channel?.groupId.isNotEmpty() ?? false
                ? this.topic
                : channel?.topic ?? this.topic;
            this.memberCount = channel?.memberCount ?? this.memberCount;
            this.isMute = channel?.isMute ?? this.isMute;
            this.live = channel?.live ?? this.live;
            this.lastMessage = channel?.lastMessage == null
                ? this.lastMessage
                : ChannelMessageView.fromChannelMessageLite(channel.lastMessage);
        }

        public static ChannelView fromSocketResponseUpdateChannelData(SocketResponseUpdateChannelData channel) {
            ChannelView channelView = new ChannelView {
                atAll = false,
                memberIds = new List<string>(),
                membersDict = new Dictionary<string, ChannelMember>(),
                messageIds = new List<string>(),
                localMessageIds = new List<string>(),
                oldMessageIds = new List<string>(),
                newMessageIds = new List<string>(),
                currentMember = new ChannelMember()
            };
            channelView.updateFromSocketResponseUpdateChannelData(channel);
            return channelView;
        }

        public void completeMissingFieldsFromGroup(Group group) {
            this.groupId = group.id.isEmpty() ? this.groupId : group.id;
            this.topic = group.id.isEmpty() ? this.topic : group.description;
        }

        public void handleUnreadMessage(ChannelMessageView message, string userId) {
            if (message.mentionEveryone || message.mentions.Any(user => user.id == userId)) {
                this.mentioned += 1;
                this.atMe = true;
            }

            this.atAll = this.atAll || message.mentionEveryone;
            this.unread += 1;
        }

        public void clearUnread() {
            this.unread = 0;
            this.mentioned = 0;
            this.lastReadMessageId = null;
            this.atAll = false;
            this.atMe = false;
        }

        public ChannelMember getMember(string userId) {
            return this.membersDict.TryGetValue(key: userId, out var member) ? member : null;
        }

        public void updateMessageUser(MessageUser user, bool addUserIfNotExist = true) {
            if (this.membersDict.TryGetValue(user.id, out var member)) {
                member.user.id = user.id;
                member.user.username = user.username;
                member.user.fullName = user.fullname;
                member.user.avatar = user.avatar;
                member.user.title = user.title;
                member.user.coverImage = user.coverImage;
                member.user.followCount = user.followCount;
                member.presenceStatus = user.presenceStatus;
                return;
            }

            if (addUserIfNotExist) {
                this.membersDict[user.id] = new ChannelMember {
                    user = new User {
                        id = user.id,
                        username = user.username,
                        fullName = user.fullname,
                        avatar = user.avatar,
                        title = user.title,
                        coverImage = user.coverImage,
                        followCount = user.followCount
                    },
                    id = user.id,
                    presenceStatus = user.presenceStatus
                };
            }
        }
    }

    public enum ChannelMessageType {
        deleted,
        text,
        image,
        file,
        embedExternal,
        embedImage,
        skip
    }

    public class ChannelMessageView {
        public string id;
        public long nonce;
        public string channelId;
        public User author;
        public DateTime time;
        public ChannelMessageType type = ChannelMessageType.text;
        public string content;
        public string plainText;
        public long fileSize = 0;
        public int width;
        public int height;
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
        public string status = "normal";
        public byte[] imageData;
        public byte[] videoData;
        public float? buildHeight;

        static ChannelMessageType getType(string content, bool deleted, List<Attachment> attachments = null,
            List<Embed> embeds = null) {
            if (deleted) {
                return ChannelMessageType.deleted;
            }

            if (content != null || (attachments?.Count ?? 0) == 0) {
                if ((embeds?.Count ?? 0) == 0) {
                    return ChannelMessageType.text;
                }

                switch (embeds.First().embedType) {
                    case "image":
                        return ChannelMessageType.embedImage;
                    case "external":
                        return ChannelMessageType.embedExternal;
                    default:
                        return ChannelMessageType.skip;
                }
            }

            return attachments.First().contentType.StartsWith("image") &&
                   !attachments.First().filename.EndsWith(".svg")
                ? ChannelMessageType.image
                : ChannelMessageType.file;
        }

        static long getNonce(string nonce) {
            return nonce.isEmpty() ? 0 : Convert.ToInt64(nonce, 16);
        }

        static string getContent(string content, bool deleted, List<Attachment> attachments = null, List<Embed> embeds = null) {
            switch (getType(content, deleted, attachments, embeds)) {
                case ChannelMessageType.text:
                case ChannelMessageType.embedExternal:
                case ChannelMessageType.embedImage:
                    return content ?? "";
                case ChannelMessageType.image:
                    return attachments.FirstOrDefault().url;
                case ChannelMessageType.file:
                    return attachments.FirstOrDefault().filename;
                case ChannelMessageType.skip:
                case ChannelMessageType.deleted:
                    return "";
                default:
                    return "";
            }
        }

        static int getFileSize(string content, bool deleted, List<Attachment> attachments = null, List<Embed> embeds = null) {
            return getType(content, deleted, attachments, embeds) == ChannelMessageType.file ? attachments[0].size : 0;
        }

        static int getImageWidth(string content, bool deleted, List<Attachment> attachments = null, List<Embed> embeds = null) {
            return getType(content, deleted, attachments, embeds) == ChannelMessageType.image ? attachments[0].width : 0;
        }

        static int getImageHeight(string content, bool deleted, List<Attachment> attachments = null, List<Embed> embeds = null) {
            return getType(content, deleted, attachments, embeds) == ChannelMessageType.image ? attachments[0].height : 0;
        }

        public static ChannelMessageView fromPushMessage(SocketResponseMessageData message) {
            return message == null
                ? new ChannelMessageView()
                : new ChannelMessageView {
                    id = message.id,
                    nonce = getNonce(message.nonce),
                    channelId = message.channelId,
                    author = message.author,
                    content = getContent(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
                    fileSize = getFileSize(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
                    width = getImageWidth(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
                    height = getImageHeight(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
                    time = DateConvert.DateTimeFromNonce(message.id),
                    attachments = message.attachments,
                    type = getType(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
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
            return message == null
                ? new ChannelMessageView()
                : new ChannelMessageView {
                    id = message.id,
                    nonce = getNonce(message.nonce),
                    channelId = message.channelId,
                    author = new User {id = message.author.id},
                    content = getContent(message.content, message.deletedTime.isNotEmpty(), message.attachments),
                    fileSize = getFileSize(message.content, message.deletedTime.isNotEmpty(), message.attachments),
                    width = getImageWidth(message.content, message.deletedTime.isNotEmpty(), message.attachments),
                    height = getImageHeight(message.content, message.deletedTime.isNotEmpty(), message.attachments),
                    time = DateConvert.DateTimeFromNonce(message.id),
                    attachments = message.attachments,
                    type = getType(message.content, message.deletedTime.isNotEmpty(), message.attachments),
                    mentionEveryone = message.mentionEveryone,
                    mentions = message.mentions?.Select(user => new User {id = user.id}).ToList(),
                    deleted = message.deletedTime != null
                };
        }

        public static ChannelMessageView fromChannelMessage(ChannelMessage message) {
            return message == null
                ? new ChannelMessageView()
                : new ChannelMessageView {
                    id = message.id,
                    nonce = getNonce(message.nonce),
                    channelId = message.channelId,
                    author = message.author,
                    content = getContent(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
                    fileSize = getFileSize(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
                    width = getImageWidth(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
                    height = getImageHeight(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
                    time = DateConvert.DateTimeFromNonce(message.id),
                    attachments = message.attachments,
                    type = getType(message.content, message.deletedTime.isNotEmpty(), message.attachments, message.embeds),
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