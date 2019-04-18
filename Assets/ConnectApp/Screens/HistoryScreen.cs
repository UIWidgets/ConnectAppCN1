using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
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
                    return new HistoryScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class HistoryScreen : StatefulWidget {
        public HistoryScreen(
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
            return new _HistoryScreenState();
        }
    }

    internal class _HistoryScreenState : State<HistoryScreen> {
        private PageController _pageController;
        private int _selectedIndex;

        public override void initState() {
            base.initState();
            _pageController = new PageController();
            _selectedIndex = 0;
        }

        public override void dispose() {
            _pageController.dispose();
            base.dispose();
        }

        private void _deleteAllHistory() {
            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "确定删除浏览历史？",
                items: new List<ActionSheetItem> {
                    new ActionSheetItem(
                        "删除",
                        ActionType.destructive,
                        () => {
                            if (_selectedIndex == 0) {
                                widget.actionModel.deleteAllArticleHistory();
                            } else {
                                widget.actionModel.deleteAllEventHistory();
                            }
                        }),
                    new ActionSheetItem("取消", ActionType.cancel)
                }
            ));
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    child: new Container(
                        color: CColors.White,
                        child: new Column(
                            children: new List<Widget> {
                                _buildNavigationBar(context),
                                _buildSelectView(),
                                _buildContentView()
                            }
                        )
                    )
                )
            );
        }

        private Widget _buildNavigationBar(BuildContext context) {
            return new Container(
                decoration: new BoxDecoration(
                    CColors.White
                ),
                width: MediaQuery.of(context).size.width,
                height: 94,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.symmetric(8, 16),
                            onPressed: () => widget.actionModel.mainRouterPop(),
                            child: new Icon(
                                Icons.arrow_back,
                                size: 24,
                                color: CColors.icon3
                            )
                        ),
                        new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new Container(
                                    margin: EdgeInsets.only(16, bottom: 12),
                                    child: new Text(
                                        "浏览历史",
                                        style: CTextStyle.H2
                                    )
                                ),
                                _buildDeleteButton()
                            }
                        )
                    }
                )
            );
        }

        private Widget _buildDeleteButton() {
            if (_selectedIndex == 0) {
                var articleHistory = widget.viewModel.articleHistory;
                if (articleHistory.Count > 0) {
                    return new CustomButton(
                        padding: EdgeInsets.symmetric(12, 16),
                        onPressed: _deleteAllHistory,
                        child: new Icon(
                            Icons.delete_outline,
                            size: 28,
                            color: CColors.icon3
                        )
                    );
                }
            }
            if (_selectedIndex == 1) {
                var eventHistory = widget.viewModel.eventHistory;
                if (eventHistory.Count > 0) {
                    return new CustomButton(
                        padding: EdgeInsets.symmetric(12, 16),
                        onPressed: _deleteAllHistory,
                        child: new Icon(
                            Icons.delete_outline,
                            size: 28,
                            color: CColors.icon3
                        )
                    );
                }
            }
            return new Container();
        }

        private Widget _buildSelectView() {
            return new CustomSegmentedControl(
                new List<string> {"文章", "活动"},
                newValue => {
                    setState(() => _selectedIndex = newValue);
                    _pageController.animateToPage(
                        newValue,
                        new TimeSpan(0, 0, 0, 0, 250),
                        Curves.ease
                    );
                },
                _selectedIndex
            );
        }

        private Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    child: new PageView(
                        physics: new BouncingScrollPhysics(),
                        controller: _pageController,
                        onPageChanged: index => { setState(() => { _selectedIndex = index; }); },
                        children: new List<Widget> {
                            new HistoryArticleScreenConnector(),
                            new HistoryEventScreenConnector()
                        }
                    )
                )
            );
        }
    }
}