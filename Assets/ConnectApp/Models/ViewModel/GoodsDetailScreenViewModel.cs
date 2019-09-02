using System.Collections.Generic;

namespace ConnectApp.Models.ViewModel {
    public class GoodsDetailScreenViewModel {
        public string goodsId;
        public string goodsName;
        public int price;
        public string priceUnit;
        public bool goodsDetailLoading;
        public List<string> imageUrls;
    }
}