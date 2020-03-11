using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Components.Swiper;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class RecommendArticleScreenConnector : StatelessWidget {
        public RecommendArticleScreenConnector(
            int selectedIndex,
            Key key = null
        ) : base(key: key) {
            this.selectedIndex = selectedIndex;
        }

        readonly int selectedIndex;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenViewModel>(
                converter: state => new ArticlesScreenViewModel {
                    articlesLoading = state.articleState.articlesLoading,
                    recommendArticleIds = state.articleState.recommendArticleIds,
                    articleDict = state.articleState.articleDict,
                    blockArticleList = state.articleState.blockArticleList,
                    hottestHasMore = state.articleState.hottestHasMore,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict,
                    followMap = state.followState.followDict.ContainsKey(state.loginState.loginInfo.userId ?? "")
                        ? state.followState.followDict[state.loginState.loginInfo.userId ?? ""]
                        : new Dictionary<string, bool>(),
                    favoriteTagDict = state.favoriteState.favoriteTagDict,
                    favoriteTagArticleDict = state.favoriteState.favoriteTagArticleDict,
                    rankDict = state.leaderBoardState.rankDict,
                    homeSliderIds = state.articleState.homeSliderIds,
                    homeTopCollectionIds = state.articleState.homeTopCollectionIds,
                    homeCollectionIds = state.articleState.homeCollectionIds,
                    homeBloggerIds = state.articleState.homeBloggerIds,
                    dailySelectionId = state.articleState.dailySelectionId,
                    isLoggedIn = state.loginState.isLoggedIn,
                    hosttestOffset = state.articleState.recommendArticleIds.Count,
                    currentUserId = state.loginState.loginInfo.userId ?? "",
                    leaderBoardUpdatedTime = state.articleState.leaderBoardUpdatedTime,
                    selectedIndex = this.selectedIndex,
                    hasNewArticle = state.articleState.recommendHasNewArticle
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
                        openUrl = JPushPlugin.openUrlScheme,
                        pushToLeaderBoard = initIndex => dispatcher.dispatch(new MainNavigatorPushToLeaderBoardAction {
                            initIndex = initIndex
                        }),
                        pushToLeaderBoardDetail = id =>
                            dispatcher.dispatch(new MainNavigatorPushToLeaderBoardDetailAction {id = id}
                            ),
                        pushToHomeEvent = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.HomeEvent
                        }),
                        pushToBlogger = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Blogger
                        }),
                        pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                            userId = userId
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
                        startFetchArticles = () => dispatcher.dispatch(new StartFetchArticlesAction()),
                        fetchArticles = (userId, offset) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchArticles(userId: userId, offset: offset)),
                        startFollowUser = userId =>
                            dispatcher.dispatch(new StartFollowUserAction {followUserId = userId}),
                        followUser = userId =>
                            dispatcher.dispatch<IPromise>(Actions.fetchFollowUser(followUserId: userId)),
                        startUnFollowUser = userId =>
                            dispatcher.dispatch(new StartUnFollowUserAction {unFollowUserId = userId}),
                        unFollowUser = userId => dispatcher.dispatch<IPromise>(Actions.fetchUnFollowUser(userId)),
                        shareToWechat = (type, title, description, linkUrl, imageUrl) => dispatcher.dispatch<IPromise>(
                            Actions.shareToWechat(type, title, description, linkUrl, imageUrl))
                    };
                    return new RecommendArticleScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class RecommendArticleScreen : StatefulWidget {
        public RecommendArticleScreen(
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
            return new _RecommendArticleScreenState();
        }
    }

    class _RecommendArticleScreenState : AutomaticKeepAliveClientMixin<RecommendArticleScreen> {
        const int initOffset = 0;
        int offset = initOffset;
        RefreshController _refreshController;
        bool _hasBeenLoadedData;
        string _articleTabSubId;


        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            this._hasBeenLoadedData = false;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchArticles();
                this.widget.actionModel.fetchArticles(arg1: this.widget.viewModel.currentUserId, arg2: initOffset).Then(
                    () => {
                        if (this._hasBeenLoadedData) {
                            return;
                        }

                        this._hasBeenLoadedData = true;
                        this.setState(() => { });
                    });
            });
            this._articleTabSubId = EventBus.subscribe(sName: EventBusConstant.article_tab, args => {
                if (this.widget.viewModel.selectedIndex == 1) {
                    this._refreshController.sendBack(true, mode: RefreshStatus.refreshing);
                    this._refreshController.animateTo(0.0f, TimeSpan.FromMilliseconds(300), curve: Curves.linear);
                }
            });
        }

        public override void dispose() {
            EventBus.unSubscribe(sName: EventBusConstant.article_tab, id: this._articleTabSubId);
            base.dispose();
        }

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            return new Container(
                color: CColors.Background,
                child: this._buildArticleList(context: context)
            );
        }

        Widget _buildArticleList(BuildContext context) {
            Widget content;
            var recommendArticleIds = this.widget.viewModel.recommendArticleIds;
            if (!this._hasBeenLoadedData || this.widget.viewModel.articlesLoading && recommendArticleIds.isEmpty()) {
                content = ListView.builder(
                    physics: new NeverScrollableScrollPhysics(),
                    itemCount: 6,
                    itemBuilder: (cxt, index) => new ArticleLoading()
                );
            }
            else if (recommendArticleIds.isEmpty()) {
                content = new Container(
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context)),
                    child: new BlankView(
                        "哎呀，暂无推荐文章",
                        "image/default-article",
                        true,
                        () => {
                            this.widget.actionModel.startFetchArticles();
                            this.widget.actionModel.fetchArticles(arg1: this.widget.viewModel.currentUserId,
                                arg2: initOffset);
                        }
                    )
                );
            }
            else {
                var items = this._buildItems(recommendArticleIds);
                var enablePullUp = this.widget.viewModel.hottestHasMore;
                content = new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    hasBottomMargin: true,
                    itemCount: items.Count,
                    itemBuilder: (cxt, index) => items[index],
                    headerWidget: new Column(
                        children: new List<Widget> {
                            this._buildSwiper(),
                            new KingKongView(
                                leaderBoardUpdatedTime: this.widget.viewModel.leaderBoardUpdatedTime,
                                type => {
                                    if (type == KingKongType.dailyCollection) {
                                        var articleId = this.widget.viewModel.dailySelectionId;
                                        this.widget.actionModel.pushToArticleDetail(obj: articleId);
                                    }

                                    if (type == KingKongType.leaderBoard) {
                                        this.widget.actionModel.pushToLeaderBoard(0);
                                    }

                                    if (type == KingKongType.activity) {
                                        this.widget.actionModel.pushToHomeEvent();
                                    }

                                    if (type == KingKongType.blogger) {
                                        this.widget.actionModel.pushToBlogger();
                                    }
                                }
                            )
                        }
                    ),
                    footerWidget: enablePullUp ? null : new EndView(hasBottomMargin: true),
                    hasScrollBar: false
                );
            }

            return new CustomScrollbar(child: content);
        }

        Widget _buildSwiper() {
            var homeSliderIds = this.widget.viewModel.homeSliderIds;
            if (homeSliderIds.isNullOrEmpty()) {
                return new Container();
            }

            Widget swiperContent;
            if (homeSliderIds.Count == 1) {
                var homeSliderId = homeSliderIds[0];
                var imageUrl = this.widget.viewModel.rankDict.ContainsKey(key: homeSliderId)
                    ? this.widget.viewModel.rankDict[key: homeSliderId].image
                    : "";
                swiperContent = new GestureDetector(
                    onTap: () => {
                        var redirectURL = this.widget.viewModel.rankDict.ContainsKey(key: homeSliderId)
                            ? this.widget.viewModel.rankDict[key: homeSliderId].redirectURL
                            : "";
                        if (redirectURL.isNotEmpty()) {
                            this.widget.actionModel.openUrl(obj: redirectURL);
                        }
                    },
                    child: new PlaceholderImage(
                        imageUrl: imageUrl,
                        fit: BoxFit.fill,
                        useCachedNetworkImage: true,
                        color: CColorUtils.GetSpecificDarkColorFromId(id: homeSliderId)
                    )
                );
            }
            else {
                swiperContent = new Swiper(
                    (cxt, index) => {
                        var homeSliderId = homeSliderIds[index: index];
                        var imageUrl = this.widget.viewModel.rankDict.ContainsKey(key: homeSliderId)
                            ? this.widget.viewModel.rankDict[key: homeSliderId].image
                            : "";
                        return new PlaceholderImage(
                            CImageUtils.SizeToScreenImageUrl(imageUrl: imageUrl),
                            fit: BoxFit.fill,
                            useCachedNetworkImage: true,
                            color: CColorUtils.GetSpecificDarkColorFromId(id: homeSliderId)
                        );
                    },
                    itemCount: homeSliderIds.Count,
                    autoplay: true,
                    onTap: index => {
                        var homeSliderId = homeSliderIds[index: index];
                        var redirectURL = this.widget.viewModel.rankDict.ContainsKey(key: homeSliderId)
                            ? this.widget.viewModel.rankDict[key: homeSliderId].redirectURL
                            : "";
                        if (redirectURL.isNotEmpty()) {
                            this.widget.actionModel.openUrl(obj: redirectURL);
                        }
                    },
                    pagination: new SwiperPagination(margin: EdgeInsets.only(bottom: 5))
                );
            }

            return new Container(
                padding: EdgeInsets.only(top: 8, left: 16, right: 16),
                decoration: new BoxDecoration(
                    borderRadius: BorderRadius.all(8),
                    color: CColors.White
                ),
                child: new AspectRatio(
                    aspectRatio: 3,
                    child: new ClipRRect(
                        borderRadius: BorderRadius.all(8),
                        child: swiperContent
                    )
                )
            );
        }

        List<Widget> _buildItems(List<string> articleIds) {
            var items = new List<Widget>();
            articleIds.ForEach(articleId => { items.Add(this._buildArticleCard(articleId)); });
            if (items.Count >= 6) {
                items.Insert(6, this._buildRecommendLeaderBoard());
                items.Insert(3, this._buildRecommendBlogger());
            }

            if (!this.widget.viewModel.hasNewArticle) {
                items.Insert(0, this._buildLeaderBoard());
            }

            return items;
        }


        Widget _buildArticleCard(string articleId) {
            if (this.widget.viewModel.blockArticleList.Contains(item: articleId)) {
                return new Container();
            }

            if (!this.widget.viewModel.articleDict.ContainsKey(key: articleId)) {
                return new Container();
            }

            var article = this.widget.viewModel.articleDict[key: articleId];
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
                    this.widget.actionModel.pushToArticleDetail(obj: articleId);
                    AnalyticsManager.ClickEnterArticleDetail("Home_Article", articleId: article.id,
                        articleTitle: article.title);
                },
                () => ShareManager.showArticleShareView(
                    this.widget.viewModel.currentUserId != userId,
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

        Widget _buildRecommendBlogger() {
            return new RecommendBlogger(
                bloggerIds: this.widget.viewModel.homeBloggerIds,
                rankDict: this.widget.viewModel.rankDict,
                userDict: this.widget.viewModel.userDict,
                followMap: this.widget.viewModel.followMap,
                isLoggedIn: this.widget.viewModel.isLoggedIn,
                userId => {
                    this.widget.actionModel.startFollowUser(obj: userId);
                    this.widget.actionModel.followUser(arg: userId);
                },
                userId => {
                    this.widget.actionModel.startUnFollowUser(obj: userId);
                    this.widget.actionModel.unFollowUser(arg: userId);
                },
                () => this.widget.actionModel.pushToLogin(),
                () => this.widget.actionModel.pushToLeaderBoard(2),
                () => this.widget.actionModel.pushToBlogger(),
                userId => this.widget.actionModel.pushToUserDetail(obj: userId)
            );
        }

        Widget _buildRecommendLeaderBoard() {
            return new RecommendLeaderBoard(
                data: this.widget.viewModel.homeCollectionIds,
                rankDict: this.widget.viewModel.rankDict,
                favoriteTagDict: this.widget.viewModel.favoriteTagDict,
                favoriteTagArticleDict: this.widget.viewModel.favoriteTagArticleDict,
                () => this.widget.actionModel.pushToLeaderBoard(0),
                collectionId => this.widget.actionModel.pushToLeaderBoardDetail(obj: collectionId)
            );
        }

        Widget _buildLeaderBoard() {
            return new LeaderBoard(
                data: this.widget.viewModel.homeTopCollectionIds,
                rankDict: this.widget.viewModel.rankDict,
                favoriteTagDict: this.widget.viewModel.favoriteTagDict,
                () => this.widget.actionModel.pushToLeaderBoard(0),
                collectionId => this.widget.actionModel.pushToLeaderBoardDetail(obj: collectionId)
            );
        }

        Widget _buildRefreshDivider() {
            return new RefreshDivider(
                () => {
                    this._refreshController.sendBack(true, mode: RefreshStatus.refreshing);
                    this._refreshController.animateTo(0.0f, TimeSpan.FromMilliseconds(300), curve: Curves.linear);
                }
            );
        }

        void _onRefresh(bool up) {
            this.offset = up ? initOffset : this.widget.viewModel.hosttestOffset;
            this.widget.actionModel.fetchArticles(arg1: this.widget.viewModel.currentUserId, arg2: this.offset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }
    }
}