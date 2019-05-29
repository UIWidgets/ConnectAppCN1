using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
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
                    return new HistoryEventScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class HistoryEventScreen : StatelessWidget {
        public HistoryEventScreen(
            HistoryScreenViewModel viewModel = null,
            HistoryScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        readonly HistoryScreenViewModel viewModel;
        readonly HistoryScreenActionModel actionModel;

        readonly CustomDismissibleController _controller = new CustomDismissibleController();

        public override Widget build(BuildContext context) {
            if (this.viewModel.eventHistory.Count == 0) {
                return new BlankView("暂无浏览活动记录");
            }

            return new Container(
                color: CColors.background3,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: this.viewModel.eventHistory.Count,
                    itemExtent: 108,
                    itemBuilder: (cxt, index) => {
                        var model = this.viewModel.eventHistory[index];
                        var eventType = model.mode == "online" ? EventType.online : EventType.offline;
                        return CustomDismissible.builder(
                            Key.key(model.id),
                            new EventCard(
                                model,
                                model.place,
                                () => this.actionModel.pushToEventDetail(model.id, eventType)
                            ),
                            new CustomDismissibleDrawerDelegate(),
                            secondaryActions: new List<Widget> {
                                new GestureDetector(
                                    onTap: () => this.actionModel.deleteEventHistory(model.id),
                                    child: new Container(
                                        color: CColors.Separator2,
                                        width: 80,
                                        alignment: Alignment.center,
                                        child: new Container(
                                            width: 44,
                                            height: 44,
                                            alignment: Alignment.center,
                                            decoration: new BoxDecoration(
                                                CColors.White,
                                                borderRadius: BorderRadius.circular(22)
                                            ),
                                            child: new Icon(Icons.delete_outline, size: 28, color: CColors.Error)
                                        )
                                    )
                                )
                            },
                            controller: this._controller
                        );
                    }
                )
            );
        }
    }
}