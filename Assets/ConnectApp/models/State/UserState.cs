using System;
using System.Collections.Generic;

namespace ConnectApp.models {
    [Serializable]
    public class UserState {
        public Dictionary<string, User> UserDict { get; set; }
    }
}