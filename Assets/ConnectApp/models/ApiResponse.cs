using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class ArticlesResponse {
        public List<Article> items;
        public Dictionary<string, User> userMap;
    }
}