using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventCompletedScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventsScreenViewModel>(
                converter: state => new EventsScreenViewModel {
                    eventCompletedLoading = state.eventState.eventsCompletedLoading,
                    completedEvents = state.eventState.completedEvents,
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
                        startFetchEventCompleted = () => dispatcher.dispatch(new StartFetchEventCompletedAction()),
                        fetchEvents = (pageNumber, tab) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber, tab))
                    };
                    return new EventCompletedScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class EventCompletedScreen : StatefulWidget {
        public EventCompletedScreen(
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
            return new _EventCompletedScreenState();
        }
    }

    public class _EventCompletedScreenState : AutomaticKeepAliveClientMixin<EventCompletedScreen> {
        const int firstPageNumber = 1;
        RefreshController _completedRefreshController;
        int pageNumber = firstPageNumber;
        string _loginSubId;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._completedRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchEventCompleted();
                this.widget.actionModel.fetchEvents(firstPageNumber, "completed");
            });
//            _loginSubId = EventBus.subscribe(EventBusConstant.login_success, args => {
//                widget.actionModel.startFetchEventCompleted();
//                widget.actionModel.fetchEvents(firstPageNumber, "completed");
//            });
        }

        public override void dispose() {
//            EventBus.unSubscribe(EventBusConstant.login_success, _loginSubId);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            if (this.widget.viewModel.eventCompletedLoading && this.widget.viewModel.completedEvents.isEmpty()) {
                return new GlobalLoading();
            }

            if (this.widget.viewModel.completedEvents.Count <= 0) {
                return new BlankView(
                    "暂无往期活动",
                    true,
                    () => {
                        this.widget.actionModel.startFetchEventCompleted();
                        this.widget.actionModel.fetchEvents(firstPageNumber, "completed");
                    }
                );
            }

            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._completedRefreshController,
                        enablePullDown: true,
                        enablePullUp: this.widget.viewModel.completedEvents.Count <
                                      this.widget.viewModel.completedEventTotal,
                        onRefresh: this._completedRefresh,
                        child: ListView.builder(
                            itemExtent: 108,
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: this.widget.viewModel.completedEvents.Count,
                            itemBuilder: (cxt, index) => {
                                var eventId = this.widget.viewModel.completedEvents[index];
                                var model = this.widget.viewModel.eventsDict[eventId];
                                var place = model.placeId.isEmpty()
                                    ? new Place()
                                    : this.widget.viewModel.placeDict[model.placeId];
                                return new EventCard(
                                    model,
                                    place.name,
                                    () => this.widget.actionModel.pushToEventDetail(
                                        model.id,
                                        model.mode == "online" ? EventType.online : EventType.offline
                                    ),
                                    new ObjectKey(model.id)
                                );
                            }
                        )
                    )
                )
            );
        }

        void _completedRefresh(bool up) {
            if (up) {
                this.pageNumber = firstPageNumber;
            }
            else {
                this.pageNumber++;
            }

            this.widget.actionModel.fetchEvents(this.pageNumber, "completed")
                .Then(() => this._completedRefreshController.sendBack(up,
                    up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._completedRefreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}