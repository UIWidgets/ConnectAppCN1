using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class FavoriteState {
        public bool favoriteTagLoading { get; set; }
        public bool followFavoriteTagLoading { get; set; }
        public bool favoriteDetailLoading { get; set; }
        public Dictionary<string, List<string>> favoriteTagIdDict { get; set; }
        public Dictionary<string, List<string>> followFavoriteTagIdDict { get; set; }
        public Dictionary<string, List<string>> favoriteDetailArticleIdDict { get; set; }
        public bool favoriteTagHasMore { get; set; }
        public bool followFavoriteTagHasMore { get; set; }
        public bool favoriteDetailHasMore { get; set; }
        public Dictionary<string, FavoriteTag> favoriteTagDict { get; set; }
        public Dictionary<string, FavoriteTagArticle> favoriteTagArticleDict { get; set; }

        public Dictionary<string, Dictionary<string, bool>> collectedTagMap { get; set; }
        public Dictionary<string, string> collectedTagChangeMap { get; set; }
    }
}