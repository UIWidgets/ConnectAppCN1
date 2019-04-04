using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.redux.actions {
    public class FetchMessagesAction : RequestAction {
        public bool isFirstLoad;
        public string channelId;
        public string currOldestMessageId = "";
    }

    public class FetchMessagesSuccessAction : BaseAction {
        public bool isFirstLoad;
        public string channelId;
        public List<Message> messages;
        public bool hasMore;
        public string currOldestMessageId = "";
    }

    public class SendMessageAction : RequestAction {
        public string channelId;
        public string content;
        public string nonce;
        public string parentMessageId = "";
    }

    public class SendMessageSuccessAction : BaseAction {
        public string channelId;
        public string content;
        public string nonce;
        public string parentMessageId = "";
    }
    
    public class SendMessageFailedAction : BaseAction {
    }
}