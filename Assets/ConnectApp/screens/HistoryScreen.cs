using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class HistoryScreen : StatefulWidget {
        public HistoryScreen(
            Key key = null
        ) : base(key) {
        }

        public override State createState() {
            return new _HistoryScreenState();
        }
    }

    internal class _HistoryScreenState : State<HistoryScreen> {
        private PageController _pageController;
        private int _selectedIndex;

        public override void initState() {
            base.initState();
            _pageController = new PageController();
            _selectedIndex = 0;
            if (StoreProvider.store.state.articleState.articleHistory.Count == 0)
                StoreProvider.store.Dispatch(new GetArticleHistoryAction());
            if (StoreProvider.store.state.eventState.eventHistory.Count == 0)
                StoreProvider.store.Dispatch(new GetEventHistoryAction());
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                _buildNavigationBar(context),
                                _buildSelectView(),
                                _buildContentView()
                            }
                        )
                    )
                )
            );
        }

        private static Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(
                    CColors.White
                ),
                width: MediaQuery.of(context).size.width,
                height: 140,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(height:44,
                            child:new CustomButton(
                                padding: EdgeInsets.only(16),
                                onPressed: () => Router.navigator.pop(),
                                child: new Icon(
                                    Icons.arrow_back,
                                    size: 24,
                                    color: CColors.icon3
                                )
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(16, bottom: 12),
                            child: new Text(
                                "浏览历史",
                                style: CTextStyle.H2
                            )
                        )
                    }
                )
            );
        }

        private Widget _buildSelectView() {
            return new Container(
                decoration: new BoxDecoration(
                    border: new Border(
                        bottom: new BorderSide(
                            CColors.Separator2
                        )
                    )
                ),
                child: new CustomSegmentedControl(
                    new List<string> {"文章", "活动"},
                    index => {
                        if (_selectedIndex != index) {
                            setState(() => _selectedIndex = index);
                            _pageController.animateToPage(
                                index,
                                new TimeSpan(0, 0, 0, 0, 250),
                                Curves.ease
                            );
                        }
                    },
                    _selectedIndex
                )
            );
        }

        private Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: _pageController,
                        onPageChanged: index => { setState(() => { _selectedIndex = index; }); },
                        children: new List<Widget> {
                            _buildArticleHistory(),
                            _buildEventHistory()
                        }
                    )
                )
            );
        }

        private static Widget _buildArticleHistory() {
            return new Container(
                child: new StoreConnector<AppState, Dictionary<string, List<Article>>>(
                    converter: (state, dispatch) => new Dictionary<string, List<Article>> {
                        {"articleHistory", state.articleState.articleHistory}
                    },
                    builder: (_context, viewModel) => {
                        var data = viewModel["articleHistory"];
                        if (data.Count <= 0) return new BlankView("暂无浏览文章记录");
                        return ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: data.Count,
                            itemBuilder: (cxt, idx) => {
                                var model = data[idx];
                                return new Dismissible(
                                    Key.key(model.id),
                                    new ArticleCard(
                                        model,
                                        () => {
                                            StoreProvider.store.Dispatch(new NavigatorToArticleDetailAction
                                                {detailId = model.id});
                                            Router.navigator.pushNamed("/article-detail");
                                        }
                                    ),
                                    new Container(
                                        color: CColors.Red,
                                        child: new Row(
                                            mainAxisAlignment: MainAxisAlignment.end,
                                            children: new List<Widget> {
                                                new Text("删除")
                                            }
                                        )
                                    ),
                                    direction: DismissDirection.endToStart,
                                    onDismissed: direction =>
                                        StoreProvider.store.Dispatch(new DeleteArticleHistoryAction
                                            {articleId = model.id})
                                );
                            }
                        );
                    }
                )
            );
        }

        private static Widget _buildEventHistory() {
            return new Container(
                child: new StoreConnector<AppState, Dictionary<string, List<IEvent>>>(
                    converter: (state, dispatch) => new Dictionary<string, List<IEvent>> {
                        {"eventHistory", state.eventState.eventHistory}
                    },
                    builder: (_context, viewModel) => {
                        var data = viewModel["eventHistory"];
                        if (data.Count <= 0) return new BlankView("暂无浏览活动记录");
                        return ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: data.Count,
                            itemBuilder: (cxt, idx) => {
                                var model = data[idx];
                                return new Dismissible(
                                    Key.key(model.id),
                                    new EventCard(
                                        model,
                                        () => {
                                            StoreProvider.store.Dispatch(new NavigatorToEventDetailAction
                                                {eventId = model.id});
                                            Router.navigator.pushNamed("/event-detail");
                                        }
                                    ),
                                    new Container(
                                        color: CColors.Red,
                                        child: new Row(
                                            mainAxisAlignment: MainAxisAlignment.end,
                                            children: new List<Widget> {
                                                new Text("删除")
                                            }
                                        )
                                    ),
                                    direction: DismissDirection.endToStart,
                                    onDismissed: direction =>
                                        StoreProvider.store.Dispatch(new DeleteEventHistoryAction {eventId = model.id})
                                );
                            }
                        );
                    }
                )
            );
        }
    }
}