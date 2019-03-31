using System.Collections.Generic;
using System.Linq;
using ConnectApp.api;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.components.refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;
using RefreshController = ConnectApp.components.pull_to_refresh.RefreshController;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

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
        private int pageNumber = 1;
        RefreshController _refreshController;
        
        public override void initState() {
            base.initState();
            _refreshController = new RefreshController();
            _offsetY = 0;
            if (StoreProvider.store.state.articleState.articleList.Count == 0)
                StoreProvider.store.Dispatch(new FetchArticlesAction {pageNumber = pageNumber});
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
                        onPressed: () => StoreProvider.store.Dispatch(new MainNavigatorPushToAction {RouteName = MainNavigatorRoutes.Search}),
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
                            if (articlesLoading&&articleList.isEmpty())
                                return ListView.builder(
                                    itemCount: 4,
                                    itemBuilder: (cxt, index) => new ArticleLoading()
                                );
                            var smartRefreshPage = new SmartRefresher(
                                controller: _refreshController,
                                enablePullDown: true,
                                enablePullUp: articleList.Count < articleTotal,
                                headerBuilder: (cxt,mode) =>
                                    new SmartRefreshHeader(mode), 
                                footerBuilder: (cxt,mode) =>
                                    new SmartRefreshHeader(mode),
                                onRefresh: onRefresh,
                                child:ListView.builder(
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    itemCount: articleList.Count,
                                    itemBuilder: (cxt, index) => {
                                        var articleId = articleList[index];
                                        var article = articleDict[articleId];
                                        return new ArticleCard(
                                            article,
                                            () => {
                                                StoreProvider.store.Dispatch(new MainNavigatorPushToArticleDetailAction {ArticleId = articleId});
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
                            return smartRefreshPage;
                        }
                    )
                )
            );
        }

        private void onRefresh(bool up)
        {
            if (up)
            {
                pageNumber = 1;
                StoreProvider.store.Dispatch(new FetchArticlesAction() {pageNumber = pageNumber});
                
                ArticleApi.FetchArticles(pageNumber)
                    .Then(articlesResponse => {
                        var newArticleList = new List<string>();
                        var newArticleDict = new Dictionary<string, Article>();
                        articlesResponse.items.ForEach(item => {
                            newArticleList.Add(item.id);
                            newArticleDict.Add(item.id, item);
                        });
                        StoreProvider.store.Dispatch(new UserMapAction {userMap = articlesResponse.userMap});
                        StoreProvider.store.Dispatch(new TeamMapAction {teamMap = articlesResponse.teamMap});
                        StoreProvider.store.Dispatch(new FetchArticleSuccessAction
                            {articleDict = newArticleDict, articleList = newArticleList, total = articlesResponse.total});
                        _refreshController.sendBack(true, RefreshStatus.completed);
                    })
                    .Catch(error =>
                    {
                        _refreshController.sendBack(true, RefreshStatus.failed);

                    });
            }
            else
            {
                pageNumber++;
                ArticleApi.FetchArticles(pageNumber)
                    .Then(articlesResponse => {
                        if (articlesResponse.items.Count != 0) {
                            var newArticleList = StoreProvider.store.state.articleState.articleList;
                            var newArticleDict = StoreProvider.store.state.articleState.articleDict;
                            articlesResponse.items.ForEach(item => {
                                if (!newArticleDict.Keys.Contains(item.id)) {
                                    newArticleList.Add(item.id);
                                    newArticleDict.Add(item.id, item);
                                }
                            });
                            StoreProvider.store.Dispatch(new UserMapAction {userMap = articlesResponse.userMap});
                            StoreProvider.store.Dispatch(new TeamMapAction {teamMap = articlesResponse.teamMap});
                            StoreProvider.store.Dispatch(new FetchArticleSuccessAction
                                {articleDict = newArticleDict, articleList = newArticleList, total = articlesResponse.total});
                            _refreshController.sendBack(false, RefreshStatus.idle);

                        }
                    })
                    .Catch(error =>
                    { 
                        _refreshController.sendBack(true, RefreshStatus.failed);
                    });
            }
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