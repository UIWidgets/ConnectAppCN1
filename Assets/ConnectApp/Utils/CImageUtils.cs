using System;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class CImageUtils {
        public static string SuitableSizeImageUrl(float imageWidth, string imageUrl) {
            var devicePixelRatio = Window.instance.devicePixelRatio;
            if (imageWidth < 100) {
                imageWidth = 100;
            }

            var networkImageWidth = Math.Ceiling(imageWidth * devicePixelRatio / 100) * 100;
            var url = $"{imageUrl}.{networkImageWidth}x0x1.jpg";
            return url;
        }
    }
}