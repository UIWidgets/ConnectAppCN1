using System;
using ConnectApp.Constants;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Components.LikeButton.Utils {
    public static class LikeButtonUtil {
        public static float degToRad(float deg) {
            return deg * (Mathf.PI / 180.0f);
        }

        public static float radToDeg(float rad) {
            return rad * (180.0f / Mathf.PI);
        }

        public static float mapValueFromRangeToRange(float value, float fromLow, float fromHigh,
            float toLow, float toHigh) {
            return toLow + (value - fromLow) / (fromHigh - fromLow) * (toHigh - toLow);
        }

        public static float clamp(float value, float low, float high) {
            return Math.Min(Math.Max(value, low), high);
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