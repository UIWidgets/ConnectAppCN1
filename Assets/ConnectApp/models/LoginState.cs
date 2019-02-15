using System;

namespace ConnectApp.models
{
    [Serializable]
    public class LoginState
    {
        public string email;
        public bool loading;
        public bool isLoggedIn;
    }
}