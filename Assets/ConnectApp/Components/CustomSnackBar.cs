using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Main;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    class _SnackBarRoute : OverlayRoute {
        public _SnackBarRoute(
            CustomSnackBar customSnackBar = null,
            RouteSettings settings = null
        ) : base(settings) {
            this.customSnackBar = customSnackBar;
            this._builder = new Builder(builder: innerContext => customSnackBar);
        }

        readonly CustomSnackBar customSnackBar;

        readonly Builder _builder;
        readonly Alignment _initialAlignment = new Alignment(-1, 2);
        readonly Alignment _endAlignment = new Alignment(-1, 1);
        public Timer _timer;

        public bool opaque {
            get { return false; }
        }

        public override ICollection<OverlayEntry> createOverlayEntries() {
            return new List<OverlayEntry> {
                new OverlayEntry(
                    context => {
                        var annotatedChild = new AlignTransition(
                            alignment: this.animation,
                            child: this._builder
                        );
                        return annotatedChild;
                    },
                    maintainState: false,
                    opaque: this.opaque
                )
            };
        }

        protected override bool finishedWhenPopped {
            get { return this.controller.status == AnimationStatus.dismissed; }
        }

        Animation<Alignment> animation {
            get { return this._animation; }
        }

        Animation<Alignment> _animation;

        AnimationController controller {
            get { return this._controller; }
        }

        AnimationController _controller;

        AnimationController createAnimationController() {
            var duration = new TimeSpan(0, 0, 1);
            D.assert(duration != null && duration >= TimeSpan.Zero);
            return new AnimationController(
                duration: duration,
                debugLabel: this.debugLabel,
                vsync: this.navigator
            );
        }

        Animation<Alignment> createAnimation() {
            D.assert(this._controller != null);
            return new AlignmentTween(this._initialAlignment, this._endAlignment).animate(
                new CurvedAnimation(this._controller, this.customSnackBar.forwardAnimationCurve,
                    this.customSnackBar.reverseAnimationCurve
                )
            );
        }

        protected override void install(OverlayEntry insertionPoint) {
            this._controller = this.createAnimationController();
            D.assert(this._controller != null, () => $"{this.GetType()}.createAnimationController() returned null.");
            this._animation = this.createAnimation();
            D.assert(this._animation != null, () => $"{this.GetType()}.createAnimation() returned null.");
            base.install(insertionPoint);
        }

        protected override TickerFuture didPush() {
            D.assert(this._controller != null,
                () => $"{this.GetType()}.didPush called before calling install() or after calling dispose().");
            this._configureTimer();
            return this._controller.forward();
        }

        protected override bool didPop(object result) {
            D.assert(this._controller != null,
                () => $"{this.GetType()}.didPop called before calling install() or after calling dispose().");

            this._cancelTimer();
            this._controller.reverse();
            return base.didPop(result);
        }

        void _configureTimer() {
            if (this._timer != null) {
                return;
            }

            this._timer = Window.instance.run((TimeSpan) this.customSnackBar.duration, () => { this.navigator.pop(); });
        }

        void _cancelTimer() {
            if (this._timer != null) {
                this._timer.cancel();
            }
        }

        public string debugLabel {
            get { return $"{this.GetType()}"; }
        }
    }

    public class CustomSnackBar : StatefulWidget {
        public CustomSnackBar(
            string message,
            TimeSpan? duration = null,
            Color color = null,
            Key key = null
        ) : base(key) {
            D.assert(message != null);
            this.message = message;
            this.duration = duration ?? new TimeSpan(0, 0, 0, 2);
            this.color = color ?? CColors.Error;
        }

        public readonly string message;
        public readonly TimeSpan? duration;
        public readonly Color color;
        public readonly Curve forwardAnimationCurve = Curves.easeOut;
        public readonly Curve reverseAnimationCurve = Curves.fastOutSlowIn;

        _SnackBarRoute _snackBarRoute;

        public void show() {
            D.assert(this != null);
            this._snackBarRoute = new _SnackBarRoute(
                this,
                new RouteSettings("/snackBarRoute")
            );
            Router.navigator.push(this._snackBarRoute);
        }

        public void dismiss() {
            if (this._snackBarRoute == null) {
                return;
            }

            if (this._snackBarRoute.isCurrent) {
                Router.navigator.pop();
            }
            else if (this._snackBarRoute.isActive) {
                this._snackBarRoute.navigator.removeRoute(this._snackBarRoute);
            }

            if (this._snackBarRoute._timer != null) {
                this._snackBarRoute._timer.cancel();
                this._snackBarRoute._timer = null;
            }
        }

        public override State createState() {
            return new _CustomSnackBarState();
        }
    }

    class _CustomSnackBarState : State<CustomSnackBar> {
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
                                    child: new Text(this.widget.message,
                                        maxLines: 3,
                                        style: CTextStyle.PRegularError.copyWith(color: this.widget.color)
                                    )
                                ),
                                new CustomButton(
                                    padding: EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                                    onPressed: this.widget.dismiss,
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