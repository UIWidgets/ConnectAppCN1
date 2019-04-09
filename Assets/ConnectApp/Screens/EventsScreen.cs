using System;
using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;
using UnityEngine;
using EventType = ConnectApp.models.EventType;

namespace ConnectApp.screens {
    
    public class EventsScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventsScreenModel>(
                pure: true,
                converter: state => new EventsScreenModel {
                    eventsLoading = state.eventState.eventsLoading, 
                    ongoingEvents = state.eventState.ongoingEvents, 
                    completedEvents = state.eventState.completedEvents, 
                    ongoingEventTotal = state.eventState.ongoingEventTotal, 
                    completedEventTotal = state.eventState.completedEventTotal, 
                    eventsDict = state.eventState.eventsDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new EventsScreen(
                        viewModel,
                        (eventId, eventType) => dispatcher.dispatch(new MainNavigatorPushToEventDetailAction {
                            eventId = eventId, eventType = eventType
                        }),
                        (pageNumber, tab) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber, tab))
                    );
                });
        }
    }
    public class EventsScreen : StatefulWidget {
        public EventsScreen(
            EventsScreenModel screenModel = null,
            Action<string, EventType> pushToEventDetail = null,
            Func<int, string, IPromise> fetchEvents = null,
            Key key = null
            ) : base(key)
        {
            this.screenModel = screenModel;
            this.pushToEventDetail = pushToEventDetail;
            this.fetchEvents = fetchEvents;
        }

        public EventsScreenModel screenModel;
        public Action<string, EventType> pushToEventDetail;
        public Func<int, string, IPromise> fetchEvents;

        public override State createState() {
            return new _EventsScreenState();
        }
    }

    internal class _EventsScreenState : AutomaticKeepAliveClientMixin<EventsScreen>
    {
        private const int firstPageNumber = 1;
        private const float headerHeight = 80;
        private PageController _pageController;
        private int _selectedIndex;
        private int pageNumber = firstPageNumber;
        private int completedPageNumber = firstPageNumber;
        private float _offsetY;
        private RefreshController _ongoingRefreshController;
        private RefreshController _completedRefreshController;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            _offsetY = 0;
            _ongoingRefreshController = new RefreshController();
            _completedRefreshController = new RefreshController();
            _pageController = new PageController();
            _selectedIndex = 0;
            widget.fetchEvents(pageNumber, "ongoing");
            widget.fetchEvents(completedPageNumber, "completed");
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new CustomNavigationBar(
                            new Text("活动", style: CTextStyle.H2),
                            null,
                            CColors.White,
                            _offsetY
                        ),
                        _buildSelectView(),
                        _buildContentView()
                    }
                )
            );
        }

        private Widget _buildSelectItem(string title, int index) {
            Widget lineView = new Positioned(new Container());
            if (index == _selectedIndex)
                lineView = new Positioned(
                    bottom: 0,
                    left: 0,
                    right: 0,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: new List<Widget> {
                            new Container(
                                width: 80,
                                height: 2,
                                decoration: new BoxDecoration(
                                    CColors.PrimaryBlue
                                )
                            )
                        }
                    )
                );

            return new Container(
                child: new Stack(
                    children: new List<Widget> {
                        new CustomButton(
                            onPressed: () => {
                                if (_selectedIndex != index) setState(() => _selectedIndex = index);
                                _pageController.animateToPage(
                                    index,
                                    new TimeSpan(0, 0,
                                        0, 0, 250),
                                    Curves.ease
                                );
                            },
                            child: new Container(
                                height: 44,
                                width: 96,
                                alignment: Alignment.center,
                                child: new Text(
                                    title,
                                    style: index == _selectedIndex ? CTextStyle.PLargeBlue : CTextStyle.PLargeMedium
                                )
                            )
                        ),
                        lineView
                    }
                )
            );
        }

        private Widget _buildSelectView() {
            return new Container(
                child: new Container(
                    decoration: new BoxDecoration(
                        border: new Border(bottom: new BorderSide(CColors.Separator2))
                    ),
                    height: 44,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: new List<Widget> {
                            _buildSelectItem("即将开始", 0),
                            _buildSelectItem("往期活动", 1)
                        }
                    )
                )
            );
        }

        private Widget _buildOngoingEventList() {
            if (widget.screenModel.eventsLoading) return new GlobalLoading();
            return new SmartRefresher(
                controller: _ongoingRefreshController,
                enablePullDown: true,
                enablePullUp: widget.screenModel.ongoingEvents.Count < widget.screenModel.ongoingEventTotal,
                headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                onRefresh: _ongoingRefresh,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: widget.screenModel.ongoingEvents.Count,
                    itemBuilder: (cxt, index) => {
                        var eventId = widget.screenModel.ongoingEvents[index];
                        var model = widget.screenModel.eventsDict[eventId];
                        return new EventCard(
                            model,
                            () => widget.pushToEventDetail(
                                model.id,
                                model.mode == "online" ? EventType.onLine : EventType.offline
                            ),
                            new ObjectKey(model.id)
                        );
                    }
                )
            );
        }

        private Widget _buildCompletedEventList() {
            if (widget.screenModel.eventsLoading) return new GlobalLoading();
            return new SmartRefresher(
                controller: _completedRefreshController,
                enablePullDown: true,
                enablePullUp: widget.screenModel.completedEvents.Count < widget.screenModel.completedEventTotal,
                headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                onRefresh: _completedRefresh,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: widget.screenModel.completedEvents.Count,
                    itemBuilder: (cxt, index) => {
                        var eventId = widget.screenModel.completedEvents[index];
                        var model = widget.screenModel.eventsDict[eventId];
                        return new EventCard(
                            model,
                            () => widget.pushToEventDetail(
                                model.id,
                                model.mode == "online" ? EventType.onLine : EventType.offline
                            ),
                            new ObjectKey(model.id)
                        );
                    }
                )
            );
        }

        private Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    padding: EdgeInsets.only(bottom: 49),
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: _pageController,
                        onPageChanged: index => { setState(() => { _selectedIndex = index; }); },
                        children: new List<Widget> {
                            _buildOngoingEventList(),
                            _buildCompletedEventList()
                        }
                    )
                )
            );
        }

        private void _ongoingRefresh(bool up) {
            if (up) {
                pageNumber = 1;
            } else {
                pageNumber++;
            }
            widget.fetchEvents(pageNumber, "ongoing")
                .Then(() => _ongoingRefreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _ongoingRefreshController.sendBack(up, RefreshStatus.failed));
        }

        private void _completedRefresh(bool up) {
            if (up) {
                completedPageNumber = 1;
            } else {
                completedPageNumber++;
            }
            widget.fetchEvents(completedPageNumber, "completed")
                .Then(() => _ongoingRefreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _ongoingRefreshController.sendBack(up, RefreshStatus.failed));
        }


        public override void dispose() {
            base.dispose();
            _pageController.dispose();
        }
    }
}