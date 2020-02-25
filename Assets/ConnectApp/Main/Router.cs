using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.screens;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Main {
    static class MainNavigatorRoutes {
        public const string Root = "/";
        public const string Splash = "/splash";
        public const string Main = "/main";
        public const string Search = "/search";
        public const string ArticleDetail = "/article-detail";
        public const string Notification = "/notification";
        public const string Setting = "/setting";
        public const string MyEvent = "/my-event";
        public const string MyFavorite = "/my-favorite";
        public const string History = "/history";
        public const string Login = "/login";
        public const string BindUnity = "/bind-unity";
        public const string Report = "/report";
        public const string AboutUs = "/aboutUs";
        public const string WebView = "/web-view";
        public const string UserDetail = "/user-detail";
        public const string UserFollowing = "/user-following";
        public const string UserFollower = "/user-follower";
        public const string UserLike = "/user-like";
        public const string EditPersonalInfo = "/edit-personalInfo";
        public const string PersonalRole = "/personal-role";
        public const string TeamDetail = "/team-detail";
        public const string TeamFollower = "/team-follower";
        public const string TeamMember = "/team-member";
        public const string QRScanLogin = "/qr-login";
        public const string Feedback = "/feedback";
        public const string FeedbackType = "/feedback-type";
        public const string DiscoverChannel = "/discover-channel";
        public const string ChannelScreen = "/channel-screen";
        public const string ChannelDetail = "/channel-detail";
        public const string ChannelMembers = "/channel-members";
        public const string ChannelIntroduction = "/channel-introduction";
        public const string ReactionsDetail = "/reactions-detail";
        public const string HomeEvent = "/home-event";
        public const string Blogger = "/blogger";
        public const string ForceUpdate = "/force-update";
        public const string Game = "/game";
    }

    class Router : StatelessWidget {
        static readonly GlobalKey globalKey = GlobalKey.key("main-router");
        static readonly RouteObserve<PageRoute> _routeObserve = new RouteObserve<PageRoute>();
        static readonly HeroController _heroController = new HeroController();
        bool _exitApp;
        Timer _timer;

        public static NavigatorState navigator {
            get { return globalKey.currentState as NavigatorState; }
        }

        public static RouteObserve<PageRoute> routeObserve {
            get { return _routeObserve; }
        }

        static Dictionary<string, WidgetBuilder> mainRoutes {
            get {
                var routes = new Dictionary<string, WidgetBuilder> {
                    {MainNavigatorRoutes.Search, context => new SearchScreenConnector()},
                    {MainNavigatorRoutes.ArticleDetail, context => new ArticleDetailScreenConnector("")},
                    {MainNavigatorRoutes.Notification, context => new NotificationScreenConnector()},
                    {MainNavigatorRoutes.Setting, context => new SettingScreenConnector()},
                    {MainNavigatorRoutes.MyEvent, context => new MyEventsScreenConnector()},
                    {MainNavigatorRoutes.MyFavorite, context => new MyFavoriteScreenConnector()},
                    {MainNavigatorRoutes.History, context => new HistoryScreenConnector()},
                    {MainNavigatorRoutes.Login, context => new LoginScreen()},
                    {MainNavigatorRoutes.BindUnity, context => new BindUnityScreenConnector(FromPage.setting)},
                    {MainNavigatorRoutes.Report, context => new ReportScreenConnector("", ReportType.article)},
                    {MainNavigatorRoutes.AboutUs, context => new AboutUsScreenConnector()},
                    {MainNavigatorRoutes.WebView, context => new WebViewScreen()},
                    {MainNavigatorRoutes.UserDetail, context => new UserDetailScreenConnector("")},
                    {MainNavigatorRoutes.UserFollowing, context => new UserFollowingScreenConnector("")},
                    {MainNavigatorRoutes.UserFollower, context => new UserFollowerScreenConnector("")},
                    {MainNavigatorRoutes.UserLike, context => new UserLikeArticleScreenConnector("")},
                    {MainNavigatorRoutes.EditPersonalInfo, context => new EditPersonalInfoScreenConnector("")},
                    {MainNavigatorRoutes.PersonalRole, context => new PersonalJobRoleScreenConnector()},
                    {MainNavigatorRoutes.TeamDetail, context => new TeamDetailScreenConnector("")},
                    {MainNavigatorRoutes.TeamFollower, context => new TeamFollowerScreenConnector("")},
                    {MainNavigatorRoutes.TeamMember, context => new TeamMemberScreenConnector("")},
                    {MainNavigatorRoutes.QRScanLogin, context => new QRScanLoginScreenConnector("")},
                    {MainNavigatorRoutes.Feedback, context => new FeedbackScreenConnector()},
                    {MainNavigatorRoutes.FeedbackType, context => new FeedbackTypeScreenConnector()},
                    {MainNavigatorRoutes.DiscoverChannel, context => new DiscoverChannelsScreenConnector()},
                    {MainNavigatorRoutes.ChannelScreen, context => new ChannelScreenConnector("")},
                    {MainNavigatorRoutes.ChannelDetail, context => new ChannelDetailScreenConnector("")},
                    {MainNavigatorRoutes.ChannelMembers, context => new ChannelMembersScreenConnector("")},
                    {MainNavigatorRoutes.ChannelIntroduction, context => new ChannelIntroductionScreenConnector("")},
                    {MainNavigatorRoutes.ReactionsDetail, context => new ReactionsDetailScreenConnector("")},
                    {MainNavigatorRoutes.HomeEvent, context => new HomeEventsScreenConnector()},
                    {MainNavigatorRoutes.Blogger, context => new BloggerScreenConnector()},
                    {MainNavigatorRoutes.ForceUpdate, context => new ForceUpdateScreen()},
                    {MainNavigatorRoutes.Game, context => new GameScreenConnector()}
                };

                if (Application.isEditor) {
                    var isExistSplash = SplashManager.isExistSplash();
                    if (isExistSplash) {
                        routes.Add(key: MainNavigatorRoutes.Root, context => new SplashScreen());
                        routes.Add(key: MainNavigatorRoutes.Main, context => new MainScreen());
                    }
                    else {
                        routes.Add(key: MainNavigatorRoutes.Root, context => new MainScreen());
                    }
                }
                else {
                    routes.Add(key: MainNavigatorRoutes.Splash, context => new SplashScreen());
                    routes.Add(key: MainNavigatorRoutes.Main, context => new MainScreen());
                    routes.Add(key: MainNavigatorRoutes.Root, context => new RootScreen());
                }

                if (VersionManager.needForceUpdate()) {
                    routes[key: MainNavigatorRoutes.Root] = context => new ForceUpdateScreen();
                }

                return routes;
            }
        }

        static Dictionary<string, WidgetBuilder> fullScreenRoutes {
            get {
                return new Dictionary<string, WidgetBuilder> {
                    {MainNavigatorRoutes.Search, context => new SearchScreenConnector()},
                    {MainNavigatorRoutes.Login, context => new LoginScreen()},
                    {MainNavigatorRoutes.ForceUpdate, context => new ForceUpdateScreen()}
                };
            }
        }

        public override Widget build(BuildContext context) {
            GlobalContext.context = context;
            return new WillPopScope(
                onWillPop: () => {
                    TipMenu.dismiss();
                    var promise = new Promise<bool>();
                    if (VersionManager.needForceUpdate()) {
                        promise.Resolve(false);
                    }
                    else if (LoginScreen.navigator?.canPop() ?? false) {
                        LoginScreen.navigator.pop();
                        promise.Resolve(false);
                    }
                    else if (Screen.orientation != ScreenOrientation.Portrait) {
                        //视频全屏时禁止物理返回按钮
                        EventBus.publish(sName: EventBusConstant.fullScreen, new List<object> {true});
                        promise.Resolve(false);
                    }
                    else if (navigator.canPop()) {
                        navigator.pop();
                        promise.Resolve(false);
                    }
                    else {
                        if (Application.platform == RuntimePlatform.Android) {
                            if (this._exitApp) {
                                CustomToast.hidden();
                                promise.Resolve(true);
                                if (this._timer != null) {
                                    this._timer.Dispose();
                                    this._timer = null;
                                }
                            }
                            else {
                                this._exitApp = true;
                                CustomToast.show(new CustomToastItem(
                                    context: context,
                                    "再按一次退出",
                                    TimeSpan.FromMilliseconds(2000)
                                ));
                                this._timer = Window.instance.run(TimeSpan.FromMilliseconds(2000),
                                    () => { this._exitApp = false; });
                                promise.Resolve(false);
                            }
                        }
                        else {
                            promise.Resolve(true);
                        }
                    }

                    return promise;
                },
                child: new Navigator(
                    key: globalKey,
                    observers: new List<NavigatorObserver> {
                        _routeObserve,
                        _heroController
                    },
                    onGenerateRoute: settings => new CustomPageRoute(
                        settings: settings,
                        fullscreenDialog: fullScreenRoutes.ContainsKey(key: settings.name),
                        builder: context1 => mainRoutes[key: settings.name](context: context1)
                    )
                )
            );
        }
    }
}