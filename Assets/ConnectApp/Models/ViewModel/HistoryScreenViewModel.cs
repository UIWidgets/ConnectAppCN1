using System;
using System.Collections.Generic;
using ConnectApp.models;

namespace ConnectApp.Models.ViewModel {
    public class HistoryScreenViewModel {
        public List<IEvent> eventHistory;
        public List<Article> articleHistory;
        public Dictionary<string, User> userDict;
        public Dictionary<string, Team> teamDict;
        public Dictionary<string, Place> placeDict;
    }
}