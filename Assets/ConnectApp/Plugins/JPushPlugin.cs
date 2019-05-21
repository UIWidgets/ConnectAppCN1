using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.constants;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using EventType = ConnectApp.models.EventType;

namespace ConnectApp.plugins {
    public static class JPushPlugin {
        public static BuildContext context;
        public static bool isListen;
        static int callbackId = 0;
        
        public static void addListener() {
            if (!isListen) {
                UIWidgetsMessageManager.instance.AddChannelMessageDelegate("jpush", _handleMethodCall);
                completed();
                setJPushChannel(Config.store);
                setJPushTags(new List<string>{Config.versionCode.ToString()});
                isListen = true;
            }
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (context != null) {
                using (WindowProvider.of(context).getScope()) {
                    switch (method) {
                        case "OnOpenNotification": {
                            var node = args[0];
                            var dict = JSON.Parse(node);
                            var type = dict["type"];
                            var id = dict["id"];
                            if (type == "article") {
                                StoreProvider.store.dispatcher.dispatch(
                                    new MainNavigatorPushToArticleDetailAction {articleId = id});
                            }
                            else if (type == "event") {
                                var eventType = EventType.offline;
                                if (dict["eventType"] == "online") {
                                    eventType = EventType.online;
                                }

                                StoreProvider.store.dispatcher.dispatch(
                                    new MainNavigatorPushToEventDetailAction {eventId = id, eventType = eventType});
                            }
                        }
                            break;
                    }
                }
            }
        }

        static void completed() {
            listenCompleted();
        }

        public static void setJPushChannel(string channel) {
            if (channel.isEmpty()) return;
            setChannel(channel);
        }
        
        public static void setJPushAlias(string alias) {
            if (alias.isEmpty()) return;
            setAlias(callbackId++, alias);
        }
        
        public static void deleteJPushAlias() {
            deleteAlias(callbackId++);
        }
        
        public static void setJPushTags(List<string> tags)  {
            string tagsJsonStr = JsonHelper.ToJson(tags);
            if (tagsJsonStr.isEmpty()) return;
            setTags(callbackId++, tagsJsonStr);
        }

#if UNITY_IOS

        [DllImport("__Internal")]
        static extern void listenCompleted();

        [DllImport("__Internal")]
        static extern void setChannel(string channel);
        
        [DllImport("__Internal")]
        static extern void setAlias(int sequence, string alias);

        [DllImport("__Internal")]
        static extern void deleteAlias(int sequence);

        [DllImport("__Internal")]
        static extern void setTags(int sequence, string tagsJsonStr);

#elif UNITY_ANDROID
        
        static void listenCompleted() {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JPushPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("listenCompleted");
                }
            }
        }
        static void setChannel(string channel) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JPushPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("setChannel", channel);
                }
            }
        }
        
        static void setAlias(int sequence, string alias) {
            using ( 
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JPushPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("setAlias", sequence, alias);
                }
            }
        }
        
        static void deleteAlias(int sequence) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JPushPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("deleteAlias", sequence);
                }
            }
        }
        
        static void setTags(int sequence, string tagsJsonStr) {
            using (
                AndroidJavaClass managerClass = new AndroidJavaClass("com.unity3d.unityconnect.plugins.JPushPlugin")
            ) {
                using (
                    AndroidJavaObject managerInstance = managerClass.CallStatic<AndroidJavaObject>("getInstance")
                ) {
                    managerInstance.Call("setTags", sequence, tagsJsonStr);
                }
            }
        }

#else
        public static void listenCompleted() {}
        public static void setChannel(string channel) {}
        public static void setTags(int sequence, string tagsJsonStr) {}
        public static void deleteAlias(int sequence) {}
#endif
    }
}