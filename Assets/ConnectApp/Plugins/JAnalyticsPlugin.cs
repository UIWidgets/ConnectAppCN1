using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif


namespace ConnectApp.Plugins {
    public static class JAnalyticsPlugin {

        public static void Login(string loginType) {
//            loginEvent(loginType: loginType);
        }

        public static void CountEvent(string eventId, Dictionary<string, string> extras) {
//            var extra = JsonConvert.SerializeObject(value: extras);
//            countEvent(eventId: eventId, extra: extra);
        }

        public static void BrowseEvent(string eventId, string name, string type, string duration,
            Dictionary<string, string> extras) {
//            var extra = JsonConvert.SerializeObject(value: extras);
//            browseEvent(eventId: eventId, name: name, type: type, duration: duration, extra: extra);
        }


#if UNITY_IOS

        [DllImport("__Internal")]
        static extern void loginEvent(string loginType);

        [DllImport("__Internal")]
        static extern void countEvent(string eventId, string extra);

        [DllImport("__Internal")]
        static extern void browseEvent(string eventId, string name, string type, string duration, string extra);

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

        static void loginEvent(string loginType) {
            Plugin().Call("loginEvent", loginType);
        }

        static void countEvent(string eventId, string extra) {
            Plugin().Call("countEvent", eventId, extra);
        }

        static void browseEvent(string eventId, string name, string type, string duration, string extra) {
            Plugin().Call("browseEvent", eventId, name, type, duration, extra);
        }

#else
        static void loginEvent(string loginType) {}
        static void countEvent(string eventId, string extra) {}
        static void browseEvent(string eventId,string name, string type,string duration, string extra) {}
        
#endif
    }
}