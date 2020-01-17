using System;
using ConnectApp.Components;
using ConnectApp.Components.pull_to_refresh;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class MyPastEventsScreenConnector : StatelessWidget {
        public MyPastEventsScreenConnector(
            string mode,
            Key key = null
        ) : base(key: key) {
            this.mode = mode;
        }

        readonly string mode;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyEventsScreenViewModel>(
                converter: state => new MyEventsScreenViewModel {
                    pastEventIds = state.mineState.pastEventIds,
                    pastListLoading = state.mineState.pastListLoading,
                    pastEventTotal = state.mineState.pastEventTotal,
                    eventsDict = state.eventState.eventsDict,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyEventsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToEventDetail = (id, type) =>
                            dispatcher.dispatch(new MainNavigatorPushToEventDetailAction
                                {eventId = id, eventType = type}),
                        clearMyPastEvents = () => dispatcher.dispatch(new ClearMyPastEventsAction()),
                        startFetchMyPastEvents = () => dispatcher.dispatch(new StartFetchMyPastEventsAction()),
                        fetchMyPastEvents = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchMyPastEvents(pageNumber: pageNumber, mode: this.mode))
                    };
                    return new MyPastEventsScreen(viewModel: viewModel, actionModel: actionModel, mode: this.mode);
                }
            );
        }
    }

    public class MyPastEventsScreen : StatefulWidget {
        public MyPastEventsScreen(
            MyEventsScreenViewModel viewModel = null,
            MyEventsScreenActionModel actionModel = null,
            string mode = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
            this.mode = mode;
        }

        public readonly MyEventsScreenViewModel viewModel;
        public readonly MyEventsScreenActionModel actionModel;
        public readonly string mode;

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
                this.widget.actionModel.fetchMyPastEvents(arg: firstPageNumber);
            });
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            if (oldWidget is MyPastEventsScreen _oldWidget) {
                if (this.widget.mode != _oldWidget.mode) {
                    Window.instance.run(TimeSpan.FromMilliseconds(0.1f), () => {
                        this._refreshController.animateTo(0, TimeSpan.FromMilliseconds(100), curve: Curves.linear);
                        this.widget.actionModel.clearMyPastEvents();
                        this.widget.actionModel.startFetchMyPastEvents();
                        this.widget.actionModel.fetchMyPastEvents(arg: firstPageNumber);
                    });
                }
            }
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var pastEventIds = this.widget.viewModel.pastEventIds;
            if (this.widget.viewModel.pastListLoading && pastEventIds.isEmpty()) {
                return new GlobalLoading();
            }

            if (pastEventIds.Count <= 0) {
                return new BlankView(
                    "还没有参与过的活动",
                    "image/default-event",
                    true,
                    () => {
                        this.widget.actionModel.startFetchMyPastEvents();
                        this.widget.actionModel.fetchMyPastEvents(arg: firstPageNumber);
                    }
                );
            }

            var pastEventTotal = this.widget.viewModel.pastEventTotal;
            var enablePullUp = pastEventTotal > pastEventIds.Count;

            return new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: pastEventIds.Count,
                    itemBuilder: this._buildEventCard,
                    headerWidget: CustomListViewConstant.defaultHeaderWidget,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                )
            );
        }

        Widget _buildEventCard(BuildContext context, int index) {
            var pastEventIds = this.widget.viewModel.pastEventIds;

            var pastEventId = pastEventIds[index: index];
            if (!this.widget.viewModel.eventsDict.ContainsKey(key: pastEventId)) {
                return new Container();
            }

            var model = this.widget.viewModel.eventsDict[key: pastEventId];
            var eventType = model.mode == "online" ? EventType.online : EventType.offline;
            var placeName = model.placeId.isEmpty()
                ? null
                : this.widget.viewModel.placeDict[key: model.placeId].name;
            return new EventCard(
                model: model,
                place: placeName,
                () => this.widget.actionModel.pushToEventDetail(arg1: model.id, arg2: eventType),
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

            this.widget.actionModel.fetchMyPastEvents(arg: this._pageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }
    }
}