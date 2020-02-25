using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class GameScreenViewModel {
        public bool gameLoading;
        public List<string> gameIds;
        public int gamePage;
        public bool gameHasMore;
        public Dictionary<string, RankData> rankDict;
    }
}