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
    public class HistoryArticleScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, HistoryScreenViewModel>(
                converter: state => new HistoryScreenViewModel {
                    articleHistory = state.articleState.articleHistory,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict,
                    placeDict = state.placeState.placeDict
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new HistoryScreenActionModel {
                        pushToArticleDetail = id =>
                            dispatcher.dispatch(new MainNavigatorPushToArticleDetailAction {articleId = id}),
                        pushToReport = (reportId, reportType) => dispatcher.dispatch(
                            new MainNavigatorPushToReportAction {
                                reportId = reportId,
                                reportType = reportType
                            }
                        ),
                        deleteArticleHistory = id =>
                            dispatcher.dispatch(new DeleteArticleHistoryAction {articleId = id})
                    };
                    return new HistoryArticleScreen(viewModel, actionModel);
                }
            );
        }
    }
    public class HistoryArticleScreen : StatefulWidget {
        public HistoryArticleScreen(
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
            return new _HistoryArticleScreenState();
        }
    }

    public class _HistoryArticleScreenState : State<HistoryArticleScreen> {

        public override Widget build(BuildContext context) {
            if (widget.viewModel.articleHistory.Count == 0) return new BlankView("暂无浏览文章记录");

            return ListView.builder(
                physics: new AlwaysScrollableScrollPhysics(),
                itemCount: widget.viewModel.articleHistory.Count,
                itemBuilder: (cxt, index) => {
                    var model = widget.viewModel.articleHistory[index];
                    Widget child;
                    if (model.ownerType == OwnerType.user.ToString()) {
                        var _user = new User();
                        if (widget.viewModel.userDict.ContainsKey(model.userId))
                            _user = widget.viewModel.userDict[model.userId];
                        child = ArticleCard.User(
                            model,
                            () => widget.actionModel.pushToArticleDetail(model.id),
                            () => {
                                ActionSheetUtils.showModalActionSheet(new ActionSheet(
                                    items: new List<ActionSheetItem> {
                                        new ActionSheetItem(
                                            "举报",
                                            ActionType.destructive,
                                            () => widget.actionModel.pushToReport(model.id, ReportType.article)
                                        ),
                                        new ActionSheetItem("取消", ActionType.cancel)
                                    }
                                ));
                            },
                            new ObjectKey(model.id),
                            _user
                        );
                    }
                    else {
                        var _team = new Team();
                        if (widget.viewModel.teamDict.ContainsKey(model.teamId))
                            _team = widget.viewModel.teamDict[model.teamId];
                        child = ArticleCard.Team(
                            model,
                            () =>
                                widget.actionModel.pushToArticleDetail(model.id),
                            () => {
                                ActionSheetUtils.showModalActionSheet(new ActionSheet(
                                    items: new List<ActionSheetItem> {
                                        new ActionSheetItem(
                                            "举报",
                                            ActionType.destructive,
                                            () => widget.actionModel.pushToReport(model.id, ReportType.article)
                                        ),
                                        new ActionSheetItem("取消", ActionType.cancel)
                                    }
                                ));
                            },
                            new ObjectKey(model.id),
                            _team
                        );
                    }

                    return new Dismissible(
                        Key.key(model.id),
                        child,
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
                        onDismissed: direction => widget.actionModel.deleteArticleHistory(model.id)
                    );
                }
            );
        }
    }
}