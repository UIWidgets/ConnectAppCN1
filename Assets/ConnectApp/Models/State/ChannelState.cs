using System;
using System.Collections.Generic;
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

        public void updateChannel(Channel channel) {
            if (!this.channelDict.TryGetValue(channel.id, out var channelView)) {
                this.channelDict[channel.id] = ChannelView.fromChannel(channel);
                this.channelDict[channel.id].upToDate = this.upToDate(channel.id);
                return;
            }
            channelView.updateFromChannel(channel);
            channelView.upToDate = this.upToDate(channel.id);
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