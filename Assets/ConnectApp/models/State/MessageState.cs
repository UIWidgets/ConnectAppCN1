using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class MessageState {
        public Dictionary<string, List<string>> channelMessageList { get; set; }
        public Dictionary<string, Dictionary<string, Message>> channelMessageDict { get; set; }
    }
}