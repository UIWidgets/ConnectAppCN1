using System;
using System.Collections.Generic;
using ConnectApp.Components.LikeButton.Painter;
using ConnectApp.Components.LikeButton.Utils;
using ConnectApp.Constants;
using ConnectApp.Utils;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.LikeButton {
    public class LikeButton : StatefulWidget {
        public LikeButton(
            LikeWidgetBuilder likeBuilder = null,
            bool showLikeCount = false,
            int? likeCount = null,
            float size = 30,
            float? bubblesSize = null,
            float? circleSize = null,
            bool isLiked = false,
            bool isShowBubbles = true,
            MainAxisAlignment? mainAxisAlignment = null,
            TimeSpan? animationDuration = null,
            LikeCountAnimationType? likeCountAnimationType = null,
            TimeSpan? likeCountAnimationDuration = null,
            EdgeInsets likeButtonPadding = null,
            EdgeInsets likeCountPadding = null,
            BubblesColor bubblesColor = null,
            CircleColor circleColor = null,
            LikeButtonTapCallback onTap = null,
            Key key = null
        ) : base(key: key) {
            this.likeBuilder = likeBuilder;
            this.showLikeCount = showLikeCount;
            this.likeCount = likeCount;
            this.size = size;
            this.bubblesSize = bubblesSize ?? size * 2.0f;
            this.circleSize = circleSize ?? size * 1.2f;
            this.isLiked = isLiked;
            this.isShowBubbles = isShowBubbles;
            this.mainAxisAlignment = mainAxisAlignment ?? MainAxisAlignment.center;
            this.animationDuration = animationDuration ?? TimeSpan.FromMilliseconds(500);
            this.likeCountAnimationType = likeCountAnimationType ?? LikeCountAnimationType.part;
            this.likeCountAnimationDuration = likeCountAnimationDuration ?? TimeSpan.FromMilliseconds(500);
            this.likeButtonPadding = likeButtonPadding;
            this.likeCountPadding = likeCountPadding ?? EdgeInsets.only(4);
            this.bubblesColor = bubblesColor ?? new BubblesColor(
                                    new Color(0xFFFFC107),
                                    new Color(0xFFFF9800),
                                    new Color(0xFFFF5722),
                                    new Color(0xFFF44336)
                                );
            this.circleColor = circleColor ?? new CircleColor(
                                   new Color(0xFFFF5722),
                                   new Color(0xFFFFC107)
                               );
            this.onTap = onTap;
        }

        public readonly LikeWidgetBuilder likeBuilder;
        public readonly bool showLikeCount;
        public readonly int? likeCount;
        public readonly float size;
        public readonly float bubblesSize;
        public readonly float circleSize;
        public readonly bool isLiked;
        public readonly bool isShowBubbles;
        public readonly TimeSpan animationDuration;
        public readonly BubblesColor bubblesColor;
        public readonly CircleColor circleColor;
        public readonly LikeButtonTapCallback onTap;
        public readonly MainAxisAlignment mainAxisAlignment;
        public readonly TimeSpan likeCountAnimationDuration;
        public readonly LikeCountAnimationType likeCountAnimationType;
        public readonly EdgeInsets likeCountPadding;
        public readonly EdgeInsets likeButtonPadding;

        public override State createState() {
            return new _LikeButtonState();
        }
    }

    public class _LikeButtonState : TickerProviderStateMixin<LikeButton> {
        AnimationController _controller;
        Animation<float> _outerCircleAnimation;
        Animation<float> _innerCircleAnimation;
        Animation<float> _scaleAnimation;
        Animation<float> _bubblesAnimation;
        Animation<Offset> _slidePreValueAnimation;
        Animation<Offset> _slideCurrentValueAnimation;
        AnimationController _likeCountController;
        Animation<float> _opacityAnimation;

        bool _isLiked;
        int? _likeCount;
        int? _preLikeCount;

        public override void initState() {
            base.initState();
            this._isLiked = this.widget.isLiked;
            this._likeCount = this.widget.likeCount ?? 0;
            this._preLikeCount = this._likeCount;

            this._controller =
                new AnimationController(duration: this.widget.animationDuration, vsync: this);
            this._likeCountController = new AnimationController(
                duration: this.widget.likeCountAnimationDuration, vsync: this);

            this._initAnimations();
        }

        public override void dispose() {
            this._controller.dispose();
            this._likeCountController.dispose();
            base.dispose();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is LikeButton likeButton) {
                if (this.widget.isLiked != likeButton.isLiked) {
                    this._handleIsLikeChanged(this.widget.isLiked);
                }
            }
        }

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                behavior: HitTestBehavior.translucent,
                onTap: this._onTap,
                child: new Row(
                    mainAxisAlignment: this.widget.mainAxisAlignment,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        this._buildLikeButtonWidget(),
                        this._buildLikeCountWidget()
                    }
                )
            );
        }

        Widget _buildLikeCountWidget() {
            return !this.widget.showLikeCount
                ? new Container()
                : new Container(
                    margin: EdgeInsets.only(4),
                    child: new Text(
                        CStringUtils.CountToString(count: this._likeCount ?? 0, "点赞"),
                        style: CTextStyle.PRegularBody5.merge(new TextStyle(height: 1))
                    )
                );
        }

        Widget _buildLikeButtonWidget() {
            return new AnimatedBuilder(
                animation: this._controller,
                builder: (c, w) => {
                    float left = 0.0f;
                    float right = 0.0f;
                    float top = 0.0f;
                    float bottom = 0.0f;
                    if (this.widget.likeButtonPadding != null) {
                        left = this.widget.likeButtonPadding.left;
                        right = this.widget.likeButtonPadding.right;
                        top = this.widget.likeButtonPadding.top;
                        bottom = this.widget.likeButtonPadding.bottom;
                    }

                    List<Widget> children = new List<Widget>();
                    if (this.widget.isShowBubbles) {
                        children.Add(
                            new Positioned(
                                top: (this.widget.size + top + bottom - this.widget.bubblesSize) / 2.0f,
                                left: (this.widget.size + left + right - this.widget.bubblesSize) / 2.0f,
                                child: new CustomPaint(
                                    size: new Size(this.widget.bubblesSize, this.widget.bubblesSize),
                                    painter: new BubblesPainter(
                                        currentProgress: this._bubblesAnimation.value,
                                        color1: this.widget.bubblesColor.dotPrimaryColor,
                                        color2: this.widget.bubblesColor.dotSecondaryColor,
                                        color3: this.widget.bubblesColor.dotThirdColorReal,
                                        color4: this.widget.bubblesColor.dotLastColorReal
                                    )
                                )
                            )
                        );
                    }

                    children.Add(
                        new Positioned(
                            top: (this.widget.size + top + bottom - this.widget.circleSize) / 2.0f,
                            left: (this.widget.size + left + right - this.widget.circleSize) / 2.0f,
                            child: new CustomPaint(
                                size: new Size(this.widget.circleSize, this.widget.circleSize),
                                painter: new CirclePainter(
                                    innerCircleRadiusProgress: this._innerCircleAnimation.value,
                                    outerCircleRadiusProgress: this._outerCircleAnimation.value,
                                    circleColor: this.widget.circleColor
                                )
                            )
                        )
                    );
                    var likeWidget = this.widget.likeBuilder?.Invoke(this._isLiked) ??
                                     LikeButtonUtil.defaultWidgetBuilder(this._isLiked, this.widget.size);
                    children.Add(
                        new Container(
                            padding: this.widget.likeButtonPadding,
                            child: Transform.scale(
                                alignment: Alignment.center,
                                scale: this._isLiked && this._controller.isAnimating
                                    ? this._scaleAnimation.value
                                    : 1,
                                child: new SizedBox(
                                    child: likeWidget,
                                    height: this.widget.size,
                                    width: this.widget.size
                                )
                            )
                        )
                    );
                    return new Stack(
                        overflow: Overflow.visible,
                        children: children
                    );
                }
            );
        }

        void _onTap() {
            if (this._controller.isAnimating || this._likeCountController.isAnimating) {
                return;
            }

            if (this.widget.onTap != null) {
                this.widget.onTap();
            }
            else {
                this._handleIsLikeChanged(!this._isLiked);
            }
        }

        void _handleIsLikeChanged(bool isLiked) {
            if (isLiked != this._isLiked) {
                if (this._likeCount != null) {
                    this._preLikeCount = this._likeCount;
                    if (isLiked) {
                        this._likeCount++;
                    }
                    else {
                        this._likeCount--;
                    }
                }

                this._isLiked = isLiked;

                if (this.mounted) {
                    this.setState(() => {
                        if (this._isLiked) {
                            this._controller.reset();
                            this._controller.forward();
                        }

                        if (this.widget.likeCountAnimationType != LikeCountAnimationType.none &&
                            this._likeCount != null) {
                            this._likeCountController.reset();
                            this._likeCountController.forward();
                        }
                    });
                }
            }
        }

        void _initAnimations() {
            this._outerCircleAnimation = new FloatTween(0.1f, 1).animate(
                new CurvedAnimation(
                    parent: this._controller,
                    new Interval(
                        0,
                        0.3f,
                        curve: Curves.linear
                    )
                )
            );

            this._innerCircleAnimation = new FloatTween(0.2f, 1).animate(
                new CurvedAnimation(
                    parent: this._controller,
                    new Interval(
                        0.2f,
                        0.45f,
                        curve: Curves.linear
                    )
                )
            );

            this._scaleAnimation = new FloatTween(0, 1).animate(
                new CurvedAnimation(
                    parent: this._controller,
                    new Interval(
                        0.25f,
                        1.0f,
                        new OvershootCurve()
                    )
                )
            );

            this._bubblesAnimation = new FloatTween(0, 1).animate(
                new CurvedAnimation(
                    parent: this._controller,
                    new Interval(
                        0.1f,
                        1,
                        curve: Curves.decelerate
                    )
                )
            );

            this._slidePreValueAnimation = this._likeCountController.drive(new OffsetTween(
                begin: Offset.zero,
                new Offset(0, 1)
            ));

            this._slideCurrentValueAnimation = this._likeCountController.drive(new OffsetTween(
                new Offset(0, -1),
                end: Offset.zero
            ));

            this._opacityAnimation = this._likeCountController.drive(new FloatTween(0, 1));
        }
    }
}