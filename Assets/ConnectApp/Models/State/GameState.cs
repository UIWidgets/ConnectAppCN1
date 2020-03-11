using System;
using System.Collections.Generic;

namespace ConnectApp.Models.State {
    [Serializable]
    public class GameState {
        public bool gameLoading { get; set; }
        public bool gameDetailLoading { get; set; }
        public List<string> gameIds { get; set; }
        public bool gameHasMore { get; set; }
        public int gamePage  { get; set; }
    }
}