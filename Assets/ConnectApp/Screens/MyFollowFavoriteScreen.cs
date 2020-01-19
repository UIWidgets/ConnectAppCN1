using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
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
    public class MyFollowFavoriteScreenConnector : StatelessWidget {
        public MyFollowFavoriteScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyFavoriteScreenViewModel>(
                converter: state => {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var myFollowFavoriteIds = state.favoriteState.followFavoriteTagIdDict.ContainsKey(key: currentUserId)
                        ? state.favoriteState.followFavoriteTagIdDict[key: currentUserId]
                        : new List<string>();
                    return new MyFavoriteScreenViewModel {
                        myFollowFavoriteLoading = state.favoriteState.followFavoriteTagLoading,
                        myFollowFavoriteIds = myFollowFavoriteIds,
                        myFollowFavoriteHasMore = state.favoriteState.followFavoriteTagHasMore,
                        currentUserId = currentUserId,
                        favoriteTagDict = state.favoriteState.favoriteTagDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyFavoriteScreenActionModel {
                        startFetchFollowFavorite = () => dispatcher.dispatch(new StartFetchFollowFavoriteTagAction()),
                        fetchFollowFavorite = offset =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowFavoriteTags(userId: viewModel.currentUserId,
                                offset: offset)),
                        deleteFavoriteTag = tagId =>
                            dispatcher.dispatch<IPromise>(Actions.deleteFavoriteTag(tagId: tagId)),
                        pushToFavoriteDetail = (userId, tagId) => dispatcher.dispatch(
                            new MainNavigatorPushToFavoriteDetailAction {
                                userId = userId,
                                tagId = tagId,
                                type = FavoriteType.follow
                            })
                    };
                    return new MyFollowFavoriteScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class MyFollowFavoriteScreen : StatefulWidget {
        public MyFollowFavoriteScreen(
            MyFavoriteScreenViewModel viewModel,
            MyFavoriteScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly MyFavoriteScreenViewModel viewModel;
        public readonly MyFavoriteScreenActionModel actionModel;

        public override State createState() {
            return new _MyFollowFavoriteScreenState();
        }
    }

    class _MyFollowFavoriteScreenState : AutomaticKeepAliveClientMixin<MyFollowFavoriteScreen> {
        readonly CustomDismissibleController _controller = new CustomDismissibleController();
        readonly RefreshController _refreshController = new RefreshController();

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchFollowFavorite();
                this.widget.actionModel.fetchFollowFavorite(0);
            });
        }

        void _onRefresh(bool up) {
            var offset = up ? 0 : this.widget.viewModel.myFollowFavoriteIds.Count;
            this.widget.actionModel.fetchFollowFavorite(arg: offset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var myFollowFavoriteIds = this.widget.viewModel.myFollowFavoriteIds;
            Widget content;
            if (this.widget.viewModel.myFollowFavoriteLoading && myFollowFavoriteIds.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (myFollowFavoriteIds.Count <= 0) {
                content = new BlankView(
                    "暂无我关注的收藏",
                    "image/default-following",
                    true,
                    () => {
                        this.widget.actionModel.startFetchFollowFavorite();
                        this.widget.actionModel.fetchFollowFavorite(0);
                    }
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.myFollowFavoriteHasMore;
                content = new Container(
                    color: CColors.Background,
                    child: new CustomListView(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: enablePullUp,
                        onRefresh: this._onRefresh,
                        itemCount: myFollowFavoriteIds.Count,
                        itemBuilder: this._buildFavoriteCard,
                        footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                    )
                );
            }

            return new Container(
                color: CColors.White,
                child: content
            );
        }

        Widget _buildFavoriteCard(BuildContext context, int index) {
            var favoriteId = this.widget.viewModel.myFollowFavoriteIds[index: index];
            var favoriteTagDict = this.widget.viewModel.favoriteTagDict;
            if (!favoriteTagDict.ContainsKey(key: favoriteId)) {
                return new Container();
            }

            var favoriteTag = favoriteTagDict[key: favoriteId];
            return CustomDismissible.builder(
                Key.key(value: favoriteTag.id),
                new FavoriteCard(
                    favoriteTag: favoriteTag,
                    false,
                    () => {
                        CustomDismissible.of(context: context)?.close();
                        this.widget.actionModel.pushToFavoriteDetail(arg1: this.widget.viewModel.currentUserId,
                            arg2: favoriteTag.id);
                    }
                ),
                new CustomDismissibleDrawerDelegate(),
                secondaryActions: this._buildSecondaryActions(favoriteTag: favoriteTag),
                controller: this._controller
            );
        }

        List<Widget> _buildSecondaryActions(FavoriteTag favoriteTag) {
            if (favoriteTag.type == "default") {
                return new List<Widget>();
            }

            return new List<Widget> {
                new DeleteActionButton(
                    80,
                    EdgeInsets.only(24, right: 12),
                    () => {
                        ActionSheetUtils.showModalActionSheet(
                            new ActionSheet(
                                title: "确定要取消关注吗？",
                                items: new List<ActionSheetItem> {
                                    new ActionSheetItem(
                                        "确定",
                                        type: ActionType.normal,
                                        () => this.widget.actionModel.deleteFavoriteTag(arg: favoriteTag.id)
                                    ),
                                    new ActionSheetItem("取消", type: ActionType.cancel)
                                }
                            )
                        );
                    }
                )
            };
        }
    }
}