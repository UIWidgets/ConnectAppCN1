using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class HistoryScreenViewModel {
        public List<IEvent> eventHistory;
        public List<Article> articleHistory;
        public bool isLoggedIn;
    }
}