using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public delegate bool BoolCallback();
    public delegate void LikeCallback(int likeCount);

    public class CrazyLikeButton : StatefulWidget {
        public CrazyLikeButton(
            Widget child,
            bool like,
            int likeCount,
            int totalLikeCount,
            bool isPullUp,
            BoolCallback boolCallback,
            LikeCallback likeCallback = null,
            Key key = null
        ) : base(key: key) {
            D.assert(child != null);
            this.child = child;
            this.like = like;
            this.likeCount = likeCount;
            this.totalLikeCount = totalLikeCount;
            this.isPullUp = isPullUp;
            this.boolCallback = boolCallback;
            this.likeCallback = likeCallback;
        }

        public readonly Widget child;
        public readonly bool like;
        public readonly int likeCount;
        public readonly int totalLikeCount;
        public readonly bool isPullUp;
        public readonly BoolCallback boolCallback;
        public readonly LikeCallback likeCallback;

        public override State createState() {
            return new _CrazyLikeButtonState();
        }
    }

    class _CrazyLikeButtonState: State<CrazyLikeButton>, TickerProvider {
        bool _like;
        bool _isLike;
        int _likeCount;
        DateTime? _lastDateTime;
        Timer _timer;
        AnimationController _controller;
        Animation<float> _scaleAnimation;
        const float likeTime = 500;

        public override void initState() {
            base.initState();
            this._like = false;
            this._isLike = false;
            this._likeCount = 0;
            this._lastDateTime = null;
            this._controller = new AnimationController(
                duration: TimeSpan.FromMilliseconds(50),
                vsync: this
            );
            this._scaleAnimation = new FloatTween(1.0f, 1.1f).animate(parent: this._controller);
            this._controller.addStatusListener(listener: this._animationStatusListener);
        }

        public override void dispose() {
            this._timer?.Dispose();
            this._controller.removeStatusListener(listener: this._animationStatusListener);
            this._controller.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        void _animationStatusListener(AnimationStatus status) {
            if (status == AnimationStatus.completed) {
                this._controller.reverse();
            }
        }

        void _startTimer() {
            this._timer?.cancel();
            this._timer = Window.instance.run(TimeSpan.FromMilliseconds(value: likeTime), () => {
                this.widget.likeCallback?.Invoke(likeCount: this._likeCount);
                this.setState(() => {
                    this._isLike = false;
                    this._likeCount = 0;
                    this._lastDateTime = null;
                });
            });
        }

        public override Widget build(BuildContext context) {
            Widget likeCountWidget;
            if (this.widget.totalLikeCount + this._likeCount > 0) {
                var totalLikeCount = CStringUtils.CountToString(this.widget.totalLikeCount + this._likeCount);
                likeCountWidget = new Text(
                    data: totalLikeCount,
                    style: new TextStyle(
                        fontSize: 12,
                        fontFamily: "Roboto-Regular",
                        color: CColors.Thumb
                    )
                );
            }
            else {
                likeCountWidget = new Container();
            }

            return new Stack(
                fit: StackFit.expand,
                children: new List<Widget> {
                    this.widget.child,
                    new AnimatedPositioned(
                        right: this.widget.isPullUp ? -24 : 16,
                        bottom: 24,
                        duration: TimeSpan.FromMilliseconds(200),
                        child: new GestureDetector(
                            onTap: () => {
                                if (this.widget.isPullUp) {
                                    return;
                                }

                                var isLoginIn = this.widget.boolCallback();
                                if (!isLoginIn) {
                                    return;
                                }
                                this._controller.reset();
                                this._controller.forward();
                                if (!this._like) {
                                    this.setState(() => this._like = true);
                                }
                                if (this._likeCount + this.widget.likeCount < 50) {
                                    this.setState(() => this._likeCount += 1);
                                }
                                if (this._lastDateTime == null) {
                                    this._lastDateTime = DateTime.Now;
                                    this.setState(() => this._isLike = true);
                                    this._startTimer();
                                }
                                else {
                                    var currentDateTime = DateTime.Now;
                                    var ts = currentDateTime.Subtract((DateTime) this._lastDateTime);
                                    if (ts.TotalMilliseconds <= likeTime) {
                                        this._startTimer();
                                    }
                                    this._lastDateTime = currentDateTime;
                                }
                            },
                            child: new Opacity(
                                this.widget.isPullUp ? 0.5f : 1,
                                child: new ScaleTransition(
                                    scale: this._scaleAnimation,
                                    child: new Container(
                                        width: 48,
                                        height: 48,
                                        decoration: new BoxDecoration(
                                            color: CColors.White,
                                            borderRadius: BorderRadius.all(24),
                                            boxShadow: new List<BoxShadow> {
                                                new BoxShadow(
                                                    CColors.Black.withOpacity(0.1f),
                                                    blurRadius: 8,
                                                    spreadRadius: 0,
                                                    offset: new Offset(0, 0))
                                            }
                                        ),
                                        child: new Column(
                                            mainAxisAlignment: MainAxisAlignment.center,
                                            children: new List<Widget> {
                                                new Icon(
                                                    this.widget.like || this._like ? Icons.thumb_bold : Icons.thumb_line, 
                                                    size: 24, 
                                                    color: CColors.Thumb
                                                ),
                                                likeCountWidget
                                            }
                                        )
                                    )
                                )
                            )
                        )
                    ),
                    new AnimatedPositioned(
                        child: new Opacity(
                            this._isLike ? 1 : 0,
                            child: new ScaleTransition(
                                scale: this._scaleAnimation,
                                alignment: Alignment.center,
                                child: new Container(
                                    width: 32,
                                    height: 32,
                                    decoration: new BoxDecoration(
                                        color: CColors.Coral,
                                        borderRadius: BorderRadius.all(16)
                                    ),
                                    alignment: Alignment.center,
                                    child: new Text(
                                        $"+{this._likeCount + this.widget.likeCount}",
                                        style: new TextStyle(
                                            fontSize: 12,
                                            fontFamily: "Roboto-Regular",
                                            color: CColors.White
                                        )
                                    )
                                )
                            )
                        ),
                        duration: TimeSpan.FromMilliseconds(134),
                        bottom: this._isLike ? 96 : 70,
                        right: 24,
                        curve: Curves.bounceOut
                    )
                }
            );
        }
    }
}