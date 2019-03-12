namespace ConnectApp.redux.actions {
    public class FetchMessagesAction : RequestAction {
        public string channelId;
        public string currOldestMessageId = "";
    }

    public class FetchMessagesSuccessAction : BaseAction {
    }

    public class SendMessageAction : RequestAction {
        public string channelId;
        public string content;
        public string nonce;
        public string parentMessageId = "";
    }

    public class SendMessageSuccessAction : BaseAction {
    }
}