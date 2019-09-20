using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class ChannelScreenActionModel : BaseActionModel {
        public Action pushToChannelDetail;
        public Func<string, string, IPromise> fetchMessages;
        public Func<string, string, string, string, IPromise> sendMessage;
        public Action startSendMessage;
        public Func<string, string, string, IPromise> sendImage;
        public Action clearUnread;
        public Action<float, float> updateScrollOffset;
    }
}