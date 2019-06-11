using ConnectApp.Components.LikeButton.Utils;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.LikeButton.Painter {
    class CirclePainter : AbstractCustomPainter {
        readonly Paint m_CirclePaint = new Paint();
        readonly Paint m_MaskPaint = new Paint();

        public CirclePainter(
            float outerCircleRadiusProgress,
            float innerCircleRadiusProgress,
            CircleColor circleColor = null
        ) {
            this.m_OuterCircleRadiusProgress = outerCircleRadiusProgress;
            this.m_InnerCircleRadiusProgress = innerCircleRadiusProgress;
            this.m_CircleColor = circleColor ?? new CircleColor(new Color(0xFFFF5722), new Color(0xFFFFC107));
            this.m_CirclePaint.style = PaintingStyle.fill;
            this.m_MaskPaint.blendMode = BlendMode.clear;
        }

        readonly float m_OuterCircleRadiusProgress;
        readonly float m_InnerCircleRadiusProgress;
        readonly CircleColor m_CircleColor;

        public override void paint(Canvas canvas, Size size) {
            float center = size.width * 0.5f;
            this._updateCircleColor();
            canvas.saveLayer(Offset.zero & size, new Paint());
            canvas.drawCircle(new Offset(center, center), this.m_OuterCircleRadiusProgress * center,
                this.m_CirclePaint);
            canvas.drawCircle(new Offset(center, center), this.m_InnerCircleRadiusProgress * center + 1,
                this.m_MaskPaint);
            canvas.restore();
        }

        void _updateCircleColor() {
            float colorProgress = LikeButtonUtil.clamp(this.m_OuterCircleRadiusProgress, 0.5f, 1);
            colorProgress = LikeButtonUtil.mapValueFromRangeToRange(colorProgress, 0.5f, 1, 0, 1);
            this.m_CirclePaint.color = Color.lerp(this.m_CircleColor.start, this.m_CircleColor.end, colorProgress);
        }

        public override bool shouldRepaint(CustomPainter oldDelegate) {
            return true;
        }
    }
}