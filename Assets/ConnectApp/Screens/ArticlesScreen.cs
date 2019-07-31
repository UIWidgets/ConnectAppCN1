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
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class ArticlesScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, ArticlesScreenViewModel>(
                converter: state => new ArticlesScreenViewModel {
                    isLoggedIn = state.loginState.isLoggedIn
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ArticlesScreenActionModel {
                        pushToSearch = () => {
                            dispatcher.dispatch(new MainNavigatorPushToAction {
                                routeName = MainNavigatorRoutes.Search
                            });
                            AnalyticsManager.ClickEnterSearch("Home_Article");
                        },
                        fetchReviewUrl = () => dispatcher.dispatch<IPromise>(Actions.fetchReviewUrl())
                    };
                    return new ArticlesScreen(viewModel, actionModel);
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

    public class _ArticlesScreenState : AutomaticKeepAliveClientMixin<ArticlesScreen>, RouteAware {
        const float _maxNavBarHeight = 96;
        const float _minNavBarHeight = 44;
        const float _maxTitleFontSize = 32;
        const float _minTitleFontSize = 20;
        PageController _pageController;
        int _selectedIndex;
        float _titleFontSize;
        float _navBarHeight;
        string _loginSubId;

        protected override bool wantKeepAlive {
            get { return true; }
        }

        public override void initState() {
            base.initState();
            HttpManager.initVSCode();
            StatusBarManager.statusBarStyle(false);
            this._selectedIndex = 1;
            this._pageController = new PageController(initialPage: this._selectedIndex);
            this._titleFontSize = _maxTitleFontSize;
            this._navBarHeight = _maxNavBarHeight;
            StatusBarManager.hideStatusBar(false);
            SplashManager.fetchSplash();
            AnalyticsManager.AnalyticsOpenApp();
            SchedulerBinding.instance.addPostFrameCallback(_ => { this.widget.actionModel.fetchReviewUrl(); });
            this._loginSubId = EventBus.subscribe(sName: EventBusConstant.login_success, args => {
                if (this._selectedIndex != 1) {
                    this._selectedIndex = 1;
                    this._pageController = new PageController(initialPage: this._selectedIndex);
                }
            });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(this.context));
        }
        
        public override void dispose() {
            EventBus.unSubscribe(sName: EventBusConstant.login_success, id: this._loginSubId);
            Router.routeObserve.unsubscribe(this);
            base.dispose();
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
            if (!this.widget.viewModel.isLoggedIn) {
                return new RecommendArticleScreenConnector();
            }

            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        this._buildSelectView(),
                        this._buildContentView()
                    }
                )
            );
        }

        Widget _buildSelectView() {
            var items = new List<string> {"关注", "推荐"};
            var widgets = new List<Widget>();
            items.ForEach(item => {
                var itemIndex = items.IndexOf(item: item);
                var itemWidget = this._buildSelectItem(title: item, index: itemIndex);
                widgets.Add(item: itemWidget);
                widgets.Add(new SizedBox(width: 16));
            });
            return new Container(
                padding: EdgeInsets.only(16),
                height: this._navBarHeight,
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(bottom: new BorderSide(this._navBarHeight < _maxNavBarHeight
                        ? CColors.Separator2
                        : CColors.Transparent))
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.end,
                    children: new List<Widget> {
                        new Row(
                            children: widgets
                        ),
                        new CustomButton(
                            padding: EdgeInsets.only(16, 8, 16, 8),
                            onPressed: () => this.widget.actionModel.pushToSearch(),
                            child: new Icon(
                                icon: Icons.search,
                                size: 28,
                                color: CColors.Icon
                            )
                        )
                    }
                )
            );
        }

        Widget _buildContentView() {
            return new Flexible(
                child: new Container(
                    child: new NotificationListener<ScrollNotification>(
                        onNotification: this._onNotification,
                        child: new PageView(
                            physics: new BouncingScrollPhysics(),
                            controller: this._pageController,
                            onPageChanged: index => { this.setState(() => this._selectedIndex = index); },
                            children: new List<Widget> {
                                new FollowArticleScreenConnector(),
                                new RecommendArticleScreenConnector()
                            }
                        )
                    )
                )
            );
        }

        Widget _buildSelectItem(string title, int index) {
            var textColor = CColors.TextTitle;
            float titleFontSize = _minTitleFontSize;
            float lineHeight = this._navBarHeight <= _minNavBarHeight ? 2 : 4;
            float radius = this._navBarHeight <= _minNavBarHeight ? 0 : 2;
            Widget lineView = new Align(
                alignment: Alignment.bottomCenter,
                child: new Container(
                    width: 40,
                    height: lineHeight
                )
            );
            if (index == this._selectedIndex) {
                if (this._navBarHeight <= _minNavBarHeight) {
                    textColor = CColors.PrimaryBlue;
                }

                titleFontSize = this._titleFontSize;
                lineView = new Align(
                    alignment: Alignment.bottomCenter,
                    child: new Container(
                        width: 40,
                        height: lineHeight,
                        decoration: new BoxDecoration(
                            color: CColors.PrimaryBlue,
                            borderRadius: BorderRadius.circular(radius: radius)
                        )
                    )
                );
            }

            return new CustomButton(
                onPressed: () => {
                    if (this._selectedIndex != index) {
                        this.setState(() => this._selectedIndex = index);
                        this._pageController.animateToPage(
                            page: index,
                            TimeSpan.FromMilliseconds(250),
                            curve: Curves.ease
                        );
                    }
                },
                padding: EdgeInsets.zero,
                child: new Container(
                    height: this._navBarHeight,
                    child: new Stack(
                        alignment: Alignment.bottomCenter,
                        children: new List<Widget> {
                            new Container(
                                padding: EdgeInsets.symmetric(10),
                                child: new Text(
                                    data: title,
                                    style: new TextStyle(
                                        fontSize: titleFontSize,
                                        fontFamily: "Roboto-Bold",
                                        color: textColor
                                    )
                                )
                            ),
                            lineView
                        }
                    )
                )
            );
        }
        
        public void didPopNext() {
            StatusBarManager.statusBarStyle(false);
        }

        public void didPush() {
        }

        public void didPop() {
        }

        public void didPushNext() {
        }
    }
}