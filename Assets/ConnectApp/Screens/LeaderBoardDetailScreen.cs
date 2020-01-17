using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
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
    public enum LeaderBoardType {
        collection,
        column
    }

    public class LeaderBoardDetailScreenConnector : StatelessWidget {
        public LeaderBoardDetailScreenConnector(
            string tagId,
            LeaderBoardType type = LeaderBoardType.collection,
            Key key = null
        ) : base(key: key) {
            this.tagId = tagId;
            this.type = type;
        }

        readonly string tagId;
        readonly LeaderBoardType type;


        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, LeaderBoardDetailScreenViewModel>(
                converter: state => {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var dict = this.type == LeaderBoardType.collection
                        ? state.leaderBoardState.collectionDict
                        : state.leaderBoardState.columnDict;
                    var articleList = dict.ContainsKey(this.tagId)
                        ? dict[this.tagId]
                        : new List<string>();
                    var collectedMap = state.loginState.isLoggedIn
                        ? state.favoriteState.collectedTagMap.ContainsKey(state.loginState.loginInfo.userId)
                            ? state.favoriteState.collectedTagMap[state.loginState.loginInfo.userId]
                            : new Dictionary<string, bool>()
                        : new Dictionary<string, bool>();
                    var followedMap = state.loginState.isLoggedIn
                        ? state.followState.followDict.ContainsKey(state.loginState.loginInfo.userId)
                            ? state.followState.followDict[state.loginState.loginInfo.userId]
                            : new Dictionary<string, bool>()
                        : new Dictionary<string, bool>();
                    var rankData =
                        state.leaderBoardState.rankDict.isNotEmpty() &&
                        state.leaderBoardState.rankDict.ContainsKey(this.tagId)
                            ? state.leaderBoardState.rankDict[this.tagId]
                            : null;
                    var isCollected = false;
                    var isFollowed = false;
                    var isHost = false;
                    if (rankData != null) {
                        isCollected = state.loginState.isLoggedIn && collectedMap.ContainsKey(rankData.itemId) &&
                                      collectedMap[rankData.itemId];
                        isFollowed = state.loginState.isLoggedIn && followedMap.ContainsKey(rankData.itemId) &&
                                     followedMap[rankData.itemId];
                        isHost = state.loginState.isLoggedIn &&
                                 state.favoriteState.favoriteTagDict.isNotEmpty() &&
                                 state.favoriteState.favoriteTagDict.ContainsKey(rankData.itemId) &&
                                 state.favoriteState.favoriteTagDict[rankData.itemId].userId ==
                                 state.loginState.loginInfo.userId;
                    }

                    return new LeaderBoardDetailScreenViewModel {
                        rankData = rankData,
                        type = this.type,
                        tagId = this.tagId,
                        articleList = articleList,
                        articleDict = state.articleState.articleDict,
                        userDict = state.userState.userDict,
                        teamDict = state.teamState.teamDict,
                        userArticleDict = state.articleState.userArticleDict,
                        isCollected = isCollected,
                        isFollowed = isFollowed,
                        isHost = isHost,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn,
                        hasMore = state.leaderBoardState.detailHasMore,
                        loading = state.leaderBoardState.detailLoading,
                        collectLoading = state.leaderBoardState.detailCollectLoading,
                        favoriteTagDict = state.favoriteState.favoriteTagDict,
                        favoriteTagArticleDict = state.favoriteState.favoriteTagArticleDict
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardDetailScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToArticleDetail = id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }
                        ),
                        startFetchDetailList = () => dispatcher.dispatch(new StartFetchLeaderBoardDetailAction()),
                        fetchDetailList = page =>
                            dispatcher.dispatch<IPromise>(
                                Actions.fetchLeaderBoardDetail(this.tagId, page, this.type)),
                        pushToReport = (reportId, reportType) => dispatcher.dispatch(
                            new MainNavigatorPushToReportAction {
                                reportId = reportId,
                                reportType = reportType
                            }
                        ),
                        pushToBlock = articleId => {
                            dispatcher.dispatch(new BlockArticleAction {articleId = articleId});
                            dispatcher.dispatch(new DeleteArticleHistoryAction {articleId = articleId});
                        },
                        collectFavoriteTag =
                            (itemId, rankDataId) =>
                                dispatcher.dispatch<IPromise>(Actions.collectFavoriteTag(itemId, rankDataId)),
                        cancelCollectFavoriteTag = (myFavoriteId, itemId) =>
                            dispatcher.dispatch<IPromise>(Actions.cancelCollectFavoriteTag(myFavoriteId, itemId)),
                        startFollowUser = followUserId => dispatcher.dispatch(new StartFollowUserAction {
                            followUserId = followUserId
                        }),
                        startUnFollowUser = userId =>
                            dispatcher.dispatch(new StartUnFollowUserAction {unFollowUserId = userId}),
                        followUser = userId => dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(userId)),
                        unFollowUser = userId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(userId))
                    };
                    return new LeaderBoardDetailScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class LeaderBoardDetailScreen : StatefulWidget {
        public LeaderBoardDetailScreen(
            LeaderBoardDetailScreenViewModel viewModel = null,
            LeaderBoardDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly LeaderBoardDetailScreenViewModel viewModel;
        public readonly LeaderBoardDetailScreenActionModel actionModel;

        public override State createState() {
            return new _LeaderBoardDetailScreenState();
        }
    }

    class _LeaderBoardDetailScreenState : State<LeaderBoardDetailScreen>, TickerProvider, RouteAware {
        int _pageNumber;
        bool _isHaveTitle;
        RefreshController _refreshController;
        Animation<RelativeRect> _animation;
        AnimationController _controller;


        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._pageNumber = 1;
            this._refreshController = new RefreshController();
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
                this.widget.actionModel.startFetchDetailList();
                this.widget.actionModel.fetchDetailList(this._pageNumber);
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            this._controller.dispose();
            base.dispose();
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = 1;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.fetchDetailList(this._pageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: this._isHaveTitle ? CColors.White : CColors.Background,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Flexible(child: this._buildContent())
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            Widget titleWidget;
            if (this._isHaveTitle) {
                var name = "";
                if (this.widget.viewModel.type == LeaderBoardType.collection) {
                    if (this.widget.viewModel.favoriteTagDict.isNotEmpty() &&
                        this.widget.viewModel.rankData != null &&
                        this.widget.viewModel.favoriteTagDict.ContainsKey(this.widget.viewModel.rankData.itemId)) {
                        var favoriteTag = this.widget.viewModel.favoriteTagDict[this.widget.viewModel.rankData.itemId];
                        name = this.widget.viewModel.rankData.resetTitle.isNotEmpty()
                            ? this.widget.viewModel.rankData.resetTitle
                            : favoriteTag?.name;
                    }
                }
                else {
                    if (this.widget.viewModel.userDict.isNotEmpty() &&
                        this.widget.viewModel.rankData != null &&
                        this.widget.viewModel.userDict.ContainsKey(this.widget.viewModel.rankData.itemId)) {
                        var user = this.widget.viewModel.userDict[this.widget.viewModel.rankData.itemId];
                        name = this.widget.viewModel.rankData.resetTitle.isNotEmpty()
                            ? this.widget.viewModel.rankData.resetTitle
                            : $"{user.fullName}的专栏";
                    }
                }


                titleWidget = new Text(
                    name,
                    style: CTextStyle.PXLargeMedium,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    textAlign: TextAlign.center
                );
            }
            else {
                titleWidget = new Container();
            }

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
                if (this._fetchButtonState()) {
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

            var child = this.widget.viewModel.type == LeaderBoardType.column
                ? this._buildFollowButton()
                : this.widget.viewModel.isHost
                    ? (Widget) new Container()
                    : new CustomButton(
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
            Widget rightWidget = new Container();
            if (this._isHaveTitle) {
                rightWidget = new Padding(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: child);
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
                backgroundColor: this._isHaveTitle ? CColors.White : CColors.Background,
                bottomSeparatorColor: this._isHaveTitle ? CColors.Separator2 : CColors.Transparent
            );
        }

        Widget _buildContent() {
            var itemCount = this.widget.viewModel.loading ? 1 : this.widget.viewModel.articleList.Count + 1;
            Widget content = new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: this.widget.viewModel.hasMore,
                    onRefresh: this._onRefresh,
                    itemCount: itemCount,
                    itemBuilder: this._buildAlbumCard,
                    headerWidget: this._buildListHeader(),
                    footerWidget: this.widget.viewModel.loading || this.widget.viewModel.hasMore ||
                                  this.widget.viewModel.articleList.isEmpty()
                        ? null
                        : CustomListViewConstant.defaultFooterWidget
                )
            );

            return new NotificationListener<ScrollNotification>(child: content, onNotification: this._onNotification);
        }

        Widget _buildAlbumCard(BuildContext context, int index) {
            if (index == 0) {
                if (this.widget.viewModel.loading) {
                    return new Container(
                        height: MediaQuery.of(context).size.height - 202 - MediaQuery.of(context).padding.top,
                        child: new Center(
                            child: new GlobalLoading(color: CColors.White)
                        )
                    );
                }

                if (this.widget.viewModel.articleList.isEmpty()) {
                    return new Container(
                        height: MediaQuery.of(context).size.height - 202 - MediaQuery.of(context).padding.top,
                        child: new BlankView(
                            "暂无文章",
                            "image/default-article",
                            true,
                            () => {
                                this.widget.actionModel.startFetchDetailList();
                                this.widget.actionModel.fetchDetailList(this._pageNumber);
                            },
                            new BoxDecoration(
                                color: CColors.White,
                                borderRadius: BorderRadius.only(12, 12)
                            ))
                    );
                }

                return new Container(
                    height: 16,
                    decoration: new BoxDecoration(color: CColors.White, borderRadius: BorderRadius.only(12, 12))
                );
            }

            var article = this.widget.viewModel.articleDict[this.widget.viewModel.articleList[index - 1]];

            if (!this.widget.viewModel.articleDict.ContainsKey(key: article.id)) {
                return new Container();
            }

            var fullName = "";
            var userId = "";
            if (article.ownerType == OwnerType.user.ToString()) {
                userId = article.userId;
                if (this.widget.viewModel.userDict.ContainsKey(key: article.userId)) {
                    fullName = this.widget.viewModel.userDict[key: article.userId].fullName
                               ?? this.widget.viewModel.userDict[key: article.userId].name;
                }
            }

            if (article.ownerType == OwnerType.team.ToString()) {
                userId = article.teamId;
                if (this.widget.viewModel.teamDict.ContainsKey(key: article.teamId)) {
                    fullName = this.widget.viewModel.teamDict[key: article.teamId].name;
                }
            }

            var linkUrl = CStringUtils.JointProjectShareLink(projectId: article.id);
            return new ArticleCard(
                article: article,
                () => {
                    this.widget.actionModel.pushToArticleDetail(obj: article.id);
                    AnalyticsManager.ClickEnterArticleDetail("Home_Article", articleId: article.id,
                        articleTitle: article.title);
                },
                () => ShareManager.showArticleShareView(
                    false,
                    isLoggedIn: this.widget.viewModel.isLoggedIn,
                    () => {
                        Clipboard.setData(new ClipboardData(text: linkUrl));
                        CustomDialogUtils.showToast("复制链接成功", iconData: Icons.check_circle_outline);
                    },
                    () => this.widget.actionModel.pushToLogin(),
                    () => this.widget.actionModel.pushToBlock(obj: article.id),
                    () => this.widget.actionModel.pushToReport(arg1: article.id,
                        arg2: ReportType.article),
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
            );
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (pixels > 104) {
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

        Widget _buildListHeader() {
            var title = "";
            var subTitle = "";
            var images = new List<string>();
            if (this.widget.viewModel.type == LeaderBoardType.collection) {
                if (this.widget.viewModel.favoriteTagDict.isNotEmpty() &&
                    this.widget.viewModel.favoriteTagArticleDict.isNotEmpty() &&
                    this.widget.viewModel.rankData != null &&
                    this.widget.viewModel.favoriteTagDict.ContainsKey(this.widget.viewModel.rankData.itemId) &&
                    this.widget.viewModel.favoriteTagArticleDict.ContainsKey(this.widget.viewModel.rankData.itemId)
                ) {
                    var favoriteTag = this.widget.viewModel.favoriteTagDict[this.widget.viewModel.rankData.itemId];
                    var favoriteTagArticle =
                        this.widget.viewModel.favoriteTagArticleDict[this.widget.viewModel.rankData.itemId];
                    favoriteTagArticle.list.ForEach(tagArticle => { images.Add(item: tagArticle.thumbnail.url); });
                    title = this.widget.viewModel.rankData.resetTitle.isNotEmpty()
                        ? this.widget.viewModel.rankData.resetTitle
                        : favoriteTag.name;
                    subTitle = $"作者 {favoriteTagArticle.authorCount} • 文章 {favoriteTag?.stasitics.count}";
                }
            }
            else {
                if (this.widget.viewModel.userDict.isNotEmpty() &&
                    this.widget.viewModel.userArticleDict.isNotEmpty() &&
                    this.widget.viewModel.rankData != null &&
                    this.widget.viewModel.userDict.ContainsKey(this.widget.viewModel.rankData.itemId) &&
                    this.widget.viewModel.userArticleDict.ContainsKey(this.widget.viewModel.rankData.itemId)) {
                    var user = this.widget.viewModel.userDict[this.widget.viewModel.rankData.itemId];
                    var userArticle = this.widget.viewModel.userArticleDict[this.widget.viewModel.rankData.itemId];
                    userArticle.list.ForEach(
                        userArticleItem => { images.Add(item: userArticleItem.thumbnail.url); });
                    title = this.widget.viewModel.rankData.resetTitle.isNotEmpty()
                        ? this.widget.viewModel.rankData.resetTitle
                        : $"{user.fullName}的专栏";
                    subTitle = $"{user.fullName} • 文章 {userArticle.total}";
                }
            }


            return new LeaderBoardDetailHeader(
                title
                , subTitle, images: images,
                type: this.widget.viewModel.type,
                isLoading: this.widget.viewModel.collectLoading,
                isHost: this.widget.viewModel.isHost,
                isCollected: this._fetchButtonState(),
                ClickButtonCallback: this._onPressed,
                followButton: this._buildFollowButton());
        }

        void _onFollow(UserType userType, string userId) {
            if (this.widget.viewModel.isLoggedIn) {
                if (userType == UserType.follow) {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不再关注？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", type: ActionType.normal, () => {
                                    this.widget.actionModel.startUnFollowUser(obj: userId);
                                    this.widget.actionModel.unFollowUser(arg: userId);
                                }),
                                new ActionSheetItem("取消", type: ActionType.cancel)
                            }
                        )
                    );
                }

                if (userType == UserType.unFollow) {
                    this.widget.actionModel.startFollowUser(obj: userId);
                    this.widget.actionModel.followUser(arg: userId);
                }
            }
            else {
                this.widget.actionModel.pushToLogin();
            }
        }


        Widget _buildFollowButton() {
            if (this.widget.viewModel.type == LeaderBoardType.collection || this.widget.viewModel.userDict.isEmpty() ||
                this.widget.viewModel.rankData == null ||
                !this.widget.viewModel.userDict.ContainsKey(this.widget.viewModel.rankData.itemId)) {
                return new Container();
            }

            var user = this.widget.viewModel.userDict[this.widget.viewModel.rankData.itemId];
            UserType userType = UserType.unFollow;
            if (!this.widget.viewModel.isLoggedIn) {
                userType = UserType.unFollow;
            }
            else {
                if (UserInfoManager.getUserInfo().userId == user.id) {
                    userType = UserType.me;
                }
                else if (user.followUserLoading ?? false) {
                    userType = UserType.loading;
                }
                else if (this._fetchButtonState()) {
                    userType = UserType.follow;
                }
            }

            return new FollowButton(userType, () => this._onFollow(userType, user.id));
        }

        bool _fetchButtonState() {
            if (this.widget.viewModel.type == LeaderBoardType.collection) {
                return this.widget.viewModel.isCollected;
            }

            return this.widget.viewModel.isFollowed;
        }

        void _onPressed() {
            if (!UserInfoManager.isLogin()) {
                this.widget.actionModel.pushToLogin();
                return;
            }

            if (this.widget.viewModel.type == LeaderBoardType.collection) {
                if (this.widget.viewModel.isCollected) {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定取消收藏？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", type: ActionType.normal,
                                    () => {
                                        this.widget.actionModel.cancelCollectFavoriteTag(this.widget.viewModel.rankData
                                            .myFavoriteTagId, this.widget.viewModel.rankData
                                            .itemId);
                                    }),
                                new ActionSheetItem("取消", type: ActionType.cancel)
                            }
                        )
                    );
                }
                else {
                    this.widget.actionModel.collectFavoriteTag(this.widget.viewModel.rankData.itemId,
                        this.widget.viewModel.rankData.id);
                }
            }
        }


        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        public void didPopNext() {
            StatusBarManager.statusBarStyle(true);
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}