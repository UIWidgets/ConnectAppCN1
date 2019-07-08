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
                        fetchEvents = (pageNumber, tab, mode) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber, tab, mode))
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
        ) : base(key: key) {
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
        const string eventTab = "ongoing";
        const string eventMode = "offline";
        const int firstPageNumber = 1;
        RefreshController _ongoingRefreshController;
        int pageNumber = firstPageNumber;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._ongoingRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchEventOngoing();
                this.widget.actionModel.fetchEvents(firstPageNumber, eventTab, eventMode);
            });
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            if (this.widget.viewModel.eventOngoingLoading && this.widget.viewModel.ongoingEvents.isEmpty()) {
                return new GlobalLoading();
            }

            if (this.widget.viewModel.ongoingEvents.Count <= 0) {
                return new BlankView(
                    "暂无新活动，看看往期活动吧",
                    "image/default-event",
                    true,
                    () => {
                        this.widget.actionModel.startFetchEventOngoing();
                        this.widget.actionModel.fetchEvents(firstPageNumber, eventTab, eventMode);
                    }
                );
            }

            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._ongoingRefreshController,
                        enablePullDown: true,
                        enablePullUp: this.widget.viewModel.ongoingEvents.Count <
                                      this.widget.viewModel.ongoingEventTotal,
                        onRefresh: this._ongoingRefresh,
                        child: ListView.builder(
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
                                        AnalyticsManager.ClickEnterEventDetail("Home_Event_Ongoing", model.id,
                                            model.title, model.mode);
                                        this.widget.actionModel.pushToEventDetail(
                                            model.id,
                                            model.mode == "online" ? EventType.online : EventType.offline
                                        );
                                    },
                                    new ObjectKey(model.id),
                                    index == 0
                                );
                            }
                        )
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

            this.widget.actionModel.fetchEvents(this.pageNumber, eventTab, eventMode)
                .Then(() => this._ongoingRefreshController.sendBack(up,
                    up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._ongoingRefreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}