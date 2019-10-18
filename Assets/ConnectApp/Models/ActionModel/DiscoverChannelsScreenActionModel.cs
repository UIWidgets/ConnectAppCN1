using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class DiscoverChannelsScreenActionModel : BaseActionModel {
        public Action<string> pushToChannel;
        public Action<string> pushToChannelDetail;
        public Action<string> startJoinChannel;
        public Action<string, string> joinChannel;
        public Func<int, IPromise> fetchChannels;
    }
}