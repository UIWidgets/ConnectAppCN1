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
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class UserDetailScreenConnector : StatelessWidget {
        public UserDetailScreenConnector(
            string userId,
            bool isSlug = false,
            Key key = null
        ) : base(key: key) {
            this.userId = userId;
            this.isSlug = isSlug;
        }

        readonly string userId;
        readonly bool isSlug;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, UserDetailScreenViewModel>(
                converter: state => {
                    var user = this.fetchUser(this.userId, state.userState.userDict, state.userState.slugDict);
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new UserDetailScreenViewModel {
                        userLoading = state.userState.userLoading,
                        userArticleLoading = state.userState.userArticleLoading,
                        userFavoriteLoading = state.favoriteState.favoriteTagLoading,
                        userFollowFavoriteLoading = state.favoriteState.followFavoriteTagLoading,
                        favoriteTagIdDict = state.favoriteState.favoriteTagIdDict,
                        followFavoriteTagIdDict = state.favoriteState.followFavoriteTagIdDict,
                        userFavoriteHasMore = state.favoriteState.favoriteTagHasMore,
                        userFollowFavoriteHasMore = state.favoriteState.followFavoriteTagHasMore,
                        user = user,
                        userId = this.userId,
                        userLicenseDict = state.userState.userLicenseDict,
                        articleDict = state.articleState.articleDict,
                        favoriteTagDict = state.favoriteState.favoriteTagDict,
                        followMap = followMap,
                        userDict = state.userState.userDict,
                        teamDict = state.teamState.teamDict,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn,
                        isBlockUser = HistoryManager.isBlockUser(userId: this.userId),
                        isMe = UserInfoManager.isMe(userId: this.userId)
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new UserDetailScreenActionModel {
                        startFetchUserProfile = () => dispatcher.dispatch(new StartFetchUserProfileAction()),
                        fetchUserProfile = () =>
                            dispatcher.dispatch<IPromise>(Actions.fetchUserProfile(userId: this.userId)),
                        startFetchUserArticle = () => dispatcher.dispatch(new StartFetchUserArticleAction()),
                        fetchUserArticle = (userId, pageNumber) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchUserArticle(userId: userId,
                                pageNumber: pageNumber)),
                        startFetchUserFavorite = () => dispatcher.dispatch(new StartFetchFavoriteTagAction()),
                        fetchUserFavorite = (userId, offset) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFavoriteTags(userId: userId, offset: offset)),
                        startFetchUserFollowFavorite =
                            () => dispatcher.dispatch(new StartFetchFollowFavoriteTagAction()),
                        fetchUserFollowFavorite = (userId, offset) =>
                            dispatcher.dispatch<IPromise>(
                                Actions.fetchFollowFavoriteTags(userId: userId, offset: offset)),
                        startFollowUser = userId =>
                            dispatcher.dispatch(new StartFollowUserAction {followUserId = userId}),
                        followUser = userId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId: userId)),
                        startUnFollowUser = userId => dispatcher.dispatch(new StartUnFollowUserAction
                            {unFollowUserId = userId}),
                        unFollowUser = userId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(unFollowUserId: userId)),
                        deleteFavoriteTag = tagId =>
                            dispatcher.dispatch<IPromise>(Actions.deleteFavoriteTag(tagId: tagId)),
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToArticleDetail = id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }
                        ),
                        pushToFavoriteDetail = (userId, tagId) => dispatcher.dispatch(
                            new MainNavigatorPushToFavoriteDetailAction {
                                userId = userId,
                                tagId = tagId,
                                type = FavoriteType.userDetail
                            }
                        ),
                        pushToCreateFavorite = tagId => dispatcher.dispatch(
                            new MainNavigatorPushToEditFavoriteAction {
                                tagId = tagId
                            }
                        ),
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
                        pushToUserFollowing = (userId, initialPage) => dispatcher.dispatch(
                            new MainNavigatorPushToUserFollowingAction {
                                userId = userId,
                                initialPage = initialPage
                            }
                        ),
                        pushToUserFollower = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserFollowerAction {
                                userId = userId
                            }
                        ),
                        pushToEditPersonalInfo = userId => dispatcher.dispatch(
                            new MainNavigatorPushToEditPersonalInfoAction {
                                userId = userId
                            }
                        ),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl)),
                        blockUserAction = remove => {
                            dispatcher.dispatch(new BlockUserAction {blockUserId = this.userId, remove = remove});
                        }
                    };
                    return new UserDetailScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }

        User fetchUser(string userId, Dictionary<string, User> userDict, Dictionary<string, string> slugDict) {
            if (userDict.ContainsKey(userId)) {
                return userDict[userId];
            }

            if (this.isSlug && slugDict.ContainsKey(userId)) {
                return userDict[slugDict[userId]];
            }

            return null;
        }
    }

    public class UserDetailScreen : StatefulWidget {
        public UserDetailScreen(
            UserDetailScreenViewModel viewModel = null,
            UserDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly UserDetailScreenViewModel viewModel;
        public readonly UserDetailScreenActionModel actionModel;

        public override State createState() {
            return new _UserDetailScreenState();
        }
    }

    class _UserDetailScreenState : State<UserDetailScreen>, TickerProvider, RouteAware {
        const float imageBaseHeight = 212;
        const float navBarHeight = 44;
        const float _transformSpeed = 0.005f;
        int _articlePageNumber;
        RefreshController _refreshController;
        float _factor = 1;
        bool _isHaveTitle;
        bool _hideNavBar;
        bool _isShowTop;
        int _selectedIndex;
        Animation<RelativeRect> _animation;
        AnimationController _controller;
        readonly CustomDismissibleController _dismissibleController = new CustomDismissibleController();
        CustomTabController _tabController;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(true);
            this._articlePageNumber = 1;
            this._refreshController = new RefreshController();
            this._isHaveTitle = false;
            this._hideNavBar = true;
            this._isShowTop = false;
            this._selectedIndex = 0;
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            RelativeRectTween rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, top: navBarHeight, 0, 0),
                RelativeRect.fromLTRB(0, 0, 0, 0)
            );
            this._animation = rectTween.animate(parent: this._controller);
            this._tabController = new CustomTabController(3, this);
            this._tabController.addListener(() => {
                if (this._selectedIndex != this._tabController.index) {
                    this.setState(() => this._selectedIndex = this._tabController.index);
                }
            });
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchUserProfile();
                this.widget.actionModel.fetchUserProfile();
                this.widget.actionModel.startFetchUserArticle();
                this.widget.actionModel.startFetchUserFavorite();
                this.widget.actionModel.startFetchUserFollowFavorite();
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            this._controller.dispose();
            this._tabController.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        void _scrollListener() {
            var scrollController = this._refreshController.scrollController;
            if (scrollController.offset < 0) {
                this._factor = 1 + scrollController.offset.abs() * _transformSpeed;
                this.setState(() => { });
            }
            else {
                if (this._factor != 1) {
                    this.setState(() => this._factor = 1);
                }
            }
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            var navBarBottomPosition = navBarHeight + CCommonUtils.getSafeAreaTopPadding(context: this.context);

            if (pixels >= navBarBottomPosition) {
                if (this._hideNavBar) {
                    this.setState(() => this._hideNavBar = false);
                    StatusBarManager.statusBarStyle(false);
                }
            }
            else {
                if (!this._hideNavBar) {
                    this.setState(() => this._hideNavBar = true);
                    StatusBarManager.statusBarStyle(true);
                }
            }

            if (pixels > imageBaseHeight - navBarHeight - 24) {
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

            if (pixels > imageBaseHeight - navBarHeight) {
                if (!this._isShowTop) {
                    this.setState(() => this._isShowTop = true);
                }
            }
            else {
                if (this._isShowTop) {
                    this.setState(() => this._isShowTop = false);
                }
            }

            return true;
        }

        void _onRefresh(bool up) {
            var userId = this.widget.viewModel.user.id;
            if (this._selectedIndex == 1) {
                int offset;
                if (up) {
                    offset = 0;
                }
                else {
                    var favoriteTagIds = this.widget.viewModel.favoriteTagIdDict.ContainsKey(key: userId)
                        ? this.widget.viewModel.favoriteTagIdDict[key: userId]
                        : new List<string>();
                    offset = favoriteTagIds.Count;
                }

                this.widget.actionModel.fetchUserFavorite(arg1: userId, arg2: offset)
                    .Then(() => this._refreshController.sendBack(up: up,
                        up ? RefreshStatus.completed : RefreshStatus.idle))
                    .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
                return;
            }

            if (this._selectedIndex == 2) {
                int offset;
                if (up) {
                    offset = 0;
                }
                else {
                    var favoriteTagIds = this.widget.viewModel.followFavoriteTagIdDict.ContainsKey(key: userId)
                        ? this.widget.viewModel.followFavoriteTagIdDict[key: userId]
                        : new List<string>();
                    offset = favoriteTagIds.Count;
                }

                this.widget.actionModel.fetchUserFollowFavorite(arg1: userId, arg2: offset)
                    .Then(() => this._refreshController.sendBack(up: up,
                        up ? RefreshStatus.completed : RefreshStatus.idle))
                    .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
                return;
            }

            if (up) {
                this._articlePageNumber = 1;
            }
            else {
                this._articlePageNumber++;
            }

            this.widget.actionModel.fetchUserArticle(arg1: this.widget.viewModel.user.id, arg2: this._articlePageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            Widget content;
            if (this.widget.viewModel.userLoading && this.widget.viewModel.user == null) {
                content = new GlobalLoading();
            }
            else if (this.widget.viewModel.user == null || this.widget.viewModel.user.errorCode == "ResourceNotFound") {
                content = new BlankView("用户不存在");
            }
            else {
                content = this._buildUserContent(context: context);
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: false,
                    bottom: false,
                    child: new Stack(
                        children: new List<Widget> {
                            content,
                            this._buildNavigationBar(context: context),
                            new Positioned(
                                left: 0,
                                top: navBarHeight + CCommonUtils.getSafeAreaTopPadding(context: context),
                                right: 0,
                                child: new Offstage(
                                    offstage: !this._isShowTop,
                                    child: this._buildUserArticleTitle()
                                )
                            )
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar(BuildContext context) {
            Widget titleWidget;
            if (this._isHaveTitle) {
                var user = this.widget.viewModel.user ?? new User();
                titleWidget = new Row(
                    children: new List<Widget> {
                        new Expanded(
                            child: new Text(
                                user.fullName ?? user.name,
                                style: CTextStyle.PXLargeMedium,
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis
                            )
                        ),
                        new SizedBox(width: 8),
                        this.widget.viewModel.isBlockUser ? new Container() : this._buildFollowButton(true),
                        new SizedBox(width: 8)
                    }
                );
            }
            else {
                titleWidget = new Container();
            }

            var hasUser = !(this.widget.viewModel.user == null ||
                            this.widget.viewModel.user.errorCode == "ResourceNotFound");
            return new Positioned(
                left: 0,
                top: 0,
                right: 0,
                height: navBarHeight + CCommonUtils.getSafeAreaTopPadding(context: context),
                child: new Container(
                    padding: EdgeInsets.only(top: CCommonUtils.getSafeAreaTopPadding(context: context)),
                    decoration: new BoxDecoration(
                        this._hideNavBar ? CColors.Transparent : CColors.White,
                        border: new Border(
                            bottom: new BorderSide(this._isHaveTitle ? CColors.Separator2 : CColors.Transparent))
                    ),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: new List<Widget> {
                            new CustomButton(
                                padding: EdgeInsets.only(16, 8, 8, 8),
                                onPressed: () => this.widget.actionModel.mainRouterPop(),
                                child: new Icon(
                                    icon: Icons.arrow_back,
                                    size: 24,
                                    color: hasUser
                                        ? this._hideNavBar ? CColors.White : CColors.Icon
                                        : CColors.Icon
                                )
                            ),
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
                            this._buildMoreButton(hasUser: hasUser)
                        }
                    )
                )
            );
        }

        Widget _buildUserContent(BuildContext context) {
            var articleIds = this.widget.viewModel.user.articleIds;
            var favoriteIds = this.widget.viewModel.favoriteTagIdDict.ContainsKey(key: this.widget.viewModel.user.id)
                ? this.widget.viewModel.favoriteTagIdDict[key: this.widget.viewModel.user.id]
                : null;
            var followFavoriteIds =
                this.widget.viewModel.followFavoriteTagIdDict.ContainsKey(key: this.widget.viewModel.user.id)
                    ? this.widget.viewModel.followFavoriteTagIdDict[key: this.widget.viewModel.user.id]
                    : null;
            var articlesHasMore = this.widget.viewModel.user.articlesHasMore ?? false;
            var userFavoriteHasMore = this.widget.viewModel.userFavoriteHasMore;
            var userFollowFavoriteHasMore = this.widget.viewModel.userFollowFavoriteHasMore;
            var userArticleLoading = this.widget.viewModel.userArticleLoading && articleIds == null;
            var userFavoriteLoading = this.widget.viewModel.userFavoriteLoading && favoriteIds == null;
            var userFollowFavoriteLoading =
                this.widget.viewModel.userFollowFavoriteLoading && followFavoriteIds == null;
            int itemCount;
            if (userArticleLoading && this._selectedIndex == 0) {
                itemCount = 3;
            }
            else if (userFavoriteLoading && this._selectedIndex == 1) {
                itemCount = 3;
            }
            else if (userFollowFavoriteLoading && this._selectedIndex == 2) {
                itemCount = 3;
            }
            else {
                if (articleIds == null && this._selectedIndex == 0) {
                    itemCount = 3;
                }
                else if (favoriteIds == null && this._selectedIndex == 1) {
                    itemCount = 3;
                }
                else if (followFavoriteIds == null && this._selectedIndex == 2) {
                    itemCount = 3;
                }
                else {
                    if (this._selectedIndex == 0) {
                        var articleCount = articlesHasMore ? articleIds.Count : articleIds.Count + 1;
                        itemCount = 2 + (articleIds.Count == 0 ? 1 : articleCount);
                    }
                    else if (this._selectedIndex == 1) {
                        var favoriteCount = userFavoriteHasMore ? favoriteIds.Count : favoriteIds.Count + 1;
                        itemCount = 2 + (favoriteIds.Count == 0 ? 1 : favoriteCount);
                    }
                    else {
                        var favoriteCount = userFollowFavoriteHasMore
                            ? followFavoriteIds.Count
                            : followFavoriteIds.Count + 1;
                        itemCount = 2 + (followFavoriteIds.Count == 0 ? 1 : favoriteCount);
                    }
                }
            }

            bool enablePullUp;
            if (this._selectedIndex == 0) {
                enablePullUp = articlesHasMore;
            }
            else if (this._selectedIndex == 1) {
                enablePullUp = userFavoriteHasMore;
            }
            else {
                enablePullUp = userFollowFavoriteHasMore;
            }

            var headerHeight = imageBaseHeight + 44 + CCommonUtils.getSafeAreaTopPadding(context: context);

            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: enablePullUp,
                        onRefresh: this._onRefresh,
                        onNotification: this._onNotification,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: itemCount,
                            itemBuilder: (cxt, index) => {
                                if (index == 0) {
                                    return Transform.scale(
                                        scale: this._factor,
                                        child: this._buildUserInfo(context)
                                    );
                                }

                                if (index == 1) {
                                    return this._buildUserArticleTitle();
                                }

                                if (this.widget.viewModel.isBlockUser && index == 2 && this._selectedIndex == 0) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "该用户已被您屏蔽",
                                            "image/default-blocked"
                                        )
                                    );
                                }
                                
                                if (this.widget.viewModel.isBlockUser && index == 2 && this._selectedIndex == 1) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "该用户已被您屏蔽",
                                            "image/default-blocked"
                                        )
                                    );
                                }
                                
                                if (this.widget.viewModel.isBlockUser && index == 2 && this._selectedIndex == 2) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "该用户已被您屏蔽",
                                            "image/default-blocked"
                                        )
                                    );
                                }
                                
                                if (userArticleLoading && index == 2 && this._selectedIndex == 0) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new GlobalLoading()
                                    );
                                }

                                if (userFavoriteLoading && index == 2 && this._selectedIndex == 1) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new GlobalLoading()
                                    );
                                }

                                if (userFollowFavoriteLoading && index == 2 && this._selectedIndex == 2) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new GlobalLoading()
                                    );
                                }

                                if (articleIds.isNullOrEmpty() && index == 2 && this._selectedIndex == 0) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "哎呀，暂无已发布的文章",
                                            "image/default-article"
                                        )
                                    );
                                }

                                if (favoriteIds.isNullOrEmpty() && index == 2 && this._selectedIndex == 1) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "暂无我的收藏列表",
                                            "image/default-following"
                                        )
                                    );
                                }

                                if (followFavoriteIds.isNullOrEmpty() && index == 2 && this._selectedIndex == 2) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "暂无我关注的收藏",
                                            "image/default-following"
                                        )
                                    );
                                }

                                if (index == itemCount - 1 && !articlesHasMore && this._selectedIndex == 0) {
                                    return new EndView();
                                }

                                if (index == itemCount - 1 && !userFavoriteHasMore && this._selectedIndex == 1) {
                                    return new EndView();
                                }

                                if (index == itemCount - 1 && !userFollowFavoriteHasMore && this._selectedIndex == 2) {
                                    return new EndView();
                                }

                                if (this._selectedIndex == 1) {
                                    var favoriteId = favoriteIds[index - 2];
                                    return this._buildFavoriteCard(favoriteId: favoriteId);
                                }

                                if (this._selectedIndex == 2) {
                                    var favoriteId = followFavoriteIds[index - 2];
                                    return this._buildFavoriteCard(favoriteId: favoriteId);
                                }

                                var articleId = articleIds[index - 2];
                                if (!this.widget.viewModel.articleDict.ContainsKey(key: articleId)) {
                                    return new Container();
                                }

                                var article = this.widget.viewModel.articleDict[key: articleId];
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

                                return new ArticleCard(
                                    article: article,
                                    () => this.widget.actionModel.pushToArticleDetail(obj: article.id),
                                    () => ShareManager.showArticleShareView(
                                        this.widget.viewModel.currentUserId != article.userId,
                                        isLoggedIn: this.widget.viewModel.isLoggedIn,
                                        () => {
                                            Clipboard.setData(new ClipboardData(text: linkUrl));
                                            CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                                        },
                                        () => this.widget.actionModel.pushToLogin(),
                                        () => this.widget.actionModel.pushToBlock(article.id),
                                        () => this.widget.actionModel.pushToReport(article.id, ReportType.article),
                                        type => {
                                            CustomDialogUtils.showCustomDialog(
                                                child: new CustomLoadingDialog()
                                            );
                                            string imageUrl = CImageUtils.SizeTo200ImageUrl(article.thumbnail.url);
                                            this.widget.actionModel.shareToWechat(arg1: type, arg2: article.title,
                                                    arg3: article.subTitle, arg4: linkUrl, arg5: imageUrl)
                                                .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                                                .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                                        },
                                        () => this.widget.actionModel.mainRouterPop()
                                    ),
                                    fullName,
                                    key: new ObjectKey(value: article.id)
                                );
                            }
                        )
                    )
                )
            );
        }

        Widget _buildUserInfo(BuildContext context) {
            var user = this.widget.viewModel.user ?? new User();
            Widget titleWidget;
            if (user.title != null && user.title.isNotEmpty()) {
                titleWidget = new Text(
                    data: user.title,
                    style: new TextStyle(
                        fontSize: 14,
                        fontFamily: "Roboto-Regular",
                        color: CColors.BgGrey
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }
            else {
                titleWidget = new Container();
            }

            return new CoverImage(
                coverImage: user.coverImage,
                imageBaseHeight + CCommonUtils.getSafeAreaTopPadding(context: context),
                new Container(
                    padding: EdgeInsets.only(16, 0, 16, 24),
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget> {
                            new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 16),
                                        child: Avatar.User(
                                            user: user,
                                            80,
                                            true
                                        )
                                    ),
                                    new Expanded(
                                        child: new Column(
                                            mainAxisAlignment: MainAxisAlignment.center,
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                new Row(
                                                    crossAxisAlignment: CrossAxisAlignment.start,
                                                    children: new List<Widget> {
                                                        new Flexible(
                                                            child: new Text(
                                                                user.fullName ?? user.name,
                                                                style: CTextStyle.H4White.merge(
                                                                    new TextStyle(height: 1)),
                                                                maxLines: 1,
                                                                overflow: TextOverflow.ellipsis
                                                            )
                                                        ),
                                                        CImageUtils.GenBadgeImage(
                                                            badges: user.badges,
                                                            CCommonUtils.GetUserLicense(
                                                                userId: user.id,
                                                                userLicenseMap: this.widget.viewModel.userLicenseDict
                                                            ),
                                                            EdgeInsets.only(4, 6)
                                                        )
                                                    }
                                                ),
                                                titleWidget
                                            }
                                        )
                                    )
                                }
                            ),
                            new Container(
                                margin: EdgeInsets.only(top: 16),
                                child: new Row(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    children: new List<Widget> {
                                        new Row(
                                            children: new List<Widget> {
                                                _buildFollowCount(
                                                    "关注",
                                                    $"{(user.followingUsersCount ?? 0) + (user.followingTeamsCount ?? 0)}",
                                                    () =>
                                                        this.widget.actionModel.pushToUserFollowing(
                                                            arg1: this.widget.viewModel.user.id, 0)
                                                ),
                                                new SizedBox(width: 16),
                                                _buildFollowCount(
                                                    "粉丝",
                                                    $"{user.followCount ?? 0}",
                                                    () =>
                                                        this.widget.actionModel.pushToUserFollower(
                                                            obj: this.widget.viewModel.user.id)
                                                )
                                            }
                                        ),
                                        this.widget.viewModel.isBlockUser ? new Container() : this._buildFollowButton()
                                    }
                                )
                            )
                        }
                    )
                )
            );
        }

        Widget _buildUserArticleTitle() {
            return new Container(
                height: 44,
                padding: EdgeInsets.only(bottom: 10),
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(
                        bottom: new BorderSide(
                            color: CColors.Separator2
                        )
                    )
                ),
                child: new CustomTabBarHeader(
                    tabs: new List<Widget> {
                        new Text("文章"),
                        new Text("收藏"),
                        new Text("关注")
                    },
                    controller: this._tabController,
                    indicatorSize: CustomTabBarIndicatorSize.fixedOrLabel,
                    indicator: new CustomGradientsTabIndicator(
                        height: 8,
                        gradient: new LinearGradient(
                            begin: Alignment.centerLeft,
                            end: Alignment.centerRight,
                            new List<Color> {
                                new Color(0xFFB1E0FF),
                                new Color(0xFF6EC6FF)
                            }
                        )
                    ),
                    indicatorPadding: EdgeInsets.zero,
                    indicatorChangeStyle: CustomTabBarIndicatorChangeStyle.enlarge,
                    labelPadding: EdgeInsets.symmetric(horizontal: 16),
                    unselectedLabelColor: CColors.TextBody4,
                    labelColor: CColors.TextTitle,
                    unselectedLabelStyle: new TextStyle(
                        fontSize: 16,
                        fontFamily: "Roboto-Regular"
                    ),
                    labelStyle: new TextStyle(
                        fontSize: 16,
                        fontFamily: "Roboto-Medium"
                    ),
                    isScrollable: true
                )
            );
        }

        static Widget _buildFollowCount(string title, string subTitle, GestureTapCallback onTap) {
            return new GestureDetector(
                onTap: onTap,
                child: new Container(
                    height: 32,
                    alignment: Alignment.center,
                    color: CColors.Transparent,
                    child: new Row(
                        children: new List<Widget> {
                            new Text(data: title, style: CTextStyle.PRegularWhite),
                            new SizedBox(width: 2),
                            new Text(
                                data: subTitle,
                                style: new TextStyle(
                                    height: 1.27f,
                                    fontSize: 20,
                                    fontFamily: "Roboto-Bold",
                                    color: CColors.White
                                )
                            )
                        }
                    )
                )
            );
        }

        Widget _buildMoreButton(bool hasUser) {
            return this.widget.viewModel.isMe
                ? (Widget) new Container()
                : new CustomButton(
                    padding: EdgeInsets.only(16, 8, 8, 8),
                    onPressed: () => ReportManager.showBlockUserView(
                        isLoggedIn: this.widget.viewModel.isLoggedIn,
                        hasBeenBlocked: this.widget.viewModel.isBlockUser,
                        () => this.widget.actionModel.pushToLogin(),
                        () => this.widget.actionModel.blockUserAction(false),
                        () => this.widget.actionModel.blockUserAction(true)
                    ),
                    child: new Icon(
                        icon: Icons.ellipsis,
                        size: 24,
                        color: hasUser
                            ? this._hideNavBar ? CColors.White : CColors.Icon
                            : CColors.Icon
                    )
                );
        }

        Widget _buildFollowButton(bool isTop = false) {
            if (this.widget.viewModel.isLoggedIn
                && this.widget.viewModel.currentUserId == this.widget.viewModel.user.id) {
                if (isTop) {
                    return new Container();
                }

                return new CustomButton(
                    padding: EdgeInsets.zero,
                    child: new Container(
                        width: 100,
                        height: 32,
                        alignment: Alignment.center,
                        decoration: new BoxDecoration(
                            color: CColors.Transparent,
                            borderRadius: BorderRadius.all(4),
                            border: Border.all(
                                color: CColors.White
                            )
                        ),
                        child: new Text("编辑资料", style: CTextStyle.PMediumWhite)
                    ),
                    onPressed: () => {
                        if (this.widget.viewModel.isLoggedIn) {
                            if (this.widget.viewModel.user.jobRoleMap == null) {
                                return;
                            }

                            this.widget.actionModel.pushToEditPersonalInfo(this.widget.viewModel.user.id);
                        }
                        else {
                            this.widget.actionModel.pushToLogin();
                        }
                    }
                );
            }

            bool isFollow;
            string followText;
            Color followBgColor;
            GestureTapCallback onTap;
            if (this.widget.viewModel.isLoggedIn
                && this.widget.viewModel.followMap.ContainsKey(key: this.widget.viewModel.user.id)) {
                isFollow = true;
                followText = "已关注";
                followBgColor = CColors.Transparent;
                onTap = () => {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不再关注？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", type: ActionType.normal,
                                    () => {
                                        this.widget.actionModel.startUnFollowUser(obj: this.widget.viewModel.user.id);
                                        this.widget.actionModel.unFollowUser(arg: this.widget.viewModel.user.id);
                                    }),
                                new ActionSheetItem("取消", type: ActionType.cancel)
                            }
                        )
                    );
                };
            }
            else {
                isFollow = false;
                followText = "关注";
                followBgColor = CColors.PrimaryBlue;
                onTap = () => {
                    this.widget.actionModel.startFollowUser(obj: this.widget.viewModel.user.id);
                    this.widget.actionModel.followUser(arg: this.widget.viewModel.user.id);
                };
            }

            Widget buttonChild;
            bool isEnable;
            if (this.widget.viewModel.user.followUserLoading ?? false) {
                buttonChild = new CustomActivityIndicator(
                    loadingColor: isTop ? LoadingColor.black : LoadingColor.white,
                    size: LoadingSize.small
                );
                isEnable = false;
            }
            else {
                buttonChild = new Text(
                    data: followText,
                    style: isTop
                        ? new TextStyle(
                            fontSize: 14,
                            fontFamily: "Roboto-Medium",
                            color: isFollow ? new Color(0xFF959595) : CColors.PrimaryBlue
                        )
                        : CTextStyle.PMediumWhite
                );
                isEnable = true;
            }

            if (isTop) {
                return new CustomButton(
                    padding: EdgeInsets.zero,
                    child: new Container(
                        width: 60,
                        height: 28,
                        alignment: Alignment.center,
                        decoration: new BoxDecoration(
                            color: CColors.Transparent,
                            borderRadius: BorderRadius.circular(14),
                            border: isFollow
                                ? Border.all(color: CColors.Disable2)
                                : Border.all(color: CColors.PrimaryBlue)
                        ),
                        child: buttonChild
                    ),
                    onPressed: () => {
                        if (!isEnable) {
                            return;
                        }

                        if (this.widget.viewModel.isLoggedIn) {
                            onTap();
                        }
                        else {
                            this.widget.actionModel.pushToLogin();
                        }
                    }
                );
            }

            return new CustomButton(
                padding: EdgeInsets.zero,
                child: new Container(
                    width: 100,
                    height: 32,
                    alignment: Alignment.center,
                    decoration: new BoxDecoration(
                        color: followBgColor,
                        borderRadius: BorderRadius.all(4),
                        border: isFollow ? Border.all(color: CColors.White) : null
                    ),
                    child: buttonChild
                ),
                onPressed: () => {
                    if (!isEnable) {
                        return;
                    }

                    if (this.widget.viewModel.isLoggedIn) {
                        onTap();
                    }
                    else {
                        this.widget.actionModel.pushToLogin();
                    }
                }
            );
        }

        Widget _buildFavoriteCard(string favoriteId) {
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
                    () => this.widget.actionModel.pushToFavoriteDetail(arg1: this.widget.viewModel.user.id,
                        arg2: favoriteTag.id)
                ),
                new CustomDismissibleDrawerDelegate(),
                secondaryActions: this._buildSecondaryActions(favoriteTag: favoriteTag),
                controller: this._dismissibleController
            );
        }

        List<Widget> _buildSecondaryActions(FavoriteTag favoriteTag) {
            if (!this.widget.viewModel.isLoggedIn) {
                return new List<Widget>();
            }

            if (this.widget.viewModel.currentUserId != this.widget.viewModel.user.id) {
                return new List<Widget>();
            }

            if (favoriteTag.type == "default") {
                return new List<Widget>();
            }

            var secondaryActions = new List<Widget> {
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
                )
            };
            if (this._selectedIndex == 1) {
                secondaryActions.Add(new EditActionButton(
                    80,
                    EdgeInsets.only(12, right: 24),
                    () => this.widget.actionModel.pushToCreateFavorite(obj: favoriteTag.id)
                ));
            }

            return secondaryActions;
        }

        public void didPopNext() {
            if (this.widget.viewModel.userId.isNotEmpty()) {
                CTemporaryValue.currentPageModelId = this.widget.viewModel.userId;
            }

            StatusBarManager.statusBarStyle(isLight: this._hideNavBar);
        }

        public void didPush() {
            if (this.widget.viewModel.userId.isNotEmpty()) {
                CTemporaryValue.currentPageModelId = this.widget.viewModel.userId;
            }
        }

        public void didPop() {
            if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                this.widget.viewModel.userId == CTemporaryValue.currentPageModelId) {
                CTemporaryValue.currentPageModelId = null;
            }
        }

        public void didPushNext() {
            CTemporaryValue.currentPageModelId = null;
        }
    }
}