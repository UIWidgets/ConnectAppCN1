using System;
using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;
using UnityEngine;
using EventType = ConnectApp.models.EventType;

namespace ConnectApp.screens {
    
    public class MyEventsScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyEventsScreenViewModel>(
                converter: (state) => new MyEventsScreenViewModel {
                    futureEventsList = state.mineState.futureEventsList,
                    pastEventsList = state.mineState.pastEventsList,
                    futureListLoading = state.mineState.futureListLoading,
                    pastListLoading = state.mineState.pastListLoading,
                    futureEventTotal = state.mineState.futureEventTotal,
                    pastEventTotal = state.mineState.pastEventTotal,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyEventsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToEventDetail = (id, type) =>
                            dispatcher.dispatch(new MainNavigatorPushToEventDetailAction
                                {eventId = id, eventType = type}),
                        startFetchMyFutureEvents = () => dispatcher.dispatch(new StartFetchMyFutureEventsAction()),
                        fetchMyFutureEvents = (pageNumber) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchMyFutureEvents(pageNumber)),
                        startFetchMyPastEvents = () => dispatcher.dispatch(new StartFetchMyPastEventsAction()),
                        fetchMyPastEvents = (pageNumber) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchMyPastEvents(pageNumber))
                    };
                    return new MyEventsScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class MyEventsScreen : StatefulWidget {
        public MyEventsScreen(
            MyEventsScreenViewModel viewModel = null,
            MyEventsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly MyEventsScreenViewModel viewModel;
        public readonly MyEventsScreenActionModel actionModel;
        
        public override State createState() {
            return new _MyEventsScreenState();
        }
    }

    internal class _MyEventsScreenState : State<MyEventsScreen> {
        private const int firstPageNumber = 1;
        private PageController _pageController;
        private int _selectedIndex;
        private int _myFuturePageNumber;
        private int _myPastPageNumber;
        private RefreshController _refreshController;

        public override void initState() {
            base.initState();
            _pageController = new PageController();
            _selectedIndex = 0;
            _myFuturePageNumber = firstPageNumber;
            _myPastPageNumber = firstPageNumber;
            _refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchMyFutureEvents();
                widget.actionModel.fetchMyFutureEvents(_myFuturePageNumber);
            });
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
        
        private void _fetchMyPastEvents() {
            widget.actionModel.fetchMyPastEvents(1);
        }

        private void _onRefresh(bool up) {
            if (_selectedIndex == 0) {
                if (up)
                    _myFuturePageNumber = firstPageNumber;
                else
                    _myFuturePageNumber++;
                widget.actionModel.fetchMyFutureEvents(_myFuturePageNumber)
                    .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                    .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
            }

            if (_selectedIndex == 1) {
                if (up)
                    _myPastPageNumber = firstPageNumber;
                else
                    _myPastPageNumber++;
                widget.actionModel.fetchMyPastEvents(_myPastPageNumber)
                    .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                    .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
            }
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
                                onPressed: () => widget.actionModel.mainRouterPop(),
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
            var data = index == 0 ? widget.viewModel.futureEventsList : widget.viewModel.pastEventsList;
            var hasMore = true;
            if (index == 0) {
                if (widget.viewModel.futureListLoading) return new GlobalLoading();
                if (data.Count <= 0) return new BlankView("暂无我的即将开始活动");
                var futureEventTotal = widget.viewModel.futureEventTotal;
                hasMore = futureEventTotal == data.Count;
            }

            if (index == 1) {
                if (widget.viewModel.pastListLoading) return new GlobalLoading();
                if (data.Count <= 0) return new BlankView("暂无我的往期活动");
                var pastEventTotal = widget.viewModel.pastEventTotal;
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
                        var place = widget.viewModel.placeDict[model.placeId];
                        return new EventCard(
                            model,
                            place,
                            () => widget.actionModel.pushToEventDetail(model.id, eventType)
                        );
                    }
                )
            );
        }
    }
}