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
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.canvas {
    public class ConnectAppCanvas : UIWidgetsPanel {
        protected override void OnEnable() {
            base.OnEnable();
            Application.targetFrameRate = 300;
            var iconFont = Resources.Load<Font>("MaterialIcons");
            FontManager.instance.addFont(iconFont);
            var regularFont = Resources.Load<Font>("PingFang-Regular");
            FontManager.instance.addFont(regularFont);
            var mediumFont = Resources.Load<Font>("PingFang-Medium");
            FontManager.instance.addFont(mediumFont);
            var semiboldFont = Resources.Load<Font>("PingFang-Semibold");
            FontManager.instance.addFont(semiboldFont);
        }


        protected override Widget createWidget() {
            return new WidgetsApp(
                initialRoute: "/",
                textStyle: new TextStyle(fontSize: 24),
                pageRouteBuilder: pageRouteBuilder,
                routes: new Dictionary<string, WidgetBuilder> {
                    {"/", (context) => new MainScreen()},
//                    {"/", (context) => new TestScreen()},
                    {"/detail", (context) => new DetailScreen()},
                    {"/mine", (context) => new MineScreen()},
                    {"/setting", (context) => new SettingScreen()},
                    {"/login", (context) => new LoginScreen()},
                    {"/setting-unity", (context) => new BindUnityScreen()}
                });
        }

        protected Dictionary<string, WidgetBuilder> fullScreenRoutes => new Dictionary<string, WidgetBuilder> {
            {"/login", (context) => new LoginScreen()},
            {"/wechat-unity", (context) => new DetailScreen()},
        };

        protected PageRouteFactory pageRouteBuilder {
            get {
                return (RouteSettings settings, WidgetBuilder builder) =>
                    new PageRouteBuilder(
                        settings,
                        pageBuilder: (context, animation, secondaryAnimation) =>
                            new StoreProvider<AppState>(StoreProvider.store, builder(context)),
                        transitionsBuilder: (context, animation, secondaryAnimation, child) => {
                            if (fullScreenRoutes.ContainsKey(settings.name))
                                return new ModalPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                );
                            else
                                return new PushPageTransition(
                                    routeAnimation: animation,
                                    child: child
                                );
                        }
                    );
            }
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

        private static Tween<Offset> _bottomUpTween = new OffsetTween(
            new Offset(0.0f, 1.0f),
            Offset.zero
        );

        private static Animatable<float> _fastOutSlowInTween = new CurveTween(Curves.fastOutSlowIn);
        private static Animatable<float> _easeInTween = new CurveTween(Curves.easeIn);

        private readonly Animation<Offset> _positionAnimation;
        private readonly Animation<float> _opacityAnimation;
        public readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: _positionAnimation,
                child: child
            );
        }
    }
}