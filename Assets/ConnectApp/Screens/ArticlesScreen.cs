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
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ArticlesScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenViewModel>(
                converter: state => new ArticlesScreenViewModel {
                    isLoggedIn = state.loginState.isLoggedIn,
                    feedHasNew = state.articleState.feedHasNew,
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
        const float _maxNavBarHeight = 96;
        const float _minNavBarHeight = 44;
        const float _maxTitleFontSize = 31.9f;
        const float _minTitleFontSize = 20;
        CustomTabController _tabController;
        int _selectedIndex;
        float _titleFontSize;
        float _navBarHeight;
        string _loginSubId;
        string _logoutSubId;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._selectedIndex = 1;
            this._tabController = new CustomTabController(2, this, initialIndex: this._selectedIndex);
            this._titleFontSize = _maxTitleFontSize;
            this._navBarHeight = _maxNavBarHeight;
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
            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (pixels > 0 && pixels <= _maxNavBarHeight - _minNavBarHeight) {
                    this._titleFontSize = _maxTitleFontSize
                                          - (_maxTitleFontSize - _minTitleFontSize) /
                                          (_maxNavBarHeight - _minNavBarHeight)
                                          * pixels;
                    this._navBarHeight = _maxNavBarHeight - pixels;
                    this.setState(() => { });
                }
                else if (pixels <= 0) {
                    if (this._navBarHeight <= _maxNavBarHeight) {
                        this._titleFontSize = _maxTitleFontSize;
                        this._navBarHeight = _maxNavBarHeight;
                        this.setState(() => { });
                    }
                }
                else if (pixels > 52) {
                    if (!(this._navBarHeight <= _minNavBarHeight)) {
                        this._titleFontSize = _minTitleFontSize;
                        this._navBarHeight = _minNavBarHeight;
                        this.setState(() => { });
                    }
                }
            });
            return true;
        }

        public override Widget build(BuildContext context) {
            base.build(context: context);
            ScrollPhysics physics;
            if (this.widget.viewModel.isLoggedIn) {
                physics = new BouncingScrollPhysics();
            }
            else {
                physics = new NeverScrollableScrollPhysics();
            }

            return new Container(
                padding: EdgeInsets.only(top: CCommonUtils.getSafeAreaTopPadding(context: context)),
                color: CColors.White,
                child: new NotificationListener<ScrollNotification>(
                    onNotification: this._onNotification,
                    child: new CustomSegmentedControl(
                        new List<object> {
                            this._buildSelectItem("关注", 0),
                            this._buildSelectItem("推荐", 1)
                        },
                        new List<Widget> {
                            new FollowArticleScreenConnector(),
                            new RecommendArticleScreenConnector()
                        },
                        newValue => {
                            this.setState(() => this._selectedIndex = newValue);
                            if (newValue == 0) {
                                AnalyticsManager.AnalyticsClickHomeFocus();
                            }
                        },
                        1,
                        headerHeight: this._navBarHeight,
                        this._buildTrailing(),
                        new BoxDecoration(
                            color: CColors.White,
                            border: new Border(bottom: new BorderSide(this._navBarHeight < _maxNavBarHeight
                                ? CColors.Separator2
                                : CColors.Transparent))
                        ),
                        new CustomUnderlineTabIndicator(
                            borderSide: new BorderSide(
                                width: this._navBarHeight <= _minNavBarHeight ? 2 : 4,
                                color: CColors.PrimaryBlue
                            )
                        ),
                        EdgeInsets.only(8),
                        labelPadding: EdgeInsets.zero,
                        40,
                        selectedColor: this._navBarHeight <= _minNavBarHeight ? CColors.PrimaryBlue : CColors.TextTitle,
                        unselectedColor: CColors.TextTitle,
                        unselectedTextStyle: new TextStyle(
                            fontSize: _minTitleFontSize,
                            fontFamily: "Roboto-Bold"
                        ),
                        selectedTextStyle: new TextStyle(
                            fontSize: this._titleFontSize,
                            fontFamily: "Roboto-Bold"
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
                height: this._navBarHeight,
                alignment: Alignment.bottomCenter,
                child: new Stack(
                    children: new List<Widget> {
                        new Container(
                            padding: EdgeInsets.only(8, 4, 8, 10),
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
            return new Row(
                children: new List<Widget> {
                    new CustomButton(
                        padding: EdgeInsets.only(16, 10, 8, 10),
                        onPressed: () => this.widget.actionModel.pushToReality(),
                        child: new Container(
                            color: CColors.Transparent,
                            child: new EggButton(
                                isNationalDay: this.widget.viewModel.nationalDayEnabled
                            )
                        )
                    ),
                    new CustomButton(
                        padding: EdgeInsets.only(8, 8, 16, 8),
                        onPressed: () => this.widget.actionModel.pushToSearch(),
                        child: new Icon(
                            icon: Icons.search,
                            size: 28,
                            color: CColors.Icon
                        )
                    )
                }
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