using System;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Splash {
        public string id;
        public string name;
        public string image;
        public int duration;
        public bool archived;
        public DateTime createdTime;
        public string url;
        public bool isShowLogo;
        public string color;
    }
}