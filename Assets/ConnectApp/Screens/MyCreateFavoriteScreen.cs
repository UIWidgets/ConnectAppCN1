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
    public class MyCreateFavoriteScreenConnector : StatelessWidget {
        public MyCreateFavoriteScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyFavoriteScreenViewModel>(
                converter: state => {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var myFavoriteIds = state.favoriteState.favoriteTagIdDict.ContainsKey(key: currentUserId)
                        ? state.favoriteState.favoriteTagIdDict[key: currentUserId]
                        : new List<string>();
                    return new MyFavoriteScreenViewModel {
                        myFavoriteLoading = state.favoriteState.favoriteTagLoading,
                        myFavoriteIds = myFavoriteIds,
                        myFavoriteHasMore = state.favoriteState.favoriteTagHasMore,
                        currentUserId = currentUserId,
                        favoriteTagDict = state.favoriteState.favoriteTagDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyFavoriteScreenActionModel {
                        startFetchMyFavorite = () => dispatcher.dispatch(new StartFetchFavoriteTagAction()),
                        fetchMyFavorite = offset =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFavoriteTags(userId: viewModel.currentUserId,
                                offset: offset)),
                        deleteFavoriteTag = tagId =>
                            dispatcher.dispatch<IPromise>(Actions.deleteFavoriteTag(tagId: tagId)),
                        pushToCreateFavorite = tagId => dispatcher.dispatch(
                            new MainNavigatorPushToEditFavoriteAction {
                                tagId = tagId
                            }),
                        pushToFavoriteDetail = (userId, tagId) => dispatcher.dispatch(
                            new MainNavigatorPushToFavoriteDetailAction {
                                userId = userId,
                                tagId = tagId,
                                type = FavoriteType.my
                            })
                    };
                    return new MyCreateFavoriteScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class MyCreateFavoriteScreen : StatefulWidget {
        public MyCreateFavoriteScreen(
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
            return new _MyCreateFavoriteScreenState();
        }
    }

    class _MyCreateFavoriteScreenState : AutomaticKeepAliveClientMixin<MyCreateFavoriteScreen> {
        readonly CustomDismissibleController _controller = new CustomDismissibleController();
        readonly RefreshController _refreshController = new RefreshController();
        int _favoriteOffset;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._favoriteOffset = 0;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchMyFavorite();
                this.widget.actionModel.fetchMyFavorite(0);
            });
        }

        void _onRefresh(bool up) {
            this._favoriteOffset = up ? 0 : this.widget.viewModel.myFavoriteIds.Count;
            this.widget.actionModel.fetchMyFavorite(arg: this._favoriteOffset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var myFavorites = this.widget.viewModel.myFavoriteIds;
            Widget content;
            if (this.widget.viewModel.myFavoriteLoading && myFavorites.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (myFavorites.Count <= 0) {
                content = new BlankView(
                    "暂无我的收藏列表",
                    "image/default-following",
                    true,
                    () => {
                        this.widget.actionModel.startFetchMyFavorite();
                        this.widget.actionModel.fetchMyFavorite(0);
                    }
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.myFavoriteHasMore;
                content = new Container(
                    color: CColors.Background,
                    child: new CustomListView(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: enablePullUp,
                        onRefresh: this._onRefresh,
                        itemCount: myFavorites.Count,
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
            var favoriteId = this.widget.viewModel.myFavoriteIds[index: index];
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
                                title: "确定删除收藏夹及收藏夹中的内容？",
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
                ),
                new EditActionButton(
                    80,
                    EdgeInsets.only(12, right: 24),
                    () => this.widget.actionModel.pushToCreateFavorite(obj: favoriteTag.id)
                )
            };
        }
    }
}