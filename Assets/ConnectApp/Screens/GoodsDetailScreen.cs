using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class GoodsDetailScreenConnector : StatelessWidget {
        public GoodsDetailScreenConnector(
            string goodsId,
            string goodsName,
            int price,
            List<string> imageUrls,
            string priceUnit = "¥",
            Key key = null
        ) : base(key : key) {
            this.goodsId = goodsId;
            this.goodsName = goodsName;
            this.price = price;
            this.priceUnit = priceUnit;
            this.imageUrls = imageUrls;
        }

        readonly string goodsId;
        readonly string goodsName;
        readonly string priceUnit;
        readonly int price;
        readonly List<string> imageUrls;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, GoodsDetailScreenViewModel>(
                converter : state => new GoodsDetailScreenViewModel {
                    goodsId = this.goodsId,
                    goodsName = this.goodsName,
                    price = this.price,
                    priceUnit = this.priceUnit,
                    imageUrls = this.imageUrls,
                },
                builder : (context1, viewModel, dispatcher) => {
                    var actionModel = new GoodsDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                    };
                    return new GoodsDetailScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class GoodsDetailScreen : StatefulWidget {

        public GoodsDetailScreen(
            GoodsDetailScreenViewModel viewModel = null,
            GoodsDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly GoodsDetailScreenViewModel viewModel;
        public readonly GoodsDetailScreenActionModel actionModel;
        
        public override State createState() {
            return new _GoodsDetailScreenState();
        }
    }

    class _GoodsDetailScreenState : State<GoodsDetailScreen> {
        public override Widget build(BuildContext context) {
            var contentWidget = new ListView(
                padding: EdgeInsets.symmetric(0, 15),
                children: new List<Widget>(
                )
            );
                
            var child = new Container(
                color: CColors.Background,
                child: new Column(
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        this._buildGoodsImage(),
                        new Expanded(
                            child: new CustomScrollbar(
                                child: contentWidget
                            )
                        ),
                    }
                )
            );
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: child
                )
            );
        }
        
        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    this.widget.viewModel.goodsName,
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildGoodsImage() {
            return null;
        }
    }
}