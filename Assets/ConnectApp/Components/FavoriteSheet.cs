using System.Collections.Generic;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class FavoriteSheetConnector : StatelessWidget {
        public FavoriteSheetConnector(
            string articleId,
            Key key = null
        ) : base(key: key) {
            this.articleId = articleId;
        }

        readonly string articleId;

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
                        article = state.articleState.articleDict[key: this.articleId],
                        favoriteTagDict = state.favoriteState.favoriteTagDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyFavoriteScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        startFetchMyFavorite = () => dispatcher.dispatch(new StartFetchFavoriteTagAction()),
                        fetchMyFavorite = offset =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFavoriteTags(userId: viewModel.currentUserId,
                                offset: offset)),
                        favoriteArticle = tagIds =>
                            dispatcher.dispatch<IPromise>(Actions.favoriteArticle(articleId: this.articleId,
                                tagIds: tagIds)),
                        pushToCreateFavorite = tagId => dispatcher.dispatch(
                            new MainNavigatorPushToEditFavoriteAction {
                                tagId = tagId
                            })
                    };
                    return new FavoriteSheet(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class FavoriteSheet : StatefulWidget {
        public FavoriteSheet(
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
            return new _FavoriteSheetState();
        }
    }

    class _FavoriteSheetState : State<FavoriteSheet> {
        readonly RefreshController _refreshController = new RefreshController();
        int _favoriteOffset;
        readonly List<string> _checkTagIds = new List<string>();

        public override void initState() {
            base.initState();
            this._favoriteOffset = 0;
            this.widget.viewModel.article.favorites?.ForEach(favorite => {
                this._checkTagIds.Add(item: favorite.tagId);
            });

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
            var mediaQueryData = MediaQuery.of(context: context);

            return new Column(
                mainAxisAlignment: MainAxisAlignment.end,
                children: new List<Widget> {
                    new Container(
                        decoration: new BoxDecoration(
                            color: CColors.White,
                            borderRadius: BorderRadius.only(12, 12)
                        ),
                        width: mediaQueryData.size.width,
                        height: 384 + mediaQueryData.padding.bottom,
                        child: new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                this._buildFavoriteTitle(),
                                this._buildCreateFavoriteTag(),
                                new Expanded(
                                    child: this._buildFavoriteTags()
                                ),
                                new Container(
                                    height: mediaQueryData.padding.bottom
                                )
                            }
                        )
                    )
                }
            );
        }

        Widget _buildFavoriteTitle() {
            return new Container(
                height: 58,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.all(16),
                            onPressed: ActionSheetUtils.hiddenModalPopup,
                            child: new Text("取消", style: CTextStyle.PLargeBody5)
                        ),
                        new Text("收藏到", style: CTextStyle.PXLargeMedium),
                        new CustomButton(
                            padding: EdgeInsets.all(16),
                            onPressed: () => {
                                ActionSheetUtils.hiddenModalPopup();
                                this.widget.actionModel.favoriteArticle(arg: this._checkTagIds);
                            },
                            child: new Text("完成", style: CTextStyle.PLargeBlue)
                        )
                    }
                )
            );
        }

        Widget _buildCreateFavoriteTag() {
            return new GestureDetector(
                onTap: () => this.widget.actionModel.pushToCreateFavorite(""),
                child: new Container(
                    height: 54,
                    padding: EdgeInsets.only(16, 19, bottom: 10),
                    decoration: new BoxDecoration(
                        color: CColors.White,
                        border: new Border(
                            new BorderSide(color: CColors.Separator2)
                        )
                    ),
                    child: new Row(
                        children: new List<Widget> {
                            new Icon(
                                icon: Icons.add,
                                color: CColors.PrimaryBlue, 
                                size: 24
                            ),
                            new Container(
                                margin: EdgeInsets.only(8),
                                child: new Text("新建收藏夹",
                                    style: CTextStyle.PLargeMediumBlue.merge(new TextStyle(height: 1)))
                            )
                        }
                    )
                )
            );
        }

        Widget _buildFavoriteTags() {
            var myFavoriteIds = this.widget.viewModel.myFavoriteIds;
            if (this.widget.viewModel.myFavoriteLoading && myFavoriteIds.isEmpty()) {
                return new GlobalLoading();
            }

            if (myFavoriteIds.Count <= 0) {
                return new BlankView(
                    "暂无我的收藏列表",
                    "image/default-following"
                );
            }

            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: this.widget.viewModel.myFavoriteHasMore,
                        onRefresh: this._onRefresh,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: myFavoriteIds.Count,
                            itemBuilder: (cxt, index) => {
                                var favoriteId = myFavoriteIds[index: index];
                                var favoriteTagDict = this.widget.viewModel.favoriteTagDict;
                                if (!favoriteTagDict.ContainsKey(key: favoriteId)) {
                                    return new Container();
                                }

                                var favoriteTag = favoriteTagDict[key: favoriteId];
                                return new FavoriteCard(
                                    favoriteTag: favoriteTag,
                                    true,
                                    () => {
                                        if (this._checkTagIds.Contains(item: favoriteTag.id)) {
                                            this._checkTagIds.Remove(item: favoriteTag.id);
                                        }
                                        else {
                                            this._checkTagIds.Add(item: favoriteTag.id);
                                        }
                                        this.setState(() => { });
                                    },
                                    this._checkTagIds.Contains(item: favoriteTag.id)
                                );
                            }
                        )
                    )
                )
            );
        }
    }
}