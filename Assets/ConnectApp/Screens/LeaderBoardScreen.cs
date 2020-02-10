using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class LeaderBoardScreenConnector : StatelessWidget {
        public LeaderBoardScreenConnector(
            int initIndex = 0,
            Key key = null
        ) : base(key: key) {
            this.initIndex = initIndex;
        }

        readonly int initIndex;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, LeaderBoardScreenViewModel>(
                converter: state => new LeaderBoardScreenViewModel {
                    initIndex = this.initIndex
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new LeaderBoardScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction())
                    };
                    return new LeaderBoardScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class LeaderBoardScreen : StatefulWidget {
        public LeaderBoardScreen(
            LeaderBoardScreenViewModel viewModel,
            LeaderBoardScreenActionModel actionModel,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly LeaderBoardScreenViewModel viewModel;
        public readonly LeaderBoardScreenActionModel actionModel;

        public override State createState() {
            return new _LeaderBoardScreenState();
        }
    }

    class _LeaderBoardScreenState : TickerProviderStateMixin<LeaderBoardScreen>, RouteAware {
        readonly List<string> tabTitles = new List<string> {"合辑", "专栏", "博主"};

        readonly List<Widget> tabWidgets = new List<Widget> {
            new LeaderBoardCollectionScreenConnector(),
            new LeaderBoardColumnScreenConnector(),
            new LeaderBoardBloggerScreenConnector()
        };

        int _selectedIndex;
        CustomTabController _tabController;
        ScrollController _scrollController;
        bool _isHaveTitle;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(true);
            this._selectedIndex = this.widget.viewModel.initIndex;
            this._tabController = new CustomTabController(
                length: this.tabTitles.Count,
                this,
                initialIndex: this._selectedIndex
            );
            this._scrollController = new ScrollController();
            this._isHaveTitle = true;
            this._tabController.addListener(() => {
                if (this._tabController.index != this._selectedIndex) {
                    this.setState(() => this._selectedIndex = this._tabController.index);
                }
            });
            LocalDataManager.markLeaderBoardUpdatedTime();
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            this._tabController.dispose();
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        bool _onNotification(ScrollNotification notification) {
            var axisDirection = notification.metrics.axisDirection;
            if (axisDirection == AxisDirection.left || axisDirection == AxisDirection.right) {
                return true;
            }

            var pixels = notification.metrics.pixels;
            if (pixels >= 3) {
                if (this._isHaveTitle) {
                    StatusBarManager.statusBarStyle(false);
                    this.setState(() => this._isHaveTitle = false);
                }
            }
            else {
                if (!this._isHaveTitle) {
                    StatusBarManager.statusBarStyle(true);
                    this.setState(() => this._isHaveTitle = true);
                }
            }

            return true;
        }

        public override Widget build(BuildContext context) {
            List<Color> colors;
            if (this._selectedIndex == 2) {
                colors = new List<Color> {
                    new Color(0xFFFFB84F),
                    new Color(0xFFFF8024)
                };
            }
            else {
                colors = new List<Color> {
                    new Color(0xFF6EC6FF),
                    CColors.PrimaryBlue,
                    CColors.MessageReactionCount
                };
            }

            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    top: false,
                    bottom: false,
                    child: new Container(
                        decoration: new BoxDecoration(
                            gradient: new LinearGradient(
                                colors: colors,
                                begin: Alignment.topLeft,
                                end: Alignment.bottomRight
                            )
                        ),
                        child: new Stack(
                            alignment: Alignment.topLeft,
                            fit: StackFit.expand,
                            children: new List<Widget> {
                                Image.asset("image/leaderboard-pattern-curves", fit: BoxFit.fill),
                                Positioned.fill(
                                    new Column(
                                        children: new List<Widget> {
                                            this._buildNavigationBar(context: context),
                                            new Flexible(child: this._buildContent())
                                        }
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar(BuildContext context) {
            Color navigationBarColor;
            Border border;
            Color backColor;
            if (this._isHaveTitle) {
                navigationBarColor = CColors.Transparent;
                border = null;
                backColor = CColors.White;
            }
            else {
                navigationBarColor = CColors.White;
                border = new Border(bottom: new BorderSide(color: CColors.Separator2));
                backColor = CColors.TextTitle;
            }

            return new Container(
                height: CustomAppBarUtil.appBarHeight + CCommonUtils.getSafeAreaTopPadding(context: context),
                padding: EdgeInsets.only(top: CCommonUtils.getSafeAreaTopPadding(context: context)),
                decoration: new BoxDecoration(
                    color: navigationBarColor,
                    border: border
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.only(16, 10, 16, 10),
                            onPressed: () => this.widget.actionModel.mainRouterPop(),
                            child: new Icon(
                                icon: Icons.arrow_back,
                                size: 24,
                                color: backColor
                            )
                        ),
                        new Expanded(
                            child: new Stack(
                                fit: StackFit.expand,
                                children: new List<Widget> {
                                    new AnimatedPositioned(
                                        left: 0,
                                        top: this._isHaveTitle ? 13 : -44,
                                        right: 0,
                                        duration: TimeSpan.FromMilliseconds(100),
                                        child: new Text(
                                            "优选榜单",
                                            textAlign: TextAlign.center,
                                            style: CTextStyle.PXLargeMediumWhite
                                        )
                                    ),
                                    new AnimatedPositioned(
                                        left: 0,
                                        top: this._isHaveTitle ? 44 : 0,
                                        right: 0,
                                        duration: TimeSpan.FromMilliseconds(100),
                                        child: new Container(
                                            alignment: Alignment.center,
                                            child: this._buildTabBarHeader()
                                        )
                                    )
                                }
                            )
                        ),
                        new Container(width: 56)
                    }
                )
            );
        }

        Widget _buildTabBarHeader() {
            var tabs = new List<Widget>();
            this.tabTitles.ForEach(tabTitle => {
                var tab = new Padding(
                    padding: EdgeInsets.symmetric(10),
                    child: new Text(data: tabTitle)
                );
                tabs.Add(item: tab);
            });
            return new Container(
                height: 44,
                color: CColors.Transparent,
                alignment: Alignment.center,
                child: new CustomTabBarHeader(
                    tabs: tabs,
                    controller: this._tabController,
                    indicatorSize: CustomTabBarIndicatorSize.fixedOrLabel,
                    indicatorFixedSize: 16,
                    indicatorPadding: EdgeInsets.zero,
                    indicatorChangeStyle: CustomTabBarIndicatorChangeStyle.enlarge,
                    indicatorColor: this._isHaveTitle ? CColors.White : CColors.TextTitle,
                    unselectedLabelStyle: this._isHaveTitle ? CTextStyle.PLargeWhite : CTextStyle.PLargeTitle,
                    unselectedLabelColor: this._isHaveTitle ? CColors.White : CColors.TextTitle,
                    labelStyle: this._isHaveTitle ? CTextStyle.PLargeMediumWhite : CTextStyle.PLargeMedium,
                    labelColor: this._isHaveTitle ? CColors.White : CColors.TextTitle,
                    isScrollable: true
                )
            );
        }

        Widget _buildContent() {
            var tabChildren = new List<Widget>();
            this.tabWidgets.ForEach(tabWidget => {
                var tabChild = new Builder(
                    builder: context => tabWidget
                );
                tabChildren.Add(item: tabChild);
            });
            return new NestedScrollView(
                controller: this._scrollController,
                headerSliverBuilder: (context, innerBoxIsScrolled) => new List<Widget> {
                    new SliverToBoxAdapter(
                        child: this._buildTabBarHeader()
                    )
                },
                body: new NotificationListener<ScrollNotification>(
                    onNotification: this._onNotification,
                    child: new CustomTabBarView(
                        children: tabChildren,
                        controller: this._tabController
                    )
                )
            );
        }

        public void didPopNext() {
            StatusBarManager.statusBarStyle(isLight: this._isHaveTitle);
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}