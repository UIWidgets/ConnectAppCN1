using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel {
    public class EventDetailScreenViewModel {
        public string eventId;
        public string currOldestMessageId;
        public bool isLoggedIn;
        public bool eventDetailLoading;
        public bool joinEventLoading;
        public bool showChatWindow;
        public string channelId;
        public List<string> messageList;
        public bool messageLoading;
        public bool hasMore;
        public bool sendMessageLoading;
        public Dictionary<string, Dictionary<string, Message>> channelMessageDict;
        public Dictionary<string, IEvent> eventsDict;
    }
}