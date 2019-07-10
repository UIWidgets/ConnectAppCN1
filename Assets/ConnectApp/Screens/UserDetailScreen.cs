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
using UnityEngine;
using Avatar = ConnectApp.Components.Avatar;
using Color = Unity.UIWidgets.ui.Color;
using Config = ConnectApp.Constants.Config;
using Transform = Unity.UIWidgets.widgets.Transform;

namespace ConnectApp.screens {
    public class UserDetailScreenConnector : StatelessWidget {
        public UserDetailScreenConnector(
            string userId,
            Key key = null
        ) : base(key: key) {
            this.userId = userId;
        }

        readonly string userId;
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, UserDetailScreenViewModel>(
                converter: state => {
                    var user = state.userState.userDict.ContainsKey(key: this.userId)
                        ? state.userState.userDict[key: this.userId] : null;
                    var articleOffset = user == null ? 0 : user.articles == null ? 0 : user.articles.Count;
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new UserDetailScreenViewModel {
                        userId = this.userId,
                        userLoading = state.userState.userLoading,
                        userArticleLoading = state.userState.userArticleLoading,
                        followUserLoading = state.userState.followUserLoading,
                        user = user,
                        followMap = followMap,
                        articleOffset = articleOffset,
                        currentUserId = currentUserId,
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new UserDetailScreenActionModel {
                        startFetchUserProfile = () => dispatcher.dispatch(new StartFetchUserProfileAction()),
                        fetchUserProfile = () => dispatcher.dispatch<IPromise>(Actions.fetchUserProfile(this.userId)),
                        fetchUserArticle = offset => dispatcher.dispatch<IPromise>(Actions.fetchUserArticle(this.userId, offset)),
                        startFollowUser = () => dispatcher.dispatch(new StartFetchFollowUserAction()),
                        followUser = () => dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(this.userId)),
                        startUnFollowUser = () => dispatcher.dispatch(new StartFetchUnFollowUserAction()),
                        unFollowUser = () => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(this.userId)),
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToArticleDetail = id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
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
                        pushToUserFollowing = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserFollowingAction {
                                userId = userId
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
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new UserDetailScreen(viewModel, actionModel);
                }
            );
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

    class _UserDetailScreenState : State<UserDetailScreen>, TickerProvider {
        readonly GlobalKey personAvatarKey = GlobalKey.key("person-avatar");
        const float headerHeight = 256;
        const float _transformSpeed = 0.005f;
        int _articleOffset;
        RefreshController _refreshController;
        float _factor = 1;
        float _avatarHeight;
        bool _isHaveTitle;
        bool _showNavBarShadow;
        float _topPadding;
        Animation<RelativeRect> _animation;
        AnimationController _controller;
        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(true);
            this._articleOffset = 0;
            this._refreshController = new RefreshController();
            this._avatarHeight = 0.0f;
            this._isHaveTitle = false;
            this._showNavBarShadow = true;
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            RelativeRectTween rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, 44, 0, 0),
                RelativeRect.fromLTRB(0, 0, 0, 0)
            );
            this._animation = rectTween.animate(this._controller);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchUserProfile();
                this.widget.actionModel.fetchUserProfile();
            });
        }

        public override void dispose() {
            StatusBarManager.statusBarStyle(false);
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick, () => $"created by {this}");
        }

        void _scrollListener() {
            var scrollController = this._refreshController.scrollController;
            if (scrollController.offset < 0) {
                this._factor = 1 + scrollController.offset.abs() * _transformSpeed;
                this.setState(() => { });
            } else {
                if (this._factor != 1) {
                    this.setState(() => this._factor = 1);
                }
            }
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (this._avatarHeight == 0.0f) {
                var renderBox = this.personAvatarKey.currentContext.findRenderObject();
                this._avatarHeight = renderBox.getTransformTo(null).getTranslateY()
                                     - (44 + this._topPadding) // (44 + this._topPadding) 是顶部的高度
                                     + 80;  // 80 是头像的高度;
            }

            if (pixels >= 44 + this._topPadding) {
                if (this._showNavBarShadow) {
                    this.setState(() => this._showNavBarShadow = false);
                    StatusBarManager.statusBarStyle(false);
                }
            }
            else {
                if (!this._showNavBarShadow) {
                    this.setState(() => this._showNavBarShadow = true);
                    StatusBarManager.statusBarStyle(true);
                }
            }

            if (pixels > this._avatarHeight) {
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

        void _onRefresh(bool up) {
            this._articleOffset = up ? 0 : this.widget.viewModel.articleOffset;
            this.widget.actionModel.fetchUserArticle(this._articleOffset)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }

        void _share(Article article) {
            ShareUtils.showShareView(new ShareView(
                projectType: ProjectType.article,
                onPressed: type => {
                    string linkUrl = $"{Config.apiAddress}/p/{article.id}";
                    if (type == ShareType.clipBoard) {
                        Clipboard.setData(new ClipboardData(linkUrl));
                        CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                    }
                    else if (type == ShareType.block) {
                        ReportManager.block(this.widget.viewModel.isLoggedIn, article.id,
                            this.widget.actionModel.pushToLogin, this.widget.actionModel.pushToBlock,
                            this.widget.actionModel.mainRouterPop
                        );
                    }
                    else if (type == ShareType.report) {
                        ReportManager.report(this.widget.viewModel.isLoggedIn, article.id,
                            ReportType.article, this.widget.actionModel.pushToLogin,
                            this.widget.actionModel.pushToReport
                        );
                    }
                    else {
                        CustomDialogUtils.showCustomDialog(
                            child: new CustomLoadingDialog()
                        );
                        string imageUrl = $"{article.thumbnail.url}.200x0x1.jpg";
                        this.widget.actionModel.shareToWechat(type, article.title, article.subTitle,
                                linkUrl,
                                imageUrl).Then(CustomDialogUtils.hiddenCustomDialog)
                            .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                    }
                }
            ));
        }

        public override Widget build(BuildContext context) {
            if (this._topPadding != MediaQuery.of(context).padding.top &&
                Application.platform != RuntimePlatform.Android) {
                this._topPadding = MediaQuery.of(context).padding.top;
            }
            Widget content = new Container();
            if (this.widget.viewModel.userLoading && this.widget.viewModel.user == null) {
                content = new GlobalLoading();
            } else if (this.widget.viewModel.user == null) {
                content = new Container();
            }
            else {
                content = this._buildUserContent(context: context);
            }
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    child: new Stack(
                        children: new List<Widget> {
                            content,
                            this._buildNavigationBar()
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            Widget titleWidget = new Container();
            if (this._isHaveTitle) {
                var user = this.widget.viewModel.user ?? new User();
                titleWidget = new Row(
                    children: new List<Widget> {
                        Avatar.User(
                            this.widget.viewModel.userId,
                            user
                        ),
                        new SizedBox(width: 16),
                        this._buildFollowButton()
                    }
                );
            }
            return new Positioned(
                left: 0,
                top: 0,
                right: 0,
                height: 44 + this._topPadding,
                child: new Container(
                    decoration: new BoxDecoration(
                        CColors.White,
                        border: new Border(
                            bottom: new BorderSide(this._isHaveTitle ? CColors.Separator2 : CColors.Transparent)),
                        gradient: this._showNavBarShadow
                            ? new LinearGradient(
                                colors: new List<Color> {
                                    new Color(0x80000000),
                                    new Color(0x0)
                                },
                                begin: Alignment.topCenter,
                                end: Alignment.bottomCenter
                            )
                            : null
                    ),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new GestureDetector(
                                onTap: () => this.widget.actionModel.mainRouterPop(),
                                child: new Container(
                                    padding: EdgeInsets.only(16, 10, 0, 10),
                                    color: CColors.Transparent,
                                    child: new Icon(
                                        Icons.arrow_back,
                                        size: 24,
                                        color: this._showNavBarShadow ? CColors.White : CColors.Icon
                                    )
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
                            )
                        }
                    )
                )
            );
        }
        
        Widget _buildUserContent(BuildContext context) {
            var articles = this.widget.viewModel.user.articles ?? new List<Article>();
            var articlesHasMore = this.widget.viewModel.user.articlesHasMore;
            int itemCount;
            if (this.widget.viewModel.userArticleLoading && articles.Count == 0) {
                itemCount = 2;
            }
            else {
                var articleCount = articlesHasMore ? articles.Count : articles.Count + 1;
                itemCount = 2 + (articles.Count == 0 ? 1 : articleCount);
            }
            return new Container(
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: false,
                        enablePullUp: articlesHasMore,
                        onRefresh: this._onRefresh,
                        onNotification: this._onNotification,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: itemCount,
                            itemBuilder: (cxt, index) => {
                                if (index == 0) {
                                    return Transform.scale(
                                        scale: this._factor,
                                        child: this._buildUserInfo()
                                    );
                                }

                                if (index == 1) {
                                    return _buildUserArticleTitle();
                                }

                                if (articles.Count == 0 && index == 2) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight - 44;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "哎呀，暂无已发布的文章",
                                            "image/default-article"
                                        )
                                    );
                                }

                                if (index == itemCount - 1) {
                                    return new EndView();
                                }

                                var article = articles[index - 2];
                                return new ArticleCard(
                                    article,
                                    () => this.widget.actionModel.pushToArticleDetail(article.id),
                                    () => this._share(article: article),
                                    this.widget.viewModel.user.fullName,
                                    key: new ObjectKey(article.id)
                                );
                            }
                        )
                    )
                )
            );
        }

        Widget _buildUserInfo() {
            var user = this.widget.viewModel.user ?? new User();
            Widget titleWidget = new Container();
            if (user.title != null && user.title.isNotEmpty()) {
                titleWidget = new Text(
                    user.title,
                    style: new TextStyle(
                        height: 1.46f,
                        fontSize: 14,
                        fontFamily: "Roboto-Regular",
                        color: CColors.BgGrey
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis
                );
            }
            
            Widget bgWidget = new Container(
                color: CColors.Red
            );
            if (user.coverImage.isNotEmpty()) {
                bgWidget = new PlaceholderImage(
                    user.coverImage,
                    height: headerHeight,
                    fit: BoxFit.cover
                );
            }

            return new Container(
                height: headerHeight,
                child: new Stack(
                    children: new List<Widget> {
                        bgWidget,
                        Positioned.fill(
                            new Container(
                                color: Color.fromRGBO(0, 0, 0, 0.08f),
                                padding: EdgeInsets.only(16, 0, 16, 24),
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.end,
                                    children: new List<Widget> {
                                        new Row(
                                            children: new List<Widget> {
                                                new Container(
                                                    margin: EdgeInsets.only(right: 16),
                                                    child: Avatar.User(
                                                        this.widget.viewModel.userId,
                                                        user,
                                                        80,
                                                        this.personAvatarKey
                                                    )
                                                ),
                                                new Expanded(
                                                    child: new Column(
                                                        crossAxisAlignment: CrossAxisAlignment.start,
                                                        children: new List<Widget> {
                                                            new Text(
                                                                user.fullName,
                                                                style: CTextStyle.H4White,
                                                                maxLines: 1,
                                                                overflow: TextOverflow.ellipsis
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
                                                    new Container(
                                                        child: new Row(
                                                            children: new List<Widget> {
                                                                _buildPersonalButton(
                                                                    "关注",
                                                                    $"{user.followingCount}",
                                                                    () =>
                                                                        this.widget.actionModel.pushToUserFollowing(
                                                                            this.widget.viewModel.userId)
                                                                ),
                                                                new SizedBox(width: 16),
                                                                _buildPersonalButton(
                                                                    "粉丝",
                                                                    $"{user.followCount}",
                                                                    () =>
                                                                        this.widget.actionModel.pushToUserFollower(
                                                                            this.widget.viewModel.userId)
                                                                )
                                                            }
                                                        )
                                                    ),
                                                    this._buildFollowButton()
                                                }
                                            )
                                        )
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }

        static Widget _buildUserArticleTitle() {
            return new Container(
                padding: EdgeInsets.only(16),
                height: 44,
                decoration: new BoxDecoration(
                    border: new Border(
                        bottom: new BorderSide(
                            CColors.Separator2
                        )
                    )
                ),
                alignment: Alignment.centerLeft,
                child: new Text("文章", style: CTextStyle.PLargeTitle)
            );
        }

        static Widget _buildPersonalButton(string title, string subTitle, GestureTapCallback onTap) {
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

        Widget _buildFollowButton() {
            if (this.widget.viewModel.isLoggedIn
                && this.widget.viewModel.currentUserId == this.widget.viewModel.userId) {
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
                            this.widget.actionModel.pushToEditPersonalInfo(this.widget.viewModel.userId);
                        }
                        else {
                            this.widget.actionModel.pushToLogin();
                        }
                    }
                );
            }

            bool isFollow = false;
            string followText = "关注";
            Color followBgColor = CColors.PrimaryBlue;
            GestureTapCallback onTap = () => {
                this.widget.actionModel.startFollowUser();
                this.widget.actionModel.followUser();
            };
            if (this.widget.viewModel.isLoggedIn
                && this.widget.viewModel.followMap.ContainsKey(this.widget.viewModel.userId)) {
                isFollow = true;
                followText = "已关注";
                followBgColor = CColors.Transparent;
                onTap = () => {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定取消关注吗？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", ActionType.normal,
                                    () => {
                                        this.widget.actionModel.startUnFollowUser();
                                        this.widget.actionModel.unFollowUser();
                                    }),
                                new ActionSheetItem("取消", ActionType.cancel)
                            }
                        )
                    );
                };
            }
            Widget rightChild = new Container();
            bool isEnable;
            if (this.widget.viewModel.followUserLoading) {
                rightChild = new CustomActivityIndicator(
                    loadingColor: LoadingColor.white,
                    size: LoadingSize.small
                );
                isEnable = false;
            }
            else {
                rightChild = new Text(data: followText, style: CTextStyle.PMediumWhite);
                isEnable = true;
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
                        border: isFollow ? Border.all(CColors.White) : null
                    ),
                    child: rightChild
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
    }
}