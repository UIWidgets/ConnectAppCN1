using System.Collections.Generic;
using System.Linq;
using ConnectApp.api;
using ConnectApp.components;
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
        private float _offsetY = 0;
        private int pageNumber = 1;

        public override void initState() {
            base.initState();

            if (StoreProvider.store.state.articleState.articleList.Count == 0)
                StoreProvider.store.Dispatch(new FetchArticlesAction {pageNumber = pageNumber});
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Stack(
                    children: new List<Widget> {
                        _ArticelList(context),
                        new Positioned(
                            top: 0,
                            left: 0,
                            right: 0,
                            child: new CustomNavigationBar(new Text("文章", style: new TextStyle(
                                height: 1.25f,
                                fontSize: 32 / headerHeight * (headerHeight - _offsetY),
                                fontFamily: "PingFang-Semibold",
                                color: CColors.TextTitle
                            )), new List<Widget> {
                                new Container(child: new Icon(Icons.search, null, 28,
                                    Color.fromRGBO(255, 255, 255, 0.8f))),
                                new GestureDetector(
                                    onTap: () => { StoreProvider.store.Dispatch(new LoginByEmailAction()); },
                                    child: new Container(
                                        color: CColors.BrownGrey,
                                        child: new Text(
                                            "LoginByEmail",
                                            style: CTextStyle.H2
                                        )
                                    )
                                )
                            }, CColors.White, _offsetY)
                        )
                    }
                )
            );
        }

        private Widget _ArticelList(BuildContext context) {
            return new NotificationListener<ScrollNotification>(
                onNotification: (ScrollNotification notification) => { return _OnNotification(context, notification); },
                child: new Container(
                    margin: EdgeInsets.only(0, headerHeight - _offsetY, 0, 49),
                    child: new StoreConnector<AppState, ArticleState>(
                        converter: (state, dispatch) => { return state.articleState; },
                        builder: (_context, viewModel) => {
                            if (viewModel.articlesLoading) return new Container();
                            var refreshPage = new Refresh(
                                onHeaderRefresh: onHeaderRefresh,
                                onFooterRefresh: onFooterRefresh,
                                child: new ListView(
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    children: _buildArtileCards(context,viewModel.articleList)
                                )
                            );
                            return refreshPage;
                        }
                    )
                )
            );
        }

        private IPromise onHeaderRefresh() {
            pageNumber = 1;
            return ArticleApi.FetchArticles(pageNumber)
                .Then((articlesResponse) => {
                    var articleList = new List<string>();
                    var articleDict = new Dictionary<string, Article>();
                    articlesResponse.items.ForEach((item) => {
                        articleList.Add(item.id);
                        articleDict.Add(item.id, item);
                    });
                    StoreProvider.store.Dispatch(new FetchArticleSuccessAction
                        {ArticleDict = articleDict, ArticleList = articleList});
                })
                .Catch(error => { Debug.Log(error); });
        }

        private IPromise onFooterRefresh() {
            pageNumber++;
            return ArticleApi.FetchArticles(pageNumber)
                .Then((articlesResponse) => {
                    if (articlesResponse.items.Count != 0) {
                        var articleList = StoreProvider.store.state.articleState.articleList;
                        var articleDict = StoreProvider.store.state.articleState.articleDict;
                        articlesResponse.items.ForEach((item) => {
                            if (!articleDict.Keys.Contains(item.id)) {
                                articleList.Add(item.id);
                                articleDict.Add(item.id, item);
                            }
                        });
                        StoreProvider.store.Dispatch(new FetchArticleSuccessAction
                            {ArticleDict = articleDict, ArticleList = articleList});
                    }
                })
                .Catch(error => { Debug.Log(error); });
        }

        private List<Widget> _buildArtileCards(BuildContext context,List<string> items) {
            var list = new List<Widget>();
            items.ForEach((id) => {
                list.Add(new ArticleCard(
                    StoreProvider.store.state.articleState.articleDict[id],
                    () => {
                        StoreProvider.store.Dispatch(new NavigatorToArticleDetailAction() {detailId = id});
                        Navigator.pushNamed(context, "/article-detail");
                    }, Key.key(id)));
            });
            return list;
        }


        private bool _OnNotification(BuildContext context, ScrollNotification notification) {
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