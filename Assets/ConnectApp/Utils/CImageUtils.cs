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

            var url = $"{imageUrl}.{networkImageWidth}x0x1.jpg";
            return url;
        }

        public static string SizeTo200ImageUrl(string imageUrl) {
            return $"{imageUrl}.200x0x1.jpg";
        }

        public static string SplashImageUrl(string imageUrl) {
            var imageWidth = Math.Ceiling(Window.instance.physicalSize.width);
            return $"{imageUrl}.{imageWidth}x0x1.jpg";
        }

        public static bool isNationalDay = false;

        public static Widget GenBadgeImage(List<string> badges, string license, EdgeInsets padding) {
            Widget w1 = new Container();
            var hasFirst = false;

            if (license.isNotEmpty()) {
                if (license == "UnityPro") {
                    w1 = Image.asset(
                        "image/pro-badge",
                        height: 15,
                        width: 26
                    );
                    hasFirst = true;
                }

                if (license == "UnityPersonalPlus") {
                    w1 = Image.asset(
                        "image/plus-badge",
                        height: 15,
                        width: 30
                    );
                    hasFirst = true;
                }
            }

            if (badges != null && badges.isNotEmpty()) {
                if (badges.Any(badge => badge.isNotEmpty() && badge.Equals("official"))) {
                    w1 = Image.asset(
                        "image/official-badge",
                        height: 18,
                        width: 18
                    );
                    hasFirst = true;
                }
            }

            Widget w2 = new Container();
            if (isNationalDay) {
                w2 = new Container(
                    padding: EdgeInsets.only(hasFirst ? 4 : 0),
                    child: Image.asset(
                        "image/china-flag-badge",
                        height: 14,
                        width: 16
                    )
                );
            }

            return new Container(
                padding: padding,
                child: new Row(
                    children: new List<Widget> {
                        w1,
                        w2
                    }
                )
            );
        }
    }
}