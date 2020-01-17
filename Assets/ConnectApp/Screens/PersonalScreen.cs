using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.Plugins;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class PersonalScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, PersonalScreenViewModel>(
                converter: state => new PersonalScreenViewModel {
                    isLoggedIn = state.loginState.isLoggedIn,
                    user = state.loginState.loginInfo,
                    userDict = state.userState.userDict,
                    userLicenseDict = state.userState.userLicenseDict,
                    currentTabBarIndex = state.tabBarState.currentTabIndex,
                    hasUnreadNotifications = state.loginState.newNotifications != null
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new PersonalScreen(
                        viewModel: viewModel,
                        new PersonalScreenActionModel {
                            mainRouterPushTo = routeName => dispatcher.dispatch(new MainNavigatorPushToAction {
                                routeName = routeName
                            }),
                            pushToNotifications = () => {
                                dispatcher.dispatch(new MainNavigatorPushToAction {
                                    routeName = MainNavigatorRoutes.Notification
                                });
                                dispatcher.dispatch(new UpdateNewNotificationAction {
                                    notification = null
                                });
                            },
                            pushToUserDetail = userId => dispatcher.dispatch(new MainNavigatorPushToUserDetailAction {
                                userId = userId
                            }),
                            pushToUserFollowing = (userId, initialPage) => dispatcher.dispatch(
                                new MainNavigatorPushToUserFollowingAction {
                                    userId = userId,
                                    initialPage = initialPage
                                }
                            ),
                            pushToUserLike = userId => dispatcher.dispatch(
                                new MainNavigatorPushToUserLikeAction {
                                    userId = userId
                                }
                            ),
                            pushToUserFollower = userId => dispatcher.dispatch(
                                new MainNavigatorPushToUserFollowerAction {
                                    userId = userId
                                }
                            )
                        }
                    );
                }
            );
        }
    }

    public class PersonalScreen : StatefulWidget {
        public PersonalScreen(
            PersonalScreenViewModel viewModel = null,
            PersonalScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly PersonalScreenViewModel viewModel;
        public readonly PersonalScreenActionModel actionModel;

        public override State createState() {
            return new _PersonalScreenState();
        }
    }

    public class _PersonalScreenState : State<PersonalScreen>, RouteAware {
        string _loginSubId;
        string _logoutSubId;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(UserInfoManager.isLogin());
            this._loginSubId = EventBus.subscribe(sName: EventBusConstant.login_success,
                _ => { StatusBarManager.statusBarStyle(true); });
            this._logoutSubId = EventBus.subscribe(sName: EventBusConstant.logout_success,
                _ => { StatusBarManager.statusBarStyle(false); });
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(context: this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            EventBus.unSubscribe(sName: EventBusConstant.login_success, id: this._loginSubId);
            EventBus.unSubscribe(sName: EventBusConstant.logout_success, id: this._logoutSubId);
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.Background,
                child: new Column(
                    children: new List<Widget> {
                        this.widget.viewModel.isLoggedIn
                            ? this._buildLoginInNavigationBar()
                            : this._buildNotLoginInNavigationBar(),
                        this._buildMidView(),
                        new Container(height: 16),
                        this._buildBottomView()
                    }
                )
            );
        }

        Widget _buildBottomView() {
            return new Container(
                child: new Column(
                    children: new List<Widget> {
                        new CustomListTile(
                            new Icon(icon: Icons.outline_settings, size: 24, color: CColors.TextBody2),
                            "设置",
                            trailing: CustomListTileConstant.defaultTrailing,
                            onTap: () => this.widget.actionModel.mainRouterPushTo(obj: MainNavigatorRoutes.Setting)
                        ),
                        new CustomListTile(
                            new Icon(icon: Icons.outline_mail, size: 24, color: CColors.TextBody2),
                            "意见反馈",
                            trailing: CustomListTileConstant.defaultTrailing,
                            onTap: () => {
                                AnalyticsManager.ClickEnterAboutUs();
                                this.widget.actionModel.mainRouterPushTo(obj: MainNavigatorRoutes.Feedback);
                            }),
                        new CustomListTile(
                            new Icon(icon: Icons.outline_sentiment_smile, size: 24, color: CColors.TextBody2),
                            "关于我们",
                            trailing: CustomListTileConstant.defaultTrailing,
                            onTap: () => this.widget.actionModel.mainRouterPushTo(obj: MainNavigatorRoutes.AboutUs)
                        )
                    }
                )
            );
        }

        Widget _buildMidView() {
            return new Container(
                color: CColors.White,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        new Expanded(
                            child: new CustomButton(
                                onPressed: () => {
                                    var routeName = this.widget.viewModel.isLoggedIn
                                        ? MainNavigatorRoutes.MyFavorite
                                        : MainNavigatorRoutes.Login;
                                    this.widget.actionModel.mainRouterPushTo(obj: routeName);
                                },
                                padding: EdgeInsets.symmetric(16),
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        Image.asset(
                                            "image/mine-collection",
                                            width: 32,
                                            height: 32
                                        ),
                                        new Container(height: 4),
                                        new Text(
                                            "我的收藏",
                                            style: CTextStyle.PRegularTitle.defaultHeight()
                                        )
                                    }
                                )
                            )
                        ),
                        new Expanded(
                            child: new CustomButton(
                                onPressed: () => {
                                    var routeName = this.widget.viewModel.isLoggedIn
                                        ? MainNavigatorRoutes.MyEvent
                                        : MainNavigatorRoutes.Login;
                                    this.widget.actionModel.mainRouterPushTo(obj: routeName);
                                },
                                padding: EdgeInsets.symmetric(16),
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        Image.asset(
                                            "image/mine-events",
                                            width: 32,
                                            height: 32
                                        ),
                                        new Container(height: 4),
                                        new Text(
                                            "我的活动",
                                            style: CTextStyle.PRegularTitle.defaultHeight()
                                        )
                                    }
                                )
                            )
                        ),
                        new Expanded(
                            child: new CustomButton(
                                onPressed: () =>
                                    this.widget.actionModel.mainRouterPushTo(obj: MainNavigatorRoutes.History),
                                padding: EdgeInsets.symmetric(16),
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: new List<Widget> {
                                        Image.asset(
                                            "image/mine-history",
                                            width: 32,
                                            height: 32
                                        ),
                                        new Container(height: 4),
                                        new Text(
                                            "浏览历史",
                                            style: CTextStyle.PRegularTitle.defaultHeight()
                                        )
                                    }
                                )
                            )
                        )
                    }
                )
            );
        }

        Widget _buildNotLoginInNavigationBar() {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(top: CCommonUtils.getSafeAreaTopPadding(context: this.context)),
                margin: EdgeInsets.only(bottom: 16),
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.end,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        this._buildQrScanWidget(false),
                        new Container(
                            padding: EdgeInsets.only(16, 8, 16, 24),
                            child: new Row(
                                children: new List<Widget> {
                                    new Column(
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new Text("欢迎来到", style: CTextStyle.H4.defaultHeight()),
                                            new Text("Unity Connect", style: CTextStyle.H2.defaultHeight()),
                                            new Container(
                                                margin: EdgeInsets.only(top: 24),
                                                child: new CustomButton(
                                                    padding: EdgeInsets.zero,
                                                    onPressed: () =>
                                                        this.widget.actionModel.mainRouterPushTo(
                                                            obj: MainNavigatorRoutes.Login),
                                                    child: new Container(
                                                        height: 40,
                                                        width: 120,
                                                        alignment: Alignment.center,
                                                        decoration: new BoxDecoration(
                                                            color: CColors.PrimaryBlue,
                                                            borderRadius: BorderRadius.all(20)
                                                        ),
                                                        child: new Text(
                                                            "登录/注册",
                                                            style: CTextStyle.PLargeMediumWhite.defaultHeight()
                                                        )
                                                    )
                                                )
                                            )
                                        }
                                    ),
                                    new Expanded(
                                        child: Image.asset(
                                            "image/mine_mascot_u"
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildLoginInNavigationBar() {
            var user = this.widget.viewModel.userDict[key: this.widget.viewModel.user.userId];
            Widget titleWidget;
            if (user.title != null && user.title.isNotEmpty()) {
                titleWidget = new Container(
                    padding: EdgeInsets.only(top: 4),
                    child: new Text(
                        data: user.title,
                        style: new TextStyle(
                            fontSize: 14,
                            fontFamily: "Roboto-Regular",
                            color: CColors.Grey80
                        ),
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis
                    )
                );
            }
            else {
                titleWidget = new Container();
            }

            return new Stack(
                children: new List<Widget> {
                    new Container(
                        color: CColors.BgGrey,
                        padding: EdgeInsets.only(bottom: 56),
                        child: new GestureDetector(
                            onTap: () => this.widget.actionModel.pushToUserDetail(obj: user.id),
                            child: new Container(
                                padding: EdgeInsets.only(
                                    top: CCommonUtils.getSafeAreaTopPadding(context: this.context)),
                                decoration: new BoxDecoration(
                                    color: CColors.Black,
                                    new DecorationImage(
                                        new AssetImage("image/default-background-cover"),
                                        fit: BoxFit.cover
                                    )
                                ),
                                child: new Column(
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    children: new List<Widget> {
                                        this._buildQrScanWidget(true),
                                        new Container(
                                            padding: EdgeInsets.only(16, 16, 16, 64),
                                            child: new Row(
                                                children: new List<Widget> {
                                                    new Container(
                                                        margin: EdgeInsets.only(right: 12),
                                                        child: Avatar.User(
                                                            user: user,
                                                            64,
                                                            true
                                                        )
                                                    ),
                                                    new Expanded(
                                                        child: new Column(
                                                            crossAxisAlignment: CrossAxisAlignment.start,
                                                            children: new List<Widget> {
                                                                new Row(
                                                                    crossAxisAlignment: CrossAxisAlignment.start,
                                                                    children: new List<Widget> {
                                                                        new Flexible(
                                                                            child: new Text(
                                                                                user.fullName ?? user.name,
                                                                                style: CTextStyle.H4White
                                                                                    .defaultHeight(),
                                                                                maxLines: 1,
                                                                                overflow: TextOverflow.ellipsis
                                                                            )
                                                                        ),
                                                                        CImageUtils.GenBadgeImage(
                                                                            badges: user.badges,
                                                                            CCommonUtils.GetUserLicense(
                                                                                userId: user.id,
                                                                                userLicenseMap: this.widget.viewModel
                                                                                    .userLicenseDict
                                                                            ),
                                                                            EdgeInsets.only(4, 6)
                                                                        )
                                                                    }
                                                                ),
                                                                titleWidget
                                                            }
                                                        )
                                                    ),
                                                    new Padding(
                                                        padding: EdgeInsets.symmetric(horizontal: 4),
                                                        child: new Text(
                                                            "个人主页",
                                                            style: new TextStyle(
                                                                fontSize: 14,
                                                                fontFamily: "Roboto-Regular",
                                                                color: CColors.Grey80
                                                            )
                                                        )
                                                    ),
                                                    new Icon(
                                                        icon: Icons.baseline_forward_arrow,
                                                        size: 16,
                                                        color: CColors.LightBlueGrey
                                                    )
                                                }
                                            )
                                        )
                                    }
                                )
                            )
                        )
                    ),
                    new Align(
                        alignment: Alignment.bottomCenter,
                        child: new Container(
                            margin: EdgeInsets.only(16, 0, 16, 16),
                            height: 80,
                            decoration: new BoxDecoration(
                                color: CColors.White,
                                borderRadius: BorderRadius.all(8),
                                boxShadow: new List<BoxShadow> {
                                    new BoxShadow(
                                        CColors.Black.withOpacity(0.2f),
                                        blurRadius: 8
                                    )
                                }
                            ),
                            child: new Row(
                                children: new List<Widget> {
                                    new Expanded(
                                        child: new CustomButton(
                                            onPressed: () => {
                                                if (this.widget.viewModel.isLoggedIn && user.id.isNotEmpty()) {
                                                    this.widget.actionModel.pushToUserFollowing(arg1: user.id, 0);
                                                }
                                            },
                                            child: new Column(
                                                mainAxisAlignment: MainAxisAlignment.center,
                                                children: new List<Widget> {
                                                    new Text(
                                                        CStringUtils.CountToString((user.followingUsersCount ?? 0) + (user.followingTeamsCount ?? 0), "0"),
                                                        style: CTextStyle.Bold20
                                                    ),
                                                    new Container(height: 4),
                                                    new Text(
                                                        "关注",
                                                        style: CTextStyle.PSmallBody3.defaultHeight()
                                                    )
                                                }
                                            )
                                        )
                                    ),
                                    new Container(height: 32, width: 1, color: CColors.Separator.withOpacity(0.5f)),
                                    new Expanded(
                                        child: new CustomButton(
                                            onPressed: () => {
                                                if (this.widget.viewModel.isLoggedIn && user.id.isNotEmpty()) {
                                                    this.widget.actionModel.pushToUserFollower(obj: user.id);
                                                }
                                            },
                                            child: new Column(
                                                mainAxisAlignment: MainAxisAlignment.center,
                                                children: new List<Widget> {
                                                    new Text(
                                                        CStringUtils.CountToString(user.followCount ?? 0, "0"),
                                                        style: CTextStyle.Bold20
                                                    ),
                                                    new Container(height: 4),
                                                    new Text(
                                                        "粉丝",
                                                        style: CTextStyle.PSmallBody3.defaultHeight()
                                                    )
                                                }
                                            )
                                        )
                                    ),
                                    new Container(height: 32, width: 1, color: CColors.Separator.withOpacity(0.5f)),
                                    new Expanded(
                                        child: new CustomButton(
                                            onPressed: () => {
                                                // if (this.widget.viewModel.isLoggedIn && user.id.isNotEmpty()) {
                                                //     this.widget.actionModel.pushToUserLike(obj: user.id);
                                                // }
                                            },
                                            child: new Column(
                                                mainAxisAlignment: MainAxisAlignment.center,
                                                children: new List<Widget> {
                                                    new Text(
                                                        CStringUtils.CountToString(user.appArticleLikedCount ?? 0, "0"),
                                                        style: CTextStyle.Bold20
                                                    ),
                                                    new Container(height: 4),
                                                    new Text(
                                                        "赞",
                                                        style: CTextStyle.PSmallBody3.defaultHeight()
                                                    )
                                                }
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    )
                }
            );
        }

        Widget _buildQrScanWidget(bool isLoggedIn) {
            return new Container(
                height: 44,
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: new List<Widget> {
                        isLoggedIn
                            ? (Widget) new CustomButton(
                                padding: EdgeInsets.symmetric(8, 16),
                                onPressed: () => this.widget.actionModel.pushToNotifications(),
                                child: new Container(
                                    width: 28,
                                    height: 28,
                                    child: new Stack(
                                        children: new List<Widget> {
                                            new Icon(
                                                icon: Icons.outline_notifications,
                                                color: CColors.LightBlueGrey,
                                                size: 28
                                            ),
                                            Positioned.fill(
                                                new Align(
                                                    alignment: Alignment.topRight,
                                                    child: new NotificationDot(
                                                        this.widget.viewModel.hasUnreadNotifications ? "" : null,
                                                        new BorderSide(color: CColors.White, 2)
                                                    )
                                                )
                                            )
                                        }
                                    )
                                )
                            )
                            : new Container(),
                        new CustomButton(
                            padding: EdgeInsets.symmetric(8, 16),
                            onPressed: QRScanPlugin.PushToQRScan,
                            child: new Icon(
                                icon: Icons.outline_scan,
                                size: 28,
                                color: CColors.LightBlueGrey
                            )
                        )
                    }
                )
            );
        }

        public void didPopNext() {
            if (this.widget.viewModel.currentTabBarIndex == 3) {
                StatusBarManager.statusBarStyle(UserInfoManager.isLogin());
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