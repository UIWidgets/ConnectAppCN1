using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class HistoryScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, HistoryScreenViewModel>(
                converter: state => new HistoryScreenViewModel {
                    articleHistory = state.articleState.articleHistory,
                    eventHistory = state.eventState.eventHistory
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new HistoryScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        deleteAllArticleHistory = () => dispatcher.dispatch(new DeleteAllArticleHistoryAction()),
                        deleteAllEventHistory = () => dispatcher.dispatch(new DeleteAllEventHistoryAction())
                    };
                    return new HistoryScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class HistoryScreen : StatefulWidget {
        public HistoryScreen(
            HistoryScreenViewModel viewModel = null,
            HistoryScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly HistoryScreenViewModel viewModel;
        public readonly HistoryScreenActionModel actionModel;

        public override State createState() {
            return new _HistoryScreenState();
        }
    }

    class _HistoryScreenState : State<HistoryScreen> {
        int _selectedIndex;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._selectedIndex = 0;
        }

        void _deleteAllHistory() {
            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "确定删除浏览历史？",
                items: new List<ActionSheetItem> {
                    new ActionSheetItem(
                        "删除",
                        type: ActionType.destructive,
                        () => {
                            if (this._selectedIndex == 0) {
                                this.widget.actionModel.deleteAllArticleHistory();
                            }
                            else {
                                this.widget.actionModel.deleteAllEventHistory();
                            }
                        }),
                    new ActionSheetItem("取消", type: ActionType.cancel)
                }
            ));
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Expanded(
                                    child: this._buildContentView()
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomNavigationBar(
                new Text(
                    "浏览历史",
                    style: CTextStyle.H2
                ),
                new List<Widget> {
                    this._buildDeleteButton()
                },
                padding: EdgeInsets.only(16, bottom: 8),
                onBack: () => this.widget.actionModel.mainRouterPop()
            );
        }

        Widget _buildDeleteButton() {
            if (this._selectedIndex == 0) {
                var articleHistory = this.widget.viewModel.articleHistory;
                if (articleHistory.Count > 0) {
                    return new CustomButton(
                        padding: EdgeInsets.symmetric(12, 16),
                        onPressed: this._deleteAllHistory,
                        child: new Icon(
                            icon: Icons.delete_outline,
                            size: 28,
                            color: CColors.Icon
                        )
                    );
                }
            }

            if (this._selectedIndex == 1) {
                var eventHistory = this.widget.viewModel.eventHistory;
                if (eventHistory.Count > 0) {
                    return new CustomButton(
                        padding: EdgeInsets.symmetric(12, 16),
                        onPressed: this._deleteAllHistory,
                        child: new Icon(
                            icon: Icons.delete_outline,
                            size: 28,
                            color: CColors.Icon
                        )
                    );
                }
            }

            return new Container();
        }

        Widget _buildContentView() {
            return new CustomSegmentedControl(
                new List<object> {"文章", "活动"},
                new List<Widget> {
                    new HistoryArticleScreenConnector(),
                    new HistoryEventScreenConnector()
                },
                newValue => this.setState(() => this._selectedIndex = newValue)
            );
        }
    }
}