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
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ArticlesScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenViewModel>(
                converter: state => new ArticlesScreenViewModel {
                    articlesLoading = state.articleState.articlesLoading,
                    articleList = state.articleState.articleList,
                    articleDict = state.articleState.articleDict,
                    blockArticleList = state.articleState.blockArticleList,
                    hottestHasMore = state.articleState.hottestHasMore,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict,
                    isLoggedIn = state.loginState.isLoggedIn,
                    hosttestOffset = state.articleState.articleList.Count
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ArticlesScreenActionModel {
                        pushToSearch = () => {
                            dispatcher.dispatch(new MainNavigatorPushToAction {
                                routeName = MainNavigatorRoutes.Search
                            });
                            AnalyticsManager.ClickEnterSearch("Home_Article");
                        },
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
                        fetchArticles = offset => dispatcher.dispatch<IPromise>(Actions.fetchArticles(offset)),
                        fetchReviewUrl = () => dispatcher.dispatch<IPromise>(Actions.fetchReviewUrl())
                    };
                    return new ArticlesScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class ArticlesScreen : StatefulWidget {
        public override State createState() {
            return new _ArticlesScreenState();
        }

        public ArticlesScreen(
            ArticlesScreenViewModel viewModel = null,
            ArticlesScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ArticlesScreenViewModel viewModel;
        public readonly ArticlesScreenActionModel actionModel;
    }


    public class _ArticlesScreenState : AutomaticKeepAliveClientMixin<ArticlesScreen> {
        const int initOffset = 0;
        int offset = initOffset;
        RefreshController _refreshController;
        TextStyle titleStyle;
        const float maxNavBarHeight = 96;
        const float minNavBarHeight = 44;
        float navBarHeight;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            this.navBarHeight = maxNavBarHeight;
            this.titleStyle = CTextStyle.H2;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchArticles();
                this.widget.actionModel.fetchArticles(initOffset);
                this.widget.actionModel.fetchReviewUrl();
            });
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            return new Container(
                color: CColors.BgGrey,
                child: new Column(
                    children: new List<Widget> {
                        this._buildNavigationBar(),
                        new Flexible(
                            child: this._buildArticleList()
                        )
                    }
                )
            );
        }

        Widget _buildNavigationBar() {
            return new AnimatedContainer(
                height: this.navBarHeight,
                color: CColors.White,
                duration: new TimeSpan(0, 0, 0, 0, 0),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.end,
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(16, bottom: 8),
                            child: new AnimatedDefaultTextStyle(
                                child: new Text("文章"),
                                style: this.titleStyle,
                                duration: new TimeSpan(0, 0, 0, 0, 100)
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(16, 8, 16, 8),
                            onPressed: () => this.widget.actionModel.pushToSearch(),
                            child: new Icon(
                                Icons.search,
                                size: 28,
                                color: CColors.BrownGrey
                            )
                        )
                    }
                )
            );
        }

        Widget _buildArticleList() {
            Widget content = new Container();

            if (this.widget.viewModel.articlesLoading && this.widget.viewModel.articleList.isEmpty()) {
                content = ListView.builder(
                    itemCount: 4,
                    itemBuilder: (cxt, index) => new ArticleLoading()
                );
            }
            else if (this.widget.viewModel.articleList.Count <= 0) {
                content = new BlankView("暂无文章", true, () => {
                    this.widget.actionModel.startFetchArticles();
                    this.widget.actionModel.fetchArticles(initOffset);
                });
            }
            else {
                content = new SmartRefresher(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: this.widget.viewModel.hottestHasMore,
                    onRefresh: this._onRefresh,
                    child: ListView.builder(
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: this.widget.viewModel.articleList.Count,
                        itemBuilder: (cxt, index) => {
                            var articleId = this.widget.viewModel.articleList[index];
                            if (this.widget.viewModel.blockArticleList.Contains(articleId)) {
                                return new Container();
                            }

                            var article = this.widget.viewModel.articleDict[articleId];
                            var fullName = "";
                            if (article.ownerType == OwnerType.user.ToString()) {
                                if (this.widget.viewModel.userDict.ContainsKey(article.userId)) {
                                    fullName = this.widget.viewModel.userDict[article.userId].fullName;
                                }
                            }

                            if (article.ownerType == OwnerType.team.ToString()) {
                                if (this.widget.viewModel.teamDict.ContainsKey(article.teamId)) {
                                    fullName = this.widget.viewModel.teamDict[article.teamId].name;
                                }
                            }

                            return new ArticleCard(
                                article,
                                () => {
                                    this.widget.actionModel.pushToArticleDetail(articleId);
                                    AnalyticsManager.ClickEnterArticleDetail("Home_Article", article.id, article.title);
                                },
                                () => ReportManager.showReportView(this.widget.viewModel.isLoggedIn,
                                    articleId,
                                    ReportType.article, this.widget.actionModel.pushToLogin,
                                    this.widget.actionModel.pushToReport, this.widget.actionModel.pushToBlock
                                ),
                                fullName,
                                new ObjectKey(article.id)
                            );
                        }
                    )
                );
            }

            return new NotificationListener<ScrollNotification>(
                onNotification: this._onNotification,
                child: new Container(
                    color: CColors.Background,
                    child: new CustomScrollbar(content)
                )
            );
        }

        void _onRefresh(bool up) {
            this.offset = up ? initOffset : this.widget.viewModel.hosttestOffset;
            this.widget.actionModel.fetchArticles(this.offset)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }

        bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (pixels > 0 && pixels <= 52) {
                    this.titleStyle = CTextStyle.H5;
                    this.navBarHeight = maxNavBarHeight - pixels;
                    this.setState(() => { });
                }
                else if (pixels <= 0) {
                    if (this.navBarHeight <= maxNavBarHeight) {
                        this.titleStyle = CTextStyle.H2;
                        this.navBarHeight = maxNavBarHeight;
                        this.setState(() => { });
                    }
                }
                else if (pixels > 52) {
                    if (!(this.navBarHeight <= minNavBarHeight)) {
                        this.titleStyle = CTextStyle.H5;
                        this.navBarHeight = minNavBarHeight;
                        this.setState(() => { });
                    }
                }
            });
            return true;
        }
    }
}