using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Utils {
    public static class CImageUtils {
        const float ImageWidthMin = 200;
        const float ImageWidthMax = 4000;

        public static string SuitableSizeImageUrl(float imageWidth, string imageUrl) {
            var devicePixelRatio = Window.instance.devicePixelRatio;
            if (imageWidth <= 0) {
                DebugerUtils.DebugAssert(imageWidth <= 0, $"Image width error, width: {imageWidth}");
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

        public static string SizeToScreenImageUrl(string imageUrl) {
            var data = MediaQuery.of(GlobalContext.context);
            return $"{imageUrl}.{(int) (data.size.width * data.devicePixelRatio)}x0x1.jpg";
        }

        public static string SplashImageUrl(string imageUrl) {
            var imageWidth = Math.Ceiling(Window.instance.physicalSize.width);
            return $"{imageUrl}.{imageWidth}x0x1.jpg";
        }

        public static bool isNationalDay = false;

        public static Widget GenBadgeImage(List<string> badges, string license, EdgeInsets padding,
            bool showFlag = true) {
            var badgeList = new List<Widget>();
            Widget badgeWidget = null;

            if (license.isNotEmpty()) {
                if (license == "UnityPro") {
                    badgeWidget = Image.asset(
                        "image/pro-badge",
                        height: 12,
                        width: 24
                    );
                }
                else if (license == "UnityPersonalPlus") {
                    badgeWidget = Image.asset(
                        "image/plus-badge",
                        height: 13,
                        width: 28
                    );
                }
                else if (license == "UnityLearnPremium") {
                    badgeWidget = Image.asset(
                        "image/prem-badge",
                        height: 13,
                        width: 33
                    );
                }
            }

            if (badges != null && badges.isNotEmpty()) {
                if (badges.Any(badge => badge.isNotEmpty() && badge.Equals("official"))) {
                    badgeWidget = Image.asset(
                        "image/official-badge",
                        height: 18,
                        width: 18
                    );
                }
            }

            if (badgeWidget != null) {
                if (badgeList.Count >= 1) {
                    badgeList.Add(new SizedBox(width: 4));
                }

                badgeList.Add(item: badgeWidget);
            }

            if (isNationalDay && showFlag) {
                if (badgeList.Count >= 1) {
                    badgeList.Add(new SizedBox(width: 4));
                }

                badgeList.Add(Image.asset(
                    "image/china-flag-badge",
                    height: 14,
                    width: 16
                ));
            }

            if (badgeList.Count > 0) {
                return new Container(
                    padding: padding,
                    child: new Row(
                        children: badgeList
                    )
                );
            }

            return new Container();
        }

        static readonly List<string> PatternImages = new List<string> {
            "image/pattern1",
            "image/pattern2",
            "image/pattern3",
            "image/pattern4"
        };
        
        public static string GetSpecificPatternImageNameFromId(string id) {
            return PatternImages[CCommonUtils.GetStableHash(s: id) % PatternImages.Count];
        }

        public const string FavoriteCoverImagePath = "image/favorites";

        public static readonly List<string> FavoriteCoverImages = new List<string> {
            "favor-gamepad",
            "favor-programming",
            "favor-trophy",
            "favor-document",
            "favor-gamer",
            "favor-360_rotate",
            "favor-musical_note",
            "favor-book",
            "favor-smartphone",
            "favor-headphones",
            "favor-keyboard",
            "favor-game_console",
            "favor-hamburger",
            "favor-pokemon_go",
            "favor-security",
            "favor-beer",
            "favor-magazine",
            "favor-vr",
            "favor-french_fries",
            "favor-balloons",
            "favor-smartwatch",
            "favor-analytics",
            "favor-coffee_cup",
            "favor-computer",
            "favor-pencil",
            "favor-gamepad2",
            "favor-smartphone2",
            "favor-blog",
            "favor-muffin",
            "favor-camera",
            "favor-layers",
            "favor-cmyk",
            "favor-hot_air_balloon",
            "favor-video_camera",
            "favor-idea",
            "favor-map"
        };


        public static byte[] readImage(string path) {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int) fs.Length);
            fs.Close();
            fs.Dispose();
            return bytes;
        }

        public static Promise<byte[]> asyncLoadFile(string path) {
            var promise = new Promise<byte[]>();
            Window.instance.startCoroutine(_read(path, promise));
            return promise;
        }

        static IEnumerator _read(string path, Promise<byte[]> promise) {
            byte[] bytes = readImage(path);
            yield return bytes;
            promise.Resolve(bytes);
        }
    }
}