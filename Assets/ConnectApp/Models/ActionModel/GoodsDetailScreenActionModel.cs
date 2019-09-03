using System;
using RSG;

namespace ConnectApp.Models.ActionModel {
    public class GoodsDetailScreenActionModel : BaseActionModel {
        public Func<string, IPromise> fetchGoodsDetail;
    }
}