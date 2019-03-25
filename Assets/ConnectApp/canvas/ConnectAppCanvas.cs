using System.Collections.Generic;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.screens;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.canvas {
    public sealed class ConnectAppCanvas : UIWidgetsPanel {
        protected override void OnEnable() {
            base.OnEnable();
            Application.targetFrameRate = 60;
            FontManager.instance.addFont(Resources.Load<Font>("Material Icons"));
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Regular"));
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Medium"));
            FontManager.instance.addFont(Resources.Load<Font>("Roboto-Bold")); 
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Regular"));
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Medium"));
            FontManager.instance.addFont(Resources.Load<Font>("PingFangSC-Semibold"));
        }

        protected override Widget createWidget() {
            return new StoreProvider<AppState>(
                StoreProvider.store,
                new WidgetsApp(
                    home: new Router(),
                    pageRouteBuilder: pageRouteBuilder
                )
            );
        }

        private PageRouteFactory pageRouteBuilder {
            get {
                return (RouteSettings settings, WidgetBuilder builder) =>
                    new PageRouteBuilder(
                        settings,
                        pageBuilder: (BuildContext context, Animation<float> animation,
                            Animation<float> secondaryAnimation) => builder(context)
                    );
            }
        }
    }

    internal class Router : StatelessWidget {
        private static readonly GlobalKey globalKey = GlobalKey.key(debugLabel: "main-router");
        public static NavigatorState navigator => globalKey.currentState as NavigatorState;

        private static Dictionary<string, WidgetBuilder> mainRoutes => new Dictionary<string, WidgetBuilder> {
            {"/", context => new MainScreen()},
//            {"/", context => new TestScreen()},
            {"/search", context => new SearchScreen()},
            {"/event-detail", context => new EventDetailScreen()},
            {"/article-detail", context => new ArticleDetailScreen()},
            {"/mine", context => new MineScreen()},
            {"/setting", context => new SettingScreen()},
            {"/my-event", context => new MyEventsScreen()},
            {"/history", context => new HistoryScreen()},
            {"/login", context => new LoginSwitchScreen()},
            {"/bind-unity", context => new BindUnityScreen()}
        };

        private static Dictionary<string, WidgetBuilder> fullScreenRoutes => new Dictionary<string, WidgetBuilder> {
            {"/login", context => new LoginSwitchScreen()}
        };

        public override Widget build(BuildContext context) {
            return new Navigator(
//                globalKey,
                onGenerateRoute: (RouteSettings settings) => {
                    return new PageRouteBuilder(
                        settings,
                        (context1, animation, secondaryAnimation) => mainRoutes[settings.name](context1),
                        (context1, animation, secondaryAnimation, child) => {
                            if (fullScreenRoutes.ContainsKey(settings.name))
                                return new ModalPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                );
                            return new PushPageTransition(
                                routeAnimation: animation,
                                child: child
                            );
                        }
                    );
                }
            );
        }
    }

    internal class PushPageTransition : StatelessWidget {
        internal PushPageTransition(
            Key key = null,
            Animation<float> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) : base(key) {
            _positionAnimation = _leftPushTween.chain(_fastOutSlowInTween).animate(routeAnimation);
            this.child = child;
        }

        private readonly Tween<Offset> _leftPushTween = new OffsetTween(
            new Offset(1.0f, 0.0f),
            Offset.zero
        );

        private readonly Animatable<float> _fastOutSlowInTween = new CurveTween(Curves.fastOutSlowIn);
        private readonly Animation<Offset> _positionAnimation;
        private readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: _positionAnimation,
                child: child
            );
        }
    }


    internal class ModalPageTransition : StatelessWidget {
        internal ModalPageTransition(
            Key key = null,
            Animation<float> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) : base(key) {
            _positionAnimation = _bottomUpTween.chain(_fastOutSlowInTween).animate(routeAnimation);
            _opacityAnimation = _easeInTween.animate(routeAnimation);
            this.child = child;
        }

        private readonly Tween<Offset> _bottomUpTween = new OffsetTween(
            new Offset(0.0f, 1.0f),
            Offset.zero
        );

        private readonly Animatable<float> _fastOutSlowInTween = new CurveTween(Curves.fastOutSlowIn);
        private readonly Animatable<float> _easeInTween = new CurveTween(Curves.easeIn);

        private readonly Animation<Offset> _positionAnimation;
        private readonly Animation<float> _opacityAnimation;
        private readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: _positionAnimation,
                child: child
            );
        }
    }
}