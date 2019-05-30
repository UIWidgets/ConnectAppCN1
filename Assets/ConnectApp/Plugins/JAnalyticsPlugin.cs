using UnityEngine;

namespace ConnectApp.Plugins {
    public static class JAnalyticsPlugin {
        public static void startPageView(string pageName) {
            startLogPageView(pageName);
        }

        public static void stopPageView(string pageName) {
            stopLogPageView(pageName);
        }

        public static void login(string loginType) {
            loginEvent(loginType);
        }


#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void startLogPageView(string pageName);

        [DllImport("__Internal")]
        static extern void stopLogPageView(string pageName);

        [DllImport("__Internal")]
        static extern void loginEvent(string loginType);

#elif UNITY_ANDROID
        static AndroidJavaObject _plugin;

        static AndroidJavaObject Plugin() {
            if (_plugin == null) {
                using (
                    AndroidJavaClass managerClass =
                        new AndroidJavaClass("com.unity3d.unityconnect.plugins.JAnalyticsPlugin")
                ) {
                    _plugin = managerClass.CallStatic<AndroidJavaObject>("getInstance");
                }
            }

            return _plugin;
        }

        static void startLogPageView(string pageName) {
            Plugin().Call("startLogPageView", pageName);
        }

        static void stopLogPageView(string pageName) {
            Plugin().Call("stopLogPageView", pageName);
        }

        static void loginEvent(string loginType) {
            Plugin().Call("loginEvent", loginType);
        }

#else
        static void startLogPageView(string pageName) {}
        static void stopLogPageView(string pageName) {}
        static void loginEvent(string loginType) {}
#endif
    }
}