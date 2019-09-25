using System;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class FavoriteTag {
        public string id;
        public string type;
        public string name;
        public string description;
        public IconStyle iconStyle;
        public Statistics stasitics;
    }

    [Serializable]
    public class Favorite {
        public string id;
        public string itemId;
        public string tagId;
    }

    [Serializable]
    public class Statistics {
        public int count;
    }

    [Serializable]
    public class IconStyle {
        public string name;
        public string bgColor;
    }
}