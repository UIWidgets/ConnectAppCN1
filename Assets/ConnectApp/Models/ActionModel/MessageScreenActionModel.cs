using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class MessageScreenActionModel : BaseActionModel {
        public Action pushToNotificatioins;
        public Action pushToDiscoverChannels;
        public Action pushToChannelDetail;
        public Action<string> pushToChannel;
        public Func<IPromise> fetchPublicChannels;
        public Func<IPromise> fetchJoinedChannels;
        public Action fetchMessages;
    }
}