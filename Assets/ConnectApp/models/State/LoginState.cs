using System;

namespace ConnectApp.models {
    [Serializable]
    public class LoginState {
        public string email;
        public string password;
        public LoginInfo loginInfo;
        public bool loading;
        public bool isLoggedIn;
    }
}