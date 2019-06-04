using System;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomScrollbar : StatefulWidget {
        public CustomScrollbar(
            Widget child,
            Key key = null
        ) : base(key: key) {
            this.child = child;
        }

        public readonly Widget child;

        public override State createState() {
            return new _CustomScrollbarState();
        }
    }

    public class _CustomScrollbarState : TickerProviderStateMixin<CustomScrollbar> {
        
        static readonly Color _kScrollbarColor = new Color(0x99777777);
        static readonly float _kScrollbarMinLength = 36;
        static readonly float _kScrollbarMinOverscrollLength = 8;
        static readonly Radius _kScrollbarRadius = Radius.circular(1.25f);
        static readonly TimeSpan _kScrollbarTimeToFade = TimeSpan.FromMilliseconds(50);
        static readonly TimeSpan _kScrollbarFadeDuration = TimeSpan.FromMilliseconds(250);
        
        static readonly float _kScrollbarThickness = 2.5f;
        static readonly float _kScrollbarMainAxisMargin = 3;
        static readonly float _kScrollbarCrossAxisMargin = 3;
        
        ScrollbarPainter _scrollbarPainter;
        TextDirection _textDirection;
        AnimationController _fadeoutAnimationController;
        Animation<float> _fadeoutOpacityAnimation;
        Timer _fadeoutTimer;

        public override void initState() {
            base.initState();
            this._fadeoutAnimationController = new AnimationController(
                vsync: this,
                duration: _kScrollbarFadeDuration
            );
            this._fadeoutOpacityAnimation = new CurvedAnimation(
                parent: this._fadeoutAnimationController,
                curve: Curves.fastOutSlowIn
            );
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            this._textDirection = Directionality.of(context: this.context);
            this._scrollbarPainter = this._buildScrollbarPainter();
        }

        public override void dispose() {
            this._fadeoutAnimationController.dispose();
            this._fadeoutTimer?.cancel();
            this._scrollbarPainter?.dispose();
            base.dispose();
        }

        ScrollbarPainter _buildScrollbarPainter() {
            return new ScrollbarPainter(
                _kScrollbarColor,
                textDirection: this._textDirection,
                _kScrollbarThickness,
                fadeoutOpacityAnimation: this._fadeoutOpacityAnimation,
                _kScrollbarMainAxisMargin,
                _kScrollbarCrossAxisMargin,
                _kScrollbarRadius,
                _kScrollbarMinLength,
                _kScrollbarMinOverscrollLength
            );
        }

        bool _handleScrollNotification(ScrollNotification notification) {
            if (notification is ScrollUpdateNotification || notification is OverscrollNotification) {
                if (this._fadeoutAnimationController.status != AnimationStatus.forward) {
                    this._fadeoutAnimationController.forward();
                }

                this._fadeoutTimer?.cancel();
                this._scrollbarPainter.update(
                    metrics: notification.metrics,
                    axisDirection: notification.metrics.axisDirection
                );
            }
            else if (notification is ScrollEndNotification) {
                this._fadeoutTimer?.cancel();
                this._fadeoutTimer = Window.instance.run(_kScrollbarTimeToFade, () => {
                    this._fadeoutAnimationController.reverse();
                    this._fadeoutTimer = null;
                });
            }

            return false;
        }

        public override Widget build(BuildContext context) {
            return new NotificationListener<ScrollNotification>(
                onNotification: this._handleScrollNotification,
                child: new RepaintBoundary(
                    child: new CustomPaint(
                        foregroundPainter: this._scrollbarPainter,
                        child: new RepaintBoundary(
                            child: this.widget.child
                        )
                    )
                )
            );
        }
    }
}