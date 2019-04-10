using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.screens;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Icons = ConnectApp.constants.Icons;

namespace ConnectApp.components {
    internal class _SnackBarRoute : OverlayRoute {
        public _SnackBarRoute(
            CustomSnackBar customSnackBar = null,
            RouteSettings settings = null
        ) : base(settings) {
            this.customSnackBar = customSnackBar;
            _builder = new Builder(builder: innerContext => customSnackBar);
        }

        private readonly CustomSnackBar customSnackBar;

        private readonly Builder _builder;
        private readonly Alignment _initialAlignment = new Alignment(-1, 2);
        private readonly Alignment _endAlignment = new Alignment(-1, 1);
        public Timer _timer;

        public bool opaque => false;

        public override ICollection<OverlayEntry> createOverlayEntries() {
            return new List<OverlayEntry> {
                new OverlayEntry(
                    context => {
                        var annotatedChild = new AlignTransition(
                            alignment: animation,
                            child: _builder
                        );
                        return annotatedChild;
                    },
                    maintainState: false,
                    opaque: opaque
                )
            };
        }

        protected override bool finishedWhenPopped => controller.status == AnimationStatus.dismissed;

        private Animation<Alignment> animation => _animation;
        private Animation<Alignment> _animation;

        private AnimationController controller => _controller;
        private AnimationController _controller;

        private AnimationController createAnimationController() {
            var duration = new TimeSpan(0, 0, 1);
            D.assert(duration != null && duration >= TimeSpan.Zero);
            return new AnimationController(
                duration: duration,
                debugLabel: debugLabel,
                vsync: navigator
            );
        }

        private Animation<Alignment> createAnimation() {
            D.assert(_controller != null);
            return new AlignmentTween(_initialAlignment, _endAlignment).animate(
                new CurvedAnimation(
                    _controller,
                    customSnackBar.forwardAnimationCurve,
                    customSnackBar.reverseAnimationCurve
                )
            );
        }

        protected override void install(OverlayEntry insertionPoint) {
            _controller = createAnimationController();
            D.assert(_controller != null, $"runtimeType.createAnimationController() returned null.");
            _animation = createAnimation();
            D.assert(_animation != null, "runtimeType.createAnimation() returned null.");
            base.install(insertionPoint);
        }

        protected override TickerFuture didPush() {
            D.assert(_controller != null,
                "runtimeType.didPush called before calling install() or after calling dispose().");
            _configureTimer();
            return _controller.forward();
        }

        protected override bool didPop(object result) {
            D.assert(_controller != null,
                "runtimeType.didPop called before calling install() or after calling dispose().");

            _cancelTimer();
            _controller.reverse();
            return base.didPop(result);
        }

        private void _configureTimer() {
            if (_timer != null) return;
            _timer = Window.instance.run(customSnackBar.duration, () => { navigator.pop(); });
        }

        private void _cancelTimer() {
            if (_timer != null) _timer.cancel();
        }

        public static string debugLabel => "runtimeType";
    }

    public class CustomSnackBar : StatefulWidget {
        public CustomSnackBar(
            string message,
            TimeSpan duration,
            Key key = null
        ) : base(key) {
            D.assert(message != null);
            this.message = message;
            this.duration = duration;
        }

        public readonly string message;
        public readonly TimeSpan duration;
        public readonly Curve forwardAnimationCurve = Curves.easeOut;
        public readonly Curve reverseAnimationCurve = Curves.fastOutSlowIn;

        private _SnackBarRoute _snackBarRoute;

        public void show() {
            D.assert(this != null);
            _snackBarRoute = new _SnackBarRoute(
                this,
                new RouteSettings("/snackBarRoute")
            );
            StoreProvider.store.dispatcher.dispatch(new MainNavigatorPushToRouteAction{route = _snackBarRoute});
        }

        public void dismiss() {
            if (_snackBarRoute == null) return;

            if (_snackBarRoute.isCurrent)
                StoreProvider.store.dispatcher.dispatch(new MainNavigatorPopAction());
            else if (_snackBarRoute.isActive) _snackBarRoute.navigator.removeRoute(_snackBarRoute);
            if (_snackBarRoute._timer != null) {
                _snackBarRoute._timer.cancel();
                _snackBarRoute._timer = null;
            }
        }

        public override State createState() {
            return new _CustomSnackBarState();
        }
    }

    internal class _CustomSnackBarState : State<CustomSnackBar> {
        public override Widget build(BuildContext context) {
            var mediaQuery = MediaQuery.of(context);
            return new Container(
                width: mediaQuery.size.width,
                child: new DecoratedBox(
                    decoration: new BoxDecoration(
                        CColors.White,
                        border: new Border(
                            new BorderSide(
                                CColors.Separator
                            )
                        )
                    ),
                    child: new Padding(
                        padding: EdgeInsets.only(
                            16,
                            13.5f,
                            8,
                            13.5f + mediaQuery.padding.bottom
                        ),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: new List<Widget> {
                                new Expanded(
                                    child: new Text(
                                        widget.message,
                                        maxLines: 3,
                                        style: CTextStyle.PRegularError
                                    )
                                ),
                                new CustomButton(
                                    padding: EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                                    onPressed: widget.dismiss,
                                    child: new Icon(
                                        Icons.close,
                                        size: 24,
                                        color: new Color(0xFFC7CBCF)
                                    )
                                )
                            }
                        )
                    )
                )
            );
        }
    }
}