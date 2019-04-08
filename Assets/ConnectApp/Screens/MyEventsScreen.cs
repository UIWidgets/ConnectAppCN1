using System;
using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.Screen;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    
    public class MyEventsScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyEventsScreenModel>(
                pure: true,
                converter: (state) => new MyEventsScreenModel {
                    futureEventsList = state.mineState.futureEventsList,
                    pastEventsList = state.mineState.pastEventsList,
                    futureListLoading = state.mineState.futureListLoading,
                    pastListLoading = state.mineState.pastListLoading,
                    futureEventTotal = state.mineState.futureEventTotal,
                    pastEventTotal = state.mineState.pastEventTotal
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new MyEventsScreen(
                        viewModel,
                        mainNavigatorPop: () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToEventDetail: (id, type) =>
                            dispatcher.dispatch(new MainNavigatorPushToEventDetailAction
                                {eventId = id, eventType = type}),
                        fetchMyFutureEvents: (pageNumber) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchMyFutureEvents(pageNumber)),
                        fetchMyPastEvents: (pageNumber) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchMyPastEvents(pageNumber))
                    );
                }
            );
        }
    }
    
    public class MyEventsScreen : StatefulWidget {
        public MyEventsScreen(
            MyEventsScreenModel screenModel = null,
            Action mainNavigatorPop = null,
            Action<string, EventType> pushToEventDetail = null,
            Func<int, IPromise> fetchMyFutureEvents = null,
            Func<int, IPromise> fetchMyPastEvents = null,
            Key key = null
        ) : base(key) {
            this.screenModel = screenModel;
            this.mainNavigatorPop = mainNavigatorPop;
            this.pushToEventDetail = pushToEventDetail;
            this.fetchMyFutureEvents = fetchMyFutureEvents;
            this.fetchMyPastEvents = fetchMyPastEvents;
        }

        public MyEventsScreenModel screenModel;
        public Action mainNavigatorPop;
        public Action<string, EventType> pushToEventDetail;
        public Func<int, IPromise> fetchMyFutureEvents;
        public Func<int, IPromise> fetchMyPastEvents;

        public override State createState() {
            return new _MyEventsScreenState();
        }
    }

    internal class _MyEventsScreenState : State<MyEventsScreen> {
        private PageController _pageController;
        private int _selectedIndex;
        private int _myFuturePageNumber;
        private int _myPastPageNumber;
        private RefreshController _refreshController;


        public override void initState() {
            base.initState();
            _pageController = new PageController();
            _selectedIndex = 0;
            _myFuturePageNumber = 0;
            _myPastPageNumber = 0;
            _refreshController = new RefreshController();
            widget.fetchMyFutureEvents(1);
        }

        private void _fetchMyPastEvents() {
            widget.fetchMyPastEvents(1);
        }

        private void _onRefresh(bool up) {
            if (_selectedIndex == 0) {
                if (up)
                    _myFuturePageNumber = 1;
                else
                    _myFuturePageNumber++;
                    widget.fetchMyFutureEvents(_myFuturePageNumber)
                        .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                        .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
            }

            if (_selectedIndex == 1) {
                if (up)
                    _myPastPageNumber = 1;
                else
                    _myPastPageNumber++;
                MineApi.FetchMyPastEvents(_myPastPageNumber)
                    .Then(eventsResponse => {
                        _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle);
                    })
                    .Catch(error => { _refreshController.sendBack(up, RefreshStatus.failed); });
            }
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
                decoration: new BoxDecoration(CColors.White),
                width: MediaQuery.of(context).size.width,
                height: 140,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            child: new CustomButton(
                                padding: EdgeInsets.only(16, 10, 16),
                                onPressed: () => widget.mainNavigatorPop(),
                                child: new Icon(
                                    Icons.arrow_back,
                                    size: 24,
                                    color: CColors.icon3
                                )
                            ),
                            height: 44
                        ),
                        new Container(
                            margin: EdgeInsets.only(16, bottom: 12),
                            child: new Text(
                                "我的活动",
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
                    new List<string> {"即将开始", "往期活动"},
                    index => {
                        if (_selectedIndex != index) {
                            if (index == 1) _fetchMyPastEvents();
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
                        onPageChanged: index => {
                            if (index == 1) _fetchMyPastEvents();
                            setState(() => { _selectedIndex = index; });
                        },
                        children: new List<Widget> {
                            _buildMyEventContent(0),
                            _buildMyEventContent(1)
                        }
                    )
                )
            );
        }

        private Widget _buildMyEventContent(int index) {
            var data = index == 0 ? widget.screenModel.futureEventsList : widget.screenModel.pastEventsList;
            var hasMore = true;
            if (index == 0) {
                if (widget.screenModel.futureListLoading) return new GlobalLoading();
                if (data.Count <= 0) return new BlankView("暂无我的即将开始活动");
                var futureEventTotal = widget.screenModel.futureEventTotal;
                hasMore = futureEventTotal == data.Count;
            }

            if (index == 1) {
                if (widget.screenModel.pastListLoading) return new GlobalLoading();
                if (data.Count <= 0) return new BlankView("暂无我的往期活动");
                var pastEventTotal = widget.screenModel.pastEventTotal;
                hasMore = pastEventTotal == data.Count;
            }
            
            return new SmartRefresher(
                controller: _refreshController,
                enablePullDown: true,
                enablePullUp: !hasMore,
                headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                onRefresh: _onRefresh,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: data.Count,
                    itemBuilder: (cxt, idx) => {
                        var model = data[idx];
                        var eventType = model.mode == "online" ? EventType.onLine : EventType.offline;
                        return new EventCard(
                            model,
                            () => widget.pushToEventDetail(model.id, eventType)
                        );
                    }
                )
            );
        }
    }
}