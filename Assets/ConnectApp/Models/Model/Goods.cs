using System;
using System.Collections.Generic;
using ConnectApp.Models.ViewModel;

namespace ConnectApp.Models.Model {
    [Serializable]
    public class Goods {
        public string id;
        public string goodsName;
        public int price;
        public string priceUnit;
        public bool goodsDetailLoading;
        public List<string> imageUrls;
        public List<GoodsDetailItem> contents;
    }
}