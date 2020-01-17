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
    public class MyFutureEventsScreenConnector : StatelessWidget {
        public MyFutureEventsScreenConnector(
            string mode,
            Key key = null
        ) : base(key: key) {
            this.mode = mode;
        }

        readonly string mode;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, MyEventsScreenViewModel>(
                converter: state => new MyEventsScreenViewModel {
                    futureListLoading = state.mineState.futureListLoading,
                    futureEventIds = state.mineState.futureEventIds,
                    futureEventTotal = state.mineState.futureEventTotal,
                    eventsDict = state.eventState.eventsDict,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new MyEventsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToEventDetail = (id, type) =>
                            dispatcher.dispatch(new MainNavigatorPushToEventDetailAction
                                {eventId = id, eventType = type}),
                        clearMyFutureEvents = () => dispatcher.dispatch(new ClearMyFutureEventsAction()),
                        startFetchMyFutureEvents = () => dispatcher.dispatch(new StartFetchMyFutureEventsAction()),
                        fetchMyFutureEvents = pageNumber =>
                            dispatcher.dispatch<IPromise>(Actions.fetchMyFutureEvents(pageNumber: pageNumber, mode: this.mode))
                    };
                    return new MyFutureEventsScreen(viewModel: viewModel, actionModel: actionModel, mode: this.mode);
                }
            );
        }
    }

    public class MyFutureEventsScreen : StatefulWidget {
        public MyFutureEventsScreen(
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
            return new _MyFutureEventsScreenState();
        }
    }

    public class _MyFutureEventsScreenState : AutomaticKeepAliveClientMixin<MyFutureEventsScreen> {
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
                this.widget.actionModel.startFetchMyFutureEvents();
                this.widget.actionModel.fetchMyFutureEvents(arg: firstPageNumber);
            });
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget: oldWidget);
            if (oldWidget is MyFutureEventsScreen _oldWidget) {
                if (this.widget.mode != _oldWidget.mode) {
                    Window.instance.run(TimeSpan.FromMilliseconds(0.1f), () => {
                        this._refreshController.animateTo(0, TimeSpan.FromMilliseconds(100), curve: Curves.linear);
                        this.widget.actionModel.clearMyFutureEvents();
                        this.widget.actionModel.startFetchMyFutureEvents();
                        this.widget.actionModel.fetchMyFutureEvents(arg: firstPageNumber);
                    });
                }
            }
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            var futureEventIds = this.widget.viewModel.futureEventIds;
            if (this.widget.viewModel.futureListLoading && futureEventIds.isEmpty()) {
                return new GlobalLoading();
            }

            if (futureEventIds.Count <= 0) {
                return new BlankView(
                    "还没有即将开始的活动",
                    "image/default-event",
                    true,
                    () => {
                        this.widget.actionModel.startFetchMyFutureEvents();
                        this.widget.actionModel.fetchMyFutureEvents(arg: firstPageNumber);
                    }
                );
            }

            var futureEventTotal = this.widget.viewModel.futureEventTotal;
            var enablePullUp = futureEventTotal > futureEventIds.Count;

            return new Container(
                color: CColors.Background,
                child: new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: futureEventIds.Count,
                    itemBuilder: this._buildEventCard,
                    headerWidget: CustomListViewConstant.defaultHeaderWidget,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                )
            );
        }

        Widget _buildEventCard(BuildContext context, int index) {
            var futureEventIds = this.widget.viewModel.futureEventIds;

            var futureEventId = futureEventIds[index: index];
            if (!this.widget.viewModel.eventsDict.ContainsKey(key: futureEventId)) {
                return new Container();
            }

            var model = this.widget.viewModel.eventsDict[key: futureEventId];
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

            this.widget.actionModel.fetchMyFutureEvents(arg: this._pageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }
    }
}