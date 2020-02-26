using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class GameDetailScreenActionModel : BaseActionModel {
        public Action startFetchGameDetail;
        public Func<IPromise> fetchGameDetail;
    }
}