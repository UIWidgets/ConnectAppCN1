using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
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