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
    public class LeaderBoardCollectionScreenConnector : StatelessWidget {
        public LeaderBoardCollectionScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, LeaderBoardScreenViewModel>(
                converter: state => new LeaderBoardScreenViewModel {
                    collectionLoading = state.leaderBoardState.collectionLoading,
                    collectionRankList = state.leaderBoardState.collectionRankList,
                    collectionHasMore = state.leaderBoardState.collectionHasMore,
                    collectionPageNumber = state.leaderBoardState.collectionPageNumber,
                    favoriteTagDict = state.favoriteState.favoriteTagDict,
                    favoriteTagArticleDict = state.favoriteState.favoriteTagArticleDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardScreenActionModel {
                        pushToAlbumAction = () => dispatcher.dispatch(new MainNavigatorPushToAction
                            {routeName = MainNavigatorRoutes.AlbumScreen}),
                        startFetchCollection = () => dispatcher.dispatch(new StartFetchLeaderBoardCollectionAction()),
                        fetchCollection = page => dispatcher.dispatch<IPromise>(Actions.fetchLeaderBoardCollection(page: page))
                    };
                    return new LeaderBoardCollectionScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class LeaderBoardCollectionScreen : StatefulWidget {
        public LeaderBoardCollectionScreen(
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
            return new _LeaderBoardCollectionScreenState();
        }
    }

    class _LeaderBoardCollectionScreenState : State<LeaderBoardCollectionScreen> {
        const int firstPageNumber = 1;
        int _collectionPageNumber = firstPageNumber;

        public override void initState() {
            base.initState();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchCollection();
                this.widget.actionModel.fetchCollection(arg: firstPageNumber);
            });
        }

        bool _onNotification(ScrollNotification notification) {
            return false;
        }

        public override Widget build(BuildContext context) {
            var collectionRankList = this.widget.viewModel.collectionRankList;
            Widget content;
            if (this.widget.viewModel.collectionLoading && collectionRankList.isEmpty()) {
                content = new GlobalLoading(
                    color: CColors.Transparent,
                    loadingColor: LoadingColor.white
                );
            }
            else if (collectionRankList.Count <= 0) {
                content = new CustomScrollbar(
                    new CustomScrollView(
                        new PageStorageKey<string>("合辑"),
                        slivers: new List<Widget> {
                            new SliverToBoxAdapter(
                                child: new Container(height: 16)
                            ),
                            new SliverFillRemaining(
                                child: new BlankView(
                                    "暂无合辑",
                                    "image/default-article",
                                    true,
                                    () => {
                                        this.widget.actionModel.startFetchCollection();
                                        this.widget.actionModel.fetchCollection(arg: firstPageNumber);
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
                var enablePullUp = this.widget.viewModel.collectionHasMore;
                Widget endView;
                if (!enablePullUp) {
                    endView = new EndView();
                }
                else {
                    endView = new Container();
                }
                content = new NotificationListener<ScrollNotification>(
                    onNotification: this._onNotification,
                    child: new CustomScrollbar(
                        new CustomScrollView(
                            new PageStorageKey<string>("合辑"),
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
                                        builder: this._buildCollectionCard,
                                        childCount: collectionRankList.Count
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

        Widget _buildCollectionCard(BuildContext context, int index) {
            var collection = this.widget.viewModel.collectionRankList[index: index];
            return new LeaderBoardCollectionCard(
                collection: collection,
                favoriteTags: this.widget.viewModel.favoriteTagDict,
                favoriteTagArticles: this.widget.viewModel.favoriteTagArticleDict,
                index: index,
                () => this.widget.actionModel.pushToAlbumAction()
            );
        }
    }
}