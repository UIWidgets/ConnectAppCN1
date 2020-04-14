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
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class ArticlesScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenViewModel>(
                converter: state => new ArticlesScreenViewModel {
                    isLoggedIn = state.loginState.isLoggedIn,
                    feedHasNew = state.articleState.feedHasNew,
                    searchSuggest = state.articleState.searchSuggest,
                    currentTabBarIndex = state.tabBarState.currentTabIndex,
                    nationalDayEnabled = state.serviceConfigState.nationalDayEnabled
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ArticlesScreenActionModel {
                        pushToSearch = () => {
                            dispatcher.dispatch(new MainNavigatorPushToAction {
                                routeName = MainNavigatorRoutes.Search
                            });
                            AnalyticsManager.ClickEnterSearch("Home_Article");
                        },
                        pushToLogin = () => dispatcher.dispatch(new MainNavigatorPushToAction {
                            routeName = MainNavigatorRoutes.Login
                        }),
                        pushToReality = () => {
                            dispatcher.dispatch(new EnterRealityAction());
                            AnalyticsManager.AnalyticsClickEgg(1);
                        },
                        pushToGame = () => {
                            var url = LocalDataManager.getTinyGameUrl();
                            if (url.isEmpty() || url.Equals("no_game")) {
                                CustomToast.show(new CustomToastItem(
                                    context: context,
                                    "暂无游戏",
                                    TimeSpan.FromMilliseconds(2000)
                                ));
                                return;
                            }
                            dispatcher.dispatch(new MainNavigatorPushToWebViewAction {
                                url = url,
                                landscape = true,
                                fullscreen = true,
                                showOpenInBrowser = false
                            });
                        }
                    };
                    return new ArticlesScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class ArticlesScreen : StatefulWidget {
        public ArticlesScreen(
            ArticlesScreenViewModel viewModel = null,
            ArticlesScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ArticlesScreenViewModel viewModel;
        public readonly ArticlesScreenActionModel actionModel;

        public override State createState() {
            return new _ArticlesScreenState();
        }
    }

    public class _ArticlesScreenState : AutomaticKeepAliveClientMixin<ArticlesScreen>, RouteAware, TickerProvider {
        CustomTabController _tabController;
        int _selectedIndex;
        float _navBarHeight;
        string _loginSubId;
        string _logoutSubId;
        bool _isRefresh;
        float _recommendArticlePixels;
        float _followArticlePixels;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._selectedIndex = 1;
            this._tabController = new CustomTabController(2, this, initialIndex: this._selectedIndex);
            this._navBarHeight = CustomAppBarUtil.appBarHeight;
            this._isRefresh = false;
            this._recommendArticlePixels = 0;
            this._followArticlePixels = 0;
            this._loginSubId = EventBus.subscribe(sName: EventBusConstant.login_success, args => {
                if (this._selectedIndex != 1) {
                    this._selectedIndex = 1;
                    this._tabController.animateTo(value: this._selectedIndex);
                }
            });
            this._logoutSubId = EventBus.subscribe(sName: EventBusConstant.logout_success, args => {
                if (this._selectedIndex != 1) {
                    this._selectedIndex = 1;
                    this._tabController.animateTo(value: this._selectedIndex);
                }
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            EventBus.unSubscribe(sName: EventBusConstant.login_success, id: this._loginSubId);
            EventBus.unSubscribe(sName: EventBusConstant.logout_success, id: this._logoutSubId);
            Router.routeObserve.unsubscribe(this);
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        bool _onNotification(ScrollNotification notification) {
            var axisDirection = notification.metrics.axisDirection;
            if (axisDirection == AxisDirection.left || axisDirection == AxisDirection.right) {
                return true;
            }

            var pixels = notification.metrics.pixels;
            if (this._selectedIndex == 0) {
                this._followArticlePixels = pixels;
            }

            if (this._selectedIndex == 1) {
                this._recommendArticlePixels = pixels;
            }

            if (pixels <= 0) {
                if (this._navBarHeight != CustomAppBarUtil.appBarHeight) {
                    this._navBarHeight = CustomAppBarUtil.appBarHeight;
                    this.setState(() => { });
                }
            }
            else {
                if (pixels <= CustomAppBarUtil.appBarHeight) {
                    this._navBarHeight = CustomAppBarUtil.appBarHeight - pixels;
                    this.setState(() => { });
                }
                else {
                    if (this._navBarHeight != 0) {
                        this._navBarHeight = 0;
                        this.setState(() => { });
                    }
                }
            }

            this._changeTabBarItemStatus(pixels: pixels, status: TabBarItemStatus.toHome);
            return true;
        }

        void _changeTabBarItemStatus(float pixels, TabBarItemStatus status) {
            if (pixels > MediaQuery.of(context: this.context).size.height) {
                if (!this._isRefresh) {
                    this._isRefresh = true;
                    EventBus.publish(sName: EventBusConstant.article_refresh,
                        new List<object> {TabBarItemStatus.toRefresh});
                }
            }
            else {
                if (this._isRefresh) {
                    this._isRefresh = false;
                    EventBus.publish(sName: EventBusConstant.article_refresh, new List<object> {status});
                }
            }
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            return new Container(
                padding: EdgeInsets.only(top: CCommonUtils.getSafeAreaTopPadding(context: context)),
                color: CColors.White,
                child: new NotificationListener<ScrollNotification>(
                    onNotification: this._onNotification,
                    child: new Column(
                        children: new List<Widget> {
                            this._buildNavigationBar(),
                            this._buildContent()
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new Container(
                color: CColors.White,
                height: this._navBarHeight,
                child: new Row(
                    children: new List<Widget> {
                        new SizedBox(width: 16),
                        new Expanded(
                            child: new GestureDetector(
                                onTap: () => this.widget.actionModel.pushToSearch(),
                                child: new Container(
                                    height: 32,
                                    decoration: new BoxDecoration(
                                        color: CColors.EmojiBottomBar,
                                        borderRadius: BorderRadius.all(16)
                                    ),
                                    child: new Row(
                                        children: new List<Widget> {
                                            new Padding(
                                                padding: EdgeInsets.only(16, right: 8),
                                                child: new Icon(
                                                    icon: Icons.outline_search,
                                                    size: 16,
                                                    color: CColors.Icon
                                                )
                                            ),
                                            new Text(
                                                this.widget.viewModel.searchSuggest.isNotEmpty()
                                                    ? this.widget.viewModel.searchSuggest
                                                    : "搜索",
                                                style: new TextStyle(
                                                    fontSize: 14,
                                                    fontFamily: "Roboto-Regular",
                                                    color: CColors.TextBody5
                                                )
                                            )
                                        }
                                    )
                                )
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(16, 8, 16, 8),
                            onPressed: () => this.widget.actionModel.pushToGame(),
                            child: new GamepadButton()
                        )
                    }
                )
            );
        }

        Widget _buildContent() {
            ScrollPhysics physics;
            if (this.widget.viewModel.isLoggedIn) {
                physics = new BouncingScrollPhysics();
            }
            else {
                physics = new NeverScrollableScrollPhysics();
            }

            return new Expanded(
                child: new CustomSegmentedControl(
                    new List<object> {
                        this._buildSelectItem("关注", 0),
                        this._buildSelectItem("推荐", 1)
                    },
                    new List<Widget> {
                        new FollowArticleScreenConnector(selectedIndex: this._selectedIndex),
                        new RecommendArticleScreenConnector(selectedIndex: this._selectedIndex)
                    },
                    newValue => {
                        this.setState(() => this._selectedIndex = newValue);
                        if (newValue == 0) {
                            AnalyticsManager.AnalyticsClickHomeFocus();
                            this._changeTabBarItemStatus(pixels: this._followArticlePixels,
                                status: TabBarItemStatus.normal);
                        }

                        if (newValue == 1) {
                            this._changeTabBarItemStatus(pixels: this._recommendArticlePixels,
                                status: TabBarItemStatus.normal);
                        }
                    },
                    1,
                    trailing: this._buildTrailing(),
                    headerDecoration: new BoxDecoration(
                        color: CColors.White,
                        border: new Border(bottom: new BorderSide(this._navBarHeight <= 0
                            ? CColors.Separator2
                            : CColors.Transparent))
                    ),
                    indicator: new CustomGradientsTabIndicator(
                        insets: EdgeInsets.symmetric(horizontal: 8),
                        height: 8,
                        gradient: new LinearGradient(
                            begin: Alignment.centerLeft,
                            end: Alignment.centerRight,
                            new List<Color> {
                                new Color(0xFFB1E0FF),
                                new Color(0xFF6EC6FF)
                            }
                        )
                    ),
                    headerPadding: EdgeInsets.only(8, bottom: 8),
                    labelPadding: EdgeInsets.zero,
                    selectedColor: CColors.TextTitle,
                    unselectedColor: CColors.TextBody4,
                    unselectedTextStyle: new TextStyle(
                        fontSize: 18,
                        fontFamily: "Roboto-Medium"
                    ),
                    selectedTextStyle: new TextStyle(
                        fontSize: 18,
                        fontFamily: "Roboto-Medium"
                    ),
                    controller: this._tabController,
                    physics: physics,
                    onTap: index => {
                        if (this._selectedIndex != index) {
                            if (index == 0) {
                                if (!this.widget.viewModel.isLoggedIn) {
                                    this.widget.actionModel.pushToLogin();
                                    return;
                                }
                            }

                            this.setState(() => this._selectedIndex = index);
                            this._tabController.animateTo(value: index);
                        }
                    }
                )
            );
        }

        Widget _buildSelectItem(string title, int index) {
            Widget redDot;
            if (index == 0 && this.widget.viewModel.isLoggedIn && this.widget.viewModel.feedHasNew) {
                redDot = new Positioned(
                    top: 0,
                    right: 0,
                    child: new Container(
                        width: 8,
                        height: 8,
                        decoration: new BoxDecoration(
                            color: CColors.Error,
                            borderRadius: BorderRadius.circular(4)
                        )
                    )
                );
            }
            else {
                redDot = new Container();
            }

            return new Container(
                height: 44,
                alignment: Alignment.bottomCenter,
                child: new Stack(
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(8, 4, 8),
                            color: CColors.Transparent,
                            child: new Text(
                                data: title
                            )
                        ),
                        redDot
                    }
                )
            );
        }

        Widget _buildTrailing() {
            return new Opacity(
                (CustomAppBarUtil.appBarHeight - this._navBarHeight) / CustomAppBarUtil.appBarHeight,
                child: new Row(
                    children: new List<Widget> {
                        new CustomButton(
                            padding: EdgeInsets.only(16, 8, 8, 8),
                            onPressed: () => this.widget.actionModel.pushToSearch(),
                            child: new Icon(
                                icon: Icons.outline_search,
                                size: 28,
                                color: CColors.Icon
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(8, 8, 16, 8),
                            onPressed: () => this.widget.actionModel.pushToGame(),
                            child: Image.asset("image/egg-gamepad")
                        )
                    }
                )
            );
        }

        public void didPopNext() {
            if (this.widget.viewModel.currentTabBarIndex == 0) {
                StatusBarManager.statusBarStyle(false);
            }
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}