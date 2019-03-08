using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.components.refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
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
        private List<Article> _articles = new List<Article>();
        private int pageNumber = 1;
        
        public override void initState()
        {
            base.initState();

            if (_articles.Count==0)
            {
                StoreProvider.store.Dispatch(new FetchArticlesAction{pageNumber = pageNumber});
            }
            
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

        private Widget _ArticelList(BuildContext context)
        {
            return new NotificationListener<ScrollNotification>(
                    onNotification: (ScrollNotification notification) => { return _OnNotification(context, notification); },
                    child: new Container(
                        margin: EdgeInsets.only(0, headerHeight - _offsetY, 0, 49),
                            child:new StoreConnector<AppState,ArticleState>(
                                converter: (state, dispatch) => { return state.ArticleState; },
                                builder: (_context, viewModel) =>
                                {
                                    var refreshPage = new Refresh(
                                        onHeaderRefresh: onHeaderRefresh,
                                        onFooterRefresh: onHeaderRefresh,
                                        child: new ListView(
                                            physics: new AlwaysScrollableScrollPhysics(),
                                            children: _buildArtileCards(viewModel.ArticleList)
                                        )
                                    );
                                    return refreshPage;
                                }
                        )
                    )
            );

        }

        Promise onHeaderRefresh()
        {
            var promise = new Promise((resolve, reject) => { resolve(); }, false);
            
            return promise;
        }
        
        List<Widget> _buildArtileCards(List<string> items)
        {
            var list = new List<Widget>();
            items.ForEach((id) =>
            {
                list.Add(new ArticleCard(
                    StoreProvider.store.state.ArticleState.ArticleDict[id],
                    () =>
                    {
                        StoreProvider.store.Dispatch(new NavigatorToLiveAction {eventId = id});
                        Navigator.pushName(context, "/detail");
                    }));
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