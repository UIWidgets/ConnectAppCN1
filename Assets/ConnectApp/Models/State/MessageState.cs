using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class MessageState {
        public bool messageLoading { get; set; }
        public bool sendMessageLoading { get; set; }
        public Dictionary<string, List<string>> channelMessageList { get; set; }
        public Dictionary<string, Dictionary<string, Message>> channelMessageDict { get; set; }
        public string currOldestMessageId { get; set; }
        public bool hasMore { get; set; }
    }
}