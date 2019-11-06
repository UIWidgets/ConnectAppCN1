using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace ConnectApp.Plugins {
    public static class UrlLauncherPlugin {
        public static void Launch(string urlString, bool forceSafariVC = true, bool forceWebView = false) {
            if (Application.isEditor || CCommonUtils.isAndroid) {
                Application.OpenURL(url: urlString);
                return;
            }

            launch(urlString: urlString, forceSafariVC: forceSafariVC, forceWebView: forceWebView);
        }

        public static bool CanLaunch(string urlString) {
            if (urlString.isEmpty()) {
                return false;
            }

            return Application.isEditor || CCommonUtils.isAndroid || canLaunch(urlString: urlString);
        }

#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void launch(string urlString, bool forceSafariVC = true, bool forceWebView = false);

        [DllImport("__Internal")]
        static extern bool canLaunch(string urlString);
#elif UNITY_ANDROID
        static AndroidJavaClass _plugin;

        static AndroidJavaClass Plugin() {
            if (_plugin == null) {
                _plugin = new AndroidJavaClass("com.unity3d.unityconnect.plugins.CommonPlugin");
            }

            return _plugin;
        }

        static void launch(string urlString, bool forceSafariVC = true, bool forceWebView = false) {
            Plugin().CallStatic("launch", urlString, forceSafariVC, forceWebView);
        }

        static bool canLaunch(string urlString) {
            return Plugin().CallStatic<bool>("canLaunch", urlString);
        }
#else
        static void launch(string urlString, bool forceSafariVC = true, bool forceWebView = false) { }
        static bool canLaunch(string urlString) { return true; }
#endif
    }
}