using System;
using ConnectApp.screens;

namespace ConnectApp.models {
    [Serializable]
    public class LoginState {
        public LoginInfo loginInfo;
        public string email;
        public string password;
        public bool loading;
        public bool isLoggedIn;
        public FromPage fromPage;
    }
}