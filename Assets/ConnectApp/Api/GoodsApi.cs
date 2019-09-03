using System.Collections.Generic;
using System.Reflection;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.ViewModel;
using RSG;

namespace ConnectApp.Api {
    public static class GoodsApi {
        public static Promise<FetchGoodsDetailResponse> FetchGoodsDetail(string goodsId, bool isPush = false) {
            var promise = new Promise<FetchGoodsDetailResponse>();
            var para = new Dictionary<string, object> {
                {"view", "true"}
            };
            if (isPush) {
                para.Add("isPush", "true");
            }

//            var request = HttpManager.GET($"{Config.apiAddress}/api/connectapp/p/{goodsId}", parameter: para);
//            HttpManager.resume(request).Then(responseText => {
//                var goodsDetailResponse = JsonConvert.DeserializeObject<FetchGoodsDetailResponse>(responseText);
//                promise.Resolve(goodsDetailResponse);
//            }).Catch(exception => { promise.Reject(exception); });
            promise.Resolve(new FetchGoodsDetailResponse {
                    goods = new Goods {
                        id = goodsId,
                        price = 4999,
                        priceUnit = "$",
                        goodsDetailLoading = false,
                        contents = new List<GoodsDetailItem> {
                            new GoodsDetailItem {
                                title = "关于商品",
                                content = "You won't want to take off this best-selling unisex tee, featuring a uniquely soft triblend fabric, modern fit, crew neck and short sleeves."
                            },
                            new GoodsDetailItem {
                                title = "属性",
                                content = "- 3.8 oz.\n" +
                                          "- 50/25/25 polyester/combed and ringspun cotton/rayon"
                            },
                            new GoodsDetailItem {
                                title = "特性",
                                content = "- Side seams, retail fit"
                            }
                        },
                        goodsName = "Logo Hoodie",
                        imageUrls = new List<string> {
                            "https://connect-prd-cdn.unity.com/20190902/p/images/b961e571-8da0-41aa-9e54-fda0fef95ba8_image2_9.png",
                            "https://connect-prd-cdn.unity.com/20190830/p/images/afdcf9a4-6d14-4998-a92c-54779a45a159_SceneView.png",
                            "https://connect-prd-cdn.unity.com/20190829/p/images/303ef688-56dd-43e7-aec6-2373c89105f9_pasted_image_0.png"
                        }
                    }
                }
            );
            return promise;
        }
    }
}