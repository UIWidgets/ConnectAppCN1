using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.plugins;
using ConnectApp.screens;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Components {
    static class MainNavigatorRoutes {
        public const string Root = "/";
        public const string Search = "/search";
        public const string ArticleDetail = "/article-detail";
        public const string Setting = "/setting";
        public const string MyEvent = "/my-event";
        public const string History = "/history";
        public const string Login = "/login";
        public const string BindUnity = "/bind-unity";
        public const string Report = "/report";
        public const string AboutUs = "/aboutUs";
        public const string WebView = "/web-view";
    }

    class Router : StatelessWidget {
        static readonly GlobalKey globalKey = GlobalKey.key("main-router");
        bool _exitApp;
        Timer _timer;

        public static NavigatorState navigator {
            get { return globalKey.currentState as NavigatorState; }
        }

        static Dictionary<string, WidgetBuilder> mainRoutes {
            get {
                return new Dictionary<string, WidgetBuilder> {
                    {MainNavigatorRoutes.Root, context => new MainScreen()},
                    {MainNavigatorRoutes.Search, context => new SearchScreenConnector()},
                    {MainNavigatorRoutes.ArticleDetail, context => new ArticleDetailScreenConnector("")},
                    {MainNavigatorRoutes.Setting, context => new SettingScreenConnector()},
                    {MainNavigatorRoutes.MyEvent, context => new MyEventsScreenConnector()},
                    {MainNavigatorRoutes.History, context => new HistoryScreenConnector()},
                    {MainNavigatorRoutes.Login, context => new LoginScreen()},
                    {MainNavigatorRoutes.BindUnity, context => new BindUnityScreenConnector(FromPage.setting)},
                    {MainNavigatorRoutes.Report, context => new ReportScreenConnector("", ReportType.article)},
                    {MainNavigatorRoutes.AboutUs, context => new AboutUsScreenConnector()},
                    {MainNavigatorRoutes.WebView, context => new WebViewScreen()}
                };
            }
        }

        static Dictionary<string, WidgetBuilder> fullScreenRoutes {
            get {
                return new Dictionary<string, WidgetBuilder> {
                    {MainNavigatorRoutes.Search, context => new SearchScreenConnector()},
                    {MainNavigatorRoutes.Login, context => new LoginScreen()}
                };
            }
        }

        public override Widget build(BuildContext context) {
            JPushPlugin.context = context;
            return new WillPopScope(
                onWillPop: () => {
                    var promise = new Promise<bool>();
                    if (LoginScreen.navigator?.canPop() ?? false) {
                        LoginScreen.navigator.pop();
                        promise.Resolve(false);
                    }
                    else if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
                        //视频全屏时禁止物理返回按钮
                        promise.Resolve(false);
                    }
                    else if (navigator.canPop()) {
                        navigator.pop();
                        promise.Resolve(false);
                    }
                    else {
                        if (Config.platform == "android") {
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
                    onGenerateRoute: settings => {
                        return new PageRouteBuilder(
                            settings: settings,
                            (context1, animation, secondaryAnimation) => mainRoutes[settings.name](context1),
                            (context1, animation, secondaryAnimation, child) => {
                                if (fullScreenRoutes.ContainsKey(settings.name)) {
                                    return new ModalPageTransition(
                                        routeAnimation: animation,
                                        child: child
                                    );
                                }

                                return new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                );
                            }
                        );
                    }
                )
            );
        }
    }

    class PushPageTransition : StatelessWidget {
        internal PushPageTransition(
            Key key = null,
            Animation<float> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) : base(key: key) {
            this._positionAnimation = this._leftPushTween.chain(this._fastOutSlowInTween).animate(routeAnimation);
            this.child = child;
        }

        readonly Tween<Offset> _leftPushTween = new OffsetTween(
            new Offset(1.0f, 0.0f),
            Offset.zero
        );

        readonly Animatable<float> _fastOutSlowInTween = new CurveTween(Curves.fastOutSlowIn);
        readonly Animation<Offset> _positionAnimation;
        readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: this._positionAnimation,
                child: this.child
            );
        }
    }

    class ModalPageTransition : StatelessWidget {
        internal ModalPageTransition(
            Key key = null,
            Animation<float> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) : base(key: key) {
            this._positionAnimation = this._bottomUpTween.chain(this._fastOutSlowInTween).animate(routeAnimation);
            this.child = child;
        }

        readonly Tween<Offset> _bottomUpTween = new OffsetTween(
            new Offset(0.0f, 1.0f),
            Offset.zero
        );

        readonly Animatable<float> _fastOutSlowInTween = new CurveTween(Curves.fastOutSlowIn);
        readonly Animation<Offset> _positionAnimation;
        readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: this._positionAnimation,
                child: this.child
            );
        }
    }
}