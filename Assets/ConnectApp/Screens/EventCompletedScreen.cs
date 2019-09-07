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
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventCompletedScreenConnector : StatelessWidget {
        public EventCompletedScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

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
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber: pageNumber, tab: tab))
                    };
                    return new EventCompletedScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class EventCompletedScreen : StatefulWidget {
        public EventCompletedScreen(
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
            return new _EventCompletedScreenState();
        }
    }

    public class _EventCompletedScreenState : AutomaticKeepAliveClientMixin<EventCompletedScreen> {
        const string eventTab = "completed";
        const int firstPageNumber = 1;
        RefreshController _completedRefreshController;
        int _pageNumber = firstPageNumber;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._completedRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchEventCompleted();
                this.widget.actionModel.fetchEvents(arg1: firstPageNumber, arg2: eventTab);
            });
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var completedEvents = this.widget.viewModel.completedEvents;
            if (this.widget.viewModel.eventCompletedLoading && completedEvents.isEmpty()) {
                return new Container(
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context)),
                    child: new GlobalLoading()
                );
            }

            if (0 == completedEvents.Count) {
                return new Container(
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context)),
                    child: new BlankView(
                        "暂无往期活动，看看新活动吧",
                        "image/default-event",
                        true,
                        () => {
                            this.widget.actionModel.startFetchEventCompleted();
                            this.widget.actionModel.fetchEvents(arg1: firstPageNumber, arg2: eventTab);
                        }
                    )
                );
            }

            var enablePullUp = completedEvents.Count < this.widget.viewModel.completedEventTotal;
            var itemCount = enablePullUp ? completedEvents.Count : completedEvents.Count + 1;
            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._completedRefreshController,
                        enablePullDown: true,
                        enablePullUp: enablePullUp,
                        onRefresh: this._completedRefresh,
                        hasBottomMargin: true,
                        child: ListView.builder(
                            physics: new AlwaysScrollableScrollPhysics(),
                            itemCount: itemCount,
                            itemBuilder: this._buildEventCard
                        )
                    )
                )
            );
        }

        Widget _buildEventCard(BuildContext context, int index) {
            var completedEvents = this.widget.viewModel.completedEvents;
            if (index == completedEvents.Count) {
                return new EndView(hasBottomMargin: true);
            }

            var eventId = completedEvents[index: index];
            var model = this.widget.viewModel.eventsDict[key: eventId];
            var place = model.placeId.isEmpty()
                ? new Place()
                : this.widget.viewModel.placeDict[key: model.placeId];
            return new EventCard(
                model: model,
                place: place.name,
                () => this.widget.actionModel.pushToEventDetail(
                    arg1: model.id,
                    model.mode == "online" ? EventType.online : EventType.offline
                ),
                index == 0,
                new ObjectKey(value: model.id)
            );
        }

        void _completedRefresh(bool up) {
            if (up) {
                this._pageNumber = firstPageNumber;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.fetchEvents(arg1: this._pageNumber, arg2: eventTab)
                .Then(() => this._completedRefreshController.sendBack(up: up,
                    up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._completedRefreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }
    }
}