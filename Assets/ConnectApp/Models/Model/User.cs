using System;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class User {
        public string id;
        public string type;
        public string username;
        public string fullName;
        public string name;
        public string title;
        public string avatar;
        public string coverImage;
        public string description;
    }
}