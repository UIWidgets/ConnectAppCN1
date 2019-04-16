using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
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
    public class HistoryArticleScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, HistoryScreenViewModel>(
                converter: state => new HistoryScreenViewModel {
                    articleHistory = state.articleState.articleHistory,
                    userDict = state.userState.userDict,
                    teamDict = state.teamState.teamDict,
                    placeDict = state.placeState.placeDict,
                    isLoggedIn = state.loginState.isLoggedIn
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new HistoryScreenActionModel {
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
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
    public class HistoryArticleScreen : StatelessWidget {
        public HistoryArticleScreen(
            HistoryScreenViewModel viewModel = null,
            HistoryScreenActionModel actionModel = null,
            Key key = null
        ) : base(key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        private readonly HistoryScreenViewModel viewModel;
        private readonly HistoryScreenActionModel actionModel;
        
        private readonly CustomDismissibleController _controller = new CustomDismissibleController();

        public override Widget build(BuildContext context) {
            if (viewModel.articleHistory.Count == 0) return new BlankView("暂无浏览文章记录");

            return ListView.builder(
                physics: new AlwaysScrollableScrollPhysics(),
                itemCount: viewModel.articleHistory.Count,
                itemBuilder: (cxt, index) => {
                    var model = viewModel.articleHistory[index];
                    Widget child;
                    if (model.ownerType == OwnerType.user.ToString()) {
                        var _user = new User();
                        if (viewModel.userDict.ContainsKey(model.userId))
                            _user = viewModel.userDict[model.userId];
                        child = ArticleCard.User(
                            model,
                            () => actionModel.pushToArticleDetail(model.id),
                            () => {
                                if (!viewModel.isLoggedIn) {
                                    actionModel.pushToLogin();
                                    return;
                                }
                                ActionSheetUtils.showModalActionSheet(new ActionSheet(
                                    items: new List<ActionSheetItem> {
                                        new ActionSheetItem(
                                            "举报",
                                            ActionType.normal,
                                            () => actionModel.pushToReport(model.id, ReportType.article)
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
                        if (viewModel.teamDict.ContainsKey(model.teamId))
                            _team = viewModel.teamDict[model.teamId];
                        child = ArticleCard.Team(
                            model,
                            () =>
                                actionModel.pushToArticleDetail(model.id),
                            () => {
                                if (!viewModel.isLoggedIn) {
                                    actionModel.pushToLogin();
                                    return;
                                }
                                ActionSheetUtils.showModalActionSheet(new ActionSheet(
                                    items: new List<ActionSheetItem> {
                                        new ActionSheetItem(
                                            "举报",
                                            ActionType.normal,
                                            () => actionModel.pushToReport(model.id, ReportType.article)
                                        ),
                                        new ActionSheetItem("取消", ActionType.cancel)
                                    }
                                ));
                            },
                            new ObjectKey(model.id),
                            _team
                        );
                    }

                    return CustomDismissible.builder(
                        Key.key(model.id),
                        child,
                        new CustomDismissibleDrawerDelegate(),
                        secondaryActions: new List<Widget> {
                            new GestureDetector(
                                onTap: () => actionModel.deleteArticleHistory(model.id),
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
                        controller: _controller
                    );
                }
            );
        }
    }
}