using System;
using System.Collections.Generic;
using System.Linq;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Utils {
    public static class CImageUtils {
        const float ImageWidthMin = 200;
        const float ImageWidthMax = 4000;

        public static string SuitableSizeImageUrl(float imageWidth, string imageUrl) {
            var devicePixelRatio = Window.instance.devicePixelRatio;
            if (imageWidth <= 0) {
                Debug.Assert(imageWidth <= 0, $"Image width error, width: {imageWidth}");
            }

            var networkImageWidth = Math.Ceiling(imageWidth * devicePixelRatio);
            if (networkImageWidth <= ImageWidthMin) {
                networkImageWidth = ImageWidthMin;
            }
            else if (networkImageWidth >= ImageWidthMax) {
                networkImageWidth = ImageWidthMax;
            }

            return imageUrl;
            var url = $"{imageUrl}.{networkImageWidth}x0x1.jpg";
            return url;
        }

        public static string SizeTo200ImageUrl(string imageUrl) {
            return imageUrl;
            return $"{imageUrl}.200x0x1.jpg";
        }

        public static string SplashImageUrl(string imageUrl) {
            var imageWidth = Math.Ceiling(Window.instance.physicalSize.width);
            return imageUrl;
            return $"{imageUrl}.{imageWidth}x0x1.jpg";
        }

        public static Widget GenBadgeImage(List<string> badges, string license, EdgeInsets padding) {
            if (badges != null && badges.isNotEmpty()) {
                if (badges.Any(badge => badge.isNotEmpty() && badge.Equals("official"))) {
                    return new Container(
                        padding: padding,
                        child: Image.asset(
                            "image/official-badge",
                            height: 16,
                            width: 16
                        )
                    );
                }
            }

            if (license.isNotEmpty()) {
                if (license == "UnityPro") {
                    return new Container(
                        padding: padding,
                        child: Image.asset(
                            "image/pro-badge",
                            height: 13,
                            width: 24
                        )
                    );
                }

                if (license == "UnityPersonalPlus") {
                    return new Container(
                        padding: padding,
                        child: Image.asset(
                            "image/plus-badge",
                            height: 13,
                            width: 28
                        )
                    );
                }
            }

            return new Container();
        }
    }
}