using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.ViewModel {
    public class GoodsDetailScreenViewModel {
        public string goodsId;
        public Dictionary<string, Goods> goodsDict;
        public bool goodsDetailLoading;
    }

    public class GoodsDetailItem {
        public string title;
        public string content;
    }
}