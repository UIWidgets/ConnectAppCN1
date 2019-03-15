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
                    fontFamily: "PingFang-Semibold",
                    color: CColors.TextTitle
                )),
                new List<Widget> {
                    new CustomButton(
                        onPressed: () => Navigator.pushNamed(context, "/search"),
                        child: new Icon(
                            Icons.search,
                            size: 28,
                            color: Color.fromRGBO(181, 181, 181, 1))
                     ),
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
                },
                CColors.White,
                _offsetY);
        }

        private Widget _buildArticleList(BuildContext context) {
            return new NotificationListener<ScrollNotification>(
                onNotification: _onNotification,
                child: new Container(
                    margin: EdgeInsets.only(bottom: 49),
                    child: new StoreConnector<AppState, Dictionary<string,object>>(
                        converter: (state, dispatch) => new Dictionary<string, object>{
                            {"articlesLoading",state.articleState.articlesLoading},
                            {"articleList",state.articleState.articleList}
                        },
                        builder: (_context, viewModel) => {
                            bool articlesLoading = (bool)viewModel["articlesLoading"];
                            if (articlesLoading) return new Container();
                            
                            var articleList = (List<string>)viewModel["articleList"];
                            var refreshPage = new Refresh(
                                onHeaderRefresh: onHeaderRefresh,
                                onFooterRefresh: onFooterRefresh,
                                child: new ListView(
                                    physics: new AlwaysScrollableScrollPhysics(),
                                    children: _buildArticleCards(context, articleList)
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
                .Then(articlesResponse => {
                    var articleList = new List<string>();
                    var articleDict = new Dictionary<string, Article>();
                    articlesResponse.items.ForEach(item => {
                        articleList.Add(item.id);
                        articleDict.Add(item.id, item);
                    });
                    StoreProvider.store.Dispatch(new UserMapAction{userMap = articlesResponse.userMap});
                    StoreProvider.store.Dispatch(new FetchArticleSuccessAction
                        {ArticleDict = articleDict, ArticleList = articleList});
                })
                .Catch(error => { Debug.Log(error); });
        }

        private IPromise onFooterRefresh() {
            pageNumber++;
            return ArticleApi.FetchArticles(pageNumber)
                .Then(articlesResponse => {
                    if (articlesResponse.items.Count != 0) {
                        var articleList = StoreProvider.store.state.articleState.articleList;
                        var articleDict = StoreProvider.store.state.articleState.articleDict;
                        articlesResponse.items.ForEach(item => {
                            if (!articleDict.Keys.Contains(item.id)) {
                                articleList.Add(item.id);
                                articleDict.Add(item.id, item);
                            }
                        });
                        StoreProvider.store.Dispatch(new UserMapAction{userMap = articlesResponse.userMap});
                        StoreProvider.store.Dispatch(new FetchArticleSuccessAction
                            {ArticleDict = articleDict, ArticleList = articleList});
                    }
                })
                .Catch(error => { Debug.Log(error); });
        }

        private static List<Widget> _buildArticleCards(BuildContext context, List<string> items) {
            var list = new List<Widget>();
            items.ForEach(id => {
                var article = StoreProvider.store.state.articleState.articleDict[id];
                var user = new User();
                if (StoreProvider.store.state.userState.userDict.ContainsKey(article.userId)) {
                    user = StoreProvider.store.state.userState.userDict[article.userId];
                }
                list.Add(new ArticleCard(
                    article,
                    user,
                    () => {
                        StoreProvider.store.Dispatch(new NavigatorToArticleDetailAction{detailId = id});
                        Navigator.pushNamed(context, "/article-detail");
                    }
                ));
            });
            return list;
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