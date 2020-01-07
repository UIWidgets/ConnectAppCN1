using System;
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
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class EventCompletedScreenConnector : StatelessWidget {
        public EventCompletedScreenConnector(
            string mode,
            Key key = null
        ) : base(key: key) {
            this.mode = mode;
        }

        readonly string mode;

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
                        clearEventCompleted = () => dispatcher.dispatch(new ClearEventCompletedAction()),
                        startFetchEventCompleted = () => dispatcher.dispatch(new StartFetchEventCompletedAction()),
                        fetchEvents = (pageNumber, tab) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber: pageNumber, tab: tab, mode: this.mode))
                    };
                    return new EventCompletedScreen(viewModel: viewModel, actionModel: actionModel, mode: this.mode);
                }
            );
        }
    }

    public class EventCompletedScreen : StatefulWidget {
        public EventCompletedScreen(
            EventsScreenViewModel viewModel = null,
            EventsScreenActionModel actionModel = null,
            string mode = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
            this.mode = mode;
        }

        public readonly EventsScreenViewModel viewModel;
        public readonly EventsScreenActionModel actionModel;
        public readonly string mode;

        public override State createState() {
            return new _EventCompletedScreenState();
        }
    }

    public class _EventCompletedScreenState : AutomaticKeepAliveClientMixin<EventCompletedScreen> {
        const string eventTab = "completed";
        const int firstPageNumber = 1;
        RefreshController _completedRefreshController;
        int _pageNumber = firstPageNumber;
        bool _hasBeenLoadedData;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._hasBeenLoadedData = false;
            this._completedRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchEventCompleted();
                this.widget.actionModel.fetchEvents(arg1: firstPageNumber, arg2: eventTab).Then(() => {
                    if (this._hasBeenLoadedData) {
                        return;
                    }

                    this._hasBeenLoadedData = true;
                    this.setState(() => { });
                });
            });
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            if (oldWidget is EventCompletedScreen _oldWidget) {
                if (this.widget.mode != _oldWidget.mode) {
                    Window.instance.run(TimeSpan.FromMilliseconds(0.1f), () => {
                        this._completedRefreshController.animateTo(0, TimeSpan.FromMilliseconds(100), curve: Curves.linear);
                        this.widget.actionModel.clearEventCompleted();
                        this.widget.actionModel.startFetchEventCompleted();
                        this.widget.actionModel.fetchEvents(arg1: firstPageNumber, arg2: eventTab);
                    });
                }
            }
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var completedEvents = this.widget.viewModel.completedEvents;
            if (!this._hasBeenLoadedData || this.widget.viewModel.eventCompletedLoading && completedEvents.isEmpty()) {
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
            return new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._completedRefreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._completedRefresh,
                    hasBottomMargin: true,
                    itemCount: completedEvents.Count,
                    itemBuilder: this._buildEventCard,
                    headerWidget: CustomListViewConstant.defaultHeaderWidget,
                    footerWidget: enablePullUp ? null : new EndView(hasBottomMargin: true)
                )
            );
        }

        Widget _buildEventCard(BuildContext context, int index) {
            var completedEvents = this.widget.viewModel.completedEvents;

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