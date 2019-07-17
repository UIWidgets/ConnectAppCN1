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
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Config = ConnectApp.Constants.Config;

namespace ConnectApp.screens {
    public class FollowArticleScreenConnector : StatelessWidget {
        public FollowArticleScreenConnector(
            Key key = null
        ) : base(key: key) {
            
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenViewModel>(
                converter: state => {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var user = state.userState.userDict.ContainsKey(key: currentUserId)
                        ? state.userState.userDict[key: currentUserId]
                        : new User();
                    var followings = user.followings ?? new List<User>();
                    var likeMap = state.likeState.likeDict.ContainsKey(key: currentUserId)
                        ? state.likeState.likeDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    var followMap = state.followState.followDict.ContainsKey(key: currentUserId)
                        ? state.followState.followDict[key: currentUserId]
                        : new Dictionary<string, bool>();
                    return new ArticlesScreenViewModel {
                        followArticlesLoading = state.articleState.followArticlesLoading,
                        followingLoading = state.userState.followingLoading,
                        followArticleList = state.articleState.followArticleList,
                        hotArticleList = state.articleState.hotArticleList,
                        followingList = followings,
                        articleDict = state.articleState.articleDict,
                        blockArticleList = state.articleState.blockArticleList,
                        followArticleHasMore = state.articleState.followArticleHasMore,
                        hotArticleHasMore = state.articleState.hotArticleHasMore,
                        userDict = state.userState.userDict,
                        teamDict = state.teamState.teamDict,
                        likeMap = likeMap,
                        followMap = followMap,
                        isLoggedIn = state.loginState.isLoggedIn,
                        currentUserId = state.loginState.loginInfo.userId ?? ""
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ArticlesScreenActionModel {
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
                        pushToUserDetail = userId => dispatcher.dispatch(
                            new MainNavigatorPushToUserDetailAction {
                                userId = userId
                            }
                        ),
                        pushToTeamDetail = teamId => dispatcher.dispatch(
                            new MainNavigatorPushToTeamDetailAction {
                                teamId = teamId
                            }
                        ),
                        startFollowUser = userId => dispatcher.dispatch(new StartFetchFollowUserAction {followUserId = userId}),
                        followUser = userId => dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(userId)),
                        startUnFollowUser = userId => dispatcher.dispatch(new StartFetchUnFollowUserAction {unFollowUserId = userId}),
                        unFollowUser = userId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(userId)),
                        startFollowTeam = teamId => dispatcher.dispatch(new StartFetchFollowTeamAction {followTeamId = teamId}),
                        followTeam = teamId => dispatcher.dispatch<IPromise>(Actions.fetchFollowTeam(teamId)),
                        startUnFollowTeam = teamId => dispatcher.dispatch(new StartFetchUnFollowTeamAction {unFollowTeamId = teamId}),
                        unFollowTeam = teamId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowTeam(teamId)),
                        sendComment = (articleId, channelId, content, nonce, parentMessageId) => {
                            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog());
                            return dispatcher.dispatch<IPromise>(
                                Actions.sendComment(articleId, channelId, content, nonce, parentMessageId));
                        },
                        likeArticle = articleId => dispatcher.dispatch<IPromise>(Actions.likeArticle(articleId)),
                        startFetchFollowing = () => dispatcher.dispatch(new StartFetchFollowingAction()),
                        fetchFollowing = offset => dispatcher.dispatch<IPromise>(Actions.fetchFollowing(viewModel.currentUserId, offset)),
                        startFetchFollowArticles = () => dispatcher.dispatch(new StartFetchFollowArticlesAction()),
                        fetchFollowArticles = pageNumber => dispatcher.dispatch<IPromise>(Actions.fetchFollowArticles(pageNumber))
                    };
                    return new FollowArticleScreen(viewModel, actionModel);
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
        int _pageNumber = firstPageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchFollowing();
                this.widget.actionModel.fetchFollowing(0);
                this.widget.actionModel.startFetchFollowArticles();
                this.widget.actionModel.fetchFollowArticles(firstPageNumber);
            });
        }

        protected override bool wantKeepAlive {
            get { return true; }
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = firstPageNumber;
                this.widget.actionModel.startFetchFollowing();
                this.widget.actionModel.fetchFollowing(0);
            }
            else {
                this._pageNumber++;
            }
            this.widget.actionModel.fetchFollowArticles(arg: this._pageNumber)
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
                            null
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
                    string imageUrl = $"{article.thumbnail.url}.200x0x1.jpg";
                    this.widget.actionModel.shareToWechat(arg1: type, arg2: article.title,
                            arg3: article.subTitle, arg4: linkUrl, arg5: imageUrl)
                        .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                        .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                }
            );
        }
        public override Widget build(BuildContext context) {
            base.build(context: context);
            return new Container(
                color: CColors.White,
                child: this._buildContent()
            );
        }

        Widget _buildContent() {
            var followArticleList = this.widget.viewModel.followArticleList;
            var followingList = this.widget.viewModel.followingList;
            var hotArticleList = this.widget.viewModel.hotArticleList;
            Widget content;
            if (this.widget.viewModel.followArticlesLoading
                && followArticleList.isEmpty()
                && this.widget.viewModel.followingLoading
                && followingList.isEmpty()
                && hotArticleList.isEmpty()) {
                content = new FollowArticleLoading();
            }
            else if (followArticleList.isEmpty()) {
                var itemCount = followingList.isEmpty()
                    ? hotArticleList.Count
                    : hotArticleList.Count + 1;
                content = new SmartRefresher(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: this.widget.viewModel.hotArticleHasMore,
                    onRefresh: this._onRefresh,
                    child: ListView.builder(
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: itemCount,
                        itemBuilder: (cxt, index) => {
                            if (followingList.isNotEmpty() && index == 0) {
                                return this._buildFollowingList(context: cxt);
                            }

                            var newIndex = followingList.isNotEmpty()
                                ? index - 1
                                : index;
                            return this._buildFollowArticleCard(index: newIndex, false);
                        }
                    )
                );
            }
            else {
                var itemCount = followingList.isEmpty()
                    ? followArticleList.Count
                    : followArticleList.Count + 1;
                content = new SmartRefresher(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: this.widget.viewModel.followArticleHasMore,
                    onRefresh: this._onRefresh,
                    child: ListView.builder(
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: itemCount,
                        itemBuilder: (cxt, index) => {
                            if (followingList.isNotEmpty() && index == 0) {
                                return this._buildFollowingList(context: cxt);
                            }

                            var newIndex = followingList.isNotEmpty()
                                ? index - 1
                                : index;
                            return this._buildFollowArticleCard(index: newIndex);
                        }
                    )
                );
            }

            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(child: content)
            );
        }

        Widget _buildFollowingList(BuildContext context) {
            var followingList = this.widget.viewModel.followingList;
            if (followingList.isEmpty()) {
                return new Container();
            }
            var followButtons = new List<Widget>();
            if (followingList.Count > 5) {
                followingList = followingList.GetRange(0, 5);
            }
            followingList.ForEach(followingUser => {
                var followButton = _buildFollowButton(
                    context: context,
                    user: followingUser,
                    () => this.widget.actionModel.pushToUserDetail(obj: followingUser.id)
                );
                followButtons.Add(item: followButton);
            });
            return new Container(
                height: 162,
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
                            padding: EdgeInsets.only(16, 16, 8, 12),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                children: new List<Widget> {
                                    new Text(
                                        "最近关注",
                                        style: CTextStyle.PMediumBody2
                                    ),
                                    new GestureDetector(
                                        onTap: () => this.widget.actionModel.pushToUserFollowing(obj: this.widget.viewModel.currentUserId),
                                        child: new Container(
                                            color: CColors.Transparent,
                                            child: new Row(
                                                children: new List<Widget> {
                                                    new Text(
                                                        "查看全部",
                                                        style: new TextStyle(
                                                            fontSize: 12,
                                                            fontFamily: "Roboto-Regular",
                                                            color: CColors.TextBody4
                                                        )
                                                    ),
                                                    new Icon(
                                                        icon: Icons.chevron_right,
                                                        size: 24,
                                                        color: Color.fromRGBO(199, 203, 207, 1)
                                                    )
                                                }
                                            )
                                        )
                                    )
                                }
                            )
                        ),
                        new Expanded(
                            child: new Row(
                                children: followButtons
                            )
                        )
                    }
                )
            );
        }

        static Widget _buildFollowButton(BuildContext context, User user, GestureTapCallback onTap) {
            return new GestureDetector(
                onTap: onTap,
                child: new Container(
                    color: CColors.Transparent,
                    width: MediaQuery.of(context: context).size.width / 5.0f,
                    child: new Column(
                        children: new List<Widget> {
                            Avatar.User(
                                user: user,
                                52
                            ),
                            new Container(
                                margin: EdgeInsets.only(top: 6),
                                child: new Text(
                                    user.fullName ?? user.name,
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

        Widget _buildFollowArticleCard(int index, bool isFollow = true) {
            var articleList = isFollow
                ? this.widget.viewModel.followArticleList
                : this.widget.viewModel.hotArticleList;
            var articleId = articleList[index: index];
            if (this.widget.viewModel.blockArticleList.Contains(item: articleId)) {
                return new Container();
            }

            var article = this.widget.viewModel.articleDict[key: articleId];
            if (article.ownerType == OwnerType.user.ToString()) {
                if (this.widget.viewModel.currentUserId == article.userId) {
                    return new Container();
                }
                var user = this.widget.viewModel.userDict[key: article.userId];
                UserType userType = UserType.unFollow;
                if (isFollow) {
                    userType = UserType.me;
                }
                else if (!this.widget.viewModel.isLoggedIn) {
                    userType = UserType.unFollow;
                }
                else {
                    if (this.widget.viewModel.currentUserId == article.userId) {
                        userType = UserType.me;
                    } else if (user.followUserLoading ?? false) {
                        userType = UserType.loading;
                    } else if (this.widget.viewModel.followMap.ContainsKey(key: article.userId)) {
                        userType = UserType.follow;
                    }
                }

                return FollowArticleCard.User(
                    article: article,
                    user: user,
                    this.widget.viewModel.likeMap.ContainsKey(key: articleId),
                    userType: userType,
                    () => this.widget.actionModel.pushToArticleDetail(obj: articleId),
                    () => this._onFollow(article: article, userType: userType),
                    () => this._onLike(article: article),
                    () => this._onComment(article: article),
                    () => this._onShare(article: article),
                    new ObjectKey(value: articleId)
                );
            }

            if (article.ownerType == OwnerType.team.ToString()) {
                var team = this.widget.viewModel.teamDict[key: article.teamId];
                UserType userType = UserType.unFollow;
                if (isFollow) {
                    userType = UserType.me;
                }
                else if (!this.widget.viewModel.isLoggedIn) {
                    userType = UserType.unFollow;
                }
                else {
                    if (this.widget.viewModel.currentUserId == article.teamId) {
                        userType = UserType.me;
                    } else if (team.followTeamLoading ?? false) {
                        userType = UserType.loading;
                    } else if (this.widget.viewModel.followMap.ContainsKey(key: article.teamId)) {
                        userType = UserType.follow;
                    }
                }
                return FollowArticleCard.Team(
                    article: article,
                    team: team,
                    this.widget.viewModel.likeMap.ContainsKey(key: articleId),
                    userType: userType,
                    () => this.widget.actionModel.pushToArticleDetail(obj: articleId),
                    () => this._onFollow(article: article, userType: userType),
                    () => this._onLike(article: article),
                    () => this._onComment(article: article),
                    () => this._onShare(article: article),
                    new ObjectKey(value: articleId)
                );
            }
            return new Container();
        }
    }
}