using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class DiscoverChannelsScreenActionModel : BaseActionModel {
        public Action<string> pushToChannel;
        public Action<string> pushToChannelDetail;
        public Action<string, string> joinChannel;
        public Func<IPromise> fetchChannels;
    }
}