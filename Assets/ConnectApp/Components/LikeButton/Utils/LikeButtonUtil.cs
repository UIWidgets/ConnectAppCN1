using System;
using ConnectApp.Constants;
using Unity.UIWidgets.widgets;
using Icons = Unity.UIWidgets.material.Icons;

namespace ConnectApp.Components.LikeButton.Utils {
    public class LikeButtonUtil {
        public static float degToRad(float deg) {
            return (float) (deg * (Math.PI / 180.0));
        }

        float radToDeg(float rad) {
            return (float) (rad * (180.0 / Math.PI));
        }

        public static float mapValueFromRangeToRange(float value, float fromLow, float fromHigh,
            float toLow, float toHigh) {
            return toLow + (value - fromLow) / (fromHigh - fromLow) * (toHigh - toLow);
        }

        public static float clamp(double value, double low, double high) {
            return (float) Math.Min(Math.Max(value, low), high);
        }

        public static Widget defaultWidgetBuilder(bool isLiked, float size) {
            return new Icon(
                icon: Icons.favorite,
                color: isLiked ? CColors.Red : CColors.Grey,
                size: size
            );
        }
    }
}