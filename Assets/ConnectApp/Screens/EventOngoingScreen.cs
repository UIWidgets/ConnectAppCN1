using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
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
        const int firstPageNumber = 1;
        RefreshController _ongoingRefreshController;
        int pageNumber = firstPageNumber;
        string _loginSubId;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._ongoingRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchEventOngoing();
                this.widget.actionModel.fetchEvents(firstPageNumber, "ongoing");
            });
//            _loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
//                widget.actionModel.startFetchEventOngoing();
//                widget.actionModel.fetchEvents(firstPageNumber, "ongoing");
//            });
        }

        public override void dispose() {
//            EventBus.unSubscribe(EventBusConstant.login_success, _loginSubId);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            if (this.widget.viewModel.eventOngoingLoading && this.widget.viewModel.ongoingEvents.isEmpty()) {
                return new GlobalLoading();
            }

            if (this.widget.viewModel.ongoingEvents.Count <= 0) {
                return new BlankView(
                    "暂无即将开始活动",
                    true,
                    () => {
                        this.widget.actionModel.startFetchEventOngoing();
                        this.widget.actionModel.fetchEvents(firstPageNumber, "ongoing");
                    }
                );
            }

            return new Container(
                color: CColors.Background,
                child: new SmartRefresher(
                    controller: this._ongoingRefreshController,
                    enablePullDown: true,
                    enablePullUp: this.widget.viewModel.ongoingEvents.Count < this.widget.viewModel.ongoingEventTotal,
                    onRefresh: this._ongoingRefresh,
                    child: ListView.builder(
                        itemExtent: 108,
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: this.widget.viewModel.ongoingEvents.Count,
                        itemBuilder: (cxt, index) => {
                            var eventId = this.widget.viewModel.ongoingEvents[index];
                            var model = this.widget.viewModel.eventsDict[eventId];
                            var placeName = model.placeId.isEmpty()
                                ? null
                                : this.widget.viewModel.placeDict[model.placeId].name;
                            return new EventCard(
                                model,
                                placeName,
                                () => {
                                    this.widget.actionModel.pushToEventDetail(
                                        model.id,
                                        model.mode == "online" ? EventType.online : EventType.offline
                                    );
                                    AnalyticsManager.ClickEnterEventDetail("Home_Future_Event", model.id, model.title,
                                        model.mode);
                                },
                                new ObjectKey(model.id)
                            );
                        }
                    )
                )
            );
        }

        void _ongoingRefresh(bool up) {
            if (up) {
                this.pageNumber = firstPageNumber;
            }
            else {
                this.pageNumber++;
            }

            this.widget.actionModel.fetchEvents(this.pageNumber, "ongoing")
                .Then(() => this._ongoingRefreshController.sendBack(up,
                    up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._ongoingRefreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}