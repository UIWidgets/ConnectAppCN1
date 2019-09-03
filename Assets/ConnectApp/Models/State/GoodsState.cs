using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;

namespace ConnectApp.Models.State {
    [Serializable]
    public class GoodsState {
        public bool goodsDetailLoading { get; set; }
        public Dictionary<string, Goods> goodsDict { get; set; }
    }
}