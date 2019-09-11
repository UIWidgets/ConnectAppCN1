using System.Collections.Generic;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Utils {
    public class CCommonUtils {
        public static float getSafeAreaBottomPadding(BuildContext context) {
            float bottomPadding = 0;
            if (Application.platform == RuntimePlatform.IPhonePlayer) {
                bottomPadding = MediaQuery.of(context).padding.bottom;
            }

            return bottomPadding;
        }

        public static string GetUserLicense(string userId, Dictionary<string, UserLicense> userLicenseMap = null) {
            var license = "";
            if (userLicenseMap == null || !userLicenseMap.ContainsKey(key: userId)) {
                return license;
            }

            var userLicense = userLicenseMap[key: userId];
            if (userLicense != null && userId == userLicense.userId && userLicense.license.isNotEmpty()) {
                license = userLicense.license;
            }

            return license;
        }
    }
}