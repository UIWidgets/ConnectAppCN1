using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ChannelShareScreenActionModel : BaseActionModel {
        public Action pushToChannel;
        public Func<IPromise> fetchChannelInfo;
        public Action joinChannel;
    }
}