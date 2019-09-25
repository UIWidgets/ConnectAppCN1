using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.ViewModel;
using Unity.UIWidgets.foundation;

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
        public Dictionary<string, ChannelMember> membersDict;
        public Dictionary<string, long> unreadDict;
        public Dictionary<string, bool> channelTop;

        public void updateChannel(Channel channel) {
            if (!this.channelDict.TryGetValue(channel.id, out var channelView)) {
                this.channelDict[channel.id] = ChannelView.fromChannel(channel);
                this.channelDict[channel.id].upToDate = this.upToDate(channel.id);
                return;
            }

            channelView.updateFromChannel(channel);
            channelView.upToDate = this.upToDate(channel.id);
        }

        public void updateNormalChannelLite(NormalChannelLite channel) {
            if (!this.channelDict.TryGetValue(channel.id, out var channelView)) {
                this.channelDict[channel.id] = ChannelView.fromNormalChannelLite(channel);
                this.channelDict[channel.id].upToDate = this.upToDate(channel.id);
                return;
            }

            channelView.updateFromNormalChannelLite(channel);
            channelView.upToDate = this.upToDate(channel.id);
        }

        public void updateMessageUser(MessageUser user) {
            if (this.membersDict.TryGetValue(user.id, out var member)) {
                member.user.id = user.id;
                member.user.username = user.username;
                member.user.fullName = user.fullname;
                member.user.avatar = user.avatar;
                member.user.title = user.title;
                member.user.coverImage = user.coverImage;
                member.user.followCount = user.followCount;
                member.presenceStatus = user.presenceStatus;
            }

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

        public ChannelMember getMember(string userId) {
            if (this.membersDict.TryGetValue(userId, out var member)) {
                return member;
            }

            return null;
        }

        public bool upToDate(string channelId) {
            if (!this.channelDict.TryGetValue(channelId, out var channelView)) {
                return false;
            }

            if (channelView.messageIds.isEmpty()) {
                return false;
            }

            return this.messageDict[channelView.messageIds.last()].nonce >= channelView.lastMessage.nonce;
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
                this.updateMessageUser(sessionReadyData.users[i]);
            }

            for (int i = 0; i < sessionReadyData.lastMessages.Count; i++) {
                var message = sessionReadyData.lastMessages[i];
                var channelId = message.channelId;
                if (this.channelDict.TryGetValue(channelId, out var channel)) {
                    channel.lastMessageId = message.id;
                    channel.lastMessage = ChannelMessageView.fromChannelMessageLite(message);
                    channel.lastMessage.author =
                        this.getMember(channel.lastMessage.author.id)?.user;
                    channel.lastMessage.mentions = channel.lastMessage.mentions?.Select(
                        user => this.getMember(user.id).user)?.ToList();
                }
            }

            for (int i = 0; i < sessionReadyData.readState.Count; i++) {
                var readState = sessionReadyData.readState[i];
                var channelId = readState.channelId;
                if (this.channelDict.TryGetValue(channelId, out var channel)) {
                    channel.mentioned = readState.mentionCount;
                    channel.unread = readState.lastMessageId != channel.lastMessageId ? 1 : 0;
                    channel.atMe = channel.mentioned > 0 && channel.unread > 0;
                }
            }
        }
    }
}