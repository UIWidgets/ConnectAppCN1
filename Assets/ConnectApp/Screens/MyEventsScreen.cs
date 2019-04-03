using System;
using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
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
    public class MyEventsScreen : StatefulWidget {
        public MyEventsScreen(
            Key key = null
        ) : base(key) {
        }

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
            if (StoreProvider.store.state.mineState.futureEventsList.Count == 0)
                StoreProvider.store.Dispatch(new FetchMyFutureEventsAction {pageNumber = 1});
        }

        private static void _fetchMyPastEvents() {
            if (StoreProvider.store.state.mineState.pastEventsList.Count == 0)
                StoreProvider.store.Dispatch(new FetchMyPastEventsAction {pageNumber = 1});
        }

        private void _onRefresh(bool up) {
            if (_selectedIndex == 0) {
                if (up)
                    _myFuturePageNumber = 1;
                else
                    _myFuturePageNumber++;
                MineApi.FetchMyFutureEvents(_myFuturePageNumber)
                    .Then(eventsResponse => {
                        StoreProvider.store.Dispatch(new FetchMyFutureEventsSuccessAction
                            {eventsResponse = eventsResponse, pageNumber = _myFuturePageNumber});
                        _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle);
                    })
                    .Catch(error => { _refreshController.sendBack(up, RefreshStatus.failed); });
            }

            if (_selectedIndex == 1) {
                if (up)
                    _myPastPageNumber = 1;
                else
                    _myPastPageNumber++;
                MineApi.FetchMyPastEvents(_myPastPageNumber)
                    .Then(eventsResponse => {
                        StoreProvider.store.Dispatch(new FetchMyPastEventsSuccessAction
                            {eventsResponse = eventsResponse, pageNumber = _myPastPageNumber});
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

        private static Widget _buildNavigationBar(BuildContext context) {
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
                                onPressed: () => StoreProvider.store.Dispatch(new MainNavigatorPopAction()),
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
            return new Container(
                child: new StoreConnector<AppState, MineState>(
                    converter: (state, dispatch) => state.mineState,
                    builder: (_context, viewModel) => {
                        var data = index == 0 ? viewModel.futureEventsList : viewModel.pastEventsList;
                        var hasMore = true;
                        if (index == 0) {
                            if (viewModel.futureListLoading) return new GlobalLoading();
                            if (data.Count <= 0) return new BlankView("暂无我的即将开始活动");
                            var futureEventTotal = viewModel.futureEventTotal;
                            hasMore = futureEventTotal == data.Count;
                        }

                        if (index == 1) {
                            if (viewModel.pastListLoading) return new GlobalLoading();
                            if (data.Count <= 0) return new BlankView("暂无我的往期活动");
                            var pastEventTotal = viewModel.pastEventTotal;
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
                                    return new EventCard(
                                        model,
                                        () => {
                                            StoreProvider.store.Dispatch(new MainNavigatorPushToEventDetailAction {
                                                eventId = model.id,
                                                eventType = model.mode == "online"
                                                    ? EventType.onLine
                                                    : EventType.offline
                                            });
                                        }
                                    );
                                }
                            )
                        );
                    }
                )
            );
        }
    }
}