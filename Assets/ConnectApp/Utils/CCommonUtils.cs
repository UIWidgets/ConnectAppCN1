using System.Collections.Generic;
using System.Text;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Rect = Unity.UIWidgets.ui.Rect;

namespace ConnectApp.Utils {
    public static class CCommonUtils {
        public static float getSafeAreaTopPadding(BuildContext context) {
            return Application.platform == RuntimePlatform.IPhonePlayer 
                ? MediaQuery.of(context: context).padding.top
                : 0;
        }

        public static float getSafeAreaBottomPadding(BuildContext context) {
            return Application.platform == RuntimePlatform.IPhonePlayer 
                ? MediaQuery.of(context: context).padding.bottom
                : 0;
        }

        public static Rect getContextRect(this BuildContext context) {
            var renderBox = (RenderBox) context.findRenderObject();
            return renderBox.localToGlobal(point: Offset.zero) & renderBox.size;
        }

        public static bool isIPhone {
            get { return Application.platform == RuntimePlatform.IPhonePlayer; }
        }

        public static bool isAndroid {
            get { return Application.platform == RuntimePlatform.Android; }
        }

        public static string GetUserLicense(string userId, Dictionary<string, UserLicense> userLicenseMap = null) {
            if (userLicenseMap == null || !userLicenseMap.ContainsKey(key: userId)) {
                return "";
            }

            var userLicense = userLicenseMap[key: userId];
            if (userLicense != null && userId == userLicense.userId && userLicense.license.isNotEmpty()) {
                return userLicense.license;
            }

            return "";
        }
        
        const int MUST_BE_LESS_THAN = 10000; // 4 decimal digits

        public static int GetStableHash(string s) {
            uint hash = 0;
            // if you care this can be done much faster with unsafe 
            // using fixed char* reinterpreted as a byte*
            foreach (var b in Encoding.Unicode.GetBytes(s: s)) {
                hash += b;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            // final avalanche
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            // helpfully we only want positive integer < MUST_BE_LESS_THAN
            // so simple truncate cast is ok if not perfect
            return (int) (hash % MUST_BE_LESS_THAN);
        }
    }
}