using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ChannelScreenActionModel : BaseActionModel {
        public Action pushToChannelDetail;
        public Action<string, string> fetchMessages;
        public Func<string, string, string, string, IPromise> sendMessage;
        public Action startSendMessage;
    }
}