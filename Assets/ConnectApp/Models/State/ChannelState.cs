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

        public void updateChannel(Channel channel) {
            if (!this.channelDict.TryGetValue(channel.id, out var channelView)) {
                this.channelDict[channel.id] = ChannelView.fromChannel(channel);
                return;
            }

            channelView.updateFromChannel(channel);
        }

        public void updateNormalChannelLite(NormalChannelLite channel) {
            if (!this.channelDict.TryGetValue(channel.id, out var channelView)) {
                this.channelDict[channel.id] = ChannelView.fromNormalChannelLite(channel);
                return;
            }

            channelView.updateFromNormalChannelLite(channel);
        }

        public void updateTotalMention() {
            this.totalUnread = 0;
            for (int i = 0; i < this.joinedChannels.Count; i++) {
                this.totalUnread += this.getJoinedChannel(i).unread;
            }

            this.totalMention = 0;
            for (int i = 0; i < this.joinedChannels.Count; i++) {
                this.totalMention += this.getJoinedChannel(i).mentioned;
            }
        }

        public string totalNotification() {
            return this.totalUnread > 0
                ? this.totalMention > 0
                    ? $"{this.totalMention}"
                    : ""
                : null;
        }

        public ChannelView getJoinedChannel(int i) {
            return this.channelDict[this.joinedChannels[i]];
        }

        public void updateSessionReadyData(SocketResponseSessionData sessionReadyData) {
            for (int i = 0; i < sessionReadyData.lobbyChannels.Count; i++) {
                var channel = sessionReadyData.lobbyChannels[i];
                this.updateNormalChannelLite(channel);
                if (!this.joinedChannels.Contains(channel.id)) {
                    this.joinedChannels.Add(channel.id);
                }
            }

            for (int i = 0; i < sessionReadyData.publicChannels.Count; i++) {
                var channel = sessionReadyData.publicChannels[i];
                this.updateNormalChannelLite(channel);
                if (!this.joinedChannels.Contains(channel.id)) {
                    this.joinedChannels.Add(channel.id);
                }
            }
            
            for (int i = 0; i < sessionReadyData.users.Count; i++) {
                for (int j = 0; j < this.joinedChannels.Count; j++) {
                    this.channelDict[this.joinedChannels[j]].updateMessageUser(sessionReadyData.users[i]);
                }
            }

            for (int i = 0; i < sessionReadyData.lastMessages.Count; i++) {
                var message = sessionReadyData.lastMessages[i];
                var channelId = message.channelId;
                if (this.channelDict.TryGetValue(channelId, out var channel)) {
                    channel.lastMessageId = message.id;
                    channel.lastMessage = ChannelMessageView.fromChannelMessageLite(message);
                    channel.lastMessage.author = channel.getMember(channel.lastMessage.author.id)?.user;
                    channel.lastMessage.mentions = channel.lastMessage.mentions?.Select(
                        user => channel.getMember(user.id).user)?.ToList();
                }
            }

            for (int i = 0; i < sessionReadyData.readState.Count; i++) {
                var readState = sessionReadyData.readState[i];
                var channelId = readState.channelId;
                if (this.channelDict.TryGetValue(channelId, out var channel)) {
                    channel.mentioned = readState.mentionCount;
                    channel.unread = readState.lastMessageId != channel.lastMessageId &&
                                     channel.lastMessageId != null ? 1 : 0;
                    channel.atMe = channel.mentioned > 0 && channel.unread > 0;
                }
            }
        }
    }
}