using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class GameScreenActionModel : BaseActionModel {
        public Action startFetchGame;
        public Func<int, IPromise> fetchGame;
        public Action<string> pushToGameDetail;
    }
}