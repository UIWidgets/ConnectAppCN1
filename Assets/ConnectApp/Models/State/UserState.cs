using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class UserState {
        public Dictionary<string, User> userDict { get; set; }
    }
}