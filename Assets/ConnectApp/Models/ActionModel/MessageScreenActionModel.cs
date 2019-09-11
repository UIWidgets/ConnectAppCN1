using System;

namespace ConnectApp.Models.ActionModel {
    public class MessageScreenActionModel : BaseActionModel {
        public Action pushToNotificatioins;
        public Action pushToDiscoverChannels;
        public Action pushToChannelDetail;
        public Action pushToChannel;
        public Action fetchPublicChannels;
    }
}