using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Utils;

namespace ConnectApp.Models.State {
    public class ChannelState {
        public bool channelLoading;
        public List<string> publicChannels;
        public List<string> joinedChannels;
        public List<string> createChannelFilterIds;
        public int discoverPage;
        public bool discoverHasMore;
        public bool channelShareInfoLoading;
        public int totalUnread;
        public int totalMention;
        public Dictionary<string, bool> channelMessageLoadingDict;
        public Dictionary<string, bool> channelInfoLoadingDict;
        public Dictionary<string, ChannelView> channelDict;
        public Dictionary<string, ChannelMessageView> messageDict;
        public Dictionary<string, ChannelMessageView> localMessageDict;
        public Dictionary<string, bool> channelTop;
        public bool socketConnected;
        public string mentionUserId;
        public string mentionUserName;
        public bool mentionAutoFocus;
        public Dictionary<string, List<ChannelMember>> mentionSuggestions;
        public bool mentionLoading;
        public string lastMentionQuery;
        public bool mentionSearching;
        public List<ChannelMember> queryMentions;

        public void updateMentionSuggestion(string channelId, User userInfo) {
            if (this.mentionSuggestions.ContainsKey(key: channelId)) {
                var suggestions = this.mentionSuggestions[key: channelId];
                for (int i = 0; i < suggestions.Count; i++) {
                    if (suggestions[i].user.id == userInfo.id) {
                        suggestions[i].user = userInfo;
                        break;
                    }
                }
            }
        }

        public void updateChannel(Channel channel) {
            if (!this.channelDict.TryGetValue(key: channel.id, out var channelView)) {
                this.channelDict[key: channel.id] = ChannelView.fromChannel(channel: channel);
                return;
            }

            channelView.updateFromChannel(channel: channel);
        }

        public void updateNormalChannelLite(NormalChannelLite channel) {
            if (!this.channelDict.TryGetValue(key: channel.id, out var channelView)) {
                this.channelDict[key: channel.id] = ChannelView.fromNormalChannelLite(channel: channel);
                return;
            }

            channelView.updateFromNormalChannelLite(channel: channel);
        }

        public void clearMentions() {
            this.totalUnread = 0;
            this.totalMention = 0;
            this.joinedChannels.ForEach(channelId => {
                this.channelDict[key: channelId].unread = 0;
                this.channelDict[key: channelId].mentioned = 0;
            });
        }

        public void updateTotalMention() {
            this.totalUnread = 0;
            this.totalMention = 0;
            this.joinedChannels.ForEach(channelId => {
                var channel = this.channelDict[key: channelId];
                this.totalUnread += channel.unread;
                this.totalMention += channel.unread > 0
                    ? channel.mentioned
                    : 0;
            });
        }

        public string totalNotification() {
            return this.totalUnread > 0
                ? this.totalMention > 0
                    ? $"{this.totalMention}"
                    : ""
                : null;
        }

        public void removeLocalMessage(ChannelMessageView channelMessage) {
            var channel = this.channelDict[channelMessage.channelId];
            var key = $"{channelMessage.author.id}:{channel.id}:{channelMessage.nonce:x16}";
            if (channel.localMessageIds.Contains($"{channelMessage.nonce:x16}") &&
                this.localMessageDict.ContainsKey(key)) {
                if (channelMessage.type == ChannelMessageType.image) {
                    channelMessage.imageData = this.localMessageDict[key].imageData;
                }

                this.localMessageDict.Remove(key);
                channel.localMessageIds.Remove($"{channelMessage.nonce:x16}");
            }
        }

        public void updateSessionReadyData(SocketResponseSessionData sessionReadyData) {
            sessionReadyData.lobbyChannels.ForEach(channel => {
                this.updateNormalChannelLite(channel: channel);
                if (!this.joinedChannels.Contains(item: channel.id)) {
                    this.joinedChannels.Add(item: channel.id);
                }
            });

            sessionReadyData.publicChannels.ForEach(channel => {
                this.updateNormalChannelLite(channel: channel);
                if (!this.joinedChannels.Contains(item: channel.id)) {
                    this.joinedChannels.Add(item: channel.id);
                }
            });

            sessionReadyData.privateChannels.ForEach(channel => {
                this.updateNormalChannelLite(channel: channel);
                if (!this.joinedChannels.Contains(item: channel.id)) {
                    this.joinedChannels.Add(item: channel.id);
                }
            });

            sessionReadyData.users.ForEach(user => {
                this.joinedChannels.ForEach(channelId => {
                    this.channelDict[key: channelId].updateMessageUser(user: user);
                });
            });

            sessionReadyData.lastMessages.ForEach(message => {
                var channelId = message.channelId;
                if (this.channelDict.TryGetValue(key: channelId, out var channel)) {
                    channel.lastMessageId = message.id;
                    channel.lastMessage = ChannelMessageView.fromChannelMessageLite(message: message);
                    channel.lastMessage.author = channel.getMember(userId: channel.lastMessage.author.id)?.user;
                    channel.lastMessage.mentions = channel.lastMessage.mentions?.Select(
                        user => channel.getMember(userId: user.id).user)?.ToList();
                }
            });

            sessionReadyData.readState.ForEach(readState => {
                var channelId = readState.channelId;
                if (this.channelDict.TryGetValue(key: channelId, out var channel)) {
                    channel.mentioned = readState.mentionCount;
                    channel.unread = channel.lastMessageId != null && readState.lastMessageId != null &&
                                     readState.lastMessageId.hexToLong() < channel.lastMessageId.hexToLong()
                        ? 1
                        : 0;
                    channel.lastReadMessageId = readState.lastMessageId;
                    channel.atMe = channel.mentioned > 0 && channel.unread > 0;
                }
            });
        }

        public void markCurrentChannelAsNeedingFetch() {
            this.joinedChannels.ForEach(channelId => {
                if (this.channelDict.ContainsKey(channelId)) {
                    var channel = this.channelDict[channelId];
                    if (channel.active) {
                        channel.needFetchMessages = true;
                    }
                }
            });
        }
    }
}