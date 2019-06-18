using System.Collections.Generic;
using Newtonsoft.Json;
#if UNITY_IOS
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
#elif UNITY_ANDROID
using UnityEngine;
#endif


namespace ConnectApp.Plugins {
    public static class JAnalyticsPlugin {
        public static void StartPageView(string pageName) {
            startLogPageView(pageName);
        }

        public static void StopPageView(string pageName) {
            stopLogPageView(pageName);
        }

        public static void Login(string loginType) {
            loginEvent(loginType);
        }

        public static void CountEvent(string eventId, Dictionary<string, string> extras) {
            var extra = JsonConvert.SerializeObject(extras);
            countEvent(eventId, extra);
        }

        public static void CalculateEvent(string eventId, string value, Dictionary<string, string> extras) {
            var extra = JsonConvert.SerializeObject(extras);
            calculateEvent(eventId, value, extra);
        }

        public static void BrowseEvent(string eventId, string name, string type, string duration,
            Dictionary<string, string> extras) {
            var extra = JsonConvert.SerializeObject(extras);
            browseEvent(eventId, name, type, duration, extra);
        }


#if UNITY_IOS
        [DllImport("__Internal")]
        static extern void startLogPageView(string pageName);

        [DllImport("__Internal")]
        static extern void stopLogPageView(string pageName);

        [DllImport("__Internal")]
        static extern void loginEvent(string loginType);
        
        [DllImport("__Internal")]
        static extern void countEvent(string eventId,string extra);
        
        [DllImport("__Internal")]
        static extern void calculateEvent(string eventId,string value,string extra);
        
        [DllImport("__Internal")]
        static extern void browseEvent(string eventId,string name, string type,string duration, string extra);

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

        static void countEvent(string eventId, string extra) {
            Plugin().Call("countEvent", eventId, extra);
        }

        static void calculateEvent(string eventId, string value, string extra) {
            Plugin().Call("calculateEvent", eventId, value, extra);
        }

        static void browseEvent(string eventId, string name, string type, string duration, string extra) {
            Plugin().Call("browseEvent", eventId, name, type, duration, extra);
        }

#else
        static void startLogPageView(string pageName) {}
        static void stopLogPageView(string pageName) {}
        static void loginEvent(string loginType) {}
        static void countEvent(string eventId, string extra) {}
        static void calculateEvent(string eventId,string value,string extra) {}
        static void browseEvent(string eventId,string name, string type,string duration, string extra) {}
        
#endif
    }
}