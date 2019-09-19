using System;
using System.Collections.Generic;
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
    }
}