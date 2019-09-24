using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class EditFavoriteScreenViewModel {
        public string tagId;
        public Dictionary<string, FavoriteTag> favoriteTagDict;
    }
}