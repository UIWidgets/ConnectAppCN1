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
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.service;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class RecommendArticleScreenConnector : StatelessWidget {
        public RecommendArticleScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

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
                    isLoggedIn = state.loginState.isLoggedIn,
                    hosttestOffset = state.articleState.recommendArticleIds.Count,
                    currentUserId = state.loginState.loginInfo.userId ?? "",
                    showFirstEgg = state.eggState.showFirst
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
                        startFetchArticles = () => dispatcher.dispatch(new StartFetchArticlesAction()),
                        fetchArticles = (userId, offset) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchArticles(userId: userId, offset: offset)),
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

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            this._hasBeenLoadedData = false;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchArticles();
                this.widget.actionModel.fetchArticles(arg1: this.widget.viewModel.currentUserId, arg2: initOffset).Then(() => {
                    if (this._hasBeenLoadedData) {
                        return;
                    }

                    this._hasBeenLoadedData = true;
                    this.setState(() => { });
                });
            });
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
            else if (0 == recommendArticleIds.Count) {
                content = new Container(
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context)),
                    child: new BlankView(
                        "哎呀，暂无推荐文章",
                        "image/default-article",
                        true,
                        () => {
                            this.widget.actionModel.startFetchArticles();
                            this.widget.actionModel.fetchArticles(arg1: this.widget.viewModel.currentUserId, arg2: initOffset);
                        }
                    )
                );
            }
            else {
                content = new SmartRefresher(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: this.widget.viewModel.hottestHasMore,
                    onRefresh: this._onRefresh,
                    hasBottomMargin: true,
                    child: ListView.builder(
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: recommendArticleIds.Count,
                        itemBuilder: (cxt, index) => {
                            var articleId = recommendArticleIds[index: index];
                            if (this.widget.viewModel.blockArticleList.Contains(item: articleId)) {
                                return new Container();
                            }

                            if (!this.widget.viewModel.articleDict.ContainsKey(key: articleId)) {
                                return new Container();
                            }

                            if (!this.widget.viewModel.hottestHasMore && recommendArticleIds.Count > 0 &&
                                index + 1 == recommendArticleIds.Count) {
                                return new EndView(hasBottomMargin: true);
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
                                        string imageUrl = CImageUtils.SizeTo200ImageUrl(article.thumbnail.url);
                                        this.widget.actionModel.shareToWechat(arg1: type, arg2: article.title,
                                                arg3: article.subTitle, arg4: linkUrl, arg5: imageUrl)
                                            .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                                            .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                                    }
                                ),
                                fullName: fullName,
                                key: new ObjectKey(value: article.id)
                            );
                        }
                    )
                );
            }

            return new CustomScrollbar(child: content);
        }

        void _onRefresh(bool up) {
            this.offset = up ? initOffset : this.widget.viewModel.hosttestOffset;
            this.widget.actionModel.fetchArticles(arg1: this.widget.viewModel.currentUserId, arg2: this.offset)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }
    }
}