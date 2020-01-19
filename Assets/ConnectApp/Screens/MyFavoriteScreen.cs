using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MyFavoriteScreenConnector : StatelessWidget {
        public MyFavoriteScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, object>(
                converter: state => null,
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyFavoriteScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToCreateFavorite = tagId => dispatcher.dispatch(
                            new MainNavigatorPushToEditFavoriteAction {
                                tagId = tagId
                            })
                    };
                    return new MyFavoriteScreen(actionModel: actionModel);
                }
            );
        }
    }

    public class MyFavoriteScreen : StatelessWidget {
        public MyFavoriteScreen(
            MyFavoriteScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.actionModel = actionModel;
        }

        readonly MyFavoriteScreenActionModel actionModel;

        public override Widget build(BuildContext context) {
            StatusBarManager.statusBarStyle(false);
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(
                                    child: new CustomSegmentedControl(
                                        new List<object> {"我创建的", "我关注的"},
                                        new List<Widget> {
                                            new MyCreateFavoriteScreenConnector(),
                                            new MyFollowFavoriteScreenConnector()
                                        }
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text(
                    "我的收藏",
                    style: CTextStyle.H2
                ),
                topRightWidget: new CustomButton(
                    padding: EdgeInsets.symmetric(8, 16),
                    onPressed: () => this.actionModel.pushToCreateFavorite(""),
                    child: new Text(
                        "新建",
                        style: CTextStyle.PLargeBlue
                    )
                ),
                onBack: () => this.actionModel.mainRouterPop()
            );
        }
    }
}