using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class LeaderBoardBloggerScreenConnector : StatelessWidget {
        public LeaderBoardBloggerScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, LeaderBoardScreenViewModel>(
                converter: state => new LeaderBoardScreenViewModel(),
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardScreenActionModel {
                        pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
                        })
                    };
                    return new LeaderBoardBloggerScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class LeaderBoardBloggerScreen : StatefulWidget {
        public LeaderBoardBloggerScreen(
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
            return new _LeaderBoardBloggerScreenState();
        }
    }

    class _LeaderBoardBloggerScreenState : State<LeaderBoardBloggerScreen> {
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
                        new PageStorageKey<string>("博主"),
                        slivers: new List<Widget> {
                            new SliverToBoxAdapter(
                                child: new Container(
                                    child: new Column(
                                        children: new List<Widget> {
                                            new LeaderBoardBloggerHeader(),
                                            new LeaderBoardUpdateTip(),
                                            new CustomDivider(height: 1, color: CColors.Separator2)
                                        }
                                    )
                                )
                            ),
                            new SliverFixedExtentList(
                                del: new SliverChildBuilderDelegate(
                                    builder: this._buildBloggerCard,
                                    20
                                ),
                                itemExtent: 88
                            )
                        }
                    )
                )
            );
        }

        Widget _buildBloggerCard(BuildContext context, int index) {
            return new LeaderBoardBloggerCard(
                index: index,
                () => this.widget.actionModel.pushToUserDetail("5a0535f5880c64001886db04")
            );
        }
    }
}