using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MyFavoriteScreenConnector : StatelessWidget {
        public MyFavoriteScreenConnector(
            Key key = null
        ) : base(key: key) {
            
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyFavoriteScreenViewModel>(
                converter: state => new MyFavoriteScreenViewModel {
                    myFavoriteLoading = state.favoriteState.favoriteTagLoading,
                    myFavoriteIds = state.favoriteState.favoriteTagIds,
                    myFavoriteHasMore = state.favoriteState.favoriteTagHasMore,
                    currentUserId = state.loginState.loginInfo.userId ?? "",
                    favoriteTagDict = state.favoriteState.favoriteTagDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyFavoriteScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
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
                                tagId = tagId
                            })
                    };
                    return new MyFavoriteScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class MyFavoriteScreen : StatefulWidget {
        public MyFavoriteScreen(
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
            return new _MyFavoriteScreenState();
        }
    }

    class _MyFavoriteScreenState : State<MyFavoriteScreen> {
        readonly CustomDismissibleController _controller = new CustomDismissibleController();
        readonly RefreshController _refreshController = new RefreshController();
        int _favoriteOffset;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
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
                        headerWidget: CustomListViewConstant.defaultHeaderWidget,
                        footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                    )
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(context: context),
                                new Expanded(
                                    child: content
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                width: MediaQuery.of(context: context).size.width,
                height: 94,
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(
                        bottom: new BorderSide(color: CColors.Separator2)
                    )
                ),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new CustomButton(
                                    padding: EdgeInsets.symmetric(8, 16),
                                    onPressed: () => this.widget.actionModel.mainRouterPop(),
                                    child: new Icon(
                                        icon: Icons.arrow_back,
                                        size: 24,
                                        color: CColors.Icon
                                    )
                                ),
                                new CustomButton(
                                    padding: EdgeInsets.symmetric(8, 16),
                                    onPressed: () => this.widget.actionModel.pushToCreateFavorite(""),
                                    child: new Text(
                                        "新建",
                                        style: CTextStyle.PLargeBlue
                                    )
                                )
                            }
                        ),
                        new Container(
                            margin: EdgeInsets.only(16, bottom: 8),
                            child: new Text(
                                "我的收藏",
                                style: CTextStyle.H2
                            )
                        )
                    }
                )
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
                                        () => {
                                            this.widget.actionModel.deleteFavoriteTag(arg: favoriteTag.id);
                                        }
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