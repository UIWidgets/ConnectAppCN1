using System;

namespace ConnectApp.models {
    [Serializable]
    public class Team {
        public string id;
        public string avatar;
        public string name;
        public string slug;
        public string isDefault;
        public DateTime createdTime;
        public bool isManager;
        public bool isPrivate;
    }
}