using System;
using System.Collections.Generic;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Member {
        public string id;
        public string userId;
        public string status;
        public string email;
        public string invitedBy;
        public List<string> role;
    }
}