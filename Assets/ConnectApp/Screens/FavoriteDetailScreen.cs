using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public enum FavoriteType {
        my,
        follow
    }

    public class FavoriteDetailScreenConnector : StatelessWidget {
        public FavoriteDetailScreenConnector(
            string tagId,
            string userId,
            FavoriteType type = FavoriteType.my,
            Key key = null
        ) : base(key: key) {
            this.tagId = tagId;
            this.userId = userId;
            this.type = type;
        }

        readonly string tagId;
        readonly string userId;
        readonly FavoriteType type;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, FavoriteDetailScreenViewModel>(
                converter: state => {
                    var favoriteTagDict = state.favoriteState.favoriteTagDict;
                    var favoriteTag = favoriteTagDict.ContainsKey(key: this.tagId)
                        ? favoriteTagDict[key: this.tagId]
                        : null;
                    var favoriteDetailArticleIdDict = state.favoriteState.favoriteDetailArticleIdDict;
                    var favoriteDetailArticleIds = favoriteDetailArticleIdDict.ContainsKey(key: this.tagId)
                        ? favoriteDetailArticleIdDict[key: this.tagId]
                        : null;
                    return new FavoriteDetailScreenViewModel {
                        userId = this.userId,
                        tagId = this.tagId,
                        type = this.type,
                        favoriteDetailLoading = state.favoriteState.favoriteDetailLoading,
                        favoriteDetailArticleIds = favoriteDetailArticleIds,
                        favoriteArticleOffset = favoriteDetailArticleIds?.Count ?? 0,
                        favoriteArticleHasMore = state.favoriteState.favoriteDetailHasMore,
                        isLoggedIn = state.loginState.isLoggedIn,
                        favoriteTag = favoriteTag,
                        articleDict = state.articleState.articleDict,
                        userDict = state.userState.userDict,
                        teamDict = state.teamState.teamDict,
                        currentUserId = state.loginState.loginInfo.userId ?? ""
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new FavoriteDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        startFetchFavoriteDetail = () => dispatcher.dispatch(new StartFetchFavoriteDetailAction()),
                        fetchFavoriteDetail = (tagId, offset) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFavoriteDetail(userId: this.userId, tagId: tagId,
                                offset: offset)),
                        unFavoriteArticle = (articleId, favoriteId) =>
                            dispatcher.dispatch<IPromise>(Actions.unFavoriteArticle(articleId: articleId,
                                favoriteId: favoriteId)),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToReport = reportId => dispatcher.dispatch(
                            new MainNavigatorPushToReportAction {
                                reportId = reportId,
                                reportType = ReportType.article
                            }
                        ),
                        pushToBlock = articleId => {
                            dispatcher.dispatch(new BlockArticleAction {articleId = articleId});
                            dispatcher.dispatch(new DeleteArticleHistoryAction {articleId = articleId});
                        },
                        pushToArticleDetail = articleId => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = articleId
                            }
                        ),
                        pushToEditFavorite = tagId => dispatcher.dispatch(
                            new MainNavigatorPushToEditFavoriteAction {
                                tagId = tagId
                            }
                        ),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type: type, title: title, description: description, linkUrl: linkUrl,
                                imageUrl: imageUrl))
                    };
                    return new FavoriteDetailScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class FavoriteDetailScreen : StatefulWidget {
        public FavoriteDetailScreen(
            FavoriteDetailScreenViewModel viewModel,
            FavoriteDetailScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly FavoriteDetailScreenViewModel viewModel;
        public readonly FavoriteDetailScreenActionModel actionModel;

        public override State createState() {
            return new _FavoriteDetailScreenState();
        }
    }

    class _FavoriteDetailScreenState : State<FavoriteDetailScreen>, TickerProvider {
        readonly CustomDismissibleController _dismissibleController = new CustomDismissibleController();
        readonly RefreshController _refreshController = new RefreshController();
        readonly GlobalKey _favoriteInfoKey = GlobalKey.key("favorite-info");
        float _favoriteInfoHeight;
        int _favoriteArticleOffset;
        bool _isHaveTitle;
        Animation<RelativeRect> _animation;
        AnimationController _controller;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._favoriteInfoHeight = 0;
            this._favoriteArticleOffset = 0;
            this._isHaveTitle = false;
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            RelativeRectTween rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, 44, 0, 0),
                RelativeRect.fromLTRB(0, 13, 0, 0)
            );
            this._animation = rectTween.animate(parent: this._controller);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchFavoriteDetail();
                this.widget.actionModel.fetchFavoriteDetail(arg1: this.widget.viewModel.tagId, 0);
            });
            WidgetsBinding.instance.addPostFrameCallback(_ => {
                var renderBox = (RenderBox) this._favoriteInfoKey.currentContext.findRenderObject();
                var favoriteInfoSize = renderBox.size;
                if (this._favoriteInfoHeight != favoriteInfoSize.height) {
                    this.setState(() => this._favoriteInfoHeight = favoriteInfoSize.height);
                }
            });
        }

        public override void dispose() {
            this._controller.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        void _onRefresh(bool up) {
            this._favoriteArticleOffset = up ? 0 : this.widget.viewModel.favoriteArticleOffset;
            this.widget.actionModel
                .fetchFavoriteDetail(arg1: this.widget.viewModel.tagId, arg2: this._favoriteArticleOffset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;

            if (pixels > this._favoriteInfoHeight - 16) {
                if (!this._isHaveTitle) {
                    this._controller.forward();
                    this.setState(() => this._isHaveTitle = true);
                }
            }
            else {
                if (this._isHaveTitle) {
                    this._controller.reverse();
                    this.setState(() => this._isHaveTitle = false);
                }
            }

            return true;
        }

        bool _hasFavoriteArticle() {
            var favoriteDetailArticleIds = this.widget.viewModel.favoriteDetailArticleIds;
            return favoriteDetailArticleIds != null && favoriteDetailArticleIds.Count > 0;
        }

        public override Widget build(BuildContext context) {
            var favoriteTag = this.widget.viewModel.favoriteTag;
            Widget content;
            if (this.widget.viewModel.favoriteDetailLoading && favoriteTag == null) {
                content = new GlobalLoading();
            }
            else if (favoriteTag == null) {
                content = new BlankView(
                    "暂无我的收藏详情",
                    "image/default-following",
                    true,
                    () => {
                        this.widget.actionModel.startFetchFavoriteDetail();
                        this.widget.actionModel.fetchFavoriteDetail(arg1: this.widget.viewModel.tagId, 0);
                    }
                );
            }
            else {
                content = new NotificationListener<ScrollNotification>(
                    child: this._buildFavoriteContent(),
                    onNotification: this._onNotification
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Column(
                        children: new List<Widget> {
                            this._buildNavigationBar(),
                            new Expanded(
                                child: content
                            )
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            var tagId = this.widget.viewModel.tagId;
            var favoriteTag = this.widget.viewModel.favoriteTag;

            string title;
            Widget rightWidget;
            if (favoriteTag == null) {
                title = "";
                rightWidget = new Container(width: 56);
            }
            else {
                if (favoriteTag.type == "default") {
                    title = "默认";
                    rightWidget = new Container(width: 56);
                }
                else {
                    title = favoriteTag.name;
                    if (!this.widget.viewModel.isLoggedIn) {
                        rightWidget = new Container(width: 56);
                    } else if (!this.widget.viewModel.userId.Equals(value: this.widget.viewModel.currentUserId)) {
                        rightWidget = new Container(width: 56);
                    } else if (this.widget.viewModel.type == FavoriteType.follow) {
                        rightWidget = new Container(width: 56);
                    }
                    else {
                        rightWidget = new CustomButton(
                            padding: EdgeInsets.symmetric(8, 16),
                            onPressed: () => this.widget.actionModel.pushToEditFavorite(obj: tagId),
                            child: new Text(
                                "编辑",
                                style: CTextStyle.PLargeMediumBlue.merge(new TextStyle(height: 1))
                            )
                        );
                    }
                }
            }

            Widget titleWidget;
            if (this._isHaveTitle) {
                titleWidget = new Text(
                    data: title,
                    style: CTextStyle.PXLargeMedium,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    textAlign: TextAlign.center
                );
            }
            else {
                titleWidget = new Container();
            }

            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Expanded(
                    child: new Stack(
                        fit: StackFit.expand,
                        children: new List<Widget> {
                            new PositionedTransition(
                                rect: this._animation,
                                child: titleWidget
                            )
                        }
                    )
                ),
                rightWidget: rightWidget
            );
        }

        Widget _buildFavoriteContent() {
            var favoriteDetailArticleIds = this.widget.viewModel.favoriteDetailArticleIds;
            var favoriteArticleHasMore = this.widget.viewModel.favoriteArticleHasMore;
            bool favoriteArticleLoading =
                this.widget.viewModel.favoriteDetailLoading && favoriteDetailArticleIds == null;
            int itemCount;
            if (favoriteArticleLoading) {
                itemCount = 1;
            }
            else if (!this._hasFavoriteArticle()) {
                itemCount = 1;
            }
            else {
                itemCount = favoriteDetailArticleIds.Count;
            }

            return new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: favoriteArticleHasMore,
                    onRefresh: this._onRefresh,
                    itemCount: itemCount,
                    itemBuilder: (cxt, index) => {
                        if (index == 0 && favoriteArticleLoading) {
                            var height = MediaQuery.of(context: cxt).size.height - this._favoriteInfoHeight - 44;
                            return new Container(
                                height: height,
                                child: new GlobalLoading()
                            );
                        }

                        if (index == 0 && !this._hasFavoriteArticle()) {
                            var height = MediaQuery.of(context: cxt).size.height - this._favoriteInfoHeight - 44;
                            return new Container(
                                height: height,
                                child: new BlankView(
                                    "这里一片荒芜",
                                    "image/default-article"
                                )
                            );
                        }

                        return this._buildArticleCard(index: index);
                    },
                    headerWidget: this._buildFavoriteInfo(),
                    footerWidget: favoriteArticleHasMore || !this._hasFavoriteArticle() ? null : new EndView()
                )
            );
        }

        Widget _buildFavoriteInfo() {
            var favoriteTag = this.widget.viewModel.favoriteTag;
            if (favoriteTag == null) {
                return new Container();
            }

            string title;
            string imageName;
            Color color;
            if (favoriteTag.type == "default") {
                title = "默认";
                imageName = $"{CImageUtils.FavoriteCoverImagePath}/{CImageUtils.FavoriteCoverImages[0]}";
                color = CColorUtils.FavoriteCoverColors[0];
            }
            else {
                title = favoriteTag.name;
                imageName = $"{CImageUtils.FavoriteCoverImagePath}/{favoriteTag.iconStyle.name}";
                color = new Color(long.Parse(s: favoriteTag.iconStyle.bgColor));
            }

            return new Column(
                key: this._favoriteInfoKey,
                children: new List<Widget> {
                    new Container(
                        color: CColors.White,
                        padding: EdgeInsets.all(16),
                        child: new Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: new List<Widget> {
                                new Row(
                                    children: new List<Widget> {
                                        new FavoriteTagCoverImage(
                                            coverImage: imageName,
                                            coverColor: color,
                                            64,
                                            margin: EdgeInsets.only(right: 16)
                                        ),
                                        new Expanded(
                                            child: new Column(
                                                crossAxisAlignment: CrossAxisAlignment.start,
                                                children: new List<Widget> {
                                                    new Text(
                                                        data: title,
                                                        style: CTextStyle.H5,
                                                        maxLines: 1,
                                                        overflow: TextOverflow.ellipsis
                                                    ),
                                                    new SizedBox(height: 4),
                                                    new Text(
                                                        $"{favoriteTag.stasitics?.count ?? 0}个内容",
                                                        style: CTextStyle.PSmallBody4
                                                    )
                                                }
                                            )
                                        )
                                    }
                                ),
                                favoriteTag.description.isNotEmpty()
                                    ? new Container(
                                        margin: EdgeInsets.only(top: 16),
                                        child: new Text(
                                            data: favoriteTag.description,
                                            style: CTextStyle.PRegularBody3.merge(new TextStyle(height: 1))
                                        )
                                    )
                                    : new Container()
                            }
                        )
                    ),
                    new CustomDivider(
                        color: CColors.Background
                    )
                }
            );
        }

        Widget _buildArticleCard(int index) {
            var articleId = this.widget.viewModel.favoriteDetailArticleIds[index: index];
            var articleDict = this.widget.viewModel.articleDict;
            if (!articleDict.ContainsKey(key: articleId)) {
                return new Container();
            }

            var article = articleDict[key: articleId];
            var linkUrl = CStringUtils.JointProjectShareLink(projectId: article.id);
            var fullName = "";
            if (article.ownerType == OwnerType.user.ToString()) {
                if (this.widget.viewModel.userDict.ContainsKey(key: article.userId)) {
                    fullName = this.widget.viewModel.userDict[key: article.userId].fullName
                               ?? this.widget.viewModel.userDict[key: article.userId].name;
                }
            }

            if (article.ownerType == OwnerType.team.ToString()) {
                if (this.widget.viewModel.teamDict.ContainsKey(key: article.teamId)) {
                    fullName = this.widget.viewModel.teamDict[key: article.teamId].name;
                }
            }

            return CustomDismissible.builder(
                Key.key(value: article.id),
                new ArticleCard(
                    article: article,
                    () => this.widget.actionModel.pushToArticleDetail(obj: article.id),
                    () => ShareManager.showArticleShareView(
                        false,
                        isLoggedIn: this.widget.viewModel.isLoggedIn,
                        () => {
                            Clipboard.setData(new ClipboardData(text: linkUrl));
                            CustomDialogUtils.showToast("复制链接成功", iconData: Icons.check_circle_outline);
                        },
                        () => this.widget.actionModel.pushToLogin(),
                        () => this.widget.actionModel.pushToBlock(obj: article.id),
                        () => this.widget.actionModel.pushToReport(obj: article.id),
                        type => {
                            CustomDialogUtils.showCustomDialog(
                                child: new CustomLoadingDialog()
                            );
                            string imageUrl = CImageUtils.SizeTo200ImageUrl(imageUrl: article.thumbnail.url);
                            this.widget.actionModel.shareToWechat(arg1: type, arg2: article.title,
                                    arg3: article.subTitle, arg4: linkUrl, arg5: imageUrl)
                                .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                                .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                        }
                    ),
                    fullName: fullName,
                    new ObjectKey(value: article.id)
                ),
                new CustomDismissibleDrawerDelegate(),
                secondaryActions: this._buildSecondaryActions(article: article),
                controller: this._dismissibleController
            );
        }

        List<Widget> _buildSecondaryActions(Article article) {
            if (!this.widget.viewModel.isLoggedIn) {
                return new List<Widget>();
            }

            if (!this.widget.viewModel.userId.Equals(value: this.widget.viewModel.currentUserId)) {
                return new List<Widget>();
            }

            return new List<Widget> {
                new DeleteActionButton(
                    80,
                    onTap: () => {
                        ActionSheetUtils.showModalActionSheet(
                            new ActionSheet(
                                title: "确定不再收藏？",
                                items: new List<ActionSheetItem> {
                                    new ActionSheetItem(
                                        "确定",
                                        type: ActionType.normal,
                                        () => {
                                            var currentFavorite = article.favorites.Find(
                                                favorite => favorite.tagId == this.widget.viewModel.tagId);
                                            this.widget.actionModel.unFavoriteArticle(arg1: article.id,
                                                arg2: currentFavorite.id);
                                        }
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