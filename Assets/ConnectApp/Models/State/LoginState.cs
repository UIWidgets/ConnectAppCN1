using System;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class LoginState {
        public LoginInfo loginInfo;
        public string email;
        public string password;
        public bool loading;
        public bool isLoggedIn;
        public string newNotifications;
    }
}