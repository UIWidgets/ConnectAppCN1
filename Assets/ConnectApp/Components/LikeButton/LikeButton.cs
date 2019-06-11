using System;
using System.Collections.Generic;
using ConnectApp.Components.LikeButton.Painter;
using ConnectApp.Components.LikeButton.Utils;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.LikeButton {
    public class LikeButton : StatefulWidget {
        public LikeButton(
            LikeWidgetBuilder likeBuilder,
            LikeCountWidgetBuilder countBuilder,
            int likeCount,
            float size = 30,
            float? bubblesSize = null,
            float? circleSize = null,
            bool? isLiked = null,
            MainAxisAlignment? mainAxisAlignment = null,
            TimeSpan? animationDuration = null,
            LikeCountAnimationType? likeCountAnimationType = null,
            TimeSpan? likeCountAnimationDuration = null,
            EdgeInsets likeCountPadding = null,
            BubblesColor bubblesColor = null,
            CircleColor circleColor = null,
            LikeButtonTapCallback onTap = null,
            Key key = null
        ) : base(key) {
            D.assert(likeBuilder != null);
            D.assert(countBuilder != null);
            this.likeBuilder = likeBuilder;
            this.countBuilder = countBuilder;
            this.likeCount = likeCount;
            this.size = size;
            this.bubblesSize = bubblesSize ?? size * 4;
            this.circleSize = circleSize ?? size * 0.8f;
            this.isLiked = isLiked ?? false;
            this.mainAxisAlignment = mainAxisAlignment ?? MainAxisAlignment.center;
            this.animationDuration = animationDuration ?? new TimeSpan(0, 0, 0, 1);
            this.likeCountAnimationType = likeCountAnimationType ?? LikeCountAnimationType.part;
            this.likeCountAnimationDuration = likeCountAnimationDuration ?? new TimeSpan(0, 0, 0, 0, 500);
            this.likeCountPadding = likeCountPadding ?? EdgeInsets.only(3);
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

        // size of like widget
        public readonly float size;

        // animation duration to change isLiked state
        public readonly TimeSpan animationDuration;

        // total size of bubbles
        public readonly float bubblesSize;

        // colors of bubbles
        public readonly BubblesColor bubblesColor;

        // size of circle
        public readonly float circleSize;

        // colors of circle
        public readonly CircleColor circleColor;

        // tap call back of like button
        public readonly LikeButtonTapCallback onTap;

        // whether it is liked
        public readonly bool isLiked;

        // like count
        // if null, will not show
        public readonly int likeCount;

        // mainAxisAlignment for like button
        public readonly MainAxisAlignment mainAxisAlignment;

        // builder to create like widget
        public readonly LikeWidgetBuilder likeBuilder;

        // builder to create like count widget
        public readonly LikeCountWidgetBuilder countBuilder;

        // animation duration to change like count
        public readonly TimeSpan likeCountAnimationDuration;

        // animation type to change like count(none,part,all)
        public readonly LikeCountAnimationType likeCountAnimationType;

        // padding for like count widget
        public readonly EdgeInsets likeCountPadding;

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
        int _likeCount;
        int _preLikeCount;

        public override void initState() {
            base.initState();
            this._isLiked = this.widget.isLiked;
            this._likeCount = this.widget.likeCount;
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

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                behavior: HitTestBehavior.translucent,
                onTap: this._onTap,
                child: new Row(
                    mainAxisAlignment: this.widget.mainAxisAlignment,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new AnimatedBuilder(
                            animation: this._controller,
                            builder: (c, w) => {
                                var likeWidget = this.widget.likeBuilder?.Invoke(this._isLiked) ??
                                                 LikeButtonUtil.defaultWidgetBuilder(this._isLiked, this.widget.size);
                                return new Stack(
                                    overflow: Overflow.visible,
                                    children: new List<Widget> {
                                        new Align(
                                            alignment: Alignment.center,
                                            child: new CustomPaint(
                                                size: new Size(this.widget.size, this.widget.bubblesSize),
                                                painter: new BubblesPainter(
                                                    currentProgress: this._bubblesAnimation.value,
                                                    color1: this.widget.bubblesColor.dotPrimaryColor,
                                                    color2: this.widget.bubblesColor.dotSecondaryColor,
                                                    color3: this.widget.bubblesColor.dotThirdColorReal,
                                                    color4: this.widget.bubblesColor.dotLastColorReal
                                                )
                                            )
                                        ),
                                        new Align(
                                            alignment: Alignment.center,
                                            child: new CustomPaint(
                                                size: new Size(this.widget.circleSize, this.widget.circleSize),
                                                painter: new CirclePainter(
                                                    innerCircleRadiusProgress: this._innerCircleAnimation.value,
                                                    outerCircleRadiusProgress: this._outerCircleAnimation.value,
                                                    circleColor: this.widget.circleColor
                                                )
                                            )
                                        ),
                                        new Align(
                                            alignment: Alignment.center,
                                            child: new Container(
                                                width: this.widget.size,
                                                height: this.widget.size,
                                                alignment: Alignment.center,
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
                                        )
                                    }
                                );
                            }
                        ),
                        this._getLikeCountWidget()
                    }
                )
            );
        }

        Widget _getLikeCountWidget() {
            var likeCount = this._likeCount.ToString();
            var preLikeCount = this._preLikeCount.ToString();

            int didIndex = 0;
            if (preLikeCount.Length == likeCount.Length) {
                for (; didIndex < likeCount.Length; didIndex++) {
                    if (likeCount[didIndex] != preLikeCount[didIndex]) {
                        break;
                    }
                }
            }

            bool allChange = preLikeCount.Length != likeCount.Length || didIndex == 0;

            Widget result;

            if (this.widget.likeCountAnimationType == LikeCountAnimationType.none ||
                this._likeCount == this._preLikeCount) {
                result = this._createLikeCountWidget(this._likeCount, this._isLiked, this._likeCount.ToString());
            }
            else if (this.widget.likeCountAnimationType == LikeCountAnimationType.part &&
                     !allChange) {
                var samePart = likeCount.Substring(0, didIndex);
                var preText = preLikeCount.Substring(didIndex, preLikeCount.Length);
                var text = likeCount.Substring(didIndex, likeCount.Length);
                var preSameWidget = this._createLikeCountWidget(this._preLikeCount, !this._isLiked, samePart);
                var currentSameWidget = this._createLikeCountWidget(this._likeCount, this._isLiked, samePart);
                var preWidget = this._createLikeCountWidget(this._preLikeCount, !this._isLiked, preText);
                var currentWidget = this._createLikeCountWidget(this._likeCount, this._isLiked, text);

                result = new AnimatedBuilder(
                    animation: this._likeCountController,
                    builder: (b, w) => new Row(
                        children: new List<Widget> {
                            new Stack(
                                fit: StackFit.passthrough,
                                overflow: Overflow.clip,
                                children: new List<Widget> {
                                    new Opacity(
                                        child: currentSameWidget,
                                        opacity: this._opacityAnimation.value
                                    ),
                                    new Opacity(
                                        child: preSameWidget,
                                        opacity: 1 - this._opacityAnimation.value
                                    )
                                }
                            ),
                            new Stack(
                                fit: StackFit.passthrough,
                                overflow: Overflow.clip,
                                children: new List<Widget> {
                                    new FractionalTranslation(
                                        child: currentWidget,
                                        translation: this._preLikeCount > this._likeCount
                                            ? this._slideCurrentValueAnimation.value
                                            : -this._slideCurrentValueAnimation.value
                                    ),
                                    new FractionalTranslation(
                                        child: preWidget,
                                        translation: this._preLikeCount > this._likeCount
                                            ? this._slidePreValueAnimation.value
                                            : -this._slidePreValueAnimation.value
                                    )
                                }
                            )
                        }
                    ));
            }
            else {
                result = new AnimatedBuilder(
                    animation: this._likeCountController,
                    builder: (b, w) => new Stack(
                        fit: StackFit.passthrough,
                        overflow: Overflow.clip,
                        children: new List<Widget> {
                            new FractionalTranslation(
                                child: this._createLikeCountWidget(this._likeCount, this._isLiked,
                                    this._likeCount.ToString()),
                                translation: this._preLikeCount > this._likeCount
                                    ? this._slideCurrentValueAnimation.value
                                    : -this._slideCurrentValueAnimation.value
                            ),
                            new FractionalTranslation(
                                child: this._createLikeCountWidget(this._likeCount, !this._isLiked,
                                    this._preLikeCount.ToString()),
                                translation: this._preLikeCount > this._likeCount
                                    ? this._slidePreValueAnimation.value
                                    : -this._slidePreValueAnimation.value
                            )
                        }
                    )
                );
            }

            result = new ClipRect(
                child: result,
                clipper: new LikeCountClip()
            );

            if (this.widget.likeCountPadding != null) {
                result = new Padding(
                    padding: this.widget.likeCountPadding,
                    child: result
                );
            }

            return result;
        }

        Widget _createLikeCountWidget(int likeCount, bool isLiked, string text) {
            return this.widget.countBuilder?.Invoke(likeCount, isLiked, text) ??
                   new Text(text, style: new TextStyle(color: CColors.Grey));
        }

        void _onTap() {
            if (this._controller.isAnimating || this._likeCountController.isAnimating) {
                return;
            }

            if (this.widget.onTap != null) {
                this.widget.onTap(this._isLiked).Then(this._handleIsLikeChanged);
            }
            else {
                this._handleIsLikeChanged(!this._isLiked);
            }
        }

        void _handleIsLikeChanged(bool isLiked) {
            if (isLiked != null && isLiked != this._isLiked) {
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

                        if (this.widget.likeCountAnimationType != LikeCountAnimationType.none) {
                            this._likeCountController.reset();
                            this._likeCountController.forward();
                        }
                    });
                }
            }
        }

        void _initAnimations() {
            this._outerCircleAnimation = new FloatTween(0.1f, 1)
                .animate(
                    new CurvedAnimation(
                        parent: this._controller,
                        new Interval(
                            0,
                            0.3f,
                            curve: Curves.ease
                        )
                    )
                );

            this._innerCircleAnimation = new FloatTween(0.2f, 1)
                .animate(
                    new CurvedAnimation(
                        parent: this._controller,
                        new Interval(
                            0.2f,
                            0.5f,
                            curve: Curves.ease
                        )
                    )
                );

            this._scaleAnimation = new FloatTween(0.2f, 1)
                .animate(
                    new CurvedAnimation(
                        parent: this._controller,
                        new Interval(
                            0.35f,
                            0.7f,
                            new OvershootCurve()
                        )
                    )
                );

            this._bubblesAnimation = new FloatTween(0, 1)
                .animate(
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

            this._opacityAnimation = this._likeCountController.drive(new FloatTween(
                0,
                1
            ));
        }
    }
}