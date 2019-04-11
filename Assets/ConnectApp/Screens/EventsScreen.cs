using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using ConnectApp.utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventsScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventsScreenViewModel>(
                converter: state => new EventsScreenViewModel {
                    eventsLoading = state.eventState.eventsLoading,
                    ongoingEvents = state.eventState.ongoingEvents,
                    completedEvents = state.eventState.completedEvents,
                    ongoingEventTotal = state.eventState.ongoingEventTotal,
                    completedEventTotal = state.eventState.completedEventTotal,
                    eventsDict = state.eventState.eventsDict,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new EventsScreenActionModel {
                        pushToEventDetail = (eventId, eventType) => dispatcher.dispatch(
                            new MainNavigatorPushToEventDetailAction {
                                eventId = eventId, eventType = eventType
                            }),
                        startFetchEvents = () => dispatcher.dispatch(new StartFetchEventsAction()),
                        fetchEvents = (pageNumber, tab) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber, tab))
                    };
                    return new EventsScreen(viewModel, actionModel);
                });
        }
    }

    public class EventsScreen : StatefulWidget {
        public EventsScreen(
            EventsScreenViewModel viewModel = null,
            EventsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly EventsScreenViewModel viewModel;
        public readonly EventsScreenActionModel actionModel;

        public override State createState() {
            return new _EventsScreenState();
        }
    }

    internal class _EventsScreenState : State<EventsScreen> {
        private const int firstPageNumber = 1;
        private PageController _pageController;
        private int _selectedIndex;
        private int pageNumber = firstPageNumber;
        private int completedPageNumber = firstPageNumber;
        private float _offsetY;
        private RefreshController _ongoingRefreshController;
        private RefreshController _completedRefreshController;
        private string _loginSubId;

//        protected override bool wantKeepAlive {
//            get => true;
//        }

        public override void initState() {
            base.initState();
            _offsetY = 0;
            _ongoingRefreshController = new RefreshController();
            _completedRefreshController = new RefreshController();
            _pageController = new PageController();
            _selectedIndex = 0;
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchEvents();
                widget.actionModel.fetchEvents(pageNumber, "ongoing");
                widget.actionModel.fetchEvents(completedPageNumber, "completed");
            });
            _loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                var tab = _selectedIndex == 0 ? "ongoing" : "completed";
                widget.actionModel.fetchEvents(firstPageNumber, tab);
            });
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, _loginSubId);
            _pageController.dispose();
            base.dispose();
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
                                    style: index == _selectedIndex
                                        ? CTextStyle.PLargeMediumBlue
                                        : CTextStyle.PLargeTitle
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
            if (widget.viewModel.eventsLoading) return new GlobalLoading();
            return new SmartRefresher(
                controller: _ongoingRefreshController,
                enablePullDown: true,
                enablePullUp: widget.viewModel.ongoingEvents.Count < widget.viewModel.ongoingEventTotal,
                headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                onRefresh: _ongoingRefresh,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: widget.viewModel.ongoingEvents.Count,
                    itemBuilder: (cxt, index) => {
                        var eventId = widget.viewModel.ongoingEvents[index];
                        var model = widget.viewModel.eventsDict[eventId];
                        var place = model.placeId.isEmpty() ? null : widget.viewModel.placeDict[model.placeId];
                        return new EventCard(
                            model,
                            place,
                            () => widget.actionModel.pushToEventDetail(
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
            if (widget.viewModel.eventsLoading) return new GlobalLoading();
            return new SmartRefresher(
                controller: _completedRefreshController,
                enablePullDown: true,
                enablePullUp: widget.viewModel.completedEvents.Count < widget.viewModel.completedEventTotal,
                headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                onRefresh: _completedRefresh,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: widget.viewModel.completedEvents.Count,
                    itemBuilder: (cxt, index) => {
                        var eventId = widget.viewModel.completedEvents[index];
                        var model = widget.viewModel.eventsDict[eventId];
                        var place = model.placeId.isEmpty() ? null : widget.viewModel.placeDict[model.placeId];
                        return new EventCard(
                            model,
                            place,
                            () => widget.actionModel.pushToEventDetail(
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
            if (up)
                pageNumber = firstPageNumber;
            else
                pageNumber++;
            widget.actionModel.fetchEvents(pageNumber, "ongoing")
                .Then(() => _ongoingRefreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _ongoingRefreshController.sendBack(up, RefreshStatus.failed));
        }

        private void _completedRefresh(bool up) {
            if (up)
                completedPageNumber = firstPageNumber;
            else
                completedPageNumber++;
            widget.actionModel.fetchEvents(completedPageNumber, "completed")
                .Then(() => _completedRefreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _completedRefreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}