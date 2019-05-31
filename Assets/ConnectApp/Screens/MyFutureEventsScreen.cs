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
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;
using UnityEngine;
using EventType = ConnectApp.Models.State.EventType;

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

    public class MyFutureEventsScreen : StatefulWidget {
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
                this.widget.actionModel.fetchMyFutureEvents(firstPageNumber);
            });
        }

        public override Widget build(BuildContext context) {
            return this._buildMyFutureEvents();
        }

        Widget _buildMyFutureEvents() {
            var data = this.widget.viewModel.futureEventsList;
            if (this.widget.viewModel.futureListLoading && data.isEmpty()) {
                return new GlobalLoading();
            }

            if (data.Count <= 0) {
                return new BlankView("暂无我的即将开始活动", true, () => {
                    this.widget.actionModel.startFetchMyFutureEvents();
                    this.widget.actionModel.fetchMyFutureEvents(firstPageNumber);
                });
            }

            var futureEventTotal = this.widget.viewModel.futureEventTotal;
            var hasMore = futureEventTotal != data.Count;

            return new Container(
                color: CColors.Background,
                child: new SmartRefresher(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: hasMore,
                    onRefresh: this._onRefresh,
                    child: ListView.builder(
                        physics: new AlwaysScrollableScrollPhysics(),
                        itemCount: data.Count,
                        itemBuilder: (cxt, idx) => {
                            var model = data[idx];
                            var eventType = model.mode == "online" ? EventType.online : EventType.offline;
                            var placeName = model.placeId.isEmpty()
                                ? null
                                : this.widget.viewModel.placeDict[model.placeId].name;
                            return new EventCard(
                                model,
                                placeName,
                                () => {
                                    this.widget.actionModel.pushToEventDetail(model.id, eventType);
                                    if (!Application.isEditor) {
                                        AnalyticsManager.ClickEnterEventDetail("My_Future_Event",model.id,model.title,model.type);
                                    }
                                }
                            );
                        }
                    )
                )
            );
        }

        void _onRefresh(bool up) {
            if (up) {
                this._pageNumber = firstPageNumber;
            }
            else {
                this._pageNumber++;
            }

            this.widget.actionModel.fetchMyFutureEvents(this._pageNumber)
                .Then(() => this._refreshController.sendBack(up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up, RefreshStatus.failed));
        }
    }
}