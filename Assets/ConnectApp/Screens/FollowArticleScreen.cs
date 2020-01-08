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
    public class FollowArticleScreenConnector : StatelessWidget {
        public FollowArticleScreenConnector(
            int selectedIndex,
            Key key = null
        ) : base(key: key) {
            this.selectedIndex = selectedIndex;
        }

        readonly int selectedIndex;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenViewModel>(
                converter: state => {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var followArticleIds = state.articleState.followArticleIdDict.ContainsKey(key: currentUserId)
                        ? state.articleState.followArticleIdDict[key: currentUserId]
                        : new List<string>();
                    var hotArticleIds = state.articleState.hotArticleIdDict.ContainsKey(key: currentUserId)
                        ? state.articleState.hotArticleIdDict[key: currentUserId]
                        : new List<string>();
                    var user = state.userState.userDict.ContainsKey(key: currentUserId)
                        ? state.userState.userDict[key: currentUserId]
                        : new User();
                    var followings = user.followings ?? new List<Following>();
                    var likeMap = state.likeState.likeDict.ContainsKey(key: currentUserId)
                        ? state.likeState.likeDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new ArticlesScreenViewModel {
                        followArticlesLoading = state.articleState.followArticlesLoading,
                        followingLoading = state.userState.followingLoading,
                        followArticleIds = followArticleIds,
                        hotArticleIds = hotArticleIds,
                        followings = followings,
                        blockArticleList = state.articleState.blockArticleList,
                        followArticleHasMore = state.articleState.followArticleHasMore,
                        hotArticleHasMore = state.articleState.hotArticleHasMore,
                        hotArticlePage = state.articleState.hotArticlePage,
                        articleDict = state.articleState.articleDict,
                        userDict = state.userState.userDict,
                        userLicenseDict = state.userState.userLicenseDict,
                        teamDict = state.teamState.teamDict,
                        likeMap = likeMap,
                        followMap = followMap,
                        isLoggedIn = state.loginState.isLoggedIn,
                        currentUserId = state.loginState.loginInfo.userId ?? "",
                        beforeTime = state.articleState.beforeTime,
                        afterTime = state.articleState.afterTime,
                        selectedIndex = this.selectedIndex
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ArticlesScreenActionModel {
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToArticleDetail = id => dispatcher.dispatch(new MainNavigatorPushToArticleDetailAction {
                            articleId = id
                        }),
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
                        pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
                        }),
                        pushToTeamDetail = teamId => dispatcher.dispatch(new MainNavigatorPushToTeamDetailAction {
                            teamId = teamId
                        }),
                        startFollowUser = userId =>
                            dispatcher.dispatch(new StartFollowUserAction {followUserId = userId}),
                        followUser = userId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId: userId)),
                        startUnFollowUser = userId =>
                            dispatcher.dispatch(new StartUnFollowUserAction {unFollowUserId = userId}),
                        unFollowUser = userId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(userId)),
                        startFollowTeam = teamId =>
                            dispatcher.dispatch(new StartFetchFollowTeamAction {followTeamId = teamId}),
                        followTeam = teamId => dispatcher.dispatch<IPromise>(Actions.fetchFollowTeam(teamId)),
                        startUnFollowTeam = teamId =>
                            dispatcher.dispatch(new StartFetchUnFollowTeamAction {unFollowTeamId = teamId}),
                        unFollowTeam = teamId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowTeam(teamId)),
                        sendComment = (articleId, channelId, content, nonce, parentMessageId, upperMessageId) => {
                            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog());
                            return dispatcher.dispatch<IPromise>(
                                Actions.sendComment(articleId, channelId, content, nonce, parentMessageId,
                                    upperMessageId));
                        },
                        likeArticle = articleId =>
                            dispatcher.dispatch<IPromise>(Actions.likeArticle(articleId: articleId)),
                        startFetchFollowing = () => dispatcher.dispatch(new StartFetchFollowingAction()),
                        fetchFollowing = (userId, offset) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowing(userId: userId, offset: offset)),
                        startFetchFollowArticles = () => dispatcher.dispatch(new StartFetchFollowArticlesAction()),
                        fetchFollowArticles = (pageNumber, isFirst, isHot) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowArticles(pageNumber, viewModel.beforeTime,
                                viewModel.afterTime, isFirst, isHot)),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new FollowArticleScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class FollowArticleScreen : StatefulWidget {
        public FollowArticleScreen(
            ArticlesScreenViewModel viewModel = null,
            ArticlesScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ArticlesScreenViewModel viewModel;
        public readonly ArticlesScreenActionModel actionModel;

        public override State createState() {
            return new _FollowArticleScreenState();
        }
    }

    class _FollowArticleScreenState : AutomaticKeepAliveClientMixin<FollowArticleScreen> {
        const int firstPageNumber = 1;
        const int cardNumber = 5;
        int _pageNumber = firstPageNumber;
        RefreshController _refreshController;
        float cardWidth;
        float avatarSize;
        string _followUserSubId;
        string _loginSubId;
        string _articleTabSubId;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            this.cardWidth = 0;
            this.avatarSize = 0;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchFollowing();
                this.widget.actionModel.fetchFollowing(arg1: this.widget.viewModel.currentUserId, 0);
                this.widget.actionModel.startFetchFollowArticles();
                this.widget.actionModel.fetchFollowArticles(arg1: firstPageNumber, true, false);
            });
            this._followUserSubId = EventBus.subscribe(sName: EventBusConstant.follow_user, args => {
                this._pageNumber = firstPageNumber;
                this.widget.actionModel.fetchFollowing(arg1: this.widget.viewModel.currentUserId, 0);
                this.widget.actionModel.fetchFollowArticles(arg1: firstPageNumber, true, false);
            });
            this._loginSubId = EventBus.subscribe(sName: EventBusConstant.login_success, args => {
                var currentUserId = args != null && args.Count > 0
                    ? (string)args[0]
                    : this.widget.viewModel.currentUserId;
                this._pageNumber = firstPageNumber;
                this.widget.actionModel.fetchFollowing(arg1: currentUserId, 0);
                this.widget.actionModel.fetchFollowArticles(arg1: firstPageNumber, true, false);
            });
            this._articleTabSubId = EventBus.subscribe(sName: EventBusConstant.article_tab, args => {
                if (this.widget.viewModel.selectedIndex == 0) {
                    this._refreshController.sendBack(true, mode: RefreshStatus.refreshing);
                    this._refreshController.animateTo(0.0f, TimeSpan.FromMilliseconds(300), curve: Curves.linear);
                }
            });
        }

        public override void dispose() {
            EventBus.unSubscribe(sName: EventBusConstant.follow_user, id: this._followUserSubId);
            EventBus.unSubscribe(sName: EventBusConstant.login_success, id: this._loginSubId);
            EventBus.unSubscribe(sName: EventBusConstant.article_tab, id: this._articleTabSubId);
            base.dispose();
        }

        protected override bool wantKeepAlive {
            get { return true; }
        }

        void _onRefresh(bool up, bool isHot) {
            if (up) {
                this._pageNumber = firstPageNumber;
                this.widget.actionModel.fetchFollowing(arg1: this.widget.viewModel.currentUserId, 0);
            }
            else {
                if (isHot) {
                    this._pageNumber = this.widget.viewModel.hotArticlePage + 1;
                }
                else {
                    this._pageNumber++;
                }
            }

            this.widget.actionModel.fetchFollowArticles(arg1: this._pageNumber, arg2: up, arg3: isHot)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        void _onFollow(Article article, UserType userType) {
            if (this.widget.viewModel.isLoggedIn) {
                if (userType == UserType.follow) {
                    ActionSheetUtils.showModalActionSheet(
                        new ActionSheet(
                            title: "确定不再关注？",
                            items: new List<ActionSheetItem> {
                                new ActionSheetItem("确定", type: ActionType.normal, () => {
                                    if (article.ownerType == OwnerType.user.ToString()) {
                                        this.widget.actionModel.startUnFollowUser(obj: article.userId);
                                        this.widget.actionModel.unFollowUser(arg: article.userId);
                                    }

                                    if (article.ownerType == OwnerType.team.ToString()) {
                                        this.widget.actionModel.startUnFollowTeam(obj: article.teamId);
                                        this.widget.actionModel.unFollowTeam(arg: article.teamId);
                                    }
                                }),
                                new ActionSheetItem("取消", type: ActionType.cancel)
                            }
                        )
                    );
                }

                if (userType == UserType.unFollow) {
                    if (article.ownerType == OwnerType.user.ToString()) {
                        this.widget.actionModel.startFollowUser(obj: article.userId);
                        this.widget.actionModel.followUser(arg: article.userId);
                    }

                    if (article.ownerType == OwnerType.team.ToString()) {
                        this.widget.actionModel.startFollowTeam(obj: article.teamId);
                        this.widget.actionModel.followTeam(arg: article.teamId);
                    }
                }
            }
            else {
                this.widget.actionModel.pushToLogin();
            }
        }

        void _onLike(Article article) {
            if (!this.widget.viewModel.isLoggedIn) {
                this.widget.actionModel.pushToLogin();
            }
            else {
                if (!this.widget.viewModel.likeMap.ContainsKey(key: article.id)) {
                    this.widget.actionModel.likeArticle(arg: article.id);
                }
            }
        }

        void _onComment(Article article) {
            if (!this.widget.viewModel.isLoggedIn) {
                this.widget.actionModel.pushToLogin();
            }
            else {
                ActionSheetUtils.showModalActionSheet(new CustomInput(
                    doneCallBack: text => {
                        ActionSheetUtils.hiddenModalPopup();
                        this.widget.actionModel.sendComment(
                            arg1: article.id,
                            arg2: article.channelId,
                            arg3: text,
                            Snowflake.CreateNonce(),
                            "",
                            ""
                        );
                    })
                );
            }
        }

        void _onShare(Article article) {
            var linkUrl = CStringUtils.JointProjectShareLink(projectId: article.id);
            ShareManager.showArticleShareView(
                true,
                isLoggedIn: this.widget.viewModel.isLoggedIn,
                () => {
                    Clipboard.setData(new ClipboardData(text: linkUrl));
                    CustomDialogUtils.showToast("复制链接成功", iconData: Icons.check_circle_outline);
                },
                () => this.widget.actionModel.pushToLogin(),
                () => this.widget.actionModel.pushToBlock(obj: article.id),
                () => this.widget.actionModel.pushToReport(arg1: article.id, arg2: ReportType.article),
                type => {
                    CustomDialogUtils.showCustomDialog(
                        child: new CustomLoadingDialog()
                    );
                    string imageUrl = CImageUtils.SizeTo200ImageUrl(article.thumbnail.url);
                    this.widget.actionModel.shareToWechat(arg1: type, arg2: article.title,
                            arg3: article.subTitle, arg4: linkUrl, arg5: imageUrl)
                        .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                        .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                }
            );
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            if (0 == this.cardWidth || 0 == this.avatarSize) {
                this.cardWidth = (MediaQuery.of(context: context).size.width - 18 * 2 - 10 * (cardNumber - 1)) /
                                 cardNumber; // 18 是边缘的 padding, 10 是 卡片间的间距
                this.avatarSize = this.cardWidth - 4 * 2; // 4 是头像的水平 padding
            }

            return new Container(
                color: CColors.White,
                child: this._buildContent()
            );
        }

        Widget _buildContent() {
            var followArticleIds = this.widget.viewModel.followArticleIds;
            var followings = this.widget.viewModel.followings;
            var hotArticleIds = this.widget.viewModel.hotArticleIds;
            var followArticleLoading = this.widget.viewModel.followArticlesLoading
                                       && followArticleIds.isEmpty()
                                       && hotArticleIds.isEmpty();
            var followingLoading = this.widget.viewModel.followingLoading
                                   && followings.isEmpty();
            Widget content;
            if (followArticleLoading || followingLoading) {
                content = new FollowArticleLoading();
            }
            else {
                var isHot = this.widget.viewModel.hotArticlePage > 0;
                var itemCount = isHot ? hotArticleIds.Count : followArticleIds.Count;
                var enablePullUp = isHot
                    ? this.widget.viewModel.hotArticleHasMore
                    : this.widget.viewModel.followArticleHasMore;
                content = new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: up => this._onRefresh(up: up, isHot: isHot),
                    hasBottomMargin: true,
                    itemCount: itemCount,
                    itemBuilder: (cxt, index) => this._buildFollowArticleCard(context: cxt, index: index, isFollow: !isHot),
                    headerWidget: followings.isEmpty() ? null : this._buildFollowingList(),
                    footerWidget: enablePullUp ? null : new EndView(hasBottomMargin: true),
                    hasScrollBar: false
                );
            }

            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(child: content)
            );
        }

        Widget _buildFollowingList() {
            var followings = this.widget.viewModel.followings;
            if (followings.isEmpty()) {
                return new Container();
            }

            var followButtons = new List<Widget>();
            if (followings.Count > 10) {
                followings = followings.GetRange(0, 10);
            }

            for (int i = 0; i < followings.Count; i++) {
                var following = followings[index: i];
                var followButton = this._buildFollowButton(following: following);
                if (i == 0) {
                    followButtons.Add(new Container(width: 18));
                }
                followButtons.Add(item: followButton);
                if (i < followings.Count - 1) {
                    followButtons.Add(new Container(width: 10));
                }
                if (i == followings.Count - 1) {
                    followButtons.Add(new Container(width: 18));
                }
            }

            return new Container(
                padding: EdgeInsets.only(bottom: 16),
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(
                        bottom: new BorderSide(
                            color: CColors.Background,
                            8
                        )
                    )
                ),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(16, 0, 0, 12),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget> {
                                    new Padding(
                                        padding: EdgeInsets.only(top: 16),
                                        child: new Text(
                                            "最近浏览",
                                            style: CTextStyle.PMediumBody2
                                        )
                                    ),
                                    new GestureDetector(
                                        onTap: () =>
                                            this.widget.actionModel.pushToUserFollowing(
                                                obj: this.widget.viewModel.currentUserId),
                                        child: new Container(
                                            color: CColors.Transparent,
                                            child: new Row(
                                                children: new List<Widget> {
                                                    new Padding(
                                                        padding: EdgeInsets.only(16, 18),
                                                        child: new Text(
                                                            "查看全部",
                                                            style: new TextStyle(
                                                                fontSize: 12,
                                                                fontFamily: "Roboto-Regular",
                                                                color: CColors.TextBody4
                                                            )
                                                        )
                                                    ),
                                                    new Padding(
                                                        padding: EdgeInsets.only(top: 16, right: 8),
                                                        child: new Icon(
                                                            icon: Icons.chevron_right,
                                                            size: 20,
                                                            color: Color.fromRGBO(199, 203, 207, 1)
                                                        )
                                                    )
                                                }
                                            )
                                        )
                                    )
                                }
                            )
                        ),
                        new Container(
                            height: this.avatarSize + 25,
                            color: CColors.White,
                            child: new ListView(
                                physics: new AlwaysScrollableScrollPhysics(),
                                scrollDirection: Axis.horizontal,
                                children: followButtons
                            )
                        )
                    }
                )
            );
        }

        Widget _buildFollowButton(Following following) {
            var user = new User();
            if (following.type == "user") {
                user = this.widget.viewModel.userDict[key: following.followeeId];
            }

            var team = new Team();
            if (following.type == "team") {
                team = this.widget.viewModel.teamDict[key: following.followeeId];
            }

            return new GestureDetector(
                onTap: () => {
                    if (following.type == "user") {
                        this.widget.actionModel.pushToUserDetail(obj: user.id);
                    }

                    if (following.type == "team") {
                        this.widget.actionModel.pushToTeamDetail(obj: team.id);
                    }
                },
                child: new Container(
                    width: this.cardWidth,
                    child: new Column(
                        children: new List<Widget> {
                            new Container(
                                padding: EdgeInsets.symmetric(horizontal: 4),
                                child: following.type == "user"
                                    ? Avatar.User(
                                        user: user,
                                        size: this.avatarSize
                                    )
                                    : Avatar.Team(
                                        team: team,
                                        size: this.avatarSize,
                                        avatarShape: AvatarShape.circle
                                    )
                            ),
                            new Container(
                                child: new Text(
                                    following.type == "user"
                                        ? user.fullName ?? user.name
                                        : team.name,
                                    style: CTextStyle.PRegularBody,
                                    textAlign: TextAlign.center,
                                    maxLines: 1,
                                    overflow: TextOverflow.ellipsis
                                )
                            )
                        }
                    )
                )
            );
        }

        Widget _buildFollowArticleCard(BuildContext context, int index, bool isFollow = true) {
            var articleIds = isFollow
                ? this.widget.viewModel.followArticleIds
                : this.widget.viewModel.hotArticleIds;

            var articleId = articleIds[index: index];
            if (!this.widget.viewModel.articleDict.ContainsKey(key: articleId)) {
                return new Container();
            }

            var article = this.widget.viewModel.articleDict[key: articleId];
            if (this.widget.viewModel.blockArticleList.Contains(item: article.id)) {
                return new Container();
            }

            if (article.ownerType == OwnerType.user.ToString()) {
                if (this.widget.viewModel.currentUserId == article.userId) {
                    return new Container();
                }

                var user = this.widget.viewModel.userDict[key: article.userId];
                UserType userType;
                if (!this.widget.viewModel.isLoggedIn) {
                    userType = UserType.unFollow;
                }
                else {
                    if (this.widget.viewModel.currentUserId == article.userId) {
                        userType = UserType.me;
                    }
                    else if (user.followUserLoading ?? false) {
                        userType = UserType.loading;
                    }
                    else if (this.widget.viewModel.followMap.ContainsKey(key: article.userId)) {
                        userType = UserType.follow;
                    }
                    else {
                        userType = UserType.unFollow;
                    }
                }

                return FollowArticleCard.User(
                    article: article,
                    user: user,
                    CCommonUtils.GetUserLicense(userId: user.id, userLicenseMap: this.widget.viewModel.userLicenseDict),
                    this.widget.viewModel.likeMap.ContainsKey(key: article.id),
                    userType: userType,
                    () => this.widget.actionModel.pushToArticleDetail(obj: article.id),
                    () => this.widget.actionModel.pushToUserDetail(obj: user.id),
                    () => this._onFollow(article: article, userType: userType),
                    () => this._onLike(article: article),
                    () => this._onComment(article: article),
                    () => this._onShare(article: article),
                    new ObjectKey(value: article.id)
                );
            }

            if (article.ownerType == OwnerType.team.ToString()) {
                var team = this.widget.viewModel.teamDict[key: article.teamId];
                UserType userType;
                if (!this.widget.viewModel.isLoggedIn) {
                    userType = UserType.unFollow;
                }
                else {
                    if (this.widget.viewModel.currentUserId == article.teamId) {
                        userType = UserType.me;
                    }
                    else if (team.followTeamLoading ?? false) {
                        userType = UserType.loading;
                    }
                    else if (this.widget.viewModel.followMap.ContainsKey(key: article.teamId)) {
                        userType = UserType.follow;
                    }
                    else {
                        userType = UserType.unFollow;
                    }
                }

                return FollowArticleCard.Team(
                    article: article,
                    team: team,
                    this.widget.viewModel.likeMap.ContainsKey(key: article.id),
                    userType: userType,
                    () => this.widget.actionModel.pushToArticleDetail(obj: article.id),
                    () => this.widget.actionModel.pushToTeamDetail(obj: team.id),
                    () => this._onFollow(article: article, userType: userType),
                    () => this._onLike(article: article),
                    () => this._onComment(article: article),
                    () => this._onShare(article: article),
                    new ObjectKey(value: article.id)
                );
            }

            return new Container();
        }
    }
}