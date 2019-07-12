using System;
using System.Collections.Generic;

namespace ConnectApp.Models.State {
    [Serializable]
    public class FollowState {
        public Dictionary<string, Dictionary<string, bool>> followDict { get; set; }
    }
}