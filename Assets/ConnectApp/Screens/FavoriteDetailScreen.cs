using System;
using System.Collections.Generic;
using System.Linq;
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
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public enum FavoriteType {
        my,
        follow,
        userDetail
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
                    var collectedMap = state.loginState.isLoggedIn
                        ? state.favoriteState.collectedTagMap.ContainsKey(state.loginState.loginInfo.userId)
                            ? state.favoriteState.collectedTagMap[state.loginState.loginInfo.userId]
                            : new Dictionary<string, bool>()
                        : new Dictionary<string, bool>();
                    var isCollected = false;
                    if (favoriteTag != null) {
                        isCollected = state.loginState.isLoggedIn &&
                                      collectedMap.ContainsKey(favoriteTag.quoteTagId.isEmpty()
                                          ? favoriteTag.id
                                          : favoriteTag.quoteTagId);
                    }

                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var myFavoriteIds = state.favoriteState.favoriteTagIdDict.ContainsKey(key: currentUserId)
                        ? state.favoriteState.favoriteTagIdDict[key: currentUserId]
                        : new List<string>();
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
                        currentUserId = state.loginState.loginInfo.userId ?? "",
                        isCollect = isCollected,
                        collectLoading = state.leaderBoardState.detailCollectLoading,
                        collectChangeMap = state.favoriteState.collectedTagChangeMap,
                        myFavoriteIds = myFavoriteIds
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
                                imageUrl: imageUrl)),
                        collectFavoriteTag =
                            (itemId, tagId) =>
                                dispatcher.dispatch<IPromise>(Actions.collectFavoriteTag(itemId, tagId: tagId)),
                        cancelCollectFavoriteTag = (tagId, itemId) =>
                            dispatcher.dispatch<IPromise>(Actions.cancelCollectFavoriteTag(tagId, itemId))
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
            var favoriteTag = this.widget.viewModel.favoriteTag;
            string title;
            var rightWidget = (Widget) new Container(width: 56);
            if (favoriteTag == null) {
                title = "";
            }
            else {
                if (favoriteTag.type == "default") {
                    title = "默认";
                }
                else {
                    title = favoriteTag.name;
                }
            }

            Widget titleWidget = new Container();

            Widget buttonChild;


            Color buttonColor = CColors.PrimaryBlue;

            if (this.widget.viewModel.collectLoading) {
                buttonColor = CColors.Disable2;
                buttonChild = new CustomActivityIndicator(
                    size: LoadingSize.xSmall
                );
            }
            else {
                string buttonText = "收藏";
                Color textColor = CColors.PrimaryBlue;
                if (this.widget.viewModel.isCollect) {
                    buttonText = $"已收藏";
                    buttonColor = CColors.Disable2;
                    textColor = new Color(0xFF959595);
                }

                buttonChild = new Text(
                    data: buttonText,
                    style: new TextStyle(
                        fontSize: 14,
                        fontFamily: "Roboto-Medium",
                        color: textColor
                    )
                );
            }

            var child = new CustomButton(
                onPressed: this._onPressed,
                padding: EdgeInsets.zero,
                child: new Container(
                    width: 60,
                    height: 28,
                    alignment: Alignment.center,
                    decoration: new BoxDecoration(
                        color: CColors.White,
                        borderRadius: BorderRadius.circular(14),
                        border: Border.all(color: buttonColor)
                    ),
                    child: buttonChild
                )
            );


            if (this._isHaveTitle) {
                titleWidget = new Text(
                    data: title,
                    style: CTextStyle.PXLargeMedium,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    textAlign: TextAlign.center
                );
                rightWidget = new Padding(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: child);
            }


            if (this.widget.viewModel.favoriteTag.type != "default" && this.widget.viewModel.isLoggedIn &&
                this.widget.viewModel.currentUserId.Equals(this.widget.viewModel.userId) &&
                this.widget.viewModel.type == FavoriteType.my) {
                rightWidget = new CustomButton(
                    padding: EdgeInsets.symmetric(8, 16),
                    onPressed: () => this.widget.actionModel.pushToEditFavorite(this.widget.viewModel.favoriteTag.id),
                    child: new Text(
                        "编辑",
                        style: CTextStyle.PLargeMediumBlue.merge(new TextStyle(height: 1))
                    )
                );
            }

            if (favoriteTag.type == "default" || (this.widget.viewModel.type == FavoriteType.userDetail &&
                                                  UserInfoManager.isLogin() &&
                                                  this.widget.viewModel.myFavoriteIds.Contains(this.widget.viewModel
                                                      .favoriteTag.quoteTagId))) {
                rightWidget = new Padding(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: new Container());
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
                rightWidget: rightWidget,
                backgroundColor: this._isHaveTitle || this.widget.viewModel.type == FavoriteType.my
                    ? CColors.White
                    : CColors.Background,
                bottomSeparatorColor: this._isHaveTitle || this.widget.viewModel.type == FavoriteType.my
                    ? CColors.Separator2
                    : CColors.Transparent
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
            var title = "";
            title = "默认";
            var subTitle = "0个内容";
            var images = new List<string>();
            Widget iconWidget = new Container();
            var isHost = false;
            if (this.widget.viewModel.favoriteTag != null) {
                title = this.widget.viewModel.favoriteTag.name;
                subTitle =
                    $"{this.widget.viewModel.favoriteTag.stasitics.count}个内容";
                string iconName;
                Color iconColor;
                if (this.widget.viewModel.favoriteTag.type == "default") {
                    title = "默认";
                    iconName = $"{CImageUtils.FavoriteCoverImagePath}/{CImageUtils.FavoriteCoverImages[0]}";
                    iconColor = CColorUtils.FavoriteCoverColors[0];
                }
                else {
                    title = this.widget.viewModel.favoriteTag.name;
                    iconName =
                        $"{CImageUtils.FavoriteCoverImagePath}/{this.widget.viewModel.favoriteTag.iconStyle.name}";
                    iconColor = new Color(long.Parse(s: this.widget.viewModel.favoriteTag.iconStyle.bgColor));
                }

                iconWidget = new FavoriteTagCoverImage(
                    coverImage: iconName,
                    coverColor: iconColor,
                    size: 84
                );
                isHost = this.widget.viewModel.type == FavoriteType.my ||
                         this.widget.viewModel.favoriteTag.type == "default" ||
                         (this.widget.viewModel.type != FavoriteType.my && UserInfoManager.isLogin() &&
                          this.widget.viewModel.myFavoriteIds.Contains(this.widget.viewModel.favoriteTag.quoteTagId));
            }

            return new LeaderBoardDetailHeader(
                title
                , subTitle,
                images: images.Count > 3 ? images.Take(3).ToList() : images,
                isCollected: this.widget.viewModel.isCollect,
                isLoading: this.widget.viewModel.collectLoading,
                isHost: isHost,
                ClickButtonCallback: this._onPressed,
                leftWidget: iconWidget,
                leftWidgetTopPadding: 4,
                key: this._favoriteInfoKey
            );
        }

        void _onPressed() {
            if (!UserInfoManager.isLogin()) {
                this.widget.actionModel.pushToLogin();
                return;
            }

            if (this.widget.viewModel.isCollect) {
                ActionSheetUtils.showModalActionSheet(
                    new ActionSheet(
                        title: "确定取消收藏？",
                        items: new List<ActionSheetItem> {
                            new ActionSheetItem("确定", type: ActionType.normal,
                                () => {
                                    var tagId = this.widget.viewModel.favoriteTag.id;
                                    if (this.widget.viewModel.collectChangeMap.isNotEmpty() &&
                                        this.widget.viewModel.collectChangeMap.ContainsKey(tagId)) {
                                        tagId = this.widget.viewModel.collectChangeMap[tagId];
                                    }

                                    this.widget.actionModel.cancelCollectFavoriteTag(
                                        tagId,
                                        this.widget.viewModel.favoriteTag.quoteTagId ?? "");
                                }),
                            new ActionSheetItem("取消", type: ActionType.cancel)
                        }
                    )
                );
            }
            else {
                this.widget.actionModel.collectFavoriteTag(
                    this.widget.viewModel.favoriteTag.quoteTagId.isEmpty()
                        ? this.widget.viewModel.favoriteTag.id
                        : this.widget.viewModel.favoriteTag.quoteTagId, this.widget.viewModel.favoriteTag.id);
            }
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
                secondaryActions: this.widget.viewModel.type == FavoriteType.my
                    ? this._buildSecondaryActions(article: article)
                    : null,
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