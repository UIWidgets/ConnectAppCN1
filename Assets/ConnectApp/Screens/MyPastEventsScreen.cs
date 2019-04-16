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
    public class MyPastEventsScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyEventsScreenViewModel>(
                converter: state => new MyEventsScreenViewModel {
                    pastEventsList = state.mineState.pastEventsList,
                    pastListLoading = state.mineState.pastListLoading,
                    pastEventTotal = state.mineState.pastEventTotal,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyEventsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToEventDetail = (id, type) =>
                            dispatcher.dispatch(new MainNavigatorPushToEventDetailAction
                                {eventId = id, eventType = type}),
                        startFetchMyPastEvents = () => dispatcher.dispatch(new StartFetchMyPastEventsAction()),
                        fetchMyPastEvents = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchMyPastEvents(pageNumber))
                    };
                    return new MyPastEventsScreen(viewModel, actionModel);
                }
            );
        }
    }
    
    public class MyPastEventsScreen: StatefulWidget {
        public MyPastEventsScreen(
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
            return new _MyPastEventsScreenState();
        }
    }

    public class _MyPastEventsScreenState : State<MyPastEventsScreen> {
        private const int firstPageNumber = 1;
        private int _pageNumber;
        private RefreshController _refreshController;

        public override void initState() {
            base.initState();
            _pageNumber = firstPageNumber;
            _refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                widget.actionModel.startFetchMyPastEvents();
                widget.actionModel.fetchMyPastEvents(firstPageNumber);
            });
        }
        public override Widget build(BuildContext context) {
            return _buildMyPastEvents();
        }
        
        private Widget _buildMyPastEvents() {
            var data = widget.viewModel.pastEventsList;
            if (widget.viewModel.pastListLoading && data.isEmpty()) return new GlobalLoading();
            if (data.Count <= 0) return new BlankView("暂无我的往期活动");
            var pastEventTotal = widget.viewModel.pastEventTotal;
            var hasMore = pastEventTotal == data.Count;

            return new SmartRefresher(
                controller: _refreshController,
                enablePullDown: true,
                enablePullUp: !hasMore,
                onRefresh: _onRefresh,
                footerConfig: new RefreshConfig(),
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: data.Count,
                    itemBuilder: (cxt, idx) => {
                        var model = data[idx];
                        var eventType = model.mode == "online" ? EventType.online : EventType.offline;
                        var place = model.placeId.isEmpty() ? null : widget.viewModel.placeDict[model.placeId];
                        return new EventCard(
                            model,
                            place,
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
                widget.actionModel.fetchMyPastEvents(_pageNumber)
                    .Then(() => _refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                    .Catch(_ => _refreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}