using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventOngoingScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventsScreenViewModel>(
                converter: state => new EventsScreenViewModel {
                    eventOngoingLoading = state.eventState.eventsOngoingLoading,
                    ongoingEvents = state.eventState.ongoingEvents,
                    ongoingEventTotal = state.eventState.ongoingEventTotal,
                    eventsDict = state.eventState.eventsDict,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new EventsScreenActionModel {
                        pushToEventDetail = (eventId, eventType) => dispatcher.dispatch(
                            new MainNavigatorPushToEventDetailAction {
                                eventId = eventId, eventType = eventType
                            }),
                        startFetchEventOngoing = () => dispatcher.dispatch(new StartFetchEventOngoingAction()),
                        fetchEvents = (pageNumber, tab) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber, tab))
                    };
                    return new EventOngoingScreen(viewModel, actionModel);
                }
            );
        }
    }


    public class EventOngoingScreen : StatefulWidget {
        public EventOngoingScreen(
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
            return new _EventOngoingScreenState();
        }
    }

    public class _EventOngoingScreenState : AutomaticKeepAliveClientMixin<EventOngoingScreen> {
        private const int firstPageNumber = 1;
        private RefreshController _ongoingRefreshController;
        private int pageNumber = firstPageNumber;
        private string _loginSubId;

        protected override bool wantKeepAlive => true;

        public override void initState() {
            base.initState();
            _ongoingRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchEventOngoing();
                widget.actionModel.fetchEvents(firstPageNumber, "ongoing");
            });
            _loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
                widget.actionModel.startFetchEventOngoing();
                widget.actionModel.fetchEvents(firstPageNumber, "ongoing");
            });
        }

        public override void dispose() {
            EventBus.unSubscribe(EventBusConstant.login_success, _loginSubId);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            if (widget.viewModel.eventOngoingLoading && widget.viewModel.ongoingEvents.isEmpty())
                return new GlobalLoading();
            if (widget.viewModel.ongoingEvents.Count <= 0) 
                return new BlankView(
                    "暂无即将开始活动",
                    true, 
                    () => {
                        widget.actionModel.startFetchEventOngoing();
                        widget.actionModel.fetchEvents(firstPageNumber, "ongoing");
                    }
                );
            return new Container(
                color: CColors.background3,
                child: new SmartRefresher(
                    controller: _ongoingRefreshController,
                    enablePullDown: true,
                    enablePullUp: widget.viewModel.ongoingEvents.Count < widget.viewModel.ongoingEventTotal,
                    onRefresh: _ongoingRefresh,
                    child: ListView.builder(
                        itemExtent: 108,
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: widget.viewModel.ongoingEvents.Count,
                        itemBuilder: (cxt, index) => {
                            var eventId = widget.viewModel.ongoingEvents[index];
                            var model = widget.viewModel.eventsDict[eventId];
                            var placeName = model.placeId.isEmpty() ? null : widget.viewModel.placeDict[model.placeId].name;
                            return new EventCard(
                                model,
                                placeName,
                                () => widget.actionModel.pushToEventDetail(
                                    model.id,
                                    model.mode == "online" ? EventType.online : EventType.offline
                                ),
                                new ObjectKey(model.id)
                            );
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
    }
}