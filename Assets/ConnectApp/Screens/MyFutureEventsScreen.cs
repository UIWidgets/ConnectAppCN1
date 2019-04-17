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

namespace ConnectApp.screens {
    public class MyFutureEventsScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyEventsScreenViewModel>(
                converter: state => new MyEventsScreenViewModel {
                    futureListLoading = state.mineState.futureListLoading,
                    futureEventsList = state.mineState.futureEventsList,
                    futureEventTotal = state.mineState.futureEventTotal,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyEventsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToEventDetail = (id, type) =>
                            dispatcher.dispatch(new MainNavigatorPushToEventDetailAction
                                {eventId = id, eventType = type}),
                        startFetchMyFutureEvents = () => dispatcher.dispatch(new StartFetchMyFutureEventsAction()),
                        fetchMyFutureEvents = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchMyFutureEvents(pageNumber))
                    };
                    return new MyFutureEventsScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class MyFutureEventsScreen: StatefulWidget {
        public MyFutureEventsScreen(
            MyEventsScreenViewModel viewModel = null,
            MyEventsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly MyEventsScreenViewModel viewModel;
        public readonly MyEventsScreenActionModel actionModel;
        public override State createState() {
            return new _MyFutureEventsScreenState();
        }
    }
    public class _MyFutureEventsScreenState : State<MyFutureEventsScreen> {
        private const int firstPageNumber = 1;
        private int _pageNumber;
        private RefreshController _refreshController;

        public override void initState() {
            base.initState();
            _pageNumber = firstPageNumber;
            _refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchMyFutureEvents();
                widget.actionModel.fetchMyFutureEvents(firstPageNumber);
            });
        }
        public override Widget build(BuildContext context) {
            return _buildMyFutureEvents();
        }
        
        private Widget _buildMyFutureEvents() {
            var data = widget.viewModel.futureEventsList;
            if (widget.viewModel.futureListLoading && data.isEmpty()) return new GlobalLoading();
            if (data.Count <= 0) return new BlankView("暂无我的即将开始活动");
            var futureEventTotal = widget.viewModel.futureEventTotal;
            var hasMore = futureEventTotal != data.Count;

            return new SmartRefresher(
                controller: _refreshController,
                enablePullDown: true,
                enablePullUp: hasMore,
                onRefresh: _onRefresh,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: data.Count,
                    itemBuilder: (cxt, idx) => {
                        var model = data[idx];
                        var eventType = model.mode == "online" ? EventType.online : EventType.offline;
                        var place = model.placeId.isEmpty() ? new Place() : widget.viewModel.placeDict[model.placeId];
                        return new EventCard(
                            model,
                            place.name,
                            () => widget.actionModel.pushToEventDetail(model.id, eventType)
                        );
                    }
                )
            );
        }
        
        private void _onRefresh(bool up) {
            if (up)
                _pageNumber = firstPageNumber;
            else
                _pageNumber++;
                widget.actionModel.fetchMyFutureEvents(_pageNumber)
                    .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                    .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}