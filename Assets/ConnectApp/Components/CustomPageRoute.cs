using System;
using System.Collections.Generic;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Canvas = Unity.UIWidgets.ui.Canvas;
using Color = Unity.UIWidgets.ui.Color;
using Rect = Unity.UIWidgets.ui.Rect;

namespace ConnectApp.Components {
    public static class CustomPageRouteUtils {
        public const float _kBackGestureWidth = 40.0f;
        public const float _kMinFlingVelocity = 1.0f;
        public const int _kMaxDroppedSwipePageForwardAnimationTime = 800; // Milliseconds.
        public const int _kMaxPageBackAnimationTime = 300; // Milliseconds.

        public static readonly Animatable<Offset> _kRightMiddleTween = new OffsetTween(
            new Offset(1.0f, 0.0f),
            end: Offset.zero
        );

        public static readonly Animatable<Offset> _kMiddleLeftTween = new OffsetTween(
            begin: Offset.zero,
            new Offset(-1.0f / 3.0f, 0.0f)
        );

        public static readonly Animatable<Offset> _kBottomUpTween = new OffsetTween(
            new Offset(0.0f, 1.0f),
            end: Offset.zero
        );

        public static readonly DecorationTween _kGradientShadowTween = new DecorationTween(
            begin: _CustomEdgeShadowDecoration.none,
            end: new _CustomEdgeShadowDecoration(
                edgeGradient: new LinearGradient(
                    begin: new Alignment(0.9f, 0.0f),
                    end: Alignment.centerRight,
                    colors: new List<Color> {
                        new Color(0x00000000),
                        new Color(0x04000000),
                        new Color(0x12000000),
                        new Color(0x38000000)
                    },
                    stops: new List<float> {0.0f, 0.3f, 0.6f, 1.0f}
                )
            )
        );
    }

    public class CustomPageRoute : PageRoute {
        public CustomPageRoute(
            WidgetBuilder builder,
            RouteSettings settings = null,
            string title = "",
            bool maintainState = true,
            bool fullscreenDialog = false,
            bool push = false,
            RouteTransitionsBuilder overrideTransitionsBuilder = null
        ) : base(settings: settings, fullscreenDialog: fullscreenDialog) {
            D.assert(builder != null);
            D.assert(this.opaque);
            this.builder = builder;
            this.title = title;
            this.maintainState = maintainState;
            this.push = push;
            this.overrideTransitionsBuilder = overrideTransitionsBuilder;
        }

        readonly WidgetBuilder builder;
        readonly string title;
        readonly RouteTransitionsBuilder overrideTransitionsBuilder;
        readonly bool push;
        ValueNotifier<string> _previousTitle;

        public ValueListenable<string> previousTitle {
            get {
                D.assert(
                    this._previousTitle != null,
                    () => "Cannot read the previousTitle for a route that has not yet been installed"
                );
                return this._previousTitle;
            }
        }

        protected override void didChangePrevious(Route previousRoute) {
            string previousTitleString = previousRoute is CustomPageRoute
                ? ((CustomPageRoute) previousRoute).title
                : null;
            if (this._previousTitle == null) {
                this._previousTitle = new ValueNotifier<string>(previousTitleString);
            }

            else {
                this._previousTitle.value = previousTitleString;
            }

            base.didChangePrevious(previousRoute);
        }

        public override bool maintainState { get; }

        public override TimeSpan transitionDuration {
            get { return new TimeSpan(0, 0, 0, 0, 400); }
        }

        public override Color barrierColor {
            get { return null; }
        }

        public string barrierLabel {
            get { return null; }
        }

        public override bool canTransitionFrom(TransitionRoute previousRoute) {
            return previousRoute is CustomPageRoute;
        }

        public override bool canTransitionTo(TransitionRoute nextRoute) {
            return nextRoute is CustomPageRoute && !((CustomPageRoute) nextRoute).fullscreenDialog;
        }

        static bool isPopGestureInProgress(PageRoute route) {
            return route.navigator.userGestureInProgress;
        }

        public bool popGestureInProgress {
            get { return isPopGestureInProgress(this); }
        }

        public bool popGestureEnabled {
            get { return _isPopGestureEnabled(this); }
        }

        static bool _isPopGestureEnabled(PageRoute route) {
            if (route.isFirst) {
                return false;
            }

            if (route.willHandlePopInternally) {
                return false;
            }

            if (route.hasScopedWillPopCallback) {
                return false;
            }

            if (route.fullscreenDialog) {
                return false;
            }

            if (route.animation.status != AnimationStatus.completed) {
                return false;
            }

            if (route.secondaryAnimation.status != AnimationStatus.dismissed) {
                return false;
            }

            if (isPopGestureInProgress(route)) {
                return false;
            }

            return true;
        }


        public override Widget buildPage(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation) {
            Widget result = this.builder(context);

            D.assert(() => {
                if (result == null) {
                    throw new UIWidgetsError(
                        $"The builder for route {this.settings.name} returned null.\nRoute builders must never return null.");
                }

                return true;
            });

            return result;
        }

        static _CustomPageRouteBackGestureController _startPopGesture(PageRoute route) {
            D.assert(_isPopGestureEnabled(route));
            return new _CustomPageRouteBackGestureController(
                navigator: route.navigator,
                controller: route.controller
            );
        }

        Widget buildPageTransitions(
            PageRoute route,
            BuildContext context,
            Animation<float> animation,
            Animation<float> secondaryAnimation,
            Widget child
        ) {
            if (route.fullscreenDialog) {
                return new CustomFullscreenDialogTransition(
                    animation: animation,
                    child: child
                );
            }

            if (this.push) {
                return new PushPageTransition(
                    animation: animation,
                    child: child
                );
            }

            return new CustomPageTransition(
                primaryRouteAnimation: animation,
                secondaryRouteAnimation: secondaryAnimation,
                linearTransition: isPopGestureInProgress(route),
                child: new _CustomPageBackGestureDetector(
                    enabledCallback: () => _isPopGestureEnabled(route),
                    onStartPopGesture: () => _startPopGesture(route),
                    child: child
                )
            );
        }

        public override Widget buildTransitions(BuildContext context, Animation<float> animation,
            Animation<float> secondaryAnimation, Widget child) {
            if (this.overrideTransitionsBuilder != null) {
                return this.overrideTransitionsBuilder(context, animation, secondaryAnimation, child);
            }

            return this.buildPageTransitions(this, context, animation, secondaryAnimation, child);
        }

        public new string debugLabel {
            get { return $"{base.debugLabel}(${this.settings.name})"; }
        }
    }

    class _CustomPageBackGestureDetector : StatefulWidget {
        public _CustomPageBackGestureDetector(
            Widget child,
            ValueGetter<bool> enabledCallback,
            ValueGetter<_CustomPageRouteBackGestureController> onStartPopGesture,
            Key key = null
        ) : base(key: key) {
            D.assert(enabledCallback != null);
            D.assert(onStartPopGesture != null);
            D.assert(child != null);
            this.child = child;
            this.enabledCallback = enabledCallback;
            this.onStartPopGesture = onStartPopGesture;
        }

        public readonly Widget child;

        public readonly ValueGetter<bool> enabledCallback;

        public readonly ValueGetter<_CustomPageRouteBackGestureController> onStartPopGesture;

        public override State createState() {
            return new _CustomPageBackGestureDetectorState();
        }
    }

    class _CustomPageBackGestureDetectorState : State<_CustomPageBackGestureDetector> {
        _CustomPageRouteBackGestureController _backGestureController;
        HorizontalDragGestureRecognizer _recognizer;


        public override void initState() {
            base.initState();
            this._recognizer = new HorizontalDragGestureRecognizer(debugOwner: this);
            this._recognizer.onStart = this._handleDragStart;
            this._recognizer.onUpdate = this._handleDragUpdate;
            this._recognizer.onEnd = this._handleDragEnd;
            this._recognizer.onCancel = this._handleDragCancel;
        }

        public override void dispose() {
            this._recognizer.dispose();
            base.dispose();
        }

        void _handleDragStart(DragStartDetails details) {
            D.assert(this.mounted);
            D.assert(this._backGestureController == null);
            this._backGestureController = this.widget.onStartPopGesture();
        }

        void _handleDragUpdate(DragUpdateDetails details) {
            D.assert(this.mounted);
            D.assert(this._backGestureController != null);
            this._backGestureController.dragUpdate(
                this._convertToLogical(details.primaryDelta / this.context.size.width));
        }

        void _handleDragEnd(DragEndDetails details) {
            D.assert(this.mounted);
            D.assert(this._backGestureController != null);
            this._backGestureController.dragEnd(
                this._convertToLogical(details.velocity.pixelsPerSecond.dx / this.context.size.width) ?? 0);
            this._backGestureController = null;
        }

        void _handleDragCancel() {
            D.assert(this.mounted);
            this._backGestureController?.dragEnd(0.0f);
            this._backGestureController = null;
        }

        void _handlePointerDown(PointerDownEvent evt) {
            if (this.widget.enabledCallback()) {
                this._recognizer.addPointer(evt: evt);
            }
        }

        float? _convertToLogical(float? value) {
            switch (Directionality.of(context: this.context)) {
                case TextDirection.rtl:
                    return -value;
                case TextDirection.ltr:
                    return value;
                default:
                    return value;
            }
        }


        public override Widget build(BuildContext context) {
            float dragAreaWidth = Directionality.of(context: context) == TextDirection.ltr
                ? MediaQuery.of(context: context).padding.left
                : MediaQuery.of(context: context).padding.right;
            dragAreaWidth = Mathf.Max(a: dragAreaWidth, b: CustomPageRouteUtils._kBackGestureWidth);
            return new Stack(
                fit: StackFit.passthrough,
                children: new List<Widget> {
                    this.widget.child,
                    new Positioned(
                        left: 0.0f,
                        width: dragAreaWidth,
                        top: 0.0f,
                        bottom: 0.0f,
                        child: new Listener(
                            onPointerDown: this._handlePointerDown,
                            behavior: HitTestBehavior.translucent
                        )
                    )
                }
            );
        }
    }

    class _CustomPageRouteBackGestureController {
        public _CustomPageRouteBackGestureController(
            NavigatorState navigator,
            AnimationController controller
        ) {
            D.assert(navigator != null);
            D.assert(controller != null);

            this.navigator = navigator;
            this.controller = controller;
            this.navigator.didStartUserGesture();
        }

        readonly AnimationController controller;
        readonly NavigatorState navigator;

        public void dragUpdate(float? delta) {
            if (delta != null) {
                this.controller.setValue(this.controller.value - (float) delta);
            }
        }

        public void dragEnd(float velocity) {
            Curve animationCurve = Curves.fastLinearToSlowEaseIn;
            bool animateForward;

            if (velocity.abs() >= CustomPageRouteUtils._kMinFlingVelocity) {
                animateForward = velocity <= 0;
            }
            else {
                animateForward = this.controller.value > 0.5;
            }

            if (animateForward) {
                int droppedPageForwardAnimationTime = Mathf.Min(
                    MathUtils.lerpFloat(a: CustomPageRouteUtils._kMaxDroppedSwipePageForwardAnimationTime, 0f,
                        t: this.controller.value).floor(),
                    b: CustomPageRouteUtils._kMaxPageBackAnimationTime
                );
                this.controller.animateTo(1.0f, TimeSpan.FromMilliseconds(value: droppedPageForwardAnimationTime),
                    curve: animationCurve);
            }
            else {
                this.navigator.pop();

                if (this.controller.isAnimating) {
                    int droppedPageBackAnimationTime =
                        MathUtils.lerpFloat(0f, b: CustomPageRouteUtils._kMaxDroppedSwipePageForwardAnimationTime,
                            t: this.controller.value).floor();
                    this.controller.animateBack(0.0f, TimeSpan.FromMilliseconds(value: droppedPageBackAnimationTime),
                        curve: animationCurve);
                }
            }

            if (this.controller.isAnimating) {
                AnimationStatusListener animationStatusCallback = null;
                animationStatusCallback = (AnimationStatus status) => {
                    this.navigator.didStopUserGesture();
                    this.controller.removeStatusListener(animationStatusCallback);
                };
                this.controller.addStatusListener(animationStatusCallback);
            }
            else {
                this.navigator.didStopUserGesture();
            }
        }
    }

    class CustomPageTransition : StatelessWidget {
        public CustomPageTransition(
            Animation<float> primaryRouteAnimation,
            Animation<float> secondaryRouteAnimation,
            Widget child,
            bool linearTransition,
            Key key = null
        ) : base(key: key) {
            this._primaryPositionAnimation =
                (linearTransition
                    ? primaryRouteAnimation
                    : new CurvedAnimation(
                        parent: primaryRouteAnimation,
                        curve: Curves.linearToEaseOut,
                        reverseCurve: Curves.easeInToLinear
                    )
                ).drive(child: CustomPageRouteUtils._kRightMiddleTween);

            this._secondaryPositionAnimation =
                (linearTransition
                    ? secondaryRouteAnimation
                    : new CurvedAnimation(
                        parent: secondaryRouteAnimation,
                        curve: Curves.linearToEaseOut,
                        reverseCurve: Curves.easeInToLinear
                    )
                ).drive(child: CustomPageRouteUtils._kMiddleLeftTween);
            this._primaryShadowAnimation =
                (linearTransition
                    ? primaryRouteAnimation
                    : new CurvedAnimation(
                        parent: primaryRouteAnimation,
                        curve: Curves.linearToEaseOut
                    )
                ).drive(child: CustomPageRouteUtils._kGradientShadowTween);
            this.child = child;
        }

        readonly Animation<Offset> _primaryPositionAnimation;
        readonly Animation<Offset> _secondaryPositionAnimation;
        readonly Animation<Decoration> _primaryShadowAnimation;
        readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: this._secondaryPositionAnimation,
                transformHitTests: false,
                child: new SlideTransition(
                    position: this._primaryPositionAnimation,
                    child: new DecoratedBoxTransition(
                        decoration: this._primaryShadowAnimation,
                        child: this.child
                    )
                )
            );
        }
    }

    class CustomFullscreenDialogTransition : StatelessWidget {
        public CustomFullscreenDialogTransition(
            Animation<float> animation,
            Widget child,
            Key key = null
        ) : base(key: key) {
            this._positionAnimation = new CurvedAnimation(
                parent: animation,
                curve: Curves.linearToEaseOut,
                reverseCurve: Curves.linearToEaseOut.flipped
            ).drive(child: CustomPageRouteUtils._kBottomUpTween);
            this.child = child;
        }

        readonly Animation<Offset> _positionAnimation;
        readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: this._positionAnimation,
                child: this.child
            );
        }
    }

    class PushPageTransition : StatelessWidget {
        public PushPageTransition(
            Animation<float> animation,
            Widget child,
            Key key = null
        ) : base(key: key) {
            this._positionAnimation = this._leftPushTween.chain(parent: this._fastOutSlowInTween)
                .animate(parent: animation);
            this.child = child;
        }

        readonly Tween<Offset> _leftPushTween = new OffsetTween(
            new Offset(1.0f, 0.0f),
            end: Offset.zero
        );

        readonly Animatable<float> _fastOutSlowInTween = new CurveTween(curve: Curves.fastOutSlowIn);
        readonly Animation<Offset> _positionAnimation;
        readonly Widget child;

        public override Widget build(BuildContext context) {
            return new SlideTransition(
                position: this._positionAnimation,
                child: this.child
            );
        }
    }

    class _CustomEdgeShadowDecoration : Decoration, IEquatable<_CustomEdgeShadowDecoration> {
        public _CustomEdgeShadowDecoration(
            LinearGradient edgeGradient = null
        ) {
            this.edgeGradient = edgeGradient;
        }

        public static readonly _CustomEdgeShadowDecoration none = new _CustomEdgeShadowDecoration();

        public readonly LinearGradient edgeGradient;

        static _CustomEdgeShadowDecoration lerpCustom(
            _CustomEdgeShadowDecoration a,
            _CustomEdgeShadowDecoration b,
            float t
        ) {
            if (a == null && b == null) {
                return null;
            }

            return new _CustomEdgeShadowDecoration(
                edgeGradient: LinearGradient.lerp(a?.edgeGradient, b?.edgeGradient, t)
            );
        }

        public override Decoration lerpFrom(Decoration a, float t) {
            if (!(a is _CustomEdgeShadowDecoration)) {
                return lerpCustom(null, this, t);
            }

            return lerpCustom((_CustomEdgeShadowDecoration) a, this, t);
        }

        public override Decoration lerpTo(Decoration b, float t) {
            if (!(b is _CustomEdgeShadowDecoration)) {
                return lerpCustom(this, null, t);
            }

            return lerpCustom(this, (_CustomEdgeShadowDecoration) b, t);
        }

        public override BoxPainter createBoxPainter(VoidCallback onChanged = null) {
            return new _CustomEdgeShadowPainter(this, onChanged);
        }

        public override int GetHashCode() {
            return this.edgeGradient.GetHashCode();
        }

        public bool Equals(_CustomEdgeShadowDecoration other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            return Equals(this.edgeGradient, other.edgeGradient);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            return this.Equals((_CustomEdgeShadowDecoration) obj);
        }

        public static bool operator ==(_CustomEdgeShadowDecoration left, _CustomEdgeShadowDecoration right) {
            return Equals(left, right);
        }

        public static bool operator !=(_CustomEdgeShadowDecoration left, _CustomEdgeShadowDecoration right) {
            return !Equals(left, right);
        }

        public int hashCode {
            get { return this.edgeGradient.GetHashCode(); }
        }

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new DiagnosticsProperty<LinearGradient>("edgeGradient", this.edgeGradient));
        }
    }

    class _CustomEdgeShadowPainter : BoxPainter {
        public _CustomEdgeShadowPainter(
            _CustomEdgeShadowDecoration decoration = null,
            VoidCallback onChange = null
        ) : base(onChanged: onChange) {
            D.assert(decoration != null);
            this._decoration = decoration;
        }

        readonly _CustomEdgeShadowDecoration _decoration;

        public override void paint(Canvas canvas, Offset offset, ImageConfiguration configuration) {
            LinearGradient gradient = this._decoration.edgeGradient;
            if (gradient == null) {
                return;
            }

            float deltaX = -configuration.size.width;
            Rect rect = (offset & configuration.size).translate(translateX: deltaX, 0.0f);
            Paint paint = new Paint {
                shader = gradient.createShader(rect: rect)
            };
            canvas.drawRect(rect: rect, paint: paint);
        }
    }
}