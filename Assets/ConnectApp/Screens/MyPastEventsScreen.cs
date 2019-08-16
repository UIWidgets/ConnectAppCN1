using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
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

    public class MyPastEventsScreen : StatefulWidget {
        public MyPastEventsScreen(
            MyEventsScreenViewModel viewModel = null,
            MyEventsScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly MyEventsScreenViewModel viewModel;
        public readonly MyEventsScreenActionModel actionModel;

        public override State createState() {
            return new _MyPastEventsScreenState();
        }
    }

    public class _MyPastEventsScreenState : AutomaticKeepAliveClientMixin<MyPastEventsScreen> {
        const int firstPageNumber = 1;
        int _pageNumber;
        RefreshController _refreshController;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            this._pageNumber = firstPageNumber;
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchMyPastEvents();
                this.widget.actionModel.fetchMyPastEvents(firstPageNumber);
            });
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var pastEventsList = this.widget.viewModel.pastEventsList;
            if (this.widget.viewModel.pastListLoading && pastEventsList.isEmpty()) {
                return new GlobalLoading();
            }

            if (pastEventsList.Count <= 0) {
                return new BlankView(
                    "还没有参与过的活动",
                    "image/default-event",
                    true,
                    () => {
                        this.widget.actionModel.startFetchMyPastEvents();
                        this.widget.actionModel.fetchMyPastEvents(firstPageNumber);
                    }
                );
            }

            var pastEventTotal = this.widget.viewModel.pastEventTotal;
            var hasMore = pastEventTotal > pastEventsList.Count;
            var itemCount = hasMore ? pastEventsList.Count : pastEventsList.Count + 1;

            return new Container(
                color: CColors.Background,
                child: new CustomScrollbar(
                    new SmartRefresher(
                        controller: this._refreshController,
                        enablePullDown: true,
                        enablePullUp: hasMore,
                        onRefresh: this._onRefresh,
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
            var pastEventsList = this.widget.viewModel.pastEventsList;
            if (index == pastEventsList.Count) {
                return new EndView();
            }

            var model = pastEventsList[index: index];
            var eventType = model.mode == "online" ? EventType.online : EventType.offline;
            var placeName = model.placeId.isEmpty()
                ? null
                : this.widget.viewModel.placeDict[key: model.placeId].name;
            return new EventCard(
                model: model,
                place: placeName,
                () => this.widget.actionModel.pushToEventDetail(arg1: model.id, arg2: eventType),
                index == 0,
                new ObjectKey(value: model.id)
            );
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = firstPageNumber;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.fetchMyPastEvents(this._pageNumber)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}