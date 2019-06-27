using System;
using Unity.UIWidgets.ui;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class CImageUtils {
        
        const float ImageWidthMin = 200;
        const float ImageWidthMax = 4000;

        public static string SuitableSizeImageUrl(float imageWidth, string imageUrl) {
            var devicePixelRatio = Window.instance.devicePixelRatio;
            if (imageWidth <= 0) {
                Debug.Assert( imageWidth <= 0, $"Image width error, width: {imageWidth}");
            }
            var networkImageWidth = Math.Ceiling(imageWidth * devicePixelRatio);
            if (networkImageWidth <= ImageWidthMin) {
                networkImageWidth = ImageWidthMin;
            }
            else if (networkImageWidth >= ImageWidthMax) {
                networkImageWidth = ImageWidthMax;
            }
            var url = $"{imageUrl}.{networkImageWidth}x0x1.jpg";
            return url;
        }
    }
}