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

namespace ConnectApp.canvas
{
    public class ConnectAppCanvas : WidgetCanvas
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            var font = Resources.Load<Font>("MaterialIcons-Regular");
            FontManager.instance.addFont(font);
        }

        protected override string initialRoute => "/";

        protected override Dictionary<string, WidgetBuilder> routes => new Dictionary<string, WidgetBuilder>
        {
            {"/", (context) => new EventsScreen()},
            {"/detail", (context) => new DetailScreen()},
            {"/mine", (context) => new MineScreen()},
            {"/setting", (context) => new SettingScreen()},
            {"/login", (context) => new LoginScreen()},
            {"/setting-unity", (context) => new BindUnityScreen()}
        };

        protected Dictionary<string, WidgetBuilder> fullScreenRoutes => new Dictionary<string, WidgetBuilder>
        {
            {"/login", (context) => new LoginScreen()},
            {"/wechat-unity", (context) => new DetailScreen()},
        };

        protected override TextStyle textStyle => new TextStyle(fontSize: 24);

        protected override PageRouteFactory pageRouteBuilder => (RouteSettings settings, WidgetBuilder builder) =>
            new PageRouteBuilder(
                settings,
                (context, animation, secondaryAnimation) =>
                    new StoreProvider<AppState>(StoreProvider.store, builder(context)),
                (context, animation, secondaryAnimation, child) =>
                {
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

    internal class PushPageTransition : StatelessWidget
    {
        internal PushPageTransition(
            Key key = null,
            Animation<double> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) : base(key)
        {
            _positionAnimation = _leftPushTween.chain(_fastOutSlowInTween).animate(routeAnimation);
            this.child = child;
        }

        private readonly Tween<Offset> _leftPushTween = new OffsetTween(
            new Offset(1.0, 0.0),
            Offset.zero
        );

        private readonly Animatable<double> _fastOutSlowInTween = new CurveTween(curve: Curves.fastOutSlowIn);
        private readonly Animation<Offset> _positionAnimation;
        private readonly Widget child;

        public override Widget build(BuildContext context)
        {
            return new SlideTransition(
                position: _positionAnimation,
                child: child
            );
        }
    }


    internal class ModalPageTransition : StatelessWidget
    {
        internal ModalPageTransition(
            Key key = null,
            Animation<double> routeAnimation = null, // The route's linear 0.0 - 1.0 animation.
            Widget child = null
        ) : base(key)
        {
            _positionAnimation = _bottomUpTween.chain(_fastOutSlowInTween).animate(routeAnimation);
            _opacityAnimation = _easeInTween.animate(routeAnimation);
            this.child = child;
        }

        private static Tween<Offset> _bottomUpTween = new OffsetTween(
            new Offset(0.0, 1.0),
            Offset.zero
        );

        private static Animatable<double> _fastOutSlowInTween = new CurveTween(curve: Curves.fastOutSlowIn);
        private static Animatable<double> _easeInTween = new CurveTween(curve: Curves.easeIn);

        private readonly Animation<Offset> _positionAnimation;
        private readonly Animation<double> _opacityAnimation;
        public readonly Widget child;

        public override Widget build(BuildContext context)
        {
            return new SlideTransition(
                position: _positionAnimation,
                child: child
            );
        }
    }
}