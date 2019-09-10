using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class CCommonUtils {
        public static float getSafeAreaBottomPadding(BuildContext context) {
            float bottomPadding = 0;
            if (Application.platform == RuntimePlatform.IPhonePlayer) {
                bottomPadding = MediaQuery.of(context).padding.bottom;
            }

            return bottomPadding;
        }
    }
}