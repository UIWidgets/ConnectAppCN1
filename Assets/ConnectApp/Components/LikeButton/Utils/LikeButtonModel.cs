using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;

namespace ConnectApp.Components.LikeButton.Utils {
    public class BubblesColor {
        public BubblesColor(
            Color dotPrimaryColor,
            Color dotSecondaryColor,
            Color dotThirdColor,
            Color dotLastColor
        ) {
            this.dotPrimaryColor = dotPrimaryColor;
            this.dotSecondaryColor = dotSecondaryColor;
            this.dotThirdColor = dotThirdColor;
            this.dotLastColor = dotLastColor;
        }

        public readonly Color dotPrimaryColor;
        public readonly Color dotSecondaryColor;
        readonly Color dotThirdColor;
        readonly Color dotLastColor;

        public Color dotThirdColorReal {
            get { return this.dotThirdColor == null ? this.dotPrimaryColor : this.dotThirdColor; }
        }

        public Color dotLastColorReal {
            get { return this.dotLastColor == null ? this.dotSecondaryColor : this.dotLastColor; }
        }
    }

    public class CircleColor {
        public CircleColor(
            Color start,
            Color end
        ) {
            this.start = start;
            this.end = end;
        }

        public readonly Color start;
        public readonly Color end;
    }

    public class OvershootCurve : Curve {
        public OvershootCurve(
            float period = 2.5f
        ) {
            this.period = period;
        }

        readonly float period;

        protected override float transformInternal(float t) {
            D.assert(t >= 0 && t <= 1);
            t -= 1;
            return t * t * ((this.period + 1) * t + this.period) + 1;
        }

        public override string ToString() {
            return $"{this.GetType()}({this.period})";
        }
    }

    public class LikeCountClip : CustomClipper<Rect> {
        public override Rect getClip(Size size) {
            return Offset.zero & size;
        }

        public override bool shouldReclip(CustomClipper<Rect> oldClipper) {
            return true;
        }
    }
}