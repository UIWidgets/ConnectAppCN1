using System;

namespace ConnectApp.models {
    [Serializable]
    public class LoginInfo {
        public string LSKey;
        public string userId;
        public string userFullName;
        public string authId;
        public bool anonymous;
    }
}