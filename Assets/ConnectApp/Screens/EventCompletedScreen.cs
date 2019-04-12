using ConnectApp.components;
using ConnectApp.components.pull_to_refresh;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens
{
    
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
                        StartFetchEventCompletedAction = () => dispatcher.dispatch(new StartFetchEventCompletedAction()),
                        fetchEvents = (pageNumber, tab) =>
                            dispatcher.dispatch<IPromise>(Actions.fetchEvents(pageNumber, tab))
                    };
                    return new EventCompletedScreen(viewModel, actionModel);
                });
        }
    }
    
    
    public class EventCompletedScreen : StatefulWidget
    {
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
        
        public override State createState()
        {
            return new _EventCompletedScreenState();
        }
    }

    public class _EventCompletedScreenState : AutomaticKeepAliveClientMixin<EventCompletedScreen>
    {
        private const int firstPageNumber = 1;
        private RefreshController _completedRefreshController;
        private int pageNumber = firstPageNumber;
        protected override bool wantKeepAlive
        {
            get => false;
        }

        public override void initState()
        {
            base.initState();
            _completedRefreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.StartFetchEventCompletedAction();
                widget.actionModel.fetchEvents(pageNumber, "completed");
            });
        }

        public override Widget build(BuildContext context)
        {
            if (widget.viewModel.eventCompletedLoading) return new GlobalLoading();
            if (widget.viewModel.completedEvents.isEmpty())
            {
                return new BlankView("暂无即将开始的活动");
            }
            return new SmartRefresher(
                controller: _completedRefreshController,
                enablePullDown: true,
                enablePullUp: widget.viewModel.completedEvents.Count < widget.viewModel.completedEventTotal,
                headerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                footerBuilder: (cxt, mode) => new SmartRefreshHeader(mode),
                onRefresh: _completedRefresh,
                child: ListView.builder(
                    itemExtent:108,
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
        private void _completedRefresh(bool up) {
            if (up)
                pageNumber = 1;
            else
                pageNumber++;
            widget.actionModel.fetchEvents(pageNumber, "completed")
                .Then(() => _completedRefreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => _completedRefreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}