using System;

namespace ConnectApp.models {
    [Serializable]
    public class Login {
        public string email;
        public bool loading;
        public bool isLoggedIn;
    }
}