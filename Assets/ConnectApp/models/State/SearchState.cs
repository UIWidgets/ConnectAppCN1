using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class SearchState {
        public bool loading { get; set; }
        public string keyword { get; set; }
        public List<Article> searchArticles { get; set; }
        public List<string> searchHistoryList { get; set; }
    }
}