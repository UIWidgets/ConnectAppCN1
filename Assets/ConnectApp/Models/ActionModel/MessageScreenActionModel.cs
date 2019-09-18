using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class MessageScreenActionModel : BaseActionModel {
        public Action pushToNotificatioins;
        public Action pushToDiscoverChannels;
        public Action<string> pushToChannel;
        public Func<IPromise> fetchChannels;
        public Action fetchMessages;
    }
}