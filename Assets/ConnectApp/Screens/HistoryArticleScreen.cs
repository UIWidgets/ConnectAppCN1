using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.utils;
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
                        pushToBlock = articleId => {
                            dispatcher.dispatch(new BlockArticleAction {articleId = articleId});
                            dispatcher.dispatch(new DeleteArticleHistoryAction {articleId = articleId});
                        },
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

        readonly HistoryScreenViewModel viewModel;
        readonly HistoryScreenActionModel actionModel;

        readonly CustomDismissibleController _controller = new CustomDismissibleController();

        public override Widget build(BuildContext context) {
            if (this.viewModel.articleHistory.Count == 0) {
                return new BlankView("暂无浏览文章记录");
            }

            return new Container(
                color: CColors.background3,
                child: ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: this.viewModel.articleHistory.Count,
                    itemBuilder: (cxt, index) => {
                        var model = this.viewModel.articleHistory[index];
                        var child = new ArticleCard(
                            model,
                            () => this.actionModel.pushToArticleDetail(model.id),
                            () => ReportManager.showReportView(this.viewModel.isLoggedIn,
                                model.id,
                                ReportType.article, this.actionModel.pushToLogin, this.actionModel.pushToReport,
                                this.actionModel.pushToBlock
                            ),
                            model.fullName,
                            new ObjectKey(model.id)
                        );

                        return CustomDismissible.builder(
                            Key.key(model.id),
                            child,
                            new CustomDismissibleDrawerDelegate(),
                            secondaryActions: new List<Widget> {
                                new GestureDetector(
                                    onTap: () => this.actionModel.deleteArticleHistory(model.id),
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