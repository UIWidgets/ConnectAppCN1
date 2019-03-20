using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class FetchCommentsResponse {
        public List<Message> items;
        public List<Message> parents;
        public string currOldestMessageId;
        public bool hasMore;
        public bool hasMoreNew;
    }
}