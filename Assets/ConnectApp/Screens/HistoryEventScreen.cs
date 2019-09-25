using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class HistoryEventScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, HistoryScreenViewModel>(
                converter: state => new HistoryScreenViewModel {
                    eventHistory = state.eventState.eventHistory
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new HistoryScreenActionModel {
                        pushToEventDetail = (id, type) =>
                            dispatcher.dispatch(new MainNavigatorPushToEventDetailAction
                                {eventId = id, eventType = type}),
                        deleteEventHistory = id =>
                            dispatcher.dispatch(new DeleteEventHistoryAction {eventId = id})
                    };
                    return new HistoryEventScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class HistoryEventScreen : StatelessWidget {
        public HistoryEventScreen(
            HistoryScreenViewModel viewModel = null,
            HistoryScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        readonly HistoryScreenViewModel viewModel;
        readonly HistoryScreenActionModel actionModel;

        readonly CustomDismissibleController _controller = new CustomDismissibleController();

        public override Widget build(BuildContext context) {
            if (this.viewModel.eventHistory.Count == 0) {
                return new BlankView("哎呀，还没有任何活动记录", "image/default-history");
            }

            return new Container(
                color: CColors.Background,
                child: new CustomListView(
                    itemCount: this.viewModel.eventHistory.Count,
                    itemBuilder: this._buildEventCard,
                    headerWidget: CustomListViewConstant.defaultHeaderWidget,
                    hasRefresh: false
                )
            );
        }

        Widget _buildEventCard(BuildContext context, int index) {
            var model = this.viewModel.eventHistory[index: index];
            var eventType = model.mode == "online" ? EventType.online : EventType.offline;
            return CustomDismissible.builder(
                Key.key(value: model.id),
                new EventCard(
                    model: model,
                    place: model.place,
                    () => this.actionModel.pushToEventDetail(arg1: model.id, arg2: eventType),
                    new ObjectKey(value: model.id)
                ),
                new CustomDismissibleDrawerDelegate(),
                secondaryActions: new List<Widget> {
                    new DeleteActionButton(
                        80,
                        onTap: () => this.actionModel.deleteEventHistory(obj: model.id)
                    )
                },
                controller: this._controller
            );
        }
    }
}