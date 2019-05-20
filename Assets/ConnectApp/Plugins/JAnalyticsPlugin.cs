using System.Runtime.InteropServices;
using UnityEngine;

namespace ConnectApp.plugins {
    public class JAnalyticsPlugin {

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
        static void startLogPageView(string pageName) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JAnalyticsPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("startLogPageView",pageName);
                }
            }
        }
        static void stopLogPageView(string pageName) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JAnalyticsPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("stopLogPageView",pageName);
                }
            }
        }
        static void loginEvent(string loginType) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JAnalyticsPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("loginEvent",loginType);
                }
            }
        }

#else
        static void startLogPageView(string pageName) {}
        static void stopLogPageView(string pageName) {}
        static void loginEvent(string loginType) {}
#endif  
        
        
    }
}