using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    public class ChannelState {
        public List<string> publicChannels;
        public List<string> joinedChannels;
        public int discoverPage;
        public bool messageLoading;
        public int totalUnread;
        public int totalMention;
        public Dictionary<string, ChannelView> channelDict;
        public Dictionary<string, ChannelMessageView> messageDict;
        public Dictionary<string, bool> channelTop;
        public bool socketConnected;
        
        public string mentionUserId;
        public bool mentionAutoFocus;
        public Dictionary<string, Dictionary<string, ChannelMember>> mentionSuggestions;
        public bool mentionLoading;

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

        public void updateTotalMention() {
            this.totalUnread = 0;
            this.totalMention = 0;
            this.joinedChannels.ForEach(channelId => {
                this.totalUnread += this.channelDict[key: channelId].unread;
                this.totalMention += this.channelDict[key: channelId].mentioned;
            });
        }

        public string totalNotification() {
            return this.totalUnread > 0
                ? this.totalMention > 0
                    ? $"{this.totalMention}"
                    : ""
                : null;
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
                    channel.unread = readState.lastMessageId != channel.lastMessageId &&
                                     channel.lastMessageId != null ? 1 : 0;
                    channel.atMe = channel.mentioned > 0 && channel.unread > 0;
                }
            });
        }
    }
}