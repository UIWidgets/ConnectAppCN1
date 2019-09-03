using System.Collections.Generic;
using System.Linq;
using ConnectApp.Components;
using ConnectApp.Components.Swiper;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class GoodsDetailScreenConnector : StatelessWidget {
        public GoodsDetailScreenConnector(
            string goodsId,
            bool isPush = false,
            Key key = null
        ) : base(key: key) {
            this.goodsId = goodsId;
            this.isPush = isPush;
        }

        readonly string goodsId;
        readonly bool isPush;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, GoodsDetailScreenViewModel>(
                converter: state => new GoodsDetailScreenViewModel {
                    goodsId = this.goodsId,
                    goodsDict = state.goodsState.goodsDict,
                    goodsDetailLoading = state.goodsState.goodsDetailLoading
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new GoodsDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        fetchGoodsDetail = id =>
                            dispatcher.dispatch<IPromise>(Actions.FetchGoodsDetail(id, this.isPush)),
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
        Goods _goods = new Goods();

        public override void initState() {
            base.initState();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.fetchGoodsDetail(arg: this.widget.viewModel.goodsId);
            });
        }

        public override Widget build(BuildContext context) {
            if (!this.widget.viewModel.goodsDict.TryGetValue(this.widget.viewModel.goodsId, out this._goods)) {
                return new Container();
            }

            var content = new List<Widget> {
                this._buildGoodsNamePrice(),
                new Divider(height: 1, color: CColors.VeryLightPink)
            };
            content.AddRange(this._buildContentList());

            var child = new Container(
                color: CColors.White,
                child: new Column(
                    mainAxisSize: MainAxisSize.min,
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        // this._buildGoodsImage(),
                        new Expanded(
                            child: new ListView(
                                padding: EdgeInsets.symmetric(0, 15),
                                children: content
                            )
                        )
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
                    this._goods.goodsName ?? "",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildGoodsImage() {
            if (this._goods.imageUrls?.isEmpty() ?? true) {
                return new Container(
                    width: MediaQuery.of(this.context).size.width,
                    height: MediaQuery.of(this.context).size.width
                );
            }

            return new Swiper(
                itemBuilder: (context, index) => {
                    return Image.network(
                        this._goods.imageUrls[index],
                        fit: BoxFit.cover
                    );
                },
                indicatorLayout: PageIndicatorLayout.SLIDE,
                itemCount: this._goods.imageUrls.Count,
                itemWidth: MediaQuery.of(this.context).size.width,
                itemHeight: MediaQuery.of(this.context).size.width // Make the image square
            );
        }

        Widget _buildGoodsNamePrice() {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.symmetric(17, 0),
                child: new SizedBox(height: 38,
                    child: new Row(
                        children: new List<Widget> {
                            new Expanded(
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Text(this._goods.goodsName ?? "", style: CTextStyle.PRegularTitle, textAlign: TextAlign.left),
                                        new Expanded(
                                            child: new Text(
                                                $"{this._goods.priceUnit}{this._goods.price}",
                                                style: CTextStyle.PRegularBlue,
                                                textAlign: TextAlign.left)
                                        )
                                    }
                                )
                            ),
                            new CustomButton(
                                padding: EdgeInsets.zero,
                                onPressed: () => {},
                                child: new Container(
                                    padding: EdgeInsets.symmetric(9, 0),
                                    child: new Text("立即购买",
                                        style: CTextStyle.PRegularButton,
                                        textAlign: TextAlign.center),
                                    width: 90,
                                    height: 38,
                                    decoration: new BoxDecoration(
                                        border: Border.all(color: CColors.Black, width: 2),
                                        borderRadius: BorderRadius.all(Radius.circular(3))
                                    )
                                )
                            )
                        }
                    )
                )
            );
        }

        List<Widget> _buildContentList() {
            if (this._goods.contents?.isEmpty() ?? true) {
                return new List<Widget>();
            }

            return this._goods.contents.Select<GoodsDetailItem, Widget>(
                item => {
                    return new Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            new Container(height: 16),
                            new Text(item.title, style: CTextStyle.PXLargeBold),
                            new Opacity(
                                opacity: 0.58f,
                                child: new Text(item.content, style: CTextStyle.PRegularTitle)
                            )
                        }
                    );
                }
            ).ToList();
        }
    }
}