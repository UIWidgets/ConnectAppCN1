using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace Unity.UIWidgets.Samples.ConnectApp {
    public class ConnectAppCanvas : WidgetCanvas {
        protected override void OnEnable() {
            base.OnEnable();
            var font = Resources.Load<Font>("MaterialIcons-Regular");
            FontManager.instance.addFont(font);
        }

        protected override string initialRoute => "/";

        protected override Dictionary<string, WidgetBuilder> routes => new Dictionary<string, WidgetBuilder> {
            {"/", (context) => new EventsScreen()},
            {"/detail", (context) => new DetailScreen()},
            {"/mine", (context) => new MineScreen()},
            {"/setting", (context) => new SettingScreen()},
            {"/login", (context) => new LoginScreen()},
            {"/setting-unity", (context) => new BindUnityScreen()}
        };

        protected Dictionary<string, WidgetBuilder> fullScreenRoutes => new Dictionary<string, WidgetBuilder> {
            {"/login", (context) => new LoginScreen()},
            {"/wechat-unity", (context) => new DetailScreen()},
        };

        protected override TextStyle textStyle => new TextStyle(fontSize: 24);

        protected override PageRouteFactory pageRouteBuilder => (RouteSettings settings, WidgetBuilder builder) =>
            new PageRouteBuilder(
                settings,
                (context, animation, secondaryAnimation) => builder(context),
                (context, animation, secondaryAnimation, child) => {
                    if (this.fullScreenRoutes.ContainsKey(settings.name)) {
                        return new _ModalPageTransition(
                            routeAnimation: animation,
                            child: child
                        );
                    }
                    else {
                        return new _PushPageTransition(
                            routeAnimation: animation,
                            child: child
                        );
                    }
                }
            );
    }

    internal class _PushPageTransition : StatelessWidget {
        internal _PushPageTransition(
            Key key = null,
            Animation<double> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) : base(key) {
            this._positionAnimation = this._leftPushTween.chain(this._fastOutSlowInTween).animate(routeAnimation);
            this.child = child;
        }

        readonly Tween<Offset> _leftPushTween = new OffsetTween(
            new Offset(1.0, 0.0),
            Offset.zero
        );

        readonly Animatable<double> _fastOutSlowInTween = new CurveTween(curve: Curves.fastOutSlowIn);
        readonly Animation<Offset> _positionAnimation;
        readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: this._positionAnimation,
                child: this.child
            );
        }
    }


    internal class _ModalPageTransition : StatelessWidget {
        internal _ModalPageTransition(
            Key key = null,
            Animation<double> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) : base(key) {
            this._positionAnimation = _bottomUpTween.chain(_fastOutSlowInTween).animate(routeAnimation);
            this._opacityAnimation = _easeInTween.animate(routeAnimation);
            this.child = child;
        }

        static Tween<Offset> _bottomUpTween = new OffsetTween(
            new Offset(0.0, 1.0),
            Offset.zero
        );

        static Animatable<double> _fastOutSlowInTween = new CurveTween(curve: Curves.fastOutSlowIn);
        static Animatable<double> _easeInTween = new CurveTween(curve: Curves.easeIn);

        readonly Animation<Offset> _positionAnimation;
        readonly Animation<double> _opacityAnimation;
        public readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: this._positionAnimation,
                child: this.child
            );
        }
    }
}