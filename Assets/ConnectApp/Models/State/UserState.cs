using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class UserState {
        public Dictionary<string, User> userDict { get; set; }
    }
}