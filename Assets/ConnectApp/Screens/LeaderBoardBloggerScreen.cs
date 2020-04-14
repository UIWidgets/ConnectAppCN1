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
    public class LeaderBoardBloggerScreenConnector : StatelessWidget {
        public LeaderBoardBloggerScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, LeaderBoardScreenViewModel>(
                converter: state => new LeaderBoardScreenViewModel {
                    bloggerLoading = state.leaderBoardState.bloggerLoading,
                    bloggerIds = state.leaderBoardState.bloggerIds,
                    bloggerHasMore = state.leaderBoardState.bloggerHasMore,
                    bloggerPageNumber = state.leaderBoardState.bloggerPageNumber,
                    userDict = state.userState.userDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardScreenActionModel {
                        startFetchBlogger = () => dispatcher.dispatch(new StartFetchLeaderBoardBloggerAction()),
                        fetchBlogger = page => dispatcher.dispatch<IPromise>(Actions.fetchLeaderBoardBlogger(page: page)),
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
        const int firstPageNumber = 1;
        int leaderBoardBloggerPageNumber = firstPageNumber;
        bool _isLoading;

        public override void initState() {
            base.initState();
            this._isLoading = false;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchBlogger();
                this.widget.actionModel.fetchBlogger(arg: firstPageNumber);
            });
        }

        bool _onNotification(ScrollNotification notification) {
            var enablePullUp = this.widget.viewModel.bloggerHasMore;
            if (!enablePullUp) {
                return false;
            }

            if (notification.metrics.pixels >= notification.metrics.maxScrollExtent && !this._isLoading) {
                this.setState(() => this._isLoading = true);
                this.leaderBoardBloggerPageNumber++;
                this.widget.actionModel.fetchBlogger(arg: this.leaderBoardBloggerPageNumber)
                    .Then(() => this.setState(() => this._isLoading = false))
                    .Catch(_ => this.setState(() => this._isLoading = false));
            }
            return false;
        }

        public override Widget build(BuildContext context) {
            var bloggerIds = this.widget.viewModel.bloggerIds;
            Widget content;
            if (this.widget.viewModel.bloggerLoading && bloggerIds.isEmpty()) {
                content = new GlobalLoading(
                    color: CColors.Transparent,
                    loadingColor: LoadingColor.white
                );
            }
            else if (bloggerIds.Count <= 0) {
                content = new CustomScrollbar(
                    new CustomScrollView(
                        new PageStorageKey<string>("博主"),
                        physics: new AlwaysScrollableScrollPhysics(),
                        slivers: new List<Widget> {
                            new SliverToBoxAdapter(
                                child: new Container(height: 16)
                            ),
                            new SliverFillRemaining(
                                child: new BlankView(
                                    "暂无博主",
                                    "image/default-following",
                                    true,
                                    () => {
                                        this.widget.actionModel.startFetchBlogger();
                                        this.widget.actionModel.fetchBlogger(arg: firstPageNumber);
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
                var enablePullUp = this.widget.viewModel.bloggerHasMore;
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
                            new PageStorageKey<string>("博主"),
                            physics: new AlwaysScrollableScrollPhysics(),
                            slivers: new List<Widget> {
                                new SliverToBoxAdapter(
                                    child: new Container(
                                        child: new Column(
                                            children: new List<Widget> {
                                                new LeaderBoardBloggerHeader(
                                                    bloggerIds: bloggerIds,
                                                    userDict: this.widget.viewModel.userDict,
                                                    userId => this.widget.actionModel.pushToUserDetail(obj: userId)
                                                ),
                                                new LeaderBoardUpdateTip(),
                                                new CustomDivider(height: 1, color: CColors.Separator2)
                                            }
                                        )
                                    )
                                ),
                                new SliverList(
                                    del: new SliverChildBuilderDelegate(
                                        builder: this._buildBloggerCard,
                                        childCount: bloggerIds.Count
                                    )
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

        Widget _buildBloggerCard(BuildContext context, int index) {
            if (index < 3) {
                return new Container();
            }

            var bloggerId = this.widget.viewModel.bloggerIds[index: index];
            if (!this.widget.viewModel.userDict.ContainsKey(key: bloggerId)) {
                return new Container();
            }

            var blogger = this.widget.viewModel.userDict[key: bloggerId];
            return new LeaderBoardBloggerCard(
                blogger: blogger,
                index: index,
                () => this.widget.actionModel.pushToUserDetail(obj: blogger.id)
            );
        }
    }
}