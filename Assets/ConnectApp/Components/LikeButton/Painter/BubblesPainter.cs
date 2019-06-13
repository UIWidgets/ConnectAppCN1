using System.Collections.Generic;
using ConnectApp.Components.LikeButton.Utils;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Canvas = Unity.UIWidgets.ui.Canvas;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.Components.LikeButton.Painter {
    public class BubblesPainter : AbstractCustomPainter {
        readonly float m_OuterBubblesPositionAngle = 51.42f;
        const int paintsNumber = 4;

        float m_CenterX;
        float m_CenterY;

        readonly List<Paint> m_CirclePaints = new List<Paint>();

        float m_MaxOuterDotsRadius;
        float m_MaxInnerDotsRadius;
        float m_MaxDotSize;

        readonly float m_CurrentProgress;

        float m_CurrentRadius1;
        float m_CurrentDotSize1;
        float m_CurrentDotSize2;
        float m_CurrentRadius2;

        bool m_IsFirst = true;

        public BubblesPainter(
            float currentProgress,
            int bubblesCount = 7,
            Color color1 = null,
            Color color2 = null,
            Color color3 = null,
            Color color4 = null
        ) {
            this.m_OuterBubblesPositionAngle =
                bubblesCount > 0 ? 360f / bubblesCount : this.m_OuterBubblesPositionAngle;
            for (int i = 0; i < paintsNumber; i++) {
                this.m_CirclePaints.Add(new Paint {style = PaintingStyle.fill});
            }

            this.m_CurrentProgress = currentProgress;
            this.m_BubblesCount = bubblesCount;
            this.m_Color1 = color1 ?? new Color(0xFFFFC107);
            this.m_Color2 = color2 ?? new Color(0xFFFF9800);
            this.m_Color3 = color3 ?? new Color(0xFFFF5722);
            this.m_Color4 = color4 ?? new Color(0xFFF44336);
        }

        readonly int m_BubblesCount;
        readonly Color m_Color1;
        readonly Color m_Color2;
        readonly Color m_Color3;
        readonly Color m_Color4;

        public override void paint(Canvas canvas, Size size) {
            if (this.m_IsFirst) {
                this.m_CenterX = size.width * 0.5f;
                this.m_CenterY = size.height * 0.5f;
                this.m_MaxDotSize = size.width * 0.05f;
                this.m_MaxOuterDotsRadius = size.width * 0.5f - this.m_MaxDotSize * 2f;
                this.m_MaxInnerDotsRadius = 0.8f * this.m_MaxOuterDotsRadius;
                this.m_IsFirst = false;
            }

            this._updateOuterBubblesPosition();
            this._updateInnerBubblesPosition();
            this._updateBubblesPaints();
            this._drawOuterBubblesFrame(canvas);
            this._drawInnerBubblesFrame(canvas);
        }

        void _drawOuterBubblesFrame(Canvas canvas) {
            if (this.m_CirclePaints.Count == 0) {
                return;
            }

            for (int i = 0; i < this.m_BubblesCount; i++) {
                float cX = this.m_CenterX + this.m_CurrentRadius1 *
                           Mathf.Cos(i * LikeButtonUtil.degToRad(deg: this.m_OuterBubblesPositionAngle));
                float cY = this.m_CenterY + this.m_CurrentRadius1 *
                           Mathf.Sin(i * LikeButtonUtil.degToRad(deg: this.m_OuterBubblesPositionAngle));
                canvas.drawCircle(new Offset(dx: cX, dy: cY), radius: this.m_CurrentDotSize1,
                    this.m_CirclePaints[i % this.m_CirclePaints.Count]);
            }
        }

        void _drawInnerBubblesFrame(Canvas canvas) {
            if (this.m_CirclePaints.Count == 0) {
                return;
            }

            for (int i = 0; i < this.m_BubblesCount; i++) {
                float cX = this.m_CenterX + this.m_CurrentRadius2 *
                           Mathf.Cos(i * LikeButtonUtil.degToRad(this.m_OuterBubblesPositionAngle - 10));
                float cY = this.m_CenterY + this.m_CurrentRadius2 *
                           Mathf.Sin(i * LikeButtonUtil.degToRad(this.m_OuterBubblesPositionAngle - 10));
                canvas.drawCircle(new Offset(dx: cX, dy: cY), radius: this.m_CurrentDotSize2,
                    this.m_CirclePaints[(i + 1) % this.m_CirclePaints.Count]);
            }
        }

        void _updateOuterBubblesPosition() {
            if (this.m_CurrentProgress < 0.3f) {
                this.m_CurrentRadius1 = LikeButtonUtil.mapValueFromRangeToRange(this.m_CurrentProgress, 0, 0.3f, 0,
                    this.m_MaxOuterDotsRadius * 0.8f);
            }
            else {
                this.m_CurrentRadius1 = LikeButtonUtil.mapValueFromRangeToRange(this.m_CurrentProgress, 0.3f, 1,
                    0.8f * this.m_MaxOuterDotsRadius, this.m_MaxOuterDotsRadius);
            }

            if (this.m_CurrentProgress == 0) {
                this.m_CurrentDotSize1 = 0;
            }
            else if (this.m_CurrentProgress < 0.7f) {
                this.m_CurrentDotSize1 = this.m_MaxDotSize;
            }
            else {
                this.m_CurrentDotSize1 =
                    LikeButtonUtil.mapValueFromRangeToRange(this.m_CurrentProgress, 0.7f, 1, this.m_MaxDotSize, 0);
            }
        }

        void _updateInnerBubblesPosition() {
            this.m_CurrentRadius2 = this.m_CurrentProgress < 0.3f
                ? LikeButtonUtil.mapValueFromRangeToRange(this.m_CurrentProgress, 0, 0.3f, 0, this.m_MaxInnerDotsRadius)
                : this.m_MaxInnerDotsRadius;
            if (this.m_CurrentProgress == 0) {
                this.m_CurrentDotSize2 = 0;
            }
            else if (this.m_CurrentProgress < 0.2f) {
                this.m_CurrentDotSize2 = this.m_MaxDotSize;
            }
            else if (this.m_CurrentProgress < 0.5f) {
                this.m_CurrentDotSize2 = LikeButtonUtil.mapValueFromRangeToRange(this.m_CurrentProgress, 0.2f, 0.5f,
                    this.m_MaxDotSize, 0.3f * this.m_MaxDotSize);
            }
            else {
                this.m_CurrentDotSize2 =
                    LikeButtonUtil.mapValueFromRangeToRange(this.m_CurrentProgress, 0.5f, 1, this.m_MaxDotSize * 0.3f,
                        0);
            }
        }

        void _updateBubblesPaints() {
            float progress1 = LikeButtonUtil.clamp(this.m_CurrentProgress, 0.6f, 1);
            int alpha =
                (int) LikeButtonUtil.mapValueFromRangeToRange(progress1, 0.6f, 1, 255, 0);
            if (this.m_CurrentProgress < 0.5) {
                float progress2 =
                    LikeButtonUtil.mapValueFromRangeToRange(this.m_CurrentProgress, 0, 0.5f, 0, 1);
                this.m_CirclePaints[0].color = Color.lerp(this.m_Color1, this.m_Color2, progress2).withAlpha(alpha);
                this.m_CirclePaints[1].color = Color.lerp(this.m_Color2, this.m_Color3, progress2).withAlpha(alpha);
                this.m_CirclePaints[2].color = Color.lerp(this.m_Color3, this.m_Color4, progress2).withAlpha(alpha);
                this.m_CirclePaints[3].color = Color.lerp(this.m_Color4, this.m_Color1, progress2).withAlpha(alpha);
            }
            else {
                float progress3 =
                    LikeButtonUtil.mapValueFromRangeToRange(this.m_CurrentProgress, 0.5f, 1, 0, 1);
                this.m_CirclePaints[0].color = Color.lerp(this.m_Color2, this.m_Color3, progress3).withAlpha(alpha);
                this.m_CirclePaints[1].color = Color.lerp(this.m_Color3, this.m_Color4, progress3).withAlpha(alpha);
                this.m_CirclePaints[2].color = Color.lerp(this.m_Color4, this.m_Color1, progress3).withAlpha(alpha);
                this.m_CirclePaints[3].color = Color.lerp(this.m_Color1, this.m_Color2, progress3).withAlpha(alpha);
            }
        }

        public override bool shouldRepaint(CustomPainter oldDelegate) {
            return true;
        }
    }
}