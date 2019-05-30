using System;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomProgress : StatefulWidget {
        public CustomProgress(
            float? value = null,
            Color backgroundColor = null,
            Animation<Color> valueColor = null,
            Key key = null
        ) : base(key) {
            D.assert(value == null || (value >= 0.0f && value <= 1.0f));
            this.value = value;
            this.backgroundColor = backgroundColor ?? CColors.Grey;
            this.valueColor = valueColor;
        }

        public readonly float? value;
        public readonly Color backgroundColor;
        public readonly Animation<Color> valueColor;

        public override void debugFillProperties(DiagnosticPropertiesBuilder properties) {
            base.debugFillProperties(properties);
            properties.add(new PercentProperty("value", this.value ?? 0.0f, showName: false,
                ifNull: "<indeterminate>"));
        }

        public override State createState() {
            return new _CustomProgressState();
        }
    }

    class _CustomProgressState : SingleTickerProviderStateMixin<CustomProgress> {
        AnimationController _controller;

        public override void initState() {
            base.initState();
            this._controller = new AnimationController(
                duration: new TimeSpan(0, 0, 0, 0, 1800),
                vsync: this
            );
            if (this.widget.value == null) {
                this._controller.repeat();
            }
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (this.widget.value == null && !this._controller.isAnimating) {
                this._controller.repeat();
            }
            else if (this.widget.value != null && this._controller.isAnimating) {
                this._controller.stop();
            }
        }

        public override void dispose() {
            this._controller.dispose();
            base.dispose();
        }

        Widget _buildIndicator(float animationValue) {
            return new Container(
                constraints: BoxConstraints.tightFor(
                    float.PositiveInfinity,
                    2
                ),
                child: new CustomPaint(
                    painter: new _CustomProgressPainter(this.widget.backgroundColor,
                        this.widget.valueColor?.value ?? CColors.PrimaryBlue, this.widget.value,
                        animationValue
                    )
                )
            );
        }

        public override Widget build(BuildContext context) {
            if (this.widget.value != null) {
                return this._buildIndicator(this._controller.value);
            }

            return new AnimatedBuilder(
                animation: this._controller.view,
                builder: (cxt, child) => this._buildIndicator(this._controller.value)
            );
        }
    }

    class _CustomProgressPainter : AbstractCustomPainter {
        public _CustomProgressPainter(
            Color backgroundColor = null,
            Color valueColor = null,
            float? value = null,
            float? animationValue = null
        ) {
            this.backgroundColor = backgroundColor;
            this.valueColor = valueColor;
            this.value = value;
            this.animationValue = animationValue;
        }

        readonly Color backgroundColor;
        readonly Color valueColor;
        readonly float? value;
        readonly float? animationValue;

        const int _kIndeterminateLinearDuration = 1800;

        static readonly Curve line1Head = new Interval(
            0.0f,
            750.0f / _kIndeterminateLinearDuration,
            new Cubic(0.2f, 0.0f, 0.8f, 1.0f)
        );

        static readonly Curve line1Tail = new Interval(
            333.0f / _kIndeterminateLinearDuration,
            (333.0f + 750.0f) / _kIndeterminateLinearDuration,
            new Cubic(0.4f, 0.0f, 1.0f, 1.0f)
        );

        static readonly Curve line2Head = new Interval(
            1000.0f / _kIndeterminateLinearDuration,
            (1000.0f + 567.0f) / _kIndeterminateLinearDuration,
            new Cubic(0.0f, 0.0f, 0.65f, 1.0f)
        );

        static readonly Curve line2Tail = new Interval(
            1267.0f / _kIndeterminateLinearDuration,
            (1267.0f + 533.0f) / _kIndeterminateLinearDuration,
            new Cubic(0.10f, 0.0f, 0.45f, 1.0f)
        );

        public override void paint(Canvas canvas, Size size) {
            var paint = new Paint {
                color = this.backgroundColor,
                style = PaintingStyle.fill
            };
            canvas.drawRect(Offset.zero & size, paint);
            paint.color = this.valueColor;

            void drawBar(float x, float width) {
                if (width <= 0.0f) {
                    return;
                }

                canvas.drawRect(new Offset(x, 0.0f) & new Size(width, size.height), paint);
            }

            if (this.value != null) {
                drawBar(0.0f, this.value.Value.clamp(0.0f, 1.0f) * size.width);
            }
            else {
                var x1 = size.width * line1Tail.transform(this.animationValue ?? 0.0f);
                var width1 = size.width * line1Head.transform(this.animationValue ?? 0.0f) - x1;

                var x2 = size.width * line2Tail.transform(this.animationValue ?? 0.0f);
                var width2 = size.width * line2Head.transform(this.animationValue ?? 0.0f) - x2;

                drawBar(x1, width1);
                drawBar(x2, width2);
            }
        }

        public override bool shouldRepaint(CustomPainter oldPainter) {
            D.assert(oldPainter is _CustomProgressPainter);
            var painter = oldPainter as _CustomProgressPainter;
            return painter.backgroundColor != this.backgroundColor
                   || painter.valueColor != this.valueColor
                   || painter.value != this.value
                   || painter.animationValue != this.animationValue;
        }
    }
}