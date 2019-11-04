using System.Collections.Generic;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;

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
    }
}