using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ArticlesScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenViewModel>(
                converter: state => new ArticlesScreenViewModel {
                    articlesLoading = state.articleState.articlesLoading,
                    articleList = state.articleState.articleList,
                    articleDict = state.articleState.articleDict,
                    articleTotal = state.articleState.articleTotal,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ArticlesScreenActionModel {
                        pushToSearch = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Search
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
                        startFetchArticles = () => dispatcher.dispatch(new StartFetchArticlesAction()),
                        fetchArticles = pageNumber => dispatcher.dispatch<IPromise>(Actions.fetchArticles(pageNumber))
                    };
                    return new ArticlesScreen(viewModel, actionModel);
                });
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


    public class _ArticlesScreenState : State<ArticlesScreen> {
        private const int firstPageNumber = 0;
        private static readonly float headerHeight = CustomNavigationBar.height;
        private float _offsetY;
        private int pageNumber = firstPageNumber;
        private RefreshController _refreshController;

//        protected override bool wantKeepAlive
//        {
//            get => false;
//        }
        public override void initState() {
            base.initState();
            _refreshController = new RefreshController();
            _offsetY = 0;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchArticles();
                widget.actionModel.fetchArticles(firstPageNumber);
            });
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.BgGrey,
                child: new Column(
                    children: new List<Widget> {
                        _buildNavigationBar(context),
                        new Flexible(
                            child: _buildArticleList(context)
                        )
                    }
                )
            );
        }

        private Widget _buildNavigationBar(BuildContext context) {
            return new CustomNavigationBar(
                new Text("文章", style: new TextStyle(
                    height: 1.25f,
                    fontSize: 32 / headerHeight * (headerHeight - _offsetY),
                    fontFamily: "Roboto-Bold",
                    color: CColors.TextTitle
                )),
                new List<Widget> {
                    new CustomButton(
                        onPressed: () => widget.actionModel.pushToSearch(),
                        child: new Icon(
                            Icons.search,
                            size: 28,
                            color: Color.fromRGBO(181, 181, 181, 1)
                        )
                    )
                },
                CColors.White,
                _offsetY
            );
        }

        private Widget _buildArticleList(BuildContext context) {
            Widget content = new Container();

            if (widget.viewModel.articlesLoading && widget.viewModel.articleList.isEmpty())
                content = ListView.builder(
                    itemCount: 4,
                    itemBuilder: (cxt, index) => new ArticleLoading()
                );
            else if (widget.viewModel.articleList.Count <= 0)
                content = new BlankView("暂无文章");
            else
                content = new SmartRefresher(
                    controller: _refreshController,
                    enablePullDown: true,
                    enablePullUp: widget.viewModel.articleList.Count < widget.viewModel.articleTotal,
                    headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                    footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                    onRefresh: onRefresh,
                    child: ListView.builder(
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: widget.viewModel.articleList.Count,
                        itemBuilder: (cxt, index) => {
                            var articleId = widget.viewModel.articleList[index];
                            var article = widget.viewModel.articleDict[articleId];
                            if (article.ownerType == OwnerType.user.ToString()) {
                                var _user = new User();
                                if (widget.viewModel.userDict.ContainsKey(article.userId))
                                    _user = widget.viewModel.userDict[article.userId];
                                return ArticleCard.User(article,
                                    () => widget.actionModel.pushToArticleDetail(articleId),
                                    () => {
                                        ActionSheetUtils.showModalActionSheet(new ActionSheet(
                                            items: new List<ActionSheetItem> {
                                                new ActionSheetItem(
                                                    "举报",
                                                    ActionType.destructive,
                                                    () => widget.actionModel.pushToReport(articleId, ReportType.article)
                                                ),
                                                new ActionSheetItem("取消", ActionType.cancel)
                                            }
                                        ));
                                    },
                                    new ObjectKey(article.id),
                                    _user
                                );
                            }
                            var _team = new Team();
                            if (widget.viewModel.teamDict.ContainsKey(article.teamId))
                                _team = widget.viewModel.teamDict[article.teamId];
                            return ArticleCard.Team(article,
                                () => widget.actionModel.pushToArticleDetail(articleId),
                                () => {
                                    ActionSheetUtils.showModalActionSheet(new ActionSheet(
                                        items: new List<ActionSheetItem> {
                                            new ActionSheetItem(
                                                "举报",
                                                ActionType.destructive,
                                                () => widget.actionModel.pushToReport(articleId, ReportType.article)
                                            ),
                                            new ActionSheetItem("取消", ActionType.cancel)
                                        }
                                    ));
                                },
                                new ObjectKey(article.id),
                                _team
                            );
                        }
                    )
                );

            return new NotificationListener<ScrollNotification>(
                onNotification: _onNotification,
                child: new Container(
                    margin: EdgeInsets.only(bottom: 49),
                    child: content
                )
            );
        }

        private void onRefresh(bool up) {
            if (up)
                pageNumber = firstPageNumber;
            else
                pageNumber++;
            widget.actionModel.fetchArticles(pageNumber)
                .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
        }

        private bool _onNotification(ScrollNotification notification) {
//            var pixels = notification.metrics.pixels;
//            Debug.Log($"pixels: {pixels}");
//            if (pixels >= 0) {
//                if (pixels <= headerHeight && _offsetY != pixels / 2.8f) setState(() => { _offsetY = pixels / 2.8f; });
//            }
//            else {
//                if (_offsetY != 0) setState(() => { _offsetY = 0; });
//            }
            return true;
        }
    }
}