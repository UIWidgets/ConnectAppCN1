using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.models;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class FetchMessagesAction : RequestAction {}

    public class FetchMessagesSuccessAction : BaseAction {
        public bool isFirstLoad;
        public string channelId;
        public List<Message> messages;
        public bool hasMore;
        public string currOldestMessageId = "";
    }
    
    public class FetchMessagesFailureAction : BaseAction {}

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
    
    public static partial class Actions {
        public static object fetchMessages(string channelId, string currOldestMessageId, bool isFirstLoad)
        {
            return new ThunkAction<AppState>((dispatcher, getState) => {                
                return MessageApi.FetchMessages(channelId, currOldestMessageId)
                    .Then(messagesResponse => {
                        dispatcher.dispatch(new FetchMessagesSuccessAction {
                            isFirstLoad = isFirstLoad,
                            channelId = channelId,
                            messages = messagesResponse.items,
                            hasMore = messagesResponse.hasMore,
                            currOldestMessageId = messagesResponse.currOldestMessageId
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchMessagesFailureAction());
                        Debug.Log(error);
                    });
            });
        }
    }
}