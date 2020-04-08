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
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class TeamDetailScreenConnector : StatelessWidget {
        public TeamDetailScreenConnector(
            string teamId,
            bool isSlug = false,
            Key key = null
        ) : base(key: key) {
            this.teamId = teamId;
            this.isSlug = isSlug;
        }

        readonly string teamId;
        readonly bool isSlug;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, TeamDetailScreenViewModel>(
                converter: state => {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var team = this.FetchTeam(this.teamId, state.teamState.teamDict, state.teamState.slugDict);
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new TeamDetailScreenViewModel {
                        teamLoading = state.teamState.teamLoading,
                        teamArticleLoading = state.teamState.teamArticleLoading,
                        team = team,
                        teamId = this.teamId,
                        articleDict = state.articleState.articleDict,
                        followMap = followMap,
                        currentUserId = state.loginState.loginInfo.userId ?? "",
                        isLoggedIn = state.loginState.isLoggedIn
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new TeamDetailScreenActionModel {
                        startFetchTeam = () => dispatcher.dispatch(new StartFetchTeamAction()),
                        fetchTeam = () => dispatcher.dispatch<IPromise>(Actions.fetchTeam(this.teamId)),
                        startFetchTeamArticle = () => dispatcher.dispatch(new StartFetchTeamArticleAction()),
                        fetchTeamArticle = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchTeamArticle(viewModel.team.id, pageNumber)),
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
                        pushToTeamFollower = teamId => dispatcher.dispatch(
                            new MainNavigatorPushToTeamFollowerAction {
                                teamId = teamId
                            }
                        ),
                        pushToTeamMember = teamId => dispatcher.dispatch(
                            new MainNavigatorPushToTeamMemberAction {
                                teamId = teamId
                            }
                        ),
                        startFollowTeam = () =>
                            dispatcher.dispatch(new StartFetchFollowTeamAction {followTeamId = viewModel.team.id}),
                        followTeam = teamId => dispatcher.dispatch<IPromise>(Actions.fetchFollowTeam(teamId)),
                        startUnFollowTeam = () => dispatcher.dispatch(new StartFetchUnFollowTeamAction
                            {unFollowTeamId = viewModel.team.id}),
                        unFollowTeam = teamId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowTeam(teamId)),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new TeamDetailScreen(viewModel, actionModel);
                }
            );
        }

        Team FetchTeam(string teamId, Dictionary<string, Team> teamDict, Dictionary<string, string> slugDict) {
            if (teamDict.ContainsKey(teamId)) {
                return teamDict[teamId];
            }

            if (this.isSlug && slugDict.ContainsKey(teamId)) {
                return teamDict[slugDict[teamId]];
            }

            return null;
        }
    }

    public class TeamDetailScreen : StatefulWidget {
        public TeamDetailScreen(
            TeamDetailScreenViewModel viewModel = null,
            TeamDetailScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly TeamDetailScreenViewModel viewModel;
        public readonly TeamDetailScreenActionModel actionModel;

        public override State createState() {
            return new _TeamDetailScreenState();
        }
    }

    class _TeamDetailScreenState : State<TeamDetailScreen>, TickerProvider, RouteAware {
        const float imageBaseHeight = 212;
        const float navBarHeight = 44;
        const float _transformSpeed = 0.005f;
        int _articlePageNumber;
        RefreshController _refreshController;
        float _factor = 1;
        bool _isHaveTitle;
        bool _hideNavBar;
        Animation<RelativeRect> _animation;
        AnimationController _controller;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(true);
            this._articlePageNumber = 1;
            this._refreshController = new RefreshController();
            this._isHaveTitle = false;
            this._hideNavBar = true;
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(100),
                vsync: this
            );
            RelativeRectTween rectTween = new RelativeRectTween(
                RelativeRect.fromLTRB(0, top: navBarHeight, 0, 0),
                RelativeRect.fromLTRB(0, 0, 0, 0)
            );
            this._animation = rectTween.animate(parent: this._controller);
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchTeam();
                this.widget.actionModel.fetchTeam();
                this.widget.actionModel.startFetchTeamArticle();
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

            return true;
        }

        void _onRefresh(bool up) {
            if (up) {
                this._articlePageNumber = 1;
            }
            else {
                this._articlePageNumber++;
            }

            this.widget.actionModel.fetchTeamArticle(arg: this._articlePageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        void _share(Article article) {
            ActionSheetUtils.showModalActionSheet(new ShareView(
                projectType: ProjectType.article,
                onPressed: type => {
                    var linkUrl = CStringUtils.JointProjectShareLink(projectId: article.id);
                    if (type == ShareType.clipBoard) {
                        Clipboard.setData(new ClipboardData(text: linkUrl));
                        CustomDialogUtils.showToast("复制链接成功", iconData: Icons.check_circle_outline);
                    }
                    else if (type == ShareType.block) {
                        ReportManager.blockProject(
                            isLoggedIn: this.widget.viewModel.isLoggedIn,
                            () => this.widget.actionModel.pushToLogin(),
                            () => this.widget.actionModel.pushToBlock(article.id),
                            () => this.widget.actionModel.mainRouterPop()
                        );
                    }
                    else if (type == ShareType.report) {
                        ReportManager.report(
                            isLoggedIn: this.widget.viewModel.isLoggedIn,
                            () => this.widget.actionModel.pushToLogin(),
                            () => this.widget.actionModel.pushToReport(arg1: article.id, arg2: ReportType.article)
                        );
                    }
                    else {
                        CustomDialogUtils.showCustomDialog(
                            child: new CustomLoadingDialog()
                        );
                        string imageUrl = CImageUtils.SizeTo200ImageUrl(article.thumbnail.url);
                        this.widget.actionModel.shareToWechat(type, article.title, article.subTitle,
                                linkUrl,
                                imageUrl).Then(CustomDialogUtils.hiddenCustomDialog)
                            .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                    }
                }
            ));
        }

        public override Widget build(BuildContext context) {
            Widget content = new Container();
            if (this.widget.viewModel.teamLoading && this.widget.viewModel.team == null) {
                content = new GlobalLoading();
            }
            else if (this.widget.viewModel.team == null || this.widget.viewModel.team.errorCode == "ResourceNotFound") {
                content = new BlankView("公司不存在");
            }
            else {
                content = this._buildContent(context: context);
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: false,
                    bottom: false,
                    child: new Stack(
                        children: new List<Widget> {
                            content,
                            this._buildNavigationBar(context: context)
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar(BuildContext context) {
            Widget titleWidget = new Container();
            if (this._isHaveTitle) {
                var team = this.widget.viewModel.team ?? new Team();
                titleWidget = new Row(
                    children: new List<Widget> {
                        new Expanded(
                            child: new Text(
                                data: team.name,
                                style: CTextStyle.PXLargeMedium,
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis
                            )
                        ),
                        new SizedBox(width: 8),
                        this._buildFollowButton(true),
                        new SizedBox(width: 16)
                    }
                );
            }

            var hasTeam = !(this.widget.viewModel.team == null ||
                            this.widget.viewModel.team.errorCode == "ResourceNotFound");

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
                        children: new List<Widget> {
                            new GestureDetector(
                                onTap: () => this.widget.actionModel.mainRouterPop(),
                                child: new Container(
                                    padding: EdgeInsets.only(16, 8, 8, 8),
                                    color: CColors.Transparent,
                                    child: new Icon(
                                        icon: Icons.arrow_back,
                                        size: 24,
                                        color: hasTeam
                                            ? this._hideNavBar ? CColors.White : CColors.Icon
                                            : CColors.Icon
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

        Widget _buildContent(BuildContext context) {
            var articleIds = this.widget.viewModel.team.articleIds;
            var articlesHasMore = this.widget.viewModel.team.articlesHasMore ?? false;
            var teamArticleLoading = this.widget.viewModel.teamArticleLoading && articleIds == null;
            int itemCount;
            if (teamArticleLoading) {
                itemCount = 3;
            }
            else {
                if (articleIds == null) {
                    itemCount = 3;
                }
                else {
                    var articleCount = articlesHasMore ? articleIds.Count : articleIds.Count + 1;
                    itemCount = 2 + (articleIds.Count == 0 ? 1 : articleCount);
                }
            }

            var headerHeight = imageBaseHeight + 44 + CCommonUtils.getSafeAreaTopPadding(context: context);

            return new Container(
                color: CColors.Background,
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
                                        child: this._buildTeamInfo(context: context)
                                    );
                                }

                                if (index == 1) {
                                    return _buildTeamArticleTitle();
                                }

                                if (teamArticleLoading && index == 2) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new GlobalLoading()
                                    );
                                }

                                if ((articleIds == null || articleIds.Count == 0) && index == 2) {
                                    var height = MediaQuery.of(context: context).size.height - headerHeight;
                                    return new Container(
                                        height: height,
                                        child: new BlankView(
                                            "哎呀，暂无已发布的文章",
                                            "image/default-article"
                                        )
                                    );
                                }

                                if (index == itemCount - 1 && !articlesHasMore) {
                                    return new EndView();
                                }

                                var articleId = articleIds[index - 2];
                                if (!this.widget.viewModel.articleDict.ContainsKey(key: articleId)) {
                                    return new Container();
                                }

                                var article = this.widget.viewModel.articleDict[key: articleId];
                                return new ArticleCard(
                                    article: article,
                                    () => this.widget.actionModel.pushToArticleDetail(obj: article.id),
                                    () => this._share(article: article),
                                    fullName: this.widget.viewModel.team.name,
                                    key: new ObjectKey(value: article.id)
                                );
                            }
                        )
                    )
                )
            );
        }

        Widget _buildTeamInfo(BuildContext context) {
            var team = this.widget.viewModel.team;
            return new CoverImage(
                coverImage: team.coverImage,
                height: imageBaseHeight + CCommonUtils.getSafeAreaTopPadding(context: context),
                new Container(
                    padding: EdgeInsets.only(16, 0, 16, 24),
                    child: new Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: new List<Widget> {
                            new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 16),
                                        child: Avatar.Team(
                                            team: team,
                                            80,
                                            true
                                        )
                                    ),
                                    new Expanded(
                                        child: new Column(
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                new Row(
                                                    crossAxisAlignment: CrossAxisAlignment.start,
                                                    children: new List<Widget> {
                                                        new Flexible(
                                                            child: new Text(
                                                                data: team.name,
                                                                style: CTextStyle.H4White.merge(
                                                                    new TextStyle(height: 1)),
                                                                maxLines: 1,
                                                                overflow: TextOverflow.ellipsis
                                                            )
                                                        ),
                                                        CImageUtils.GenBadgeImage(
                                                            badges: team.badges,
                                                            CCommonUtils.GetUserLicense(
                                                                userId: team.id
                                                            ),
                                                            EdgeInsets.only(4, 6)
                                                        )
                                                    }
                                                )
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
                                                    "粉丝",
                                                    $"{team.stats?.followCount ?? 0}",
                                                    () => {
                                                        if (this.widget.viewModel.isLoggedIn) {
                                                            this.widget.actionModel.pushToTeamFollower(
                                                                obj: this.widget.viewModel.team.id);
                                                        }
                                                        else {
                                                            this.widget.actionModel.pushToLogin();
                                                        }
                                                    }
                                                ),
                                                new SizedBox(width: 16),
                                                _buildFollowCount(
                                                    "成员",
                                                    $"{team.stats?.membersCount ?? 0}",
                                                    () => this.widget.actionModel.pushToTeamMember(
                                                        obj: this.widget.viewModel.team.id)
                                                )
                                            }
                                        ),
                                        this._buildFollowButton()
                                    }
                                )
                            )
                        }
                    )
                )
            );
        }

        static Widget _buildTeamArticleTitle() {
            return new Container(
                padding: EdgeInsets.only(16),
                height: 44,
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(
                        bottom: new BorderSide(
                            color: CColors.Separator2
                        )
                    )
                ),
                alignment: Alignment.centerLeft,
                child: new Text("文章", style: CTextStyle.PLargeTitle)
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

        Widget _buildFollowButton(bool isTop = false) {
            if (this.widget.viewModel.isLoggedIn
                && this.widget.viewModel.currentUserId == this.widget.viewModel.team.id) {
                return new Container();
            }

            bool isFollow = false;
            string followText = "关注";
            Color followBgColor = CColors.PrimaryBlue;
            GestureTapCallback onTap = () => {
                this.widget.actionModel.startFollowTeam();
                this.widget.actionModel.followTeam(arg: this.widget.viewModel.team.id);
            };
            if (this.widget.viewModel.isLoggedIn
                && this.widget.viewModel.followMap.ContainsKey(key: this.widget.viewModel.team.id)) {
                isFollow = true;
                followText = "已关注";
                followBgColor = CColors.Transparent;
                onTap = () => {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不再关注？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", type: ActionType.normal, () => {
                                    this.widget.actionModel.startUnFollowTeam();
                                    this.widget.actionModel.unFollowTeam(arg: this.widget.viewModel.team.id);
                                }),
                                new ActionSheetItem("取消", type: ActionType.cancel)
                            }
                        )
                    );
                };
            }

            Widget buttonChild;
            bool isEnable;
            if (this.widget.viewModel.team.followTeamLoading ?? false) {
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
                        border: isFollow ? Border.all(CColors.White) : null
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

        public void didPopNext() {
            if (this.widget.viewModel.teamId.isNotEmpty()) {
                CTemporaryValue.currentPageModelId = this.widget.viewModel.teamId;
            }

            StatusBarManager.statusBarStyle(isLight: this._hideNavBar);
        }

        public void didPush() {
            if (this.widget.viewModel.teamId.isNotEmpty()) {
                CTemporaryValue.currentPageModelId = this.widget.viewModel.teamId;
            }
        }

        public void didPop() {
            if (CTemporaryValue.currentPageModelId.isNotEmpty() &&
                this.widget.viewModel.teamId == CTemporaryValue.currentPageModelId) {
                CTemporaryValue.currentPageModelId = null;
            }
        }

        public void didPushNext() {
            CTemporaryValue.currentPageModelId = null;
        }
    }
}