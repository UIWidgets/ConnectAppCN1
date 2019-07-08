using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
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
                        fetchEvents = (pageNumber, tab, mode) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber, tab, mode))
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
        const string eventTab = "completed";
        const string eventMode = "";
        const int firstPageNumber = 1;
        RefreshController _completedRefreshController;
        int pageNumber = firstPageNumber;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._completedRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchEventCompleted();
                this.widget.actionModel.fetchEvents(firstPageNumber, eventTab, eventMode);
            });
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            if (this.widget.viewModel.eventCompletedLoading && this.widget.viewModel.completedEvents.isEmpty()) {
                return new GlobalLoading();
            }

            if (this.widget.viewModel.completedEvents.Count <= 0) {
                return new BlankView(
                    "暂无往期活动，看看新活动吧",
                    "image/default-event",
                    true,
                    () => {
                        this.widget.actionModel.startFetchEventCompleted();
                        this.widget.actionModel.fetchEvents(firstPageNumber, eventTab, eventMode);
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
                                    () => {
                                        AnalyticsManager.ClickEnterEventDetail("Home_Event_Completed", model.id,
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

        void _completedRefresh(bool up) {
            if (up) {
                this.pageNumber = firstPageNumber;
            }
            else {
                this.pageNumber++;
            }

            this.widget.actionModel.fetchEvents(this.pageNumber, eventTab, eventMode)
                .Then(() => this._completedRefreshController.sendBack(up,
                    up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._completedRefreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}