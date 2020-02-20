using System;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class RankData {
        public string id;
        public string rankType;
        public string itemId;
        public int rank;
        public DateTime? createdTime;
        public DateTime? updatedTime;
        public string resetTitle;
        public string resetLabel;
        public string resetDesc;
        public string redirectURL;
        public string image;
        public string myFavoriteTagId;
    }
}