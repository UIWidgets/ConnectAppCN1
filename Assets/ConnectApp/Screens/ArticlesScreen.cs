using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ArticleScreen : StatefulWidget {
        public override State createState() {
            return new _ArticleScreenState();
        }

        public ArticleScreen(
            Key key = null
        ) : base(key) {
        }
    }


    public class _ArticleScreenState : State<ArticleScreen> {
        private const float headerHeight = 140;
        private float _offsetY;
        private int _pageNumber = 1;
        private RefreshController _refreshController;
        private string _eventBusId;

        public override void initState() {
            base.initState();
            _refreshController = new RefreshController();
            _offsetY = 0;
            _eventBusId = EventBus.subscribe(EventBusConstant.article_refresh, args => {
                _refreshController.sendBack((bool)args[0], (int)args[1]);
            });
            _pageNumber = StoreProvider.store.state.articleState.pageNumber;
            if (StoreProvider.store.state.articleState.articleList.Count == 0)
                StoreProvider.store.Dispatch(new FetchArticlesAction {pageNumber = _pageNumber, isRefresh = false});
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.article_refresh, _eventBusId);
            base.dispose();
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
                        onPressed: () => StoreProvider.store.Dispatch(new MainNavigatorPushToAction
                            {routeName = MainNavigatorRoutes.Search}),
                        child: new Icon(
                            Icons.search,
                            size: 28,
                            color: Color.fromRGBO(181, 181, 181, 1))
                    )
                },
                CColors.White,
                _offsetY);
        }

        private Widget _buildArticleList(BuildContext context) {
            return new NotificationListener<ScrollNotification>(
                onNotification: _onNotification,
                child: new Container(
                    margin: EdgeInsets.only(bottom: 49),
                    child: new StoreConnector<AppState, Dictionary<string, object>>(
                        converter: (state, dispatch) => new Dictionary<string, object> {
                            {"articlesLoading", state.articleState.articlesLoading},
                            {"articleList", state.articleState.articleList},
                            {"articleDict", state.articleState.articleDict},
                            {"articleTotal", state.articleState.articleTotal}
                        },
                        builder: (context1, viewModel) => {
                            var articlesLoading = (bool) viewModel["articlesLoading"];
                            var articleList = (List<string>) viewModel["articleList"];
                            var articleDict = (Dictionary<string, Article>) viewModel["articleDict"];
                            var articleTotal = (int) viewModel["articleTotal"];
                            if (articlesLoading && articleList.isEmpty())
                                return ListView.builder(
                                    itemCount: 4,
                                    itemBuilder: (cxt, index) => new ArticleLoading()
                                );
                            if (articleList.Count < 0)
                                return new BlankView("暂无文章");
                            return new SmartRefresher(
                                controller: _refreshController,
                                enablePullDown: true,
                                enablePullUp: articleList.Count < articleTotal,
                                headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                                footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                                onRefresh: _onRefresh,
                                child: ListView.builder(
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    itemCount: articleList.Count,
                                    itemBuilder: (cxt, index) => {
                                        var articleId = articleList[index];
                                        var article = articleDict[articleId];
                                        return new ArticleCard(
                                            article,
                                            () => {
                                                StoreProvider.store.Dispatch(new MainNavigatorPushToArticleDetailAction
                                                    {articleId = articleId});
                                            },
                                            () => {
                                                ActionSheetUtils.showModalActionSheet(new ActionSheet(
                                                    items: new List<ActionSheetItem> {
                                                        new ActionSheetItem("举报", ActionType.destructive, () => { }),
                                                        new ActionSheetItem("取消", ActionType.cancel)
                                                    }
                                                ));
                                            },
                                            new ObjectKey(article.id)
                                        );
                                    }
                                )
                            );
                        }
                    )
                )
            );
        }

        private void _onRefresh(bool up) {
            if (up) {
                _pageNumber = 1;
            } else {
                _pageNumber++;
            }
            StoreProvider.store.Dispatch(new FetchArticlesAction {pageNumber = _pageNumber, isRefresh = true});
        }

        private bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (pixels >= 0) {
                if (pixels <= headerHeight) setState(() => { _offsetY = pixels / 2.8f; });
            }
            else {
                if (_offsetY != 0) setState(() => { _offsetY = 0; });
            }

            return true;
        }
    }
}