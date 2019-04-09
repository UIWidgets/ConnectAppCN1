using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class HistoryScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, HistoryScreenModel>(
                pure: true,
                converter: (state) => new HistoryScreenModel {
                    eventHistory = state.eventState.eventHistory,
                    articleHistory = state.articleState.articleHistory
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new HistoryScreen(
                        viewModel,
                        mainNavigatorPop: () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToArticleDetail: (id) =>
                            dispatcher.dispatch(new MainNavigatorPushToArticleDetailAction {articleId = id}),
                        pushToEventDetail: (id, type) =>
                            dispatcher.dispatch(new MainNavigatorPushToEventDetailAction
                                {eventId = id, eventType = type}),
                        getArticleHistory: () =>
                            dispatcher.dispatch(new GetArticleHistoryAction()),
                        getEventHistory: () =>
                            dispatcher.dispatch(new GetEventHistoryAction()),
                        deleteArticleHistory: (id) =>
                            dispatcher.dispatch(new DeleteArticleHistoryAction {articleId = id}),
                        deleteEventHistory: (id) =>
                            dispatcher.dispatch(new DeleteEventHistoryAction {eventId = id})
                    );
                }
            );
        }
    }

    public class HistoryScreen : StatefulWidget {
        public HistoryScreen(
            HistoryScreenModel screenModel = null,
            Action mainNavigatorPop = null,
            Action<string> pushToArticleDetail = null,
            Action<string, EventType> pushToEventDetail = null,
            Action getArticleHistory = null,
            Action getEventHistory = null,
            Action<string> deleteArticleHistory = null,
            Action<string> deleteEventHistory = null,
            Key key = null
        ) : base(key) {
            this.screenModel = screenModel;
            this.mainNavigatorPop = mainNavigatorPop;
            this.pushToArticleDetail = pushToArticleDetail;
            this.pushToEventDetail = pushToEventDetail;
            this.getArticleHistory = getArticleHistory;
            this.getEventHistory = getEventHistory;
            this.deleteArticleHistory = deleteArticleHistory;
            this.deleteEventHistory = deleteEventHistory;
        }

        public HistoryScreenModel screenModel;
        public Action mainNavigatorPop;
        public Action<string> pushToArticleDetail;
        public Action<string, EventType> pushToEventDetail;
        public Action getArticleHistory;
        public Action getEventHistory;
        public Action<string> deleteArticleHistory;
        public Action<string> deleteEventHistory;

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
            widget.getArticleHistory();
            widget.getEventHistory();
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

        private Widget _buildNavigationBar(BuildContext context) {
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
                        new Container(height: 44,
                            child: new CustomButton(
                                padding: EdgeInsets.only(16),
                                onPressed: () => widget.mainNavigatorPop(),
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

        private Widget _buildArticleHistory() {
            if (widget.screenModel.articleHistory.Count == 0) return new BlankView("暂无浏览文章记录");
            return ListView.builder(
                physics: new AlwaysScrollableScrollPhysics(),
                itemCount: widget.screenModel.articleHistory.Count,
                itemBuilder: (cxt, index) => {
                    var model = widget.screenModel.articleHistory[index];
                    return new Dismissible(
                        Key.key(model.id),
                        new ArticleCard(
                            model,
                            "",
                            () => widget.pushToArticleDetail(model.id)
                        ),
                        new Container(
                            color: CColors.Error,
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.end,
                                children: new List<Widget> {
                                    new Text(
                                        "删除",
                                        style: CTextStyle.PLargeWhite
                                    )
                                }
                            )
                        ),
                        direction: DismissDirection.endToStart,
                        onDismissed: direction => widget.deleteArticleHistory(model.id)
                    );
                }
            );
        }

        private Widget _buildEventHistory() {
            if (widget.screenModel.eventHistory.Count == 0) return new BlankView("暂无浏览活动记录");
            return ListView.builder(
                physics: new AlwaysScrollableScrollPhysics(),
                itemCount: widget.screenModel.eventHistory.Count,
                itemBuilder: (cxt, index) => {
                    var model = widget.screenModel.eventHistory[index];
                    var eventType = model.mode == "online" ? EventType.onLine : EventType.offline;
                    return new Dismissible(
                        Key.key(model.id),
                        new EventCard(
                            model,
                            () => widget.pushToEventDetail(model.id, eventType)
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
                        onDismissed: direction => widget.deleteEventHistory(model.id)
                    );
                }
            );
        }
    }
}