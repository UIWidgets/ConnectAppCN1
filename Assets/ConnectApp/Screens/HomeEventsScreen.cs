using System.Collections.Generic;
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
    public class HomeEventsScreenConnector : StatelessWidget {
        public HomeEventsScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EventsScreenViewModel>(
                converter: state => new EventsScreenViewModel {
                    homeEventLoading = state.eventState.homeEventsLoading,
                    homeEvents = state.eventState.homeEvents,
                    homeEventHasMore = state.eventState.homeEventHasMore,
                    homeEventPageNumber = state.eventState.homeEventPageNumber,
                    eventsDict = state.eventState.eventsDict,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new EventsScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToEventDetail = (eventId, eventType) => dispatcher.dispatch(
                            new MainNavigatorPushToEventDetailAction {
                                eventId = eventId, eventType = eventType
                            }),
                        startFetchHomeEvent = () => dispatcher.dispatch(new StartFetchHomeEventsAction()),
                        fetchHomeEvents = page => dispatcher.dispatch<IPromise>(Actions.fetchHomeEvents(page: page))
                    };
                    return new HomeEventsScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class HomeEventsScreen : StatefulWidget {
        public HomeEventsScreen(
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
            return new _HomeEventsScreenState();
        }
    }

    public class _HomeEventsScreenState : State<HomeEventsScreen> {
        const int firstPageNumber = 1;
        int homeEventPageNumber = firstPageNumber;
        RefreshController _refreshController;

        public override void initState() {
            base.initState();
            this._refreshController = new RefreshController();
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                this.widget.actionModel.startFetchHomeEvent();
                this.widget.actionModel.fetchHomeEvents(arg: firstPageNumber);
            });
        }

        void _onRefresh(bool up) {
            this.homeEventPageNumber = up ? firstPageNumber : this.homeEventPageNumber + 1;
            this.widget.actionModel.fetchHomeEvents(arg: this.homeEventPageNumber)
                .Then(() => this._refreshController.sendBack(up: up, up ? RefreshStatus.completed : RefreshStatus.idle))
                .Catch(_ => this._refreshController.sendBack(up: up, mode: RefreshStatus.failed));
        }

        public override Widget build(BuildContext context) {
            var homeEvents = this.widget.viewModel.homeEvents;
            Widget content;
            if (this.widget.viewModel.homeEventLoading && homeEvents.isEmpty()) {
                content = new GlobalLoading();
            }
            else if (homeEvents.Count == 0) {
                content = new BlankView(
                    "暂无推荐活动",
                    "image/default-event",
                    true,
                    () => {
                        this.widget.actionModel.startFetchHomeEvent();
                        this.widget.actionModel.fetchHomeEvents(arg: firstPageNumber);
                    }
                );
            }
            else {
                var enablePullUp = this.widget.viewModel.homeEventHasMore;
                content = new CustomListView(
                    controller: this._refreshController,
                    enablePullDown: true,
                    enablePullUp: enablePullUp,
                    onRefresh: this._onRefresh,
                    itemCount: homeEvents.Count,
                    itemBuilder: this._buildEventCard,
                    footerWidget: enablePullUp ? null : CustomListViewConstant.defaultFooterWidget
                );
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(child: content)
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text(
                    "热门活动",
                    style: CTextStyle.H2
                ),
                onBack: () => this.widget.actionModel.mainRouterPop()
            );
        }

        Widget _buildEventCard(BuildContext context, int index) {
            var homeEvents = this.widget.viewModel.homeEvents;

            var eventId = homeEvents[index: index];
            if (!this.widget.viewModel.eventsDict.ContainsKey(key: eventId)) {
                return new Container();
            }

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
                new ObjectKey(value: model.id)
            );
        }
    }
}