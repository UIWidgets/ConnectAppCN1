using System;

namespace ConnectApp.models
{
    [Serializable]
    public class User
    {
        public string id;
        public string type;
        public string username;
        public string fullName;
        public string title;
        public string avatar;
        public string coverImage;
        public string description;
    }
}