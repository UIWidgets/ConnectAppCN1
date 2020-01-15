using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class LeaderBoardColumnScreenConnector : StatelessWidget {
        public LeaderBoardColumnScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, LeaderBoardScreenViewModel>(
                converter: state => new LeaderBoardScreenViewModel {
                    columnLoading = state.leaderBoardState.columnLoading,
                    columnRankList = state.leaderBoardState.columnRankList,
                    columnHasMore = state.leaderBoardState.columnHasMore,
                    columnPageNumber = state.leaderBoardState.columnPageNumber
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardScreenActionModel {
                        pushToAlbumAction = () => dispatcher.dispatch(new MainNavigatorPushToAction
                            {routeName = MainNavigatorRoutes.AlbumScreen}),
                        startFetchColumn= () => dispatcher.dispatch(new StartFetchLeaderBoardColumnAction()),
                        fetchColumn = page => dispatcher.dispatch<IPromise>(Actions.fetchLeaderBoardColumn(page: page))
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
        const int firstPageNumber = 1;
        int _columnPageNumber = firstPageNumber;

        public override void initState() {
            base.initState();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchColumn();
                this.widget.actionModel.fetchColumn(arg: firstPageNumber);
            });
        }

        bool _onNotification(ScrollNotification notification) {
            return false;
        }

        public override Widget build(BuildContext context) {
            var columnRankList = this.widget.viewModel.columnRankList;
            Widget content;
            if (this.widget.viewModel.columnLoading && columnRankList.isEmpty()) {
                content = new GlobalLoading(
                    color: CColors.Transparent,
                    loadingColor: LoadingColor.white
                );
            }
            else if (columnRankList.Count <= 0) {
                content = new CustomScrollbar(
                    new CustomScrollView(
                        new PageStorageKey<string>("专栏"),
                        slivers: new List<Widget> {
                            new SliverToBoxAdapter(
                                child: new Container(height: 16)
                            ),
                            new SliverFillRemaining(
                                child: new BlankView(
                                    "暂无专栏",
                                    "image/default-article",
                                    true,
                                    () => {
                                        this.widget.actionModel.startFetchColumn();
                                        this.widget.actionModel.fetchColumn(arg: firstPageNumber);
                                    },
                                    new BoxDecoration(
                                        color: CColors.White,
                                        borderRadius: BorderRadius.only(12, 12)
                                    )
                                )
                            )
                        }
                    )
                );
            }
            else {
                content = new NotificationListener<ScrollNotification>(
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
                                        childCount: columnRankList.Count
                                    ),
                                    itemExtent: 112
                                )
                            }
                        )
                    )
                );
            }
            return new Container(
                child: content
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