using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
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
                    columnIds = state.leaderBoardState.columnIds,
                    columnHasMore = state.leaderBoardState.columnHasMore,
                    columnPageNumber = state.leaderBoardState.columnPageNumber,
                    userDict = state.userState.userDict,
                    userArticleDict = state.articleState.userArticleDict,
                    rankDict = state.leaderBoardState.rankDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardScreenActionModel {
                        pushToDetailAction = id => dispatcher.dispatch(new MainNavigatorPushToLeaderBoardDetailAction
                            {id = id, type = LeaderBoardType.column}),
                        startFetchColumn = () => dispatcher.dispatch(new StartFetchLeaderBoardColumnAction()),
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
        int leaderBoardColumnPageNumber = firstPageNumber;
        bool _isLoading;

        public override void initState() {
            base.initState();
            this._isLoading = false;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchColumn();
                this.widget.actionModel.fetchColumn(arg: firstPageNumber);
            });
        }

        bool _onNotification(ScrollNotification notification) {
            var enablePullUp = this.widget.viewModel.columnHasMore;
            if (!enablePullUp) {
                return false;
            }

            if (notification.metrics.pixels >= notification.metrics.maxScrollExtent && !this._isLoading) {
                this.setState(() => this._isLoading = true);
                this.leaderBoardColumnPageNumber++;
                this.widget.actionModel.fetchColumn(arg: this.leaderBoardColumnPageNumber)
                    .Then(() => this.setState(() => this._isLoading = false))
                    .Catch(_ => this.setState(() => this._isLoading = false));
            }

            return false;
        }

        public override Widget build(BuildContext context) {
            var columnIds = this.widget.viewModel.columnIds;
            Widget content;
            if (this.widget.viewModel.columnLoading && columnIds.isEmpty()) {
                content = new GlobalLoading(
                    color: CColors.Transparent,
                    loadingColor: LoadingColor.white
                );
            }
            else if (columnIds.Count <= 0) {
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
                var enablePullUp = this.widget.viewModel.columnHasMore;
                Widget endView;
                if (!enablePullUp) {
                    endView = new EndView();
                }
                else {
                    endView = new Visibility(
                        visible: this._isLoading,
                        child: new Container(
                            color: CColors.Background,
                            padding: EdgeInsets.symmetric(16),
                            child: new CustomActivityIndicator(
                                loadingColor: LoadingColor.black,
                                animating: this._isLoading ? AnimatingType.repeat : AnimatingType.reset
                            )
                        )
                    );
                }

                content = new NotificationListener<ScrollNotification>(
                    onNotification: this._onNotification,
                    child: new CustomScrollbar(
                        new CustomScrollView(
                            new PageStorageKey<string>("专栏"),
                            physics: new AlwaysScrollableScrollPhysics(),
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
                                        childCount: columnIds.Count
                                    ),
                                    itemExtent: 112
                                ),
                                new SliverToBoxAdapter(
                                    child: endView
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
            var columnId = this.widget.viewModel.columnIds[index: index];
            var rankData = this.widget.viewModel.rankDict[key: columnId];
            return new LeaderBoardColumnCard(
                column: rankData,
                this.widget.viewModel.userDict[key: rankData.itemId],
                this.widget.viewModel.userArticleDict[key: rankData.itemId],
                index: index,
                onPress: () => this.widget.actionModel.pushToDetailAction(columnId)
            );
        }
    }
}