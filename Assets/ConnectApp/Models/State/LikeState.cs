using System;
using System.Collections.Generic;

namespace ConnectApp.Models.State {
    [Serializable]
    public class LikeState {
        public Dictionary<string, Dictionary<string, bool>> likeDict { get; set; }
    }
}