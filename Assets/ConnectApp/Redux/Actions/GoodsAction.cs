using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;
using UnityEngine;

namespace ConnectApp.redux.actions {
    public class FetchGoodsDetailSuccessAction {
        public Goods goods;
        public string goodsId;
    }

    public class FetchGoodsDetailFailureAction { }

    public static partial class Actions {
        public static object FetchGoodsDetail(string goodsId, bool isPush = false) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return GoodsApi.FetchGoodsDetail(goodsId, isPush)
                    .Then(goodsDetailResponse => {
                        dispatcher.dispatch(new FetchGoodsDetailSuccessAction {
                            goodsId = goodsId,
                            goods = goodsDetailResponse.goods
                        });
                    })
                    .Catch(error => {
                        dispatcher.dispatch(new FetchGoodsDetailFailureAction());
                        Debug.Log(error);
                    });
            });
        }
    }
}