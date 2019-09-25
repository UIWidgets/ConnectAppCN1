using System;
using ConnectApp.Models.Model;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class EditFavoriteScreenActionModel : BaseActionModel {
        public Func<string, IconStyle, string, string, IPromise> editFavoriteTag;
        public Func<IconStyle, string, string, IPromise> createFavoriteTag;
    }
}