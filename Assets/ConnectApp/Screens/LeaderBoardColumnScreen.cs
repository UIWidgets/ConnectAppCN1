using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class LeaderBoardColumnScreenConnector : StatelessWidget {
        public LeaderBoardColumnScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, LeaderBoardScreenViewModel>(
                converter: state => new LeaderBoardScreenViewModel(),
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardScreenActionModel {
                        pushToAlbumAction = () => dispatcher.dispatch(new MainNavigatorPushToAction
                            {routeName = MainNavigatorRoutes.AlbumScreen})
                    };
                    return new LeaderBoardColumnScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class LeaderBoardColumnScreen : StatefulWidget {
        public LeaderBoardColumnScreen(
            LeaderBoardScreenViewModel viewModel,
            LeaderBoardScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly LeaderBoardScreenViewModel viewModel;
        public readonly LeaderBoardScreenActionModel actionModel;

        public override State createState() {
            return new _LeaderBoardColumnScreenState();
        }
    }

    class _LeaderBoardColumnScreenState : State<LeaderBoardColumnScreen> {
        public override void initState() {
            base.initState();
        }

        bool _onNotification(ScrollNotification notification) {
            return false;
        }

        public override Widget build(BuildContext context) {
            return new NotificationListener<ScrollNotification>(
                onNotification: this._onNotification,
                child: new CustomScrollbar(
                    new CustomScrollView(
                        new PageStorageKey<string>("专栏"),
                        slivers: new List<Widget> {
                            new SliverToBoxAdapter(
                                child: new Container(
                                    child: new Column(
                                        children: new List<Widget> {
                                            new SizedBox(height: 16),
                                            new LeaderBoardUpdateTip(),
                                            new CustomDivider(height: 1, color: CColors.Separator2)
                                        }
                                    )
                                )
                            ),
                            new SliverFixedExtentList(
                                del: new SliverChildBuilderDelegate(
                                    builder: this._buildColumnCard,
                                    20
                                ),
                                itemExtent: 112
                            )
                        }
                    )
                )
            );
        }

        Widget _buildColumnCard(BuildContext context, int index) {
            return new LeaderBoardColumnCard(
                index: index,
                onPress: () => this.widget.actionModel.pushToAlbumAction()
            );
        }
    }
}