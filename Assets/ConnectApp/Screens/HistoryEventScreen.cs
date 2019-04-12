using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class HistoryEventScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, HistoryScreenViewModel>(
                converter: state => new HistoryScreenViewModel {
                    eventHistory = state.eventState.eventHistory,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict,
                    placeDict = state.placeState.placeDict
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
    public class HistoryEventScreen : StatefulWidget {
        public HistoryEventScreen(
            HistoryScreenViewModel viewModel = null,
            HistoryScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly HistoryScreenViewModel viewModel;
        public readonly HistoryScreenActionModel actionModel;

        public override State createState() {
            return new _HistoryEventScreenState();
        }
    }

    public class _HistoryEventScreenState : State<HistoryEventScreen> {

        public override Widget build(BuildContext context) {
            if (widget.viewModel.eventHistory.Count == 0) return new BlankView("暂无浏览活动记录");
            return ListView.builder(
                physics: new AlwaysScrollableScrollPhysics(),
                itemCount: widget.viewModel.eventHistory.Count,
                itemExtent: 108,
                itemBuilder: (cxt, index) => {
                    var model = widget.viewModel.eventHistory[index];
                    var eventType = model.mode == "online" ? EventType.online : EventType.offline;
                    var place = model.placeId.isEmpty() ? null : widget.viewModel.placeDict[model.placeId];
                    return new Dismissible(
                        Key.key(model.id),
                        new EventCard(
                            model,
                            place,
                            () => widget.actionModel.pushToEventDetail(model.id, eventType)
                        ),
                        new Container(
                            color: CColors.Error,
                            padding: EdgeInsets.symmetric(horizontal: 16),
                            child: new Row(
                                mainAxisAlignment: MainAxisAlignment.end,
                                children: new List<Widget> {
                                    new Text(
                                        "删除",
                                        style: CTextStyle.PLargeWhite
                                    )
                                }
                            )
                        ),
                        direction: DismissDirection.endToStart,
                        onDismissed: direction => widget.actionModel.deleteEventHistory(model.id)
                    );
                }
            );
        }
    }
}