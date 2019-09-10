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
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventOngoingScreenConnector : StatelessWidget {
        public EventOngoingScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

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
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber: pageNumber, tab: tab))
                    };
                    return new EventOngoingScreen(viewModel: viewModel, actionModel: actionModel);
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
        const int firstPageNumber = 1;
        RefreshController _ongoingRefreshController;
        int _pageNumber = firstPageNumber;
        bool _hasBeenLoadedData;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._hasBeenLoadedData = false;
            this._ongoingRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchEventOngoing();
                this.widget.actionModel.fetchEvents(arg1: firstPageNumber, arg2: eventTab).Then(() => {
                    if (this._hasBeenLoadedData) {
                        return;
                    }

                    this._hasBeenLoadedData = true;
                    this.setState(() => { });
                });
            });
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var ongoingEvents = this.widget.viewModel.ongoingEvents;
            if (!this._hasBeenLoadedData || this.widget.viewModel.eventOngoingLoading && ongoingEvents.isEmpty()) {
                return new Container(
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context)),
                    child: new GlobalLoading()
                );
            }

            if (0 == ongoingEvents.Count) {
                return new Container(
                    padding: EdgeInsets.only(bottom: CConstant.TabBarHeight +
                                                     CCommonUtils.getSafeAreaBottomPadding(context: context)),
                    child: new BlankView(
                        "暂无新活动，看看往期活动吧",
                        "image/default-event",
                        true,
                        () => {
                            this.widget.actionModel.startFetchEventOngoing();
                            this.widget.actionModel.fetchEvents(arg1: firstPageNumber, arg2: eventTab);
                        }
                    )
                );
            }

            var enablePullUp = ongoingEvents.Count < this.widget.viewModel.ongoingEventTotal;
            var itemCount = enablePullUp ? ongoingEvents.Count : ongoingEvents.Count + 1;
            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._ongoingRefreshController,
                        enablePullDown: true,
                        enablePullUp: enablePullUp,
                        onRefresh: this._ongoingRefresh,
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
            var ongoingEvents = this.widget.viewModel.ongoingEvents;
            if (index == ongoingEvents.Count) {
                return new EndView(hasBottomMargin: true);
            }

            var eventId = ongoingEvents[index: index];
            var model = this.widget.viewModel.eventsDict[key: eventId];
            var placeName = model.placeId.isEmpty()
                ? null
                : this.widget.viewModel.placeDict[key: model.placeId].name;
            return new EventCard(
                model: model,
                place: placeName,
                () => this.widget.actionModel.pushToEventDetail(
                    arg1: model.id,
                    model.mode == "online" ? EventType.online : EventType.offline
                ),
                index == 0,
                new ObjectKey(value: model.id)
            );
        }

        void _ongoingRefresh(bool up) {
            if (up) {
                this._pageNumber = firstPageNumber;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.fetchEvents(arg1: this._pageNumber, arg2: eventTab)
                .Then(() => this._ongoingRefreshController.sendBack(up: up,
                    up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._ongoingRefreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }
    }
}