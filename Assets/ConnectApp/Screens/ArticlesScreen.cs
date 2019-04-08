using System;
using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.Screen;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    
    public class ArticlesScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenModel>(
                pure: true,
                converter: (state) => new ArticlesScreenModel {
                    articlesLoading = state.articleState.articlesLoading,
                    articleList = state.articleState.articleList,
                    articleDict = state.articleState.articleDict,
                    articleTotal = state.articleState.articleTotal
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new ArticlesScreen(
                        viewModel,
                        () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Search
                        }),
                        id => dispatcher.dispatch(
                            new MainNavigatorPushToArticleDetailAction {
                                articleId = id
                            }
                        ),
                        pageNumber =>
                            dispatcher.dispatch<IPromise<FetchArticlesResponse>>(Actions.fetchArticles(pageNumber))
                    );
                });
        }
    }
    
    public class ArticlesScreen : StatefulWidget {
        
        public override State createState() {
            return new _ArticlesScreenState();
        }

        public ArticlesScreen(
            ArticlesScreenModel screenModel = null,
            Action pushToSearch = null,    
            Action<string> pushToArticleDetail = null,    
            Func<int, IPromise<FetchArticlesResponse>> fetchArticles = null,    
            Key key = null
        ) : base(key)
        {
            this.screenModel = screenModel;
            this.pushToSearch = pushToSearch;
            this.pushToArticleDetail = pushToArticleDetail;
            this.fetchArticles = fetchArticles;
        }

        public ArticlesScreenModel screenModel;
        public Action pushToSearch;
        public Action<string> pushToArticleDetail;
        public Func<int, IPromise<FetchArticlesResponse>> fetchArticles;
    }


    public class _ArticlesScreenState : State<ArticlesScreen>
    {
        private const int firstPageNumber = 1;
        private const float headerHeight = 140;
        private float _offsetY;
        private int pageNumber = firstPageNumber;
        private RefreshController _refreshController;

        public override void initState() {
            base.initState();
            _refreshController = new RefreshController();
            _offsetY = 0;
            
            widget.fetchArticles(firstPageNumber);
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
                        onPressed: () => widget.pushToSearch(),
                        child: new Icon(
                            Icons.search,
                            size: 28,
                            color: Color.fromRGBO(181, 181, 181, 1))
                    )
                },
                CColors.White,
                _offsetY);
        }

        private Widget _buildArticleList(BuildContext context)
        {
            object content = new Container();

            if (widget.screenModel.articlesLoading && widget.screenModel.articleList.isEmpty())
            {
                content = ListView.builder(
                    itemCount: 4,
                    itemBuilder: (cxt, index) => new ArticleLoading()
                );
            } else if (widget.screenModel.articleList.Count <= 0)
            {
                content = new BlankView("暂无文章");
            }
            else
            {
                content = new SmartRefresher(
                    controller: _refreshController,
                    enablePullDown: true,
                    enablePullUp: widget.screenModel.articleList.Count < widget.screenModel.articleTotal,
                    headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                    footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                    onRefresh: onRefresh,
                    child: ListView.builder(
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: widget.screenModel.articleList.Count,
                        itemBuilder: (cxt, index) =>
                        {
                            var articleId = widget.screenModel.articleList[index];
                            var article = widget.screenModel.articleDict[articleId];
                            return new ArticleCard(
                                article,
                                () => widget.pushToArticleDetail(articleId),
                                () =>
                                {
                                    ActionSheetUtils.showModalActionSheet(new ActionSheet(
                                        items: new List<ActionSheetItem>
                                        {
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
            
            return new NotificationListener<ScrollNotification>(
                onNotification: _onNotification,
                child: new Container(
                    margin: EdgeInsets.only(bottom: 49),
                    child: (Widget)content
                )
            );
        }

        private void onRefresh(bool up) {
            if (up)
                pageNumber = firstPageNumber;
            else
                pageNumber++;
            widget.fetchArticles(firstPageNumber)
                .Then(_ => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
        }

        private bool _onNotification(ScrollNotification notification) {
            var pixels = notification.metrics.pixels;
            if (pixels >= 0) {
                if (pixels <= headerHeight) setState(() => { _offsetY = pixels / 2.0f; });
            }
            else {
                if (_offsetY != 0) setState(() => { _offsetY = 0; });
            }

            return true;
        }
    }
}